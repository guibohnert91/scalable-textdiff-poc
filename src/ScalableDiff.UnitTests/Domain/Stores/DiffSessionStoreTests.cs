using ScalableDiff.Domain.Models;
using ScalableDiff.Domain.Stores;
using ScalableDiff.Infrastructure.Stores;

namespace ScalableDiff.UnitTests.Domain.Stores
{
    public class DiffSessionStoreTests : StoreBaseTests<Diff>
    {
        protected override IStore<Diff> SetupStore()
        {
            return new DiffSessionStore();
        }
    }
}
