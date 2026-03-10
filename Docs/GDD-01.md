# Witch's Pantry Automation GDD

- Version: 1.0
- Engine: Unity 6000.3
- Platform: Steam (Windows)
- Genre: Incremental / Idle / Automation
- Art Style: Cozy Pixel Art
- Target Development Time: 3 months (solo dev MVP)

## 1. Game Overview

### High Concept

*Witch's Pantry Automation* is a cozy incremental automation game where the player runs a magical potion pantry. The player gathers mystical ingredients, processes them using enchanted kitchen devices, and builds an automated potion factory.

The core gameplay focuses on creating efficient potion production pipelines.

The game evolves from simple manual crafting into a fully automated magical factory.

Reference fantasy:

- Factorio x Potion Craft x Idle Game

## 2. Core Pillars

1. Cozy Automation: Players build relaxing production systems.
2. Satisfying Progression: Numbers go up constantly with meaningful unlocks.
3. Visual Factory Growth: The pantry physically expands with machines and creatures.
4. Relaxed Idle Gameplay: Production continues while the player is away.

## 3. Target Audience

Primary audience:

- Idle / incremental players

Secondary audience:

- Cozy simulation players
- Automation / factory game fans

Comparable games:

- Cookie Clicker
- Rusty's Retirement
- Factory Town Idle
- Melvor Idle

## 4. Gameplay Loop

### Early Game Loop

```text
Collect ingredients manually
  ->
Process ingredients
  ->
Brew potion
  ->
Bottle potion
  ->
Sell potion
  ->
Buy upgrades
```

### Mid Game Loop

```text
Ingredients harvested automatically
  ->
Machines process ingredients
  ->
Potion lines operate automatically
  ->
Player optimizes throughput
  ->
Unlocks new ingredients
```

### Late Game Loop

```text
Full automation
  ->
Contract fulfillment
  ->
Market economy
  ->
Prestige system
  ->
New realms unlocked
```

## 5. Game Systems

### Ingredient System

Ingredients are resources used in potion recipes.

#### Ingredient Types

Common:

- Mushroom
- Mint
- Spring Water

Uncommon:

- Bat Wing
- Glow Moss
- Crystal Dust

Rare:

- Phoenix Feather
- Ghost Essence

Legendary:

- Dragon Blood
- Time Sand

### Potion Recipes

Potion recipes combine ingredients.

Example:

- Potion: Healing Potion
- Ingredients: Redcap Mushroom, Spring Water
- Value: 10 gold
- Brew Time: 5 seconds

### Automation Machines

Machines automate production.

Examples:

- Mortar Golem: grinds ingredients
- Self-Stirring Cauldron: brews potions automatically
- Bottle Sprite: bottles finished potions
- Delivery Owl: ships potions to customers

## 6. Production Chain Example

```text
Forest Portal
  ->
Mushroom Harvester
  ->
Mortar Golem
  ->
Brewing Cauldron
  ->
Bottle Sprite
  ->
Potion Shelf
  ->
Delivery Owl
```

## 7. Progression

Progression is driven by:

- Unlocking ingredients
- Unlocking machines
- Increasing production speed
- Completing contracts
- Prestige resets

## 8. Economy System

Players earn gold by selling potions.

Gold is used for:

- Machines
- Ingredient unlocks
- Research
- Pantry expansion

## 9. Contracts

Villagers request potions.

Example contract:

- Name: Village Order
- Goal: 50 Healing Potions
- Reward: 500 gold

## 10. Events

Random events add variation.

Examples:

- Full Moon: potion production doubled
- Goblin Raid: potions stolen
- Merchant Caravan: high-value contracts appear
- Potion Explosion: machine damage

## 11. Prestige System

Prestige represents becoming a stronger witch.

Effects:

- Reset pantry
- Gain permanent upgrades

Examples:

- Potion production multiplier
- New ingredient realms
- Advanced automation

## 12. UI Layout

Main screen:

- Ingredient Production
- Potion Production
- Active Machines
- Contracts
- Gold Balance

Side panel:

- Recipes
- Upgrades
- Research
- Prestige

## 13. Art Direction

Style:

- Pixel art
- Warm cozy lighting
- Magical fantasy aesthetic

Visual inspirations:

- Stardew Valley
- Potion Craft
- Terraria

## 14. Audio Direction

- Relaxing ambient music
- Magical bubbling sounds
- Creature noises
- Potion brewing effects

## 15. Technical Architecture

Data-driven design:

- ScriptableObjects for ingredients
- ScriptableObjects for recipes
- ScriptableObjects for machines
- ScriptableObjects for contracts

Managers:

- `ProductionManager`
- `EconomyManager`
- `SaveManager`
- `EventManager`

## 16. Save System

- Local save
- JSON save file
- Auto-save every 30 seconds
- Offline progression calculated on load

## 17. Content Scope (MVP)

- Ingredients: 12
- Potions: 15
- Machines: 6
- Events: 6
- Contracts: 10
- Prestige Layers: 1
- Realms: 1

## 18. Development Roadmap

Month 1:

- Core gameplay systems
- Ingredient system
- Recipe system
- Machine system
- Basic UI

Month 2:

- Automation logic
- Contracts
- Economy
- Prestige
- Content creation

Month 3:

- Art
- Audio
- Balancing
- Steam integration
- QA

## 19. Monetization

- Premium game
- Price target: $6.99 to $9.99

Possible DLC:

- New ingredient realms
- Additional machines

## 20. Risks

- Scope creep
- Balancing complexity
- Automation bugs
- Save corruption

## 21. Success Metrics

- 10k Steam wishlists before launch
- 20k copies sold in the first year
- Positive Steam rating above 85%

## 22. Future Expansion

- Additional realms
- Ingredient mutation system
- Potion shop interior
- Online potion trading
- Steam Workshop modding
