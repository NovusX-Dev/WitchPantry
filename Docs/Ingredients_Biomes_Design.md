# Witch's Pantry Automation Ingredient and Biome Progression Design

- Scope: 3-month project
- Engine: Unity 6
- Genre: Incremental / Automation / Cozy Fantasy
- Production Model: Virtual Connections (Shared Pantry Inventory)

This document defines a tight ingredient and biome progression suitable for a solo 3-month project.

Goals:

- About 15 ingredients
- 5 biomes
- 10 to 12 potions
- Predictable economy scaling
- Clear player progression

## 1. Biome Progression Overview

| Biome | Tier | Ingredients | Machines | Unlock Cost |
| --- | --- | --- | --- | --- |
| Backyard Garden | Tier 1 | Herb, Water | Herb Garden | Start Area |
| Damp Cave | Tier 1 | Mushroom | Mushroom Cave | 150 Gold |
| Crystal Cavern | Tier 2 | Crystal Dust, Glow Shard | Crystal Mine | 800 Gold + 25 Healing Potions |
| Shadow Woods | Tier 2 | Nightshade, Moonleaf | Shadow Grove | 2000 Gold + Research: Toxicology |
| Floating Isles | Tier 3 | Sky Lotus, Phoenix Feather | Sky Garden | 8000 Gold + Prestige Level 1 |

Total primary ingredients introduced: `9`

## 2. Ingredient List

### Raw Ingredients

| Ingredient | Source Machine | Biome |
| --- | --- | --- |
| Herb | Herb Garden | Backyard Garden |
| Water | Well | Backyard Garden |
| Mushroom | Mushroom Cave | Damp Cave |
| Crystal Dust | Crystal Mine | Crystal Cavern |
| Glow Shard | Crystal Mine | Crystal Cavern |
| Nightshade | Shadow Grove | Shadow Woods |
| Moonleaf | Shadow Grove | Shadow Woods |
| Sky Lotus | Sky Garden | Floating Isles |
| Phoenix Feather | Sky Garden | Floating Isles |

### Processed Ingredients

These are created by converter machines.

| Ingredient | Machine | Inputs |
| --- | --- | --- |
| Ground Herb | Mortar Golem | Herb |
| Mushroom Paste | Mortar Golem | Mushroom |
| Crystal Powder | Crystal Grinder | Crystal Dust |
| Shadow Essence | Essence Still | Nightshade |
| Lotus Extract | Essence Still | Sky Lotus |

Total processed ingredients: `5`

## 3. Total Ingredient Count

- Raw ingredients: 9
- Processed ingredients: 5
- Total: 14 ingredients

This number is ideal for a 3-month production schedule.

## 4. Potion Recipes

| Potion | Inputs |
| --- | --- |
| Healing Potion | Ground Herb + Water |
| Energy Potion | Mushroom Paste |
| Mana Potion | Crystal Powder + Water |
| Clarity Elixir | Glow Shard + Ground Herb |
| Antidote | Moonleaf + Mushroom Paste |
| Shadow Cure | Shadow Essence + Crystal Powder |
| Fire Resistance | Crystal Powder + Mushroom Paste |
| Levitation Potion | Lotus Extract |
| Resurrection Tonic | Phoenix Feather + Lotus Extract |

Total potions: `9`

## 5. Biome Unlock Details

### Backyard Garden

Starting biome.

Machines:

- Herb Garden
- Well

Resources:

- Herb
- Water

Purpose: starter production chain.

### Damp Cave

Unlock cost:

- 150 Gold

Machines:

- Mushroom Cave

Resources:

- Mushroom

Purpose: adds a secondary potion branch.

### Crystal Cavern

Unlock cost:

- 800 Gold
- Deliver 25 Healing Potions

Machines:

- Crystal Mine
- Crystal Grinder

Resources:

- Crystal Dust
- Glow Shard

Purpose: introduces magical resources and mana potions.

### Shadow Woods

Unlock cost:

- 2000 Gold
- Research: Toxicology

Machines:

- Shadow Grove
- Essence Still

Resources:

- Nightshade
- Moonleaf

Purpose: unlocks antidotes and rare cures.

### Floating Isles

Unlock cost:

- 8000 Gold
- Prestige Level 1

Machines:

- Sky Garden

Resources:

- Sky Lotus
- Phoenix Feather

Purpose: late-game exotic potion crafting.

## 6. Example Progression Curve

- Stage 1: Herb + Water -> Healing Potions
- Stage 2: Mushroom -> Energy Potions
- Stage 3: Crystal Dust -> Mana Potions
- Stage 4: Nightshade -> Antidotes
- Stage 5: Sky Lotus -> Legendary Potions

## 7. Machine Count (MVP)

| Machine | Type |
| --- | --- |
| Herb Garden | Producer |
| Well | Producer |
| Mushroom Cave | Producer |
| Crystal Mine | Producer |
| Shadow Grove | Producer |
| Sky Garden | Producer |
| Mortar Golem | Converter |
| Crystal Grinder | Converter |
| Essence Still | Converter |
| Enchanted Cauldron | Converter |
| Bottling Sprite | Utility |
| Market Cart | Utility |

Total machines: `12`

## 8. Why This Works

This progression ensures:

- Steady ingredient discovery
- Manageable code complexity
- Clear economy scaling
- Consistent player goals
- Expandable systems

Each biome adds:

- 2 ingredients
- 1 to 2 machines
- 2 to 3 potions

## 9. Final Scope Summary

- Biomes: 5
- Ingredients: 14
- Potions: 9
- Machines: 12

This design fits comfortably within a solo 12-week development cycle.
