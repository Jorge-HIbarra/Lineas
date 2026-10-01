using Raylib_cs;
using System; 

static void DrawLineDDA(int x1, int y1, int x2, int y2, bool[,] pixels, int n)
{

    int dx = x2 - x1;
    int dy = y2 - y1;
    int steps = Math.Max(Math.Abs(dx), Math.Abs(dy));
    if (steps == 0) { pixels[x1, y1] = true ; return; }

    float x = x1, y = y1;
    float xInc = dx / (float)steps;
    float yInc = dy / (float)steps;
    for (int i = 0; i <= steps; i++) {
        int px = (int)MathF.Round(x);
        int py = (int)MathF.Round(y);
        if (px >= 0 && px < n && py >= 0 && py < n)
            pixels[py, px] = true;
        x += xInc; y += yInc;
    }
}

Raylib.InitWindow(680, 680, "DDA Grid");
Raylib.SetTargetFPS(60);

const int N = 32;
const int cellSize = 20;
const int gridW = N * cellSize;
const int gridH = N * cellSize;
const int gridX = (680 - gridW) / 2;
const int gridY = (680 - gridH) / 2;

bool[,] pixels = new bool[N, N];
int startX = -1, startY = -1;

while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.RayWhite);

    System.Numerics.Vector2 mouse = Raylib.GetMousePosition();

    if (Raylib.CheckCollisionPointRec(mouse, new Rectangle(gridX, gridY, gridW, gridH)))
    {
        int cx = (int)((mouse.X - gridX) / cellSize);
        int cy = (int)((mouse.Y - gridY) / cellSize);

         if (Raylib.IsKeyPressed(KeyboardKey.Space))
    {
        for (int r = 0; r < N; r++)
            for (int c = 0; c < N; c++)
                pixels[r, c] = false;
        startX = -1;
        startY = -1;
    }
        
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            startX = cx;
            startY = cy;
        }

   
        if (Raylib.IsMouseButtonPressed(MouseButton.Right) && startX >= 0)
        {
            DrawLineDDA(startX, startY, cx, cy, pixels, N);
            startX = -1;
        }
    }

    for (int r = 0; r < N; r++)
    {
        for (int c = 0; c < N; c++)
        {
            int x = gridX + c * cellSize;
            int y = gridY + r * cellSize;
            Color col = pixels[r, c] ? Color.Black : Color.RayWhite;
            Raylib.DrawRectangle(x, y, cellSize, cellSize, col);
            Raylib.DrawRectangleLines(x, y, cellSize, cellSize, Color.LightGray);
        }
    }
    Raylib.DrawRectangleLines(gridX, gridY, gridW, gridH, Color.Black);
       
    Raylib.EndDrawing();
}

Raylib.CloseWindow();