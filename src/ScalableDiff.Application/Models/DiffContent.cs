using System;

namespace ScalableDiff.Application.Models
{
    public class DiffContent
    {
        public Guid Id { get; init; }

        public string Content { get; init; }

        protected DiffContent() { }

        /// <summary>
        /// Creates a new diff content for the supplied id.
        /// </summary>
        /// <param name="id">The diff id.</param>
        /// <param name="content">The diff content.</param>
        /// <returns>A new diff content</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="content"/> is <c>empty</c>.
        /// </exception>
        public static DiffContent Create(Guid id, string content)
        {
            if(id == Guid.Empty)
                throw new ArgumentException(nameof(id), "Id can't be empty.");

            return new DiffContent
            {
                Id = id,
                Content = content
            };
        }
    }
}
