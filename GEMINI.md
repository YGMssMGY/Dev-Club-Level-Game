# Project Overview

This is a **Unity 2D Game** project focused on core platformer mechanics. It implements a functional player controller, a smooth camera follow system, and a complete game-over loop with a death sequence and UI.

## Technologies & Architecture

*   **Engine:** Unity (2D Template)
*   **Language:** C#
*   **Physics:** Unity Physics 2D (`Rigidbody2D`, `Collider2D`)
*   **Input:** Legacy Unity Input Manager (`Input.GetAxis`, `Input.GetButtonDown`)
*   **Rendering:** Universal Render Pipeline (URP) or Standard 2D (inferred from assets).

### Core Components

The project logic is contained in `Assets/Scripts/`:

| Script | Purpose | Key Dependencies |
| :--- | :--- | :--- |
| `PlayerController2D.cs` | Handles horizontal movement and jumping. Uses `Physics2D.OverlapCircle` for ground checks and applies forces via `Rigidbody2D`. | `Rigidbody2D`, `Collider2D` |
| `CameraFollow2D.cs` | Smoothly follows a target `Transform` (usually the player) using `Vector3.SmoothDamp`. | `Transform` (Target) |
| `GameManager.cs` | Singleton responsible for global game state, specifically restarting the current scene and quitting the application. | `SceneManager` |
| `PlayerDeathHandler.cs` | Monitors player state for death conditions (falling below Y threshold). Triggers visual effects (breaking apart) and the UI sequence. | `SpriteRenderer`, `Rigidbody2D` |
| `DeathUI.cs` | Generates the Game Over UI (Red overlay, RIP text, Restart button) at runtime via code, eliminating the need for a prefab. | `Canvas`, `EventSystem` |
| `DeathZone.cs` | Simple trigger script that kills the player upon collision. | `Collider2D` (IsTrigger) |

## Building and Running

### Prerequisites
*   **Unity Editor:** (Version matching `ProjectSettings/ProjectVersion.txt`)

### How to Run in Editor
1.  Open the project in Unity Hub.
2.  Open the main scene (likely `Assets/Scenes/SampleScene.unity`).
3.  Press the **Play** button.

### Controls
*   **Move:** A / D or Left Arrow / Right Arrow
*   **Jump:** Spacebar

### Build Process
1.  Go to **File > Build Settings**.
2.  Ensure `SampleScene` (or your active scene) is added to the **Scenes In Build** list.
3.  Select your target platform (Windows, WebGL, etc.).
4.  Click **Build** or **Build And Run**.

## Development Conventions

*   **Script Location:** All C# source files are located in `Assets/Scripts/`.
*   **Singleton Pattern:** Used for managers (`GameManager`, `DeathUI`) to ensure easy global access.
*   **Runtime UI:** This project uses a code-first approach for the Death UI, constructing the Canvas and elements in `Awake()`.
*   **Physics-Based Movement:** The player moves via `Rigidbody2D.linearVelocity` manipulation in `FixedUpdate` for consistent physics behavior.
*   **Code Style:**
    *   `[SerializeField]` is used to expose private fields to the Inspector.
    *   Explicit dependencies are enforced via `[RequireComponent]`.
    *   Standard C# naming conventions (PascalCase for classes/methods, camelCase for fields).

## Setup Instructions (New Scenes)

If setting up a new scene from scratch:

1.  **Player:** Add `PlayerController2D` and `PlayerDeathHandler` to your player GameObject.
2.  **Camera:** Add `CameraFollow2D` to the Main Camera and assign the Player as the target.
3.  **Managers:** Create empty GameObjects for `GameManager` and `DeathUI` and attach their respective scripts.
