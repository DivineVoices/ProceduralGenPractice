## Procedural Gen Practice Tool
The Proecudral Gen Practice Tool was a 4 day attempt to make a simple room generator using Unity. This tool is free to use by all, and has a few features to customize your room generation.
This tool uses Unity 6000.1.16f1 make sure to install this version before use.

Within this code you can find 4 different Generation algorithms : Simple Room Placement, Binary Space Partioning, Cellular Automata, and NoiseMaps.

## General Usage
After loading the project, make your way to Components > ProceduralGeneration
Here choose which generation you'd like to use for the simulation.

NAVIGATION :
- [Simple Room Placement](#Simple-Room-Placement)
- [Binary Space Partioning](#Binary-Space-Partioning)
- [Cellular Automata](#Cellular-Automata)
- [NoiseMaps](#NoiseMaps)


## Simple Room Placement
The simplest of the algorithms (Of course)

HOW IT WORKS : 
This algorithm places random rooms within a grid of a random size between its [MaxSize/MinSize](#MaxSize/MinSize).

<img width="445" height="184" alt="image" src="https://github.com/user-attachments/assets/29033c5b-0793-4163-bf60-eed8b96efbd6" />
SRP PARAMETERS : 
- ## Max Steps :
- ## MaxSize/MinSize :


## Binary Space Partioning
## Cellular Automata
## NoiseMaps
The most complete generation process in this tool. Uses the 

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
