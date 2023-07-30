

namespace OctoBackend.Application.Models
{
    public class EmailBox
    {
        public List<Email> Emails { get; set; } = null!;
        public string Subject { get; set; } = null!;

        public EmailBox(List<Email> emails, string subject)
        {
            Emails = emails;
            Subject = subject;
        }
    }

    public class Email
    {
        public string EmailAdress { get; set; } = null!;
        public string Body { get; set; } = null!;
    }
}
