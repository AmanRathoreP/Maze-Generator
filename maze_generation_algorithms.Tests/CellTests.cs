using System.Collections.Generic;
using maze_generation_algorithms.Utils;

namespace maze_generation_algorithms.Tests;

public class CellTests
{
    [Fact]
    public void Constructor_SetsRowAndColumn()
    {
        var cell = new Cell(2, 3);

        Assert.Equal(2, cell.Row);
        Assert.Equal(3, cell.Column);
    }

    [Fact]
    public void TopCell_IncrementsRow()
    {
        var cell = new Cell(2, 3);

        var topCell = cell.TopCell();

        Assert.Equal(3, topCell.Row);
        Assert.Equal(3, topCell.Column);
    }

    [Fact]
    public void BottomCell_DecrementsRow()
    {
        var cell = new Cell(2, 3);

        var bottomCell = cell.BottomCell();

        Assert.Equal(1, bottomCell.Row);
        Assert.Equal(3, bottomCell.Column);
    }

    [Fact]
    public void LeftCell_DecrementsColumn()
    {
        var cell = new Cell(2, 3);

        var leftCell = cell.LeftCell();

        Assert.Equal(2, leftCell.Row);
        Assert.Equal(2, leftCell.Column);
    }

    [Fact]
    public void RightCell_IncrementsColumn()
    {
        var cell = new Cell(2, 3);

        var rightCell = cell.RightCell();

        Assert.Equal(2, rightCell.Row);
        Assert.Equal(4, rightCell.Column);
    }

    [Fact]
    public void Cells_WithSameCoordinates_AreEqual()
    {
        var first = new Cell(2, 3);
        var second = new Cell(2, 3);

        Assert.Equal(first, second);
        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }

    [Fact]
    public void Cells_WithDifferentCoordinates_AreNotEqual()
    {
        var first = new Cell(2, 3);
        var second = new Cell(3, 3);

        Assert.NotEqual(first, second);
    }

    [Fact]
    public void Cell_ToString_ReturnsReadableFormat()
    {
        var cell = new Cell(2, 3);

        Assert.Equal("Cell(2, 3)", cell.ToString());
    }

    [Fact]
    public void Cell_CanBeUsedAsDictionaryKey()
    {
        var cell = new Cell(2, 3);
        var dictionary = new Dictionary<Cell, string>
        {
            [cell] = "occupied"
        };

        Assert.True(dictionary.ContainsKey(new Cell(2, 3)));
        Assert.Equal("occupied", dictionary[new Cell(2, 3)]);
    }
}
