using System.Collections.Concurrent;

namespace SecuritiesPortal
{
    public interface IBidCache
    {
            void AddOrUpdate(string itemName, int amount);
            IEnumerable<TBillBids> GetAllItems();
    }
}