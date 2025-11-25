using maze_generation_algorithms.Utils;

namespace maze_generation_algorithms;

public enum MazeAlgorithm
{
    BinaryTree,
    Prim,
}

public class MazeGenerator
{
    public int Rows { get; }
    public int Columns { get; }
    public List<Cell> StartingCells { get; }
    public List<Cell> EndingCells { get; }
    public int Seed { get; }

    public MazeGenerator(int rows, int columns, List<Cell> startingCells, List<Cell> endingCells, int seed)
    {
        if (rows <= 0)
            throw new ArgumentException("Rows must be greater than 0", nameof(rows));
        
        if (columns <= 0)
            throw new ArgumentException("Columns must be greater than 0", nameof(columns));
        
        if (startingCells == null || startingCells.Count == 0)
            throw new ArgumentException("Starting cells cannot be null or empty", nameof(startingCells));
        
        if (endingCells == null || endingCells.Count == 0)
            throw new ArgumentException("Ending cells cannot be null or empty", nameof(endingCells));

        Rows = rows;
        Columns = columns;
        StartingCells = [.. startingCells];
        EndingCells = [.. endingCells];
        Seed = seed;
    }
}
