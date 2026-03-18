using UnityEngine;

namespace WitchPantry.Data
{
    [CreateAssetMenu(fileName = "Recipe Definition", menuName = "WitchPantry/Content Def/Recipe", order = 0)]
    public class RecipeDefinition : ContentDefinition
    {
        [field: Header("Recipe Properties")]
        [field: SerializeField] public IngredientAmount[] Inputs { get; private set; }
        [field: SerializeField] public PotionAmount PotionAmount { get; private set; }
        [field: SerializeField] public float CraftTime { get; private set; }
    }
}