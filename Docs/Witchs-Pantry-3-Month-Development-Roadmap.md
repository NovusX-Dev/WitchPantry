# Witch's Pantry Automation 3-Month Development Roadmap

- Engine: Unity 6000.3
- Platform: Steam (Windows)
- Total Duration: 12 weeks

Goal: ship a polished incremental automation game with replayability and prestige mechanics.

## Development Strategy

The project is divided into four phases:

1. Pre-production
2. Core systems
3. Content expansion
4. Polish and launch preparation

## Month 1: Core Gameplay Foundations

Goal: build the gameplay backbone.

### Week 1: Project Setup

Tasks:

- Create Unity project
- Implement core folder structure
- Create `GameLoopManager`
- Implement tick-based simulation
- Create basic UI layout
- Create debug console

Milestone: basic project running with tick simulation.

### Week 2: Production Graph System

Tasks:

- Create `ProductionNode` class
- Create machine instances
- Implement resource buffers
- Create node connections
- Add example machines

Example machines:

- Herb Farm
- Grinder
- Cauldron

Milestone: first automated potion production chain works.

### Week 3: Economy System

Tasks:

- Implement potion selling
- Implement cost scaling formula
- Add machine purchasing
- Add upgrade multipliers

Formula:

```text
cost = baseCost * growthRate^owned
```

Milestone: game can run for 20 minutes without breaking.

### Week 4: Save and Offline Progress

Tasks:

- Implement `SaveData` structure
- Create JSON save system
- Add auto-save
- Implement offline progression simulation

Milestone: game can be closed and resumed with progress.

## Month 2: Content Expansion

Goal: turn the prototype into a real game.

### Week 5: Machine Variety

Add machines:

- Mushroom Farm
- Mortar Golem
- Fermentation Barrel
- Bottling Sprite

Add machine upgrades:

- Speed
- Capacity
- Efficiency

Milestone: 5-machine production chains.

### Week 6: Potion System

Add potion types:

- Healing Potion
- Mana Potion
- Stamina Potion
- Luck Potion

Add rarity system:

- Common
- Rare
- Epic
- Legendary

Milestone: potion diversity system functional.

### Week 7: Contracts System

Village requests potions.

Example:

- Deliver 50 healing potions
- Reward gold or rare ingredients

Milestone: contracts drive gameplay progression.

### Week 8: Prestige System

Implement prestige reset.

Add bonuses:

- Faster brewing
- More valuable potions
- Rare ingredient drops

Milestone: complete gameplay loop.

## Month 3: Polish and Release Preparation

Goal: prepare for Steam launch.

### Week 9: UI Polish

Improve:

- Machine management UI
- Inventory UI
- Contracts UI

Add:

- Animations
- Sound effects

Milestone: game feels visually satisfying.

### Week 10: Economy Balancing

Run the economy simulation tool.

Adjust:

- Machine cost curves
- Potion values
- Prestige scaling

Target pacing:

- First prestige: about 2 hours

Milestone: economy is stable.

### Week 11: Steam Preparation

Tasks:

- Create Steam store page
- Produce trailer
- Capture screenshots
- Create capsule art
- Write store description
- Start collecting wishlists

Milestone: Steam page live.

### Week 12: Final QA and Launch

Tasks:

- Bug fixing
- Performance testing
- Steam build upload
- Launch day marketing

Milestone: game released.

## Estimated Development Time

| System | Time |
| --- | --- |
| Production Graph | 4 days |
| Economy | 3 days |
| Save System | 2 days |
| UI | 10 days |
| Content | 14 days |
| Polish | 10 days |

Total: about 8 to 10 weeks.

## Recommended Launch Metrics

Goals for first launch:

- 500 to 1000 wishlists
- 10 to 20 percent sales conversion
- 50 to 200 expected launch sales

## Post-Launch Plan

Week 13 and beyond:

- Add content updates
- New potions
- New machines
- Seasonal events
- Steam discounts

## Long-Term Vision

Add expansions such as:

- Alchemy guild
- Potion trading market
- Magic automation network
- Multiplayer potion trading

## End

End of document.
