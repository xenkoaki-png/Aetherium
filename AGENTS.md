# Aetherium Multi-Agent Operating Model

This repository uses a 10-agent loop designed to move from idea to implementation to review without losing the master vision.

## Agent roster

1. Builder Alpha — core gameplay and combat systems
2. Builder Beta — world, encounters, and mission systems
3. Builder Gamma — progression and polish
4. Reviewer Echo — compare against online survival/RPG reference quality
5. Reviewer Foil — validate adherence to the master design doc and issue prompt
6. Reviewer Zenith — final technical correctness and regression risk review
7. Code Monkey One — mechanical implementation and core wiring
8. Code Monkey Two — bulk implementation and follow-up integration fixes
9. OverViewer — system-level strategic synthesis and next-step direction
10. Pager — continuous research, idea generation, and feed-forward to every other agent

## Execution flow

### Stage 0: Research and direction
- The Pager gathers ideas, references, and design prompts.
- The OverViewer checks the current work against the master brief and issue prompt.
- The team decides the next concrete task.

### Stage 1: Implementation
- Builder Alpha creates the core patch.
- Builder Beta handles the world/system layer.
- Builder Gamma improves progression and usability.
- Code Monkey One and Code Monkey Two execute the mechanical wiring and repetitive fixes required to support the patch.

### Stage 2: Review
- Reviewer Echo tests the patch against reference-game quality expectations.
- Reviewer Foil checks the patch against the master document and prompt constraints.
- Reviewer Zenith performs final technical review and catches regressions.

### Stage 3: Synthesis
- The OverViewer compares the whole result with the project brief and decides whether the feature is complete, needs tuning, or must be revised.
- The Pager proposes the next improvement track and feeds it to the builders.

## Foundation gate and priority order

The grounded-world and collision requirements in `AETHERIUM_MASTER_DOCUMENT.md` are a blocker gate. If ground contact, collision, camera, or project integrity is broken, fix that foundation first. Do not layer more gameplay or cosmetic work onto a broken baseline.

After the foundation gate passes, continue in this sequence:

1. visual consistency
2. world variation
3. player readability and combat loop

This order is the hard priority for the working loop. Everything else is subordinate until those three goals are meaningfully improved.

## Quality bar

Every patch should be judged by the following questions:

- Does it make the game more readable?
- Does it improve the world’s distinctive identity?
- Does it make combat and survival feel better?
- Does it align with the Aetherium master blueprint?
- Does it meaningfully improve the game beyond the reference foundation?

## Decision rule

If a feature does not improve one of those goals, it should be rejected or deferred.

## Recommended usage

Use the agent files in `.github/agents/` as Copilot specialist roles during iterative development. The OverViewer should own the final route and the Pager should constantly supply the next wave of ideas. Keep project status in `AI_HANDOFF.md` so Gemini in an IDE or another assistant can continue without access to this chat or these custom agents.

## Run an Iteration

In VS Code Chat, run `/aetherium-iteration` from `.github/prompts/aetherium-iteration.prompt.md`. Add a focused feature or bug-fix request as the prompt input; with no input, the OverViewer selects the next unfinished item after checking the foundation gate and then the 1-2-3 priority order. For Gemini or other IDE assistants, start with `AI_HANDOFF.md` and the relevant design/code files.

The prompt coordinates the Pager, OverViewer, three Builders, two Code Monkeys, and three Reviewers through scoped handoffs, independent review, focused verification, and a final synthesis. It must report when delegation or Unity runtime evidence is unavailable rather than claiming those steps ran. Run one bounded feature slice per invocation and update `AI_HANDOFF.md` with verified results. The requested sync target is the official `main` branch: before syncing, verify the remote, branch, worktree, and that the update can be applied without rewriting or overwriting history. Never force-push or create release tags as part of a routine iteration. If safe direct sync is blocked, report the reason instead of changing branches or publishing elsewhere without approval.
