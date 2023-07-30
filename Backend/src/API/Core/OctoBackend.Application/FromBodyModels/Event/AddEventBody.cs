namespace OctoBackend.Application.FromBodyModels.Event
{
    public class AddEventBody
    {
        public string Name { get; set; } = null!;
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
    }
}
