
using System.Numerics;
using Raylib_cs;
using System.IO;
using System.Text.Json;
using MapCreationUsingSeed;


public class Program
{
    public static void Main()
    {
        Raylib.InitWindow(950, 751, "Map Creation");
        Raylib.SetTargetFPS(300);


        int[,] g = new int[,]
        {
            {0,0,0,0,0},
            {0,1,1,1,1},
            {0,0,1,1,0},
            {0,0,1,0,0},
            {0,0,0,0,0}
        };

        QuickDisplay test = new QuickDisplay(100, 100, 10, g, Color.Orange);
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Blue);
  
            
            QuickDisplay.DrawDisplays();
            Raylib.EndDrawing();
        }
        Raylib.CloseWindow();
        

    }
}