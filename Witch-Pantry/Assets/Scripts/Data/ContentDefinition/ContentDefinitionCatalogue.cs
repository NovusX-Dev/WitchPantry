using UnityEngine;

namespace WitchPantry.Data.ContentDefinition
{
    [CreateAssetMenu(fileName = "Content Catalogue", menuName = "WitchPantry/Content Catalogue", order = 0)]
    public class ContentDefinitionCatalogue : ScriptableObject
    {
        [field: SerializeField] public ContentDefinition[] ContentDefinitions {get; private set;}
    }
}