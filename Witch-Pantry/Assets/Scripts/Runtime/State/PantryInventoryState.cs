using System.Collections.Generic;

namespace WitchPantry.Runtime.State
{
    public class PantryInventoryState
    {
        private readonly Dictionary<string, int> _resources = new();

        //TODO: Add actions if necessary
        
        /// <summary>
        /// Adds a resource to the inventory.
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="amount"></param>
        public void AddResource(string resourceId, int amount)
        {
            if(amount <= 0) return; //TODO: possible UI/UX for player

            //Check if a resource already exists
            if (!_resources.TryAdd(resourceId, amount))
            {
                _resources[resourceId] += amount;
            }
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
            if (!HasResource(resourceId))
            {
                return false; //TODO: possible UI/UX
            }
            
            _resources[resourceId] -= amount;
            if(_resources[resourceId] < 0) _resources[resourceId] = 0; 
            
            return true;
        }
        
        /// <summary>
        /// Gets all resources in the inventory.
        /// </summary>
        /// <param name="resources"></param>
        public void GetAllResources(out Dictionary<string, int> resources)
        {
            resources = _resources;
        }
    }
}