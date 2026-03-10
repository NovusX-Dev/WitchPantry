# Steam-Optimized Incremental Game Design Formula

This document describes the design structure commonly used by successful incremental and idle games on Steam.

Goals:

- Player retention
- Wishlists
- Long playtime
- Viral progression loops

## 1. Core Gameplay Loop

The main loop of successful incremental games is extremely simple.

Example loop:

```text
earn resources
  ->
buy generators
  ->
increase production
  ->
unlock automation
  ->
prestige reset
  ->
repeat
```

This loop creates constant progression.

## 2. The Growth Equation

The core math behind incremental games is a balance between two curves:

- Production grows linearly.
- Costs grow exponentially.

Production formula:

```text
production_total = (production_base * owned) * multipliers
```

Cost formula:

```text
cost_next = base_cost * growth_rate^owned
```

Example:

- `base_cost = 4`
- `growth_rate = 1.07`
- `owned = 10`
- `cost = 4 * 1.07^10`
- `production = 1.67 * 10`

## 3. Upgrade Pacing Formula

Good incremental pacing follows a logarithmic reward structure.

Example timing:

- Upgrade 1: 10 seconds
- Upgrade 2: 20 seconds
- Upgrade 3: 40 seconds
- Upgrade 4: 80 seconds
- Upgrade 5: 3 minutes
- Upgrade 6: 10 minutes
- Upgrade 7: 30 minutes
- Prestige: 2 hours

## 4. Generator Tier Structure

Successful incremental games use multiple generator tiers.

Example:

- Tier 1: Farm, `1/sec`
- Tier 2: Factory, `10/sec`
- Tier 3: Laboratory, `100/sec`

Each tier costs roughly 10 times the previous one.

## 5. Automation Curve

Automation unlocks gradually:

- Phase 1: Manual clicking
- Phase 2: Automated generators
- Phase 3: Automation chains
- Phase 4: Self-expanding production systems

## 6. Prestige System

Prestige resets progression but grants permanent bonuses.

Formula:

```text
prestige_bonus = 1 + sqrt(total_currency)
```

Example:

- `total_currency = 1,000,000`
- `bonus = 1001`

Prestige allows the player to replay the game faster.

## 7. Retention Mechanics

Successful Steam idle games often include:

- Achievements
- Research trees
- Unlockable machines
- Meta progression
- Rare resources

## 8. Idle Progression

Steam idle players expect offline progression.

```text
offline_rewards = production_rate * offline_time
```

Example:

- `production = 20/sec`
- `offline_time = 3600 seconds`
- `reward = 72,000 resources`

## 9. Scaling Numbers

Incremental games eventually reach huge numbers.

Use scientific notation:

- `1e6 = 1 million`
- `1e12 = 1 trillion`
- `1e100 = late-game scale`

## 10. Engagement Design

Successful idle games maintain engagement through:

- Frequent upgrades
- New unlocks
- Visible production growth
- Rewarding resets

## 11. Content Layers

The best incremental games introduce layers of systems:

- Layer 1: Basic production
- Layer 2: Automation chains
- Layer 3: Prestige
- Layer 4: Meta upgrades
- Layer 5: Late-game mechanics

## 12. Steam-Friendly Features

Idle games that succeed on Steam usually include:

- Visible progress bars
- Relaxing gameplay
- Achievements
- Long playtime
- Optional active play

## 13. Player Motivation Loop

```text
progress feels fast
  ->
progress slows
  ->
prestige unlocks
  ->
progress becomes fast again
```

This loop drives retention.

## 14. Example Balanced Parameters

- `growth_rate = 1.07 to 1.15`
- `production_multiplier = 2x per upgrade`
- `prestige_multiplier = sqrt(total_currency)`
- `upgrade_cost_growth = exponential`

## 15. Simulation Requirement

All incremental economies must be simulated.

Typical loop:

```text
for tick in simulation
    gold += production

    if gold >= next_upgrade
        buy upgrade
        increase production
```

## 16. Long-Term Player Goals

Idle games succeed when players always have goals.

Examples:

- Unlock new machines
- Complete achievements
- Reach prestige milestones
- Build max-efficiency setups

## 17. Steam Success Pattern

Many successful Steam idle games follow this structure:

- Simple initial gameplay
- Deep automation systems
- Long progression curves
- Prestige mechanics
- Relaxing visuals

## 18. Example Steam Idle Game Loop

```text
Idle farm
  ->
automated workers
  ->
factory production
  ->
prestige upgrades
  ->
new automation layer
```

## End

End of document.
