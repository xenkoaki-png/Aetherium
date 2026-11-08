# Aetherium
A Unity Isometric Game with Sci-fi RPG Space opera And fantasy Isekai

## Master Project Brief

- [AI Handoff](./AI_HANDOFF.md) — current verified project state and the next task for Copilot, Gemini, or another IDE assistant.
- [Aetherium Master Document](./AETHERIUM_MASTER_DOCUMENT.md) — the technical and design document covering the current prototype issues, root causes, and the plan to make Aetherium better than the reference backend foundation.
- [Unity Implementation Plan](./UNITY_IMPLEMENTATION_PLAN.md) — the practical milestone roadmap for building Aetherium in Unity while keeping the design ambitious and grounded.
- [First Playable Checklist](./FIRST_PLAYABLE_CHECKLIST.md) — the minimum proof that the first functional survival slice is working.

## Release build

- [RELEASE_BUILD.md](./RELEASE_BUILD.md) — build instructions for generating a Windows release executable and publishing it to GitHub Releases.

The repo includes a GitHub Actions workflow at [.github/workflows/release-windows.yml](.github/workflows/release-windows.yml) that builds a Windows 64-bit standalone EXE and uploads it as a release asset when a version tag is pushed.

### Release trigger

```bash
git tag v0.1.0
git push origin v0.1.0
```

Then the GitHub release page will contain the downloadable Windows zip package.
