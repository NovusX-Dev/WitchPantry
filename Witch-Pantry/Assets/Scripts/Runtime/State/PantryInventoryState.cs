using System;
using System.Collections.Generic;

namespace WitchPantry.Runtime.State
{
    public class PantryInventoryState
    {
        private readonly Dictionary<string, int> _resources = new();
        public IReadOnlyDictionary<string, int> Resources => _resources;

        public event Action InventoryChanged;
        public event Action<string, int> ResourceChanged;
        public event Action<string, int> OnInventoryUpdated;
        
        /// <summary>
        /// Adds a resource to the inventory.
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="amount"></param>
        public void AddResource(string resourceId, int amount)
        {
            if (string.IsNullOrEmpty(resourceId) || amount <= 0) return;

            if (!_resources.TryAdd(resourceId, amount))
            {
                _resources[resourceId] += amount;
            }

            RaiseResourceChanged(resourceId);
        }
        
        /// <summary>
        /// Gets the amount of a resource in the inventory.
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public int GetResourceAmount(string resourceId)
        {
            return _resources.GetValueOrDefault(resourceId, 0);
        }

        /// <summary>
        /// Checks if the inventory has a resource.
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public bool HasResource(string resourceId)
        {
            if (!_resources.TryGetValue(resourceId, out var resource)) return false;
            
            return resource > 0;
        }

        /// <summary>
        /// Consumes a resource from the inventory.
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool TryConsume(string resourceId, int amount)
        {
            if (string.IsNullOrEmpty(resourceId) || amount <= 0) return false;
            if (!HasResource(resourceId))
            {
                return false;
            }

            if(_resources[resourceId] < amount) return false;
            if (_resources[resourceId] == 0) return false;
            _resources[resourceId] -= amount;
            RaiseResourceChanged(resourceId);
            return true;
        }

        private void RaiseResourceChanged(string resourceId)
        {
            var amount = GetResourceAmount(resourceId);
            InventoryChanged?.Invoke();
            ResourceChanged?.Invoke(resourceId, amount);
            OnInventoryUpdated?.Invoke(resourceId, amount);
        }
    }
}
