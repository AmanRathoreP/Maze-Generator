using System;
using System.Collections.Generic;
using System.Linq;
using maze_generation_algorithms.DataStructures;
using maze_generation_algorithms.Models;

namespace maze_generation_algorithms.Algorithms
{
    /// <summary>
    /// Iterative depth-first search (recursive-backtracking) maze generation
    /// over all non-frozen cells. Builds a perfect maze in each non-frozen
    /// connected region of the template (frozen areas preserved).
    /// </summary>
    public class DepthFirstSearchAlgorithm : IMazeAlgorithm
    {
        public Maze Generate(in Maze maze, in int seed)
        {
            var mazeCopy = maze.Clone();
            var random = new Random(seed);

            var allNonFrozen = GetAllNonFrozenCells(mazeCopy);
            if (allNonFrozen.Count == 0)
                return mazeCopy;

            var visited = new HashSet<(int row, int col)>();

            foreach (var start in allNonFrozen)
            {
                if (visited.Contains(start))
                    continue;

                var stack = new Stack<(int row, int col)>();
                visited.Add(start);
                stack.Push(start);

                while (stack.Count > 0)
                {
                    var current = stack.Peek();
                    var neighbors = GetNonFrozenNeighbors(mazeCopy, current.row, current.col)
                        .Where(n => !visited.Contains(n))
                        .ToList();

                    if (neighbors.Count == 0)
                    {
                        stack.Pop();
                        continue;
                    }

                    var next = neighbors[random.Next(neighbors.Count)];
                    mazeCopy.ConnectCells(current.row, current.col, next.row, next.col);
                    visited.Add(next);
                    stack.Push(next);
                }
            }

            return mazeCopy;
        }

        private static List<(int row, int col)> GetAllNonFrozenCells(Maze maze)
        {
            var cells = new List<(int row, int col)>();
            for (int r = 1; r <= maze.Rows; r++)
            {
                for (int c = 1; c <= maze.Columns; c++)
                {
                    if (!maze.IsCellFrozen(r, c))
                        cells.Add((r, c));
                }
            }
            return cells;
        }

        private static List<(int row, int col)> GetNonFrozenNeighbors(Maze maze, int row, int col)
        {
            var result = new List<(int row, int col)>();
            var adj = maze.GetAdjacentCells(row, col);
            foreach (MazeCell cell in adj)
            {
                if (!maze.IsCellFrozen(cell.Row, cell.Column))
                    result.Add((cell.Row, cell.Column));
            }
            return result;
        }
    }
}