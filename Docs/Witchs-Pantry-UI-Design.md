# Witch's Pantry Automation UI and UX Layout Design

- Engine: Unity 6000.3
- Resolution: 1920x1080

Goal: create a clean incremental-game interface optimized for Steam players.

## 1. UI Design Principles

The UI follows these rules:

- Always show important numbers
- Avoid clutter
- Introduce systems gradually
- Provide constant feedback

Primary visible data:

- Gold
- Gold per second
- Potion production per second
- Next machine cost

Secondary data:

- Inventory
- Contracts
- Prestige upgrades
- Achievements

## 2. Main Screen Layout

```text
-----------------------------------------------------
| GOLD | GOLD/SEC | POTIONS/SEC | PRESTIGE LEVEL   |
-----------------------------------------------------

| Machines Panel | Factory View | Upgrades Panel |
|                |              |                |
|                |              |                |
|                |              |                |

-----------------------------------------------------
| Contracts | Inventory | Prestige | Settings |
-----------------------------------------------------
```

## 3. Top Resource Bar

The most important numbers appear here and remain displayed permanently.

Example:

- Gold: 12,500
- Gold/sec: 420
- Potions/sec: 32
- Prestige Level: Witch Rank 3

## 4. Factory Visualization Panel

The center screen shows animated machines working.

Example machines:

- Herb Garden
- Mortar Golem
- Cauldron
- Bottling Sprite

Purpose: make the idle game satisfying to watch.

## 5. Machines Panel (Left)

Lists all machines.

Example card:

```text
Herb Garden
Owned: 3
Production: 3 herbs/sec
Buy Button
Cost: 120 gold
```

Another example:

```text
Mortar Golem
Owned: 1
Grinding Speed: 2/sec
Upgrade Button
Cost: 500 gold
```

## 6. Upgrades Panel (Right)

Contains permanent upgrades.

Examples:

- Faster Brewing: `+20% potion speed`, cost `1000 gold`
- Better Bottles: `+15% potion value`, cost `800 gold`
- Advanced Cauldron: double potion output, cost `2000 gold`

## 7. Contracts Panel

Village requests potions.

Example:

- Deliver 50 Healing Potions
- Reward: 500 gold and a rare ingredient

Contract difficulty should scale with progression.

## 8. Inventory Panel

Displays ingredients and potions.

Example:

- Herbs: 120
- Mushrooms: 80
- Crystal Dust: 12
- Healing Potions: 30
- Mana Potions: 10

## 9. Prestige Panel

Displays witch progression.

Example:

- Witch Rank 3
- Prestige reward: Arcane Essence
- Example upgrade: Ancient Brewing, `+200% potion speed`

## 10. Machine Tooltip

Hovering over a machine shows detailed stats.

Example:

```text
Mortar Golem
Grinding Speed: 2 ingredients/sec
Input Buffer: 20
Output Buffer: 20
Efficiency Bonus: +15%
```

## 11. Visual Feedback

Every action should produce feedback.

Examples:

- Gold gain animation
- Potion brewing animation
- Upgrade flash effect
- Sound effects

## 12. Large Number Formatting

Numbers use suffixes.

Examples:

- `1000 -> 1K`
- `1000000 -> 1M`
- `1000000000 -> 1B`

Late-game numbers may use scientific notation, for example `1e12`.

## 13. Automation Indicators

Machines display icons showing automation status.

Examples:

- Auto Feed Enabled
- Auto Sell Enabled
- Auto Upgrade Enabled

## 14. Progress Bars

Machines display processing bars.

Example:

```text
Cauldron
Brewing Progress
███████░░░░
```

## 15. Achievement Popups

Achievements appear in a corner.

Example:

- Potion Apprentice
- Brew 100 potions
- Reward: `+5% production`

## 16. Event Notifications

Random magical events appear in a banner.

Example:

- Fairy Blessing: potion production x2 for 60 seconds

## 17. Accessibility

Include:

- Colorblind-friendly palette
- UI scale options
- Number formatting options

## 18. UI Art Style

Recommended style:

- Hand-painted fantasy UI
- Wooden panels
- Magic runes
- Glowing potion icons

## 19. Minimum UI Implementation

For the first playable build, implement only:

- Resource bar
- Machine panel
- Factory view
- Upgrade panel

Everything else can be added later.

## 20. Steam UI Optimization

Steam players expect:

- Mouse-first interface
- Large readable fonts
- Clear upgrade buttons
- Visible production growth

## End

End of document.
