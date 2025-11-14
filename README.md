## Procedural Gen Practice Tool
The Proecudral Gen Practice Tool was a 4 day attempt to make a simple room generator using Unity. This tool is free to use by all, and has a few features to customize some room generation.
This tool uses Unity 6000.1.16f1 make sure to install this version before use.

Within this code you can find 4 different Generation algorithms : Simple Room Placement, Binary Space Partioning, Cellular Automata, and NoiseMaps.

## General Usage
After loading the project, make your way to Components > ProceduralGeneration
Here choose which generation you'd like to use for the simulation.
When you've chosen, click on the ProceduralGridGenerator GameObject, and modify its "Generation Method" Parameter with the scriptableObject of your choosing
Feel free to modify the Cellsize, GridX and GridY values, aswell as the seed.

If you want to know more about a particular generation, navigate here.

### NAVIGATION :
- [Simple Room Placement](#Simple-Room-Placement)
- [Binary Space Partioning](#Binary-Space-Partioning)
- [Cellular Automata](#Cellular-Automata)
- [NoiseMaps](#NoiseMaps)


## Simple Room Placement
The simplest of the algorithms (Of course)


### HOW IT WORKS : 

This algorithm places random rooms within a grid of a random size between its [MaxSize](#max-size) and [MinSize](#min-size).
If these randomly placed rooms overlap an already placed room, the overlapping room is not placed, and it instead restarts the placement process, until either [Max Steps](#max-steps) is hit, or [Max Rooms](#max-rooms) is hit.
Additionally you can connect the rooms by turning on the [Generate Paths](#generate-paths) option.

<img width="445" height="184" alt="image" src="https://github.com/user-attachments/assets/29033c5b-0793-4163-bf60-eed8b96efbd6" />

### SRP PARAMETERS : 
#### Max Steps
Int : The amount of times the code loops to try to place a room, if this number of loops is exceeded, generation stops.

#### Max Rooms
Int : The maximum amount of rooms that can be placed, once that number is attained, generation stops.

#### Max Size
Vector2Int : The maximum Width and Length that a placed room can be.

#### Min Size
Vector2Int : The minimum Width and Length that a placed room can be.

#### Generate Paths
Bool : When on, paths will generate and connect the randomly generated rooms.

<img width="496" height="495" alt="image" src="https://github.com/user-attachments/assets/12a3d372-d7e6-4b53-8cfe-b3a79c27f009" />


## Binary Space Partioning
A more structured approach to room generation that recursively splits the space.

### HOW IT WORKS : 

This algorithm works by recursively dividing the available space into smaller partitions using binary space partitioning. Each split creates two children nodes, and this process continues until the desired [Children Count](#children-count) is reached or the partitions become too small. Rooms are then generated within the leaf nodes of this partition tree.

**Note:** This implementation is currently a little broken and may not always produce optimal results, but it demonstrates the core BSP concept.

<img width="438" height="153" alt="image" src="https://github.com/user-attachments/assets/782c465b-5b4e-43f6-9fc3-3fe686afdd48" />

### BSP PARAMETERS : 
#### Max Steps
Int : The amount of times the code loops to try to place a room, if this number of loops is exceeded, generation stops.

#### Max Rooms
Int : The maximum amount of rooms that can be placed, once that number is attained, generation stops.

#### Children Count
Int : The number of child partitions to create through recursive splitting. Higher values create more but smaller rooms, while lower values create fewer but larger rooms.

#### Max Size
Vector2Int : The maximum Width and Length that a placed room can be.

#### Min Size
Vector2Int : The minimum Width and Length that a placed room can be.

<img width="494" height="495" alt="image" src="https://github.com/user-attachments/assets/5144f873-dc4a-4951-9a1a-94aa73767eb8" />


## Cellular Automata
A organic, natural-looking generation method that uses cellular automata rules to create cave-like structures.

### HOW IT WORKS : 

This algorithm starts by randomly filling the grid with ground tiles based on the [Noise Level](#noise-level). It then applies cellular automata rules over multiple iterations ([Max Steps](#max-steps)) where each tile's state changes based on its neighbors. The [Grass Requirement](#grass-requirement) determines how many surrounding grass tiles are needed for a tile to become or remain grass. When [Generate Water](#generate-water) is enabled, tiles that don't meet the grass conditions can become water tiles, creating natural-looking pools and rivers.

<img width="434" height="108" alt="image" src="https://github.com/user-attachments/assets/9380e041-60c9-46da-b08f-34c54a066dba" />

### CELLULAR AUTOMATA PARAMETERS : 
#### Max Steps
Int : The number of iterations the cellular automata rules are applied. More steps create smoother, more refined terrain patterns.

#### Noise Level
Float : Controls the initial density of ground tiles at the start of generation. Higher values create more initial ground coverage.

#### Grass Requirement
Int : The number of surrounding grass tiles required for a tile to become or remain grass. Higher values create more sparse, isolated grass patches.

#### Generate Water
Bool : When enabled, tiles that don't meet the grass requirements will become water tiles instead of remaining empty, creating natural water features.

<img width="492" height="495" alt="image" src="https://github.com/user-attachments/assets/631dd7c2-1431-4bd4-a9bc-787b3c779358" />


## NoiseMaps
The most advanced and complete generation process in this tool, using Simplex noise to create realistic, biome-based terrain.

### HOW IT WORKS : 

This algorithm generates terrain heightmaps using multi-octave Simplex noise, controlled by parameters like [Frequency](#frequency), [Amplitude](#amplitude), [Octaves](#octaves), [Lacunarity](#lacunarity), and [Persistence](#persistence). The resulting noise values are then interpreted into different terrain types based on height thresholds: [Water Height](#water-height), [Sand Height](#sand-height), [Grass Height](#grass-height), and [Rock Height](#rock-height). Multiple iterations ([Max Steps](#max-steps)) can be applied to refine the terrain generation.

**Pre-configured Biomes:** This generator includes three scriptable objects for quick biome simulation:
- **Desert:** Low water, expansive sand and small rock formations
- **Swamp:** High water levels with lots of grass patches
- **Archipelago:** Island chains surrounded by water with varied terrain heights

<img width="425" height="310" alt="image" src="https://github.com/user-attachments/assets/1c05025e-e746-493a-a00b-9fb7a6a6271d" />

### NOISEMAPS PARAMETERS : 
#### Max Steps
Int : The number of refinement iterations applied to the generated terrain. More steps can create smoother transitions and more detailed features.

#### Frequency
Float : Controls the scale of the noise pattern. Higher frequency creates more frequent, smaller features, a zoom effect.

#### Amplitude
Float : Controls the intensity or height variation of the noise. Higher amplitude creates more dramatic terrain elevation changes.

#### Octaves
Int : The number of layers of noise combined together. More octaves create more detailed and complex terrain.

#### Lacunarity
Float : Controls how the frequency changes between octaves. Higher values create more detailed high-frequency content.

#### Persistence
Float : Controls how the amplitude changes between octaves. Higher values maintain more influence from higher octaves.

#### Water Height
Float : The height threshold above which tiles become water. Higher values create more water coverage.

#### Sand Height
Float : The height threshold for sand tiles, typically between water and grass levels.

#### Grass Height
Float : The height threshold for grass tiles, typically between sand and rock levels.

#### Rock Height
Float : The height threshold below which tiles become rock. Lower values create more mountainous terrain.

<img width="497" height="496" alt="image" src="https://github.com/user-attachments/assets/5e55439a-26c7-4b6c-8fe7-ec8f2fb270dc" />

<!-- GamePlan : 
Explain the program
Talk about the use of each method
Talk about their parameters if applicable
Recommended Settings/Scriptables if applicable
Show examples -->

<!-- Comments are here, below is some format info -->

<!-- 
For dropdown menus : 
<details>
<summary>Details</summary>

  - [Introduction](#introduction)
  - [Get Started](#Get-Started)
    - [How to install](#How-to-install)
    - [Using Scriptable Object](#Using-Scriptable-Object)
    - [How to use editable variables](#How-to-use-editable-variable)
  - [Features](#Features)
  - [Documentation](#Documentation)
  - [Misusing / Limitations](#Misusing-/-Limitations)
  - [License](#License)


For images : 
<img width="1881" height="942" alt="image" src="https://github.com/user-attachments/assets/90596d99-89f8-4239-bead-6ae489d08ad2" />

For links : 
- [Download UnityHub](https://unity.com/download)

For code snippets : 
```csharp
private readonly Cell[,] _gridArray;       
private readonly List<Cell> _cells;

public Vector3 OriginPosition { get; }
public float CellSize { get; }
public int Width { get; }
public int Lenght { get; }
public IReadOnlyList<Cell> Cells => _cells;
```
-->
