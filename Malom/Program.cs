using System;

namespace Malom
{
    class Program
    {
        static void Main(string[] args)
        {
            GameLogic logic = new GameLogic();

            Console.WriteLine("Szeretnéd a korábban elmentett játékállst visszaállítani? (i/n)");
            string input = Console.ReadLine();

            bool newGame = true;

            if (input == "i")
            {
                logic.Load();
                newGame = false;
            }

            bool game = true;

            logic.Refresh();

            while (game)
            {
                logic.PlayTheGame(newGame);

                Console.WriteLine("Szeretnél új játékot kezdeni? (i/n)");
                input = Console.ReadLine();

                if (input == "n")
                {
                    game = false;
                }
            }

            Console.Clear();
            Console.WriteLine("A program bezárásához nyomj meg egy gombot.");
            Console.ReadKey();
        }
    }
}
