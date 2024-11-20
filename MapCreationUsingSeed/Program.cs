
using System.Numerics;
using Raylib_cs;
using System.IO;
using System.Text.Json;


public class Program
{
    public static void Main()
    {
        Raylib.InitWindow(950, 751, "Map Creation");
        Raylib.SetTargetFPS(300);
        while (!Raylib.WindowShouldClose())
        {
            
            Raylib.EndDrawing();
        }
        Raylib.CloseWindow();
        

    }
}