# Witch Pantry Economy Simulation Guide

## Purpose

Use this as a lightweight reference for simulating Witch Pantry's early economy.

The goal is not to copy a generic idle curve. The goal is to check whether machines, potions, contracts, and upgrades create a readable pantry pace.

## Core Simulation Inputs

Minimum inputs:

- machine purchase cost
- machine upgrade cost
- recipe craft time
- recipe input and output amounts
- potion sell value or contract reward
- active contract requirements
- offline duration

Read authored values from content definitions where possible. Do not create a parallel economy universe unless the tool is explicitly testing hypothetical balance.

## Useful Formulas

Machine purchase cost:

```text
nextCost = baseCost * growthRate^owned
```

Machine upgrade cost:

```text
upgradeCost = baseUpgradeCost * upgradeGrowth^currentLevel
```

Production output:

```text
outputPerMinute = (60 / craftTimeSeconds) * machineCount * speedMultiplier
```

Contract completion time:

```text
minutesToComplete = amountRequired / outputPerMinute
```

Offline output:

```text
offlineOutput = outputPerMinute * offlineMinutes * offlineEfficiency
```

## Early Pacing Targets

Use these as starting checks, not permanent laws:

- first visible production line: under 2 minutes
- first bottleneck fix: 5 to 10 minutes
- first contract completion: 5 to 12 minutes
- first meaningful upgrade choice: 10 to 20 minutes
- first room-pressure discussion: after the starter loop is understood

## Pantry-Specific Checks

The curve is weak if:

- the best action is always "buy the cheapest machine"
- contracts do not change production priorities
- upgrades only increase invisible numbers
- compact mode cannot explain why production slowed
- a player waits without understanding what would improve throughput

The curve is strong if:

- the player sees a machine become the bottleneck
- the next upgrade has a visible or contract-relevant reason
- demand changes which potion chain matters
- idle returns create a clear "claim, fix, improve" loop

## Simulation Output

A useful simulation report should show:

- gold over time
- potion output over time
- contract completion time
- bottleneck machine or ingredient
- suggested next upgrade
- compact-mode summary text for the same state
