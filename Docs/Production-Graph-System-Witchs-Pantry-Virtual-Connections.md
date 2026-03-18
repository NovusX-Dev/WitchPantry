# Witch's Pantry Automation Production System

- Edition: Virtual Connections
- Engine: Unity 6000.3
- Language: C#
- Connection Model: Virtual resource flow

## 1. Overview

This version replaces explicit node-to-node transport and visible connectors with a virtual connection model.

Machines are not physically connected with belts, pipes, or wires. Instead:

- All machines read from and write to a shared pantry inventory.
- Each machine checks whether its required inputs are available.
- If the inputs exist, the machine consumes them and produces outputs.
- Outputs become available to every other eligible machine through the same shared inventory.

This keeps the game visually clean, easier to build in 3 months, and closer to a cozy incremental game than a logistics-heavy factory simulator.

## 2. Design Goal

The player's mental model should be:

```text
Unlock machine -> place machine in pantry -> machine starts contributing automatically
```

Not:

```text
Place belts -> connect outputs -> debug routing
```

Production depth comes from:

- Machine unlock order
- Machine counts
- Upgrade choices
- Recipe dependencies
- Resource bottlenecks
- Market priorities

## 3. Core Concepts

### Machine

A gameplay unit that consumes and or produces resources on a tick.

Examples:

- Herb Garden
- Mushroom Cave
- Mortar Golem
- Enchanted Cauldron
- Bottling Sprite
- Storage Shelf

### Shared Inventory

A global resource pool that all machines can access.

Example stored resources:

- Herbs
- Mushrooms
- Powder
- Potion Base
- Healing Potion
- Gold

### Tick

The simulation step. Example: `1 tick = 1 second`.

### Recipe

A rule that defines what a machine consumes and what it produces.

## 4. Virtual Connection Model

### Old Model

The previous design described a directed node graph with edges and explicit transfer between source and target machines.

### New Model

Machines are connected virtually by resource availability.

Example:

1. Herb Garden produces `Herb`.
2. Herb is added to shared inventory.
3. Mortar Golem checks inventory.
4. If Herb is available, Mortar Golem consumes Herb and produces `Ground Herb`.
5. Enchanted Cauldron checks inventory.
6. If recipe ingredients are available, it produces `Healing Potion`.

No explicit connector object is needed for normal gameplay.

## 5. Player-Facing Interpretation

The player sees machines laid out visually in the workshop, but they do not draw links between them.

Example visual flow:

`Herb Garden -> Mortar Golem -> Enchanted Cauldron -> Bottling Sprite -> Market`

That flow is for readability and theme, not for actual transport simulation.

Gameplay logic:

- Herb Garden adds Herbs to inventory.
- Mortar Golem consumes Herbs from inventory.
- Cauldron consumes processed ingredients from inventory.
- Bottling Sprite consumes potions from inventory.
- Market sells bottled potions from inventory.

## 6. Why Virtual Connections Are Better Here

Advantages:

- Much faster to build
- Lower UI complexity
- Easier balancing
- Easier save and load logic
- Easier offline progression
- Easier for players to understand
- Better fit for a 3-month solo production

Tradeoff:

You lose the logistics puzzle of physical routing, but gain stronger pacing, cleaner UX, faster development, and easier content expansion.

## 7. System Architecture

The production system is made of these layers:

- Static data: `ContentDefinition`, `IngredientDefinition`, `PotionDefinition`, `RecipeDefinition`, `MachineDefinition`, and `ContractDefinition` hold authored gameplay data.
- Runtime state: runtime classes track owned machines, machine progress, upgrades, and inventory totals.
- Tick simulation: every second, all active machines run their processing logic.
- Shared inventory: resources are added and removed from one central store.

## 8. Shared Inventory Structure

```csharp
using System;
using System.Collections.Generic;

[Serializable]
public class PantryInventory
{
    private readonly Dictionary<string, double> _resources = new();

    public double GetAmount(string resourceId)
    {
        return _resources.TryGetValue(resourceId, out var value) ? value : 0d;
    }

    public void Add(string resourceId, double amount)
    {
        if (amount <= 0) return;

        if (!_resources.ContainsKey(resourceId))
            _resources[resourceId] = 0;

        _resources[resourceId] += amount;
    }

    public bool Has(string resourceId, double amount)
    {
        return GetAmount(resourceId) >= amount;
    }

    public bool TryConsume(string resourceId, double amount)
    {
        if (!Has(resourceId, amount))
            return false;

        _resources[resourceId] -= amount;
        return true;
    }
}
```

## 9. Resource Stack Model

```csharp
using System;

[Serializable]
public struct IngredientAmount
{
    public IngredientDefinition ingredient;
    public int amount;
}

[Serializable]
public struct UnlockSource
{
    public UnlockSourceType Type;
    public string SourceId;
}

[Serializable]
public struct OutputAmount
{
    public ContentDefinition output;
    public int amount;
}
```

## 10. Machine Definition Data

Use ScriptableObjects for machine definitions so behavior is data-driven and aligned with
the current content-definition layer.

```csharp
using UnityEngine;

[CreateAssetMenu(menuName = "WitchPantry/Machine Definition")]
public class MachineDefinition : ContentDefinition
{
    public float PurchaseCost { get; private set; }
    public float UpgradeCost { get; private set; }
    public float ProcessingSpeed { get; private set; }
    public int QueueCapacity { get; private set; }
    public float EnergyCost { get; private set; }
    public Vector2Int Footprint { get; private set; }
    public RecipeDefinition[] SupportedRecipes { get; private set; }
    public MachineCategory MachineCategory { get; private set; }
    public bool CanRunOffline { get; private set; }
}
```

Authoring note:

- the current editor tooling auto-seeds `UnlockSource.SourceId` prefixes based on `UnlockSource.Type`
- `StartingContent` keeps `SourceId` empty and disables editing in the inspector

## 11. Machine Runtime Instance

```csharp
using System;

[Serializable]
public class MachineInstance
{
    public string machineDefinitionId;
    public string assignedRecipeId;
    public int level = 1;
    public int owned = 0;
    public float progress = 0f;

    public MachineInstance(string machineDefinitionId)
    {
        this.machineDefinitionId = machineDefinitionId;
    }
}
```

## 12. Machine Processing Rules

### Producer Machine

Produces resources without inputs.

Examples:

- Herb Garden: `+1 Herb per cycle`
- Mushroom Cave: `+1 Mushroom per cycle`

### Converter Machine

Consumes inputs and produces outputs.

Examples:

- Mortar Golem: `Herb -> Ground Herb`
- Enchanted Cauldron: `Ground Herb + Water -> Healing Potion`

### Utility Machine

Applies a modifier rather than direct conversion.

Examples:

- Storage Shelf: value bonus to stored potions
- Labeling Imp: market value bonus to bottled potions

## 13. Machine Processing Flow

Every tick:

1. Loop over active machines.
2. Resolve the current machine definition and assigned recipe.
3. Check shared inventory for the recipe inputs.
4. Advance progress using recipe craft time plus machine speed modifiers.
5. Write outputs back to shared inventory when complete.
6. Recalculate economy and UI.

## 14. Production System Example

```csharp
using System.Collections.Generic;
using UnityEngine;

public class ProductionSystem
{
    private readonly PantryInventory _inventory;
    private readonly Dictionary<string, MachineDefinition> _definitions;
    private readonly List<MachineInstance> _machines;

    public ProductionSystem(
        PantryInventory inventory,
        Dictionary<string, MachineDefinition> definitions,
        List<MachineInstance> machines)
    {
        _inventory = inventory;
        _definitions = definitions;
        _machines = machines;
    }

    public void Tick(float deltaTime)
    {
        foreach (var machine in _machines)
        {
            if (machine.owned <= 0) continue;
            if (!_definitions.TryGetValue(machine.machineDefinitionId, out var def)) continue;

            for (int i = 0; i < machine.owned; i++)
            {
                ProcessSingleMachine(machine, def, deltaTime);
            }
        }
    }

    private void ProcessSingleMachine(
        MachineInstance machine,
        MachineDefinition def,
        float deltaTime)
    {
        machine.progress += deltaTime;
    }

    private bool HasAllInputs(IngredientAmount[] inputs)
    {
        if (inputs == null) return true;

        foreach (var input in inputs)
        {
            if (!_inventory.Has(input.ingredient.Id, input.amount))
                return false;
        }

        return true;
    }

    private void ConsumeInputs(IngredientAmount[] inputs)
    {
        if (inputs == null) return;

        foreach (var input in inputs)
            _inventory.TryConsume(input.ingredient.Id, input.amount);
    }

    private void ProduceOutputs(OutputAmount[] outputs)
    {
        if (outputs == null) return;

        foreach (var output in outputs)
        {
            _inventory.Add(output.output.Id, output.amount);
        }
    }
}
```

## 15. Example Production Chain

### Healing Potion

Machines owned:

- Herb Garden x2
- Mortar Golem x1
- Enchanted Cauldron x1
- Bottling Sprite x1

Simulation flow:

- Herb Garden adds `Herb` to inventory.
- Mortar Golem consumes `Herb` and produces `GroundHerb`.
- Enchanted Cauldron consumes `GroundHerb` and `Water`, then produces `HealingPotion`.
- Bottling Sprite consumes `HealingPotion` and produces `BottledHealingPotion`.
- Market sells `BottledHealingPotion` for gold.

No physical connector exists. The connection is purely logical through the shared inventory.

## 16. Example Layout in the Scene

```text
[Herb Garden]     [Mushroom Cave]

      [Mortar Golem]   [Crystal Grinder]

          [Enchanted Cauldron]

            [Bottling Sprite]

             [Storage Shelf]

                [Market]
```

This layout communicates progression and workshop fantasy. It does not imply physical transport.

## 17. Build Flow for the Player

1. Unlock a machine.
2. Place it into an available workshop slot.
3. If its required recipe inputs exist in shared inventory, it starts working automatically.
4. Upgrade it to improve speed, output quantity, efficiency, or value bonus.

## 18. UI Recommendations

Because connections are virtual, the UI must explain dependencies clearly.

Recommended machine tooltip should show:

- Inputs required
- Outputs produced
- Cycle time
- Current status
- Bottleneck reason

Example:

```text
Mortar Golem
Consumes: 1 Herb
Produces: 1 Ground Herb
Cycle Time: 2.0s
Status: Waiting for Herbs
```

Recommended recipe panel:

`Herb -> Ground Herb -> Healing Potion -> Bottled Healing Potion -> Gold`

## 19. Bottleneck Feedback

Add statuses such as:

- Waiting for input
- Processing
- Outputting
- Idle
- Blocked by storage cap

## 20. Build Slots

Use visual placement slots in the workshop, for example:

- Ingredients Row
- Processing Row
- Brewing Row
- Packaging Row
- Market Row

Machines still use virtual connections, but slot rows make the workshop feel organized and satisfying.

## 21. Save System Implications

You only need to serialize:

- Inventory amounts
- Machine counts
- Machine levels
- Machine progress timers
- Player gold
- Prestige values

You do not need to save physical edges, routing states, or transfer queues.

## 22. Offline Progression

Example flow:

1. Load save.
2. Compute elapsed seconds.
3. Run fast-forward production ticks.
4. Apply resource deltas.
5. Show offline summary popup.

## 23. Performance Notes

This model is cheap because it simulates:

- Machine counts
- Recipe checks
- Inventory additions and removals

It does not simulate:

- Moving items on belts
- Pathfinding
- Transfer animation state
- Connector resolution

This makes it ideal for low CPU cost and large machine counts.

## 24. Recommended MVP Machine Set

### Producers

- Herb Garden
- Mushroom Cave
- Crystal Node

### Converters

- Mortar Golem
- Crystal Grinder
- Enchanted Cauldron

### Utility

- Bottling Sprite
- Storage Shelf
- Market Cart

## 25. Testing Checklist

Functional tests:

- Producer adds resources
- Converter consumes correct inputs
- Machine stalls when input is missing
- Output appears in shared inventory
- Bottled potions sell correctly

Economy tests:

- Progression does not stall too early
- Machine costs scale correctly
- Upgrades feel meaningful

Save and load tests:

- Machine progress restores correctly
- Inventory persists
- Offline progression does not duplicate output

## 26. Final Recommendation

Use virtual connections only for the shipped version.

Represent machine relationships visually in the UI and workshop layout, but keep the simulation based on shared pantry inventory, recipe dependency checks, and tick-based processing.

## 27. Summary

### What Changed from the Previous File

- Removed explicit edge-based gameplay logic
- Removed transporter as a required machine type
- Removed source-target transfer model for normal production
- Replaced it with shared inventory and virtual dependencies

### What Remains Useful

- Tick-based simulation
- Machine definitions
- Producer and converter roles
- Runtime machine instances
- Offline progression
- Debug tools

## 28. One-Line Mental Model

Machines are visually placed in the workshop, but logically connected through a shared magical pantry inventory.
