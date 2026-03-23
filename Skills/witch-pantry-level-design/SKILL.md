---
name: witch-pantry-level-design
description: Design or review Witch Pantry room layout, machine placement, flow, wayfinding, adjacency, progression gating, and environmental readability. Use when shaping pantry rooms, unlockable spaces, machine anchors, player routing, visual hierarchy, or any spatial decision that affects clarity, pacing, player attention, screenshot readability, or the cozy-production fantasy.
---

# Witch Pantry Level Design

Use this skill to keep Witch Pantry's spaces readable, productive, and marketable in screenshots as well as play.

## Read First

1. Read:
   - `Docs/Spatial-Pantry-System.md`
   - `Docs/Experience-Pillars.md`
   - `Docs/GDD`
2. If the task is about rooms or navigation, also read `Skills/witch-pantry-spatial-layout/SKILL.md`.
3. Read `references/sources.md` when you need supporting theory.

## Core Workflow

1. Define the room job in one sentence.
2. Map the critical player loop through the space:
   - entry
   - main interaction points
   - output/reward read
   - optional detours
3. Identify what the player must understand at a glance:
   - what this room is for
   - where to click next
   - what is blocked
   - what is producing value
4. Decide which guidance is:
   - direct
   - environmental
   - diegetic
   - UI-assisted
5. Check the room from three cameras:
   - active play
   - idle glance
   - screenshot/store-readability

## Level Design Rules

- Prefer one dominant room function per space.
- Make the production chain legible from shape, placement, and motion before relying on labels.
- Use landmarks, contrast, and orientation cues so players can build a mental map quickly.
- Use adjacency to create meaningful layout decisions, not meaningless shuffling.
- Keep friction readable and recoverable; confusion is not challenge.
- Reserve complexity for later rooms rather than flooding the first room.
- Every unlock should improve either throughput, readability, or routing fantasy.
- If a layout looks clever in a diagram but noisy in a screenshot, it is not finished.

## Pantry-Specific Heuristics

- The main pantry should teach the fantasy through visible ingredient-to-potion flow.
- Machine anchors should support fast scanning of inputs, active machines, bottlenecks, and outputs.
- Special rooms should justify their existence with a different decision pattern, not just more floor area.
- Navigation between rooms must feel like progression, not menu tax.
- Compact desktop-idler mode must preserve the room's key signals without requiring hover archaeology.

## Smells To Call Out

- routes that cross for no good reason
- upgrade clutter that hides the core loop
- dead corners with no interaction, fantasy, or future unlock purpose
- guidance that depends on tutorial text because the room itself is unreadable
- layout decisions driven by asset convenience instead of play flow
- rooms that cannot produce a strong screenshot silhouette

## Good Output Shape

End with:

- room purpose
- critical path
- guidance/readability plan
- unlock or expansion hooks
- key risks to pacing or readability
