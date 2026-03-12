# Research System

- Project: Witch's Pantry
- Engine: Unity 6000.3
- Purpose: Define whether research belongs in Witch Pantry and, if so, how to implement it without damaging readability, cozy tone, or compact-mode usability.

## Summary

Research should exist in Witch Pantry, but only as a small thematic unlock system.

It should not be:

- a massive tech tree
- the main progression loop
- a duplicate of machine upgrades
- a duplicate of prestige multipliers

Recommended final framing:

- research is a medium-term capability unlock layer
- machine upgrades remain the main short-term improvement layer
- prestige remains the long-term meta layer

Best thematic name:

- `Arcane Study`

## Why Research Can Help

Research can improve the game if it adds:

- medium-term goals between upgrades and prestige
- a reason to care about rare rewards and factions
- new capability unlocks that feel intentional
- anticipation for upcoming machine and room strategies

It can help answer:

- what is my next strategic study target
- which branch do I want to specialize into
- which room utility should I unlock next

## Why Research Can Hurt

Research becomes harmful if it:

- adds menu sprawl
- creates too many passive invisible bonuses
- steals attention from layout play
- makes compact mode harder to read
- duplicates existing upgrade and prestige systems

For Witch Pantry, the main risk is obvious:

- the player stops shaping a magical pantry
- the player starts babysitting a bonus spreadsheet

That is a bad trade.

## Design Position

Research should be:

- limited
- readable
- thematic
- capability-focused
- introduced after the core pantry loop already works

Research should not be a required early-game pillar.

## Release Timing

### Prototype

Do not include research.

The prototype should prove:

- room readability
- visible machine flow
- machine purchases
- upgrades
- demand and bottlenecks

### Demo

At most, include:

- a teaser
- one tiny starter Arcane Study tier
- 3 to 5 meaningful unlocks maximum

This is optional.

### Early Access

This is the right time for the first real research layer.

Why:

- more room specialization exists
- more machine families exist
- more contracts and factions exist
- there is enough content for capability branching to matter

### 1.0

Expand research carefully and only if it still improves the pantry fantasy.

## Arcane Study Core Role

Arcane Study should sit between:

- normal upgrades
- prestige

It should mostly unlock:

- new machine families
- new room utilities
- new contract capabilities
- new recipe classes
- selective automation conveniences

It should rarely grant:

- flat passive production bonuses
- generic gold bonuses
- raw permanent multipliers that prestige already handles

## Relationship To Other Progression Systems

### Versus Unlocks

Research is one source of unlocks, not the only source.

Use research for:

- studied capabilities
- magical tools
- advanced methods
- specialized branch access

Do not use research for:

- every basic machine
- every ingredient unlock

### Versus Upgrades

Use upgrades to improve what the player already owns.

Use research to unlock what the player can pursue next.

Example:

- upgrade: Herb Garden grows faster
- research: Greenhouse Cultivation unlocks advanced herb support utility

### Versus Prestige

Use prestige for:

- broad meta progression
- repeat-loop acceleration
- long-term permanent advancement

Use research for:

- medium-term strategic branching inside the current run or account state

## Recommended Research Currency

Do not use plain gold as the main research currency.

Recommended research inputs:

- rare ingredients
- faction reputation milestones
- special contract rewards
- occasional prestige-linked catalysts

Good examples:

- Arcane Notes
- Guild Seals
- Rare reagent bundles

Simplest strong option:

- `Arcane Notes`

Where they come from:

- featured contracts
- faction milestones
- special visitors
- rare demand streak rewards

This helps demand loops matter beyond gold income.

## Recommended Research Tracks

Keep the number of tracks small.

Recommended track count:

- 3 or 4

Recommended tracks:

### Cultivation

Focus:

- source machines
- biome growth
- ingredient handling

Good unlocks:

- greenhouse support utility
- improved rare harvest rules
- access to more advanced source branches

### Alchemy

Focus:

- processors
- brewers
- special recipe handling

Good unlocks:

- Essence Still
- specialty cauldron types
- advanced reaction recipes

### Pantry Craft

Focus:

- room utility
- shelves
- adjacency tools
- placement support

Good unlocks:

- premium support shelf
- enchanted placement tile
- room-specific magical fixtures

### Trade and Contracts

Focus:

- demand control
- visitor quality
- contract utility

Good unlocks:

- additional secondary request slot
- improved featured-contract rewards
- specialized faction opportunities

## Example Research Nodes

Use nodes that unlock visible capability.

Good node examples:

- `Mushroom Handling`
  - unlocks improved Mushroom branch support utility
- `Crystal Infusion`
  - unlocks Crystal Grinder access
- `Efficient Shelving`
  - unlocks premium shelf utility piece
- `Guild Ledger`
  - unlocks one additional contract slot
- `Refined Distillation`
  - unlocks Essence Still

Bad node examples:

- `Potion Output +5%`
- `All Production +8%`
- `Everything Is Slightly Better I Guess`

Those belong to a different, duller game.

## Research Unlock Rules

Each research node should follow these rules:

- unlock one visible capability
- have a clear cost
- have an obvious reason to exist
- connect to a room role, machine family, or demand hook
- be understandable in a few seconds

Each track should:

- branch lightly
- stay readable
- avoid deep dependency webs

Recommended shape:

- shallow tree
- 2 to 4 nodes per tier
- 2 to 3 tiers early

## UI Rules

Research UI must respect compact-mode and low-click goals.

Use:

- one clear Arcane Study panel
- a few visible track columns or grouped cards
- short descriptions in plain language
- previews of what a node unlocks

Avoid:

- giant web visualizations
- tiny unreadable icons
- long chains of prerequisite mystery

Compact mode should not require deep research interaction.

Compact mode can show:

- research completed
- research available
- one suggested next study

That is enough.

## Example Early Access Research Slice

A good first real Arcane Study slice could be:

### Tier 1

- `Mushroom Handling`
- `Efficient Shelving`
- `Guild Ledger`

### Tier 2

- `Crystal Infusion`
- `Refined Distillation`
- `Greenhouse Fixtures`

### Tier 3

- `Advanced Fulfillment Charms`
- `Specialty Brew Methods`

This is enough to create planning without burying the player.

## Demo Teaser Option

If demo research exists at all, use only:

- one Arcane Study panel
- one currency
- 3 nodes maximum

Suggested demo-safe nodes:

- `Mushroom Handling`
- `Efficient Shelving`
- `Guild Ledger`

This proves the concept without overcommitting scope.

## Integration With Contracts and Factions

Research works best when contracts feed it.

Recommended relationship:

- contracts grant Arcane Notes
- faction milestones unlock new research nodes
- special visitors can offer unique study opportunities

This helps demand stay relevant beyond immediate gold output.

## Integration With Rooms

Research should reinforce room identity.

Examples:

- greenhouse studies support source rooms
- alchemy studies support prep and brew rooms
- pantry craft studies support spatial optimization
- trade studies support fulfillment rooms

If a research node cannot be tied to a room role, machine family, or demand hook, it is probably filler.

## Recommended Constraints

Use these hard limits:

- no more than 4 research tracks
- no giant universal passive bonus tree
- no research requirement for basic starter machines
- no research dependency that hides the core loop
- no more than one new research currency in MVP-adjacent scope

## Final Recommendation

Arcane Study should be a scoped Early Access progression layer that unlocks capabilities and specialization.

It should:

- support contracts
- support room identity
- support medium-term planning

It should not:

- replace upgrades
- replace prestige
- overwhelm the pantry fantasy

## What Changes

- research becomes a defined optional system instead of a stray word in the balance doc
- its scope is limited before it can metastasize into menu moss
- contracts, rare rewards, and room specialization gain a cleaner long-term hook

## Why It Helps The Product

- adds a strategic planning layer without replacing spatial play
- supports Early Access content growth
- keeps the game readable and marketable
- gives the progression ladder more texture between upgrades and prestige
