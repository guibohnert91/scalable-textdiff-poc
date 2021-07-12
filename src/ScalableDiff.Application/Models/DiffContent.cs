using System;

namespace ScalableDiff.Application.Models
{
    public class DiffContent
    {
        public Guid SessionId { get; init; }

        public string Content { get; init; }

        protected DiffContent() { }

        public static DiffContent Create(Guid sessionId, string content)
        {
            return new DiffContent
            {
                SessionId = sessionId,
                Content = content
            };
        }
    }
}
