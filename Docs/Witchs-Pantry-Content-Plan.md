# Witch's Pantry Content Plan

- Engine: Unity 6000.3
- Platform: Steam
- Genre: Cozy Automation / Incremental / Desktop Idle

For the concrete asset-by-asset authoring list, use [ScriptableObject-Authoring-Checklist.md](C:/Unity/Repos/WitchPantry/Docs/ScriptableObject-Authoring-Checklist.md) as the canonical checklist.

## 1. Content Strategy

Content should reinforce four things:

- pantry fantasy
- readable production growth
- customer demand variety
- long-session comfort

Content is not just "more stuff." Each addition should strengthen either layout depth, watchability, or player goals.

## 2. Progression Structure

The game evolves through these layers:

1. manual pantry setup
2. first visible production line
3. demand-driven optimization
4. room layout mastery
5. prestige-based witch growth

## 3. Core Resources

- Gold: room growth, machines, upgrades, convenience
- Ingredients: raw and processed inputs
- Potions: primary fulfillment output
- Rare Ingredients: demand rewards, higher-tier recipes, event hooks
- Arcane Essence: prestige currency

## 3.1 Authored Content Data Layer

The content plan should map directly to the authored ScriptableObject model:

- `ContentDefinition`
  - shared base for all authored content assets
  - concrete shared fields:
    - `Icon`
    - `Id`
    - `ContentType`
    - `DisplayName`
  - authoring rules:
    - `Id` format is `<contentType>.<slug>`
    - `ContentType` is derived from the concrete asset type and editor-validated
    - `DisplayName` is the player-facing name
  - current caveat:
    - `BiomeDefinition` currently inherits `ContentDefinition`, but there is no `ContentType.Biome` enum value yet
    - current biome placeholder assets therefore use generated `content.<slug>` ids, for example `content.crystal_cavern`

- `IngredientDefinition`
  - covers raw and processed ingredient content
  - concrete fields:
    - `EconomicValue`
    - `Tier`
    - `IngredientCategory`
    - `Stage`
    - `UnlockRequirement`

- `PotionDefinition`
  - covers sellable and fulfillable potion content
  - concrete fields:
    - `SellValue`
    - `Tier`
    - `PotionCategory`
    - `UnlockRequirement`

- `RecipeDefinition`
  - defines ingredient inputs, authored outputs, and craft time
  - concrete fields:
    - `IngredientAmount[] Inputs`
    - `OutputAmount[] Outputs`
    - `CraftTime`

- `MachineDefinition`
  - defines machine costs, footprint, throughput, and supported recipes
  - concrete fields:
    - `PurchaseCost`
    - `UpgradeCost`
    - `ProcessingSpeed`
    - `QueueCapacity`
    - `EnergyCost`
    - `Footprint`
    - `SupportedRecipes`
    - `MachineCategory`
    - `CanRunOffline`

- `ContractDefinition`
  - defines authored demand asks for runtime selection
  - concrete fields:
    - `Tier`
    - `Faction`
    - `TargetPotion`
    - `AmountRequired`
    - `RewardGold`
    - `DurationHours`
    - `Weight`

Supporting value structs used by the content layer:

- `UnlockRequirement`
  - current fields:
    - `Type`
    - `ContentDefinition`
    - `BiomeDefinition`
    - `SourceContract`
  - authoring rule:
    - `StartingContent` uses no reference
    - `ContentDefinition` uses the `ContentDefinition` reference field
    - `Biome` uses the `BiomeDefinition` reference field
    - `ContractReward` uses the `SourceContract` reference field
    - `Research`, `Prestige`, and `EventReward` are reserved for later and should not be used yet
  - current caveat:
    - biome unlocks use object references correctly, but the referenced biome asset ids are still generated with the `content.` prefix until biome content typing is formalized

- `IngredientAmount`
  - `ingredient`
  - `amount`

- `OutputAmount`
  - `output`
  - `amount`

Current enum vocabulary used by content authoring:

- `MachineCategory`
  - `Gathering`
  - `Processing`
  - `Brewing`
  - `Storage`
  - `Support`
  - `Utility`
  - `Bottling`
  - `Delivery`

- `IngredientCategory`
  - `Herb`
  - `Mineral`
  - `Animal`
  - `Mushroom`
  - `Liquid`
  - `Essence`
  - `Processed`

- `PotionCategory`
  - `Healing`
  - `Buff`
  - `Debuff`
  - `Utility`
  - `Offensive`
  - `Defensive`
  - `Mystic`
  - `Cleansing`
  - `FactionSpecific`

- `IngredientStage`
  - `Raw`
  - `Processed`
  - `Refined`
  - `Enchanted`

- `ContractFaction`
  - `VillageResidents`
  - `TravelingMerchants`
  - `ApothecaryGuild`
  - `ForestCoven`
  - `ScholarsConsortium`
  - `MoonMarket`
  - `RoyalKitchen`
  - `OdditiesCollector`

This keeps content planning, balancing, and implementation in one lane instead of splitting
design language from data language.

## 4. Customer Demand Model

Primary demand sources:

- local regulars
- guild orders
- travelers and merchants
- festivals and seasonal boosts
- special visitors

Demand should create changing priorities without requiring a full dynamic market simulation.

## 5. Customer and Faction Hooks

Examples:

- Village Apothecary: stable healing demand
- Night Market Curator: premium unusual brews
- Courier Caravan: timed bulk contracts
- Witch Guild Quartermaster: progression-gated requests
- Festival Organizer: short seasonal spikes

These groups create:

- pacing control
- stronger fantasy
- clearer goals
- better UI storytelling

## 6. Machine Content Goals

Machine content should feel like a pantry ecosystem.

Categories:

- growers and gatherers
- processors
- brewers
- bottlers and shelf helpers
- customer-facing utility pieces

Every machine should earn its place by improving:

- visible flow
- spatial choices
- output variety
- compact mode readability

## 7. Potion Content Goals

Potion content should support:

- recognizable customer asks
- visual variety
- recipe-driven optimization
- reward pacing

Good potion categories:

- staples
- utility brews
- premium curiosities
- event-responsive potions

## 8. Event Content Framework

Events are grouped into:

- boons
- visitors
- soft disruptions
- opt-in gambles

Examples:

- Fairy Blessing: one work zone gains an efficiency burst
- Traveling Buyer: temporary premium on one potion family
- Humid Weather: slows drying but improves some brew yields
- Mischief Imp: disables a bonus until bribed or redirected

## 9. Event Design Rules

- no heavy resource destruction
- no major progress rollback
- always communicate cause and remedy
- the player should feel nudged, not slapped

## 10. Pantry Fantasy Hooks

Recurring content motifs:

- regular customers with preferences
- shelves, corners, and work zones with character
- magical helpers that are functional and charming
- weather, holidays, and local happenings
- tiny domestic rituals that make the pantry feel alive

## 11. Content by Release Stage

### Demo

- strong starter ingredient set
- a few iconic machines
- 2 to 3 customer groups
- first event set
- one compact pantry room

### Early Access Core

- broader machine and recipe matrix
- richer demand cadence
- faction-specific rewards
- more room growth and adjacency options
- stronger compact-mode utility

### 1.0

- expanded biomes and ingredients
- advanced prestige unlocks
- more event chains
- higher-tier customer groups

## 12. Achievements and Long-Term Goals

Achievements should reward:

- layout mastery
- demand streaks
- efficient production
- special event responses

Long-term goals:

- create a highly efficient pantry
- fulfill faction reputations
- unlock signature machines
- complete prestige milestones

## 13. Content Scope Discipline

Avoid adding content that:

- does not change the decision space
- clutters the pantry visually
- creates unreadable demand states
- belongs to a complex market sim better saved for later
