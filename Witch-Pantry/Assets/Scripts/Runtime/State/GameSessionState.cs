using System;
using System.Collections.Generic;

namespace WitchPantry.Runtime.State
{
    public class GameSessionState
    {
        public const int MaxActiveContracts = 3;

        public PantryInventoryState PantryInventory { get; private set; } = new();
        public MachineOwnershipState MachineOwnership { get; private set; } = new();

        private readonly List<ContractRuntimeState> _activeContracts = new();
        public IReadOnlyList<ContractRuntimeState> ActiveContracts => _activeContracts;

        public int Gold { get; private set; }
        public string ActiveRoomId { get; private set; }

        public event Action<int> GoldChanged;
        public event Action<string> ActiveRoomChanged;
        public event Action ContractsChanged;

        public void SetGold(int amount)
        {
            var clampedAmount = Math.Max(0, amount);
            if (Gold == clampedAmount) return;

            Gold = clampedAmount;
            GoldChanged?.Invoke(Gold);
        }

        public void AddGold(int amount)
        {
            if (amount <= 0) return;

            SetGold(Gold + amount);
        }

        public bool TrySpendGold(int amount)
        {
            if (amount <= 0 || Gold < amount) return false;

            SetGold(Gold - amount);
            return true;
        }

        public void SetActiveRoom(string roomId)
        {
            if (ActiveRoomId == roomId) return;

            ActiveRoomId = roomId;
            ActiveRoomChanged?.Invoke(ActiveRoomId);
        }

        public bool TryAddContract(ContractRuntimeState contract)
        {
            if (contract == null || !contract.Active) return false;
            if (_activeContracts.Count >= MaxActiveContracts) return false;
            if (_activeContracts.Contains(contract)) return false;

            _activeContracts.Add(contract);
            contract.ContractChanged += RaiseContractsChanged;
            ContractsChanged?.Invoke();
            return true;
        }

        public bool RemoveContract(ContractRuntimeState contract)
        {
            if (contract == null) return false;
            if (!_activeContracts.Remove(contract)) return false;

            contract.ContractChanged -= RaiseContractsChanged;
            ContractsChanged?.Invoke();
            return true;
        }

        public void Save()
        {
            //TODO: implement Save
        }

        public void Load()
        {
            //TODO: implement Load
        }

        private void RaiseContractsChanged()
        {
            ContractsChanged?.Invoke();
        }
    }
}
