using System;
using maze_generation_algorithms.Algorithms;
using maze_generation_algorithms.DataStructures;

namespace maze_generation_algorithms;

public enum MazeAlgorithm
{
    BinaryTree,
    Prim,
    RandomWithValidPath,
    Kruskal,
    DepthFirstSearch,
    BfsTree,
    HuntAndKill,
    RecursiveDivision,
}

/// <summary>
/// Main class for generating mazes using different algorithms.
/// Respects frozen cells and their connections during generation.
/// </summary>
public class MazeGenerator
{
    private readonly IMazeAlgorithm _algorithm;
    private readonly int _seed;

    /// <summary>
    /// Initializes a new MazeGenerator with the specified algorithm and seed
    /// </summary>
    /// <param name="algorithm">The maze generation algorithm to use</param>
    /// <param name="seed">Random seed for reproducible generation</param>
    public MazeGenerator(MazeAlgorithm algorithm, int seed)
    {
        _algorithm = CreateAlgorithm(algorithm);
        _seed = seed;
    }

    /// <summary>
    /// Generates a maze using the configured algorithm.
    /// The original maze is not modified; a new maze is returned.
    /// Frozen cells and their connections are preserved in the copy.
    /// </summary>
    /// <param name="maze">The maze template. Original will not be modified.</param>
    /// <returns>A new generated maze with connections (original maze unchanged)</returns>
    public Maze Generate(in Maze maze)
    {
        if (maze == null)
            throw new ArgumentNullException(nameof(maze));

        return _algorithm.Generate(in maze, in _seed);
    }

    /// <summary>
    /// Creates an instance of the appropriate algorithm implementation
    /// </summary>
    private IMazeAlgorithm CreateAlgorithm(MazeAlgorithm algorithm)
    {
        return algorithm switch
        {
            MazeAlgorithm.RandomWithValidPath => new RandomWithValidPathAlgorithm(),
            MazeAlgorithm.BinaryTree => new BinaryTreeAlgorithm(),
            MazeAlgorithm.Prim => new PrimAlgorithm(),
            MazeAlgorithm.Kruskal => new KruskalAlgorithm(),
            MazeAlgorithm.DepthFirstSearch => new DepthFirstSearchAlgorithm(),
            MazeAlgorithm.BfsTree => new BfsTreeAlgorithm(),
            MazeAlgorithm.HuntAndKill => new HuntAndKillAlgorithm(),
            MazeAlgorithm.RecursiveDivision => new RecursiveDivisionAlgorithm(),
            _ => throw new ArgumentException($"Unknown algorithm: {algorithm}", nameof(algorithm)),
        };
    }
}
