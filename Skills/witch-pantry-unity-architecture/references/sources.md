# Sources

Use these sources when the task needs justification, tradeoff framing, or current terminology.

## Unity

- Unity Manual: ScriptableObject
  - https://docs.unity3d.com/6000.1/Documentation/Manual/class-ScriptableObject.html
  - Use for authored-data boundaries and editor-facing asset workflows.
- Unity Scripting API: `LoadSceneMode.Additive`
  - https://docs.unity3d.com/6000.1/Documentation/ScriptReference/SceneManagement.LoadSceneMode.Additive.html
  - Use for additive scene loading and persistent shell architecture.
- Unity How-To: ScriptableObject-based runtime sets
  - https://unity.com/how-to/scriptableobject-based-runtime-set
  - Use when comparing runtime sets against singleton-heavy global access.
- Unity How-To: Separate game data and logic with ScriptableObjects
  - https://unity.com/how-to/separate-game-data-logic-scriptable-objects
  - Use for data-driven architecture and designer-safe authoring.
- Unity How-To: Level up your code with game programming patterns
  - https://unity.com/how-to/level-up-your-code-with-game-programming-patterns
  - Use for pattern-selection language and maintainability framing.

## Distilled Takeaways

- ScriptableObjects are best for shared authored data and selected shared channels, not for arbitrary mutable state.
- Additive scenes support persistent shell/UI plus room-by-room loading without collapsing everything into one scene.
- Runtime sets and event-driven patterns reduce hidden dependencies compared with singleton-first architectures.
- Pattern choice should fit project scale and team workflow, not an abstract purity contest.
