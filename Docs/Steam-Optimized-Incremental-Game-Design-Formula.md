# Steam-Oriented Incremental Design Notes For Witch Pantry

## Purpose

This document translates Steam incremental and desktop-idler patterns into Witch Pantry-specific guidance.

It is a reference, not a mandate. The project should not chase every idle-game convention if doing so weakens the spatial pantry hook.

## 1. Steam-Friendly Hook

The store-page promise should be:

```text
Build a magical pantry factory you can watch, optimize, and leave running.
```

The strongest screenshots and GIFs should show:

- a compact enchanted room
- readable machine motion
- a visible production chain
- customer or faction demand
- compact desktop presence

## 2. Core Loop

Recommended loop:

```text
produce ingredients
  ->
process and brew through visible machines
  ->
fulfill customer or faction demand
  ->
buy upgrades and improve layout
  ->
unlock new content or room pressure
  ->
return later to claim, fix, and optimize
```

This is more specific than the generic idle loop of "earn resources, buy generators, prestige."

## 3. Growth Equation

Useful baseline:

```text
nextCost = baseCost * growthRate^owned
outputPerMinute = (60 / craftTimeSeconds) * machineCount * speedMultiplier
```

Witch Pantry-specific constraint:

- the slowest ingredient or machine in a recipe chain should become a readable bottleneck
- contract demand should change which chain the player cares about
- upgrades should improve visible flow, not only hidden multipliers

## 4. Desktop-Idler Standard

Desktop mode is a real differentiator only if it is useful.

Required signals:

- current demand
- pantry health
- worst bottleneck
- reward claim status
- one to three quick actions

Avoid treating desktop mode as a resized HUD. It should be intentionally glanceable.

## 5. Retention Layers

Use retention layers in this order:

1. readable starter production
2. contract goals
3. upgrades with visible impact
4. offline return summary
5. room pressure and expansion
6. faction variety
7. scoped research or prestige

Do not lead with prestige before the pantry loop proves itself.

## 6. Wishlist And Demo Readability

The demo should generate wishlists by making the product legible fast:

- first minute: player understands the pantry fantasy
- first 10 minutes: player completes or nearly completes a production goal
- first 30 minutes: player sees a bottleneck, fixes it, and wants more content

If a feature cannot help one of those moments, it is probably not demo-critical.

## 7. Common Traps

Avoid:

- generic generator tier lists
- progression that only adds bigger numbers
- prestige as a substitute for content
- UI that hides the room
- desktop mode with no meaningful actions
- screenshots that look like clutter instead of a magical workplace

## 8. Success Pattern

Witch Pantry should follow this pattern:

```text
clear fantasy
  ->
readable production
  ->
useful idle return
  ->
contract-driven goals
  ->
room and content expansion
  ->
longer-term progression
```

Steam incremental conventions are useful only when they serve that pattern.
