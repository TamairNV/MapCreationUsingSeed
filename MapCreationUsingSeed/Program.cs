
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
        
        MovablePlane plane = new MovablePlane(900, 750, 5);

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
            {0,0,0,0,0,0,0},
            {0,1,1,8,1,1,0},
            {0,1,0,0,0,1,0},
            {0,8,0,0,0,8,0},
            {0,1,0,0,0,1,0},
            {0,1,1,1,1,1,0},
            {0,0,0,0,0,0,0}
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
            {1,1,1,1,1,8,1,1,1,1,1},
            {1,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,1},
            {8,0,0,0,0,0,0,0,0,0,8},
            {1,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,1},
            {1,1,1,1,1,8,1,1,1,1,1},

        });
        structures.Add(new int[,]
        {
            {1,1,10,1,1},
            {1,0,0,0,1},
            {8,10,10,10,8},
            {1,0,0,0,1},
            {1,1,10,1,1}
        });
        Hasher hasher = new Hasher("banana");
        List<string> names = new List<string>() { "1", "2","3","4","5", "6","7","8","9","10","11","12"};
        
        Painter p = new Painter(structures,names,plane);
        PlaneObject Oobj = new PlaneObject(new int[,]
        {
            {0,0,0,0,0},
            {0,4,8,4,0},
            {0,8,9,8,0},
            {0,4,8,4,0},
            {0,0,0,0,0}
        }, new Vector2(300, 300), plane);
        Oobj.InitiateBranching(hasher,structures,1);
        int i = 0;
        int t = 0;
        while (!Raylib.WindowShouldClose())
        {
            if (t % 1 == 0)
            {
                i += 1;
            }
            plane.Objects[i].RunExpand(hasher,structures,0,3);
            
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