using ScalableDiff.Domain.ValueObjects;
using System;

namespace ScalableDiff.Domain.Models
{
    public class Diff
    {
        public Guid Id { get; init; }

        public DiffData Left { get; protected set; }

        public DiffData Right { get; protected set; }

        protected Diff() { }

        public static Diff Create(Guid sessionId)
        {
            return new Diff
            {
                Id = sessionId,
                Left = DiffData.Create(string.Empty),
                Right = DiffData.Create(string.Empty)
            };
        }

        public void SetLeftData(DiffData diffData)
        {
            if (diffData == null)
                throw new ArgumentNullException(nameof(diffData));

            Left = diffData;
        }

        public void SetRightData(DiffData diffData)
        {
            if (diffData == null)
                throw new ArgumentNullException(nameof(diffData));

            Right = diffData;
        }
    }
}
