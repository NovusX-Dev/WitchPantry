# Incremental Economy Simulation Guide

## Real Economy Numbers

Typical cost growth:

- `1.07` to `1.15`

Production:

- Linear scaling

## Cost Formula

```text
cost = baseCost * growthRate^owned
```

Example:

```text
cost = 50 * 1.15^20
```

## Production Formula

```text
production = baseProduction * owned * multipliers
```

## Example Generator Data

Machine:

- `baseCost = 50`
- `growthRate = 1.15`
- `baseProduction = 2/sec`

## Simulation Loop

Tick: `1 second`

```text
gold += production * tick

if gold >= machineCost
    buy machine
    increase production
```

## Simulation Targets

- First automation: 2 minutes
- First prestige: 2 hours
- Late-game prestige: 10 hours

## Balanced Curve

- Upgrade 1: 10 seconds
- Upgrade 2: 20 seconds
- Upgrade 3: 40 seconds
- Upgrade 4: 80 seconds
- Upgrade 5: 3 minutes
- Upgrade 6: 10 minutes
- Upgrade 7: 30 minutes
- Prestige: 2 hours

## Simulation Goals

- Avoid dead zones
- Ensure constant upgrades
- Ensure meaningful prestige resets

## Recommended Parameters

- `growthRate = 1.13`
- `upgradeMultiplier = 2`
- `prestigeMultiplier = sqrt(totalGold)`

## End

End of document.
