using System.Numerics;
using Raylib_cs;
namespace MapCreationUsingSeed;

public class MovablePlane
{
    public Vector2 visableSize;
    public Vector2 currentPostion = new Vector2(0, 0);
    public int cellSize;

    public List<PlaneObject> Objects = new List<PlaneObject>();

    public MovablePlane(int sizeX, int sizeY, int cellSize)
    {
        visableSize = new Vector2(sizeX, sizeY);
        this.cellSize = cellSize;
    }

    public void DrawAllObjects()
    {
        foreach (var object_ in Objects)
        {
       
            object_.DrawObject();
        }
    }

    private Vector2 mousePressPosition = Vector2.Zero;
    private bool isBeingPressed;
    private Vector2 differance;
    public void HandlePlaneMovement()
    {
        
        if (Raylib.IsMouseButtonDown(0))
        {

            if (mousePressPosition != Vector2.Zero)
            {
                differance = mousePressPosition - Raylib.GetMousePosition();
                currentPostion += differance;
            }
            mousePressPosition = Raylib.GetMousePosition();

            
            
            
        }

        if (Raylib.IsMouseButtonReleased(0))
        {
            mousePressPosition = Vector2.Zero;
        }
    }
    
    
    
}