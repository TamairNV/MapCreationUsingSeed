using System.Numerics;
using Raylib_cs;

namespace MapCreationUsingSeed;

public class Painter
{
    private Rectangle background = new Rectangle();
    private List<HoverDisplay> HoverDisplays = new List<HoverDisplay>();
    private List<int[,]> structures;
    public int[,] brush;
    private MovablePlane plane;
    public Painter(List<int[,]> structures,List<string> names,MovablePlane plane)
    {
        this.plane = plane;
        this.structures = structures;
        background = new Rectangle(800, 0, 150, 750);
        int yPos = 35;
        int i = 0;
        foreach (var structure in structures)
        {
            HoverDisplays.Add(new HoverDisplay(838,yPos,75,50,names[i],structure));
            yPos += 75;
            i++;
        }

        brush = structures[0];
    }


    public void HandlePainter()
    {
        Rectangle mouseRect = new Rectangle(Raylib.GetMousePosition(), 1, 1);
        Raylib.DrawRectangleRec(background,Color.Gray);
        foreach (var display in HoverDisplays)
        {
            display.HandelDisplay(mouseRect);
            if (Raylib.CheckCollisionRecs(mouseRect, display.background))
            {
                if (Raylib.IsMouseButtonPressed(0))
                {
                    brush = display.grid;
                }
            }
        }
        
        if (Raylib.IsMouseButtonPressed(MouseButton.Right))
        {
            Vector2 offset = new Vector2(brush.GetLength(0) * plane.cellSize / 2,
                brush.GetLength(1) * plane.cellSize / 2);
            plane.Objects.Add(new PlaneObject(brush,Raylib.GetMousePosition() + plane.currentPostion -offset ,plane));
        }
 
    }
}
