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

        /// <summary>
        /// Creates a new diff with empty left and right contents using the provided id.
        /// </summary>
        /// <param name="Id">The diff id.</param>
        /// <returns>A new Diff.</returns>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="Id"/> is <c>empty</c>.
        /// </exception>
        public static Diff Create(Guid Id)
        {
            if(Id == Guid.Empty)
                throw new ArgumentException(nameof(Id));

            return new Diff
            {
                Id = Id,
                Left = DiffData.Create(string.Empty),
                Right = DiffData.Create(string.Empty)
            };
        }

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
