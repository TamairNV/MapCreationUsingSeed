using System.Numerics;

namespace MapCreationUsingSeed;
using Raylib_cs;
public class PlaneObject
{
    public int[,] ObjectGrid;
    public Vector2 Position;
    public MovablePlane Plane;
    private int cellSize;
    private Vector2 gridSize;

    public PlaneObject(int[,] grid, Vector2 position,MovablePlane plane)
    {
        ObjectGrid = grid;
        Position = position;
        Plane = plane;
        cellSize = plane.cellSize;
        gridSize = new Vector2(grid.GetLength(0), grid.GetLength(1));
        plane.Objects.Add(this);
    }

    public void DrawObject()
    {
        if (ShouldObjectBeDrawn())
        {
            for (int x = 0; x < gridSize.X; x++)
            {
                for (int y = 0; y < gridSize.Y; y++)
                {
                    Vector2 pos = new Vector2(x * cellSize + Position.X, y * cellSize + Position.Y);
                    if (ObjectGrid[y,x] != 0 && ShouldPixelBeDrawn(pos))
                    {
                        
                        Rectangle rect = new Rectangle(x * cellSize + Position.X-Plane.currentPostion.X, y * cellSize + Position.Y-Plane.currentPostion.Y, cellSize,
                            cellSize);
                        
                        Raylib.DrawRectangleRec(rect,GenerateColor(ObjectGrid[y,x]));
                    }
                }
            }
        }
    }
    private Color GenerateColor(int number) {

        int red = (number * 31) % 256;   
        int green = (number * 67) % 256;
        int blue = (number * 101) % 256;
        int alpha = 255;              
    
       
        return new Color(red, green, blue, alpha);
    }

    private bool ShouldPixelBeDrawn(Vector2 pixelPosition)
    {
        return pixelPosition.X < Plane.visableSize.X + Plane.currentPostion.X &&
               pixelPosition.Y < Plane.visableSize.Y + Plane.currentPostion.Y &&
               pixelPosition.X > Plane.currentPostion.X &&
               pixelPosition.Y > Plane.currentPostion.Y;
    }
    private bool ShouldObjectBeDrawn()
    {

        return Position.X < Plane.visableSize.X + Plane.currentPostion.X
               && Position.Y < Plane.visableSize.Y + Plane.currentPostion.Y
               && Position.X + ObjectGrid.GetLength(0)*cellSize > Plane.currentPostion.X
               && Position.Y + ObjectGrid.GetLength(1)*cellSize > Plane.currentPostion.Y;

    }
}