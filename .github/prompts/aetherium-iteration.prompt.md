---
name: Aetherium Iteration
description: "Run one complete Aetherium development cycle through research, scoped implementation, independent review, verification, and next-step selection."
argument-hint: "Describe a feature or bug to work on; leave empty to continue the next project priority."
agent: "Aetherium OverViewer"
---

Run one complete, evidence-based Aetherium development cycle. Treat the user's message accompanying this prompt as the requested focus. If none is provided, read `AI_HANDOFF.md`, inspect the repository, and choose the next unfinished item after checking the foundation gate and then the mandatory priority order below. Do not ask for routine decisions; ask only when a genuine blocker requires user input.

## Foundation gate and priorities

The grounded-world and collision requirements in `AETHERIUM_MASTER_DOCUMENT.md` are a blocker gate. If ground contact, collision, camera, project integrity, or the ability to open and test the project is broken, fix that foundation first. Do not begin cosmetic polish or expand gameplay on top of a broken baseline.

After the foundation gate passes, work in this order and do not advance past an incomplete higher priority:

1. Visual consistency
2. World variation
3. Player readability and combat loop

A requested urgent bug fix may be handled immediately, but return to the sequence afterward. Choose one bounded, testable slice for this run rather than expanding into unrelated features.

## Operating rules

- Read [the repository operating model](../../AGENTS.md), [the portable project handoff](../../AI_HANDOFF.md), the Aetherium master document, and the smallest relevant code/test surface before deciding what to change.
- Check the current worktree and branch first. Preserve all user changes; never revert unrelated work or rewrite history.
- Use the ten custom agents in `.github/agents/` as real delegated roles. When subagent delegation is available, invoke each named role below and pass it the task brief plus the relevant files and constraints. Keep the sequence below; do not claim a role ran unless it actually did.
- If delegation is unavailable, state that limitation and proceed as the lead agent using each role's checklist in sequence. Do not imply that separate agents reviewed the work.
- Keep `AI_HANDOFF.md` useful for Gemini in an IDE and other assistants: update it with verified progress, remaining blockers, exact checks, and the next task. Do not make the handoff depend on access to this chat or Copilot custom agents.
- The requested sync target is the official `main` branch. Before syncing, verify the remote, branch, worktree, and that the update can be applied without overwriting concurrent work or rewriting history. Do not force-push, silently switch branches, or create release tags during routine development. If a safe direct sync is blocked, report the reason instead of publishing to another target.
- Handoffs must include the current task, decisions already made, relevant paths, constraints, and expected output. Keep each agent focused on its role.
- Preserve the existing architecture and art direction. Use existing tools, tests, and conventions. Do not add a dependency or make unrelated cleanup without a concrete need.
- For external game/reference research, use reliable, relevant sources when browsing is available and include URLs with specific takeaways. Clearly label unverified ideas; research must not override the Aetherium brief.
- Do not fabricate Unity builds, playtests, screenshots, browser checks, or external research. Capture and attach a gameplay screenshot when the environment can actually run the game; otherwise state why screenshot evidence is unavailable.

## Agent chain

### 0. OverViewer: scope and route
Inspect the current state and handoff. First decide whether a foundation blocker prevents meaningful feature work; if so, scope the smallest blocker fix. Otherwise choose the next bounded task according to the priority order. Confirm that it improves a stated Aetherium goal. Produce a short task brief with acceptance criteria, risk boundaries, and likely code ownership. Stop the cycle if no worthwhile change is supported by evidence; report the missing evidence or blocker.

### 1. Pager: research and opportunities
Give the Pager the task brief. Ask it to find relevant design or technical references, inspect nearby project evidence, and return a small set of actionable findings with source links where applicable. Keep only findings that support the current priority and can affect this implementation.

### 2. OverViewer: implementation plan
Use the research and repo evidence to finalize scope. Define observable acceptance criteria, likely files, explicit non-goals, and non-overlapping ownership for implementation. Resolve conflicts with the master document before code changes.

### 3. Builders: feature implementation
In order, involve Builder Alpha (core gameplay/combat/survival), Builder Beta (world/encounter/mission), and Builder Gamma (progression/polish). Give each only the relevant subtask and ownership boundary. Have them implement justified changes and report files, behavior, assumptions, and checks. Keep their changes coordinated in the shared worktree; do not let overlapping edits overwrite one another. A builder may recommend no change when its domain is not part of this bounded task; never add gratuitous code to satisfy an agent count.

### 4. Code Monkeys: integration and mechanical work
In order, involve Code Monkey One and Code Monkey Two for necessary wiring, serialization, repetitive updates, or integration fixes identified by the builders. Require concrete file ownership and a focused task. They should not redesign gameplay or broaden scope. If no mechanical work is needed, record that and move on.

### 5. Reviewers: independent gates
After implementation, have each reviewer inspect the resulting diff and evidence independently:

- Reviewer Echo: compare gameplay quality and relevant references; return actionable quality gaps.
- Reviewer Foil: check compliance with the user's request, master document, and priority order.
- Reviewer Zenith: check correctness, edge cases, regressions, and test coverage.

Require findings to identify the affected behavior and severity. Do not accept vague approval. Where Unity visuals can be run, include screenshot evidence in the visual review; otherwise report the limitation. Fix substantive findings within scope, then rerun the focused checks and have the affected reviewer re-check the fix. Defer unrelated findings with a reason.

### 6. Verification and synthesis
Run the cheapest focused behavioral check first after edits, then any required narrow tests/build/type checks available in this environment. Report exact commands and outcomes; distinguish unavailable Unity Editor verification from successful checks. Update `AI_HANDOFF.md` with verified current status and the next task. Have the OverViewer decide whether acceptance criteria are met, whether the slice needs another iteration, or whether it is blocked. Ask the Pager for the next priority-ranked lead, but do not start a second feature in this run. If syncing is permitted by the verified branch/worktree state, push the validated change to the requested official `main` target and report the resulting commit; otherwise state what prevented sync. Never infer that a release was published from a routine code push.

## Final response
Keep the report concise and include:

- Priority and task completed (or the specific blocker)
- Main behavior changes and touched files
- Which roles actually ran and their key findings
- Focused validation results, including unavailable checks
- Screenshot evidence or why it could not be captured
- Handoff update and GitHub sync result (or the specific blocker)
- OverViewer's decision and the next item in the foundation → 1→2→3 sequence
