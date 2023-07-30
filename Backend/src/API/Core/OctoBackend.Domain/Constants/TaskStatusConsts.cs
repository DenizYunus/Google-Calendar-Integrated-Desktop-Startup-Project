using OctoBackend.Domain.Enums;

namespace OctoBackend.Domain.Constants
{
    public static class TaskStatusConsts
    {
        public readonly static Dictionary<string, TodoStatus> statusTypes = new()
        {
            {"NextUp", TodoStatus.NextUp },
            {"InProgress", TodoStatus.InProgress },
            {"Completed", TodoStatus.Completed },
        };
    }
}
