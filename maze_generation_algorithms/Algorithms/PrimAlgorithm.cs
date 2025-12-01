using System;
using System.Collections.Generic;
using System.Linq;
using maze_generation_algorithms.DataStructures;
using maze_generation_algorithms.Models;

namespace maze_generation_algorithms.Algorithms
{
    /// <summary>
    /// Randomized Prim's algorithm over non-frozen cells.
    /// Treats frozen regions as fixed subgraphs; builds a random spanning tree
    /// over all non-frozen cells, respecting 1-based indexing and adjacency only.
    /// </summary>
    public class PrimAlgorithm : IMazeAlgorithm
    {
        public Maze Generate(in Maze maze, in int seed)
        {
            var mazeCopy = maze.Clone();
            var random = new Random(seed);

            var allNonFrozen = GetAllNonFrozenCells(mazeCopy);
            if (allNonFrozen.Count == 0)
                return mazeCopy;

            var visited = new HashSet<(int row, int col)>();
            var frontier = new List<((int r1, int c1) from, (int r2, int c2) to)>();

            // We may have multiple disconnected non-frozen components (due to frozen areas),
            // so run Prim's separately for each component.
            foreach (var start in allNonFrozen)
            {
                if (visited.Contains(start))
                    continue;

                visited.Add(start);
                AddFrontierEdges(mazeCopy, start, visited, frontier);

                while (frontier.Count > 0)
                {
                    int idx = random.Next(frontier.Count);
                    var edge = frontier[idx];
                    frontier.RemoveAt(idx);

                    var to = edge.to;
                    if (visited.Contains(to))
                        continue;

                    mazeCopy.ConnectCells(edge.from.r1, edge.from.c1, to.r2, to.c2);
                    visited.Add(to);
                    AddFrontierEdges(mazeCopy, to, visited, frontier);
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

        private static void AddFrontierEdges(
            Maze maze,
            (int row, int col) cell,
            HashSet<(int row, int col)> visited,
            List<((int r1, int c1), (int r2, int c2))> frontier)
        {
            var adj = maze.GetAdjacentCells(cell.row, cell.col);
            foreach (MazeCell neighbor in adj)
            {
                var pos = (neighbor.Row, neighbor.Column);
                if (!visited.Contains(pos) && !maze.IsCellFrozen(pos.Row, pos.Column))
                {
                    frontier.Add((cell, pos));
                }
            }
        }
    }
}