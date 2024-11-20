
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


        int[,] g = new int[,]
        {
            {0,0,0,0,0},
            {0,1,2,1,8},
            {0,0,6,7,0},
            {0,0,1,0,0},
            {0,0,0,0,0}
        };
        MovablePlane plane = new MovablePlane(900, 750, 15);
        Vector2 pos = new Vector2(100, 100);
        PlaneObject testObj = new PlaneObject(g,pos,plane);
        PlaneObject testObj2 = new PlaneObject(g,new Vector2(800, 100),plane);
        //QuickDisplay test = new QuickDisplay(100, 100, 10, g, Color.Orange);
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Blue);
            plane.HandlePlaneMovement();
            plane.DrawAllObjects();
            QuickDisplay.DrawDisplays();
            Raylib.EndDrawing();
        }
        Raylib.CloseWindow();
        

    }
}