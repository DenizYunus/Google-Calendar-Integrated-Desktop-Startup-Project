using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;

 namespace OctoBackend.Infrastructure.EventBus.RabbitMQ
{
    public class PersistentConnection : IDisposable
    {
        private IConnection? connection;
        private readonly IConnectionFactory _connectionFactory;
        private readonly int _retryCount;
        private readonly object lock_object = new();
        private bool _disposed;

        public PersistentConnection(IConnectionFactory connectionFactory, int retryCount = 5)
        {
            _connectionFactory = connectionFactory;
            _retryCount = retryCount;
        }

        public bool IsConnected => connection != null && connection.IsOpen;

        public IModel CreateModel()
        {
            return connection!.CreateModel();
        }
        public void Dispose()
        {
            _disposed = true;
            connection?.Dispose();
            GC.SuppressFinalize(this);
        }

        public bool TryConnect()
        {
            lock(lock_object)
            {
                var policy = Policy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {

                    });

                policy.Execute(() =>
                {
                    connection = _connectionFactory.CreateConnection();
                });

                if (IsConnected)
                {
                    connection!.ConnectionShutdown += Connection_ConnectionShutDown;

                    return true;
                }

                return false;
            }
        }

        private void Connection_ConnectionShutDown(object? sender, ShutdownEventArgs e)
        {
            if (_disposed) return;

            TryConnect();

        }
    }
}
