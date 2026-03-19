using UnityEngine;

namespace WitchPantry.Data.ContentDefinition
{
    [CreateAssetMenu(fileName = "Machine Definition", menuName = "WitchPantry/Content Def/Machines", order = 0)]
    public class MachineDefinition : ContentDefinition
    {
        [field: Header("Machine Properties")]
        [field: SerializeField] public float PurchaseCost { get; private set; }
        [field: SerializeField] public float UpgradeCost { get; private set; }
        [field: SerializeField] public float ProcessingSpeed { get; private set; }
        [field: SerializeField] public int QueueCapacity { get; private set; }
        [field: SerializeField] public float EnergyCost { get; private set; }
        [field: SerializeField] public Vector2Int Footprint  { get; private set; }
        [field: SerializeField] public RecipeDefinition[] SupportedRecipes { get; private set; }
        [field: SerializeField] public GlobalConstants.MachineCategory MachineCategory { get; private set; }
        [field: SerializeField] public bool CanRunOffline { get; private set; }
    }
}