# Customer Demand and Contracts

For the concrete authored contract list, use [ScriptableObject-Authoring-Checklist.md](C:/Unity/Repos/WitchPantry/Docs/ScriptableObject-Authoring-Checklist.md) as the implementation checklist.

## Core Direction

Use controllable customer demand and faction requests instead of a broad simulated export market.

## Demand Sources

- regular customers
- guild orders
- travelers
- festivals
- special visitors

## Demand Shifts

Demand can shift because of:

- progression milestones
- time-limited visitors
- season or festival windows
- event-driven opportunities
- player reputation with a faction

## Reward Types

- gold
- rare ingredients
- unlock progress
- faction reputation
- temporary pantry boons

## Contract Cadence

- one featured demand target at a time
- a small set of secondary requests
- occasional premium or urgent opportunities

## Progression Hooks

- new factions unlock with biome or prestige progress
- higher tiers ask for more specialized mixes
- demand should steer the player toward new layout and production decisions

## Contract Data Shape

Contracts should be authored as `ContractDefinition` ScriptableObjects rather than loose
hardcoded request blobs.

Recommended concrete fields:

- inherited from `ContentDefinition`:
  - `Icon`
  - `Id`
  - `ContentType`
  - `DisplayName`
- `Tier`
- `Faction`
- `TargetPotion`
- `AmountRequired`
- `RewardGold`
- `DurationHours`
- `Weight`

Supporting metadata:

- `ContractFaction` identifies who is asking
- `Weight` controls how often a valid contract appears relative to other valid contracts
- `TargetPotion` should reference `PotionDefinition` directly
- contracts do not currently use `UnlockSource`; availability is expected to be controlled by runtime generation rules and progression logic
- `Id` should follow the stable authored format `contract.<slug>`

This keeps demand readable in design terms while still giving runtime systems a concrete,
typed contract pool to generate from.

## UX Rule

The player should always know:

- who is asking
- what they want
- how long it lasts
- why it matters
