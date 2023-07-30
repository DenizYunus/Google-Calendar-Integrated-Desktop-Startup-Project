namespace OctoBackend.Application.Features.Queries.Event.GetUpcomings
{
    public class GetUpomingEventsResponse
    {
        public string Name { get; set; } = null!;
        public string? MeetingLink { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
    }
}
