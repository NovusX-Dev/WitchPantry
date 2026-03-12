# Spatial Pantry System

## Purpose

Define how space matters without turning the game into conveyor-belt tax accounting.

## Core Model

- pantry uses a compact spatial grid or slot layout
- machines have small, readable footprint classes
- adjacency and zone bonuses matter more than explicit pathing
- simulation may still use shared inventory underneath

## Machine Footprint Model

Suggested footprint classes:

- helper: 1x1
- station: 2x1
- focal machine: 2x2
- shelf or wall utility: edge slot

## Placement Rules

- players can move machines with low friction
- premium tiles should exist for high-value placements
- each room should have obvious anchor spots and support spots
- layout experimentation should feel inviting, not expensive

## Adjacency Rules

- bonus pairs should be thematic and readable
- each machine should expose at most a small number of adjacency tags
- adjacency should boost throughput, consistency, or quality
- adjacency should never require hidden formulas to understand

## Readability Constraints

- each machine family must have a distinct silhouette
- ingredient colors must remain readable at camera-default zoom
- machine states need obvious idle, active, and stalled variants

## Shared Inventory Interaction

- keep inventory simulation global if that reduces implementation cost
- use spatial bonuses and room tags to preserve layout meaning
- show local activity through helpers, FX, and work-state indicators

## Layout Strategy

The game should use:

- one main pantry screen in the early game
- clearly separated production clusters inside that screen
- a small number of connected pantry rooms later

The game should not use:

- one isolated screen per production flow
- one endlessly expanding single room that becomes unreadable soup

Recommended model:

- Demo and early progression: one room
- Early Access core: two to three connected rooms
- 1.0 and beyond: three to four specialized spaces maximum for the main pantry set

## Core Rule

A single production chain should usually be readable in one view.

The whole economy does not need to fit in one room forever, but the player should always feel they are managing one connected magical workplace, not teleporting between unrelated menus.

## Room Progression

### Stage 1: Starter Pantry

Use one compact room for:

- starter ingredients
- one prep station
- one brewing station
- one bottling or shelf station
- one customer-facing station

Purpose:

- teach the core loop
- teach adjacency
- make the room memorable and marketable

### Stage 2: Expanded Main Pantry

Keep one screen, but divide it into obvious work zones:

- ingredient corner
- prep zone
- brew line
- shelf and dispatch zone

Purpose:

- increase optimization depth without adding navigation burden
- preserve readability while machine variety increases

### Stage 3: First Secondary Room

Unlock the second room when the first room is running out of meaningful placement choices rather than simple tile count.

Recommended unlock timing:

- after the player has built a stable core chain
- after they have used adjacency bonuses successfully
- around the point where new ingredient branches or customer demand would create layout tension in the starter room

The first secondary room should be:

- visually adjacent to the main pantry
- clearly themed
- small enough to understand quickly

Recommended first options:

- greenhouse or ingredient room
- specialized prep room

### Stage 4: Specialized Wings

Later rooms should represent specialized fantasy roles rather than generic overflow.

Examples:

- greenhouse wing
- main brew kitchen
- rare ingredient lab
- order fulfillment nook

Hard guideline:

- avoid more than four actively managed rooms in the primary pantry set

## Room Roles

Each room should have a primary identity.

Suggested roles:

- Source room: farming, gathering, raw ingredient boosts
- Prep room: grinding, distilling, ingredient conversion
- Brew room: cauldrons, quality bonuses, high-visibility production
- Fulfillment room: bottling, shelving, customer counter, demand bonuses

This gives expansion meaning beyond:

`I bought another rectangle.`

## Camera and View Rules

### Main Gameplay Camera

Recommended default:

- high three-quarter view or readable isometric view
- camera angle stable enough that machine silhouettes remain recognizable
- zoom level fixed or tightly bounded in normal play

The player should not need to wrestle the camera to understand the room.

### Room Navigation

Use one of these approaches:

- horizontally connected room panels with quick tab or button switching
- a compact pantry map with one-click room changes

Do not use:

- deep scrolling across a giant mansion floor
- free camera wandering as the primary navigation layer

### Transition Rules

Room transitions should feel local and quick:

- one click from main pantry to wing
- immediate state preservation
- no loading-style interruption

## Screen Composition Rules

Each room view should support:

- one dominant focal machine or cluster
- one readable left-to-right or top-to-bottom workflow
- enough negative space to identify stations quickly
- space for overlays, status icons, or compact hints

If a room cannot be understood in a few seconds, it needs fewer active machine families or cleaner zoning.

## Machine Footprint Rules

Use a small footprint vocabulary:

- 1x1 helper
- 2x1 station
- 2x2 focal machine
- edge-slot wall or shelf utility

Rules:

- the majority of machines should fit 1x1 or 2x1 footprints
- only a few machines should be 2x2 focal anchors
- utility pieces should help shape readable lanes rather than fill random gaps

## Placement Rules

Each room should include:

- anchor tiles for major machines
- support tiles for helpers and shelves
- at least one premium placement area for adjacency play

Placement should reward:

- logical grouping
- compact efficiency
- clean work zones

Placement should not reward:

- scattered checkerboard nonsense
- unreadable hyper-optimization blobs

## Second Room Unlock Trigger

Unlock the second room when all of these are true:

- the first room already demonstrates the full starter production chain
- the player has interacted with at least 2 meaningful adjacency decisions
- a new branch of content would force ugly crowding or kill readability

Do not unlock a second room just because the player filled empty tiles.

The unlock should feel like:

`my pantry has earned a new workspace`

not:

`the spreadsheet needs another tab`

## Multi-Room Economy Rules

Shared inventory can remain global across rooms.

To preserve spatial meaning:

- bonuses should reference the current room or local tags
- room identity should shape what belongs there
- some demand or utility bonuses can key off room role

Example:

- greenhouse boosts herb output
- brew room boosts cauldron speed or quality
- fulfillment nook boosts order completion or shelf value

## Contracts and Demand Across Rooms

Demand should read at two levels:

- whole-pantry summary
- room-level contribution

Recommended behavior:

- featured demand stays visible globally
- each room can show a small contribution hint
- fulfillment-facing rooms surface the clearest demand feedback

The player should always know:

- what the pantry is trying to fulfill
- which room is helping most
- which room is currently the problem

## Compact Mode Across Rooms

Compact mode should summarize the whole pantry, not force room micromanagement.

Show:

- current featured demand
- total production summary
- worst bottleneck
- room needing attention
- one to three quick actions

Recommended compact room behavior:

- show the current primary room in the viewport
- rotate or switch the room preview only when useful
- call out room names only when needed for action

Examples:

- `Brew Room blocked: Bottles low`
- `Greenhouse full: harvest bonus wasted`

Compact mode should answer:

- is the pantry healthy
- where is the main issue
- what is the next best quick action

## Demo Layout Spec

For the demo, use exactly one main pantry room.

Recommended cluster layout:

- top-left: ingredient source nook
- left or center-left: prep station
- center: cauldron anchor
- center-right: bottling or finishing
- bottom-right: shelf and customer fulfillment

Demo layout goals:

- one full chain readable in one shot
- one visible bottleneck state
- one obvious rearrangement or upgrade opportunity

## Early Access Layout Spec

For Early Access core, expand to:

- main pantry
- one secondary specialty room
- optional third room only if a second branch truly needs it

The second room should deepen strategy, not just absorb clutter.

## 1.0 Layout Spec

For 1.0, aim for:

- three strongly themed rooms as the normal end-state
- optional fourth high-tier room only if it adds clear fantasy and gameplay value

Beyond that, readability and identity will usually degrade faster than depth improves.
