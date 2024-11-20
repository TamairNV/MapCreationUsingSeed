namespace MapCreationUsingSeed;
using System.Numerics;
using Raylib_cs;
public class QuickDisplay
{
    public static List<QuickDisplay> QuickDisplays = new List<QuickDisplay>();
    public Rectangle display;
    private Color colour;
    
    public Vector2 gridSize;
    public int[,] grid;
    private List<Rectangle> lines = new List<Rectangle>();
    private List<(int, Rectangle)> pixels = new List<(int, Rectangle)>();
    public int cellSize;
    public bool displayOn = true;
    public QuickDisplay(int X, int Y, int cellSize, int[,] grid,Color colour)
    {
        QuickDisplays.Add(this);
        int width = cellSize * grid.GetLength(0);
        int height = cellSize * grid.GetLength(1);
        display = new Rectangle(X, Y, width, height);
        this.colour = colour;
        gridSize = new Vector2(width, height);
        this.grid = grid;
        this.cellSize = cellSize;

        createGridLines();
        findPixels();

    }

    public void DrawDisplay()
    {
        if (displayOn)
        {
            Raylib.DrawRectangleRec(display, colour);
            drawGridLines(Color.Black);
            drawPixels();
        }
    }
    private void createGridLines(int lineThickness = 2)
    {
        for (int x = 0; x <= gridSize.X/cellSize; x++)
        {
            Rectangle rect = new Rectangle(x * cellSize + display.X, + display.Y, lineThickness, gridSize.Y+lineThickness);
            lines.Add(rect);
        }
        for (int x = 0; x <= gridSize.Y/cellSize; x++)
        {
            Rectangle rect = new Rectangle(+ display.X,x*cellSize+ display.Y,gridSize.X,lineThickness);
            lines.Add(rect);
        }
        
    }

    private void drawPixels()
    {
        foreach (var pixel in pixels)
        {
            Raylib.DrawRectangleRec(pixel.Item2,PlaneObject.GenerateColor(pixel.Item1));
        }
    }

    private void findPixels()
    {
        for (int x = 0; x < gridSize.X/cellSize; x++)
        {
            for (int y = 0; y < gridSize.Y/cellSize; y++)
            {
                if (grid[y,x] != 0)
                {
                    Rectangle rect = new Rectangle(x * cellSize + display.X, y * cellSize + display.Y, cellSize,
                        cellSize);
                    
                    pixels.Add((grid[y,x],rect));
                }
            }
        }
    }
    
    

    public void drawGridLines(Color colour)
    {
        foreach (var line in lines)
        {
            Raylib.DrawRectangleRec(line,colour);
        }
        
    }

    public static void DrawDisplays()
    {
        foreach (var display in QuickDisplays)
        {
            display.DrawDisplay();
        }
    }
}