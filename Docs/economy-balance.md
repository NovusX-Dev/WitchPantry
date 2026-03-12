# Witch's Pantry Automation Economy Balance Model

This document defines the target economy model for *Witch's Pantry Automation*.

The model follows a standard incremental-game structure:

- Costs scale exponentially.
- Production scales linearly and through multipliers.
- Progression feels fast early, then slows into upgrades, biome unlocks, and prestige.

This document is the balancing reference for machine costs, potion values, upgrade scaling, prestige pacing, and offline progression.

---

## Core Economic Formulas

### Generator Cost Formula

```text
cost_next = baseCost * (growthRate ^ owned)
```

Typical growth-rate ranges:

| Growth Rate | Effect |
| --- | --- |
| 1.07 | Slow scaling |
| 1.10 | Medium scaling |
| 1.15 | Fast scaling |

Recommended baseline for this project:

```text
growthRate = 1.12
```

### Production Formula

```text
production_total = baseProduction * machinesOwned * multipliers
```

Multipliers come from upgrades, research, biome progression, and prestige.

### Upgrade Cost Formula

```text
upgradeCost = baseCost * (1.5 ^ level)
```

### Prestige Formula

```text
prestigeMultiplier = 1 + (prestigeLevel * 0.2)
```

### Offline Progress Formula

```text
offlineProduction = productionRate * offlineHours * 0.5
```

The `0.5` factor is the starting balance assumption for offline rewards and should be validated in simulation.

---

## Return on Investment Targets

Target ROI by phase:

| Phase | Target ROI |
| --- | --- |
| Early game | 30 to 90 seconds |
| Mid game | 3 to 10 minutes |

Example:

```text
machine cost = 100 gold
production gain = 2 gold/sec
ROI = 50 seconds
```

These targets should inform machine pricing, upgrade pacing, and contract reward tuning.

---

## Machine Economy Table

| Machine | Base Cost | Growth Rate | Production per Second |
| --- | ---: | ---: | --- |
| Herb Garden | 10 | 1.12 | 1 Herb |
| Well | 20 | 1.12 | 1 Water |
| Mushroom Cave | 80 | 1.12 | 1 Mushroom |
| Crystal Mine | 500 | 1.12 | 1 Crystal Dust |
| Shadow Grove | 1500 | 1.12 | 1 Nightshade |
| Sky Garden | 5000 | 1.12 | 1 Sky Lotus |

These values are the starting balance targets, not final shipped numbers.

---

## Potion Value Table

| Potion | Ingredients | Sale Value |
| --- | --- | ---: |
| Healing Potion | Herb + Water | 12 gold |
| Energy Potion | Mushroom Paste | 30 gold |
| Mana Potion | Crystal Powder + Water | 90 gold |
| Antidote | Moonleaf + Mushroom Paste | 120 gold |
| Shadow Cure | Shadow Essence + Crystal Powder | 240 gold |
| Levitation Potion | Lotus Extract | 600 gold |
| Resurrection Tonic | Phoenix Feather + Lotus Extract | 1500 gold |

Potion values should be validated against biome unlock costs, machine pacing, and prestige timing.

---

## Upgrade Scaling Targets

| Level | Production Multiplier |
| --- | ---: |
| 1 | 1.25x |
| 2 | 1.50x |
| 3 | 2.00x |
| 4 | 3.00x |
| 5 | 5.00x |

This table defines the intended upgrade feel for MVP tuning and should be tested against ROI targets.

---

## Prestige Targets

Prestige unlock threshold:

```text
100,000 lifetime gold earned
```

Reference prestige curve:

| Prestige Level | Multiplier |
| --- | ---: |
| 1 | 1.2x |
| 3 | 1.6x |
| 5 | 2.0x |
| 10 | 3.0x |

Prestige pacing should support repeat loops without invalidating the early game instantly.

---

## Playtime Curve

| Stage | Target Time |
| --- | --- |
| Early game | 30 to 60 minutes |
| Mid game | 3 to 5 hours |
| Late game | 10 to 20 hours |
| Prestige loop | 3 to 6 hours |

This curve should be checked against simulation output and live playtests.

---

## Number Scaling

Short notation targets:

- `1K = 1,000`
- `1M = 1,000,000`
- `1B = 1,000,000,000`
- `1T = 1,000,000,000,000`

Recommended Unity numeric strategy:

- Use `double` for MVP if values remain manageable.
- Evaluate `BigDouble` via BreakInfinity if testing shows late-game values exceed safe `double` readability or precision expectations.

---

## Data Model Notes

Suggested machine balance data:

```text
MachineData
- name
- baseCost
- growthRate
- productionRate
```

This keeps balancing data editable without recompiling gameplay code.

---

## Final Economy Targets

| Target | Value |
| --- | --- |
| Machines | 12 |
| Ingredients | 14 |
| Potions | 9 |
| Biomes | 5 |
| Total playtime | 10 to 20 hours |
| Prestige loops | 5 to 10 |

This target structure is intended to fit the current solo 3-month production scope.
