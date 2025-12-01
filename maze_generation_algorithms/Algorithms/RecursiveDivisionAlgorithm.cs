
using System;
using maze_generation_algorithms.DataStructures;

namespace maze_generation_algorithms.Algorithms
{
    /// <summary>
    /// Recursive-division style maze generation.
    /// 1. Starts from a fully connected grid over non-frozen cells
    /// 2. Recursively "draws walls" by disconnecting edges along horizontal or
    ///    vertical lines, leaving a single gap in each wall.
    ///
    /// Frozen cells are never disconnected or connected; they act as solid blocks.
    /// </summary>
    public class RecursiveDivisionAlgorithm : IMazeAlgorithm
    {
        public Maze Generate(in Maze maze, in int seed)
        {
            var mazeCopy = maze.Clone();
            var random = new Random(seed);

            int rows = mazeCopy.Rows;
            int cols = mazeCopy.Columns;

            // Step 1: fully connect all non-frozen cells in a grid.
            for (int r = 1; r <= rows; r++)
            {
                for (int c = 1; c <= cols; c++)
                {
                    if (mazeCopy.IsCellFrozen(r, c))
                        continue;

                    if (c < cols && !mazeCopy.IsCellFrozen(r, c + 1))
                    {
                        mazeCopy.ConnectCells(r, c, r, c + 1);
                    }

                    if (r < rows && !mazeCopy.IsCellFrozen(r + 1, c))
                    {
                        mazeCopy.ConnectCells(r, c, r + 1, c);
                    }
                }
            }

            // Step 2: recursively carve "walls" by disconnecting edges.
            Divide(mazeCopy, 1, rows, 1, cols, random);

            return mazeCopy;
        }

        private static void Divide(Maze maze, int rowStart, int rowEnd, int colStart, int colEnd, Random random)
        {
            int height = rowEnd - rowStart + 1;
            int width = colEnd - colStart + 1;

            if (height < 2 || width < 2)
                return;

            bool horizontal = height >= width;

            if (horizontal)
            {
                // Choose horizontal wall between rowStart and rowEnd-1
                int wallRow = random.Next(rowStart, rowEnd); // inclusive rowStart, exclusive rowEnd+1

                // Choose a passage column
                int passageCol = random.Next(colStart, colEnd + 1);

                for (int c = colStart; c <= colEnd; c++)
                {
                    if (c == passageCol)
                        continue;

                    // Disconnect (wallRow, c) and (wallRow+1, c) if both not frozen
                    if (!maze.IsCellFrozen(wallRow, c) && !maze.IsCellFrozen(wallRow + 1, c))
                    {
                        maze.DisconnectCells(wallRow, c, wallRow + 1, c);
                    }
                }

                // Recurse on top and bottom sections
                Divide(maze, rowStart, wallRow, colStart, colEnd, random);
                Divide(maze, wallRow + 1, rowEnd, colStart, colEnd, random);
            }
            else
            {
                // Vertical division
                int wallCol = random.Next(colStart, colEnd); // between colStart and colEnd-1
                int passageRow = random.Next(rowStart, rowEnd + 1);

                for (int r = rowStart; r <= rowEnd; r++)
                {
                    if (r == passageRow)
                        continue;

                    if (!maze.IsCellFrozen(r, wallCol) && !maze.IsCellFrozen(r, wallCol + 1))
                    {
                        maze.DisconnectCells(r, wallCol, r, wallCol + 1);
                    }
                }

                // Recurse on left and right sections
                Divide(maze, rowStart, rowEnd, colStart, wallCol, random);
                Divide(maze, rowStart, rowEnd, wallCol + 1, colEnd, random);
            }
        }
    }
}