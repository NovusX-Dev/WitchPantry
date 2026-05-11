using System.Reflection;
using UnityEngine;
using WitchPantry.Data.ContentDefinition;

namespace WitchPantry.Tests.EditMode.Runtime.State
{
    internal static class RuntimeStateTestFactory
    {
        public static ContractDefinition CreateContract(
            string contractId = "contract.test",
            string potionId = "potion.test",
            int amountRequired = 5,
            int rewardGold = 25,
            float durationHours = 12f)
        {
            var potion = ScriptableObject.CreateInstance<PotionDefinition>();
            SetAutoProperty(potion, typeof(ContentDefinition), "Id", potionId);

            var contract = ScriptableObject.CreateInstance<ContractDefinition>();
            SetAutoProperty(contract, typeof(ContentDefinition), "Id", contractId);
            SetAutoProperty(contract, typeof(ContractDefinition), "TargetPotion", potion);
            SetAutoProperty(contract, typeof(ContractDefinition), "AmountRequired", amountRequired);
            SetAutoProperty(contract, typeof(ContractDefinition), "RewardGold", rewardGold);
            SetAutoProperty(contract, typeof(ContractDefinition), "DurationHours", durationHours);
            return contract;
        }

        private static void SetAutoProperty<TValue>(object target, System.Type ownerType, string propertyName, TValue value)
        {
            var field = ownerType.GetField($"<{propertyName}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(target, value);
        }
    }
}
