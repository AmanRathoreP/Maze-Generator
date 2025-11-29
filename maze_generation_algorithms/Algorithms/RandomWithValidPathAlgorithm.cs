using System;
using System.Collections.Generic;
using System.Linq;
using maze_generation_algorithms.DataStructures;
using maze_generation_algorithms.Models;

namespace maze_generation_algorithms.Algorithms
{
    /// <summary>
    /// Generates a maze by randomly adding walls while ensuring at least one valid path exists
    /// between all reachable cells. Respects frozen cells and their connections.
    /// </summary>
    public class RandomWithValidPathAlgorithm : IMazeAlgorithm
    {
        public Maze Generate(in Maze maze, in int seed)
        {
            // Create a deep copy to avoid modifying the original maze
            var mazeCopy = maze.Clone();
            var random = new Random(seed);
            
            // Get all potential edges (connections between adjacent cells)
            var allEdges = GetAllPotentialEdges(mazeCopy);
            
            // Separate frozen edges from non-frozen edges
            var frozenEdges = new List<(MazeCell, MazeCell)>();
            var nonFrozenEdges = new List<(MazeCell, MazeCell)>();
            
            foreach (var edge in allEdges)
            {
                bool cell1Frozen = mazeCopy.IsCellFrozen(edge.Item1.Row, edge.Item1.Column);
                bool cell2Frozen = mazeCopy.IsCellFrozen(edge.Item2.Row, edge.Item2.Column);
                
                if (cell1Frozen || cell2Frozen)
                {
                    frozenEdges.Add(edge);
                }
                else
                {
                    nonFrozenEdges.Add(edge);
                }
            }
            
            // Shuffle non-frozen edges for random processing
            var shuffledEdges = nonFrozenEdges.OrderBy(_ => random.Next()).ToList();
            
            // Add connections one by one, only if they help maintain connectivity
            // or create the minimum spanning tree
            foreach (var edge in shuffledEdges)
            {
                var cell1 = edge.Item1;
                var cell2 = edge.Item2;
                
                // Check if cells are already connected through other paths
                if (!AreConnectedThroughPath(mazeCopy, cell1, cell2))
                {
                    // Add this connection as it's needed for connectivity
                    mazeCopy.ConnectCells(cell1.Row, cell1.Column, cell2.Row, cell2.Column);
                }
                else
                {
                    // Randomly decide to add this connection (creates loops/multiple paths)
                    // Lower probability means more sparse maze with fewer alternative paths
                    if (random.NextDouble() < 0.1) // 10% chance to add redundant connection
                    {
                        mazeCopy.ConnectCells(cell1.Row, cell1.Column, cell2.Row, cell2.Column);
                    }
                }
            }
            
            return mazeCopy;
        }
        
        /// <summary>
        /// Gets all potential edges (pairs of adjacent cells) in the maze
        /// </summary>
        private List<(MazeCell, MazeCell)> GetAllPotentialEdges(Maze maze)
        {
            var edges = new List<(MazeCell, MazeCell)>();
            
            for (int row = 1; row <= maze.Rows; row++)
            {
                for (int col = 1; col <= maze.Columns; col++)
                {
                    var currentCell = maze.GetCell(row, col);
                    
                    // Check right neighbor
                    if (col < maze.Columns)
                    {
                        var rightCell = maze.GetCell(row, col + 1);
                        edges.Add((currentCell, rightCell));
                    }
                    
                    // Check bottom neighbor
                    if (row > 1)
                    {
                        var bottomCell = maze.GetCell(row - 1, col);
                        edges.Add((currentCell, bottomCell));
                    }
                }
            }
            
            return edges;
        }
        
        /// <summary>
        /// Checks if all cells in the maze are connected (BFS traversal)
        /// </summary>
        private bool IsFullyConnected(Maze maze)
        {
            var visited = new HashSet<MazeCell>();
            var queue = new Queue<MazeCell>();
            
            // Start from first cell
            var startCell = maze.GetCell(1, 1);
            queue.Enqueue(startCell);
            visited.Add(startCell);
            
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                
                foreach (var neighbor in current.Neighbors)
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }
            
            // Check if all cells were visited
            int totalCells = maze.Rows * maze.Columns;
            return visited.Count == totalCells;
        }
        
        /// <summary>
        /// Checks if two cells are already connected through any path (BFS)
        /// </summary>
        private bool AreConnectedThroughPath(Maze maze, MazeCell start, MazeCell target)
        {
            if (start.Equals(target))
                return true;
                
            var visited = new HashSet<MazeCell>();
            var queue = new Queue<MazeCell>();
            
            queue.Enqueue(start);
            visited.Add(start);
            
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                
                if (current.Equals(target))
                    return true;
                
                foreach (var neighbor in current.Neighbors)
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }
            
            return false;
        }
    }
}
