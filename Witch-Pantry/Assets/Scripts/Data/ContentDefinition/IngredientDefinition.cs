using UnityEngine;

namespace WitchPantry.Data
{
    [CreateAssetMenu(fileName = "Ingredient Definition", menuName = "WitchPantry/Content Def/Ingredients", order = 0)]
    public class IngredientDefinition : ContentDefinition
    {
        [field: Header("Ingredient Properties")]
        [field: SerializeField] public float EconomicValue { get; private set; }
        [field: SerializeField] public GlobalConstants.Tiers Tier { get; private set; }
        [field: SerializeField] public GlobalConstants.IngredientCategory IngredientCategory { get; private set; }
        [field: SerializeField] public GlobalConstants.IngredientStage Stage { get; private set; }
        
        [field: Header("Unlock Requirement")]
        [field: SerializeField] public UnlockRequirement UnlockRequirement { get; private set; }
    }
}