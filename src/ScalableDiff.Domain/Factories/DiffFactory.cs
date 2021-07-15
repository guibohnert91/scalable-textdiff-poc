using ScalableDiff.Domain.Models;
using ScalableDiff.Domain.ValueObjects;
using System;

namespace ScalableDiff.Domain.Factories
{
    public interface IDiffFactory
    {
        Diff Create(Guid id);
    }
    
    public class DiffFactory : IDiffFactory
    {
        /// <summary>
        /// Creates a new diff with empty left and right contents using the provided id.
        /// </summary>
        /// <param name="Id">The diff id.</param>
        /// <returns>A new Diff.</returns>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="Id"/> is <c>empty</c>.
        /// </exception>
        public Diff Create(Guid Id)
        {
            if (Id == Guid.Empty)
                throw new ArgumentException(nameof(Id));

            var diff = new Diff
            {
                Id = Id
            };

            diff.SetLeftData(DiffData.Create(string.Empty));
            diff.SetRightData(DiffData.Create(string.Empty));

            return diff;
        }
    }
}
