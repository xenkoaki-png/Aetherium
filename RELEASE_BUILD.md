# Aetherium Release Build

This project is set up for a GitHub Actions release flow that produces a Windows standalone executable for download from the GitHub Releases page.

## Release flow

1. Push a version tag such as `v0.1.0`.
2. GitHub Actions runs the build workflow in `.github/workflows/release-windows.yml`.
3. Unity builds the Windows executable in StandaloneWindows64 mode.
4. The workflow zips the output and publishes it as a GitHub release asset.

## Required GitHub secrets

Add these to the repository secrets before creating a release:

- `UNITY_LICENSE`
- `UNITY_EMAIL`
- `UNITY_PASSWORD`
- `UNITY_SERIAL`

If you use a Unity personal license, the `UNITY_LICENSE` value is typically your Unity activation license file content or the needed license secret expected by the game-ci workflow. The exact secret format depends on your Unity licensing setup.

## Local tag example

```bash
git tag v0.1.0
git push origin v0.1.0
```

This triggers the GitHub release workflow.

## Notes

- The build currently targets Windows 64-bit as a standalone executable.
- The project should be opened in Unity 6000.x for the most direct compatibility with the workflow.
- If the game uses a different Unity version, change `unityVersion` in `.github/workflows/release-windows.yml` to match the project.
