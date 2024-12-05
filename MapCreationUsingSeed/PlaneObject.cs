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
    
    private List<Rectangle> tunnels = new List<Rectangle>();
    public PlaneObject(int[,] grid, Vector2 position,MovablePlane plane)
    {
        ObjectGrid = grid;
        Position = position;
        Plane = plane;
        cellSize = plane.cellSize;
        gridSize = new Vector2(grid.GetLength(0), grid.GetLength(1));
        plane.Objects.Add(this);
        directionDict = GetDoorDirections();

    }


    public void RunExpand(Hasher hash, List<int[,]> structures, int count = 0, int nodeExpandLength = 25)
    {
        if (count > nodeExpandLength)
        {
            return;
        }
        
        count += 1;
        int index8 = 0;
        Random rng = hash.random;
        List<Vector2> shuffledKeys = directionDict.Keys.ToList(); 
        
        for (int i = shuffledKeys.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
 
            (shuffledKeys[i], shuffledKeys[j]) = (shuffledKeys[j], shuffledKeys[i]);
        }
        foreach (var location in directionDict.Keys)
        {
            int x = Convert.ToInt16(location.Y);
            int y = Convert.ToInt16(location.X);
            
            int seed = hash.GetRandomFromCoords(new Vector2(Position.X + x * cellSize,
                Position.Y + y * cellSize));
            Random ran = new Random(seed);
            if (ran.Next(0, 10) != 1)
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
                    centerOffset.X * cellSize, 
                    placementPosition.Y -
                    centerOffset.Y * cellSize 
                );

                


                adjustedPosition += directionOffset * 6 * cellSize;
                Rectangle arrayBounds2 = new Rectangle(adjustedPosition.X, adjustedPosition.Y,
                    structWidth * cellSize,
                    structHeight * cellSize);

                bool isCollide = false;
                for (int i = 1; i < Plane.Objects.Count; i++)
                {
                    PlaneObject obj = Plane.Objects[^i];
                    Rectangle arrayBounds1 = new Rectangle(obj.Position.X, obj.Position.Y,
                        obj.ObjectGrid.GetLength(0) * cellSize,
                        obj.ObjectGrid.GetLength(1) * cellSize);
                    if (Raylib.CheckCollisionRecs(arrayBounds1, arrayBounds2))
                    {
                        isCollide = true;
                        break;
                    }
                }

                if (!isCollide)
                {
                    PlaneObject newObj = new PlaneObject(strut, adjustedPosition, Plane);//Create new Object
                    Vector2 newDir = newObj.directionDict.First().Value[0]; //Get direction of a door on new object
                    newDir = new Vector2(-newDir.Y, -newDir.X); // reverse cus im silly like that
                    
                    newObj.ObjectGrid = RotateGridToAlign(newDir, directionOffset, newObj.ObjectGrid); // rotate the grid so the door is facing the connecting door.
                    
                    newObj.directionDict = newObj.GetDoorDirections(); // reset the door positions

                    Vector2 closestDoor = GetClosestDoor(newObj, placementPosition);
                    
                    Vector2 pos2 = newObj.Position + new Vector2(closestDoor.X,closestDoor.Y) * cellSize;
                    Vector2 dis = pos2-placementPosition;
                    dis = new Vector2(Math.Abs(dis.X), Math.Abs(dis.Y)) * directionOffset;
                    
                    if (dis.X == 0)
                    {
                        dis.X = cellSize;
                    }

                    if (dis.Y == 0)
                    {
                        dis.Y = cellSize;
                        
                    }

                    Rectangle tunnel = DrawBackWardsRect(placementPosition, dis);
                    tunnels.Add(tunnel);

                    if (newObj.directionDict.Keys.Count != 1)
                    {
                        newObj.RunExpand(hash, structures, count, nodeExpandLength);
                    }

                }
            }
        }
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




    private Vector2 GetClosestDoor(PlaneObject obj, Vector2 rootPosition)
    {
        Vector2 closestDoor = obj.directionDict.First().Value[0];
        float smallestDistance = 1000000;
        foreach (var d in obj.directionDict.Keys)
        {
            Vector2 door = new Vector2(d.Y, d.X);
                        
            float distance = Vector2.Distance( rootPosition , door *cellSize + obj.Position );
                        
            if (Math.Abs(distance) < smallestDistance)
            {
                smallestDistance = distance;
                closestDoor = door;
            }
        }

        return closestDoor;
    }
    private int[,] RotateGridToAlign(Vector2 currentDirection, Vector2 newDirection, int[,] grid)
    {
        
        if (currentDirection != new Vector2(-newDirection.X, -newDirection.Y) )
        {
            if (currentDirection.X == -newDirection.Y && currentDirection.Y == newDirection.X)
            {
                grid = RotateGrid90(grid);
            }
            else if (currentDirection.X == newDirection.Y && currentDirection.Y == -newDirection.X)
            {
                grid = RotateGridNegative90(grid);
            }
            else if (currentDirection == newDirection)
            {
                grid =
                    RotateGrid90(RotateGrid90(grid)); // Rotate twice for 180 degrees
            }
        }

        return grid;


    }
    
    private Dictionary<Vector2, List<Vector2>> GetDoorDirections()
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
        // Possible movement directions
        Vector2[] directions = 
        {
            new Vector2(0, 1),   // Up
            new Vector2(0, -1),  // Down
            new Vector2(1, 0),   // Right
            new Vector2(-1, 0),  // Left
        };

        List<Vector2> allowedDirections = new List<Vector2>();

        foreach (var direction in directions)
        {
            Vector2 checkPos = pos + direction;

            // Ensure the check position is within bounds
            if (checkPos.X >= 0 && checkPos.X < gridSize.X &&
                checkPos.Y >= 0 && checkPos.Y < gridSize.Y)
            {
                // If it's a door, add it to allowed directions
                if (ObjectGrid[(int)checkPos.Y, (int)checkPos.X] == 0) 
                {
                    allowedDirections.Add(direction);
                }
            }
        }
        return allowedDirections;
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
    public void DrawObject()
    {
        if (ShouldObjectBeDrawn())
        {
            foreach (var tunnel in tunnels)
            {
                Rectangle tunnelRect = new Rectangle(tunnel.Position - Plane.currentPostion, tunnel.Size);
                Raylib.DrawRectangleRec(tunnelRect,Color.Black);
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