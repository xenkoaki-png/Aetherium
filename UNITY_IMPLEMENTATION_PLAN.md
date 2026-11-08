# Aetherium Unity Implementation Plan

## 1. Strategy

Aetherium should be built in Unity using a modular gameplay architecture that keeps the simulation logic separate from the rendering layer. This keeps the project flexible, testable, and easier to evolve into a more custom engine later if needed.

The immediate goal is not to build a full custom engine. The immediate goal is to create a stable, grounded, playable prototype that demonstrates the correct survival loop and live world behavior.

---

## 2. Engine Decision

### Recommended path

Use Unity as the production engine for the first playable milestone.

### Why

- fast iteration for prototype and gameplay debugging
- mature 3D tooling for collision, animation, terrain, and scene authoring
- easier team productivity while validating systems
- better collision tools and inspector/debug workflows
- reduces risk while the game architecture is still being established

### Long-term note

A custom engine can be explored later, but only after the gameplay loop, data model, and simulation architecture are proven stable. The custom engine should be a follow-up optimization and specialization effort, not the foundation of the first playable game.

---

## 3. Core Architecture

### 3.1 Separation of concerns

Keep these layers distinct:

- Gameplay layer: survival systems, world simulation, inventory, NPC logic, progression
- Data layer: item definitions, entity templates, world state data, recipes, factions, regions
- Simulation layer: time, weather, world events, AI, interactions, resource churn
- Rendering layer: camera, animation, VFX, lighting, shaders, UI presentation
- Input/controller layer: movement, interactions, controls, accessibility

### 3.2 World architecture

Use a world-state model built around:

- world tiles or chunks
- entity IDs and component-based data
- world generation seed and region definitions
- terrain height and collision data
- object and actor transform snapshots
- save/load serialization of key world state

This keeps the project from being derivative, while making it easier to borrow the strongest systems from CDDA without copying its structure blindly.

---

## 4. Production Milestones

## Milestone 1: Grounded Prototype

### Goal

Prove the world is physically stable and coherent.

### Required systems

- ground height sampling
- capsule or controller setup for player movement
- collision for floor, walls, and platforms
- camera collision and follow logic
- simplified environment with real height variation
- debug tools for world height and collision visibility

### Deliverable

The player can walk around a small scene without clipping through the floor, getting stuck, or fighting the camera.

---

## Milestone 2: Survival Loop

### Goal

Create a playable survival rhythm.

### Required systems

- hunger, thirst, fatigue, health, temperature, stamina
- gather resources from the environment
- craft tools and simple equipment
- basic enemy threat or disaster loop
- shelter or safe-zone logic
- inventory and equipment management

### Deliverable

The player can survive in a small region for a meaningful session without gameplay falling apart.

---

## Milestone 3: Living World

### Goal

Turn the world into something reactive and emergent.

### Required systems

- faction simulation
- AI behavior for NPCs and hostile groups
- settlement or base simulation
- weather and region effects
- dynamic world events
- progression tied to world state and survival choices

### Deliverable

The game creates stories through simulation, not just scripted objectives.

---

## Milestone 4: Deep RPG Systems

### Goal

Add long-term play depth and broad system payoff.

### Required systems

- detailed progression and skill systems
- crafting trees and quality variations
- equipment specialization and deeper combat loops
- narrative arcs tied to exploration and settlements
- scaling complexity without breaking usability

### Deliverable

A substantial survival RPG with replayable variation and high emergent depth.

---

## 5. Unity Systems to Prioritize

### 5.1 Character controller and movement

- player root transform
- collider height and ground alignment
- animation root motion handling
- movement smoothing and acceleration
- slope handling and edge detection

### 5.2 Terrain and world generation

- terrain height map or voxel-like height data
- walkable surface detection
- object placement validation
- generation rules for region variety
- non-walkable obstacles and boundaries

### 5.3 Interaction system

- world-space interaction raycasts
- item pickup, placement, and use logic
- communication between UI and world object state
- consistent actor interaction zones

### 5.4 Combat and AI

- hit detection simplified but reliable
- enemy awareness and pursuit states
- damage and armor handling
- basic behaviors before full complex AI

### 5.5 Save/load system

- serialize player state
- serialize world state for placed objects and generated zones
- stable save transitions and recovery paths
- corruption detection and versioning

---

## 6. Gameplay Pillars

### Pillar 1: Grounded physicality

The player must feel stable and real in the world.

### Pillar 2: Hard survival pressure

The player should never be comfortable for too long without consequences.

### Pillar 3: Meaningful progression

Every upgrade must change how the player interacts with the world.

### Pillar 4: Emergent narrative

The world should generate stories and consequences without needing a rigid story script for every event.

### Pillar 5: Replayability

No two runs should feel identical.

---

## 7. Debug Workflow and QA

### Essential debug tools

- show colliders
- show ground height probes
- visualize interaction ranges
- show navmesh or pathing data
- show entity IDs and transforms
- log save/load world state

### QA gates

Every milestone should have a checklist:

- no floor clipping
- no broken camera transitions
- no impossible pathing or invalid movement
- no object pass-through
- no invalid save/load state
- no major gameplay break under expected stress

---

## 8. Recommended Immediate Tasks

1. Create a small test scene with a proper ground, walls, and stairs.
2. Fix player root, collider, and ground detection.
3. Add camera collision and follow rig.
4. Build a resource gathering loop.
5. Add a simple inventory system.
6. Add a basic survival stat controller.
7. Add a minimal enemy or threat loop.
8. Add a world event and save system.

These are the first practical steps toward a real, playable Aetherium.

---

## 9. Final Principle

The project’s first responsibility is to build a believable world. The second responsibility is to build a convincing survival loop. The third responsibility is to make the world feel alive.

If those three things are true, the game will have the foundation to become something truly ambitious.
