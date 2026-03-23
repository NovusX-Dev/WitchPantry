using System.Collections.Generic;

namespace WitchPantry.Runtime.State
{
    public class GameSessionState
    {
        public PantryInventoryState PantryInventory { get; private set; }
        public MachineOwnershipState MachineOwnership { get; private set; }
        public List<ContractRuntimeState> ActiveContracts { get; private set; }
        
        public int Gold { get; private set; }
        public string ActiveRoomId { get; private set; }

        public void Initialize()
        {
            PantryInventory = new PantryInventoryState();
            MachineOwnership = new MachineOwnershipState();
            ActiveContracts = new List<ContractRuntimeState>();
            Gold = 0; //TODO: Load from save data
            ActiveRoomId = null;
        }

        public void Save()
        {
            //TODO: implement Save
        }

        public void Load()
        {
            //TODO: implement Load
        }
    }
}