namespace WitchPantry.Data
{
    public static class GlobalConstants
    {
        #region Enums

        public enum ContentType { Ingredient, Machine, Recipe, Potion, Contract }
        public enum Tiers { Common, Rare, Epic, Legendary }
        public enum MachineCategory {Gathering, Processing, Brewing, Storage, Support, Utility, Bottling, Delivery}
        public enum IngredientCategory { Herb, Mineral, Animal, Mushroom, Liquid, Essence, Processed }
        public enum PotionCategory { Healing, Buff, Debuff, Utility, Offensive, Defensive, Mystic, Cleansing, 
            FactionSpecific }
        public enum IngredientStage {Raw, Processed, Refined, Enchanted}
        public enum ContractFaction
        {
            VillageResidents,
            TravelingMerchants,
            ApothecaryGuild,
            ForestCoven,
            ScholarsConsortium,
            MoonMarket,
            RoyalKitchen,
            OdditiesCollector
        }

        public enum SimulationTickPhase
        {
            PreSimulation = 0,
            Production = 10,
            Contracts = 20,
            Economy = 30,
            PostSimulation = 40,
            PresentationSummary = 50
        }
        
        #endregion
        
    }
        
}
