using System;
using UnityEngine;

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
        public ContentDefinition output;
        public int amount;

        public OutputAmount(ContentDefinition output, int amount)
        {
            this.output = output;
            this.amount = amount;
        }
    }
}
