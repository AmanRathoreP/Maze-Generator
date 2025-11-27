using System;
using System.IO;
using SkiaSharp;
using maze_generation_algorithms.DataStructures;

namespace maze_generation_examples
{
    /// <summary>
    /// Provides visualization capabilities for Maze structures using SkiaSharp
    /// </summary>
    public static class MazeVisualizer
    {
        /// <summary>
        /// Visualizes a maze as a grid and saves it as an image (PNG, JPG, or SVG)
        /// Format is determined by file extension
        /// Bottom-left cell is (1,1), coordinates increase upward and rightward
        /// </summary>
        /// <param name="maze">The maze to visualize</param>
        /// <param name="outputPath">Full path where the image should be saved (with .png, .jpg, or .svg extension)</param>
        /// <param name="cellSize">Size of each cell in pixels (default: 50)</param>
        public static void SaveMaze(Maze maze, string outputPath, int cellSize = 50)
        {
            if (maze == null)
            {
                throw new ArgumentNullException(nameof(maze));
            }

            if (string.IsNullOrWhiteSpace(outputPath))
            {
                throw new ArgumentException("Output path cannot be null or empty", nameof(outputPath));
            }

            // Ensure directory exists
            var directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Determine format from extension
            string extension = Path.GetExtension(outputPath).ToLowerInvariant();
            
            switch (extension)
            {
                case ".svg":
                    SaveAsSvg(maze, outputPath, cellSize);
                    break;
                case ".jpg":
                case ".jpeg":
                    SaveAsRaster(maze, outputPath, cellSize, SKEncodedImageFormat.Jpeg);
                    break;
                case ".png":
                default:
                    SaveAsRaster(maze, outputPath, cellSize, SKEncodedImageFormat.Png);
                    break;
            }
        }

        private static void SaveAsRaster(Maze maze, string outputPath, int cellSize, SKEncodedImageFormat format)
        {
            int padding = 20;
            int width = maze.Columns * cellSize + 2 * padding;
            int height = maze.Rows * cellSize + 2 * padding;

            var imageInfo = new SKImageInfo(width, height);
            using var surface = SKSurface.Create(imageInfo);
            var canvas = surface.Canvas;

            DrawMaze(canvas, maze, cellSize, padding);

            // Save to file
            using var image = surface.Snapshot();
            using var data = image.Encode(format, format == SKEncodedImageFormat.Jpeg ? 90 : 100);
            using var stream = File.OpenWrite(outputPath);
            data.SaveTo(stream);
        }

        private static void SaveAsSvg(Maze maze, string outputPath, int cellSize)
        {
            int padding = 20;
            int width = maze.Columns * cellSize + 2 * padding;
            int height = maze.Rows * cellSize + 2 * padding;

            using var stream = File.OpenWrite(outputPath);
            using var canvas = SKSvgCanvas.Create(new SKRect(0, 0, width, height), stream);

            DrawMaze(canvas, maze, cellSize, padding);
        }

        private static void DrawMaze(SKCanvas canvas, Maze maze, int cellSize, int padding)
        {
            // Clear background to white
            canvas.Clear(SKColors.White);

            // Define paints
            var frozenCellPaint = new SKPaint { Color = SKColors.Red, Style = SKPaintStyle.Fill };
            var normalCellPaint = new SKPaint { Color = SKColors.LightBlue, Style = SKPaintStyle.Fill };
            var borderPaint = new SKPaint { Color = SKColors.Black, Style = SKPaintStyle.Stroke, StrokeWidth = 1 };
            var frozenBorderPaint = new SKPaint { Color = SKColors.DarkRed, Style = SKPaintStyle.Stroke, StrokeWidth = 3 };
            var pathPaint = new SKPaint { Color = SKColors.Green, Style = SKPaintStyle.Stroke, StrokeWidth = 3 };
            var textPaint = new SKPaint { Color = SKColors.Black, IsAntialias = true };
            var textFont = new SKFont { Size = 12 };

            // Draw all cells (iterate through grid using 1-based indexing)
            for (int row = 1; row <= maze.Rows; row++)
            {
                for (int col = 1; col <= maze.Columns; col++)
                {
                    var cell = maze.GetCell(row, col);
                    
                    // Calculate position (bottom-left is (1,1))
                    // Convert to screen coordinates: Y increases downward in screen space
                    float x = padding + (col - 1) * cellSize;
                    float y = padding + (maze.Rows - row) * cellSize;

                    var rect = new SKRect(x, y, x + cellSize, y + cellSize);

                    // Fill cell
                    canvas.DrawRect(rect, maze.IsCellFrozen(row, col) ? frozenCellPaint : normalCellPaint);

                    // Draw border
                    canvas.DrawRect(rect, maze.IsCellFrozen(row, col) ? frozenBorderPaint : borderPaint);

                    // Draw label
                    string label = $"({row},{col})";
                    float textX = x + cellSize / 2;
                    float textY = y + cellSize / 2 + 4; // Adjust for vertical centering
                    canvas.DrawText(label, textX, textY, SKTextAlign.Center, textFont, textPaint);
                }
            }

            // Draw paths between connected cells
            for (int row = 1; row <= maze.Rows; row++)
            {
                for (int col = 1; col <= maze.Columns; col++)
                {
                    float x1 = padding + (col - 1) * cellSize + cellSize / 2;
                    float y1 = padding + (maze.Rows - row) * cellSize + cellSize / 2;

                    var neighbors = maze.GetNeighbors(row, col);
                    foreach (var neighbor in neighbors)
                    {
                        // Only draw each edge once (from lower to higher cell)
                        if (row < neighbor.Row || (row == neighbor.Row && col < neighbor.Column))
                        {
                            float x2 = padding + (neighbor.Column - 1) * cellSize + cellSize / 2;
                            float y2 = padding + (maze.Rows - neighbor.Row) * cellSize + cellSize / 2;

                            canvas.DrawLine(x1, y1, x2, y2, pathPaint);
                        }
                    }
                }
            }
        }
    }
}
