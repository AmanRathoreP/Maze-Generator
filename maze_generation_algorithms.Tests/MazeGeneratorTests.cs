using System;
using System.Collections.Generic;
using maze_generation_algorithms.Models;

namespace maze_generation_algorithms.Tests;

public class MazeGeneratorTests
{
    [Fact]
    public void Constructor_ValidInput_SetsProperties()
    {
        var startingCells = new List<Cell> { new(1, 1), new(2, 2) };
        var endingCells = new List<Cell> { new(3, 3) };

        var generator = new MazeGenerator(4, 5, startingCells, endingCells, seed: 42);

        Assert.Equal(4, generator.Rows);
        Assert.Equal(5, generator.Columns);
        Assert.Equal(42, generator.Seed);
        Assert.Equal(startingCells, generator.StartingCells);
        Assert.Equal(endingCells, generator.EndingCells);
        Assert.NotSame(startingCells, generator.StartingCells);
        Assert.NotSame(endingCells, generator.EndingCells);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_InvalidRows_ThrowsArgumentException(int rows)
    {
        var startingCells = new List<Cell> { new(1, 1) };
        var endingCells = new List<Cell> { new(2, 2) };

        Assert.Throws<ArgumentException>(() => new MazeGenerator(rows, 5, startingCells, endingCells, seed: 1));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public void Constructor_InvalidColumns_ThrowsArgumentException(int columns)
    {
        var startingCells = new List<Cell> { new(1, 1) };
        var endingCells = new List<Cell> { new(2, 2) };

        Assert.Throws<ArgumentException>(() => new MazeGenerator(4, columns, startingCells, endingCells, seed: 1));
    }

    [Fact]
    public void Constructor_NullStartingCells_ThrowsArgumentException()
    {
        var endingCells = new List<Cell> { new(2, 2) };

        Assert.Throws<ArgumentException>(() => new MazeGenerator(4, 5, null!, endingCells, seed: 1));
    }

    [Fact]
    public void Constructor_EmptyEndingCells_ThrowsArgumentException()
    {
        var startingCells = new List<Cell> { new(1, 1) };

        Assert.Throws<ArgumentException>(() => new MazeGenerator(4, 5, startingCells, new List<Cell>(), seed: 1));
    }

    [Fact]
    public void Constructor_CopiesCellLists()
    {
        var startingCells = new List<Cell> { new(1, 1) };
        var endingCells = new List<Cell> { new(3, 3) };

        var generator = new MazeGenerator(4, 5, startingCells, endingCells, seed: 99);

        startingCells.Add(new Cell(2, 2));
        endingCells.Add(new Cell(4, 4));

        Assert.Single(generator.StartingCells);
        Assert.Single(generator.EndingCells);
    }
}
