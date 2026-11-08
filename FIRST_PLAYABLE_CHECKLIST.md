# Aetherium First Playable Checklist

## Goal

Build a small but fully coherent slice of the game where the player can move, survive, gather, craft, and react to a dangerous world without engine-breaking bugs.

## Core checklist

### World and movement

- [ ] terrain has a valid height model
- [ ] player stands on the ground without sinking
- [ ] walls and obstacles block movement properly
- [ ] stairs and ramps work without clipping
- [ ] camera follows the player without clipping through geometry

### Interaction and world logic

- [ ] items can be picked up reliably
- [ ] interactables are aligned to world-space positions
- [ ] triggers and hitboxes match what the player sees
- [ ] debug collision visuals are available

### Survival loop

- [ ] hunger or thirst exists and affects the player
- [ ] resources can be gathered from the environment
- [ ] crafting or simple upgrades function
- [ ] danger or enemy pressure exists in the scene
- [ ] a shelter or safe-zone mechanic is active

### UI and feedback

- [ ] inventory works and displays correctly
- [ ] status bars or meters are readable
- [ ] player feedback for damage, hunger, and crafting is clear
- [ ] save and load works for a basic world state

### Quality gates

- [ ] no clipping through floor or walls
- [ ] no camera collision failures
- [ ] no broken item placement
- [ ] no major freeze or invalid state on save/load
- [ ] small play session feels coherent and fun

---

This is the minimum proof that Aetherium has turned from a broken prototype into a genuine game slice.
