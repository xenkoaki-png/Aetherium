# Aetherium AI Handoff

This file is the portable current-state note for any coding assistant, including Gemini in an IDE. Read it with `AGENTS.md`, `AETHERIUM_MASTER_DOCUMENT.md`, and the specific code you plan to change. Do not rely on an earlier chat transcript for current repository state.

## Current objective

Establish a reopenable, testable Unity 6 project and a stable playable scene. Treat ground, collision, and camera failures as blockers. Once that foundation passes, continue in order: visual consistency, world variation, then player readability and combat.

## Verified repository state

- Unity gameplay C# scripts exist under `Assets/Scripts/Core` and `Assets/Scripts/Gameplay`.
- `Packages/manifest.json` exists and includes URP 17.0.3 and the Input System package.
- Art direction references exist under `Art/Reference`.
- The current workspace inspection found no `ProjectSettings/` directory and no `.unity` scene files. Recheck the live Git tree before creating these.
- `AetheriumGameplayBootstrap` creates a floor and actors from primitives. Its nested `MainCameraRig` follows a target but the bootstrap does not create or assign a Unity `Camera` component.
- `AetheriumPlayerController` reads legacy `Input.GetAxisRaw` and key input. Verify the project's active input handling before changing controls.
- `.github/workflows/release-windows.yml` targets Unity `6000.0.40f1`; confirm the project/editor version matches before relying on CI.

## Validation status

- Source and documentation were inspected in the workspace. `AGENTS.md`, the iteration prompt, `README.md`, and this handoff were updated to share the same foundation-first priority rule and IDE-portable workflow.
- `AetheriumGameplayBootstrap` now positions and scales the player capsule to match its `CharacterController`, and ensures the camera rig has an enabled camera tagged `MainCamera`.
- VS Code reports no diagnostics in the edited Markdown or bootstrap C# file. This is not a Unity compile or runtime test.
- Unity Editor import, C# compilation, gameplay smoke test, screenshots, automated Unity tests, and Windows release workflow have not been verified in this handoff.
- Do not describe the game as playable or a release as successful until those checks actually pass.

## Next task

Audit the live repository for Unity project settings and scenes. The current workspace has no `ProjectSettings/` directory, `.unity` scene files, `.meta` files, or Unity Editor executable. If the live Git tree confirms those files are absent, restore the smallest valid Unity 6 project setup and a startup scene with a real camera, collidable ground, player, and light. Then open it in the matching Unity Editor, resolve compile errors, and test player grounding, wall collision, and camera follow. Do not start visual polish until this foundation gate passes.

## Development contract

1. Inspect the worktree and relevant files before editing; preserve unrelated or user-owned changes.
2. Work on one bounded slice with observable acceptance criteria.
3. Run the cheapest focused check immediately after editing, then broader tests/build/runtime checks that are available.
4. Report exact commands and outcomes. Mark Unity/runtime/screenshots/reviews unavailable when they were not performed.
5. Update this handoff with verified progress, remaining blockers, and the next task.
6. Never force-push, discard changes, or create a release tag as part of routine development. Confirm the remote, branch, worktree, and repository policy before syncing. Keep Windows releases separate from code sync.

## Reference paths

- Design intent: `AETHERIUM_MASTER_DOCUMENT.md`
- Implementation roadmap: `UNITY_IMPLEMENTATION_PLAN.md`
- Acceptance checklist: `FIRST_PLAYABLE_CHECKLIST.md`
- Milestone context: `FIRST_PLAYABLE_MILESTONE_1.md`
- Copilot workflow: `.github/prompts/aetherium-iteration.prompt.md`
- Windows release process: `RELEASE_BUILD.md`
