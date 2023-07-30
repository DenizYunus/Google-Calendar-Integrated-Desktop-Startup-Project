using OctoBackend.Domain.Enums;

namespace OctoBackend.Domain.Constants
{
    public static class TodoCategoryConsts
    {
        public readonly static Dictionary<string, TodoCategory> categories = new()
        {
            {"Personal", TodoCategory.Personal },
            {"Work", TodoCategory.Work },
            {"Education", TodoCategory.Education }
        };
    }
}
