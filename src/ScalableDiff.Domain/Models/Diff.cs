using ScalableDiff.Domain.ValueObjects;
using System;

namespace ScalableDiff.Domain.Models
{
    public class Diff
    {
        public Guid Id { get; init; }

        public DiffData Left { get; protected set; }

        public DiffData Right { get; protected set; }

        /// <summary>
        /// Sets the left diff data.
        /// </summary>
        /// <param name="diffData">The left diff data.</param>   
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="diffData"/> is <c>null</c>.
        /// </exception>
        public void SetLeftData(DiffData diffData)
        {
            if (diffData == null)
                throw new ArgumentNullException(nameof(diffData));

            Left = diffData;
        }

        /// <summary>
        /// Sets the right diff data.
        /// </summary>
        /// <param name="diffData">The left diff data.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="diffData"/> is <c>null</c>.
        /// </exception>
        public void SetRightData(DiffData diffData)
        {
            if (diffData == null)
                throw new ArgumentNullException(nameof(diffData));

            Right = diffData;
        }
    }
}
