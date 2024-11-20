using Raylib_cs;

namespace MapCreationUsingSeed;

public class HoverDisplay
{
    public static List<HoverDisplay> HoverDisplays = new List<HoverDisplay>();
    public Rectangle background;
    private QuickDisplay popUpDisplay;
    private string text;
    public int[,] grid;
    public HoverDisplay(int x , int y , int width, int height,string text,int[,] grid)
    {
        this.text = text;
        background = new Rectangle(x, y, width, height);
        HoverDisplays.Add(this);
        this.grid = grid;
        popUpDisplay = new QuickDisplay(x, y, 10, grid, Color.Magenta);
    }

    public void HandelDisplay(Rectangle mouseRect)
    {
        Raylib.DrawRectangleRec(background,Color.Magenta);
        DrawTextCentered(background,text,20);
        
        if (Raylib.CheckCollisionRecs(background, mouseRect))
        {
            popUpDisplay.DrawDisplay();
            popUpDisplay.displayOn = true;
        }
        
    }
    public static void DrawDisplays()
    {
        Rectangle mouseRect = new Rectangle(Raylib.GetMousePosition(), 1, 1);
        foreach (var display in HoverDisplays)
        {
            display.HandelDisplay(mouseRect);
        }
    }
    
    public static void DrawTextCentered(Rectangle rect, string text,int fontSize = 20)
    {
        

        int textWidth = Raylib.MeasureText(text, fontSize);
        int textHeight = fontSize;  
        
        int xPosition = (int)(rect.X + (rect.Width - textWidth) / 2);
        int yPosition = (int)(rect.Y + (rect.Height - textHeight) / 2);

       
        Raylib.DrawText(text, xPosition, yPosition, fontSize, Color.Black);
    }
}