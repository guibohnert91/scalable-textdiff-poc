using ScalableDiff.Domain.Models;
using ScalableDiff.Domain.Stores;
using ScalableDiff.Infrastructure.Stores;

namespace ScalableDiff.UnitTests.Domain.Stores
{
    public class DiffStoreTests : StoreBaseTests<Diff>
    {
        protected override IStore<Diff> SetupStore()
        {
            return new DiffStore();
        }
    }
}
