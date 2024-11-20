
using System.Numerics;
using Raylib_cs;
using System.IO;
using System.Text.Json;
using MapCreationUsingSeed;
using Object = System.Object;


public class Program
{
    public static void Main()
    {
        Raylib.InitWindow(950, 751, "Map Creation");
        Raylib.SetTargetFPS(300);
        
        MovablePlane plane = new MovablePlane(900, 750, 15);
        Vector2 pos = new Vector2(100, 100);

        //QuickDisplay test = new QuickDisplay(100, 100, 10, g, Color.Orange);

        List<int[,]> structures = new List<int[,]>();
        structures.Add(new int[,]
        {
            {0,0,0,0,0},
            {0,1,8,1,0},
            {0,1,0,1,0},
            {0,1,1,1,0},
            {0,0,0,0,0}
        });
        structures.Add(new int[,]
        {
            {0,0,0,0,0},
            {0,1,1,1,0},
            {0,1,0,8,0},
            {0,1,1,1,0},
            {0,0,0,0,0}
        });
        structures.Add(new int[,]
        {
            {0,0,0,0,0},
            {0,1,1,1,0},
            {0,1,0,1,0},
            {0,1,8,1,0},
            {0,0,0,0,0}
        });
        
        structures.Add(new int[,]
        {
            {0,0,0,0,0},
            {0,1,1,1,0},
            {0,8,0,1,0},
            {0,1,1,1,0},
            {0,0,0,0,0}
        });
        structures.Add(new int[,]
        {
            {0,0,0,0,0},
            {0,1,1,1,0},
            {0,8,0,8,0},
            {0,1,1,1,0},
            {0,0,0,0,0}
        });
        structures.Add(new int[,]
        {
            {0,0,0,0,0},
            {0,1,8,1,0},
            {0,1,0,1,0},
            {0,1,8,1,0},
            {0,0,0,0,0}
        });
        Hasher hasher = new Hasher("this is the seed");
        List<string> names = new List<string>() { "1", "2","3","4","5", "6"};
        
        Painter p = new Painter(structures,names,plane);
        while (!Raylib.WindowShouldClose())
        {


            if (Raylib.IsKeyPressed(KeyboardKey.A))
            {
                foreach (var obj in plane.Objects.ToArray())
                {
                    obj.RunExpand(hasher,structures);
        
                }
            }
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Blue);
            plane.HandlePlaneMovement();
            plane.DrawAllObjects();
            p.HandlePainter();
         
            Raylib.EndDrawing();
        }
        Raylib.CloseWindow();
        

    }
}