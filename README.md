# UI Panel
Provide simple approach to manage UI panels and add UI transition

https://github.com/Aluminum18/UICreatorDotween/assets/14157400/3acde8a5-efe8-4a11-b78b-cf4aca66ba3c

# Components
## UI Controller
Control a group of assigned UI Panels. There could be multiple UI Controllers in a scene, 2 is recommended, 1 for local UI of scene and 1 for global UI

![image](https://github.com/Aluminum18/UICreatorDotween/assets/14157400/be99d507-feb8-47dc-8e15-5eae141461a3)

**Configs**

Is Global UI: this controller controls group of global UI panels. ClickBlocker from this group are always shown

Show Init UI From Start: UIPanel that marked as ShowFromStart will be automatically opened when scene loaded

UI Panels: group of UIPanel this Controller controls

**Inspec**

Ui Stack: display UIPanel opening order during runtime. Useful for debugging

## UI Panel
Control a UI unit (a popup, menu, toast...)

![image](https://github.com/Aluminum18/UICreatorDotween/assets/14157400/8c36a1a7-cc00-4c4a-b479-8a8359e8586e)

**Configs**

Elements: UI Elements this Panel controls.

Click Blocker: UI Element that plays as Click Blocker which prevents user from clicking behind Panel

Show From Start: show from beginning without user command

Refresh When Reopen: if a Panel is opening, ask it to reopen will trigger OnRefresh() event

**Inspec**

Status: display Panel status during runtime, there 4 status IsClosing, Closed, IsOpening, Opened. Use for debugging

Showed Elements: number of UI Element have been shown. Use for debugging

## UI Element

A unit in UI Panel, it may content a single UI component (such as image, button...) or a group of them.
![image](https://github.com/Aluminum18/UICreatorDotween/assets/14157400/eb86aad7-84ea-4656-8503-f1eedb59969a)

## UI Transition

Recommended to add this component in additional with UI Element to animate UI Elements when they are opening/closing

![image](https://github.com/Aluminum18/UICreatorDotween/assets/14157400/3ed02ecf-b7d3-456a-84b1-3fa1b3fa05c5)

Transition Type: types of transition
  - Move: animate UI Element position
  - Zoom: animate UI Element scale
  - Fade: animate UI Element alpha (Canvas group is required)
  - Animation: play animation clip (Animator is required)

Show Delay / Hide Delay: delay show/hide transition

Show From / Hide From: Vector3 that presents UI Element position, scale or alpha (X value) at the beginning of Transition depend on Transition Type

Show To / Hide To: Vector3 that presents UI Element position, scale or alpha (X value) at the end of Transition depend on Transition Type

Show Transition Time / Hide Transition Time: transition duration

Show Ease Type / Hide Ease Type: Ease type of transition. Ease of [Dotween](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676) package has been used.
