using WitchPantry.Data.ContentDefinition;

namespace WitchPantry.Runtime.State
{
    /// <summary>
    /// Currently Accepted/Active Contract. This is the runtime state of a contract, which is used to track
    /// the progress of the contract and its completion status.
    /// </summary>
    public class ContractRuntimeState
    {
        public string CurrentContractId;
        public string TargetPotionId;
        public int AmountRequired;
        public int CurrentProgress;
        public int RewardGold;
        public float RemainingDuration;
        
        public bool CanBeCompleted => CurrentProgress >= AmountRequired;
        public bool IsExpired => RemainingDuration <= 0;
        public bool IsRunning => !CanBeCompleted && !IsExpired;

        /// <summary>
        /// Sets the contract to the new contract.
        /// </summary>
        /// <param name="contract"></param>
        public void SetNewContract(ContractDefinition contract)
        {
            CurrentContractId = contract.Id;
            TargetPotionId = contract.TargetPotion.Id;
            AmountRequired = contract.AmountRequired;
            RewardGold = contract.RewardGold;
            RemainingDuration = contract.DurationHours;
        }

        /// <summary>
        /// Completes the contract.
        /// </summary>
        public void CompleteContract()
        {
            Reset();
            //TODO: Action to add gold to player
        }
        
        /// <summary>
        /// Expires the contract.
        /// </summary>
        public void ExpireContract()
        {
            Reset();
            //TODO: Action to notify player of expired contract
        }

        /// <summary>
        /// Updates the progress of the contract.
        /// </summary>
        /// <param name="progress"></param>
        public void UpdateContractProgress(int progress)
        {
            CurrentProgress = progress;
        }
        
        /// <summary>
        /// Resets the contract to its initial state.
        /// </summary>
        private void Reset()
        {
            CurrentContractId = null;
            TargetPotionId = null;
            AmountRequired = 0;
            CurrentProgress = 0;
            RewardGold = 0;
            RemainingDuration = 0;
        }
        

    }
}