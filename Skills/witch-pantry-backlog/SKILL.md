---
name: witch-pantry-backlog
description: Create, update, and reorganize Witch Pantry GitHub issues and project-board fields consistently. Use when changing issue titles/bodies, adding new implementation slices, updating labels, or mapping work to Track and Release Target without creating backlog drift.
---

# Witch Pantry Backlog

## Purpose

Use this skill to keep the GitHub backlog and project board aligned with the actual product direction.

## Before Editing Issues

Read the current source of truth:

- `Docs/Product-Strategy.md`
- `Docs/Spatial-Pantry-System.md`
- `Docs/Customer-Demand-and-Contracts.md`
- `Docs/Demo-and-Early-Access-Plan.md`

Check the current issue body before rewriting it. Do not retitle a ticket into something the body no longer supports.

## Issue Update Workflow

1. Decide whether the issue should be:
   - kept as-is
   - retitled and rewritten
   - split
   - replaced by a new issue
2. Preserve issue numbers when intent is still substantially the same.
3. Rewrite title and body together when the design meaning changes.
4. Keep the standard body structure:
   - Category
   - Description
   - Player Value
   - Design Reference
   - Acceptance Criteria
5. Add labels only when they improve filtering.

## Board Rules

Use these project fields consistently:

- `Track`
  - `Core Simulation`
  - `Spatial UX`
  - `Desktop Mode`
  - `Economy/Progression`
  - `Content`
  - `Marketing/Release`
- `Release Target`
  - `Prototype`
  - `Demo`
  - `Early Access`
  - `1.0`
  - `Post-Launch`

## Label Rules

Use these labels when applicable:

- `demo` for work required to ship and validate the public demo slice
- `desktop-mode` for compact-mode or side-screen work
- `spatial-layout` for room layout, placement, adjacency, and readability work

Do not add labels just because they are available.

## Backlog Principles

- Prefer specific implementation slices over vague umbrella tickets.
- Prefer one clear ownership boundary per issue.
- Keep issue scope compatible with milestone and release target.
- When the product direction changes, update the affected issue bodies, not only the titles.
- Avoid leaving stale assumptions in acceptance criteria.

## Good Output Shapes

When using this skill, produce:

- clean issue rewrites
- minimal, justified new issues
- updated board field values
- a short summary of what moved and why
