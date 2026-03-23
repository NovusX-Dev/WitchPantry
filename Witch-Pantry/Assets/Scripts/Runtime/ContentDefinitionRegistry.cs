using System;
using System.Collections.Generic;
using WitchPantry.Data.ContentDefinition;

namespace WitchPantry.Runtime
{
    public class ContentDefinitionRegistry
    {
        private readonly Dictionary<string, ContentDefinition> _definitions = new();
        private readonly Dictionary<string, BiomeDefinition> _biomeDefinitions = new();
        private readonly Dictionary<string, ContractDefinition> _contractDefinitions = new();
        private readonly Dictionary<string, MachineDefinition> _machineDefinitions = new();
        private readonly Dictionary<string, RecipeDefinition> _recipeDefinitions = new();
        private readonly Dictionary<string, PotionDefinition> _potionDefinitions = new();
        private readonly Dictionary<string, IngredientDefinition> _ingredientDefinitions = new();

        public void Initialize(ContentDefinitionCatalogue catalogue)
        {
            foreach (var content in catalogue.ContentDefinitions)
            {
                _definitions[content.Id] = content;
                switch (content)
                {
                    case BiomeDefinition biomeDefinition:
                        _biomeDefinitions[biomeDefinition.Id] = biomeDefinition;
                        break;
                    case ContractDefinition contractDefinition:
                        _contractDefinitions[contractDefinition.Id] = contractDefinition;
                        break;
                    case MachineDefinition machineDefinition:
                        _machineDefinitions[machineDefinition.Id] = machineDefinition;
                        break;
                    case RecipeDefinition recipeDefinition:
                        _recipeDefinitions[recipeDefinition.Id] = recipeDefinition;
                        break;
                    case PotionDefinition potionDefinition:
                        _potionDefinitions[potionDefinition.Id] = potionDefinition;
                        break;
                    case IngredientDefinition ingredientDefinition:
                        _ingredientDefinitions[ingredientDefinition.Id] = ingredientDefinition;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        public BiomeDefinition GetBiomeDefinitionById(string id) => _biomeDefinitions[id];
        public ContractDefinition GetContractDefinitionById(string id) => _contractDefinitions[id];
        public MachineDefinition GetMachineDefinitionById(string id) => _machineDefinitions[id];
        public RecipeDefinition GetRecipeDefinitionById(string id) => _recipeDefinitions[id];
        public PotionDefinition GetPotionDefinitionById(string id) => _potionDefinitions[id];
        public IngredientDefinition GetIngredientDefinitionById(string id) => _ingredientDefinitions[id];
        
    }
}