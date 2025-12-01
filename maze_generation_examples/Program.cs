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

// Directory: bin/mazes under current working directory
string binDir = Path.Combine(Directory.GetCurrentDirectory(), "bin");
string mazesDir = Path.Combine(binDir, "mazes");
Directory.CreateDirectory(mazesDir);

// Common seed for reproducibility
const int seed = 54871;

// Loop over all algorithms in the enum
foreach (MazeAlgorithm algorithm in Enum.GetValues<MazeAlgorithm>())
{
    Console.WriteLine($"Generating maze with algorithm: {algorithm}");

    var generator = new MazeGenerator(algorithm, seed);

    // Use the same base maze as template; MazeGenerator/algorithms
    // internally clone it, so baseMaze is not mutated.
    Maze generatedMaze = generator.Generate(in maze);

    // File name: e.g., BinaryTree.png, Prim.png, RandomWithValidPath.png, etc.
    string fileName = $"{algorithm}.png";
    string filePath = Path.Combine(mazesDir, fileName);

    MazeVisualizer.SaveMaze(generatedMaze, filePath);

    Console.WriteLine($"Saved: {filePath}");
}

Console.WriteLine("Done. All maze visualizations saved in bin/mazes.");

