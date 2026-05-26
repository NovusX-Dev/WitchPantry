# Witch's Pantry Production Blueprint

- Engine: Unity 6; current project version: `Witch-Pantry/ProjectSettings/ProjectVersion.txt`
- Genre: Cozy Automation / Incremental
- Presentation Model: Spatial pantry layout with shared inventory simulation support

## 1. Production Philosophy

Production must satisfy three goals at once:

- feel good to optimize
- look good in motion
- read clearly in screenshots and live play

The pantry is not just a list of generators. It is a compact magical room where machine placement, work zones, and traffic readability communicate progress.

## 2. Core Production Fiction

The simulation may use shared pantry inventory and virtual links, but the player-facing fiction is spatial:

- ingredient sources occupy pantry space
- helpers and machines visually operate within local zones
- nearby groupings imply efficiency
- cluttered layouts become harder to read and manage

The game should never expose pure spreadsheet logic without visual grounding.

## 3. Pantry Space Model

The pantry is organized as a grid or slot-based room layout.

Each machine has:

- footprint size
- orientation rules if needed
- visual output direction
- adjacency tags
- room role

Example machine footprint classes:

- small helper
- standard workstation
- large focal machine
- wall or shelf utility

## 4. Core Production Flow

```text
ingredient source
  ->
processing station
  ->
cauldron or brewing station
  ->
bottling or finishing station
  ->
shelf, dispatch, or customer fulfillment
```

This flow should be visible in the scene even when the underlying logic uses shared buffers.

## 5. Machine Categories

### Producers

- Herb Garden
- Well
- Mushroom Cave

### Processors

- Mortar Golem
- Crystal Grinder
- Essence Still

### Brewers

- Enchanted Cauldron
- advanced specialty cauldrons later

### Fulfillment and Utility

- Bottling Sprite
- Enchanted Shelf
- Dispatch nook or customer counter

## 6. Spatial Rules

Spatial design should provide meaningful choices without becoming a pathfinding tax.

Recommended rules:

- limit footprint shapes to a small readable set
- favor adjacency and zoning bonuses over explicit item-conveyor simulation
- keep rearrangement friction low
- preserve a tidy cozy-room look even in optimized builds

Examples of spatial bonuses:

- Herb Garden next to Well: growth speed bonus
- Mortar Golem next to ingredient shelf: reduced cycle downtime
- Bottling Sprite near cauldron cluster: throughput bonus
- customer counter near enchanted shelf: faster order handling

## 7. Adjacency Design

Adjacency bonuses should:

- reward intuitive organization
- reinforce fantasy pairings
- be simple enough to read from iconography alone

Avoid:

- giant optimization matrices
- hidden synergies
- mandatory pixel-perfect layouts

Good adjacency reads as:

`these things belong together`

not:

`I need a doctoral thesis to place a mushroom.`

## 8. Readability Requirements

Every production scene must support:

- at-a-glance understanding of active lines
- visible identification of stalled machines
- clear focal points for high-value activity
- clean screenshots with recognizable silhouettes

Visual rules:

- each major machine needs a distinct silhouette
- ingredients need color and shape distinction
- bottlenecks need simple, obvious feedback states
- busy scenes should still have negative space and lane clarity

## 9. Watchability Requirements

The pantry should be pleasant to watch while idle.

Use:

- short repeating machine loops
- charming helper animations
- obvious brew, bottle, and shelf activity
- subtle celebratory motion on completed orders

Avoid:

- static list-only production
- overly tiny unreadable motion
- excessive VFX that hide state

## 10. Compact-Room Optimization

Optimization comes from:

- machine grouping
- support adjacency
- balancing throughput across small spaces
- deciding which room tiles hold premium machines

The player fantasy is:

`I turned a cluttered witch kitchen into a beautiful little productivity monster.`

## 11. Example Early Pantry Layout

```text
[Herb Garden] [Well]        [Ingredient Shelf]
[Mortar Golem] [Cauldron]   [Bottling Sprite]
[Customer Counter] [Enchanted Shelf] [Upgrade Corner]
```

Goals:

- production chain visible left-to-right
- customer fulfillment readable on the bottom row
- support objects grouped without clutter

## 12. Progression Through Space

Layout progression should be visible.

Examples:

- unlock a new pantry wall section
- earn a special utility tile
- place a faction request board
- add rare machine variants with larger footprints

Space progression is a reward loop, not only a storage problem.

## 13. Relationship to Shared Inventory

If the implementation uses a global pantry inventory:

- keep resource state global for simulation simplicity
- expose local zone bonuses through adjacency and room tags
- use visuals to imply transfer and workflow
- show bottlenecks through machine state rather than invisible math

This keeps implementation sane while preserving the layout fantasy.

## 14. Demo Blueprint Priorities

The demo should prove:

- one compact pantry room
- a readable machine cluster
- 2 to 3 meaningful adjacency bonuses
- one obvious bottleneck type
- one obvious layout improvement moment

## 15. Expansion Blueprint Priorities

Later releases can add:

- additional room types
- specialty wings
- advanced adjacency sets
- prestige-only decor and utility pieces
- more specialized customer fulfillment stations
