# Witch Pantry Incremental Economy Model

- Engine: Unity 6; current project version: `Witch-Pantry/ProjectSettings/ProjectVersion.txt`

## 1. Economy Philosophy

Witch Pantry uses incremental pacing to support a visible production fantasy.

Numbers should grow, but the player should still understand why the pantry is improving:

- a machine runs faster
- a recipe chain clears a bottleneck
- a contract becomes reachable
- a room layout gets more efficient
- compact mode reports a healthier workshop

## 2. Core Currency

Primary currency:

- gold

Gold is used for:

- machine purchases
- machine upgrades
- room improvements
- selected unlock costs

Gold should not be the only success signal. Contracts, faction hooks, room pressure, and production readability should matter too.

## 3. Production Formula

Basic output:

```text
outputPerMinute = (60 / craftTimeSeconds) * machineCount * speedMultiplier
```

With upgrade level:

```text
effectiveSpeed = baseSpeed * upgradeMultiplier^upgradeLevel
```

For chained recipes, the real output is constrained by the slowest required input.

```text
chainOutput = min(inputSupplyRates) converted through recipe ratios
```

This matters more than raw machine count because Witch Pantry is about visible bottlenecks.

## 4. Machine Cost Formula

Classic idle cost growth is still useful:

```text
nextCost = baseCost * growthRate^owned
```

Starting range:

- `growthRate = 1.10` to `1.18`
- lower for required starter machines
- higher for optional capacity expansion

Use the formula to shape pacing, not to hide every decision behind exponential math.

## 5. Upgrade Formula

Upgrade costs should scale with current level:

```text
upgradeCost = baseUpgradeCost * upgradeGrowth^currentLevel
```

Upgrade value should be readable:

- faster animation
- larger queue
- better recipe fit
- stronger adjacency payoff
- improved contract throughput

Avoid generic `+10% production` upgrades unless they are wrapped in a clear pantry behavior.

## 6. Potion And Contract Value

Potion value comes from authored content and balance tuning:

```text
potionValue = baseSellValue * tierMultiplier * demandMultiplier
```

Contract reward should account for:

- target potion difficulty
- amount required
- time pressure
- faction importance
- unlock or special reward value

```text
contractReward = potionValue * amountRequired * rewardMultiplier
```

Contract value should create goals, not opaque churn.

## 7. Offline Progression

Offline progression should preserve the pantry fantasy:

```text
offlineOutput = simulatedOutput * offlineEfficiency
```

Recommended rule:

- generous enough to reward return visits
- capped enough to avoid breaking contract pacing
- summarized clearly when the player returns

Return summary should answer:

- what was produced
- what was completed
- what blocked progress
- what action is recommended next

## 8. Prestige And Research

Prestige and research should not appear before the starter pantry loop is proven.

When added, they should unlock capability and specialization:

- new room roles
- machine behavior variants
- stronger contract options
- layout tools

Avoid prestige that only says:

```text
gain permanent +X% production
```

That is useful math, but weak fantasy.

## 9. Balance Targets

Early targets:

- first machine purchase: 1 to 3 minutes
- first contract completion: 5 to 12 minutes
- first bottleneck fix: 5 to 10 minutes
- first meaningful upgrade choice: 10 to 20 minutes
- first session return value: visible after a short idle break

## 10. Design Test

An economy change is good when it creates a player sentence like:

```text
I need more ground herb because the healing contract is waiting, so upgrading the Mortar Golem matters now.
```

If the sentence is only:

```text
I need more gold because the next number is bigger.
```

the economy is too generic.
