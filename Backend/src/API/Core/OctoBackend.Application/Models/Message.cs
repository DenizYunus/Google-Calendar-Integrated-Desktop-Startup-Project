
namespace OctoBackend.Application.Models
{
    public class Message
    {
        public string Content { get; set; } = null!;
        public MessageCode? Code { get; set; }

        public Message(string content, MessageCode? code)
        {
            Content = content;
            Code = code;
        }

        public Message(string content)
        {
            Content = content;
        }
    }

    public enum MessageCode
    {
        Conflict,
        NotFound,
        Forbidden
    }
}
