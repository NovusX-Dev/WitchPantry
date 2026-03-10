# WitchPantry

Witch's Pantry is a Unity game project.

## Version Control

This repository uses Git for source control and Git LFS for large binary assets.

### Why Git LFS is used

Unity projects usually contain binary art and media files that are large and do not merge well in normal Git history. Git LFS keeps the repository lighter and avoids bloating commits with asset payloads.

Git LFS is currently configured through [.gitattributes](/C:/Unity/Repos/WitchPantry/.gitattributes) for:

- `*.psd`
- `*.blend`
- `*.fbx`
- `*.png`
- `*.wav`
- `*.jpg`
- `*.mp4`

Text-based Unity assets such as `*.unity`, `*.prefab`, `*.mat`, `*.asset`, and `*.meta` stay in normal Git so diffs and merges remain usable.

## Setup

### 1. Install Git LFS

Install Git LFS on your machine, then initialize it once:

```bash
git lfs install
```

### 2. Clone the repository

Clone normally:

```bash
git clone <repo-url>
cd WitchPantry
```

Git LFS files should download automatically in most setups. If they do not, run:

```bash
git lfs pull
```

### 3. If you already cloned before installing Git LFS

Run:

```bash
git lfs install
git lfs pull
```

Otherwise you may end up with pointer files instead of the real assets, which is funny exactly once.

## Working With LFS Files

When you add a file type that is already tracked by `.gitattributes`, use Git as usual:

```bash
git add .
git commit -m "Add new assets"
git push
```

Git LFS handles tracked files automatically.

To track a new binary asset type later:

```bash
git lfs track "*.ext"
git add .gitattributes
git commit -m "Track *.ext with Git LFS"
```

## Ignore Rules

The repository ignores generated Unity folders and IDE-local files through [.gitignore](/C:/Unity/Repos/WitchPantry/.gitignore), including:

- Unity-generated folders such as `Library/`, `Temp/`, `Logs/`, `Obj/`, and build output
- Visual Studio generated files
- JetBrains Rider local files such as `.idea/`, `*.DotSettings.user`, and `*.sln.iml`

These files should remain untracked because they are generated or machine-local.
