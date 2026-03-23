using System;
using WitchPantry.Data.ContentDefinition;

namespace WitchPantry.Data
{
    [Serializable]
    public struct IngredientAmount
    {
        public IngredientDefinition ingredient;
        public int amount;

        public IngredientAmount(IngredientDefinition ingredient, int amount)
        {
            this.ingredient = ingredient;
            this.amount = amount;
        }
    }

    [Serializable]
    public struct OutputAmount
    {
        public ContentDefinition.ContentDefinition output;
        public int amount;

        public OutputAmount(ContentDefinition.ContentDefinition output, int amount)
        {
            this.output = output;
            this.amount = amount;
        }
    }
    
    [Serializable]
    public struct MachineEntry : IEquatable<MachineEntry>
    {
        public string machineId;
        public int count;
        public int upgradeLevel;
        public bool isOwned;

        public MachineEntry(string machineId, int count, int upgradeLevel, bool isOwned)
        {
            this.machineId = machineId;
            this.count = count;
            this.upgradeLevel = upgradeLevel;
            this.isOwned = isOwned;
        }

        public bool Equals(MachineEntry other)
        {
            return machineId == other.machineId && count == other.count && upgradeLevel == other.upgradeLevel && isOwned == other.isOwned;
        }

        public override bool Equals(object obj)
        {
            return obj is MachineEntry other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(machineId, count, upgradeLevel, isOwned);
        }
    }
}
