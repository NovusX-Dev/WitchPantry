# Unity Scene Architecture

- Project: Witch's Pantry
- Engine: Unity 6; current project version: `Witch-Pantry/ProjectSettings/ProjectVersion.txt`
- Purpose: Define the recommended Unity scene structure for a cozy automation / incremental game built around a spatial pantry.

## Summary

Witch's Pantry should not use one giant gameplay scene, and it should not use one scene per tiny gameplay feature.

The recommended approach is:

- one persistent bootstrap scene
- one persistent shell and UI scene
- one main pantry gameplay scene for the early game
- a small number of additive specialty room scenes later

This preserves readability, supports compact desktop-idler mode, keeps the pantry feeling like one connected magical workplace, and avoids scene-management bloat.

## Decision

Use a small additive scene stack.

Recommended base stack:

- `Bootstrap`
- `Shell_UI`
- `MainPantry`

Recommended expansion scenes later:

- `GreenhouseWing`
- `PrepLab`
- `FulfillmentNook`
- `RareIngredientLab` only if it clearly adds value

## Why This Fits Witch's Pantry

Witch's Pantry is not a traditional level-based game. It is a readable, watchable, room-driven factory space.

The project direction already establishes that:

- the player should understand production in a few seconds
- one production chain should usually be readable in one view
- the game starts with one main pantry room
- the game expands into a small number of connected specialty rooms later
- compact mode should summarize the whole pantry without forcing room-by-room micromanagement

That means the scene model should support:

- stable persistent systems
- fast room switching
- shared runtime state
- clear room identity
- low editor and implementation friction

## Recommended Scene Roles

### `Bootstrap`

Purpose:

- application startup
- service initialization
- save/load bootstrap
- audio, config, and shared runtime wiring
- loading the persistent shell and first gameplay scene

Keep this scene small and stable.

It should usually contain:

- core service installers
- game/session initializer
- scene flow coordinator

It should not contain:

- pantry content
- room art
- feature-heavy UI

### `Shell_UI`

Purpose:

- persistent top-level UI
- HUD
- overlays
- modal host
- compact desktop-idler presentation
- top-level scene and room navigation

This acts like the game's persistent shell.

It is the right place for:

- gold, demand, throughput, bottleneck display
- bottom rail and right rail shells
- compact mode controller
- pause/settings overlays
- transition and notification presenters

It is not the right place for:

- room-local world objects
- machine instances
- room-specific logic

## Gameplay Room Scenes

### `MainPantry`

This is the primary gameplay scene for prototype, demo, and early progression.

It should prove:

- one readable full production chain
- visible machine activity
- meaningful adjacency
- one obvious bottleneck and one obvious optimization opportunity

### Specialty Room Scenes

Add these only when the starter room has already proven the full loop and new content would otherwise damage readability.

Good candidates:

- source-focused room such as a greenhouse or ingredient wing
- prep-focused room for grinders, stills, and conversion machines
- fulfillment-focused room for dispatch, shelving, and demand bonuses
- high-tier rare ingredient room only if it adds real strategy and spectacle

Hard guideline:

- avoid more than four actively managed primary rooms

## What Should Not Be a Separate Scene

The following should usually stay out of separate gameplay scenes:

- each ingredient source
- each biome unlock
- each machine family
- each UI panel
- each contract or demand screen

Biomes should usually be implemented as:

- content unlocks
- room themes
- machine/source content
- progression gates
- data and art variation

Only make a biome a separate scene when the player is truly managing it as a distinct physical pantry space.

Example:

- `Mushroom Cave` as a later pantry wing can justify a scene
- `Mushroom` as a simple ingredient source does not

## Why Not One Giant Scene

One giant scene looks simpler at first, but it creates predictable problems:

- bloated hierarchy
- slower scene iteration
- harder merges
- more room clutter
- weaker visual identity
- harder bottleneck readability
- more camera/navigation problems as the pantry expands

For this genre, the biggest design risk is turning the factory into unreadable visual soup.

One giant scene increases that risk.

## Why Not One Scene Per Biome or Feature

Over-splitting scenes also causes problems:

- too much scene orchestration
- more transitions than the fantasy needs
- higher implementation overhead
- a fragmented feeling instead of one connected magical workplace
- increased maintenance for lighting, references, and navigation

This project should feel like one pantry with connected wings, not a stack of unrelated work tabs.

## Runtime Model

Use shared runtime state across rooms.

Recommended runtime rule:

- keep pantry inventory global if that simplifies implementation
- keep room meaning local through room tags, adjacency bonuses, and presentation

This lets the simulation stay sane while preserving the spatial fantasy.

Examples:

- greenhouse room boosts herb output
- brew room boosts cauldron quality or speed
- fulfillment room boosts order completion or shelf value

## How Additive Scenes Should Communicate

Different additive scenes should not depend on fragile direct references to each other.

Avoid patterns like:

- HUD scene finding machine objects in a room scene
- room logic calling UI objects directly across scenes
- audio logic polling room objects by scene name

Use this rule instead:

- `Bootstrap` owns persistent services and shared runtime state
- gameplay room scenes publish state changes and gameplay events
- `Shell_UI` reads shared state or subscribes to events
- `AudioManager` reacts to events and room context changes

Treat scenes as presentation containers, not as the source of truth.

## Recommended Communication Pattern

Use a combination of:

- shared runtime state
- event channels for cross-scene notifications
- scene lifecycle hooks for scene load and activation

### Shared Runtime State

Shared runtime state is the safest way to keep the UI, room scenes, and audio aligned.

The first plain C# runtime-state layer now exists under `Assets/Scripts/Runtime/State/`.
Future scene services should read and mutate that kind of state, then bridge only meaningful scene-facing notifications through event channels.

Examples:

- pantry inventory
- current demand target
- total throughput
- selected room
- bottleneck summary
- contract progress

The key rule is:

- room scenes write to runtime state
- UI scenes read from runtime state
- runtime C# events notify local state listeners
- ScriptableObject event channels are reserved for cross-scene notifications

That means `Shell_UI` should not care whether a production update came from `MainPantry` or `GreenhouseWing`.

It only cares that the pantry state changed.

### Event Channels

Use event channels for important cross-scene events.

Examples:

- `RoomActivated`
- `ContractCompleted`
- `BottleneckChanged`
- `MachineSelected`
- `CompactModeChanged`

For Unity, ScriptableObject-based event channels are a good fit for this kind of decoupled architecture.

### Scene Lifecycle Hooks

Use Unity scene lifecycle callbacks to coordinate additive loading.

Examples:

- detect when an additive room scene is loaded
- register room presenters or room runtime objects
- trigger room activation logic
- swap music or ambience context when the primary room changes

## Example Flow: Main Pantry Updates the UI

Recommended flow:

```text
MortarGolem completes cycle
-> updates PantryState inventory and production metrics
-> PantryState raises InventoryChanged / BottleneckChanged
-> Shell_UI HUD listens and refreshes summary displays
-> Compact mode also refreshes from the same shared state
```

This keeps the UI independent from any specific room scene hierarchy.

## Example Flow: Audio Reacts to Gameplay

Audio should be split into:

- global audio control in `Bootstrap`
- local room audio emitters inside gameplay scenes

Recommended division of responsibility:

- local room objects handle world SFX and local loops
- `AudioManager` handles music, ambience state, mixer snapshots, and high-level stingers

Example flow:

```text
SceneFlowService activates MainPantry
-> raises RoomActivated(MainPantry)
-> AudioManager transitions to pantry ambience snapshot

ContractState completes a featured contract
-> raises ContractCompleted
-> AudioManager plays success stinger

Machine in MainPantry stalls
-> room-local presenter plays a sputter or failure SFX
-> optional global alert event updates UI and alert sound
```

The audio manager should usually respond to events and room context, not inspect random room objects directly.

## Suggested Persistent Services in `Bootstrap`

Recommended persistent services:

- `GameSession`
- `PantryState`
- `ContractState`
- `RoomStateRegistry`
- `SceneFlowService`
- `AudioManager`

These systems should survive normal room scene changes.

## Suggested Responsibilities by Scene

### `Bootstrap`

- create and own persistent services
- load core persistent scenes
- coordinate session startup

### `Shell_UI`

- subscribe to runtime models and event channels
- render HUD, compact mode, overlays, and notifications
- show whole-pantry summaries

### `MainPantry` and Other Room Scenes

- contain room visuals and room-local presenters
- drive machine interactions and local effects
- publish gameplay outcomes into shared state
- publish meaningful room events when needed

## Anti-Patterns to Avoid

Avoid these unless there is a very narrow and justified reason:

- `FindObjectOfType` as cross-scene architecture
- storing direct references from persistent UI to transient room objects
- putting core game state inside room-scene MonoBehaviours
- hardcoding scene-name conditionals all over the audio system
- using `DontDestroyOnLoad` for large chunks of game logic because service ownership was never defined

If every scene is reaching into every other scene, the architecture is already rotting.

## Recommended Progression by Release Stage

### Prototype and Demo

Load:

- `Bootstrap`
- `Shell_UI`
- `MainPantry`

Design target:

- exactly one strong pantry room

### Early Access Core

Load:

- `Bootstrap`
- `Shell_UI`
- `MainPantry`
- one or two additive specialty room scenes as needed

Design target:

- two to three connected rooms total

### 1.0 Target

Load:

- `Bootstrap`
- `Shell_UI`
- `MainPantry`
- two strongly themed specialty scenes
- optional fourth room only if it clearly improves strategy and fantasy

Design target:

- three strongly themed rooms as the normal end-state

## Navigation Rules

Room navigation should feel local and quick.

Use:

- one-click room switching
- tabs or a compact pantry map
- immediate state preservation
- no fake loading interruption during ordinary room changes

Avoid:

- deep free-camera wandering across a huge mansion floor
- scrolling through a giant uninterrupted mega-room

## UI Guidance

A persistent UI shell scene is recommended.

However:

- keep global UI focused on persistent player-facing systems
- keep room-specific interactions close to the room or spawned from room-aware presenters
- do not build the whole game as one immortal canvas monster

If the UI scene becomes a dumping ground, it stops being architecture and becomes compost.

## Suggested Folder and Scene Naming

Suggested structure:

```text
Assets/Scenes/
  Core/
    Bootstrap.unity
    Shell_UI.unity
  Gameplay/
    MainPantry.unity
    GreenhouseWing.unity
    PrepLab.unity
    FulfillmentNook.unity
  Test/
    Sandbox.unity
```

## Unity Implementation Notes

Recommended Unity approach:

- load persistent scenes additively
- set the active scene deliberately when needed
- keep persistent systems in loaded core scenes instead of overusing `DontDestroyOnLoad`
- reserve `DontDestroyOnLoad` for a very small number of truly app-level objects if needed

## Unity Research References

Official Unity references used for this recommendation:

- [Use ScriptableObjects as Event Channels in Your Code](https://unity.com/how-to/scriptableobjects-event-channels-game-code)
- [SceneManager.sceneLoaded](https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager-sceneLoaded.html)
- [LoadSceneMode.Additive](https://docs.unity3d.com/ScriptReference/SceneManagement.LoadSceneMode.Additive.html)
- [SceneManager.SetActiveScene](https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.SetActiveScene.html)
- [Work with multiple scenes in Unity](https://docs.unity3d.com/Manual/MultiSceneEditing.html)
- [Audio Mixer](https://docs.unity3d.com/Manual/AudioMixer.html)

Notes from the references:

- Unity supports additive loading as the normal way to keep multiple scenes loaded together.
- Unity exposes scene lifecycle events so persistent systems can react when additive scenes load.
- Unity's ScriptableObject event-channel pattern is a strong fit when UI, audio, and gameplay need to communicate without hard references.
- Unity's Audio Mixer and snapshots are a better fit for room context changes and global mixing than scattering ad hoc audio rules across room objects.

## Final Recommendation

Follow this rule:

- one persistent bootstrap scene
- one persistent shell/UI scene
- one main pantry gameplay scene first
- add a small number of specialty room scenes later
- do not split every biome into its own scene unless it becomes a real managed space
- do not turn the entire game into one endlessly expanding mega-scene

The right target for Witch's Pantry is a connected, readable pantry made of a few meaningful spaces, not a monolith and not a pile of tabs.
