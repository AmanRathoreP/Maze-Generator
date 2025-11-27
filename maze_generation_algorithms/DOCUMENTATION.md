# Maze Generation Algorithms - Comprehensive Documentation

## Table of Contents
1. [Overview](#overview)
2. [Cell Class](#cell-class)
3. [MazeCell Class](#mazecell-class)
4. [Maze Class](#maze-class)
5. [Usage Examples](#usage-examples)
6. [Best Practices](#best-practices)

---

## Overview

This project provides a robust framework for generating and manipulating maze structures. It uses a tree-based data structure where cells can be connected to form paths, with support for freezing cells to prevent modifications.

### Key Concepts
- **1-based Indexing**: All row and column numbers start from 1 (no zeros or negatives)
- **Orthogonal Connections**: Cells can only connect to adjacent neighbors (up, down, left, right - no diagonals)
- **Bidirectional Edges**: Connections between cells are always bidirectional
- **Immutable Frozen State**: Once a cell is frozen, its connections cannot be modified

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

---

## Usage Examples

### Example 1: Creating a Simple Maze Path

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
