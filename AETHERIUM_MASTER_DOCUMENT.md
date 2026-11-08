# Aetherium Master Document

## 1. Problem Statement

The current Aetherium prototype exhibits multiple core issues that make the experience feel unfinished or broken:

- the rendered world acts like a single flat panel rather than a real spatial environment
- the player character intersects or clips through the floor
- objects and collision volumes do not align with the world geometry
- the camera and movement logic are not grounded in a consistent world height model
- gameplay systems are being layered on top of a foundation that is not stable

This is not a cosmetic issue. It is a foundational gameplay and technical issue. Until the movement, collision, and physical world logic are fixed, the project cannot be meaningfully expanded into a larger survival RPG.

---

## 2. Project Goal

Aetherium should become a rich sci-fi/fantasy survival RPG with:

- believable world space and depth
- player movement that feels grounded and physical
- interactive, destructible, and navigable environments
- survival systems, progression systems, and narrative content
- a world dense enough to feel like a lived-in universe, not a flat mockup

The target ambition is comparable in complexity to games like RimWorld and Star Citizen in terms of systems depth, but the first milestone is simply to make the world behave correctly.

---

## 3. Core Issues to Fix First

### 3.1 Single-panel world rendering

The environment must not behave like a single flat floor panel.

Fix requirements:

- actual terrain depth and height variation
- proper walls, ledges, stairs, and interior spaces
- object placement in world space instead of a flat plane
- camera and visibility logic that respects geometry
- lighting and shading that work with true depth

### 3.2 Character clipping through floor

This is the most obvious issue and usually indicates one or more of the following:

- wrong player collider height
- floor height mismatch
- the mesh root is offset from the true character pivot
- movement code is updating world position without matching collision data
- animation root motion is adding vertical displacement
- the environment is missing a proper walkable plane

This must be corrected at the root before any content scaling or feature work continues.

### 3.3 Collision mismatch between mesh and logic

If the rendered floor looks fine but the character sinks through it, then the collision mesh and visual mesh are not synchronized.

Requirements:

- every walkable surface must have collision data
- every non-walkable obstacle must be blocked by collision
- character capsule or rigid body must match the actual height and volume
- object placement should fail if it is not valid in world space

### 3.4 Camera and visual instability

A camera that passes through the ground or seems detached from the player is usually caused by transform mismatch or wrong follow logic.

Fix requirements:

- camera rig anchored to the proper player root
- collision-safe camera placement
- no camera penetration through walls or floor
- proper third-person offset and clipping behavior

---

## 4. Root Cause Categories

### 4.1 Transform mismatch

Symptoms:

- model is offset from world position
- player appears half in the floor
- interaction targets are wrong

Fix:

- standardize all object transforms to a single world coordinate system
- verify root, pivot, and mesh origin values
- test transform parenting for characters, props, and cameras

### 4.2 Collision data not aligned with visuals

Symptoms:

- visible floor but character falls through
- objects can be placed inside walls
- hit detection does not match what the player sees

Fix:

- create explicit collision volumes for every major type of surface
- validate with debug draw modes
- test movement against floor, ramps, and platforms

### 4.3 World generation shortcuts

Symptoms:

- everything is one plane or one room
- no natural height variation
- player can walk across mock geometry without a consistent floor

Fix:

- generate real terrain layers
- create walkable surfaces with proper bounds
- add environment details only after the base world is physically valid

### 4.4 Procedural complexity without foundation

Symptoms:

- large systems exist but the base feel is broken
- players notice the bug before they notice the content

Fix:

- stabilize the core world and character first
- then iterate on survival systems, NPC behavior, and economy

---

## 5. Immediate Fix Order

### Phase 1: Make the world physically valid

1. Fix the floor and ground height system.
2. Verify the player collider is correct.
3. Match character mesh origin with gameplay root.
4. Add reliable collision checks for walls and floors.
5. Test walking, jumping, stairs, and edge transitions.

### Phase 2: Make the environment feel real

1. Add layered terrain and elevation.
2. Create valid rooms, walls, and platforms.
3. Fix camera collision and follow logic.
4. Ensure interaction hitboxes match visible objects.

### Phase 3: Add gameplay systems

1. inventory and equipment
2. survival stats and progression
3. crafting and materials
4. NPC behavior and faction logic
5. world events and broader world simulation

---

## 5A. Epic Priority 1: Grounded World and Collision Foundation

This is the first and most important epic. If the player cannot stand on the world without clipping, the rest of the game is built on a broken foundation.

### Objective

Build a world that feels physically understandable and mechanically consistent. The player must walk, climb, interact, and survive without sinking through the floor or fighting geometry that does not match the visuals.

### Deliverables

- valid terrain height and walkable surface generation
- player collider, root, and mesh alignment
- proper camera follow and camera collision behavior
- stairs, ramps, ledges, and walls with correct traversal logic
- debug tools for collision, floor height, and interaction volume validation

### Non-negotiable rules

- no character may clip through the floor
- no object may be visually placed without valid collision data
- no large gameplay system may be added before movement and terrain are stable
- no world generation step may ship with fake geometry or invisible walkable surfaces

### Implementation pillars

1. Ground integrity
   - centralize height and floor data
   - ensure every walkable surface resolves to a valid world height
   - create transition logic for ramps, stairs, and elevated platforms

2. Character integrity
   - align the capsule or body collider with the visible character mesh
   - verify root transform origin, feet position, and animation offset
   - validate horizontal and vertical movement against the actual ground surface

3. Camera integrity
   - clamp camera distance using world collision checks
   - prevent camera collision with walls, floor, and nearby props
   - maintain proper framing during movement and uneven terrain traversal

4. Interaction integrity
   - ensure players can interact with visible objects in the actual world space
   - align hitboxes, trigger volumes, and UI targets with the same coordinate system
   - use debug visualization to catch mismatched world collisions immediately

### Success benchmark

The player should feel as if they are standing in a real environment, not a flat prototype. The ground should hold them, the camera should follow naturally, and all world interactions should behave according to the world itself.

---

## 5B. Epic Priority 3: The Living World, Systems, and Emergent Storytelling

This is the large-scale layer that elevates Aetherium from a technical prototype into a believable survival universe.

### Objective

Create a deep, reactive, living world in which survival, progression, faction pressure, and environmental danger all matter together. The world should not be a backdrop; it should be a system that produces stories.

### Core pillars

1. Survival depth
   - hunger, thirst, temperature, fatigue, illness, and environmental risk
   - resource scarcity and long-term survival planning
   - meaningful crafting and maintenance loops

2. Character progression
   - skills, body states, factions, status effects, and equipment specialization
   - progression tied to playstyle and environment
   - long-term consequences for actions and failures

3. Faction and social simulation
   - AI settlements, rival survivors, gangs, hostile enclaves, and lawless zones
   - negotiation, conflict, trade, and territory pressure
   - social systems that generate emergent stories rather than static quests

4. Economy and resource flow
   - scarcity-driven supply chains and trading behavior
   - logistics and route planning between settlements and hazardous regions
   - resource value shaped by player actions and world conditions

5. Story emergence
   - events caused by simulation, not just by fixed scripts
   - world state changes that alter exploration, danger, and progression
   - memorable outcomes that arise from survival logic instead of only content triggers

### Design principle

Every system should feed into others. A survival choice should affect combat, factions, economy, narrative, and the state of the world.

### Success benchmark

The player should feel like they are living inside a recurring, evolving universe rather than just moving through a scene with a checklist of tasks.

---

## 5C. Using CDDA as the Backend Reference, Not a Copy

CDDA is useful as a reference backend because it has deep, mature systemic logic for survival gameplay and emergent consequences. But Aetherium should evolve beyond it with stronger physical correctness, stronger gameplay presentation, and more modern immersion.

### What Aetherium should take from CDDA

- survival-first design philosophy
- modular crafting and resource logic
- large content ecosystem structure
- progression and world-state thinking
- broad systems that reward long-form play

### What Aetherium should improve beyond CDDA

- more polished world grounding and player movement
- stronger visual coherence between world, camera, and interaction
- better narrative pacing and UX
- more modern, immersive sci-fi/fantasy presentation
- clearer connection between world systems and player feedback

The goal is not to imitate CDDA. The goal is to absorb the strongest ideas and then build a cleaner, better, more grounded Aetherium experience.

---

## 6. Examples of Easy Bug Fixes That Must Be Done Early

### 6.1 Player sinks into floor

Likely causes:

- wrong capsule height
- ground detection offset
- vertical animation mismatch
- incorrect character placement due to scaling

Fix path:

- set character root at feet level or valid ground anchor
- ensure collision height is consistent with the visible model
- test with debug ground probe and height visualization
- verify no duplicate vertical offset from animation or movement scripts

### 6.2 Character floats above ground

Likely causes:

- ground detection is disabled or using wrong reference point
- character controller is ignoring slope or height map
- camera follow is pushing the player upward

Fix path:

- sample ground height from the closest valid surface
- clamp the player position to the ground plane
- ensure root update logic does not override ground movement

### 6.3 Flat single-room world

Likely causes:

- prototype was built as a single plane scene
- no vertical world data
- no proper walls or geometry branching

Fix path:

- build proper room structure and terrain blocks
- add height variation and floor layers
- create a walkable map with obstacles and height transitions

### 6.4 Camera intersects floor or walls

Likely causes:

- camera raycast not checked
- follow distance not clamped
- collision layer mismatch

Fix path:

- implement camera collision test against world obstacles
- lerp to safe distance when the camera enters blocked space
- adjust follow offset for different terrain height values

### 6.5 Interactions fail because hitboxes are wrong

Likely causes:

- object pivot offset
- raycast origin mismatch
- UI and world coordinates are not aligned

Fix path:

- centralize all interaction origins in world space
- validate click and use logic against visible geometry
- ensure transforms are consistent between editor and runtime

---

## 7. Architecture Requirements

### 7.1 World space must be authoritative

The game should have one clear world coordinate system. Every character, item, object, and camera should resolve from that world state.

### 7.2 Collision and render must be checked together

Visual correctness and gameplay correctness are not separate jobs. They must be tested as one system.

### 7.3 Debugging tools should exist early

A working game needs tools like:

- show collision volumes
- show floor height debug
- show interactable bounds
- show pathing and navmesh data
- log object transforms and position sanity checks

Without these, a project of this complexity will accumulate hidden bugs.

---

## 8. Scope Expectations

Aetherium should not be treated as a small single-scene prototype. It should be designed as a full game ecosystem. That includes:

- survival systems
- progression and leveling
- crafting and resource loops
- base building and settlement management
- combat, exploration, and danger
- narrative and faction systems
- world simulation and emergent play

However, none of that should be added before the fundamentals are stable.

---

## 8A. Epic Priority 2: Core Gameplay Loop and Survival Systems

This phase turns the world from an empty shell into a playable game loop. The objective is to make the player feel capable, threatened, and invested.

### Core gameplay loop

- gather resources from the world
- craft or repair tools and gear
- survive environmental hazards and enemy pressure
- manage body state, inventory, and time
- return to a safer base or shelter state
- make difficult choices under scarcity

### Key systems

1. Survival economy
   - food, water, sleep, temperature, stamina, and health as primary loops
   - scarcity pressure and context-sensitive resource decisions
   - risk/reward mechanics for exploration and gathering

2. Combat and threat
   - melee, ranged, stealth, and danger perception
   - enemy variety with real behavior patterns
   - resource cost and risk behind every fight

3. Inventory and progression
   - weight, equipment slots, tool quality, and item rarity
   - meaningful upgrades and build variants
   - skill growth tied to repeated real-world actions

4. Base and shelter logic
   - safe shelter, crafting stations, storage, defense, and maintenance
   - interior spaces that matter mechanically and socially
   - settlement-building loops built on survival consequences

### Deliverables

- stable survival drumbeat
- meaningful player choices under pressure
- progression that matters
- layered resource management with consequences

### Success benchmark

The player should feel like they are surviving, adapting, and making meaningful trade-offs, not simply walking through a scene and collecting items.

---

## 8B. Engine Strategy: Own Engine vs Existing Engine

A custom engine is an exciting long-term goal, but it should not be treated as the first milestone. Building a serious custom engine from scratch is a huge effort that requires rendering, physics, networking, tooling, editor systems, and content pipelines.

### Practical approach

- Use an existing engine as the production base for a first playable version.
- Build Aetherium’s core architecture to be modular and engine-agnostic.
- Define the game’s simulation layer so the logic can eventually be ported or optimized.
- Treat a custom engine as a long-term, high-risk expansion path, not a blocker for the game itself.

### Why this matters

The game must ship or at least become a stable, testable simulation. An engine can be built over time; a broken world cannot be patched by opinion.

---

## 9. Quality Gates for Completion

The project is not genuinely playable until all of the following are true:

- the player can walk without clipping into the floor
- walls and obstacles block movement correctly
- the camera remains stable and aligned with the player
- the world has actual depth and valid geometry
- items and interactions work in world space
- loading, saving, and world state remain consistent
- core gameplay systems are functional in a real environment

---

## 10. Reference Direction

The project should aim for the feel and systemic depth of:

- RimWorld: emergent survival and colony storytelling
- Star Citizen: large-scale sci-fi mood, world density, and player-driven immersion

But the actual implementation must first match the basic rules of a grounded, believable game world.

This document is the master reference for Aetherium’s technical and gameplay foundation, and it should be treated as the checklist for fixing the project’s early issues before expansion.
