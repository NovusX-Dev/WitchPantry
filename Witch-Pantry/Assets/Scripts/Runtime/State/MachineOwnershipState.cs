using System;
using System.Collections.Generic;
using WitchPantry.Data;

namespace WitchPantry.Runtime.State
{
    public class MachineOwnershipState
    {
        private readonly Dictionary<string, MachineEntry> _ownedMachines = new();
        public IReadOnlyDictionary<string, MachineEntry> OwnedMachines => _ownedMachines;
        
        public event Action MachinesChanged;
        public event Action<string> MachineChanged;

        /// <summary>
        /// Adds a machine to the owned machines.
        /// </summary>
        /// <param name="machineId"></param>
        /// <param name="upgradeLevel"></param>
        /// <param name="count"></param>
        /// <param name="isOwned"></param>
        public bool AddMachine(string machineId, int upgradeLevel, int count, bool isOwned)
        {
            if (string.IsNullOrEmpty(machineId)) return false;
            if (_ownedMachines.ContainsKey(machineId)) return false;
            var machineEntry = new MachineEntry(machineId, count, upgradeLevel, isOwned);
            _ownedMachines.TryAdd(machineId, machineEntry);
            RaiseMachineChanged(machineId);
            return true;
        }

        /// <summary>
        /// Updates a machine in the owned machines.
        /// </summary>
        /// <param name="machineId"></param>
        /// <param name="upgradeLevel"></param>
        /// <param name="count"></param>
        public bool UpdateMachine(string machineId, int upgradeLevel, int count)
        {
            if (!_ownedMachines.TryGetValue(machineId, out var machineEntry)) return false;
            var updatedEntry = new MachineEntry(machineId, count, upgradeLevel, machineEntry.isOwned);
            if (machineEntry.Equals(updatedEntry)) return true;
            
            machineEntry.upgradeLevel = upgradeLevel;
            machineEntry.count = count;
            _ownedMachines[machineId] = machineEntry;
            RaiseMachineChanged(machineId);
            return true;
        }

        private void RaiseMachineChanged(string machineId)
        {
            MachinesChanged?.Invoke();
            MachineChanged?.Invoke(machineId);
        }
    }
}
