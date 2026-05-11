# Event Channel Recommendations

- Project: Witch Pantry
- Engine: Unity 6000.3
- Purpose: Define where ScriptableObject event channels fit into the architecture without overbuilding the prototype.

## Summary

Use event channels for important cross-scene notifications that benefit from loose coupling.

Do not use event channels as a replacement for runtime state.

For the current project state:

- shared mutable values should live in runtime state classes under `Assets/Scripts/Runtime/State/`
- UI and future scene presenters should read from runtime state
- event channels should be used only when one part of the game needs to notify another part without creating a direct scene reference

In short:

```text
Runtime state holds facts
Event channels broadcast notable changes
Scene objects listen and react
```

## What Event Channels Are For

ScriptableObject event channels are useful when:

- a persistent service needs to notify scene presenters
- one scene needs to notify another scene indirectly
- audio should react to gameplay without poking through room hierarchies
- shell UI should react to room or contract events without knowing who triggered them

They are especially useful in the planned additive-scene architecture because:

- `Bootstrap` persists
- `Shell_UI` persists
- gameplay rooms can load and unload
- room objects should not hold hard references to each other

## What Event Channels Are Not For

Do not use event channels for:

- primary inventory storage
- gold totals
- contract progress values
- machine ownership data
- anything that should be queryable as current truth

Those belong in runtime state.

If you ever hear yourself asking "what is the current value," that usually means runtime state, not an event channel.

## Recommended Rule For Witch Pantry

Use this rule:

1. If something is a current fact the game may need to read later, store it in runtime state.
2. If something is a notable occurrence that other systems may react to immediately, raise an event channel.

Examples:

- inventory count of `ingredient.herb`
  - runtime state
- current gold
  - runtime state
- active contract progress
  - runtime state
- room just became active
  - event channel
- contract just completed
  - event channel
- player selected a machine in the room UI
  - event channel

## Recommended Scope For The Current Project

The current implementation should stay small.

Add only these event channels first:

- `RoomActivated`
- `ContractCompleted`
- `MachineSelected`

Optional later:

- `FeaturedDemandChanged`
- `CompactModeChanged`
- `GlobalAlertRaised`

Do not add inventory-changed ScriptableObject channels yet.

Your current `PantryInventoryState`, `GameSessionState`, and future runtime services should use normal C# events for internal state-change notifications. Save ScriptableObject event channels for scene and service boundaries.

## Concrete Fit With The Current Runtime Layer

The runtime layer currently exists under:

- `Assets/Scripts/Runtime/State/PantryInventoryState.cs`
- `Assets/Scripts/Runtime/State/MachineOwnershipState.cs`
- `Assets/Scripts/Runtime/State/ContractRuntimeState.cs`
- `Assets/Scripts/Runtime/State/GameSessionState.cs`

Recommended division of responsibility:

- `PantryInventoryState`
  - stores inventory totals by content definition id
  - raises normal C# events such as `InventoryChanged`
- `GameSessionState`
  - owns inventory, machine ownership, contracts, gold, active room
  - raises normal C# events such as `GoldChanged`, `ActiveRoomChanged`, and `ContractsChanged`
- ScriptableObject event channels
  - notify scene-facing systems that a significant game event just happened

This keeps the prototype sane:

- state remains testable plain C# code
- scene integration stays loosely coupled
- UI and audio can subscribe without dragging state classes into Unity object graphs

## Recommended Folder Structure

```text
Assets/
  Scripts/
    Runtime/
      State/
      Events/
        Channels/
        Payloads/
      Bridges/
```

Suggested responsibilities:

- `State/`
  - plain C# state classes
- `Events/Channels/`
  - ScriptableObject event channel types
- `Events/Payloads/`
  - small payload structs if needed later
- `Bridges/`
  - MonoBehaviours that subscribe to channels and forward behavior to UI, audio, or scene presenters

## Recommended First Event Channel Types

Keep the first pass tiny.

### `StringEventChannelSO`

Use for:

- `RoomActivated`
- simple id-driven events where a string payload is enough

Example payloads:

- `room.main_pantry`
- `machine.enchanted_cauldron`

### `ContractCompletedEventChannelSO`

Use for:

- contract success notifications

Suggested payload fields:

- `ContractDefinitionId`
- `RewardGold`
- `TargetPotionId`

This is better than a bare string because contract completion is likely to feed UI, VFX, audio, and reward handling.

### `MachineSelectedEventChannelSO`

Use for:

- machine selection changes in a room scene

Suggested payload fields:

- `MachineDefinitionId`
- `RoomId`

That is enough for shell UI or inspector panels to react without directly knowing room-object references.

## Recommended First C# Events In Runtime State

Use normal C# events inside the runtime layer first.

Suggested examples:

```csharp
public event Action<string, int> InventoryChanged;
public event Action<int> GoldChanged;
public event Action<string> ActiveRoomChanged;
public event Action ContractsChanged;
```

These events are useful because:

- they are easy to unit test
- they do not require Unity assets
- they do not pretend every state update is a global game event

## Concrete Usage Patterns For Witch Pantry

### Pattern 1: Room Activation

Use when the player changes focus from one room to another.

Flow:

```text
SceneFlowService changes active room
-> GameSessionState updates ActiveRoomId
-> GameSessionState raises ActiveRoomChanged C# event
-> SceneFlowService or a bridge raises RoomActivated event channel
-> Shell UI and AudioManager react
```

Why this works:

- current truth lives in `GameSessionState`
- scene-facing systems react through the channel
- audio does not need direct references to room controllers

### Pattern 2: Contract Completion

Use when a contract reaches fulfillment.

Flow:

```text
ContractRuntimeState reaches completion
-> GameSessionState updates contract list and gold
-> runtime state raises C# notifications
-> gameplay service raises ContractCompleted event channel
-> HUD, celebration UI, and audio react
```

Why this works:

- contract progress remains testable state
- reward feedback remains loosely coupled

### Pattern 3: Machine Selection

Use when the player clicks a placed machine in a room scene.

Flow:

```text
Room presenter detects machine selection
-> raises MachineSelected event channel with machine definition id and room id
-> Shell UI opens machine detail panel
-> panel resolves authored data through the content registry
```

Why this works:

- room scene does not need a hard reference to shell UI
- UI does not need to know about specific scene objects

## What To Avoid Right Now

Avoid these traps:

- one ScriptableObject event channel per tiny state mutation
- firing global event channels for every inventory add and consume
- building generic payload systems before there is a concrete second use
- mixing saved runtime state with scene event transport

If the event list starts looking like a stock-market feed, you built too much too early.

## Recommended Prototype Sequence

For the current implementation order:

1. keep state changes inside plain runtime classes
2. add normal C# events to the runtime state classes
3. add only one ScriptableObject channel for `RoomActivated`
4. add `ContractCompleted` when there is actual contract fulfillment feedback
5. add `MachineSelected` when the shell UI needs cross-scene selection handling

This matches the current project maturity much better than building a full event-bus zoo now.

## Relationship To Existing Architecture Docs

This file expands the event guidance from:

- `Docs/Unity-Scene-Communication-Architecture.md`
- `Docs/Unity-Scene-Architecture.md`

Use those docs for the additive-scene model.
Use this doc for the practical "when should I actually add an event channel" decision.
