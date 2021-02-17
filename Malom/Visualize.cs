using System;

namespace Malom
{
    class Visualize
    {
        private GameTable gameTable;

        public Visualize(GameTable gameTable)
        {
            this.gameTable = gameTable;
        }

        public void Print()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Kék bábuk: { gameTable.GetNumberOfFields(GameFieldPlayer.Blue) }");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Piros bábuk: { gameTable.GetNumberOfFields(GameFieldPlayer.Red) }");

            Console.ForegroundColor = ConsoleColor.White;

            for (int i = 0; i < gameTable.Fields.GetLength(0); i++)
            {
                for (int j = 0; j < gameTable.Fields.GetLength(1); j++)
                {
                    switch (gameTable.Fields[i, j].Player)
                    {
                        case GameFieldPlayer.None:
                            if (gameTable.Fields[i, j].Useable)
                            {
                                Console.Write(" O ");
                            }
                            else
                            {
                                Console.Write(" + ");
                            }
                            break;
                        case GameFieldPlayer.Red:
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.Write(" O ");
                            Console.BackgroundColor = ConsoleColor.Black;
                            break;
                        case GameFieldPlayer.Blue:
                            Console.BackgroundColor = ConsoleColor.Blue;
                            Console.Write(" O ");
                            Console.BackgroundColor = ConsoleColor.Black;
                            break;
                    }
                }

                Console.WriteLine();
            }
        }
    }
}
