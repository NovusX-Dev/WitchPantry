using System.Collections.Generic;
using WitchPantry.Data;

namespace WitchPantry.Runtime.State
{
    public class MachineOwnershipState
    {
        private readonly Dictionary<string, MachineEntry> _ownedMachines = new();

        /// <summary>
        /// Adds a machine to the owned machines.
        /// </summary>
        /// <param name="machineId"></param>
        /// <param name="upgradeLevel"></param>
        /// <param name="count"></param>
        /// <param name="isOwned"></param>
        public void AddMachine(string machineId, int upgradeLevel, int count, bool isOwned)
        {
            var machineEntry = new MachineEntry(machineId, count, upgradeLevel, isOwned);
            _ownedMachines.TryAdd(machineId, machineEntry);
        }

        /// <summary>
        /// Updates a machine in the owned machines.
        /// </summary>
        /// <param name="machineId"></param>
        /// <param name="upgradeLevel"></param>
        /// <param name="count"></param>
        public void UpdateMachine(string machineId, int upgradeLevel, int count)
        {
            var machineEntry = _ownedMachines[machineId];
            machineEntry.upgradeLevel = upgradeLevel;
            machineEntry.count = count;
            _ownedMachines[machineId] = machineEntry;
        }
        
        /// <summary>
        /// Gets all owned machines.
        /// </summary>
        /// <param name="ownedMachines"></param>
        public void GetAllOwnedMachines(out Dictionary<string, MachineEntry> ownedMachines)
        {
            ownedMachines = _ownedMachines;
        }

    }
}