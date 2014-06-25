using System;

namespace Home_Front
{
    static class Program
    {        
        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
}

