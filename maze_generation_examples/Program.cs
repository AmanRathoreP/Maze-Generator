using maze_generation_algorithms.DataStructures;
using maze_generation_algorithms.Models;
using maze_generation_algorithms;
using maze_generation_examples;

// Create a 16x16 maze (all cells are created but initially have no connections)
Maze maze = new Maze(16, 16);

// Example 3: Create another sample path
maze.ConnectCells(11, 11, 10,11);
maze.ConnectCells(3, 1, 4,1);
maze.ConnectCells(9, 9, 9,10);


maze.FreezeAndConnectCells(new List<(int, int)>
    {
        (8, 8),
        (8, 9),
        (9, 8),
        (9, 9),
    });

maze.FreezeAndConnectCells(new List<(int, int)>
    {
        (1, 1),
        (3, 1),
        (2, 1),
        (2, 2),
        (2, 3),
        (1, 2),
    });


var generator = new MazeGenerator(MazeAlgorithm.RandomWithValidPath, 54871);
var generatedMaze = generator.Generate(in maze);

// Save visualization
MazeVisualizer.SaveMaze(generatedMaze, Path.Combine(Directory.GetCurrentDirectory(), "bin/maze_visualization.png"));

Console.WriteLine("Done. Maze visualization saved.");