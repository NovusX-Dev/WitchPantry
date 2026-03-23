using UnityEngine;

namespace WitchPantry.Data.ContentDefinition
{
    [CreateAssetMenu(fileName = "Content Catalogue", menuName = "Witch Pantry/Content Catalogue", order = 0)]
    public class ContentDefinitionCatalogue : ScriptableObject
    {
        [field: SerializeField] public ContentDefinition[] ContentDefinitions {get; private set;}
    }
}