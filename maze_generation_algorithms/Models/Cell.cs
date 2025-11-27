namespace maze_generation_algorithms.Models;

public class Cell
{
    public int Row { get; }
    public int Column { get; }

    public Cell(int row, int column)
    {
        Row = row;
        Column = column;
    }

    /// <summary>
    /// Returns the cell above this one (row + 1)
    /// </summary>
    public Cell TopCell()
    {
        return new Cell(Row + 1, Column);
    }

    /// <summary>
    /// Returns the cell below this one (row - 1)
    /// </summary>
    public Cell BottomCell()
    {
        return new Cell(Row - 1, Column);
    }

    /// <summary>
    /// Returns the cell to the left (column - 1)
    /// </summary>
    public Cell LeftCell()
    {
        return new Cell(Row, Column - 1);
    }

    /// <summary>
    /// Returns the cell to the right (column + 1)
    /// </summary>
    public Cell RightCell()
    {
        return new Cell(Row, Column + 1);
    }

    public override bool Equals(object? obj)
    {
        if (obj is Cell other)
        {
            return Row == other.Row && Column == other.Column;
        }
        return false;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Row, Column);
    }
    public override string ToString()
    {
        return $"Cell({Row}, {Column})";
    }
}
