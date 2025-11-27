using maze_generation_algorithms.DataStructures;
using maze_generation_algorithms.Models;
using maze_generation_examples;

// Create a 16x16 maze (all cells are created but initially have no connections)
Maze maze = new Maze(16, 16);

// Example 3: Create another sample path
maze.ConnectCells(11, 11, 10,11);

// Example 7: Freeze important cells to protect them from modifications
maze.FreezeCell(8, 8);
maze.FreezeCell(8, 9);
maze.FreezeCell(9, 8);
maze.FreezeCell(9, 9);
maze.FreezeCell(1, 1);
maze.FreezeCell(3, 1);
maze.FreezeCell(2, 1);
maze.FreezeCell(2, 2);
maze.FreezeCell(1, 2);

// Save visualization
MazeVisualizer.SaveMaze(maze, Path.Combine(Directory.GetCurrentDirectory(), "bin/maze_visualization.png"));

Console.WriteLine("Done. Maze visualization saved.");