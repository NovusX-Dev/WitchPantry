# Unlocks and Upgrades System

- Project: Witch's Pantry
- Engine: Unity 6000.3
- Purpose: Extract the implied progression model from the current docs and formalize a clean system for unlocks, upgrades, rooms, prestige, and research.

## Summary

The current docs clearly support:

- machine upgrades
- room and layout improvements
- machine and ingredient unlocks
- biome progression
- prestige progression

The current docs do not yet define:

- one unified unlock grammar
- a clean distinction between unlocks and upgrades
- whether research exists as a real system or only as a loose economy term

This document formalizes that structure.

Recommended final model:

- unlocks open new capabilities
- upgrades improve existing capabilities
- room growth creates new spatial decision space
- prestige provides long-term meta progression
- research should exist only as a small, thematic, medium-term unlock layer, not as a giant second upgrade spreadsheet

## Extracted Implied Model From Current Docs

Across the current design docs, the implied progression model looks like this:

### 1. Core Progression Drivers

Progression currently comes from:

- gold earnings
- customer demand and faction progress
- machine purchases
- machine upgrades
- room and layout expansion
- ingredient and biome unlocks
- prestige

This is spread across:

- `Docs/GDD.md`
- `Docs/Witchs-Pantry-Content-Plan.md`
- `Docs/Spatial-Pantry-System.md`
- `Docs/Ingredients_Biomes_Design.md`
- `Docs/Customer-Demand-and-Contracts.md`
- `Docs/economy-balance.md`
- `Docs/Witchs-Pantry-Production-Blueprint.md`

### 2. What Is Already Clear

The current docs are already clear that:

- the player buys machines, upgrades, and room improvements
- new ingredients and production branches unlock over time
- biome progression expands content and fantasy
- the second room unlock is tied to layout pressure and progression readiness
- prestige is a real layer with Arcane Essence as the meta currency
- compact mode must stay useful without deep menu dependency

### 3. What Is Still Fragmented

The current docs do not yet specify:

- exactly how a machine unlock happens
- exactly how a new ingredient branch unlock happens
- whether unlocks are driven by gold, contracts, room milestones, prestige, or another gate
- whether upgrades are per-machine, per-machine family, room-wide, or pantry-wide
- whether research is a real system

## Design Rule: Distinguish Unlocks From Upgrades

The project should use this rule everywhere:

### Unlock

An unlock gives the player access to something fundamentally new.

Examples:

- a new machine family
- a new ingredient branch
- a new biome source
- a new room
- a new customer faction
- a new prestige perk tier

Unlocks expand the decision space.

### Upgrade

An upgrade improves something the player already has.

Examples:

- faster Herb Garden cycle
- more efficient Mortar Golem
- higher cauldron quality
- better shelf value bonus
- more effective room adjacency bonus

Upgrades improve an existing decision.

If a feature does not change the player's decision space, it is probably an upgrade, not an unlock.

## Recommended Progression Layers

Use five layers only.

### Layer 1: Machine Purchase Unlocks

Purpose:

- introduce new machine functions into the pantry

Examples:

- Herb Garden
- Mortar Golem
- Enchanted Cauldron
- Bottling Sprite
- Crystal Grinder
- Essence Still

Recommended unlock sources:

- gold cost
- progression milestone
- contract or faction milestone when appropriate
- biome availability when relevant

Rule:

- the player should understand why a machine is not available yet

### Layer 2: Ingredient and Recipe Unlocks

Purpose:

- expand what the player can produce and fulfill

Examples:

- Mushroom branch
- Crystal branch
- Shadow branch
- late rare ingredients

Recommended unlock sources:

- biome unlocks
- machine chain completion
- featured demand or faction milestones
- prestige gates for late content

Rule:

- new ingredients should create a new layout or demand decision, not just another row in inventory

### Layer 3: Upgrades

Purpose:

- improve production efficiency and reinforce optimization

Recommended upgrade categories:

- speed
- efficiency
- capacity
- quality or value

Recommended ownership levels:

- machine instance or machine family upgrades for most throughput upgrades
- room upgrades for spatial identity
- pantry-wide convenience upgrades in limited quantity

Rule:

- avoid too many overlapping upgrade surfaces

### Layer 4: Room and Layout Expansion

Purpose:

- keep spatial play central to the product

Examples:

- new wall section
- premium tile
- support tile
- second room unlock
- specialty room unlock

Recommended unlock sources:

- gold
- room milestone
- demand progression
- adjacency mastery milestone

Rule:

- room expansion should arrive when the current room has meaningful spatial tension, not when the spreadsheet wants another tab

### Layer 5: Prestige

Purpose:

- provide long-term replayable progression without invalidating the pantry game

Current implied prestige resource:

- `Arcane Essence`

Recommended prestige rewards:

- permanent light multipliers
- new upgrade caps
- new contract/faction opportunities
- rare room utility unlocks
- late research branches if research exists

Rule:

- prestige should accelerate mastery, not bypass the pantry fantasy

## Recommended Unlock Grammar For Witch Pantry

Use this practical progression grammar:

### New Machines

Machines should usually unlock through one of these:

- direct gold purchase after prerequisite milestone
- contract milestone
- biome availability
- prestige tier for late special machines

Suggested machine unlock flow:

```text
meet prerequisite
  ->
machine appears in shop or unlock panel
  ->
pay gold or reward cost
  ->
place machine in pantry
```

### New Ingredients

Ingredients should usually unlock through:

- source machine unlock
- biome unlock
- room unlock when the source belongs in a specialty room

Suggested ingredient rule:

- ingredients do not need separate unlock buttons if the machine or biome already implies them

### New Recipes and Potions

Recipes should usually unlock through:

- owning the required machine family
- unlocking the relevant ingredient branch
- faction or demand milestone for specialty brews

Suggested rule:

- a recipe becomes visible slightly before or at the moment it becomes realistically chaseable

### New Rooms

Rooms should unlock through a combination of:

- pantry crowding pressure
- demonstrated adjacency use
- demand progression
- gold or room improvement cost

This matches the existing spatial docs.

### New Biomes

Biomes should unlock through:

- progression milestones
- demand/faction progress
- prestige for some later biomes

They should not unlock just because a timer expired or the player bought a generic license.

## Recommended Upgrade Model

Witch Pantry should use three upgrade bands.

### Band A: Machine Upgrades

Best for:

- speed
- efficiency
- output amount
- quality consistency

Examples:

- Herb Garden: faster growth
- Mortar Golem: lower downtime
- Cauldron: better potion quality
- Bottling Sprite: faster throughput

Recommended UI behavior:

- shown from machine selection panel
- one to three upgrade lanes per machine family maximum

### Band B: Room Upgrades

Best for:

- room identity
- adjacency amplification
- utility slots
- room-specific bonuses

Examples:

- Greenhouse: source output bonus
- Prep Lab: processing efficiency bonus
- Brew Room: cauldron quality bonus
- Fulfillment Nook: order payout or dispatch bonus

Recommended UI behavior:

- shown from room overview
- low frequency, high meaning

### Band C: Pantry Convenience Upgrades

Best for:

- low-click quality of life
- compact mode support
- whole-pantry friction reduction

Examples:

- better bottleneck warnings
- extra queue convenience
- auto-claim helper
- better compact mode quick actions

Recommended UI behavior:

- limited count
- avoid turning convenience into a giant side tree

## Should Research Exist?

Yes, but only in a limited, thematic form.

It should not be a mandatory giant science-tree system like a heavy factory game.

## Genre Check: Does An Incremental Game Need Research?

No.

Incremental games often use:

- unlocks
- upgrades
- prestige
- medium-term milestone systems

Research is one possible structure for medium-term progression, but it is not a genre requirement.

Useful comparison:

- `Factorio` uses research as a central technology gate because production complexity and science automation are core to the fantasy.
- `Cookie Clicker` is built primarily around purchases, upgrades, and prestige rather than a standalone research layer.
- `Rusty's Retirement` emphasizes unlocks, automation helpers, and upgrades in a compact desktop-idler format without presenting research as the core progression loop.

For Witch Pantry, this matters because the fantasy is closer to:

- cozy visible optimization
- room growth
- demand-driven branching
- desktop glanceability

than to:

- giant technology-tree management
- science-pack logistics
- heavy menu-driven progression

That means research can help, but only if it supports the pantry fantasy instead of replacing it.

## Why Research Should Not Be the Main Progression Layer

The current product pillars favor:

- spatial pantry play
- visible machine flow
- readability
- demand-driven goals
- second-monitor usability

A large research tree would create risks:

- too much menu time
- weaker room and layout focus
- more hidden optimization instead of visible optimization
- compact-mode clutter
- overlap with upgrades and prestige

For this product, a giant research system would pull the game toward spreadsheet play faster than it adds fantasy.

## Why A Small Research Layer Can Help

A limited research layer can still add value if it does three things:

- packages medium-term goals cleanly
- gives contracts and rare ingredients another use
- unlocks qualitatively new options that normal upgrades should not handle

That means research is useful only if it answers:

`what new capability am I studying next`

not:

`which of these 40 passive bonuses should I click because numbers go up`

## Research Recommendation

Use research as a small unlock layer between normal upgrades and prestige.

Research should:

- unlock new machine families
- unlock new recipe categories
- unlock room utility pieces
- unlock special contract capabilities
- unlock compact-mode and automation conveniences carefully

Research should not mainly do:

- generic +5% production
- generic +10% gold
- duplicate the machine upgrade layer
- duplicate prestige multipliers

## Recommended Research Shape

Call it something thematic, not generic science.

Recommended naming options:

- Arcane Study
- Pantry Studies
- Witchcraft Notes
- Guild Research

Best fit:

- `Arcane Study`

### Research Currency

Recommended sources:

- rare ingredients
- faction reputation milestones
- special contract rewards
- occasional prestige unlocks

Avoid using normal gold as the only research currency, or research just becomes another shop tab wearing glasses.

### Research Scope

Keep research to a small number of visible tracks.

Recommended tracks:

- Cultivation
- Alchemy
- Pantry Craft
- Trade and Contracts

Each track should unlock new capabilities, not mostly raw multipliers.

### Example Research Unlocks

Cultivation:

- unlock Mushroom handling efficiency rules
- unlock Crystal branch access
- unlock greenhouse utility piece

Alchemy:

- unlock Essence Still
- unlock specialty cauldron variants
- unlock event-responsive potion recipes

Pantry Craft:

- unlock premium support shelf
- unlock room utility tile
- unlock improved machine placement affordances

Trade and Contracts:

- unlock better faction requests
- unlock one extra secondary request slot
- unlock higher-value visitor opportunities

## Recommended Release Scope

### Prototype

Do not build research yet.

Use:

- machine purchases
- machine upgrades
- room pressure
- first contracts
- prestige foundation

### Demo

If research appears at all, use only a teaser or tiny first tier.

Best demo-safe option:

- one small `Arcane Study` panel
- 3 to 5 meaningful unlocks maximum

### Early Access

This is the right time for the first real research layer.

Why:

- more machines
- more contracts
- more room specialization
- more need for medium-term planning

### 1.0

Expand research only if it still serves readability and the pantry fantasy.

If it starts stealing attention from layout play, cut it back.

## External Genre References

Useful external references for this decision:

- `Factorio` research overview: [Factorio Wiki - Research](https://wiki.factorio.com/Research)
- `Factorio` technology structure: [Factorio Wiki - Technologies](https://wiki.factorio.com/Technologies)
- `Rusty's Retirement` product framing: [Steam - Rusty's Retirement](https://store.steampowered.com/app/2666510/Rustys_Retirement/)
- `Cookie Clicker` product framing: [Steam - Cookie Clicker](https://store.steampowered.com/app/1454400/Cookie_Clicker/)

## Final System Recommendation

Witch Pantry should use this progression stack:

1. gold buys machines and visible upgrades
2. contracts and biome progress unlock new branches
3. room growth creates new spatial decisions
4. prestige provides meta progression through Arcane Essence
5. a small thematic research layer can unlock new capabilities, but only after the core pantry loop is already strong

## What Changes

- unlocks become a clearly defined capability layer
- upgrades become a clearly defined improvement layer
- rooms stay central instead of being treated as extra storage
- prestige keeps its meta-progression role
- research is reframed as optional, scoped, and product-supportive rather than assumed by genre habit

## Why It Helps The Product

- preserves the pantry as the main fantasy
- prevents menu sprawl
- keeps compact mode viable
- gives later progression room to grow without becoming a number swamp
- makes implementation and balancing clearer

## Recommended Release Target

- unlock and upgrade grammar: prototype and demo
- room and prestige integration: demo and Early Access
- first real research layer: Early Access
