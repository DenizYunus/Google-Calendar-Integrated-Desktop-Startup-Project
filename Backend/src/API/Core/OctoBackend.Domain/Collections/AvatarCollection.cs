

using OctoBackend.Domain.Collections.BaseCollection;

namespace OctoBackend.Domain.Collections
{
    public class AvatarCollection : BaseMongoCollection
    {
        public string FileName { get; set; } = null!;
        public string Path { get; set; } = null!;
    }
}
