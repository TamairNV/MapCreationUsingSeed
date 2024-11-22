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
    private Dictionary<Vector2, List<Vector2>> directionDict;
    public static int globalExpandCount = 0;
    public int expandCound = 0;
    private List<Rectangle> tunnels = new List<Rectangle>();
    public PlaneObject(int[,] grid, Vector2 position,MovablePlane plane)
    {
        ObjectGrid = grid;
        Position = position;
        Plane = plane;
        cellSize = plane.cellSize;
        gridSize = new Vector2(grid.GetLength(0), grid.GetLength(1));
        plane.Objects.Add(this);
        directionDict = getDoorDirections();

    }

    public void RunExpand(Hasher hash, List<int[,]> structures, int count = 0, int nodeExpandLength = 25)
    {
        if (count > nodeExpandLength)
        {
            return;
        }

        globalExpandCount += 1;
        count += 1;
        int index8 = 0;
        for (int x = 0; x < gridSize.X; x++)
        {
            for (int y = 0; y < gridSize.Y; y++)
            {

                if (ObjectGrid[y, x] == 8)
                {
                    int seed = hash.GetRandomFromCoords(new Vector2(Position.X + x * cellSize,
                        Position.Y + y * cellSize));
                    Random ran = new Random(seed);
                    if (ran.Next(0, 10) != 1 || true)
                    {

                        int[,] strut = structures[ran.Next(0, structures.Count)];


                        int structWidth = strut.GetLength(1);
                        int structHeight = strut.GetLength(0);


                        Vector2 centerOffset = new Vector2(structWidth / 2, structHeight / 2); // (middleX, middleY)

                        Vector2 directionOffset;

                        try
                        {
                            directionOffset = directionDict[new Vector2(y, x)][index8];
                        }
                        catch
                        {
                            index8 = 0;
                            directionOffset = directionDict[new Vector2(y, x)][index8];

                        }

                        index8++;
                        directionOffset = new Vector2(-directionOffset.Y, -directionOffset.X);


                        Vector2 placementPosition = new Vector2(Position.X + x * cellSize, Position.Y + y * cellSize);


                        Vector2 adjustedPosition = new Vector2(
                            placementPosition.X -
                            centerOffset.X * cellSize, // Offset based on the width of the structure
                            placementPosition.Y -
                            centerOffset.Y * cellSize // Offset based on the height of the structure
                        );

                        adjustedPosition += directionOffset * 6 * cellSize;
                        Rectangle arrayBounds2 = new Rectangle(adjustedPosition.X, adjustedPosition.Y,
                            structWidth * cellSize,
                            structHeight * cellSize);
                        foreach (var obj in Plane.Objects)
                        {
                            Rectangle arrayBounds1 = new Rectangle(obj.Position.X, obj.Position.Y,
                                obj.ObjectGrid.GetLength(0) * cellSize,
                                obj.ObjectGrid.GetLength(1) * cellSize);
                            if (Raylib.CheckCollisionRecs(arrayBounds1, arrayBounds2))
                            {
                                return;
                            }
                        }

                        Rectangle tunnel = DrawBackWardsRect(placementPosition,
                            new Vector2(5 * cellSize, 5 * cellSize) * directionOffset +
                            new Vector2(cellSize, cellSize));


                        tunnels.Add(tunnel);
                        
                        PlaneObject newObj = new PlaneObject(strut, adjustedPosition, Plane);
                        Vector2 newDir = newObj.directionDict.First().Value[0];
                        newDir = new Vector2(-newDir.Y, -newDir.X);
                        if (newDir != new Vector2(-directionOffset.X,-directionOffset.Y))
                        {
                            if (newDir.X == -directionOffset.Y && newDir.Y == directionOffset.X)
                            {
                                newObj.ObjectGrid = RotateGrid90(newObj.ObjectGrid);
                            }
                            else if (newDir.X == directionOffset.Y && newDir.Y == -directionOffset.X)
                            {
                                newObj.ObjectGrid = RotateGridNegative90(newObj.ObjectGrid);
                            }
                            else if (newDir == directionOffset)
                            {
                                newObj.ObjectGrid = RotateGrid90(RotateGrid90(newObj.ObjectGrid)); // Rotate twice for 180 degrees
                            }
                            
                            
                            newObj.directionDict = newObj.getDoorDirections();
                        }

                        if (newObj.directionDict.Keys.Count != 1)
                        {
                            newObj.RunExpand(hash, structures, count, nodeExpandLength);
                        }
                        
                       
                    }
                    
                }
            }
        }
    }

    public static void PrintGrid(int[,] grid)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(grid[i, j] + "\t"); // Print each element with a tab space
            }
            Console.WriteLine(); // Move to the next line after each row
        }
        Console.WriteLine("-------------------------");
    }


    private int[,] RotateGrid90(int[,] grid)
    {
        int originalRows = grid.GetLength(0);
        int originalCols = grid.GetLength(1);
        int[,] newGrid = new int[originalCols, originalRows];

        for (int x = 0; x < originalCols; x++)
        {
            for (int y = 0; y < originalRows; y++)
            {
                newGrid[x, y] = grid[originalRows - 1 - y, x];
            }
        }

        return newGrid;
    }
    private int[,] RotateGridNegative90(int[,] grid)
    {
        int originalRows = grid.GetLength(0);
        int originalCols = grid.GetLength(1);
        int[,] newGrid = new int[originalCols, originalRows];

        for (int x = 0; x < originalCols; x++)
        {
            for (int y = 0; y < originalRows; y++)
            {
                newGrid[x, y] = grid[y, originalCols - 1 - x];
            }
        }

        return newGrid;
    }


    public static Rectangle DrawBackWardsRect(Vector2 position, Vector2 size)
    {
        Vector2 newPosition = new Vector2();
        Vector2 newSize = new Vector2();
        if (size.X < 0)
        {
            newSize.X = Math.Abs(size.X);
            newPosition.X = position.X - newSize.X;
        }
        else
        {
            newSize.X = size.X;
            newPosition.X = position.X;
        }
        if (size.Y < 0)
        {
            newSize.Y = Math.Abs(size.Y);
            newPosition.Y = position.Y - newSize.Y;
        }
        else
        {
            newSize.Y = size.Y;
            newPosition.Y = position.Y;
        }

        return new Rectangle(newPosition, newSize);
    }

    private List<PlaneObject> GetClipedArray(int amount)
    {
        int i = 0;
        List<PlaneObject> clippedList = new List<PlaneObject>();
        foreach (var obj in Plane.Objects.ToArray().Reverse())
        {

            clippedList.Add(obj);
            if (i > amount)
            {
                return clippedList;
            }
            i++;
            
        }

        return clippedList;
    }

    private Dictionary<Vector2, List<Vector2>> getDoorDirections()
    {
        Dictionary<Vector2, List<Vector2>> directionDict = new Dictionary<Vector2, List<Vector2>>();
        for (int x = 0; x < gridSize.X; x++)
        {
            for (int y = 0; y < gridSize.Y; y++)
            {
                if (ObjectGrid[y, x] == 8)
                {
                    directionDict.Add(new Vector2(y,x),GetDirection(new Vector2(y,x)));
                }
            }
        }

        return directionDict;
    }

    private List<Vector2> GetDirection(Vector2 pos)
    {
        // Define possible movement directions
        Vector2[] directions = new[]
        {
            new Vector2(0, 1),   // Up
            new Vector2(0, -1),  // Down
            new Vector2(1, 0),   // Right
            new Vector2(-1, 0),  // Left
        };

        List<Vector2> allowedDirections = new List<Vector2>();

        // Check each direction around the given position
        foreach (var direction in directions)
        {
            Vector2 checkPos = pos + direction;

            // Ensure the position is within bounds
            if (checkPos.X < 0 || checkPos.X >= gridSize.X || checkPos.Y < 0 || checkPos.Y >= gridSize.Y)
            {
                continue; // Out of bounds, skip this direction
            }

            // Check the grid value at the new position
            if (ObjectGrid[(int)checkPos.Y, (int)checkPos.X] == 0) // Unblocked if the value is 0
            {
                allowedDirections.Add(direction); // Add this direction to the list
            }
        }


        return allowedDirections;
    }


    

    public void DrawObject()
    {
        if (ShouldObjectBeDrawn())
        {
            foreach (var tunnel in tunnels)
            {
                Rectangle temp = new Rectangle(tunnel.Position - Plane.currentPostion, tunnel.Size);
                Raylib.DrawRectangleRec(temp,Color.Black);
            }
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
    public static Color GenerateColor(int number) {

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