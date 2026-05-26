---
name: witch-pantry-spatial-layout
description: Design and review Witch Pantry room structure, machine placement, adjacency rules, camera framing, and multi-room progression. Use when planning pantry layouts, evaluating readability, defining room unlocks, or translating the spatial pantry system into implementation work.
---

# Witch Pantry Spatial Layout

## Purpose

Use this skill whenever a task touches rooms, placement, adjacency, room unlocks, camera framing, or pantry readability.

## Source of Truth

Read these docs first:

- `Docs/Spatial-Pantry-System.md`
- `Docs/Witchs-Pantry-Production-Blueprint.md`
- `Docs/Witchs-Pantry-UI-Design.md`

## Core Rules

- Start with one main pantry room.
- Keep one production chain readable in one view.
- Add connected specialty rooms later, not isolated production screens.
- Avoid giant unreadable mega-rooms.
- Keep shared inventory if it helps implementation, but preserve spatial meaning through room roles and adjacency.
- Make the focal machine, input source, bottleneck, and output/reward read clear before adding decorative density.
- Compact mode must preserve whole-pantry meaning; do not make the player babysit room tabs.

## Room Progression Model

- Prototype and demo: one main pantry room
- Early Access core: two to three connected rooms
- 1.0 normal end-state: three strongly themed rooms
- Avoid more than four actively managed primary rooms

## Footprint Rules

Use a small footprint vocabulary:

- `1x1` helper
- `2x1` station
- `2x2` focal machine
- edge-slot shelf or wall utility

Most machines should stay `1x1` or `2x1`.

## Layout Review Checklist

For any proposed room or scene, check:

- can a player identify the main workflow in a few seconds
- is there one dominant focal machine or cluster
- are bottlenecks readable without opening deep panels
- is there enough negative space
- does the room look like a magical workplace instead of a clutter pile
- does the layout produce a strong store-page screenshot or GIF

## Second Room Trigger

The first secondary room should unlock only when:

- the starter room already proves the full chain
- the player has interacted with adjacency meaningfully
- new content would otherwise create ugly crowding or unreadability

## Camera Rules

- prefer stable high three-quarter or readable isometric framing
- keep default zoom bounded
- room navigation should feel local and quick
- do not rely on free camera wandering as the main interaction model

## Compact Mode Rules

Compact mode should summarize:

- current demand
- total production
- worst bottleneck
- room needing attention
- one to three quick actions

It should not force room micromanagement.

## Good Output Shapes

Use this skill when producing:

- room specs
- layout critiques
- placement rules
- adjacency proposals
- implementation tickets for room or layout systems
- room-readability pass/fail calls with concrete fixes
