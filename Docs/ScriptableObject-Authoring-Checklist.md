# Witch's Pantry ScriptableObject Authoring Checklist

- Purpose: one canonical checklist for authored content assets
- Scope: concrete `ContentDefinition` assets that should replace placeholder data
- Use this doc when creating or reviewing real content under `Witch-Pantry/Assets/Data/`

## Related Docs

Use this file as the checklist, and use these docs for deeper design reasoning:

- content strategy and scope: [Witchs-Pantry-Content-Plan.md](C:/Unity/Repos/WitchPantry/Docs/Witchs-Pantry-Content-Plan.md)
- ingredient, potion, machine, and biome progression: [Ingredients_Biomes_Design.md](C:/Unity/Repos/WitchPantry/Docs/Ingredients_Biomes_Design.md)
- contracts and faction demand: [Customer-Demand-and-Contracts.md](C:/Unity/Repos/WitchPantry/Docs/Customer-Demand-and-Contracts.md)
- data schema and field rules: [Draft-Unity-Architecture.md](C:/Unity/Repos/WitchPantry/Docs/Draft-Unity-Architecture.md)
- production flow model: [Production-Graph-System-Witchs-Pantry-Virtual-Connections.md](C:/Unity/Repos/WitchPantry/Docs/Production-Graph-System-Witchs-Pantry-Virtual-Connections.md)

## Authoring Rules

Apply these rules to every authored asset:

- every asset inherits:
  - `Icon`
  - `Id`
  - `ContentType`
  - `DisplayName`
- `Id` format is `<contentType>.<slug>`
- `DisplayName` should be player-facing, not placeholder text
- `ContentType` must match the concrete asset type
- remove placeholder names like `ingredient.t1`, `potion.healme`, `machine.gathering`, and `contract.luna`

`UnlockRequirement` rules:

- `StartingContent`
  - no unlock reference should be assigned
- `ContentDefinition`
  - assign the `ContentDefinition` reference field
  - use this for machine-driven or recipe-driven unlocks
- `Biome`
  - assign the `BiomeDefinition` reference field
  - current biome placeholder assets still generate `content.<slug>` ids because `ContentType` does not yet include `Biome`
- `ContractReward`
  - assign the `SourceContract` reference field
- `Research`, `Prestige`, and `EventReward`
  - reserved for later
  - do not author content with these types yet

Value fields:

- `IngredientDefinition.EconomicValue`
  - use as the baseline economic worth of an ingredient
  - this is useful for recipe balance, reward tuning, and future ingredient-selling rules
  - if ingredients are never directly sellable, treat it as internal balance data rather than a player-facing price

- `PotionDefinition.SellValue`
  - use as the direct player-facing sale value of a potion
  - this should be the clearest monetization output field in the authored data

Machine fields:

- `PurchaseCost`
  - upfront machine purchase price
- `UpgradeCost`
  - base cost anchor for future upgrade scaling
- `ProcessingSpeed`
  - use `1.0` as the neutral baseline
  - higher than `1.0` means faster than recipe baseline
  - lower than `1.0` means slower than recipe baseline
- `QueueCapacity`
  - how many jobs or queued items the machine can hold
- `EnergyCost`
  - ongoing operating burden or upkeep weight
- `Footprint`
  - pantry tile size

## Canonical Starter Slice

This is the first real authored content set the project should have.

### Ingredients

- [ ] `ingredient.herb`
  - `DisplayName`: `Herb`
  - `EconomicValue`: author a starter value
  - `Tier`: `Common`
  - `IngredientCategory`: `Herb`
  - `Stage`: `Raw`
  - `UnlockRequirement`:
    - `Type = ContentDefinition`
    - `ContentDefinition = machine.herb_garden`
  - Notes: core healing-starter ingredient

- [ ] `ingredient.water`
  - `DisplayName`: `Water`
  - `EconomicValue`: author a starter value
  - `Tier`: `Common`
  - `IngredientCategory`: `Liquid`
  - `Stage`: `Raw`
  - `UnlockRequirement`:
    - `Type = ContentDefinition`
    - `ContentDefinition = machine.well`
  - Notes: staple brewing input

- [ ] `ingredient.mushroom`
  - `DisplayName`: `Mushroom`
  - `EconomicValue`: author a starter value
  - `Tier`: `Common`
  - `IngredientCategory`: `Mushroom`
  - `Stage`: `Raw`
  - `UnlockRequirement`:
    - `Type = ContentDefinition`
    - `ContentDefinition = machine.mushroom_cave`
  - Notes: early branch ingredient

- [ ] `ingredient.ground_herb`
  - `DisplayName`: `Ground Herb`
  - `EconomicValue`: author a starter value
  - `Tier`: `Common`
  - `IngredientCategory`: `Processed`
  - `Stage`: `Processed`
  - `UnlockRequirement`:
    - `Type = ContentDefinition`
    - `ContentDefinition = recipe.ground_herb`
  - Notes: intermediate for Healing Potion

- [ ] `ingredient.mushroom_paste`
  - `DisplayName`: `Mushroom Paste`
  - `EconomicValue`: author a starter value
  - `Tier`: `Common`
  - `IngredientCategory`: `Processed`
  - `Stage`: `Processed`
  - `UnlockRequirement`:
    - `Type = ContentDefinition`
    - `ContentDefinition = recipe.mushroom_paste`
  - Notes: intermediate for Energy Potion

### Potions

- [ ] `potion.healing_potion`
  - `DisplayName`: `Healing Potion`
  - `SellValue`: author a starter value
  - `Tier`: `Common`
  - `PotionCategory`: `Healing`
  - `UnlockRequirement`:
    - `Type = ContentDefinition`
    - `ContentDefinition = recipe.healing_potion`
  - Notes: first staple potion

- [ ] `potion.energy_potion`
  - `DisplayName`: `Energy Potion`
  - `SellValue`: author a starter value
  - `Tier`: `Common`
  - `PotionCategory`: `Utility`
  - `UnlockRequirement`:
    - `Type = ContentDefinition`
    - `ContentDefinition = recipe.energy_potion`
  - Notes: first branch potion

### Recipes

- [ ] `recipe.gather_herb`
  - `DisplayName`: `Gather Herb`
  - `Inputs`: none
  - `Outputs`:
    - `Herb x1`
  - `CraftTime`: author a starter value

- [ ] `recipe.draw_water`
  - `DisplayName`: `Draw Water`
  - `Inputs`: none
  - `Outputs`:
    - `Water x1`
  - `CraftTime`: author a starter value

- [ ] `recipe.grow_mushroom`
  - `DisplayName`: `Grow Mushroom`
  - `Inputs`: none
  - `Outputs`:
    - `Mushroom x1`
  - `CraftTime`: author a starter value

- [ ] `recipe.ground_herb`
  - `DisplayName`: `Ground Herb`
  - `Inputs`:
    - `Herb x1`
  - `Outputs`:
    - `Ground Herb x1`
  - `CraftTime`: author a starter value

- [ ] `recipe.mushroom_paste`
  - `DisplayName`: `Mushroom Paste`
  - `Inputs`:
    - `Mushroom x1`
  - `Outputs`:
    - `Mushroom Paste x1`
  - `CraftTime`: author a starter value

- [ ] `recipe.healing_potion`
  - `DisplayName`: `Healing Potion`
  - `Inputs`:
    - `Ground Herb x1`
    - `Water x1`
  - `Outputs`:
    - `Healing Potion x1`
  - `CraftTime`: author a starter value

- [ ] `recipe.energy_potion`
  - `DisplayName`: `Energy Potion`
  - `Inputs`:
    - `Mushroom Paste x1`
  - `Outputs`:
    - `Energy Potion x1`
  - `CraftTime`: author a starter value

### Machines

- [ ] `machine.herb_garden`
  - `DisplayName`: `Herb Garden`
  - `PurchaseCost`: `10`
  - `UpgradeCost`: `20`
  - `ProcessingSpeed`: `1.0`
  - `QueueCapacity`: `2`
  - `EnergyCost`: `0`
  - `Footprint`: `2 x 2`
  - `MachineCategory`: `Gathering`
  - `SupportedRecipes`:
    - `recipe.gather_herb`
  - `CanRunOffline`: yes
  - Notes: produces `ingredient.herb`

- [ ] `machine.well`
  - `DisplayName`: `Well`
  - `PurchaseCost`: `12`
  - `UpgradeCost`: `24`
  - `ProcessingSpeed`: `1.0`
  - `QueueCapacity`: `2`
  - `EnergyCost`: `0`
  - `Footprint`: `2 x 2`
  - `MachineCategory`: `Gathering`
  - `SupportedRecipes`:
    - `recipe.draw_water`
  - `CanRunOffline`: yes
  - Notes: produces `ingredient.water`

- [ ] `machine.mushroom_cave`
  - `DisplayName`: `Mushroom Cave`
  - `PurchaseCost`: `15`
  - `UpgradeCost`: `30`
  - `ProcessingSpeed`: `0.9`
  - `QueueCapacity`: `2`
  - `EnergyCost`: `0`
  - `Footprint`: `2 x 2`
  - `MachineCategory`: `Gathering`
  - `SupportedRecipes`:
    - `recipe.grow_mushroom`
  - `CanRunOffline`: yes
  - Notes: produces `ingredient.mushroom`

- [ ] `machine.mortar_golem`
  - `DisplayName`: `Mortar Golem`
  - `PurchaseCost`: `30`
  - `UpgradeCost`: `45`
  - `ProcessingSpeed`: `1.0`
  - `QueueCapacity`: `3`
  - `EnergyCost`: `1`
  - `Footprint`: `2 x 2`
  - `MachineCategory`: `Processing`
  - `SupportedRecipes`:
    - `recipe.ground_herb`
    - `recipe.mushroom_paste`
  - `CanRunOffline`: yes

- [ ] `machine.enchanted_cauldron`
  - `DisplayName`: `Enchanted Cauldron`
  - `PurchaseCost`: `50`
  - `UpgradeCost`: `75`
  - `ProcessingSpeed`: `1.0`
  - `QueueCapacity`: `2`
  - `EnergyCost`: `2`
  - `Footprint`: `3 x 2`
  - `MachineCategory`: `Brewing`
  - `SupportedRecipes`:
    - `recipe.healing_potion`
    - `recipe.energy_potion`
  - `CanRunOffline`: yes

- [ ] `machine.bottling_sprite`
  - `DisplayName`: `Bottling Sprite`
  - `PurchaseCost`: `40`
  - `UpgradeCost`: `60`
  - `ProcessingSpeed`: `1.1`
  - `QueueCapacity`: `2`
  - `EnergyCost`: `1`
  - `Footprint`: `2 x 1`
  - `MachineCategory`: `Bottling`
  - `CanRunOffline`: yes
  - Notes: useful for future runtime flow, but current authored recipe schema does not yet model bottled outputs directly

- [ ] `machine.enchanted_shelf`
  - `DisplayName`: `Enchanted Shelf`
  - `PurchaseCost`: `20`
  - `UpgradeCost`: `35`
  - `ProcessingSpeed`: `1.0`
  - `QueueCapacity`: `8`
  - `EnergyCost`: `0`
  - `Footprint`: `2 x 1`
  - `MachineCategory`: `Storage`
  - `CanRunOffline`: yes
  - Notes: utility/support machine for future storage or fulfillment handling

### Contracts

- [ ] `contract.village_healing_order`
  - `DisplayName`: `Village Healing Order`
  - `Tier`: `Common`
  - `Faction`: `VillageResidents`
  - `TargetPotion`: `potion.healing_potion`
  - `AmountRequired`: author a starter value
  - `RewardGold`: author a starter value
  - `DurationHours`: author a starter value
  - `Weight`: high

- [ ] `contract.merchant_energy_order`
  - `DisplayName`: `Merchant Energy Order`
  - `Tier`: `Common`
  - `Faction`: `TravelingMerchants`
  - `TargetPotion`: `potion.energy_potion`
  - `AmountRequired`: author a starter value
  - `RewardGold`: author a starter value
  - `DurationHours`: author a starter value
  - `Weight`: medium

- [ ] `contract.apothecary_healing_request`
  - `DisplayName`: `Apothecary Healing Request`
  - `Tier`: `Common`
  - `Faction`: `ApothecaryGuild`
  - `TargetPotion`: `potion.healing_potion`
  - `AmountRequired`: author a starter value
  - `RewardGold`: author a starter value
  - `DurationHours`: author a starter value
  - `Weight`: medium

## First Follow-On Branch

This is the first authored content branch after the starter slice and supports the Mana Potion chain.

### Ingredients

- [ ] `ingredient.crystal_dust`
  - `DisplayName`: `Crystal Dust`
  - `EconomicValue`: author a follow-on value
  - `Tier`: `Rare`
  - `IngredientCategory`: `Mineral`
  - `Stage`: `Raw`
  - `UnlockRequirement`:
    - `Type = Biome`
    - `BiomeDefinition = Biome.CrystalCavern.asset`
    - current referenced biome id: `content.crystal_cavern`

- [ ] `ingredient.crystal_powder`
  - `DisplayName`: `Crystal Powder`
  - `EconomicValue`: author a follow-on value
  - `Tier`: `Rare`
  - `IngredientCategory`: `Processed`
  - `Stage`: `Processed`
  - `UnlockRequirement`:
    - `Type = ContentDefinition`
    - `ContentDefinition = recipe.crystal_powder`

### Potions

- [ ] `potion.mana_potion`
  - `DisplayName`: `Mana Potion`
  - `SellValue`: author a follow-on value
  - `Tier`: `Rare`
  - `PotionCategory`: `Utility`
  - `UnlockRequirement`:
    - `Type = ContentDefinition`
    - `ContentDefinition = recipe.mana_potion`

### Recipes

- [ ] `recipe.gather_crystal_dust`
  - `DisplayName`: `Gather Crystal Dust`
  - `Inputs`: none
  - `Outputs`:
    - `Crystal Dust x1`
  - `CraftTime`: author a follow-on value

- [ ] `recipe.crystal_powder`
  - `DisplayName`: `Crystal Powder`
  - `Inputs`:
    - `Crystal Dust x1`
  - `Outputs`:
    - `Crystal Powder x1`
  - `CraftTime`: author a follow-on value

- [ ] `recipe.mana_potion`
  - `DisplayName`: `Mana Potion`
  - `Inputs`:
    - `Crystal Powder x1`
    - `Water x1`
  - `Outputs`:
    - `Mana Potion x1`
  - `CraftTime`: author a follow-on value

### Machines

- [ ] `machine.crystal_mine`
  - `DisplayName`: `Crystal Mine`
  - `PurchaseCost`: `80`
  - `UpgradeCost`: `120`
  - `ProcessingSpeed`: `0.8`
  - `QueueCapacity`: `2`
  - `EnergyCost`: `1`
  - `Footprint`: `2 x 2`
  - `MachineCategory`: `Gathering`
  - `SupportedRecipes`:
    - `recipe.gather_crystal_dust`
  - `CanRunOffline`: yes
  - Notes: produces `ingredient.crystal_dust`

- [ ] `machine.crystal_grinder`
  - `DisplayName`: `Crystal Grinder`
  - `PurchaseCost`: `90`
  - `UpgradeCost`: `135`
  - `ProcessingSpeed`: `1.0`
  - `QueueCapacity`: `3`
  - `EnergyCost`: `2`
  - `Footprint`: `2 x 2`
  - `MachineCategory`: `Processing`
  - `SupportedRecipes`:
    - `recipe.crystal_powder`
  - `CanRunOffline`: yes

## Authoring Order

Create content in this order:

1. ingredients
2. potions
3. recipes
4. machines
5. contracts

Then validate:

1. every `Id` is stable and readable
2. every `UnlockRequirement` uses the correct reference field for its type
3. every recipe input points to a real asset
4. every potion output points to a real asset
5. every machine recipe link is valid
6. every contract target potion exists

## Done Condition

This checklist is complete for the starter slice when:

- placeholder content assets are gone or replaced
- the first authored potion chain is readable from raw ingredient to contract target
- every asset passes the content-definition validator
- the authored content names are production-usable rather than temporary scaffolding
