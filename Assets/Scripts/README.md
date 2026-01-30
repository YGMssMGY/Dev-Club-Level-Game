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

### 3. Game Manager (required for Start Over)

1. Create empty GameObject: **Right‑click Hierarchy** → **Create Empty** → name it `GameManager`.
2. **Add Component** → search `GameManager` → add.

### 4. Death (fall off = break apart + red screen + RIP)

1. **Player**: Select **Square** → **Add Component** → `PlayerDeathHandler`. Set **Death Y Threshold** below your platform (e.g. **-5**; ground is ~-1.5).
2. **Death UI**: Create empty GameObject → name it `DeathUI` → **Add Component** → `DeathUI`. It builds the red overlay and RIP panel at runtime. No extra setup.
3. **Optional – pit trigger**: Create empty GameObject, add **Box Collider 2D** (or any 2D collider), check **Is Trigger**, place below the level. Add component `DeathZone`. Player dies when entering the trigger.

## Controls (default Input Manager)

- **Horizontal**: A/D or Left/Right arrows
- **Jump**: Space or Jump button

## Death flow

When the player’s Y goes below **Death Y Threshold** (or enters a **DeathZone** trigger):

1. Player breaks into pieces (same sprite, small bits with physics).
2. Red overlay fades in.
3. RIP panel appears with **Start Over** button (restarts the scene via `GameManager`).

## Build settings

Add **SampleScene** to **File → Build Settings → Scenes In Build** so `RestartCurrentScene()` works in a build.
