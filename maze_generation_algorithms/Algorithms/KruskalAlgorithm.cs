using System;
using System.Collections.Generic;
using maze_generation_algorithms.DataStructures;
using maze_generation_algorithms.Models;

namespace maze_generation_algorithms.Algorithms
{
    /// <summary>
    /// Randomized Kruskal's algorithm over non-frozen cells.
    /// Uses a disjoint-set (union–find) to build a random spanning tree.
    /// Existing connections in the template are respected by seeding the DSU
    /// accordingly. No new connections are added to frozen cells.
    /// </summary>
    public class KruskalAlgorithm : IMazeAlgorithm
    {
        public Maze Generate(in Maze maze, in int seed)
        {
            var mazeCopy = maze.Clone();
            var random = new Random(seed);

            int rows = mazeCopy.Rows;
            int cols = mazeCopy.Columns;
            int cellCount = rows * cols;

            var dsu = new DisjointSet(cellCount);

            // Seed DSU with any existing connections (including frozen regions).
            for (int r = 1; r <= rows; r++)
            {
                for (int c = 1; c <= cols; c++)
                {
                    var cell = mazeCopy.GetCell(r, c);
                    int id1 = ToIndex(r, c, cols);

                    foreach (MazeCell neighbor in cell.Neighbors)
                    {
                        int id2 = ToIndex(neighbor.Row, neighbor.Column, cols);
                        dsu.Union(id1, id2);
                    }
                }
            }

            // Build list of all candidate edges between adjacent non-frozen cells.
            var edges = new List<((int r1, int c1), (int r2, int c2))>();

            for (int r = 1; r <= rows; r++)
            {
                for (int c = 1; c <= cols; c++)
                {
                    if (mazeCopy.IsCellFrozen(r, c))
                        continue;

                    // Right neighbor
                    if (c < cols && !mazeCopy.IsCellFrozen(r, c + 1))
                    {
                        edges.Add(((r, c), (r, c + 1)));
                    }

                    // Top neighbor
                    if (r < rows && !mazeCopy.IsCellFrozen(r + 1, c))
                    {
                        edges.Add(((r, c), (r + 1, c)));
                    }
                }
            }

            // Shuffle edges
            Shuffle(edges, random);

            // Process edges
            foreach (var edge in edges)
            {
                int id1 = ToIndex(edge.Item1.r1, edge.Item1.c1, cols);
                int id2 = ToIndex(edge.Item2.r2, edge.Item2.c2, cols);

                if (dsu.Find(id1) == dsu.Find(id2))
                    continue;

                if (mazeCopy.ConnectCells(edge.Item1.r1, edge.Item1.c1, edge.Item2.r2, edge.Item2.c2))
                {
                    dsu.Union(id1, id2);
                }
            }

            return mazeCopy;
        }

        private static int ToIndex(int row, int col, int cols) =>
            (row - 1) * cols + (col - 1);

        private static void Shuffle<T>(IList<T> list, Random random)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        private sealed class DisjointSet
        {
            private readonly int[] _parent;
            private readonly int[] _rank;

            public DisjointSet(int size)
            {
                _parent = new int[size];
                _rank = new int[size];
                for (int i = 0; i < size; i++)
                {
                    _parent[i] = i;
                    _rank[i] = 0;
                }
            }

            public int Find(int x)
            {
                if (_parent[x] != x)
                    _parent[x] = Find(_parent[x]);
                return _parent[x];
            }

            public void Union(int x, int y)
            {
                int rx = Find(x);
                int ry = Find(y);
                if (rx == ry) return;

                if (_rank[rx] < _rank[ry])
                {
                    _parent[rx] = ry;
                }
                else if (_rank[rx] > _rank[ry])
                {
                    _parent[ry] = rx;
                }
                else
                {
                    _parent[ry] = rx;
                    _rank[rx]++;
                }
            }
        }
    }
}
