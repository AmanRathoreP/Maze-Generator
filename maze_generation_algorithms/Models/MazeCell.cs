using System.Collections.Generic;

namespace maze_generation_algorithms.Models
{
    /// <summary>
    /// Represents a cell in the maze with frozen state and neighbor connections
    /// </summary>
    public class MazeCell : Cell
    {
        /// <summary>
        /// Indicates whether this cell is frozen (cannot modify connections)
        /// </summary>
        public bool IsFrozen { get; private set; }

        /// <summary>
        /// Set of connected neighbor cells
        /// </summary>
        public HashSet<MazeCell> Neighbors { get; private set; }

        /// <summary>
        /// Snapshot of neighbors when cell was frozen
        /// </summary>
        private HashSet<MazeCell>? _frozenNeighbors;

        public MazeCell(int row, int column) : base(row, column)
        {
            IsFrozen = false;
            Neighbors = new HashSet<MazeCell>();
        }

        /// <summary>
        /// Marks this cell as frozen, preserving current neighbor connections
        /// </summary>
        public void Freeze()
        {
            if (!IsFrozen)
            {
                IsFrozen = true;
                _frozenNeighbors = new HashSet<MazeCell>(Neighbors);
            }
        }

        /// <summary>
        /// Adds a neighbor connection (only if not frozen)
        /// </summary>
        public bool AddNeighbor(MazeCell neighbor)
        {
            if (IsFrozen)
            {
                return false;
            }

            return Neighbors.Add(neighbor);
        }

        /// <summary>
        /// Removes a neighbor connection (only if not frozen)
        /// </summary>
        public bool RemoveNeighbor(MazeCell neighbor)
        {
            if (IsFrozen)
            {
                return false;
            }

            return Neighbors.Remove(neighbor);
        }

        /// <summary>
        /// Gets the neighbors that existed when the cell was frozen
        /// </summary>
        public IReadOnlyCollection<MazeCell> GetFrozenNeighbors()
        {
            return _frozenNeighbors ?? (IReadOnlyCollection<MazeCell>)Neighbors;
        }
    }
}
