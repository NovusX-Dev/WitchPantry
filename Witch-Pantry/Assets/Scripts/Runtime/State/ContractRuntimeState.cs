using System;
using UnityEngine;
using WitchPantry.Data.ContentDefinition;

namespace WitchPantry.Runtime.State
{
    /// <summary>
    /// Currently Accepted/Active Contract. This is the runtime state of a contract, which is used to track
    /// the progress of the contract and its completion status.
    /// </summary>
    public class ContractRuntimeState
    {
        #region Properties
        public string CurrentContractId { get; private set; }
        public string TargetPotionId { get; private set; }
        public int AmountRequired { get; private set; }
        public int CurrentProgress { get; private set; }
        public int RewardGold { get; private set; }
        public float RemainingDuration { get; private set; }
        public bool Active { get; private set; }
        
        public bool CanBeCompleted => Active && CurrentProgress >= AmountRequired && AmountRequired > 0;
        public bool IsExpired => Active && RemainingDuration <= 0;
        public bool IsRunning => Active && !CanBeCompleted && !IsExpired;
        
        #endregion

        #region Actions

        public event Action ContractChanged;
        public event Action ContractCompleted;
        public event Action ContractExpired;

        #endregion

        /// <summary>
        /// Sets the contract to the new contract.
        /// </summary>
        /// <param name="contract"></param>
        public void SetNewContract(ContractDefinition contract)
        {
            if (contract == null) throw new ArgumentNullException(nameof(contract));
            if (contract.TargetPotion == null) throw new ArgumentException("Contract must define a target potion.", nameof(contract));

            Reset();
            CurrentContractId = contract.Id;
            TargetPotionId = contract.TargetPotion.Id;
            AmountRequired = contract.AmountRequired;
            RewardGold = contract.RewardGold;
            RemainingDuration = contract.DurationHours;
            Active = true;
            ContractChanged?.Invoke();
            
            Debug.Log($"[Contract State] STARTED Contract ID: {CurrentContractId}, Target Potion: {TargetPotionId}, " +
                      $"Amount Required: {AmountRequired}, Reward Gold: {RewardGold}, Duration: {RemainingDuration} hours");
        }

        /// <summary>
        /// Completes the contract.
        /// </summary>
        public void CompleteContract()
        {
            if (!Active) return;
            Debug.Log($"[Contract State] COMPLETED Contract ID {CurrentContractId}");
            Reset();
            ContractCompleted?.Invoke();
        }
        
        /// <summary>
        /// Expires the contract.
        /// </summary>
        public void ExpireContract()
        {
            if (!Active) return;
            Debug.Log($"[Contract State] EXPIRED Contract ID {CurrentContractId}");
            Reset();
            ContractExpired?.Invoke();
        }

        /// <summary>
        /// Updates the progress of the contract.
        /// </summary>
        /// <param name="progress"></param>
        public void UpdateContractProgress(int progress)
        {
            if (!Active || CanBeCompleted) return;

            var clampedProgress = Math.Clamp(progress, 0, AmountRequired);
            if (CurrentProgress == clampedProgress) return;
            
            Debug.Log($"[Contract State] Updated progress for Contract ID {CurrentContractId} to {clampedProgress}/{AmountRequired}");
            CurrentProgress = clampedProgress;
            ContractChanged?.Invoke();
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
            Active = false;
        }
        

    }
}
