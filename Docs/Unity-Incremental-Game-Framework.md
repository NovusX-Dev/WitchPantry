# Unity Incremental Game Framework

For *Witch's Pantry*.

- Engine: Unity 6; current project version: `Witch-Pantry/ProjectSettings/ProjectVersion.txt`
- Language: C#

## 1. Purpose

This document defines the Unity framework shape for Witch Pantry's incremental systems.

It is not a generic idle-game framework. Every system should support the visible spatial pantry, customer demand, compact desktop-idler play, or the demo path.

## 2. Source Of Truth Rules

- ScriptableObjects define authored content:
  - ingredients
  - potions
  - recipes
  - machines
  - contracts
  - biomes and future unlock metadata
- Plain C# runtime state stores mutable play truth:
  - inventory totals
  - owned machines
  - active contracts
  - gold
  - active room
- MonoBehaviours present and adapt:
  - room visuals
  - machine animation
  - UI panels
  - input
  - audio/VFX triggers

Do not store mutable player progress in authored content assets.

## 3. Core Runtime Loop

Recommended first loop:

```text
tick simulation
  ->
run active machines from authored definitions and runtime ownership
  ->
consume and produce shared pantry inventory
  ->
update contract progress
  ->
raise plain C# runtime-state events
  ->
refresh UI / compact summaries / scene presenters
```

Use a predictable tick cadence for simulation. The first implementation can be simple; the important rule is that production logic stays testable outside scene hierarchies.

## 4. Folder Direction

Current and near-term code should converge around:

```text
Assets/Scripts/
  Data/
    ContentDefinition/
  Runtime/
    State/
    Simulation/
    Services/
  Gameplay/
    Rooms/
    Machines/
    Contracts/
  UI/
    HUD/
    CompactMode/
  Editor/
```

Avoid rebuilding old generic global-event folders just because they appear in idle-game tutorials.

## 5. Production Graph Shape

Witch Pantry uses virtual connections through shared pantry inventory, not belts, pipes, or direct scene-object links.

Machine runtime instances should know:

- machine definition id
- room id or placement context
- assigned recipe id when relevant
- progress
- upgrade level
- current operating status

They should not own the whole inventory, UI, contract system, or room navigation.

## 6. Economy And Progression

The economy should be tuned around pantry readability:

- early upgrades should create visible production changes
- costs can grow exponentially, but player feedback must stay concrete
- contracts should guide what the player builds next
- room growth should happen when the starter room becomes meaningfully crowded or strategically constrained
- prestige and research belong after the core pantry loop is proven

## 7. Communication Pattern

Use the smallest communication layer that fits the boundary:

- direct method calls inside one cohesive runtime service
- plain C# events from runtime state for testable state changes
- ScriptableObject event channels later for cross-scene notifications such as room activation, contract completion feedback, or machine selection

Do not fire global channels for every inventory add or consume.

## 8. Save And Offline Boundaries

Save data should serialize stable ids and runtime values:

- inventory counts
- machine ownership and upgrade levels
- active room id
- active contract progress
- gold
- offline timestamp

Save data should not serialize scene references, transient UI state, or mutable ScriptableObject instances.

## 9. Testing Strategy

Prioritize tests around:

- runtime state mutation and event firing
- production consumption/output math
- contract progress and completion
- offline simulation results
- economy pacing checkpoints

The first proof should be a small pantry loop that can run without a scene, then a scene presentation that reads the same truth.

## 10. Good Framework Test

Ask:

- Can this system be explained through the starter pantry room?
- Can it be tested without clicking Unity scene objects?
- Does it make the pantry more readable, more satisfying, or more useful in compact mode?

If not, it is probably framework theater.
