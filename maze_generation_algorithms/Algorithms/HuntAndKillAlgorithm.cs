using System;
using System.Collections.Generic;
using System.Linq;
using maze_generation_algorithms.DataStructures;
using maze_generation_algorithms.Models;

namespace maze_generation_algorithms.Algorithms
{
    /// <summary>
    /// Hunt-and-kill algorithm over non-frozen cells.
    /// Walks randomly until stuck, then "hunts" for an unvisited cell that has
    /// a visited neighbor, connects to it, and continues. Produces a perfect maze
    /// in each non-frozen region.
    /// </summary>
    public class HuntAndKillAlgorithm : IMazeAlgorithm
    {
        public Maze Generate(in Maze maze, in int seed)
        {
            var mazeCopy = maze.Clone();
            var random = new Random(seed);

            var allNonFrozen = GetAllNonFrozenCells(mazeCopy);
            if (allNonFrozen.Count == 0)
                return mazeCopy;

            var visited = new HashSet<(int row, int col)>();

            // Pick a random starting cell among non-frozen cells.
            var current = allNonFrozen[random.Next(allNonFrozen.Count)];
            visited.Add(current);

            while (true)
            {
                // Kill phase: random walk from current until no unvisited neighbor.
                while (true)
                {
                    var neighbors = GetNonFrozenNeighbors(mazeCopy, current.row, current.col)
                        .Where(n => !visited.Contains(n))
                        .ToList();

                    if (neighbors.Count == 0)
                        break;

                    var next = neighbors[random.Next(neighbors.Count)];
                    mazeCopy.ConnectCells(current.row, current.col, next.row, next.col);
                    visited.Add(next);
                    current = next;
                }

                // Hunt phase: search for unvisited cell with any visited neighbor.
                bool found = false;
                for (int r = 1; r <= mazeCopy.Rows && !found; r++)
                {
                    for (int c = 1; c <= mazeCopy.Columns && !found; c++)
                    {
                        if (mazeCopy.IsCellFrozen(r, c))
                            continue;

                        var pos = (r, c);
                        if (visited.Contains(pos))
                            continue;

                        var neighbors = GetNonFrozenNeighbors(mazeCopy, r, c);
                        var visitedNeighbors = neighbors.Where(n => visited.Contains(n)).ToList();

                        if (visitedNeighbors.Count > 0)
                        {
                            found = true;
                            var attachTo = visitedNeighbors[random.Next(visitedNeighbors.Count)];
                            mazeCopy.ConnectCells(r, c, attachTo.row, attachTo.col);
                            visited.Add(pos);
                            current = pos;
                        }
                    }
                }

                if (!found)
                    break; // Done
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