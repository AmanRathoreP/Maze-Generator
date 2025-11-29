using maze_generation_algorithms.DataStructures;

namespace maze_generation_algorithms.Algorithms
{
    /// <summary>
    /// Interface for maze generation algorithms
    /// </summary>
    public interface IMazeAlgorithm
    {
        /// <summary>
        /// Generates a maze using the specific algorithm
        /// </summary>
        /// <param name="maze">The maze to use as a template. Original will not be modified.</param>
        /// <param name="seed">Random seed for reproducible generation</param>
        /// <returns>A new maze with generated paths (original maze is not modified)</returns>
        Maze Generate(in Maze maze, in int seed);
    }
}
