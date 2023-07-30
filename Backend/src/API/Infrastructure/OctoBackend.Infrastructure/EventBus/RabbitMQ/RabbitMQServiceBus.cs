using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OctoBackend.Domain.EventBus;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Text;

namespace OctoBackend.Infrastructure.EventBus.RabbitMQ
{
    public class RabbitMQServiceBus : BaseEventBus
    {
        readonly PersistentConnection _persistentConnection;
        private readonly IConnectionFactory _connectionFactory;
        private  IModel _channel;

        public RabbitMQServiceBus(EventBusConfig config, IServiceProvider serviceProvider, IConfiguration configuration) : base(config, serviceProvider)
        {
            string connectionURI = configuration["RabbitMQ:ConnectionURI"]!;
            if (config.Connection != null)
            {
                var connJson = JsonConvert.SerializeObject(config.Connection, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                });

                _connectionFactory = JsonConvert.DeserializeObject<ConnectionFactory>(connJson)!;
                _connectionFactory.Uri = new(connectionURI);
            }
            else
            {
                _connectionFactory = new ConnectionFactory();
                _connectionFactory.Uri = new(connectionURI);
            }

            _persistentConnection = new(_connectionFactory, config.ConnectionRetryCount);

            _channel = CreateChannel();
        }        
        
        public override void Publish(IntegrationEvent integrationEvent)
        {
            if(!_persistentConnection.IsConnected) 
                _persistentConnection.TryConnect();

            var policy = Policy.Handle<SocketException>()
                   .Or<BrokerUnreachableException>()
                   .WaitAndRetry(_config.ConnectionRetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                   {
                       //log
                   });

            var eventName = integrationEvent.GetType().Name;
            eventName = ProcessEventName(eventName);          

            _channel.ExchangeDeclare(exchange: _config.DefaultTopicName,
                type: "direct");

            var message = JsonConvert.SerializeObject(integrationEvent);
            var body = Encoding.UTF8.GetBytes(message);

            policy.Execute(() =>
            {
                var properties = _channel.CreateBasicProperties();
                properties.DeliveryMode = 2;

                _channel.QueueDeclare(queue: GetSubName(eventName),
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                _channel.BasicPublish(exchange: _config.DefaultTopicName,
                    routingKey: eventName,
                    mandatory: true,
                    basicProperties: properties,
                    body: body);
            });
        }

        public override void Subscribe<T, TH>()
        {
            var eventName = typeof(T).Name;
            eventName = ProcessEventName(eventName);

            if(!_subManager.HasSubscriptionForEvent(eventName))
            {
                if(!_persistentConnection.IsConnected)
                    _persistentConnection.TryConnect();

                if (!_channel.IsOpen)
                    _channel = CreateChannel();

                _channel.QueueDeclare(queue: GetSubName(eventName),
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                _channel.QueueBind(queue: GetSubName(eventName),
                    exchange: _config.DefaultTopicName,
                    routingKey: eventName);
            }

            _subManager.AddSubscription<T, TH>();
            StartBasicConsume(eventName);
        }

        public override void UnSubscribe<T, TH>()
        {
            _subManager.RemoveSubscription<T, TH>();

            _subManager.OnEventRemoved += SubsManager_OneventRemoved;
        }

        private void SubsManager_OneventRemoved(object? sender, string eventName)
        {
            eventName= ProcessEventName(eventName);

            if(!_persistentConnection.IsConnected)
                _persistentConnection.TryConnect();

            if (!_channel.IsOpen || _channel is null)
                _channel = CreateChannel();

            _channel.QueueUnbind(queue: eventName,
                exchange:_config.DefaultTopicName,
                routingKey: eventName);

            if (_subManager.IsEmpty)
                _channel.Close();
        }

        private IModel CreateChannel()
        {
            if(!_persistentConnection.IsConnected)
                _persistentConnection.TryConnect();

            var channel = _persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: _config.DefaultTopicName, type: "direct");

            return channel;          
        }

        private void Channel_ModelShutdown(object? sender, ShutdownEventArgs e)
        {
            _channel = CreateChannel();
        }

        private void StartBasicConsume(string eventName)
        {
            if(_channel != null && _channel.IsOpen)
            {
                var consumer = new EventingBasicConsumer(_channel);

                var properties = _channel.CreateBasicProperties();

                consumer.Received += Consumer_Recieved;

                _channel.BasicConsume(
                        queue: GetSubName(eventName),
                        autoAck: true,
                        consumer: consumer
                        );
            }
        }


        private async void Consumer_Recieved(object? sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;
            eventName = ProcessEventName(eventName);

            var message = Encoding.UTF8.GetString(e.Body.Span);

            try
            {
                await ProcessEvent(eventName, message);
            }
            catch (Exception)
            {
                throw;
            }

            _channel.BasicAck(e.DeliveryTag, true);
        }
    }
}
