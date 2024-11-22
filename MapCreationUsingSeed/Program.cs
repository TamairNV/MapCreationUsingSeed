
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
        
        MovablePlane plane = new MovablePlane(900, 750, 2);
        Vector2 pos = new Vector2(100, 100);

        //QuickDisplay test = new QuickDisplay(100, 100, 10, g, Color.Orange);

        List<int[,]> structures = new List<int[,]>();
        structures.Add(new int[,]
        {
            {0,1,1,1,0},
            {0,1,0,1,0},
            {0,8,0,8,0},
            {0,1,0,1,0},
            {0,1,1,1,0}
        });
        structures.Add(new int[,]
        {
            {0,0,0,0,0},
            {0,1,8,1,0},
            {0,8,0,8,0},
            {0,1,0,1,0},
            {0,1,1,1,0}
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
            {0,8,0,8,0},
            {0,1,8,1,0},
            {0,0,0,0,0}
        });
        structures.Add(new int[,]
        {
            {1,1,8,1,1},
            {1,0,0,0,1},
            {8,0,0,0,8},
            {1,0,0,0,1},
            {1,1,8,1,1}
        });
        structures.Add(new int[,]
        {
            {1,1,1,1,8,1,1,1,1,1},
            {1,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,1},
            {8,0,0,0,0,0,0,0,0,8},
            {1,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,1},
            {1,1,1,1,8,1,1,1,1,1},

        });
        structures.Add(new int[,]
        {
            {1,1,10,1,1},
            {1,0,0,0,1},
            {8,10,10,10,8},
            {1,0,0,0,1},
            {1,1,10,1,1}
        });
        Hasher hasher = new Hasher("jam5e55356s");
        List<string> names = new List<string>() { "1", "2","3","4","5", "6","7","8","9","10"};
        
        Painter p = new Painter(structures,names,plane);
        PlaneObject Oobj = new PlaneObject(new int[,]
        {
            {0,0,0,0,0},
            {0,10,8,10,0},
            {0,8,0,8,0},
            {0,10,8,10,0},
            {0,0,0,0,0}
        }, new Vector2(300, 300), plane);
        Oobj.RunExpand(hasher,structures, 0,100);
        while (!Raylib.WindowShouldClose())
        {
            
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