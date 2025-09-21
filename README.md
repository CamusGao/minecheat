# Minecheat

A lightweight save editor for Minecraft: Java Edition.

## Features
- Opens a world save folder and parses `level.dat` (NBT) via the fNbt library.
- Shows key world metadata (e.g. game mode, time (ticks), seed, version info).
- Allows editing supported world / level data fields (where implemented in modules).
- Modular design: add new editors via `IFeature` / `IFeatureModule` implementations.
- Tick time to human-friendly conversion helper.
- Simple logging output in the main window.
- Extensible WPF (C#) UI prepared for additional feature panels.

> Always back up your world before making changes.
