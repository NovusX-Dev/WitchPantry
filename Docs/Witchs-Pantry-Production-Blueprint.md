# Witch's Pantry Automation Production Graph Blueprint

- Engine: Unity 6000.3
- Genre: Incremental / Automation

## 1. Production Graph Philosophy

The production system is designed around:

- Resource extraction
- Ingredient processing
- Potion brewing
- Bottling
- Selling

Players build automation chains that transform simple ingredients into valuable potions.

## 2. Basic Production Flow

```text
Herb Garden
  ->
Mortar Golem
  ->
Brewing Cauldron
  ->
Bottling Sprite
  ->
Potion Shelf
  ->
Gold
```

## 3. Early Game Blueprint

Phase: Manual Automation

Nodes:

- Herb Garden
- Mushroom Cave
- Mortar Golem
- Basic Cauldron

Diagram:

```text
Herbs -> Grinder -> Cauldron -> Potion -> Sell
Mushrooms -> Grinder -> Cauldron -> Potion -> Sell
```

Production rate example:

- Herb Garden: `1 herb/sec`
- Mortar Golem: `2 herbs -> powder`
- Cauldron: `1 powder -> potion`
- Potion value: `10 gold`

## 4. Mid Game Production Network

Phase: Multi-chain production

Machines:

- Herb Garden
- Mushroom Cave
- Crystal Harvester
- Mortar Golem
- Fermentation Barrel
- Enchanted Cauldron
- Bottling Sprite
- Storage Shelf

Diagram:

```text
Herbs ------.
            v
Mushrooms -> Mortar Golem -> Powder
                               |
Crystal Dust ------------------'
                               v
Fermentation Barrel -> Potion Base
                               v
Enchanted Cauldron -> Magic Potion
                               v
Bottling Sprite -> Bottled Potion
                               v
Storage Shelf -> Sell
```

Production chain complexity increases.

## 5. Late Game Factory

Phase: Fully automated magical factory

Nodes:

- Ingredient Farms
- Rare Ingredient Extractors
- Processing Machines
- Potion Factories
- Automation Spirits
- Export Network

Diagram:

```text
Ingredient Farms
      ->
Processing Machines
      ->
Intermediate Materials
      ->
Potion Factories
      ->
Automation Spirits
      ->
Export Portals
      ->
Village Economy
```

Multiple production chains run in parallel.

## 6. Potion Production Chains

Examples:

- Healing Potion: `Herbs -> Powder -> Cauldron -> Potion`
- Mana Potion: `Crystal Dust -> Powder -> Cauldron -> Potion`
- Strength Potion: `Mushrooms + Roots -> Powder -> Fermentation -> Potion`
- Invisibility Potion: `Ghost Petals -> Essence -> Cauldron -> Potion`

## 7. Advanced Chain Example

Phoenix Potion ingredients:

- Phoenix Ash
- Crystal Dust
- Dragon Scale

Graph:

```text
Phoenix Ash --.
              v
Dragon Scale -> Arcane Grinder -> Essence
              ^
Crystal Dust -'
              v
Ancient Cauldron -> Phoenix Potion
                  v
Bottling Sprite
                  v
Sell
```

Value: `500 gold`

## 8. Machine Upgrade Paths

Each machine has three upgrade tracks:

- Speed: increases processing rate
- Efficiency: consumes fewer ingredients
- Capacity: increases buffer size

Example:

- Cauldron Level 10: `2x potion output`

## 9. Production Bottlenecks

Typical bottlenecks:

- Ingredient shortages
- Slow processing machines
- Insufficient storage

Players must optimize machine ratios.

Example ratio:

- 3 Herb Farms
- 2 Mortar Golems
- 1 Cauldron

## 10. Optimal Early Factory

Best starter ratio:

- Herb Garden x3
- Mortar Golem x2
- Basic Cauldron x1
- Bottling Sprite x1

This creates a stable pipeline.

## 11. Parallel Production

Late-game factories produce multiple potions simultaneously.

Examples:

- Healing Potion line
- Mana Potion line
- Strength Potion line

Each line uses different ingredients.

## 12. Rare Ingredient Extraction

Advanced machine:

- Essence Distiller

Flow:

- Input: excess potions
- Output: rare ingredients

These are used for advanced recipes.

## 13. Automation Spirits

Late-game automation layer.

Tasks:

- Auto-build machines
- Auto-upgrade machines
- Auto-complete contracts

This transforms the factory into a self-running system.

## 14. Village Export Network

Potions are sold through trade routes.

Routes:

- Village Market
- Wizard Academy
- Royal Court

Each route has price modifiers.

## 15. Prestige Layer

Prestige resets production and unlocks Arcane Relics.

Examples:

- Golden Cauldron: `+200% brew speed`
- Endless Shelf: infinite potion storage

## 16. Production Scaling

Example progression:

- Early Game: `10 gold/sec`
- Mid Game: `500 gold/sec`
- Late Game: `50,000 gold/sec`
- End Game: `1e9 gold/sec`

## 17. Graph Visualization

Players see production networks visually.

Example:

`Herb Farm -> Grinder -> Cauldron -> Bottler -> Shelf`

Multiple chains connect into larger factory networks.

## 18. Factory Optimization Goals

Players optimize:

- Machine ratios
- Upgrade paths
- Resource distribution
- Contract completion speed

## 19. End-Game Factory

```text
Ingredient Worlds
      ->
Mega Processing Plants
      ->
Arcane Potion Factories
      ->
Magical Export Network
      ->
Infinite Wealth
```

## End

End of document.
