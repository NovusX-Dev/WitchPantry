---
name: witch-pantry-unity-architecture
description: Plan, review, or refactor Witch Pantry Unity architecture, scene flow, runtime state, ScriptableObject usage, code boundaries, and gameplay-system communication. Use when working on Unity scenes, services, managers, runtime models, event flow, save-facing data, or any technical design choice that could create coupling, singleton sprawl, fragile inspector wiring, or hard-to-test gameplay code.
---

# Witch Pantry Unity Architecture

Use this skill to keep Unity implementation scalable, editor-friendly, and aligned with the existing additive-scene direction.

## Read First

1. Read:
   - `Docs/Unity-Scene-Architecture.md`
   - `Docs/Unity-Scene-Communication-Architecture.md`
   - `Docs/Draft-Unity-Architecture.md`
2. If the task is broad or contentious, also read `references/sources.md`.

## Core Workflow

1. Identify the domain:
   - authored content
   - runtime state
   - scene orchestration
   - UI/shell
   - save/load
   - simulation/service logic
2. State where the source of truth should live:
   - ScriptableObject asset for authored data
   - runtime model/service for mutable play state
   - scene object only for presentation and local interaction
3. Choose the lightest pattern that solves the problem cleanly.
4. Explain the coupling and testing impact before writing code.
5. Prefer architecture that supports prototype speed now without sabotaging demo and Early Access scale.

## Architecture Rules

- Prefer additive scenes with a persistent shell over one giant scene.
- Keep authored content in ScriptableObject definitions with stable IDs.
- Keep mutable gameplay state in runtime services or state models, not in authored assets.
- Prefer event channels, explicit service references, or registries over hidden global lookups.
- Use singletons sparingly and only when the lifecycle is obvious, stable, and worth the trade.
- Separate simulation logic from view/controller MonoBehaviours whenever the logic needs tests, saves, or offline progression.
- Make inspector workflows safe for designers; if authoring can drift, add validation or custom editor support.
- Optimize for debuggability before cleverness.

## Pattern Selection

- Use ScriptableObjects for:
  - content definitions
  - balancing values
  - shared configuration
  - event channels or runtime sets when global access is needed without singleton coupling
- Use plain C# runtime models/services for:
  - inventory state
  - machine runtime status
  - contracts in progress
  - progression state
  - offline simulation inputs and outputs
- Use MonoBehaviours for:
  - scene hooks
  - presentation
  - input capture
  - animation/audio triggers
  - thin adapters into runtime logic
- Use object pooling when objects are spawned frequently enough to create churn.
- Use state machines when entity behavior has clear states and transitions.

## Smells To Call Out

- data duplicated across scene objects and ScriptableObjects
- systems relying on `Find*` calls or scene-global assumptions
- giant manager classes with unrelated responsibilities
- save data coupled to scene instance references
- inspector workflows that require hand-typed IDs when references can be authored safely
- feature code added to make one scene work but which breaks additive-room growth later

## Good Output Shape

End with:

- recommended ownership boundaries
- recommended communication path
- migration or implementation order
- main technical risks
