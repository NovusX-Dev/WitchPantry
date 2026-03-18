using UnityEngine;

namespace WitchPantry.Data
{
    [CreateAssetMenu(fileName = "Ingredient Definition", menuName = "WitchPantry/Content Def/Ingredients", order = 0)]
    public class IngredientDefinition : ContentDefinition
    {
        [field: Header("Ingredient Properties")]
        [field: SerializeField] public float EconomicValue { get; private set; }
        [field: SerializeField] public GlobalConstants.Tiers Tier { get; set; }
        [field: SerializeField] public GlobalConstants.IngredientCategory IngredientCategory { get; set; }
        [field: SerializeField] public GlobalConstants.IngredientStage Stage { get; set; }
        [field: SerializeField] public UnlockSource UnlockSource { get; set; }
    }
}