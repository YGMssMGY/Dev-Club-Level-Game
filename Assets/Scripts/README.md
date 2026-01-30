# 2D Game Scripts – Setup

Basic scripts for the Unity 2D sample scene.

## Scene overview

- **Square** – Player (Rigidbody2D Dynamic, SpriteRenderer, BoxCollider2D) at (0, 0)
- **Square (1)** – Ground/platform (Rigidbody2D Kinematic) below the player
- **Main Camera** – Orthographic camera at (0, 0, -10)
- **Global Light 2D** – 2D lighting

## Setup in Unity

### 1. Player (Square)

1. Select **Square** in the Hierarchy.
2. **Add Component** → search `PlayerController2D` → add.
3. Optional: set **Move Speed** (default 5), **Jump Force** (default 10).
4. **Ground Layer**: leave default “Everything” so the ground is detected, or create a **Ground** layer, assign it to **Square (1)**, and set **Ground Layer** to **Ground** in the inspector.

### 2. Camera (Main Camera)

1. Select **Main Camera**.
2. **Add Component** → search `CameraFollow2D` → add.
3. Drag **Square** from the Hierarchy into the **Target** field (or assign in the inspector).
4. Optional: adjust **Smooth Time** (default 0.2).

### 3. Game Manager (optional)

1. Create empty GameObject: **Right‑click Hierarchy** → **Create Empty** → name it `GameManager`.
2. **Add Component** → search `GameManager` → add.
3. Use from UI or other scripts: `GameManager.Instance.RestartCurrentScene()` or `GameManager.Instance.QuitGame()`.

## Controls (default Input Manager)

- **Horizontal**: A/D or Left/Right arrows
- **Jump**: Space or Jump button

## Build settings

Add **SampleScene** to **File → Build Settings → Scenes In Build** so `RestartCurrentScene()` works in a build.
