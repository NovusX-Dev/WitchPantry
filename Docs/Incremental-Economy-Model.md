# Witch's Pantry Automation Incremental Economy Model

- Engine: Unity 6000.3

## 1. Economy Design Philosophy

Incremental economies are built on a balance between production growth and upgrade cost growth.

General rule:

- Production grows linearly or polynomially.
- Costs grow exponentially.

This ensures early progress feels fast but gradually slows.

## 2. Core Currency

Primary currency:

- Gold

Used for:

- Machines
- Ingredient unlocks
- Upgrades

## 3. Production Formula

```text
production = baseProduction * machineCount * multipliers
```

Example:

- `baseProduction = 1 potion/sec`
- `machines = 10`
- `multiplier = 2`

Result:

- `production = 20 potions/sec`

## 4. Machine Cost Formula

Classic idle formula:

```text
cost_next = baseCost * growthRate^owned
```

Example:

- `baseCost = 10`
- `growthRate = 1.15`
- `owned = 5`
- `cost ~= 20`

## 5. Total Production

```text
totalProduction = baseProduction * owned * globalMultiplier
```

Example:

- `baseProduction = 2`
- `owned = 20`
- `globalMultiplier = 1.5`
- `totalProduction = 60/sec`

## 6. Potion Value

Potion sale value:

```text
value = baseValue * rarityMultiplier
```

Examples:

- Healing Potion: `10 * 1 = 10 gold`
- Rare Potion: `10 * 5 = 50 gold`

## 7. Return on Investment (ROI)

ROI determines upgrade pacing.

```text
ROI = cost / productionIncrease
```

Example:

- Upgrade cost: `100 gold`
- Production increase: `5 gold/sec`
- ROI: `20 seconds`

Ideal ranges:

- Early game: 3 to 10 seconds
- Mid game: 30 to 90 seconds
- Late game: 5 to 20 minutes

## 8. Upgrade Multipliers

```text
multiplier = base * (1.2^level)
```

Example:

- `level = 5`
- `multiplier ~= 2.49`

## 9. Machine Efficiency Scaling

```text
production = base * (1.1^level)
```

Example:

- `base = 10`
- `level = 10`
- `production ~= 25.9`

## 10. Bulk Purchase Formula

```text
totalCost = baseCost * (growthRate^n - 1) / (growthRate - 1)
```

This is required for buy-max buttons.

## 11. Prestige System

Prestige resets production but adds a multiplier.

Example:

```text
prestigeMultiplier = 1 + (prestigePoints * 0.1)
```

If `prestigePoints = 5`, then `multiplier = 1.5`.

## 12. Offline Progression

```text
offlineProduction = productionRate * offlineTime
```

Example:

- `production = 20/sec`
- `offlineTime = 3600 seconds`
- `offlineGold = 72000`

## 13. Late-Game Scaling

Eventually numbers reach huge sizes.

Use scientific notation:

- `1e6 = 1,000,000`
- `1e12 = 1 trillion`

## 14. Economy Curve

Typical progression:

- Early game: fast purchases
- Mid game: strategic upgrades
- Late game: long waits and prestige

## 15. Example Economy Table

| Machines Owned | Cost Next | Production/sec |
| ---: | ---: | ---: |
| 1 | 10 | 1 |
| 5 | 20 | 5 |
| 10 | 40 | 10 |
| 20 | 160 | 20 |
| 50 | 1080 | 50 |

## 16. Balance Targets

- First automation machine: 2 minutes
- First production chain: 10 minutes
- Prestige unlock: 2 hours
- Second prestige: 10 hours

## 17. Economy Debug Tool

Create a debug window that allows:

- Adding gold
- Adding machines
- Simulating ticks
- Forcing prestige

## 18. Testing Strategy

Simulate the economy over `100,000` ticks and check:

- Progress speed
- Upgrade pacing
- Prestige timing

## 19. Anti-Stall Mechanisms

Avoid deadlocks with:

- Temporary boosts
- Special events
- Free resources

## 20. Long-Term Retention

Introduce:

- Rare ingredients
- Special potion contracts
- New production chains

## End

End of document.
