using WitchPantry.Data.ContentDefinition;

namespace WitchPantry.Data
{
    [System.Serializable]
    public struct UnlockRequirement
    {
        public UnlockRequirementType Type;
        public ContentDefinition.ContentDefinition ContentDefinition;
        public BiomeDefinition BiomeDefinition;
        public ContractDefinition SourceContract; //??Why is this separate from the above content def?
        //TODO: Define the rest later
        // public PrestigeDefinition SourcePrestige;
        // public EventDefinition SourceEvent;
        // public int RequiredLevel;
    }
    
    public enum UnlockRequirementType
    {
        StartingContent,
        ContentDefinition,
        Biome,
        Research,
        Prestige,
        ContractReward,
        EventReward
    }

}