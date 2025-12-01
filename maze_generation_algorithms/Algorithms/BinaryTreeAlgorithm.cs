using System;
using maze_generation_algorithms.DataStructures;

namespace maze_generation_algorithms.Algorithms
{
    /// <summary>
    /// Binary-tree style maze generation.
    /// For each non-frozen cell, randomly carves a passage either "up" or "right"
    /// (when in bounds and non-frozen), producing a fast, simple perfect maze
    /// over the unfrozen region.
    /// </summary>
    public class BinaryTreeAlgorithm : IMazeAlgorithm
    {
        public Maze Generate(in Maze maze, in int seed)
        {
            var mazeCopy = maze.Clone();
            var random = new Random(seed);

            for (int row = 1; row <= mazeCopy.Rows; row++)
            {
                for (int col = 1; col <= mazeCopy.Columns; col++)
                {
                    if (mazeCopy.IsCellFrozen(row, col))
                        continue;

                    // Candidate neighbors: up (row+1) and right (col+1)
                    // (You can flip to bottom/left if you prefer different bias.)
                    var candidates = new System.Collections.Generic.List<(int r, int c)>();

                    if (row < mazeCopy.Rows && !mazeCopy.IsCellFrozen(row + 1, col))
                    {
                        candidates.Add((row + 1, col));
                    }

                    if (col < mazeCopy.Columns && !mazeCopy.IsCellFrozen(row, col + 1))
                    {
                        candidates.Add((row, col + 1));
                    }

                    if (candidates.Count == 0)
                        continue;

                    var choice = candidates[random.Next(candidates.Count)];
                    mazeCopy.ConnectCells(row, col, choice.r, choice.c);
                }
            }

            return mazeCopy;
        }
    }
}
