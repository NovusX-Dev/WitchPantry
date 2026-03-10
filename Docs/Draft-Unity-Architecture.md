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

All game content lives in ScriptableObjects.

Example types:

- `IngredientDefinition`
- `RecipeDefinition`
- `MachineDefinition`
- `ContractDefinition`

Example `IngredientDefinition` fields:

- `id`
- `name`
- `rarity`
- `icon`
- `baseValue`

Example `RecipeDefinition` fields:

- `inputs[]`
- `outputPotion`
- `brewTime`
- `value`

Example `MachineDefinition` fields:

- `speed`
- `capacity`
- `energyCost`
- `inputSlots`
- `outputSlots`

## 6. Runtime Game State

Runtime data must not live in ScriptableObjects.

Use plain C# classes:

```csharp
class PlayerState
{
    Dictionary<IngredientID, double> inventory;
    Dictionary<MachineID, int> machines;
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

Example class:

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

- `machineDefinition`
- `progress`
- `inputBuffer`
- `outputBuffer`

## 8. Economy System

Handles:

- Gold generation
- Contracts
- Potion values

Example:

```csharp
class EconomySystem
{
    double gold;

    void SellPotion()
    {
    }
}
```

## 9. Contract System

Contracts are generated dynamically.

Example contract:

- Name: Village Contract
- Objective: Produce 50 Healing Potions
- Reward: 500 gold

Structure:

```csharp
class Contract
{
    string potionType;
    int amountRequired;
    double reward;
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
    PrestigeState prestigeState;
}
```

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
Create Recipe ScriptableObject
  ->
Assign machines
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
IngredientSystem
  ->
ProductionSystem
  ->
PotionInventory
  ->
EconomySystem
  ->
Contracts
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
