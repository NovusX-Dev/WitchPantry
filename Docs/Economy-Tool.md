# Unity Economy Simulation Tool

For *Witch's Pantry Automation*.

- Engine: Unity 6; current project version: `Witch-Pantry/ProjectSettings/ProjectVersion.txt`
- Language: C#

## 1. Overview

This tool simulates the incremental economy directly inside the Unity Editor.

It automatically runs thousands of simulated seconds of gameplay and generates graphs showing:

- Gold generation
- Machine purchases
- Production growth
- Upgrade timing
- Prestige pacing

This allows designers to tune economy values quickly.

## 2. Tool Features

The simulator provides:

- Economy simulation
- Graph visualization
- Parameter tuning
- Auto-balancing experiments

Suggested graph outputs:

- Production vs. Time
- Gold vs. Time
- Machine Count vs. Time
- Upgrade Timing

## 3. Tool Architecture

```text
EditorWindow
  ->
Economy Simulator
  ->
Data Recorder
  ->
Graph Renderer
```

## 4. Folder Structure

```text
Assets/
  Editor/
    EconomySimulatorWindow.cs
    EconomySimulator.cs
    GraphRenderer.cs
  Data/
    EconomySettings.cs
```

## 5. Economy Settings ScriptableObject

Used to configure economy parameters.

For Witch Pantry, this tool should eventually read from the authored content-definition layer
instead of treating machine economics as a completely separate data universe. In practice that
means machine purchase and upgrade tuning should stay compatible with `MachineDefinition`,
recipe timing should stay compatible with `RecipeDefinition`, and potion value assumptions
should stay compatible with `PotionDefinition`.

```csharp
[CreateAssetMenu]
class EconomySettings : ScriptableObject
{
    public float baseCost = 10;
    public float growthRate = 1.15f;
    public float baseProduction = 1f;
    public float prestigeMultiplier = 2f;
}
```

## 6. Simulation State

```csharp
class SimulationState
{
    public double gold;
    public int machines;
    public double productionPerSecond;
}
```

## 7. Core Simulation Loop

Pseudo-code:

```text
for tick in simulationLength
    gold += production * tickDuration

    nextCost = baseCost * pow(growthRate, machines)

    if gold >= nextCost
        gold -= nextCost
        machines++
        production = baseProduction * machines

    record data
```

## 8. Data Recording

```csharp
class SimulationData
{
    List<float> time;
    List<float> gold;
    List<int> machines;
    List<float> production;
}
```

This data feeds the graph renderer.

## 9. Editor Window

Create a custom Unity editor window:

```csharp
public class EconomySimulatorWindow : EditorWindow
{
    [MenuItem("Tools/Economy Simulator")]
    static void Open()
    {
        GetWindow<EconomySimulatorWindow>();
    }

    void OnGUI()
    {
        if (GUILayout.Button("Run Simulation"))
        {
            RunSimulation();
        }

        DrawGraphs();
    }
}
```

## 10. Running the Simulation

```csharp
void RunSimulation()
{
    simulator = new EconomySimulator(settings);
    simulationData = simulator.Run(36000);
}
```

`36000` ticks equals `10 hours` simulated.

## 11. Graph Renderer

Use the Unity Handles API:

```csharp
Handles.DrawLine(pointA, pointB);
```

Graph example:

- Gold vs. Time

```text
for i in points
    drawLine(point[i], point[i + 1])
```

## 12. Graph Types

Recommended graphs:

- Gold vs. Time
- Production vs. Time
- Machines Owned vs. Time
- Upgrade Cost vs. Time

## 13. Example Graph

```text
Production
|
|        *
|      *
|    *
|  *
| *
|________________________
                     Time
```

## 14. Economy Debug UI

Add sliders for:

- Base Cost
- Growth Rate
- Production Rate
- Prestige Multiplier

Change values and rerun the simulation instantly.

## 15. Detect Economy Problems

The simulator should highlight issues such as:

- Flat curves: stalled progression
- Vertical spikes: runaway inflation
- Smooth exponential curves: healthy growth

## 16. Target Economy Curve

Example pacing:

- `10 seconds`: first upgrade
- `1 minute`: automation
- `10 minutes`: production chains
- `2 hours`: prestige

## 17. Automated Balance Search

Optional feature:

Run many simulations with random parameters and pick the best progression curve.

Pseudo-code:

```text
for 1000 runs
    randomize parameters
    run simulation
    score curve quality
```

## 18. Curve Quality Score

Measure:

- Average upgrade time
- Prestige timing
- Production acceleration

Score example:

```csharp
score = rewardGrowth - stallPenalty;
```

## 19. Visual Graph Upgrade

Later improvement:

Use Unity GraphView to build visual node-based production graphs inside the editor.

## 20. Performance

Simulation should run in milliseconds.

Example target:

- `10 hours` simulated in less than `0.1 seconds`

## 21. Example Output

| Time | Gold | Machines | Production |
| --- | ---: | ---: | ---: |
| 0 | 0 | 1 | 1 |
| 10 | 10 | 2 | 2 |
| 30 | 60 | 5 | 5 |
| 120 | 500 | 20 | 20 |
| 600 | 6000 | 80 | 80 |

## 22. Usage Workflow

```text
Designer adjusts economy parameters
  ->
Runs simulation
  ->
Observes graphs
  ->
Tweaks values
  ->
Repeats
```

## 23. Benefits

This tool helps prevent:

- Dead economies
- Slow progression
- Unbalanced upgrades

## 24. Future Improvements

- Export graphs to CSV
- Add auto-balance algorithms
- Add prestige simulation
- Support multiple production chains

## End

End of document.
