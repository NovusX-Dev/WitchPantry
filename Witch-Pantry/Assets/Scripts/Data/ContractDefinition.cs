using UnityEngine;

namespace WitchPantry.Data
{
    [CreateAssetMenu(fileName = "Contract Definition", menuName = "WitchPantry/Content Def/Contracts", order = 0)]
    public class ContractDefinition : ContentDefinition
    {
        [field: Header("Contract Properties")]
        [field: SerializeField] public GlobalConstants.Tiers Tier { get; private set; }
        [field: SerializeField] public GlobalConstants.ContractFaction Faction { get; private set; }
        [field: SerializeField] public PotionDefinition TargetPotion { get; private set; }
        [field: SerializeField] public int AmountRequired { get; private set; }
        [field: SerializeField] public int RewardGold { get; private set; }
        [field: SerializeField] public float DurationHours { get; private set; }
        [field: SerializeField] public int Weight { get; private set; }
        
    }
}
