using System;

namespace ScalableDiff.Application.Models
{
    public class DiffContent
    {
        public Guid Id { get; init; }

        public string Content { get; init; }

        protected DiffContent() { }

        public static DiffContent Create(Guid id, string content)
        {
            return new DiffContent
            {
                Id = id,
                Content = content
            };
        }
    }
}
