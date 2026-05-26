# Witch's Pantry UI and UX Design

- Engine: Unity 6; current project version: `Witch-Pantry/ProjectSettings/ProjectVersion.txt`
- Platform: Steam (Windows)
- Interaction Modes: Full game view and compact desktop-idler view

## 1. UI Goals

The UI must let the player:

- understand production at a glance
- solve bottlenecks quickly
- enjoy watching the pantry run
- keep the game open in a compact mode without losing key control

## 2. Core UX Principles

- surface the most important numbers constantly
- make bottlenecks obvious
- minimize drill-down clicks
- preserve a cozy fantasy presentation
- remain readable in screenshots, streams, and compact window sizes

## 3. Full Game View

Purpose:

- main planning and optimization mode
- rearranging pantry layout
- inspecting machines and demand

Recommended layout:

```text
--------------------------------------------------------------
| Gold | Demand | Potions/min | Bottleneck | Compact Toggle  |
--------------------------------------------------------------
| Pantry View                | Right Rail                  |
|                            | - selected machine          |
|                            | - upgrades                  |
|                            | - adjacency bonuses         |
--------------------------------------------------------------
| Bottom Rail: demand | inventory | events | prestige      |
--------------------------------------------------------------
```

## 4. Compact Desktop-Idler View

Purpose:

- side-screen monitoring
- quick intervention
- passive progress check-ins

Must include:

- gold
- current demand highlight
- potion throughput
- current bottleneck or alert
- top 1 to 3 quick actions

Must avoid:

- deep panel nesting
- dense inventory tables
- tiny unreadable text

## 5. At-a-Glance Rules

Within 5 seconds the player should understand:

- whether the pantry is healthy
- what the main bottleneck is
- what the next meaningful action probably is

UI should always expose:

- current demand target
- current production rate
- blocked or starved machine state
- available upgrade or layout opportunity

## 6. Pantry View Requirements

The pantry view is the primary visual anchor.

It must show:

- distinct machine silhouettes
- work-state animation
- local adjacency cues
- bottleneck states
- fulfilled order moments

The pantry view should be useful even when the player never opens a detail panel.

## 7. Bottleneck Communication

Every stall must have a simple readable cause:

- missing input
- full output
- weak adjacency
- unmet demand mix
- event-related slowdown

Each cause should have:

- icon
- tooltip
- suggested action

## 8. Low-Click Management

The game should support quick management from the main view.

Use:

- top-level quick actions
- contextual buttons on selected machines
- simple drag or swap layout actions
- buy and upgrade flows with minimal modal interruption

Avoid:

- repeated submenu digging
- forcing the player into spreadsheet panels for basic decisions

## 9. Demand and Contracts UI

Demand UI should prioritize clarity over data volume.

Show:

- who wants what
- how urgent it is
- what reward is offered
- whether the player is on pace

Visual hierarchy:

- featured demand card
- active faction or visitor requests
- optional secondary tasks

## 10. Event UI

Event presentation should respect cozy tone.

Use:

- warm banner notifications
- clear benefits and costs
- response buttons when intervention is possible

Do not use:

- harsh red panic spam
- unexplained penalties
- interruption-heavy popups

## 11. Number Presentation

Default:

- abbreviations for common large values
- readable throughput labels such as `potions/min`
- plain-language summaries where useful

Late-game fallback:

- scientific notation only after readable suffixes stop helping

## 12. Accessibility and Readability

Required options:

- UI scale
- reduced motion where needed
- colorblind-safe bottleneck states
- adjustable number formatting

Readable UI is a product feature, not cleanup work.

## 13. Screenshot and Store Readiness

Key store images should be capturable directly from in-game states.

That means:

- readable pantry silhouette
- obvious machine motion
- visible demand and reward loop
- no cluttered debug-looking UI

## 14. Demo UX Priorities

For the first public build:

- strong pantry view
- one strong full game layout
- useful compact mode
- obvious bottleneck feedback
- readable demand cards

## 15. Expansion UX Priorities

Later builds can add:

- advanced analytics panels
- more layout overlays
- faction reputation views
- richer compact mode automation controls
