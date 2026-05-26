# Unity Scene Communication Architecture

- Project: Witch's Pantry
- Engine: Unity 6; current project version: `Witch-Pantry/ProjectSettings/ProjectVersion.txt`
- Purpose: Translate the scene architecture decision into a practical Unity implementation model for runtime state, event flow, UI updates, room registration, and audio coordination.

## Summary

Use persistent services in `Bootstrap`, a persistent presentation shell in `Shell_UI`, and room-local presenters inside gameplay scenes.

Cross-scene coordination should primarily use:

- shared runtime state
- event channels for important notifications
- scene lifecycle hooks for additive scene registration

Do not make cross-scene references the default pattern.

## Design Goals

This architecture should make it easy to:

- keep UI synchronized with room activity
- add and unload room scenes safely
- support compact desktop-idler mode
- keep audio aware of room context and major gameplay events
- avoid hard references between transient room hierarchies and persistent systems

## High-Level Structure

```text
Bootstrap
  -> creates persistent services
  -> loads Shell_UI
  -> loads MainPantry

Shell_UI
  -> reads runtime state
  -> listens to event channels
  -> displays whole-pantry status

Gameplay Room Scenes
  -> own room visuals and local presenters
  -> write to runtime state
  -> raise room and gameplay events
```

## Recommended Folder Structure

```text
Assets/
  Scripts/
    Core/
      Bootstrap/
      SceneFlow/
      Runtime/
      Events/
      Audio/
    Gameplay/
      Rooms/
      Machines/
      Contracts/
    UI/
      HUD/
      CompactMode/
      Panels/
  ScriptableObjects/
    Runtime/
    Events/
    Audio/
  Prefabs/
    Core/
    UI/
    Rooms/
  Scenes/
    Core/
      Bootstrap.unity
      Shell_UI.unity
    Gameplay/
      MainPantry.unity
      GreenhouseWing.unity
      PrepLab.unity
      FulfillmentNook.unity
```

## Core Runtime Services

These should live in `Bootstrap` and remain available while normal gameplay room scenes load and unload.

### `GameSession`

Purpose:

- own the current play session
- coordinate save/load application
- expose high-level game phase or mode

Suggested responsibilities:

- initialize core systems
- restore runtime state from save data
- trigger initial scene loading

### `PantryState`

Purpose:

- act as the whole-pantry runtime state model
- build on the existing plain C# runtime-state classes under `Assets/Scripts/Runtime/State/`

Suggested responsibilities:

- inventory totals
- throughput summaries
- bottleneck summary
- selected room id
- selected machine id if globally relevant
- quick summary data for compact mode

Suggested API shape:

```csharp
public interface IPantryState
{
    IReadOnlyDictionary<string, int> Inventory { get; }
    float PotionsPerMinute { get; }
    string CurrentBottleneckKey { get; }
    string ActiveRoomId { get; }

    event Action InventoryChanged;
    event Action ThroughputChanged;
    event Action BottleneckChanged;
    event Action<string> ActiveRoomChanged;

    void AddItem(string itemId, int amount);
    bool TryConsumeItem(string itemId, int amount);
    void SetBottleneck(string bottleneckKey);
    void SetActiveRoom(string roomId);
}
```

### `ContractState`

Purpose:

- hold featured demand and contract progress

Suggested responsibilities:

- active contract list
- featured demand card
- progress values
- contract-complete notification triggers

### `RoomStateRegistry`

Purpose:

- track which room scenes are loaded and what they represent

Suggested responsibilities:

- loaded room ids
- room roles
- room status summary
- room contribution hints for UI

Suggested API shape:

```csharp
public interface IRoomStateRegistry
{
    IReadOnlyList<RoomRuntimeHandle> LoadedRooms { get; }

    event Action<RoomRuntimeHandle> RoomRegistered;
    event Action<RoomRuntimeHandle> RoomUnregistered;

    void Register(RoomRuntimeHandle handle);
    void Unregister(RoomRuntimeHandle handle);
    bool TryGetRoom(string roomId, out RoomRuntimeHandle handle);
}
```

### `SceneFlowService`

Purpose:

- manage additive scene loading and activation

Suggested responsibilities:

- load persistent scenes at startup
- load and unload room scenes
- set active scene when appropriate
- publish room activation events

Suggested API shape:

```csharp
public interface ISceneFlowService
{
    UniTask LoadInitialGameScenesAsync();
    UniTask LoadRoomAsync(string sceneName);
    UniTask UnloadRoomAsync(string sceneName);
    UniTask ActivateRoomAsync(string roomId);
}
```

If you do not want async wrappers yet, start with plain `Task` or coroutine-based orchestration and keep the interface narrow.

### `AudioManager`

Purpose:

- own music, ambience, mixer state, and high-level stingers

Suggested responsibilities:

- react to room activation
- react to contract completion
- react to compact mode changes
- swap mixer snapshots
- play global one-shots when needed

Keep room-local machinery sounds out of this service.

## Room-Level Components

These should live inside room scenes.

### `RoomSceneContext`

Purpose:

- define the room identity and expose room-local references

Suggested fields:

- `roomId`
- `roomRole`
- camera anchor references
- room-local ambience anchor
- room-local machine container root

This is the room's self-description, not the whole game state.

### `RoomSceneRegistrar`

Purpose:

- register and unregister the room with persistent services when the additive scene loads or unloads

Suggested responsibilities:

- locate `RoomStateRegistry`
- create a `RoomRuntimeHandle`
- register on `OnEnable` or controlled initialization
- unregister on scene unload

Example shape:

```csharp
public sealed class RoomSceneRegistrar : MonoBehaviour
{
    [SerializeField] private RoomSceneContext context;

    private IRoomStateRegistry registry;
    private RoomRuntimeHandle handle;

    public void Initialize(IRoomStateRegistry roomRegistry)
    {
        registry = roomRegistry;
        handle = RoomRuntimeHandle.FromContext(context);
        registry.Register(handle);
    }

    private void OnDestroy()
    {
        registry?.Unregister(handle);
    }
}
```

### Machine Runtime and Presenters

Recommended split:

- machine runtime logic updates state and progression data
- machine presenter updates visuals and local room SFX

Machine logic should not call HUD code directly.

Machine presenters should not become your inventory system in disguise.

## UI Layer Structure

`Shell_UI` should contain persistent UI roots and presenters.

Recommended UI composition:

- `HudPresenter`
- `CompactModePresenter`
- `BottleneckBannerPresenter`
- `ContractPanelPresenter`
- `NotificationFeedPresenter`

These presenters should subscribe to:

- `PantryState`
- `ContractState`
- room activation event channels
- room summary data from `RoomStateRegistry`

### Example: HUD Update Flow

```text
Machine cycle completes in MainPantry
-> machine runtime updates PantryState
-> PantryState raises ThroughputChanged
-> HudPresenter receives event
-> HUD refreshes potions/min and bottleneck card
```

### Example: Compact Mode Update Flow

```text
PrepLab stalls due to missing bottles
-> PantryState updates bottleneck summary
-> RoomStateRegistry updates PrepLab alert state
-> CompactModePresenter refreshes summary and room-needing-attention line
```

## Event Channel Recommendations

Use ScriptableObject event channels for important gameplay notifications that need loose coupling.

The detailed guidance now lives in:

- [Event-Channel-Recommendations.md](C:/Unity/Repos/WitchPantry/Docs/Event-Channel-Recommendations.md)

Short version:

- runtime state holds current truth
- normal C# events are already the first runtime-state notification layer
- ScriptableObject event channels are best for cross-scene notifications such as `RoomActivated`, `ContractCompleted`, and `MachineSelected`
- do not replace existing runtime-state events with ScriptableObject channels for inventory, gold, machine ownership, or active contract list changes

## Audio Architecture

Split audio into global and local responsibilities.

### Global Audio

Owned by `AudioManager` in `Bootstrap`.

Responsible for:

- music state
- ambience family selection
- mixer snapshots
- UI alert sounds
- contract success stingers
- compact-mode ducking rules

### Local Room Audio

Owned by room scene objects.

Responsible for:

- cauldron bubbling
- grinding loops
- shelf clinks
- room-local one-shots
- spatialized machine sound where appropriate

### Audio Event Flow Example

```text
SceneFlowService activates GreenhouseWing
-> raises RoomActivated(GreenhouseWing)
-> AudioManager transitions to greenhouse-weighted ambience snapshot

ContractState completes featured contract
-> raises ContractCompleted
-> AudioManager plays success stinger

Mortar machine jams in MainPantry
-> local machine presenter plays jam SFX
-> optional GlobalAlertRaised event updates UI banner and alert ping
```

## Scene Loading and Registration Flow

Recommended additive load flow:

```text
Bootstrap scene starts
-> GameSession initializes persistent services
-> SceneFlowService loads Shell_UI additively
-> SceneFlowService loads MainPantry additively
-> MainPantry registrar registers room in RoomStateRegistry
-> SceneFlowService marks MainPantry as active room
-> PantryState and event channels notify UI and AudioManager
```

When a new room loads later:

```text
SceneFlowService loads PrepLab additively
-> SceneManager.sceneLoaded fires
-> room registrar initializes and registers room metadata
-> RoomStateRegistry updates loaded-room list
-> UI can expose room switch control
-> Audio and UI react only if PrepLab becomes active
```

## Suggested Save/Load Boundaries

Save data should serialize data, not scene object references.

Good save targets:

- inventory values
- contract progress
- unlocked room ids
- room layout data
- machine placements by room id
- upgrade states

Avoid saving:

- direct references to room-scene instances
- transient UI state that can be rebuilt
- raw scene hierarchy pointers

## Recommended First Pass Implementation Order

1. Create `Bootstrap`, `Shell_UI`, and `MainPantry`
2. Implement `GameSession`, `PantryState`, and `SceneFlowService`
3. Implement `RoomSceneContext` and `RoomSceneRegistrar`
4. Wire `HudPresenter` to `PantryState`
5. Add one event channel for `RoomActivated`
6. Add `AudioManager` support for room activation and contract success
7. Add `RoomStateRegistry` when a second room actually exists

Do not build twelve generic infrastructure systems before the pantry can boil one potion.

## Practical Rule of Restraint

This architecture exists to support a readable cozy factory game, not to win an abstract architecture beauty contest.

Use the smallest version that keeps these truths intact:

- state lives outside transient room scenes
- UI does not depend on room object lookups
- audio reacts to events and room context
- new rooms can be loaded without rewriting the whole game

If a pattern adds more ceremony than clarity, cut it.

## Related Docs

- [Unity-Scene-Architecture.md](C:/Unity/Repos/WitchPantry/Docs/Unity-Scene-Architecture.md)
- [Spatial-Pantry-System.md](C:/Unity/Repos/WitchPantry/Docs/Spatial-Pantry-System.md)
- [Witchs-Pantry-Production-Blueprint.md](C:/Unity/Repos/WitchPantry/Docs/Witchs-Pantry-Production-Blueprint.md)
- [Witchs-Pantry-UI-Design.md](C:/Unity/Repos/WitchPantry/Docs/Witchs-Pantry-UI-Design.md)

## Unity References

- [Use ScriptableObjects as Event Channels in Your Code](https://unity.com/how-to/scriptableobjects-event-channels-game-code)
- [SceneManager.sceneLoaded](https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager-sceneLoaded.html)
- [LoadSceneMode.Additive](https://docs.unity3d.com/ScriptReference/SceneManagement.LoadSceneMode.Additive.html)
- [SceneManager.SetActiveScene](https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.SetActiveScene.html)
- [Work with multiple scenes in Unity](https://docs.unity3d.com/Manual/MultiSceneEditing.html)
- [Audio Mixer](https://docs.unity3d.com/Manual/AudioMixer.html)
