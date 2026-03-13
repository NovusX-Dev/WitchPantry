using System;
using UnityEngine;

namespace WitchPantry.Data
{
    [Serializable]
    public struct UnlockSource
    {
        public GlobalConstants.UnlockSourceType type;
        public string sourceId;
        
        public UnlockSource(GlobalConstants.UnlockSourceType type, string source)
        {
            this.type = type;
            sourceId = $"{type}.{source}";
        }
    }

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
    public struct PotionAmount
    {
        public PotionDefinition potion;
        public int amount;

        public PotionAmount(PotionDefinition potion, int amount)
        {
            this.potion = potion;
            this.amount = amount;
        }
    }
}