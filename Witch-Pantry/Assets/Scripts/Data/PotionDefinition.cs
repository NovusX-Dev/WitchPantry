using UnityEngine;

namespace WitchPantry.Data
{
    [CreateAssetMenu(fileName = "Potion Definition", menuName = "WitchPantry/Content Def/Potion", order = 0)]
    public class PotionDefinition : ContentDefinition
    {
        [field: Header("Potion Properties")]
        [field: SerializeField] public float SellValue { get; private set; }
        [field: SerializeField] public GlobalConstants.Tiers Tier { get; private set; }
        [field: SerializeField] public GlobalConstants.PotionCategory PotionCategory { get; private set; }
        [field: SerializeField] public UnlockSource UnlockSource { get; private set; }
    }
}