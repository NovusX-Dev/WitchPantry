# Witch's Pantry Automation 12-Week Production Roadmap

- Goal: Ship a playable incremental automation game in 12 weeks
- Engine: Unity 6
- Target: PC (Steam)
- Genre: Incremental / Automation / Cozy Fantasy

## High-Level Milestones

- Weeks 1 to 3: Core Simulation
- Weeks 4 to 6: Gameplay Systems
- Weeks 7 to 9: Content and Economy
- Weeks 10 to 11: Polish and UX
- Week 12: Steam release prep

## Week 1: Project Foundation

Goals:

- Create clean Unity project architecture
- Implement core data models

Tasks:

- Set up Unity project
- Set up Git repository
- Create folder structure

Systems:

- `ResourceType` enum
- `ResourceStack` struct
- `MachineDefinition` ScriptableObjects

Deliverable: basic project structure ready for development.

## Week 2: Production Graph System

Goal: implement the automation engine.

Tasks:

- `ProductionGraph` manager
- `ProductionNode` base class
- Resource buffers
- Tick simulation system

Features:

- Nodes process resources
- Resources move between nodes

Deliverable: simple automation simulation running.

## Week 3: Machine Framework

Goal: create a reusable machine system.

Tasks:

- `MachineInstance` class
- Producer nodes
- Converter nodes
- Basic machine UI

Machines:

- Herb Garden
- Mushroom Cave
- Mortar Golem

Deliverable: machines producing resources automatically.

## Week 4: Brewing System

Goal: implement potion crafting.

Tasks:

- Cauldron machine
- Recipe definitions
- Resource consumption

Recipes:

- Healing Potion
- Mana Potion

Deliverable: complete ingredient-to-potion pipeline.

## Week 5: Bottling and Storage

Goal: finalize the production pipeline.

Tasks:

- Bottling Sprite machine
- Potion storage system
- Inventory UI

Features:

- Potions stored
- Production chains visible

Deliverable: full production loop working.

## Week 6: Economy System

Goal: add selling and economy.

Tasks:

- Market system
- Currency resource
- Auto-selling system

Features:

- Potions sell automatically
- Gold generated per tick

Deliverable: first playable idle loop.

## Week 7: Upgrades

Goal: introduce player progression.

Tasks:

- Machine upgrades
- Production speed upgrades
- Efficiency upgrades

Examples:

- Faster cauldrons
- Larger buffers

Deliverable: upgrade progression system.

## Week 8: Automation Expansion

Goal: add more machines.

New machines:

- Crystal Mine
- Crystal Grinder
- Arcane Cauldron
- Labeling Imp

Tasks:

- Machine unlock tree
- UI for unlocking machines

Deliverable: multiple production chains.

## Week 9: Prestige System

Goal: introduce long-term progression.

Tasks:

- Prestige reset mechanic
- Prestige currency
- Permanent bonuses

Features:

- Reset pantry
- Gain magic essence

Deliverable: endgame loop added.

## Week 10: UI Polish

Goal: improve player experience.

Tasks:

- Resource panel
- Production graph UI
- Machine panel redesign

Features:

- Clear production stats
- Idle progress display

Deliverable: clean and readable interface.

## Week 11: Balance and Content

Goal: balance the economy.

Tasks:

- Tune resource production
- Adjust machine costs
- Add more potion types

Content:

- 8 to 12 machines total
- 10 or more potions

Deliverable: balanced progression curve.

## Week 12: Steam Release Preparation

Goal: prepare the game for launch.

Tasks:

- Save system
- Offline progress
- Steam build
- Trailer and screenshots

Polish:

- Bug fixes
- Performance optimization

Deliverable: playable Steam-ready build.

## Final Scope

- Machines: about 12 to 18
- Potions: about 10
- Core systems: 6
- Development time: 12 weeks

## Success Criteria

The game should include:

- Fully automated production chains
- Clear progression loop
- Idle and offline simulation
- Upgrade and prestige systems
- Polished UI
- Steam-ready build

## End

End of roadmap.
