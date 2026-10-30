# VR-ROOM-XP

A Meta Quest VR app built in Unity 6 for the Unity Learn **Create with VR** course and the enhanced Create with VR assignment.
It has three scenes: a start menu, the room built through the course, and a sci-fi intranet room with physics interactions,
sockets, a hinged door, an analog wall clock and animated hands.

**Author:** Ntare GAMA Allan ([@Ntare-GAMA](https://github.com/Ntare-GAMA))

---

## Scenes

| # | Scene | Path | What it is |
|---|---|---|---|
| 0 | **Main Menu** | `Assets/Scenes/Main Menu.unity` | Start screen with a card for each room, a live wall clock and a Quit button |
| 1 | **VR Room - NTARE GAMA Allan** | `Assets/Scenes/VR Room - NTARE GAMA Allan.unity` | The completed Create with VR course room |
| 2 | **VR Room (Intranet)** | `Assets/Unity - VR Room/Assets/Scenes/VR Room (Intranet).unity` | The sci-fi intranet room built for the assignment |

All three are in **File > Build Settings** in that order, so the app always starts at the Main Menu.

## Features

### Main Menu
- Two room cards, **Create with VR Room** and **VR Room (Intranet)**, each with an **Enter room** button.
- Fade to black between scenes, click sounds, and a controls hint.
- An analog + digital clock showing the headset's time.

### VR Room - NTARE GAMA Allan (course room)
Everything from the course lessons:
- **Locomotion:** teleport areas, teleport mats, snap turn and continuous move.
- **Grabbing:** hand and ray grabbing, distance grab (toggle in Settings), books, a remote, hats and more.
- **Sockets:** three hat hooks plus a hat socket on the player's head, all filtered to the **Hats** interaction layer.
- **Physics:** cabinet doors on hinge joints, the room door on a hinge (grab its handle), and paintings you can take off the wall and hang back on their hooks (**Paintings** layer).
- **Audio and haptics:** sound and controller vibration on interactions.
- **TV and remote:** play and pause a video, switch the TV on and off.
- **UI:** a welcome panel that follows your view, a Settings page, a Reset page, a menu button, plus **Main Menu** and **VR Room (Intranet)** navigation buttons.
- **Optimization and lighting:** baked lightmaps, light probes, URP settings tuned for Quest, and an FPS overlay.

### VR Room (Intranet)
- **Object grabbing:** boxes, tubes, rope, the console, the flashlight, all chess pieces, pillows and statues. Each has a Rigidbody and an XR Grab Interactable.
- **Sockets with layer filtering:**
  - **Chess board:** the 4 centre squares accept only the **Chess** layer.
  - **Flashlight dock:** a glowing charging dock that accepts only the **Flashlight** layer. Pull the trigger to switch the flashlight on or off.
  - **Generator:** accepts only **Power Cell** objects (the tubes) and lights a green status light when one is plugged in.
- **Physics door:** the glass door swings on a **Hinge Joint** at its real hinge edge. It can only be grabbed by its handles.
- **Analog wall clock:** hour, minute and second hands driven by code from the system time, plus a digital time and date display. No animation clips.
- **In-room UI:** an instructions panel that follows your view, with **Start exploring**, **Create with VR Room**, **Main Menu** and **Reset room**. Press **A** or **Menu** to open it again.
- **Teleporting** anywhere on the floor, and colliders on the walls, floors and furniture.

### Animated hands (all scenes)
Rigged hands that follow your real fingers: **grip** curls the middle, ring and little fingers, the **trigger** curls the index finger,
and the thumb follows whichever is pressed harder. Half presses give half-curled fingers.

## Controls (Meta Quest Touch controllers)

| Input | Action |
|---|---|
| Grip | Grab / curl fingers |
| Trigger | Use a held object (flashlight, remote) / press UI buttons / curl index finger |
| Left thumbstick | Move |
| Right thumbstick | Snap turn, or push forward to aim a teleport |
| A / Menu | Open the room menu |

## Scripts

All gameplay scripts are in `Assets/Scripts`. The course scripts live in `Assets/_Course Library/Scripts`.

| Script | What it does |
|---|---|
| `SceneNavigator.cs` | Loads a scene by name from a UI button. It fades to black first through the scene's `FadeCanvas`, then uses `SceneManager.LoadSceneAsync`. It also has `ReloadCurrentScene()` and `Quit()`. |
| `AnalogClock.cs` | Every frame it reads `DateTime.Now` and rotates the hands: 30° per hour, 6° per minute and 6° per second. The hour and minute hands include the fraction of the next unit, so they move continuously. It also writes `HH:mm:ss` and the full date to TextMeshPro labels. |
| `HandPoseAnimator.cs` | Reads the analog grip and trigger values from the XRI input actions, smooths them, and bends each finger joint from its rest pose around its local X axis. It runs in `LateUpdate` with no animation clips. |
| `ToggleRayDistanceGrab.cs` | Lets the Settings toggle switch the rays between pulling objects to the hand and grabbing them at a distance. |

### How the interactions are built
- **Interaction layers** (`Assets/XRI/Settings/Resources/InteractionLayerSettings.asset`): Default, Hats, House, Wrist, Chess, Flashlight, Power Cell and Paintings. A socket only accepts interactables that share one of its layers, which is how each socket filters what it accepts.
- **Hinged doors:** a non-kinematic Rigidbody with a `HingeJoint` on the hinge edge. The `XRGrabInteractable` uses **Velocity Tracking** with rotation tracking off, so the hand pushes the door and the joint keeps it on its axis. Its collider list holds only the handle.
- **Sockets that start full:** each painting hook and the flashlight dock use `startingSelectedInteractable`, so the object starts in place and snaps back to the exact same pose.
- **Shared player:** `Assets/Prefabs/XR Player.prefab` is the course XR Origin (hands, rays, locomotion, UI interaction) saved as a prefab, so every scene has the same player.

## Built with

- Unity **6000.0.80f1** (Unity 6), Universal Render Pipeline 17.1
- XR Interaction Toolkit 3.1.3, OpenXR 1.16 (Meta Quest feature, Oculus Touch controller profile)
- Input System, TextMeshPro
- Target: Android / Meta Quest, Vulkan graphics, GameActivity

## Getting started

1. Clone the repo and open the folder in **Unity Hub** with Unity **6000.0.80f1** and the Android Build Support module.
2. Open `Assets/Scenes/Main Menu.unity` and press **Play**. Use a Quest with Quest Link, or the XR Device Simulator.
3. To build for the headset, open **File > Build Settings**, choose **Android**, connect the Quest with Developer Mode on, then click **Build And Run**.

## Credits

- **Create with VR** course project and `_Course Library` assets: Unity Technologies, Unity Learn
- **VR Room** intranet scene assets (Sci-Fi Styled Modular Pack, Chess Set, Flashlight, Survival Kit, Mugs, Origami Crane): provided with the assignment
- **Free Pack - VR Hands** (rigged hands): PolyOne, Unity Asset Store
- XR Interaction Toolkit Starter Assets: Unity Technologies
