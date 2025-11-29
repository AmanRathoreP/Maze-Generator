# Maze Generation Algorithms - Comprehensive Documentation

## Table of Contents
1. [Overview](#overview)
2. [Architecture](#architecture)
3. [Cell Class](#cell-class)
4. [MazeCell Class](#mazecell-class)
5. [Maze Class](#maze-class)
6. [IMazeAlgorithm Interface](#imazealgorithm-interface)
7. [MazeGenerator Class](#mazegenerator-class)
8. [RandomWithValidPath Algorithm](#randomwithvalidpath-algorithm)
9. [Usage Examples](#usage-examples)
10. [Best Practices](#best-practices)

---

## Overview

This project provides a robust framework for generating and manipulating maze structures. It uses a tree-based data structure where cells can be connected to form paths, with support for freezing cells to prevent modifications. The architecture follows SOLID principles with an interface-based design for maze generation algorithms.

### Key Concepts
- **1-based Indexing**: All row and column numbers start from 1 (no zeros or negatives)
- **Orthogonal Connections**: Cells can only connect to adjacent neighbors (up, down, left, right - no diagonals)
- **Bidirectional Edges**: Connections between cells are always bidirectional
- **Immutable Frozen State**: Once a cell is frozen, its connections cannot be modified
- **Deep Cloning**: Maze objects can be cloned, creating independent copies in separate memory
- **Algorithm Independence**: Multiple algorithms can be plugged in through the IMazeAlgorithm interface
- **Immutable Generation**: Algorithms never modify the input maze, always returning a new copy

---

## Architecture

The system is designed with a clear separation of concerns:

```
Models/
  ├── Cell.cs                    # Basic cell with coordinates
  └── MazeCell.cs               # Maze cell with connections and frozen state

DataStructures/
  └── Maze.cs                   # Maze grid structure with connection management

Algorithms/
  ├── IMazeAlgorithm.cs        # Interface for maze generation algorithms
  └── RandomWithValidPathAlgorithm.cs  # Specific algorithm implementation

MazeGenerator.cs               # Main entry point using factory pattern
```

### Design Patterns Used
- **Factory Pattern**: MazeGenerator creates appropriate algorithm instances
- **Strategy Pattern**: Different algorithms can be swapped via IMazeAlgorithm interface
- **Template Method**: Clone() provides consistent deep copying mechanism

---

## Cell Class

**Namespace**: `maze_generation_algorithms.Models`  
**File**: `Models/Cell.cs`

### Description
Represents a basic cell in a 2D grid with row and column coordinates. Provides helper methods to navigate to adjacent cells.

### Properties

#### `Row` (int, read-only)
- **Type**: `int`
- **Access**: Public, Get-only
- **Description**: The row coordinate of the cell
- **Example**:
  ```csharp
  var cell = new Cell(5, 10);
  Console.WriteLine(cell.Row); // Output: 5
  ```

#### `Column` (int, read-only)
- **Type**: `int`
- **Access**: Public, Get-only
- **Description**: The column coordinate of the cell
- **Example**:
  ```csharp
  var cell = new Cell(5, 10);
  Console.WriteLine(cell.Column); // Output: 10
  ```

### Constructor

#### `Cell(int row, int column)`
Creates a new cell with the specified coordinates.

**Parameters**:
- `row` (int): The row coordinate
- `column` (int): The column coordinate

**Example**:
```csharp
var cell = new Cell(3, 7);
// Creates a cell at position (row: 3, column: 7)
```

### Methods

#### `TopCell()` → Cell
Returns a new cell representing the position above the current cell (row + 1).

**Returns**: A new `Cell` instance with incremented row

**Example**:
```csharp
var cell = new Cell(5, 10);
var topCell = cell.TopCell();
Console.WriteLine(topCell); // Output: Cell(6, 10)
```

#### `BottomCell()` → Cell
Returns a new cell representing the position below the current cell (row - 1).

**Returns**: A new `Cell` instance with decremented row

**Example**:
```csharp
var cell = new Cell(5, 10);
var bottomCell = cell.BottomCell();
Console.WriteLine(bottomCell); // Output: Cell(4, 10)
```

#### `LeftCell()` → Cell
Returns a new cell representing the position to the left of the current cell (column - 1).

**Returns**: A new `Cell` instance with decremented column

**Example**:
```csharp
var cell = new Cell(5, 10);
var leftCell = cell.LeftCell();
Console.WriteLine(leftCell); // Output: Cell(5, 9)
```

#### `RightCell()` → Cell
Returns a new cell representing the position to the right of the current cell (column + 1).

**Returns**: A new `Cell` instance with incremented column

**Example**:
```csharp
var cell = new Cell(5, 10);
var rightCell = cell.RightCell();
Console.WriteLine(rightCell); // Output: Cell(5, 11)
```

#### `Equals(object? obj)` → bool
Determines whether the current cell is equal to another object.

**Parameters**:
- `obj` (object?): The object to compare with

**Returns**: `true` if both cells have the same row and column; otherwise, `false`

**Example**:
```csharp
var cell1 = new Cell(5, 10);
var cell2 = new Cell(5, 10);
var cell3 = new Cell(5, 11);

Console.WriteLine(cell1.Equals(cell2)); // Output: True
Console.WriteLine(cell1.Equals(cell3)); // Output: False
```

#### `GetHashCode()` → int
Returns a hash code for the cell based on its row and column.

**Returns**: An integer hash code

**Example**:
```csharp
var cell = new Cell(5, 10);
var hashCode = cell.GetHashCode();
// Used internally by collections like HashSet and Dictionary
```

#### `ToString()` → string
Returns a string representation of the cell.

**Returns**: A string in the format "Cell(row, column)"

**Example**:
```csharp
var cell = new Cell(5, 10);
Console.WriteLine(cell.ToString()); // Output: Cell(5, 10)
```

---

## MazeCell Class

**Namespace**: `maze_generation_algorithms.Models`  
**File**: `Models/MazeCell.cs`  
**Inherits**: `Cell`

### Description
Extends the `Cell` class to add maze-specific functionality, including neighbor connections and the ability to freeze cells to prevent modifications.

### Properties

#### `IsFrozen` (bool, read-only)
- **Type**: `bool`
- **Access**: Public, Get-only
- **Description**: Indicates whether this cell is frozen (cannot modify connections)
- **Default**: `false`
- **Example**:
  ```csharp
  var cell = new MazeCell(5, 10);
  Console.WriteLine(cell.IsFrozen); // Output: False
  
  cell.Freeze();
  Console.WriteLine(cell.IsFrozen); // Output: True
  ```

#### `Neighbors` (HashSet<MazeCell>, read-only)
- **Type**: `HashSet<MazeCell>`
- **Access**: Public, Get-only
- **Description**: Set of connected neighbor cells
- **Example**:
  ```csharp
  var cell = new MazeCell(5, 10);
  var neighbor = new MazeCell(5, 11);
  cell.AddNeighbor(neighbor);
  
  Console.WriteLine(cell.Neighbors.Count); // Output: 1
  foreach (var n in cell.Neighbors)
  {
      Console.WriteLine(n); // Output: Cell(5, 11)
  }
  ```

### Constructor

#### `MazeCell(int row, int column)` : base(row, column)
Creates a new maze cell with the specified coordinates.

**Parameters**:
- `row` (int): The row coordinate (1-based)
- `column` (int): The column coordinate (1-based)

**Example**:
```csharp
var cell = new MazeCell(3, 7);
// Creates an unfrozen maze cell at position (3, 7) with no neighbors
```

### Methods

#### `Freeze()` → void
Marks this cell as frozen, preserving current neighbor connections. Once frozen, no neighbors can be added or removed.

**Side Effects**: 
- Sets `IsFrozen` to `true`
- Creates a snapshot of current neighbors

**Example**:
```csharp
var cell = new MazeCell(5, 10);
var neighbor = new MazeCell(5, 11);

cell.AddNeighbor(neighbor);
Console.WriteLine(cell.Neighbors.Count); // Output: 1

cell.Freeze();
var newNeighbor = new MazeCell(6, 10);
bool added = cell.AddNeighbor(newNeighbor);
Console.WriteLine(added); // Output: False (cannot add to frozen cell)
Console.WriteLine(cell.Neighbors.Count); // Output: 1 (unchanged)
```

#### `AddNeighbor(MazeCell neighbor)` → bool
Adds a neighbor connection to this cell (only if not frozen).

**Parameters**:
- `neighbor` (MazeCell): The cell to add as a neighbor

**Returns**: 
- `true` if the neighbor was successfully added
- `false` if the cell is frozen or neighbor already exists

**Example**:
```csharp
var cell1 = new MazeCell(5, 10);
var cell2 = new MazeCell(5, 11);

bool success1 = cell1.AddNeighbor(cell2);
Console.WriteLine(success1); // Output: True

bool success2 = cell1.AddNeighbor(cell2);
Console.WriteLine(success2); // Output: False (already exists)

cell1.Freeze();
var cell3 = new MazeCell(6, 10);
bool success3 = cell1.AddNeighbor(cell3);
Console.WriteLine(success3); // Output: False (frozen)
```

#### `RemoveNeighbor(MazeCell neighbor)` → bool
Removes a neighbor connection from this cell (only if not frozen).

**Parameters**:
- `neighbor` (MazeCell): The cell to remove from neighbors

**Returns**: 
- `true` if the neighbor was successfully removed
- `false` if the cell is frozen or neighbor doesn't exist

**Example**:
```csharp
var cell1 = new MazeCell(5, 10);
var cell2 = new MazeCell(5, 11);

cell1.AddNeighbor(cell2);
Console.WriteLine(cell1.Neighbors.Count); // Output: 1

bool removed = cell1.RemoveNeighbor(cell2);
Console.WriteLine(removed); // Output: True
Console.WriteLine(cell1.Neighbors.Count); // Output: 0

cell1.Freeze();
cell1.AddNeighbor(cell2); // Won't work, but for demonstration
removed = cell1.RemoveNeighbor(cell2);
Console.WriteLine(removed); // Output: False (frozen)
```

#### `GetFrozenNeighbors()` → IReadOnlyCollection<MazeCell>
Gets the neighbors that existed when the cell was frozen. If the cell is not frozen, returns current neighbors.

**Returns**: Read-only collection of maze cells

**Example**:
```csharp
var cell = new MazeCell(5, 10);
var neighbor1 = new MazeCell(5, 11);
var neighbor2 = new MazeCell(6, 10);

cell.AddNeighbor(neighbor1);
cell.Freeze();

var frozenNeighbors = cell.GetFrozenNeighbors();
Console.WriteLine(frozenNeighbors.Count); // Output: 1

// Even if we try to add more (which will fail)
cell.AddNeighbor(neighbor2);
Console.WriteLine(frozenNeighbors.Count); // Output: 1 (unchanged)
```

---

## Maze Class

**Namespace**: `maze_generation_algorithms.DataStructures`  
**File**: `DataStructures/Maze.cs`

### Description
Represents a rectangular maze as a tree data structure with `MazeCell` nodes. Provides functionality to connect cells, freeze them, and navigate the maze structure. Uses 1-based indexing throughout.

### Properties

#### `Rows` (int, read-only)
- **Type**: `int`
- **Access**: Public, Get-only
- **Description**: Gets the number of rows in the maze
- **Example**:
  ```csharp
  var maze = new Maze(20, 30);
  Console.WriteLine(maze.Rows); // Output: 20
  ```

#### `Columns` (int, read-only)
- **Type**: `int`
- **Access**: Public, Get-only
- **Description**: Gets the number of columns in the maze
- **Example**:
  ```csharp
  var maze = new Maze(20, 30);
  Console.WriteLine(maze.Columns); // Output: 30
  ```

### Constructor

#### `Maze(int rows, int columns)`
Initializes a new maze with the specified dimensions. All cells are created but initially have no connections.

**Parameters**:
- `rows` (int): Number of rows (must be positive, ≥ 1)
- `columns` (int): Number of columns (must be positive, ≥ 1)

**Throws**:
- `ArgumentException`: If rows or columns are less than or equal to 0

**Example**:
```csharp
// Create a 20x20 maze (400 cells total)
var maze = new Maze(20, 20);

// Create a 10x15 maze (150 cells total)
var smallMaze = new Maze(10, 15);

// This will throw an exception
try
{
    var invalid = new Maze(0, 10); // ArgumentException: "Rows must be positive"
}
catch (ArgumentException ex)
{
    Console.WriteLine(ex.Message);
}
```

### Methods

#### `GetCell(int row, int column)` → MazeCell
Gets the cell at the specified position.

**Parameters**:
- `row` (int): Row number (1-based, must be between 1 and Rows)
- `column` (int): Column number (1-based, must be between 1 and Columns)

**Returns**: The `MazeCell` at the specified position

**Throws**:
- `ArgumentOutOfRangeException`: If row or column is out of bounds

**Example**:
```csharp
var maze = new Maze(10, 10);

// Get cell at position (5, 7)
var cell = maze.GetCell(5, 7);
Console.WriteLine(cell); // Output: Cell(5, 7)

// This will throw an exception
try
{
    var invalid = maze.GetCell(0, 5); // Row 0 is invalid (must be ≥ 1)
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine(ex.Message);
}
```

#### `ConnectCells(int row1, int column1, int row2, int column2)` → bool
Connects two adjacent cells by creating a bidirectional edge between them.

**Parameters**:
- `row1` (int): Row of first cell (1-based)
- `column1` (int): Column of first cell (1-based)
- `row2` (int): Row of second cell (1-based)
- `column2` (int): Column of second cell (1-based)

**Returns**: 
- `true` if connection was successful
- `false` if either cell is frozen

**Throws**:
- `ArgumentException`: If cells are not adjacent (diagonal or too far apart)
- `ArgumentOutOfRangeException`: If any position is out of bounds

**Example**:
```csharp
var maze = new Maze(10, 10);

// Connect two adjacent cells (horizontally)
bool success = maze.ConnectCells(5, 5, 5, 6);
Console.WriteLine(success); // Output: True

// Check if they're connected
bool connected = maze.AreConnected(5, 5, 5, 6);
Console.WriteLine(connected); // Output: True

// Try to connect diagonal cells (will throw exception)
try
{
    maze.ConnectCells(5, 5, 6, 6); // Diagonal - not allowed!
}
catch (ArgumentException ex)
{
    Console.WriteLine(ex.Message); // "Cells must be adjacent (not diagonal)"
}

// Try to connect frozen cell
maze.FreezeCell(5, 5);
bool frozenResult = maze.ConnectCells(5, 5, 6, 5);
Console.WriteLine(frozenResult); // Output: False (cell is frozen)
```

#### `DisconnectCells(int row1, int column1, int row2, int column2)` → bool
Removes the bidirectional connection between two adjacent cells.

**Parameters**:
- `row1` (int): Row of first cell (1-based)
- `column1` (int): Column of first cell (1-based)
- `row2` (int): Row of second cell (1-based)
- `column2` (int): Column of second cell (1-based)

**Returns**: 
- `true` if disconnection was successful
- `false` if either cell is frozen

**Throws**:
- `ArgumentException`: If cells are not adjacent
- `ArgumentOutOfRangeException`: If any position is out of bounds

**Example**:
```csharp
var maze = new Maze(10, 10);

// First, connect two cells
maze.ConnectCells(5, 5, 5, 6);
Console.WriteLine(maze.AreConnected(5, 5, 5, 6)); // Output: True

// Now disconnect them
bool success = maze.DisconnectCells(5, 5, 5, 6);
Console.WriteLine(success); // Output: True
Console.WriteLine(maze.AreConnected(5, 5, 5, 6)); // Output: False

// Try to disconnect frozen cells
maze.ConnectCells(5, 5, 5, 6);
maze.FreezeCell(5, 5);
bool frozenResult = maze.DisconnectCells(5, 5, 5, 6);
Console.WriteLine(frozenResult); // Output: False (cell is frozen)
```

#### `FreezeCell(int row, int column)` → void
Freezes a cell, preventing any further modifications to its connections.

**Parameters**:
- `row` (int): Row number (1-based)
- `column` (int): Column number (1-based)

**Throws**:
- `ArgumentOutOfRangeException`: If position is out of bounds

**Example**:
```csharp
var maze = new Maze(10, 10);

// Connect some cells
maze.ConnectCells(5, 5, 5, 6);
maze.ConnectCells(5, 5, 6, 5);

// Freeze the cell
maze.FreezeCell(5, 5);
Console.WriteLine(maze.IsCellFrozen(5, 5)); // Output: True

// Now try to modify connections (will fail)
bool result1 = maze.ConnectCells(5, 5, 4, 5);
Console.WriteLine(result1); // Output: False

bool result2 = maze.DisconnectCells(5, 5, 5, 6);
Console.WriteLine(result2); // Output: False

// The existing connections remain intact
Console.WriteLine(maze.AreConnected(5, 5, 5, 6)); // Output: True
```

#### `IsCellFrozen(int row, int column)` → bool
Checks if a cell is frozen.

**Parameters**:
- `row` (int): Row number (1-based)
- `column` (int): Column number (1-based)

**Returns**: `true` if the cell is frozen; otherwise, `false`

**Throws**:
- `ArgumentOutOfRangeException`: If position is out of bounds

**Example**:
```csharp
var maze = new Maze(10, 10);

Console.WriteLine(maze.IsCellFrozen(5, 5)); // Output: False

maze.FreezeCell(5, 5);
Console.WriteLine(maze.IsCellFrozen(5, 5)); // Output: True

Console.WriteLine(maze.IsCellFrozen(5, 6)); // Output: False
```

#### `AreConnected(int row1, int column1, int row2, int column2)` → bool
Checks if two cells are connected by an edge.

**Parameters**:
- `row1` (int): Row of first cell (1-based)
- `column1` (int): Column of first cell (1-based)
- `row2` (int): Row of second cell (1-based)
- `column2` (int): Column of second cell (1-based)

**Returns**: `true` if the cells are connected; otherwise, `false`

**Throws**:
- `ArgumentOutOfRangeException`: If any position is out of bounds

**Example**:
```csharp
var maze = new Maze(10, 10);

// Initially, no cells are connected
Console.WriteLine(maze.AreConnected(5, 5, 5, 6)); // Output: False

// Connect cells
maze.ConnectCells(5, 5, 5, 6);
Console.WriteLine(maze.AreConnected(5, 5, 5, 6)); // Output: True

// Connection is bidirectional
Console.WriteLine(maze.AreConnected(5, 6, 5, 5)); // Output: True

// Disconnect
maze.DisconnectCells(5, 5, 5, 6);
Console.WriteLine(maze.AreConnected(5, 5, 5, 6)); // Output: False
```

#### `GetNeighbors(int row, int column)` → IReadOnlyCollection<MazeCell>
Gets all currently connected neighbors of a cell.

**Parameters**:
- `row` (int): Row number (1-based)
- `column` (int): Column number (1-based)

**Returns**: Read-only collection of connected neighbor cells

**Throws**:
- `ArgumentOutOfRangeException`: If position is out of bounds

**Example**:
```csharp
var maze = new Maze(10, 10);

// Initially, no neighbors
var neighbors = maze.GetNeighbors(5, 5);
Console.WriteLine(neighbors.Count); // Output: 0

// Connect to some adjacent cells
maze.ConnectCells(5, 5, 5, 6); // Right
maze.ConnectCells(5, 5, 6, 5); // Top
maze.ConnectCells(5, 5, 4, 5); // Bottom

neighbors = maze.GetNeighbors(5, 5);
Console.WriteLine(neighbors.Count); // Output: 3

foreach (var neighbor in neighbors)
{
    Console.WriteLine(neighbor);
    // Output: Cell(5, 6), Cell(6, 5), Cell(4, 5)
}
```

#### `GetAdjacentCells(int row, int column)` → List<MazeCell>
Gets all potential adjacent cells (up, down, left, right) that exist within maze bounds, regardless of whether they're connected.

**Parameters**:
- `row` (int): Row number (1-based)
- `column` (int): Column number (1-based)

**Returns**: List of adjacent cells within maze bounds

**Throws**:
- `ArgumentOutOfRangeException`: If position is out of bounds

**Example**:
```csharp
var maze = new Maze(10, 10);

// Cell in the middle has 4 adjacent cells
var adjacent = maze.GetAdjacentCells(5, 5);
Console.WriteLine(adjacent.Count); // Output: 4
// Returns cells at (6,5), (4,5), (5,6), (5,4)

// Corner cell has only 2 adjacent cells
adjacent = maze.GetAdjacentCells(1, 1);
Console.WriteLine(adjacent.Count); // Output: 2
// Returns cells at (2,1) and (1,2)

// Edge cell has 3 adjacent cells
adjacent = maze.GetAdjacentCells(1, 5);
Console.WriteLine(adjacent.Count); // Output: 3
// Returns cells at (2,5), (1,6), (1,4)

// This method returns potential neighbors, not connected ones
// To get connected neighbors, use GetNeighbors()
```

### Example 6: Complete Workflow

```csharp
using maze_generation_algorithms;
using maze_generation_algorithms.DataStructures;

class MazeGenerationExample
{
    public static void Main()
    {
        // Step 1: Create template with desired dimensions
        var template = new Maze(25, 25);
        
        // Step 2: Define frozen regions (optional)
        // Create entrance
        template.FreezeCell(1, 1);
        template.ConnectCells(1, 1, 1, 2);
        template.ConnectCells(1, 1, 2, 1);
        template.FreezeCell(1, 2);
        template.FreezeCell(2, 1);
        
        // Create exit
        template.FreezeCell(25, 25);
        template.FreezeCell(25, 24);
        template.FreezeCell(24, 25);
        template.ConnectCells(25, 25, 25, 24);
        template.ConnectCells(25, 25, 24, 25);
        
        // Step 3: Create generator
        var generator = new MazeGenerator(
            MazeAlgorithm.RandomWithValidPath, 
            seed: DateTime.Now.Millisecond // Random seed
        );
        
        // Step 4: Generate maze
        var maze = generator.Generate(in template);
        
        // Step 5: Use the maze
        Console.WriteLine($"Generated {maze.Rows}x{maze.Columns} maze");
        Console.WriteLine($"Entrance frozen: {maze.IsCellFrozen(1, 1)}");
        Console.WriteLine($"Exit frozen: {maze.IsCellFrozen(25, 25)}");
        
        // Step 6: Verify connectivity
        Console.WriteLine($"Entrance has {maze.GetNeighbors(1, 1).Count} connections");
        Console.WriteLine($"Exit has {maze.GetNeighbors(25, 25).Count} connections");
        
        // Template is unchanged - can generate more variations
        var maze2 = generator.Generate(in template);
        var maze3 = generator.Generate(in template);
    }
}
```

### Example 7: Custom Algorithm Implementation

```csharp
using maze_generation_algorithms.Algorithms;
using maze_generation_algorithms.DataStructures;

// Implement a custom algorithm
public class SimpleGridAlgorithm : IMazeAlgorithm
{
    public Maze Generate(in Maze maze, in int seed)
    {
        // Clone to avoid modifying original
        var mazeCopy = maze.Clone();
        var random = new Random(seed);
        
        // Connect cells in a simple grid pattern
        for (int row = 1; row <= mazeCopy.Rows; row++)
        {
            for (int col = 1; col <= mazeCopy.Columns; col++)
            {
                // Skip frozen cells
                if (mazeCopy.IsCellFrozen(row, col))
                    continue;
                
                // Randomly connect right
                if (col < mazeCopy.Columns && 
                    !mazeCopy.IsCellFrozen(row, col + 1) &&
                    random.Next(2) == 0)
                {
                    mazeCopy.ConnectCells(row, col, row, col + 1);
                }
                
                // Randomly connect down
                if (row > 1 && 
                    !mazeCopy.IsCellFrozen(row - 1, col) &&
                    random.Next(2) == 0)
                {
                    mazeCopy.ConnectCells(row, col, row - 1, col);
                }
            }
        }
        
        return mazeCopy;
    }
}

// Usage
var template = new Maze(10, 10);
var algorithm = new SimpleGridAlgorithm();
var maze = algorithm.Generate(in template, in 12345);
```

---

## Best Practices

### 1. Always Use 1-Based Indexing
```csharp
// ✓ CORRECT
var cell = maze.GetCell(1, 1); // First cell

// ✗ WRONG
var cell = maze.GetCell(0, 0); // Will throw exception
```

### 2. Never Modify Input Mazes in Algorithms
```csharp
// ✓ CORRECT - Clone first
public Maze Generate(in Maze maze, in int seed)
{
    var mazeCopy = maze.Clone();
    // Work on mazeCopy
    return mazeCopy;
}

// ✗ WRONG - Modifies original
public Maze Generate(in Maze maze, in int seed)
{
    maze.ConnectCells(1, 1, 1, 2); // Don't do this!
    return maze;
}
```

### 3. Use `in` Keyword for Readonly Semantics
```csharp
// ✓ CORRECT - Indicates no modification
public Maze Generate(in Maze maze, in int seed)

// ✓ ALSO CORRECT - When calling
var result = generator.Generate(in template);
```

### 4. Check Frozen Status Before Modification
```csharp
// ✓ CORRECT
if (!maze.IsCellFrozen(5, 5) && !maze.IsCellFrozen(5, 6))
{
    maze.ConnectCells(5, 5, 5, 6);
}

// ✓ ALSO CORRECT - Check return value
bool success = maze.ConnectCells(5, 5, 5, 6);
if (!success)
{
    Console.WriteLine("Connection failed - cell may be frozen");
}
```

### 5. Use Seeds for Reproducibility
```csharp
// ✓ CORRECT - Reproducible generation
int seed = 12345;
var gen1 = new MazeGenerator(MazeAlgorithm.RandomWithValidPath, seed);
var gen2 = new MazeGenerator(MazeAlgorithm.RandomWithValidPath, seed);

var maze1 = gen1.Generate(in template);
var maze2 = gen2.Generate(in template);
// maze1 and maze2 are identical

// ✓ FOR VARIETY - Use different seeds
var gen3 = new MazeGenerator(MazeAlgorithm.RandomWithValidPath, seed: 67890);
var maze3 = gen3.Generate(in template);
// maze3 is different
```

### 6. Reuse Templates for Multiple Generations
```csharp
// ✓ CORRECT - Efficient reuse
var template = new Maze(20, 20);
// Set up frozen cells once
template.FreezeCell(1, 1);
template.FreezeCell(20, 20);

// Generate multiple variations
var gen = new MazeGenerator(MazeAlgorithm.RandomWithValidPath, seed: 1);
var maze1 = gen.Generate(in template); // Template unchanged
var maze2 = gen.Generate(in template); // Can reuse
var maze3 = gen.Generate(in template); // Still unchanged
```

### 7. Validate Maze Connectivity
```csharp
// ✓ CORRECT - Verify your maze
public bool IsFullyConnected(Maze maze)
{
    var visited = new HashSet<(int, int)>();
    var queue = new Queue<(int, int)>();
    
    queue.Enqueue((1, 1));
    visited.Add((1, 1));
    
    while (queue.Count > 0)
    {
        var (row, col) = queue.Dequeue();
        var neighbors = maze.GetNeighbors(row, col);
        
        foreach (var neighbor in neighbors)
        {
            var pos = (neighbor.Row, neighbor.Column);
            if (!visited.Contains(pos))
            {
                visited.Add(pos);
                queue.Enqueue(pos);
            }
        }
    }
    
    return visited.Count == maze.Rows * maze.Columns;
}
```

### 8. Handle Algorithm Exceptions
```csharp
// ✓ CORRECT - Handle not implemented algorithms
try
{
    var generator = new MazeGenerator(MazeAlgorithm.BinaryTree, seed: 123);
    var maze = generator.Generate(in template);
}
catch (NotImplementedException ex)
{
    Console.WriteLine($"Algorithm not yet implemented: {ex.Message}");
    // Fall back to a working algorithm
    var fallback = new MazeGenerator(MazeAlgorithm.RandomWithValidPath, seed: 123);
    var maze = fallback.Generate(in template);
}
```

### 9. Freeze Critical Sections
```csharp
// ✓ CORRECT - Protect important areas
var template = new Maze(30, 30);

// Freeze entrance corridor
for (int col = 1; col <= 5; col++)
{
    template.FreezeCell(1, col);
    if (col < 5)
        template.ConnectCells(1, col, 1, col + 1);
}

// Freeze exit area  
template.FreezeCell(30, 30);
template.FreezeCell(30, 29);
template.ConnectCells(30, 30, 30, 29);

// Freeze critical junctions
template.FreezeCell(15, 15);
```

### 10. Use Clone for Intermediate Modifications
```csharp
// ✓ CORRECT - Test modifications without affecting original
var original = CreateMyComplexMaze();

var test1 = original.Clone();
test1.ConnectCells(5, 5, 5, 6);
if (IsValid(test1))
{
    // Use test1
}

var test2 = original.Clone();
test2.ConnectCells(5, 5, 6, 5);
if (IsValid(test2))
{
    // Use test2
}

// Original is still unchanged
```

---

## Common Patterns

### Pattern 1: Template-Based Generation

```csharp
// Create reusable template
public Maze CreateStandardTemplate(int size)
{
    var template = new Maze(size, size);
    
    // Always freeze corners
    template.FreezeCell(1, 1);
    template.FreezeCell(1, size);
    template.FreezeCell(size, 1);
    template.FreezeCell(size, size);
    
    return template;
}

// Use template for multiple generations
var template = CreateStandardTemplate(20);
var gen = new MazeGenerator(MazeAlgorithm.RandomWithValidPath, seed: 123);

var maze1 = gen.Generate(in template);
var maze2 = gen.Generate(in template);
var maze3 = gen.Generate(in template);
```

### Pattern 2: Progressive Difficulty

```csharp
public class DifficultyGenerator
{
    public Maze GenerateEasy(int size, int seed)
    {
        var template = new Maze(size, size);
        // Add more frozen paths for easier navigation
        CreateMainPath(template);
        CreateShortcuts(template, count: 3);
        
        var gen = new MazeGenerator(MazeAlgorithm.RandomWithValidPath, seed);
        return gen.Generate(in template);
    }
    
    public Maze GenerateHard(int size, int seed)
    {
        var template = new Maze(size, size);
        // Minimal frozen cells for harder maze
        template.FreezeCell(1, 1);
        template.FreezeCell(size, size);
        
        var gen = new MazeGenerator(MazeAlgorithm.RandomWithValidPath, seed);
        return gen.Generate(in template);
    }
    
    private void CreateMainPath(Maze maze) { /* ... */ }
    private void CreateShortcuts(Maze maze, int count) { /* ... */ }
}
```

### Pattern 3: Maze Serialization

```csharp
public class MazeSerializer
{
    public string Serialize(Maze maze)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{maze.Rows},{maze.Columns}");
        
        for (int row = 1; row <= maze.Rows; row++)
        {
            for (int col = 1; col <= maze.Columns; col++)
            {
                var cell = maze.GetCell(row, col);
                sb.Append(maze.IsCellFrozen(row, col) ? "F" : "U");
                sb.Append("|");
                
                // Store connections
                var neighbors = maze.GetNeighbors(row, col);
                foreach (var n in neighbors)
                {
                    sb.Append($"{n.Row},{n.Column};");
                }
                sb.AppendLine();
            }
        }
        
        return sb.ToString();
    }
}
```

### Pattern 4: Algorithm Comparison

```csharp
public class AlgorithmComparer
{
    public void CompareAlgorithms(Maze template, int seed)
    {
        var algorithms = new[]
        {
            MazeAlgorithm.RandomWithValidPath,
            // Add more when implemented
        };
        
        foreach (var algo in algorithms)
        {
            try
            {
                var gen = new MazeGenerator(algo, seed);
                var maze = gen.Generate(in template);
                
                Console.WriteLine($"{algo}:");
                Console.WriteLine($"  Connectivity: {CheckConnectivity(maze)}");
                Console.WriteLine($"  Avg Neighbors: {CalculateAvgNeighbors(maze):F2}");
                Console.WriteLine($"  Dead Ends: {CountDeadEnds(maze)}");
            }
            catch (NotImplementedException)
            {
                Console.WriteLine($"{algo}: Not implemented");
            }
        }
    }
    
    private bool CheckConnectivity(Maze maze) { /* BFS check */ }
    private double CalculateAvgNeighbors(Maze maze) { /* Calculate */ }
    private int CountDeadEnds(Maze maze) { /* Count cells with 1 neighbor */ }
}
```

---

## API Reference Summary

### Core Classes

| Class | Namespace | Purpose |
|-------|-----------|---------|
| `Cell` | `Models` | Basic cell with coordinates |
| `MazeCell` | `Models` | Cell with connections and frozen state |
| `Maze` | `DataStructures` | Grid structure with connection management |
| `IMazeAlgorithm` | `Algorithms` | Interface for maze generation algorithms |
| `MazeGenerator` | Root | Main entry point with factory pattern |
| `RandomWithValidPathAlgorithm` | `Algorithms` | Specific algorithm implementation |

### Key Methods

| Method | Class | Returns | Description |
|--------|-------|---------|-------------|
| `ConnectCells()` | Maze | bool | Creates bidirectional connection |
| `DisconnectCells()` | Maze | bool | Removes connection |
| `FreezeCell()` | Maze | void | Prevents cell modification |
| `IsCellFrozen()` | Maze | bool | Checks frozen status |
| `Clone()` | Maze | Maze | Deep copies maze |
| `Generate()` | MazeGenerator | Maze | Generates maze (immutable) |
| `Generate()` | IMazeAlgorithm | Maze | Algorithm implementation |

---

## Version History

**Version 2.0** (November 29, 2025)
- Added `IMazeAlgorithm` interface for algorithm abstraction
- Added `MazeGenerator` class with factory pattern
- Added `RandomWithValidPathAlgorithm` implementation
- Added `Clone()` method to Maze class for deep copying
- Implemented call-by-value semantics (immutable generation)
- Added `in` keyword for readonly parameters
- Updated all examples and best practices

**Version 1.0** (November 26, 2025)
- Initial release
- Cell, MazeCell, and Maze classes
- Support for 1-based indexing
- Freeze functionality
- Adjacent-only connections

---

## License

This documentation is part of the Maze Generator project by AmanRathoreP.

---

*Last Updated: November 29, 2025*

```csharp
using maze_generation_algorithms.DataStructures;
using maze_generation_algorithms.Models;

// Create a 5x5 maze
var maze = new Maze(5, 5);

// Create a simple path from (1,1) to (1,5)
maze.ConnectCells(1, 1, 1, 2);
maze.ConnectCells(1, 2, 1, 3);
maze.ConnectCells(1, 3, 1, 4);
maze.ConnectCells(1, 4, 1, 5);

// Verify the path
Console.WriteLine(maze.AreConnected(1, 1, 1, 2)); // True
Console.WriteLine(maze.AreConnected(1, 1, 1, 5)); // False (not directly connected)

// Check neighbors
var cell = maze.GetCell(1, 3);
var neighbors = maze.GetNeighbors(1, 3);
Console.WriteLine(neighbors.Count); // 2 (connected to (1,2) and (1,4))
```

### Example 2: Working with Frozen Cells

```csharp
var maze = new Maze(10, 10);

// Create a protected area by connecting and freezing cells
maze.ConnectCells(5, 5, 5, 6);
maze.ConnectCells(5, 5, 6, 5);
maze.FreezeCell(5, 5);

// Try to modify the frozen cell
bool result = maze.ConnectCells(5, 5, 4, 5);
Console.WriteLine($"Connection attempt: {result}"); // False

// The existing connections remain
Console.WriteLine($"Still connected to (5,6): {maze.AreConnected(5, 5, 5, 6)}"); // True

// Other cells can still be modified
maze.ConnectCells(5, 6, 5, 7); // Works fine
Console.WriteLine($"New connection: {maze.AreConnected(5, 6, 5, 7)}"); // True
```

### Example 3: Building a Maze with Start and End Points

```csharp
var maze = new Maze(20, 20);

// Define start (1,1) and end (20,20)
var start = maze.GetCell(1, 1);
var end = maze.GetCell(20, 20);

// Create a winding path from start to end
maze.ConnectCells(1, 1, 1, 2);
maze.ConnectCells(1, 2, 2, 2);
maze.ConnectCells(2, 2, 2, 3);
// ... continue building path ...

// Freeze start and end to prevent modification
maze.FreezeCell(1, 1);
maze.FreezeCell(20, 20);

Console.WriteLine($"Start frozen: {maze.IsCellFrozen(1, 1)}"); // True
Console.WriteLine($"End frozen: {maze.IsCellFrozen(20, 20)}"); // True
```

### Example 4: Exploring Adjacent vs Connected Cells

```csharp
var maze = new Maze(10, 10);

// Cell (5,5) has 4 adjacent cells
var adjacentCells = maze.GetAdjacentCells(5, 5);
Console.WriteLine($"Adjacent cells: {adjacentCells.Count}"); // 4

// But initially no connected neighbors
var neighbors = maze.GetNeighbors(5, 5);
Console.WriteLine($"Connected neighbors: {neighbors.Count}"); // 0

// Connect to two adjacent cells
maze.ConnectCells(5, 5, 5, 6);
maze.ConnectCells(5, 5, 6, 5);

// Now we have 2 connected neighbors
neighbors = maze.GetNeighbors(5, 5);
Console.WriteLine($"Connected neighbors: {neighbors.Count}"); // 2

// But still 4 adjacent cells
adjacentCells = maze.GetAdjacentCells(5, 5);
Console.WriteLine($"Adjacent cells: {adjacentCells.Count}"); // 4
```

### Example 5: Grid Boundary Handling

```csharp
var maze = new Maze(10, 10);

// Corner cell (1,1) has only 2 adjacent cells
var corner = maze.GetAdjacentCells(1, 1);
Console.WriteLine($"Corner adjacent: {corner.Count}"); // 2
// Can connect to (2,1) and (1,2)

// Edge cell (1,5) has 3 adjacent cells
var edge = maze.GetAdjacentCells(1, 5);
Console.WriteLine($"Edge adjacent: {edge.Count}"); // 3
// Can connect to (2,5), (1,4), and (1,6)

// Middle cell (5,5) has 4 adjacent cells
var middle = maze.GetAdjacentCells(5, 5);
Console.WriteLine($"Middle adjacent: {middle.Count}"); // 4
// Can connect to (6,5), (4,5), (5,6), and (5,4)
```

#### `Clone()` → Maze
Creates a deep copy of the maze with all cells, connections, and frozen states preserved in separate memory.

**Returns**: A new `Maze` instance with identical structure

**Example**:
```csharp
var maze = new Maze(10, 10);

// Set up some connections and frozen cells
maze.ConnectCells(1, 1, 1, 2);
maze.ConnectCells(1, 2, 1, 3);
maze.FreezeCell(1, 1);

// Clone the maze
var clonedMaze = maze.Clone();

// The clone is independent - modifying it doesn't affect original
clonedMaze.ConnectCells(2, 2, 2, 3);
Console.WriteLine(maze.AreConnected(2, 2, 2, 3)); // Output: False (original unchanged)
Console.WriteLine(clonedMaze.AreConnected(2, 2, 2, 3)); // Output: True (clone modified)

// Frozen states are preserved
Console.WriteLine(clonedMaze.IsCellFrozen(1, 1)); // Output: True

// Connections are preserved
Console.WriteLine(clonedMaze.AreConnected(1, 1, 1, 2)); // Output: True
```

**Implementation Details**:
- Uses 3-pass algorithm for correctness:
  1. First pass: Creates cell mapping
  2. Second pass: Copies all connections (before freezing)
  3. Third pass: Freezes cells after connections are established
- Completely independent memory space
- All frozen states and connections preserved exactly

---

## IMazeAlgorithm Interface

**Namespace**: `maze_generation_algorithms.Algorithms`  
**File**: `Algorithms/IMazeAlgorithm.cs`

### Description
Defines the contract that all maze generation algorithms must implement. Ensures consistent behavior across different algorithm implementations and enables the Strategy pattern for algorithm selection.

### Interface Definition

```csharp
public interface IMazeAlgorithm
{
    Maze Generate(in Maze maze, in int seed);
}
```

### Method Signature

#### `Generate(in Maze maze, in int seed)` → Maze
Generates a maze using the specific algorithm implementation.

**Parameters**:
- `maze` (Maze): The maze template to use. Marked with `in` keyword for readonly semantics.
- `seed` (int): Random seed for reproducible generation. Marked with `in` keyword for readonly semantics.

**Returns**: A new `Maze` instance with generated paths. The original maze is never modified.

**Contract Guarantees**:
- Original maze parameter is never modified (call-by-value semantics)
- Returns a new maze instance in separate memory
- Frozen cells and their connections are preserved
- Same seed produces same result (reproducible)
- All cells remain reachable (maintains connectivity)

**Example Implementation**:
```csharp
public class MyCustomAlgorithm : IMazeAlgorithm
{
    public Maze Generate(in Maze maze, in int seed)
    {
        // Clone the input to avoid modifying original
        var mazeCopy = maze.Clone();
        
        // Implement your algorithm logic here
        // Work only on mazeCopy
        
        return mazeCopy;
    }
}
```

### Why Use This Interface?

1. **Polymorphism**: Swap algorithms without changing client code
2. **Testability**: Easy to mock for unit testing
3. **Extensibility**: Add new algorithms by implementing the interface
4. **Type Safety**: Compile-time checking of algorithm compatibility
5. **Consistency**: All algorithms follow the same contract

---

## MazeGenerator Class

**Namespace**: `maze_generation_algorithms`  
**File**: `MazeGenerator.cs`

### Description
Main entry point for maze generation. Uses the Factory pattern to create algorithm instances and provides a clean API for generating mazes. Ensures immutability by never modifying input mazes.

### Enums

#### `MazeAlgorithm`
Available maze generation algorithms.

**Values**:
- `BinaryTree` - Binary tree algorithm (not yet implemented)
- `Prim` - Prim's algorithm (not yet implemented)
- `RandomWithValidPath` - Random path generation with connectivity guarantee

**Example**:
```csharp
var algorithm = MazeAlgorithm.RandomWithValidPath;
```

### Constructor

#### `MazeGenerator(MazeAlgorithm algorithm, int seed)`
Creates a new maze generator with the specified algorithm and seed.

**Parameters**:
- `algorithm` (MazeAlgorithm): The algorithm to use for maze generation
- `seed` (int): Random seed for reproducible results

**Example**:
```csharp
// Create generator with RandomWithValidPath algorithm
var generator = new MazeGenerator(MazeAlgorithm.RandomWithValidPath, seed: 12345);

// Create with different seed for different results
var generator2 = new MazeGenerator(MazeAlgorithm.RandomWithValidPath, seed: 67890);
```

### Methods

#### `Generate(in Maze maze)` → Maze
Generates a maze using the configured algorithm. The original maze is never modified.

**Parameters**:
- `maze` (Maze): The maze template. Original will not be modified. Marked with `in` for readonly.

**Returns**: A new generated maze with connections. Original maze is unchanged.

**Throws**:
- `ArgumentNullException`: If maze is null
- `NotImplementedException`: If selected algorithm is not yet implemented

**Example**:
```csharp
// Create a blank 10x10 maze template
var template = new Maze(10, 10);

// Optionally freeze some cells
template.FreezeCell(1, 1);
template.FreezeCell(10, 10);

// Generate maze
var generator = new MazeGenerator(MazeAlgorithm.RandomWithValidPath, seed: 12345);
var generatedMaze = generator.Generate(in template);

// Template is unchanged - can reuse for different variations
var variation1 = generator.Generate(in template);
var variation2 = generator.Generate(in template);

// All three mazes (generatedMaze, variation1, variation2) are identical
// because same seed was used
```

### Factory Method (Private)

#### `CreateAlgorithm(MazeAlgorithm algorithm)` → IMazeAlgorithm
Creates an instance of the appropriate algorithm implementation.

**Parameters**:
- `algorithm` (MazeAlgorithm): The algorithm enum value

**Returns**: An instance implementing `IMazeAlgorithm`

**Throws**:
- `NotImplementedException`: For algorithms not yet implemented
- `ArgumentException`: For unknown algorithm values

**Internal Implementation**:
```csharp
private IMazeAlgorithm CreateAlgorithm(MazeAlgorithm algorithm)
{
    return algorithm switch
    {
        MazeAlgorithm.RandomWithValidPath => new RandomWithValidPathAlgorithm(),
        MazeAlgorithm.BinaryTree => throw new NotImplementedException(...),
        MazeAlgorithm.Prim => throw new NotImplementedException(...),
        _ => throw new ArgumentException(...)
    };
}
```

---

## RandomWithValidPath Algorithm

**Namespace**: `maze_generation_algorithms.Algorithms`  
**File**: `Algorithms/RandomWithValidPathAlgorithm.cs`  
**Implements**: `IMazeAlgorithm`

### Description
Generates a maze by randomly adding connections while ensuring all cells remain reachable. Creates a sparse maze structure (minimal loops) with guaranteed connectivity. Respects frozen cells and their connections.

### Algorithm Overview

The algorithm works in several phases:

1. **Clone Input**: Creates deep copy to avoid modifying original
2. **Edge Collection**: Gathers all potential edges between adjacent cells
3. **Edge Separation**: Separates frozen edges from modifiable edges
4. **Random Shuffling**: Randomizes the order of non-frozen edges
5. **Selective Connection**: For each edge in random order:
   - If cells aren't connected via any path → Add connection (ensures connectivity)
   - If cells already connected → 10% chance to add (creates sparse loops)
6. **Return**: Returns the modified copy

### Key Characteristics

- **Guaranteed Connectivity**: All cells are reachable from any other cell
- **Sparse Structure**: Minimal loops (10% redundant connections)
- **Frozen Cell Respect**: Never modifies frozen cell connections
- **Reproducible**: Same seed produces identical maze
- **Efficient**: Uses BFS for connectivity checking

### Method

#### `Generate(in Maze maze, in int seed)` → Maze
Implements the IMazeAlgorithm interface.

**Parameters**:
- `maze` (Maze): Template maze (not modified)
- `seed` (int): Random seed for reproducibility

**Returns**: New maze with random valid paths

**Example**:
```csharp
var template = new Maze(15, 15);

// Set up a frozen corridor
template.ConnectCells(1, 1, 1, 2);
template.ConnectCells(1, 2, 1, 3);
template.FreezeCell(1, 1);
template.FreezeCell(1, 2);
template.FreezeCell(1, 3);

var algorithm = new RandomWithValidPathAlgorithm();
var generatedMaze = algorithm.Generate(in template, in 12345);

// Template unchanged, generatedMaze has new structure
// Frozen corridor preserved exactly
Console.WriteLine(generatedMaze.AreConnected(1, 1, 1, 2)); // True (preserved)
Console.WriteLine(generatedMaze.IsCellFrozen(1, 1)); // True (preserved)
```

### Private Helper Methods

#### `GetAllPotentialEdges(Maze maze)` → List<(MazeCell, MazeCell)>
Collects all potential edges (pairs of adjacent cells) in the maze.

**Parameters**:
- `maze` (Maze): The maze to analyze

**Returns**: List of cell pairs representing all possible connections

**Logic**:
- Iterates through all cells
- For each cell, checks right and bottom neighbors
- Avoids duplicate edges (only stores each edge once)

#### `AreConnectedThroughPath(Maze maze, MazeCell start, MazeCell target)` → bool
Checks if two cells are already connected through any path using BFS traversal.

**Parameters**:
- `maze` (Maze): The maze to check
- `start` (MazeCell): Starting cell
- `target` (MazeCell): Target cell

**Returns**: `true` if path exists; otherwise `false`

**Algorithm**: Breadth-First Search (BFS)

#### `IsFullyConnected(Maze maze)` → bool
Verifies that all cells in the maze are reachable from any starting point.

**Parameters**:
- `maze` (Maze): The maze to validate

**Returns**: `true` if all cells are connected; otherwise `false`

**Algorithm**: BFS from (1,1), counts visited cells

### Algorithm Complexity

- **Time Complexity**: O(V * E) where V = cells, E = edges
  - Each edge is processed once
  - BFS for each edge is O(V + E)
- **Space Complexity**: O(V + E)
  - Stores all edges
  - BFS visited set

### Maze Characteristics

Mazes generated by this algorithm have:
- **High Difficulty**: Many dead ends due to sparse connections
- **Single Solution Paths**: Minimal loops mean fewer alternative routes
- **Good for Puzzles**: Clear start-to-end challenges
- **Efficient**: Fast generation even for large mazes

### Customization

To adjust maze density, modify the redundant connection probability:

```csharp
// Current: 10% chance for redundant connections
if (random.NextDouble() < 0.1)

// More loops (easier maze): increase to 0.3 (30%)
if (random.NextDouble() < 0.3)

// Fewer loops (harder maze): decrease to 0.05 (5%)
if (random.NextDouble() < 0.05)

// No loops (perfect maze): remove this block entirely
```

---

## Usage Examples

### Example 1: Basic Maze Generation

```csharp
using maze_generation_algorithms;
using maze_generation_algorithms.DataStructures;

// Create a 20x20 maze template
var template = new Maze(20, 20);

// Create generator with specific algorithm and seed
var generator = new MazeGenerator(MazeAlgorithm.RandomWithValidPath, seed: 12345);

// Generate maze (template is not modified)
var maze = generator.Generate(in template);

// Verify maze properties
Console.WriteLine($"Maze size: {maze.Rows}x{maze.Columns}");

// Check a specific cell's connections
var neighbors = maze.GetNeighbors(10, 10);
Console.WriteLine($"Cell (10,10) has {neighbors.Count} connections");
```

### Example 2: Maze Generation with Frozen Sections

```csharp
using maze_generation_algorithms;
using maze_generation_algorithms.DataStructures;

// Create template
var template = new Maze(20, 20);

// Create a preserved starting corridor
for (int col = 1; col <= 5; col++)
{
    template.FreezeCell(1, col);
    if (col < 5)
        template.ConnectCells(1, col, 1, col + 1);
}

// Create a preserved ending area
template.FreezeCell(20, 20);
template.FreezeCell(20, 19);
template.FreezeCell(19, 20);
template.ConnectCells(20, 20, 20, 19);
template.ConnectCells(20, 20, 19, 20);

// Generate maze
var generator = new MazeGenerator(MazeAlgorithm.RandomWithValidPath, seed: 99999);
var maze = generator.Generate(in template);

// Frozen sections are preserved
Console.WriteLine(maze.AreConnected(1, 1, 1, 2)); // True (preserved)
Console.WriteLine(maze.IsCellFrozen(1, 1)); // True (preserved)
Console.WriteLine(maze.AreConnected(20, 20, 20, 19)); // True (preserved)
```

### Example 3: Generating Multiple Variations

```csharp
// Create one template
var template = new Maze(15, 15);

// Generate multiple mazes with different seeds
var gen1 = new MazeGenerator(MazeAlgorithm.RandomWithValidPath, seed: 1);
var gen2 = new MazeGenerator(MazeAlgorithm.RandomWithValidPath, seed: 2);
var gen3 = new MazeGenerator(MazeAlgorithm.RandomWithValidPath, seed: 3);

var maze1 = gen1.Generate(in template);
var maze2 = gen2.Generate(in template);
var maze3 = gen3.Generate(in template);

// All three mazes are different because of different seeds
// Template remains unchanged and can be reused
```

### Example 4: Reproducible Maze Generation

```csharp
var template = new Maze(10, 10);
int seed = 42;

// Generate first maze
var generator1 = new MazeGenerator(MazeAlgorithm.RandomWithValidPath, seed);
var maze1 = generator1.Generate(in template);

// Generate second maze with same seed
var generator2 = new MazeGenerator(MazeAlgorithm.RandomWithValidPath, seed);
var maze2 = generator2.Generate(in template);

// Both mazes are identical
// Can verify by checking connections at same positions
Console.WriteLine(maze1.AreConnected(5, 5, 5, 6) == 
                  maze2.AreConnected(5, 5, 5, 6)); // True
```

### Example 5: Cloning Mazes

```csharp
// Create and populate a maze
var originalMaze = new Maze(10, 10);
originalMaze.ConnectCells(1, 1, 1, 2);
originalMaze.ConnectCells(1, 2, 2, 2);
originalMaze.FreezeCell(1, 1);

// Clone the maze
var clonedMaze = originalMaze.Clone();

// Modify the clone
clonedMaze.ConnectCells(3, 3, 3, 4);

// Original is unchanged
Console.WriteLine(originalMaze.AreConnected(3, 3, 3, 4)); // False
Console.WriteLine(clonedMaze.AreConnected(3, 3, 3, 4)); // True

// Both have the original connections
Console.WriteLine(originalMaze.AreConnected(1, 1, 1, 2)); // True
Console.WriteLine(clonedMaze.AreConnected(1, 1, 1, 2)); // True

// Frozen states are preserved
Console.WriteLine(clonedMaze.IsCellFrozen(1, 1)); // True
```

### Example 6: Complete Workflow

### Example 6: Complete Maze Generation Pattern

```csharp
using maze_generation_algorithms.DataStructures;

class MazeExample
{
    public static void GenerateSimpleMaze()
    {
        // Create a 10x10 maze
        var maze = new Maze(10, 10);
        
        // Create a grid pattern with some paths
        for (int row = 1; row <= 10; row++)
        {
            for (int col = 1; col < 10; col++)
            {
                // Connect every cell horizontally
                maze.ConnectCells(row, col, row, col + 1);
            }
        }
        
        for (int col = 1; col <= 10; col++)
        {
            for (int row = 1; row < 10; row++)
            {
                // Connect every other cell vertically
                if (row % 2 == 0)
                {
                    maze.ConnectCells(row, col, row + 1, col);
                }
            }
        }
        
        // Freeze the entrance and exit
        maze.FreezeCell(1, 1);   // Entrance
        maze.FreezeCell(10, 10); // Exit
        
        // Display some statistics
        Console.WriteLine($"Maze dimensions: {maze.Rows}x{maze.Columns}");
        Console.WriteLine($"Total cells: {maze.Rows * maze.Columns}");
        
        // Check a cell in the middle
        var neighbors = maze.GetNeighbors(5, 5);
        Console.WriteLine($"Cell (5,5) has {neighbors.Count} connections");
    }
}
```

---

## Best Practices

### 1. Always Use 1-Based Indexing
```csharp
// ✓ CORRECT
var cell = maze.GetCell(1, 1); // First cell

// ✗ WRONG
var cell = maze.GetCell(0, 0); // Will throw exception
```

### 2. Check Cell Frozen Status Before Modification
```csharp
// ✓ CORRECT
if (!maze.IsCellFrozen(5, 5))
{
    maze.ConnectCells(5, 5, 5, 6);
}

// ✓ ALSO CORRECT - Check return value
bool success = maze.ConnectCells(5, 5, 5, 6);
if (!success)
{
    Console.WriteLine("Connection failed - cell may be frozen");
}
```

### 3. Validate Adjacency Before Connection
```csharp
// ✓ CORRECT - Only connect adjacent cells
maze.ConnectCells(5, 5, 5, 6); // Horizontal neighbor
maze.ConnectCells(5, 5, 6, 5); // Vertical neighbor

// ✗ WRONG - Will throw exception
maze.ConnectCells(5, 5, 6, 6); // Diagonal - not allowed!
maze.ConnectCells(5, 5, 7, 5); // Too far apart - not adjacent
```

### 4. Use GetAdjacentCells for Maze Generation
```csharp
// ✓ CORRECT - Get all potential neighbors
var cell = maze.GetCell(5, 5);
var potentialNeighbors = maze.GetAdjacentCells(5, 5);

// Randomly connect to some neighbors
var random = new Random();
foreach (var neighbor in potentialNeighbors)
{
    if (random.Next(2) == 0)
    {
        maze.ConnectCells(cell.Row, cell.Column, neighbor.Row, neighbor.Column);
    }
}
```

### 5. Freeze Critical Cells
```csharp
// ✓ CORRECT - Protect important maze features
// Freeze start and end points
maze.FreezeCell(1, 1);
maze.FreezeCell(maze.Rows, maze.Columns);

// Freeze key junction points
maze.FreezeCell(10, 10);
maze.FreezeCell(10, 20);
```

### 6. Handle Exceptions Appropriately
```csharp
// ✓ CORRECT - Validate input
try
{
    var maze = new Maze(rows, columns);
    var cell = maze.GetCell(userRow, userColumn);
    maze.ConnectCells(row1, col1, row2, col2);
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Invalid argument: {ex.Message}");
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine($"Position out of bounds: {ex.Message}");
}
```

### 7. Understand Connected vs Adjacent
```csharp
// Adjacent cells = physically next to each other (may or may not be connected)
var adjacent = maze.GetAdjacentCells(5, 5); // All cells next to (5,5)

// Connected cells = have a path between them (subset of adjacent)
var connected = maze.GetNeighbors(5, 5); // Only cells connected to (5,5)

// connected.Count <= adjacent.Count (always true)
```

### 8. Connection Operations Are Atomic
```csharp
// Both cells must be modifiable for connection to work
maze.FreezeCell(5, 5);
bool result = maze.ConnectCells(5, 5, 5, 6);
// result = false, and cell (5,6) also won't have (5,5) as neighbor
// The operation is all-or-nothing
```

---

## Common Patterns

### Pattern 1: Maze Validation
```csharp
public bool ValidateMazeConnectivity(Maze maze)
{
    // Check if maze has at least one path
    var visited = new HashSet<(int, int)>();
    var queue = new Queue<(int, int)>();
    
    queue.Enqueue((1, 1));
    visited.Add((1, 1));
    
    while (queue.Count > 0)
    {
        var (row, col) = queue.Dequeue();
        var neighbors = maze.GetNeighbors(row, col);
        
        foreach (var neighbor in neighbors)
        {
            var pos = (neighbor.Row, neighbor.Column);
            if (!visited.Contains(pos))
            {
                visited.Add(pos);
                queue.Enqueue(pos);
            }
        }
    }
    
    return visited.Count == maze.Rows * maze.Columns;
}
```

### Pattern 2: Creating Rooms in Maze
```csharp
public void CreateRoom(Maze maze, int topRow, int leftCol, int height, int width)
{
    // Connect all cells within the room
    for (int row = topRow; row < topRow + height; row++)
    {
        for (int col = leftCol; col < leftCol + width; col++)
        {
            // Connect horizontally
            if (col + 1 < leftCol + width)
                maze.ConnectCells(row, col, row, col + 1);
            
            // Connect vertically
            if (row + 1 < topRow + height)
                maze.ConnectCells(row, col, row + 1, col);
        }
    }
    
    // Optionally freeze room boundaries
    for (int col = leftCol; col < leftCol + width; col++)
    {
        maze.FreezeCell(topRow, col); // Top wall
        maze.FreezeCell(topRow + height - 1, col); // Bottom wall
    }
}
```

---

## Version History

**Version 1.0** (November 26, 2025)
- Initial release
- Cell, MazeCell, and Maze classes
- Support for 1-based indexing
- Freeze functionality
- Adjacent-only connections

---

## License

This documentation is part of the Maze Generator project by AmanRathoreP.

---

*Last Updated: November 26, 2025*
