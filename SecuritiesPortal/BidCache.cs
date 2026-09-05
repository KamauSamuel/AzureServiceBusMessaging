using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuritiesPortal
{
    public class BidCache : IBidCache
    {
        private readonly ConcurrentDictionary<string, TBillBids> _bids  = new();
        public void AddOrUpdate(string itemName, int amount)
        {
            // ConcurrentDictionary has a built-in single lookup optimized atomic update method
            _bids.AddOrUpdate(
                itemName,
                key => new TBillBids { IssueNumber = key, TotalAmount = amount },               // If it doesn't exist
                (key, existingItem) => { existingItem.TotalAmount += amount; return existingItem; } // If it exists
            );
        }

    public IEnumerable<TBillBids> GetAllItems() => _bids.Values;
    }
}