using UnityEngine;

namespace WitchPantry.Data.ContentDefinition
{
    public abstract class ContentDefinition : ScriptableObject
    {
        [field: Header("Base Content Info")]
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Id { get; private set; }  
        [field: SerializeField] public GlobalConstants.ContentType ContentType { get; private set; }
        [field: SerializeField] public string DisplayName { get; private set; }
    }
}