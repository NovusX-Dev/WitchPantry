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

`UnlockSource` rules:

- `StartingContent` keeps `SourceId` empty
- `Biome` uses `biome.<slug>`
- `Machine` uses `machine.<slug>`
- `Recipe` uses `recipe.<slug>`
- `ContractReward` uses `contract.<slug>`
- `Research` uses `research.<slug>`
- `Prestige` uses `prestige.<slug>`
- `EventReward` uses `event.<slug>`

## Canonical Starter Slice

This is the first real authored content set the project should have.

### Ingredients

- [ ] `ingredient.herb`
  - `DisplayName`: `Herb`
  - `Tier`: `Common`
  - `IngredientCategory`: `Herb`
  - `Stage`: `Raw`
  - `UnlockSource`: `Machine / machine.herb_garden`
  - Notes: core healing-starter ingredient

- [ ] `ingredient.water`
  - `DisplayName`: `Water`
  - `Tier`: `Common`
  - `IngredientCategory`: `Liquid`
  - `Stage`: `Raw`
  - `UnlockSource`: `Machine / machine.well`
  - Notes: staple brewing input

- [ ] `ingredient.mushroom`
  - `DisplayName`: `Mushroom`
  - `Tier`: `Common`
  - `IngredientCategory`: `Mushroom`
  - `Stage`: `Raw`
  - `UnlockSource`: `Machine / machine.mushroom_cave`
  - Notes: early branch ingredient

- [ ] `ingredient.ground_herb`
  - `DisplayName`: `Ground Herb`
  - `Tier`: `Common`
  - `IngredientCategory`: `Processed`
  - `Stage`: `Processed`
  - `UnlockSource`: `Recipe / recipe.ground_herb`
  - Notes: intermediate for Healing Potion

- [ ] `ingredient.mushroom_paste`
  - `DisplayName`: `Mushroom Paste`
  - `Tier`: `Common`
  - `IngredientCategory`: `Processed`
  - `Stage`: `Processed`
  - `UnlockSource`: `Recipe / recipe.mushroom_paste`
  - Notes: intermediate for Energy Potion

### Potions

- [ ] `potion.healing_potion`
  - `DisplayName`: `Healing Potion`
  - `Tier`: `Common`
  - `PotionCategory`: `Healing`
  - `UnlockSource`: `Recipe / recipe.healing_potion`
  - Notes: first staple potion

- [ ] `potion.energy_potion`
  - `DisplayName`: `Energy Potion`
  - `Tier`: `Common`
  - `PotionCategory`: `Utility`
  - `UnlockSource`: `Recipe / recipe.energy_potion`
  - Notes: first branch potion

### Recipes

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
  - `MachineCategory`: `Gathering`
  - `SupportedRecipes`: none or runtime producer behavior
  - `CanRunOffline`: yes
  - Notes: produces `ingredient.herb`

- [ ] `machine.well`
  - `DisplayName`: `Well`
  - `MachineCategory`: `Gathering`
  - `SupportedRecipes`: none or runtime producer behavior
  - `CanRunOffline`: yes
  - Notes: produces `ingredient.water`

- [ ] `machine.mushroom_cave`
  - `DisplayName`: `Mushroom Cave`
  - `MachineCategory`: `Gathering`
  - `SupportedRecipes`: none or runtime producer behavior
  - `CanRunOffline`: yes
  - Notes: produces `ingredient.mushroom`

- [ ] `machine.mortar_golem`
  - `DisplayName`: `Mortar Golem`
  - `MachineCategory`: `Processing`
  - `SupportedRecipes`:
    - `recipe.ground_herb`
    - `recipe.mushroom_paste`
  - `CanRunOffline`: yes

- [ ] `machine.enchanted_cauldron`
  - `DisplayName`: `Enchanted Cauldron`
  - `MachineCategory`: `Brewing`
  - `SupportedRecipes`:
    - `recipe.healing_potion`
    - `recipe.energy_potion`
  - `CanRunOffline`: yes

- [ ] `machine.bottling_sprite`
  - `DisplayName`: `Bottling Sprite`
  - `MachineCategory`: `Bottling`
  - `CanRunOffline`: yes
  - Notes: useful for future runtime flow, but current authored recipe schema does not yet model bottled outputs directly

- [ ] `machine.enchanted_shelf`
  - `DisplayName`: `Enchanted Shelf`
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
  - `Tier`: `Rare`
  - `IngredientCategory`: `Mineral`
  - `Stage`: `Raw`
  - `UnlockSource`: `Biome / biome.crystal_cavern`

- [ ] `ingredient.crystal_powder`
  - `DisplayName`: `Crystal Powder`
  - `Tier`: `Rare`
  - `IngredientCategory`: `Processed`
  - `Stage`: `Processed`
  - `UnlockSource`: `Recipe / recipe.crystal_powder`

### Potions

- [ ] `potion.mana_potion`
  - `DisplayName`: `Mana Potion`
  - `Tier`: `Rare`
  - `PotionCategory`: `Utility`
  - `UnlockSource`: `Recipe / recipe.mana_potion`

### Recipes

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
  - `MachineCategory`: `Gathering`
  - `CanRunOffline`: yes
  - Notes: produces `ingredient.crystal_dust`

- [ ] `machine.crystal_grinder`
  - `DisplayName`: `Crystal Grinder`
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
2. every `UnlockSource` is valid
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
