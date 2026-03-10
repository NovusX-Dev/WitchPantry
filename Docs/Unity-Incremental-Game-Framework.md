# Unity Incremental Game Framework

For *Witch's Pantry Automation*.

- Engine: Unity 6000.3
- Language: C#

## 1. Overview

This framework provides a complete foundation for building incremental games.

Included systems:

- Production Graph Engine
- Economy Simulator
- Auto-Balancing Tool
- Save System
- UI Architecture
- Event System

The architecture is modular and data-driven.

## 2. Core Architecture

Game layers:

```text
UI Layer
  ->
Gameplay Systems
  ->
Data Layer
  ->
Save System
```

All gameplay values are defined in ScriptableObjects.

## 3. Folder Structure

```text
Assets/
  Scripts/
    Core/
      GameLoop/
      EventBus/
    Systems/
      Production/
      Economy/
      Prestige/
      Contracts/
    Tools/
      EconomySimulator/
      AutoBalance/
    UI/
      Panels/
      Views/
    Save/
      Serialization/
      OfflineSimulation/
    ScriptableObjects/
      Ingredients/
      Recipes/
      Machines/
      EconomySettings/
```

## 4. Core Game Loop

Tick-based simulation.

- Tick = 1 second

Flow:

```text
GameLoopManager.Update()
  ->
TickSimulation()
  ->
ProductionSystem
  ->
EconomySystem
  ->
UI Update
```

## 5. Production Graph Engine

Machines are nodes. Resources flow between nodes.

Example chain:

```text
Herb Farm
  ->
Grinder
  ->
Cauldron
  ->
Bottle Station
  ->
Potion Storage
```

Each machine has:

- Inputs
- Outputs
- Processing time
- Buffers

## 6. Machine Definition

Use a ScriptableObject called `MachineDefinition` with fields such as:

- `machineID`
- `baseProduction`
- `processTime`
- `inputResources`
- `outputResources`

Example machine:

- Name: `MortarGolem`
- Inputs: `Mushroom`
- Outputs: `Powder`

## 7. Resource System

Generic resource model:

```csharp
class ResourceStack
{
    ResourceType type;
    double amount;
}
```

Resources include:

- Ingredients
- Potions
- Currency

## 8. Production Simulation

Each tick:

1. Update machines.
2. Consume resources.
3. Produce outputs.
4. Move resources along the graph.

Pseudo-code:

```text
for node in machines
    node.Tick()

for connection in edges
    transfer resources
```

## 9. Economy System

Handles gold generation.

Example formula:

```text
gold += potionValue * potionsProduced
```

Potion value:

```text
value = baseValue * rarityMultiplier
```

## 10. Cost Curve

```text
cost = baseCost * growthRate^owned
```

Example:

- `baseCost = 10`
- `growthRate = 1.15`

## 11. Production Curve

```text
production = baseProduction * machines * multipliers
```

Example:

- `baseProduction = 2`
- `machines = 20`
- `multiplier = 1.5`
- `production = 60/sec`

## 12. Prestige System

Prestige resets progress but grants a multiplier.

```text
prestigeMultiplier = 1 + sqrt(totalGold)
```

Example:

- `totalGold = 1e6`
- `multiplier = 101`

## 13. Economy Simulator Tool

Editor tool for testing the economy.

Features:

- Production graph simulation
- Upgrade purchases
- Graph visualization

## 14. Simulator Algorithm

```text
for tick in simulation
    gold += production

    if gold >= nextMachineCost
        buy machine
        increase production

record data
```

## 15. Data Recorder

Tracks simulation metrics.

```text
SimulationData
  time[]
  gold[]
  machines[]
  production[]
```

## 16. Graph Visualization

Graph types:

- Gold vs. Time
- Production vs. Time
- Machines vs. Time
- Upgrade Cost vs. Time

Graphs help identify economy problems.

## 17. Auto-Balancing Tool

Automatically searches for good economy parameters.

Algorithm:

1. Randomize economy values.
2. Run simulation.
3. Score the progression curve.
4. Repeat 1000 times.
5. Keep the best parameters.

## 18. Curve Quality Metrics

Score factors:

- Upgrade pacing
- Prestige timing
- Inflation rate
- Player reward frequency

Example:

```text
score = rewardFrequency - stallPenalty
```

## 19. Save System

Save structure:

```text
SaveData
  playerState
  productionState
  economyState
  prestigeState
```

Use JSON serialization.

Save events:

- Auto-save every 30 seconds
- On quit
- On major purchase

## 20. Offline Simulation

When the player returns:

```text
offlineTime = now - lastSave
offlineGold = production * offlineTime
```

## 21. UI Architecture

Use an MVVM pattern.

- Views: Unity UI objects
- ViewModels: bind UI to gameplay systems

Example panels:

- Inventory Panel
- Machine Panel
- Contracts Panel
- Prestige Panel

## 22. Debug Tools

Developer console should allow:

- Spawn resources
- Add gold
- Simulate ticks
- Force prestige

## 23. Performance Strategy

Use tick simulation and avoid frame-based logic.

Example:

- `1 update per second`

This keeps large factories performant.

## 24. Content Pipeline

Adding new content:

```text
Create ingredient ScriptableObject
  ->
Create recipe ScriptableObject
  ->
Create machine definition
  ->
Connect production graph
```

No code changes required.

## 25. Future Expansion

Possible features:

- Node graph editor
- Automation AI helpers
- Random events
- Rare ingredients
- Seasonal content

## End

End of framework.
