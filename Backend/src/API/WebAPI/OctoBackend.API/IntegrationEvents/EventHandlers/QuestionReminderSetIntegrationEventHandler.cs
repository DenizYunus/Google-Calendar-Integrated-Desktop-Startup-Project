using OctoBackend.API.IntegrationEvents.Events;
using OctoBackend.Application.Abstractions;
using OctoBackend.Application.Abstractions.Services.Email;
using OctoBackend.Application.Helpers;
using OctoBackend.Application.Models;

namespace OctoBackend.API.IntegrationEvents.EventHandlers
{
    public class QuestionReminderSetIntegrationEventHandler : IIntegrationEventHandler<QuestionReminderSetIntegrationEvent>
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public QuestionReminderSetIntegrationEventHandler(IEmailService emailService, IConfiguration configuration)
        {
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task Handle(QuestionReminderSetIntegrationEvent integrationEvent)
        {
            //TODO: WILL FOUND A BETTER SOLUTION TO HANDLE TIME +3 PROBLEM
            string htmlText = DirectoryHelper.ReadDirectoryContent(_configuration["Email:RemindQuestionsHtml"]!);
            string body = htmlText.Replace("<@name>", integrationEvent.UserName);
            string subject = "Remind Questions";

            List<Email> emails = new()
            {
                new Email {EmailAdress= integrationEvent.EmailAddress, Body = body},
            };

            EmailBox emailBox = new(emails, subject);

            if (integrationEvent.ReminderDate > DateTime.Now.AddHours(+3))
            {
                var delay = integrationEvent.ReminderDate - DateTime.Now.AddHours(+3);

                await Task.Delay(delay);
                await _emailService.SendAsync(emailBox);
                return;

            }
            await _emailService.SendAsync(emailBox);
        }

        //public async Task Handle(QuestionReminderSetIntegrationEvent integrationEvent)
        //{
        //    //TODO: WILL FOUND A BETTER SOLUTION TO HANDLE TIME +3 PROBLEM
        //    string htmlText = DirectoryHelper.ReadDirectoryContent(_configuration["Email:RemindQuestionsHtml"]!);
        //    string body = htmlText.Replace("<@name>", integrationEvent.UserName);
        //    string subject = "Remind Questions";

        //    List<Email> emails = new()
        //    {
        //        new Email {EmailAdress= integrationEvent.EmailAddress, Body = body},
        //    };

        //    EmailBox emailBox = new(emails, subject);

        //    _timer.Elapsed += async (sender, e) =>
        //    {
        //        await _emailService.SendAsync(emailBox);

        //        _timer.Stop();
        //    };

        //    _timer.AutoReset = false;

        //    var delay = integrationEvent.ReminderDate - DateTime.Now.AddHours(+3);
        //    _timer.Interval = delay.TotalMilliseconds;

        //    _timer.Start();


        //}
    }
}
