# Witch's Pantry Automation Architecture

- Engine: Unity 6000.3
- Architecture Style: Data-Driven Modular Systems
- Language: C#

## 1. Architecture Philosophy

The architecture is designed around four principles:

1. Data-driven design
2. Modular systems
3. Event-driven communication
4. Minimal scene logic

The goal is to make the game:

- Easy to extend
- Easy to balance
- Easy to debug
- Easy to add content

## 2. High-Level Architecture

Game layers:

```text
UI Layer
  ->
Gameplay Systems
  ->
Data Layer
  ->
Save Layer
```

## 3. Folder Structure

```text
Assets/
  Scripts/
    Core/
      GameLoop/
      Events/
    Systems/
      Production/
      Economy/
      Prestige/
      Contracts/
    Data/
      Ingredients/
      Recipes/
      Machines/
    UI/
      Panels/
      Views/
    Save/
      Serialization/
      OfflineSimulation/
  Prefabs/
    Machines/
    Ingredients/
    UI/
  ScriptableObjects/
    Ingredients/
    Recipes/
    Machines/
```

## 4. Core Game Loop

`GameLoopManager` responsibilities:

- Tick simulation
- Update systems
- Manage idle progression

Example tick rate: `1 second`

Flow:

```text
Update()
  ->
TickSimulation()
  ->
ProductionSystem.Update()
  ->
EconomySystem.Update()
  ->
UI refresh
```

## 5. Data Layer (ScriptableObjects)

All content definitions are concrete ScriptableObjects that inherit from a shared
`ContentDefinition` base type. This is the canonical content layer for the project.

The current direction is correct, but the schema must be stronger than the first-pass
prototype fields. In particular:

- machines should not reference other machines as input and output slots
- recipes need quantities, not just arrays of ingredient references
- contracts need room to grow beyond a single potion, amount, and gold reward
- display naming must have one clear source of truth

### Canonical Base Type

- `ContentDefinition`
  - `Id`
  - `ContentType`
  - `DisplayName`
  - `Icon`

Rules for the base type:

- `Id` is a stable authored identifier used by saves and runtime systems
- `Id` format is `<contentType>.<slug>`
- `ContentType` must match the concrete asset type and should be editor-validated
- `DisplayName` is the player-facing name
- `DisplayName` should not be treated as a duplicate of the asset file name
- runtime systems reference definitions by `Id`, not by scene object references

Recommended supporting value types:

- `ContentAmount`
  - `ContentDefinition definition`
  - `int amount`

- `IngredientAmount`
  - `IngredientDefinition ingredient`
  - `int amount`

- `PotionAmount`
  - `PotionDefinition potion`
  - `int amount`

The purpose of these small serializable structs is to avoid the weak "array of references
with implied quantity = 1" pattern while keeping recipe outputs explicit and serializable.

### Concrete Content Assets

- `IngredientDefinition`
  - inherits `ContentDefinition`
  - `BaseValue`
  - recommended additions:
    - `Tier`
    - `IngredientCategory`
    - `UnlockSource`
    - `Stage`

- `PotionDefinition`
  - inherits `ContentDefinition`
  - `SellValue`
  - recommended additions:
    - `Tier`
    - `PotionCategory`
    - `UnlockSource`

- `RecipeDefinition`
  - inherits `ContentDefinition`
  - current concrete structure:
    - `IngredientAmount[] Inputs`
    - `PotionAmount PotionAmount`
    - `float CraftTime`
  - note:
    - output quantity is carried inside `PotionAmount`
    - machine compatibility should remain owned by `MachineDefinition`, not duplicated here

- `MachineDefinition`
  - inherits `ContentDefinition`
  - current concrete structure:
    - `float PurchaseCost`
    - `float UpgradeCost`
    - `float ProcessingSpeed`
    - `int QueueCapacity`
    - `float EnergyCost`
    - `Vector2Int Footprint`
    - `RecipeDefinition[] SupportedRecipes`
    - `MachineCategory MachineCategory`
    - `bool CanRunOffline`
  - field notes:
    - `PurchaseCost` is the base cost to acquire the machine
    - `UpgradeCost` is the base upgrade cost anchor, not the fully computed runtime price
    - final runtime upgrade price can still be derived from upgrade level, scaling curves, perks, and events

- `ContractDefinition`
  - inherits `ContentDefinition`
  - current concrete structure:
    - `Tiers Tier`
    - `ContractFaction Faction`
    - `PotionDefinition TargetPotion`
    - `int AmountRequired`
    - `int RewardGold`
    - `float DurationHours`
    - `int Weight`
  - field notes:
    - `Weight` is selection weight for contract generation
    - it controls how often this contract appears relative to other valid contracts
    - it is not reward value or difficulty by itself

### Supporting Enums And Metadata

- `IngredientCategory`
  - classification for ingredient families
  - examples:
    - `Herb`
    - `Mineral`
    - `Animal`
    - `Mushroom`
    - `Liquid`
    - `Essence`
    - `Processed`

- `PotionCategory`
  - classification for potion role or fantasy grouping
  - examples:
    - `Healing`
    - `Buff`
    - `Debuff`
    - `Utility`
    - `Offensive`
    - `Defensive`
    - `Cleansing`
    - `Mystic`
    - `FactionSpecific`

- `MachineCategory`
  - broad gameplay role in the production chain
  - this should describe function, not visual theme
  - examples:
    - `Gathering`
    - `Processing`
    - `Brewing`
    - `Storage`
    - `Support`
    - `Utility`
    - `Bottling`
    - `Delivery`

- `ContractFaction`
  - source group for a contract and a driver for flavor, demand weighting, and rewards
  - examples:
    - `VillageResidents`
    - `TravelingMerchants`
    - `ApothecaryGuild`
    - `ForestCoven`
    - `ScholarsConsortium`
    - `MoonMarket`
    - `RoyalKitchen`
    - `OdditiesCollector`

- `IngredientStage`
  - progression state of an ingredient in the production chain
  - preferred over a simple boolean like `IsRawIngredient`
  - examples:
    - `Raw`
    - `Processed`
    - `Refined`
    - `Enchanted`

- `UnlockSourceType`
  - high-level classification for how content becomes available
  - examples:
    - `StartingContent`
    - `Biome`
    - `Machine`
    - `Recipe`
    - `ContractReward`
    - `Research`
    - `Prestige`
    - `EventReward`

- `UnlockSource`
  - a small serializable metadata struct
  - current fields:
    - `UnlockSourceType type`
    - `string sourceId`

`UnlockSourceType` should be an enum, but the enum alone is too vague for real progression
tracking. Pairing it with `sourceId` keeps the data model simple while still telling the
game which biome, machine, research node, contract line, or event actually unlocked the content.

### Concrete Implementation Target

For this project, the concrete ScriptableObject set should be:

- `ContentDefinition`
- `IngredientDefinition`
- `PotionDefinition`
- `RecipeDefinition`
- `MachineDefinition`
- `ContractDefinition`

New ingredients, machines, potions, recipes, and contracts are added by creating new
assets from these concrete types rather than inventing ad hoc data containers.

### Design Guidance

- keep definition assets as static authored data only
- keep progression, ownership, inventory counts, and active contract progress in runtime state
- use typed references between definitions wherever possible
- avoid stringly-typed fields when a concrete definition reference is more appropriate
- avoid fields that encode two different concepts under one name

If a field cannot be explained in one sentence to a designer without handwaving, the data
model is probably still too muddy.

## 6. Runtime Game State

Runtime data must not live in ScriptableObjects.

Use plain C# classes that reference content by `ContentDefinition.Id`.

```csharp
class PlayerState
{
    Dictionary<string, double> ingredientInventory;
    Dictionary<string, double> potionInventory;
    Dictionary<string, int> ownedMachines;
    HashSet<string> unlockedRecipeIds;
    HashSet<string> unlockedPotionIds;
    List<string> activeContractIds;
    double gold;
}
```

## 7. Production System

Responsible for potion creation.

Production pipeline:

```text
Ingredient Input
  ->
Processing Machine
  ->
Potion Output
```

Concrete runtime structure:

```csharp
class ProductionSystem
{
    List<MachineInstance> machines;

    void Update()
    {
    }
}
```

`MachineInstance` runtime fields:

- `machineDefinitionId`
- `assignedRecipeId`
- `progress`
- `inputBuffer`
- `outputBuffer`

`RecipeDefinition.Inputs` and `RecipeDefinition.PotionAmount` define what the machine consumes
and produces. `PotionDefinition` provides the sellable output metadata used by the economy.

## 8. Economy System

Handles:

- Gold generation
- Contracts
- Potion values

Concrete rule:

- `PotionDefinition.SellValue` is the source of sell price data
- `ContractDefinition.RewardGold` is the source of contract reward data

```csharp
class EconomySystem
{
    double gold;

    void SellPotion(string potionId, int amount)
    {
    }
}
```

## 9. Contract System

Contracts are runtime instances created from `ContractDefinition` assets.

Each active contract references a concrete definition and tracks run-state progress.

Concrete runtime structure:

```csharp
class Contract
{
    string contractDefinitionId;
    string targetPotionId;
    int amountRequired;
    int currentProgress;
    int rewardGold;
}
```

## 10. Prestige System

Prestige resets progress but adds multipliers.

Example:

```csharp
class PrestigeSystem
{
    int witchLevel;
    double productionMultiplier;
}
```

## 11. Event System

Use events for loose coupling.

Examples:

- `PotionBrewedEvent`
- `MachineBuiltEvent`
- `ContractCompletedEvent`

Example publish call:

```csharp
EventBus.Publish(new PotionBrewedEvent());
```

## 12. Save System

Use JSON serialization.

Save structure:

```csharp
class SaveData
{
    PlayerState playerState;
    ProductionState productionState;
    EconomyState economyState;
    List<ContractState> activeContracts;
    PrestigeState prestigeState;
}
```

Save data stores definition ids and runtime values only. It does not serialize direct
references to `ContentDefinition` assets.

Save triggers:

- Every 30 seconds
- On exit
- On major upgrade

## 13. Offline Progression

When loading the game:

```csharp
offlineTime = now - lastSaveTime;
```

Run simulation:

```csharp
production * offlineTime;
```

Then add resources.

## 14. UI Architecture

Use an MVVM-like structure.

- View: Unity UI objects
- ViewModel: binds UI to systems

Example:

- `PotionProductionView`

## 15. Machine System

Machines are prefab entities.

Machine prefab components:

- `MachineController`
- `MachineAnimator`
- `MachineView`

`MachineController` communicates with `ProductionSystem`.

## 16. Performance Strategy

Use tick simulation.

Example:

- `1 tick per second`

Do not simulate every frame. This prevents CPU waste.

## 17. Balancing System

Costs follow an exponential curve:

```csharp
cost = baseCost * pow(1.15, level);
```

Production:

```csharp
production = baseProduction * multiplier;
```

## 18. Debug Tools

Create developer UI for:

- Spawning ingredients
- Adding gold
- Instant prestige

## 19. Content Creation Workflow

```text
Create Ingredient ScriptableObject
  ->
Create Machine ScriptableObject
  ->
Create Potion ScriptableObject
  ->
Create Recipe ScriptableObject
  ->
Assign machine and recipe links
  ->
Create Contract ScriptableObject
  ->
Test production chain
```

## 20. Future Scalability

The architecture supports:

- New machines
- New realms
- New ingredients
- New potion chains

This should be possible without code changes, using only data additions.

## 21. Example System Diagram

```text
IngredientDefinition / PotionDefinition / RecipeDefinition / MachineDefinition / ContractDefinition
  ->
ProductionSystem
  ->
PotionInventory
  ->
EconomySystem
  ->
ContractSystem
  ->
Gold Rewards
```

## 22. Testing Strategy

- Unit test economy math
- Test production pipelines
- Simulate 1000 ticks to validate economy balance

## 23. Build Pipeline

- Target platform: Steam Windows
- Optional: Steam Deck support
- Resolution: 1920x1080

## End

End of architecture document.
