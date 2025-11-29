using System;
using System.Collections.Generic;
using System.Linq;
using maze_generation_algorithms.Models;

namespace maze_generation_algorithms.DataStructures
{
    /// <summary>
    /// Represents a rectangular maze as a tree data structure with MazeCell nodes
    /// Uses 1-based indexing (rows and columns start at 1)
    /// </summary>
    public class Maze
    {
        private readonly int _rows;
        private readonly int _columns;
        private readonly MazeCell[,] _cells;

        /// <summary>
        /// Gets the number of rows in the maze
        /// </summary>
        public int Rows => _rows;

        /// <summary>
        /// Gets the number of columns in the maze
        /// </summary>
        public int Columns => _columns;

        /// <summary>
        /// Initializes a new maze with the specified dimensions
        /// </summary>
        /// <param name="rows">Number of rows (must be positive)</param>
        /// <param name="columns">Number of columns (must be positive)</param>
        public Maze(int rows, int columns)
        {
            if (rows <= 0)
                throw new ArgumentException("Rows must be positive", nameof(rows));
            if (columns <= 0)
                throw new ArgumentException("Columns must be positive", nameof(columns));

            _rows = rows;
            _columns = columns;
            _cells = new MazeCell[rows, columns];

            // Initialize all cells with 1-based indexing
            for (int row = 1; row <= rows; row++)
            {
                for (int col = 1; col <= columns; col++)
                {
                    _cells[row - 1, col - 1] = new MazeCell(row, col);
                }
            }
        }

        /// <summary>
        /// Gets the cell at the specified position
        /// </summary>
        /// <param name="row">Row number (1-based)</param>
        /// <param name="column">Column number (1-based)</param>
        /// <returns>The MazeCell at the specified position</returns>
        public MazeCell GetCell(int row, int column)
        {
            ValidatePosition(row, column);
            return _cells[row - 1, column - 1];
        }

        /// <summary>
        /// Connects two adjacent cells by creating an edge between them
        /// </summary>
        /// <param name="row1">Row of first cell (1-based)</param>
        /// <param name="column1">Column of first cell (1-based)</param>
        /// <param name="row2">Row of second cell (1-based)</param>
        /// <param name="column2">Column of second cell (1-based)</param>
        /// <returns>True if connection was successful, false if cells are frozen or not adjacent</returns>
        public bool ConnectCells(int row1, int column1, int row2, int column2)
        {
            ValidatePosition(row1, column1);
            ValidatePosition(row2, column2);

            if (!AreAdjacent(row1, column1, row2, column2))
            {
                throw new ArgumentException("Cells must be adjacent (not diagonal)");
            }

            var cell1 = _cells[row1 - 1, column1 - 1];
            var cell2 = _cells[row2 - 1, column2 - 1];

            // Both cells must not be frozen for connection to succeed
            bool success1 = cell1.AddNeighbor(cell2);
            bool success2 = cell2.AddNeighbor(cell1);

            // If only one succeeded, rollback
            if (success1 != success2)
            {
                if (success1) cell1.RemoveNeighbor(cell2);
                if (success2) cell2.RemoveNeighbor(cell1);
                return false;
            }

            return success1 && success2;
        }

        /// <summary>
        /// Removes the connection between two adjacent cells
        /// </summary>
        /// <param name="row1">Row of first cell (1-based)</param>
        /// <param name="column1">Column of first cell (1-based)</param>
        /// <param name="row2">Row of second cell (1-based)</param>
        /// <param name="column2">Column of second cell (1-based)</param>
        /// <returns>True if disconnection was successful, false if cells are frozen</returns>
        public bool DisconnectCells(int row1, int column1, int row2, int column2)
        {
            ValidatePosition(row1, column1);
            ValidatePosition(row2, column2);

            if (!AreAdjacent(row1, column1, row2, column2))
            {
                throw new ArgumentException("Cells must be adjacent (not diagonal)");
            }

            var cell1 = _cells[row1 - 1, column1 - 1];
            var cell2 = _cells[row2 - 1, column2 - 1];

            // Both cells must not be frozen for disconnection to succeed
            bool success1 = cell1.RemoveNeighbor(cell2);
            bool success2 = cell2.RemoveNeighbor(cell1);

            return success1 && success2;
        }

        /// <summary>
        /// Freezes a cell, preventing any further modifications to its connections
        /// </summary>
        /// <param name="row">Row number (1-based)</param>
        /// <param name="column">Column number (1-based)</param>
        public void FreezeCell(int row, int column)
        {
            ValidatePosition(row, column);
            _cells[row - 1, column - 1].Freeze();
        }

        /// <summary>
        /// Checks if a cell is frozen
        /// </summary>
        /// <param name="row">Row number (1-based)</param>
        /// <param name="column">Column number (1-based)</param>
        /// <returns>True if the cell is frozen</returns>
        public bool IsCellFrozen(int row, int column)
        {
            ValidatePosition(row, column);
            return _cells[row - 1, column - 1].IsFrozen;
        }

        /// <summary>
        /// Checks if two cells are connected
        /// </summary>
        /// <param name="row1">Row of first cell (1-based)</param>
        /// <param name="column1">Column of first cell (1-based)</param>
        /// <param name="row2">Row of second cell (1-based)</param>
        /// <param name="column2">Column of second cell (1-based)</param>
        /// <returns>True if the cells are connected</returns>
        public bool AreConnected(int row1, int column1, int row2, int column2)
        {
            ValidatePosition(row1, column1);
            ValidatePosition(row2, column2);

            var cell1 = _cells[row1 - 1, column1 - 1];
            var cell2 = _cells[row2 - 1, column2 - 1];

            return cell1.Neighbors.Contains(cell2);
        }

        /// <summary>
        /// Gets all neighbors of a cell
        /// </summary>
        /// <param name="row">Row number (1-based)</param>
        /// <param name="column">Column number (1-based)</param>
        /// <returns>Collection of connected neighbor cells</returns>
        public IReadOnlyCollection<MazeCell> GetNeighbors(int row, int column)
        {
            ValidatePosition(row, column);
            return _cells[row - 1, column - 1].Neighbors;
        }

        /// <summary>
        /// Gets all potential adjacent cells (up, down, left, right) that exist in the maze
        /// </summary>
        /// <param name="row">Row number (1-based)</param>
        /// <param name="column">Column number (1-based)</param>
        /// <returns>List of adjacent cells within maze bounds</returns>
        public List<MazeCell> GetAdjacentCells(int row, int column)
        {
            ValidatePosition(row, column);

            var adjacent = new List<MazeCell>();

            // Top
            if (row + 1 <= _rows)
                adjacent.Add(_cells[row, column - 1]);

            // Bottom
            if (row - 1 >= 1)
                adjacent.Add(_cells[row - 2, column - 1]);

            // Right
            if (column + 1 <= _columns)
                adjacent.Add(_cells[row - 1, column]);

            // Left
            if (column - 1 >= 1)
                adjacent.Add(_cells[row - 1, column - 2]);

            return adjacent;
        }

        /// <summary>
        /// Validates that a position is within maze bounds
        /// </summary>
        private void ValidatePosition(int row, int column)
        {
            if (row < 1 || row > _rows)
                throw new ArgumentOutOfRangeException(nameof(row), $"Row must be between 1 and {_rows}");
            if (column < 1 || column > _columns)
                throw new ArgumentOutOfRangeException(nameof(column), $"Column must be between 1 and {_columns}");
        }

        /// <summary>
        /// Checks if two cells are adjacent (horizontally or vertically, not diagonally)
        /// </summary>
        private bool AreAdjacent(int row1, int column1, int row2, int column2)
        {
            int rowDiff = Math.Abs(row1 - row2);
            int colDiff = Math.Abs(column1 - column2);

            // Adjacent means one unit apart in either row or column, but not both
            return (rowDiff == 1 && colDiff == 0) || (rowDiff == 0 && colDiff == 1);
        }

        /// <summary>
        /// Creates a deep copy of the maze with all cells and their connections
        /// </summary>
        /// <returns>A new Maze instance with identical structure</returns>
        public Maze Clone()
        {
            var clonedMaze = new Maze(_rows, _columns);
            
            // Dictionary to map original cells to cloned cells
            var cellMapping = new Dictionary<MazeCell, MazeCell>();
            
            // First pass: Build mapping (don't freeze yet)
            for (int row = 1; row <= _rows; row++)
            {
                for (int col = 1; col <= _columns; col++)
                {
                    var originalCell = _cells[row - 1, col - 1];
                    var clonedCell = clonedMaze._cells[row - 1, col - 1];
                    cellMapping[originalCell] = clonedCell;
                }
            }
            
            // Second pass: Copy connections (before freezing)
            for (int row = 1; row <= _rows; row++)
            {
                for (int col = 1; col <= _columns; col++)
                {
                    var originalCell = _cells[row - 1, col - 1];
                    var clonedCell = clonedMaze._cells[row - 1, col - 1];
                    
                    foreach (var originalNeighbor in originalCell.Neighbors)
                    {
                        var clonedNeighbor = cellMapping[originalNeighbor];
                        // Only add if not already added (to avoid duplicates from bidirectional connections)
                        if (!clonedCell.Neighbors.Contains(clonedNeighbor))
                        {
                            clonedCell.AddNeighbor(clonedNeighbor);
                        }
                    }
                }
            }
            
            // Third pass: Freeze cells after all connections are established
            for (int row = 1; row <= _rows; row++)
            {
                for (int col = 1; col <= _columns; col++)
                {
                    var originalCell = _cells[row - 1, col - 1];
                    if (originalCell.IsFrozen)
                    {
                        clonedMaze.FreezeCell(row, col);
                    }
                }
            }
            
            return clonedMaze;
        }

        /// <summary>
        /// Freezes a list of cells and optionally connects them in sequence
        /// </summary>
        /// <param name="cellPositions">List of (row, column) tuples representing cell positions</param>
        /// <param name="connectPath">If true, connects cells in the order they appear in the list</param>
        /// <returns>Number of cells successfully frozen</returns>
        /// <exception cref="ArgumentNullException">If cellPositions is null</exception>
        /// <exception cref="ArgumentException">If cellPositions is empty or contains invalid positions</exception>
        public int FreezeAndConnectCells(List<(int row, int column)> cellPositions, bool connectPath = true)
        {
            if (cellPositions == null)
                throw new ArgumentNullException(nameof(cellPositions));

            if (cellPositions.Count == 0)
                throw new ArgumentException("Cell positions list cannot be empty", nameof(cellPositions));

            // Validate all positions first
            foreach (var (row, column) in cellPositions)
            {
                ValidatePosition(row, column);
            }

            // Connect cells in sequence if requested
            if (connectPath && cellPositions.Count > 1)
            {
                // Create HashSet for O(1) lookup instead of O(n) linear search
                var positionSet = new HashSet<(int, int)>(cellPositions);
                
                foreach (var (r, c) in cellPositions)
                {
                    var adj_cells = GetAdjacentCells(r, c);
                    foreach (var adj in adj_cells)
                    {
                        var adjPos = (adj.Row, adj.Column);
                        // O(1) lookup instead of O(n) loop
                        if (positionSet.Contains(adjPos))
                        {
                            // Avoid duplicate connections
                            if (!AreConnected(r, c, adj.Row, adj.Column))
                            {
                                ConnectCells(r, c, adj.Row, adj.Column);
                            }
                        }
                    }
                }
            }

            int frozenCount = 0;
            // Freeze all cells
            foreach (var (row, column) in cellPositions)
            {
                if (!IsCellFrozen(row, column))
                {
                    FreezeCell(row, column);
                    frozenCount++;
                }
            }
            return frozenCount;
        }
    }
}
