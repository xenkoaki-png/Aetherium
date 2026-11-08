# Aetherium Milestone 1: First Playable Survival Slice

AETHERIUM_MECH_2026_CLEANROOM_SIG

This milestone establishes the minimum playable loop for Aetherium in Unity without waiting for the full RPG economy and faction systems.

## Included
- A simple scene bootstrap that creates a floor, a player capsule, and a follow camera.
- A movement controller built around the isometric movement pattern used throughout the design docs.
- A survival manager tracking oxygen, hunger, and stamina.
- A clean-room fingerprint and basic Unity-compatible namespace structure.

## Core Goal
The player can move around the game space, maintain basic survival metrics, and experience a playable loop that can later be expanded with combat, vents, enemy encounters, and faction progression.

## Files
- `Assets/Scripts/Gameplay/AetheriumGameplayBootstrap.cs`
- `Assets/Scripts/Gameplay/AetheriumPlayerController.cs`
- `Assets/Scripts/Gameplay/AetheriumSurvivalManager.cs`

## Next Step
Use this milestone to add:
1. a basic collectible / loot loop,
2. hostile NPC wanderers,
3. interactive vent traversal,
4. a world-state HUD,
5. a saveable character profile tied back into `AetheriumPlayerStats`.

This is the foundation for the more ambitious living-world loop described in the master design files.
