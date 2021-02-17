using System;
using System.IO;
using System.Text;

namespace Malom
{
    class GameLogic
    {
        private static Random rnd = new Random();

        private const GameFieldPlayer user = GameFieldPlayer.Blue;
        private const GameFieldPlayer cpu = GameFieldPlayer.Red;

        private bool isRunning = true;

        public GameTable GameTable { get; private set; }
        public Visualize Visualize { get; private set; }
        public int RedFields {
            get
            {
                return GameTable.GetNumberOfFields(GameFieldPlayer.Red);
            }
        }
        public int BlueFields
        {
            get
            {
                return GameTable.GetNumberOfFields(GameFieldPlayer.Blue);
            }
        }

        public GameLogic()
        {
            GameTable = new GameTable();
            Visualize = new Visualize(GameTable);
        }

        public void PlayTheGame(bool newGame)
        {
            isRunning = true;

            if (newGame)
            {
                GameTable = new GameTable();
                Visualize = new Visualize(GameTable);
                PhaseOne();
            }
            
            PhaseTwo();
            GameOver();
        }

        //Első fázis: Bábuk elhelyezése
        private void PhaseOne()
        {

            for (int i = 0; i < 8; i++)
            {
                while (isRunning && !GetCoordinatesFromConsole(GameFieldPlayer.Blue))
                {
                    Refresh();
                   

                    Console.WriteLine("Rossz formátumban adtad meg a koordinátákat!");
                }

                if (GameTable.SteppedToMill(user))
                {
                    while (!GetCoordinatesToRemoveTheOpponentFromConsole(user))
                    {
                        Refresh();

                        Console.WriteLine("Rossz formátumban adtad meg a koordinátákat!");
                    }

                    Refresh();
                }

                //For debugging
                //SetCoordinatesByCPU(user);

                SetCoordinatesByCPU(cpu);

                Visualize.Print();
            }
        }

        private bool GetCoordinatesFromConsole(GameFieldPlayer player)
        {
            if (player == GameFieldPlayer.Blue)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Add meg a koordinánát, ahova lépni szeretnél. (0 és 6 között) (Elvárt formátum: 4,6): ");
                string input = Console.ReadLine();

                Console.ForegroundColor = ConsoleColor.White;

                int i = int.Parse(input.Split(',')[0]);
                int j = int.Parse(input.Split(',')[1]);

                if (!GameTable.IsFree(i, j))
                {
                    return false;
                }

                GameTable.SetPlayer(player, i, j);

                return true;
            }

            return false;
        }

        private void SetCoordinatesByCPU(GameFieldPlayer player = GameFieldPlayer.Red)
        {
            int i, j;

            do
            {
                i = rnd.Next(0, 7);
                j = rnd.Next(0, 7);

            } while (!GameTable.IsFree(i, j));

            GameTable.SetPlayer(player, i, j);
        }

        //Második fázis
        private void PhaseTwo()
        {
            while (isRunning && RedFields > 2 && BlueFields > 2)
            {
                //Játékos
                if (BlueFields > 3)
                {
                    while (!GetCoordinatesToMoveFromConsole(user))
                    {
                        Refresh();

                        Console.WriteLine("Rossz formátumban adtad meg a koordinátákat!");
                    }

                    Refresh();

                    if (GameTable.SteppedToMill(user))
                    {
                        while (!GetCoordinatesToRemoveTheOpponentFromConsole(user))
                        {
                            Refresh();

                            Console.WriteLine("Rossz formátumban adtad meg a koordinátákat!");
                        }

                        Refresh();
                    }

                    //For debugging
                    //GameTable.MoveCPU(user);

                    //Refresh();

                    if (GameTable.SteppedToMill(user))
                    {
                        GameTable.RemoveOpponentCPU();

                        Refresh();
                    }
                }
                else
                {
                    while (!GetCoordinatesToMoveFromConsoleEndGame(user))
                    {
                        Refresh();

                        Console.WriteLine("Rossz formátumban adtad meg a koordinátákat!");
                    }

                    Refresh();

                    if (GameTable.SteppedToMill(user))
                    {
                        while (!GetCoordinatesToRemoveTheOpponentFromConsole(user))
                        {
                            Refresh();

                            Console.WriteLine("Rossz formátumban adtad meg a koordinátákat!");
                        }

                        Refresh();
                    }
                }

                //Számítógép

                if (RedFields > 3)
                {
                    GameTable.MoveCPU(cpu);

                    Refresh();

                    if (GameTable.SteppedToMill(cpu))
                    {
                        GameTable.RemoveOpponentCPU();

                        Refresh();
                    }
                }
                else
                {
                    GameTable.MoveCPU(cpu);

                    Refresh();

                    if (GameTable.SteppedToMill(cpu))
                    {
                        GameTable.RemoveOpponentCPU();

                        Refresh();
                    }
                }
            }
        }

        private void GameOver()
        {
            if (isRunning)
            {
                if (BlueFields == 2)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;

                    Console.WriteLine("Vége a játéknak! A piros játékos nyert.");

                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Blue;

                    Console.WriteLine("Vége a játéknak! A kék játékos nyert.");

                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        public void Refresh()
        {
            Console.Clear();
            Visualize.Print();
        }

        private bool GetCoordinatesToMoveFromConsole(GameFieldPlayer player)
        {
            if (player == GameFieldPlayer.Blue)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Add meg a koordinánát, amelyik bábuval lépni szeretnél. (0 és 6 között) (Elvárt formátum: 4,6) (exit: mentés és kilépés): ");
                string input = Console.ReadLine();

                if (input == "exit")
                {
                    Save();
                    isRunning = false;

                    return true;
                }
                else
                {
                    int iFrom = int.Parse(input.Split(',')[0]);
                    int jFrom = int.Parse(input.Split(',')[1]);

                    Console.Write("A nyilak segítségével add meg, hogy melyik irányba szeretnél tovább haladni.");

                    ConsoleKey keyPressed;

                    do
                    {
                        keyPressed = Console.ReadKey(false).Key;
                    } while (keyPressed != ConsoleKey.UpArrow && keyPressed != ConsoleKey.DownArrow && keyPressed != ConsoleKey.LeftArrow && keyPressed != ConsoleKey.RightArrow);

                    Console.ForegroundColor = ConsoleColor.White;

                    switch (keyPressed)
                    {
                        case ConsoleKey.UpArrow:
                            return GameTable.MovePlayer(player, iFrom, jFrom, Direction.Up);
                        case ConsoleKey.DownArrow:
                            return GameTable.MovePlayer(player, iFrom, jFrom, Direction.Down);
                        case ConsoleKey.LeftArrow:
                            return GameTable.MovePlayer(player, iFrom, jFrom, Direction.Left);
                        case ConsoleKey.RightArrow:
                            return GameTable.MovePlayer(player, iFrom, jFrom, Direction.Right);
                    }
                }
            }

            return false;
        }

        private bool GetCoordinatesToMoveFromConsoleEndGame(GameFieldPlayer player)
        {
            if (player == GameFieldPlayer.Blue)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Add meg a koordinánát, amelyik bábuval lépni szeretnél. (0 és 6 között) (Elvárt formátum: 4,6) (exit: mentés és kilépés): ");
                string input = Console.ReadLine();

                if (input == "exit")
                {
                    Save();
                    isRunning = false;

                    return true;
                }
                else
                {
                    int iFrom = int.Parse(input.Split(',')[0]);
                    int jFrom = int.Parse(input.Split(',')[1]);

                    Console.Write("Add meg a koordinánát, ahova lépni szeretnél. (0 és 6 között) (Elvárt formátum: 4,6): ");
                    input = Console.ReadLine();

                    Console.ForegroundColor = ConsoleColor.White;

                    int iTo = int.Parse(input.Split(',')[0]);
                    int jTo = int.Parse(input.Split(',')[1]);

                    if (!GameTable.IsFree(iTo, jTo))
                    {
                        return false;
                    }

                    return GameTable.MovePlayer(player, iFrom, jFrom, iTo, jTo);
                }
            }

            return false;
        }

        private bool GetCoordinatesToRemoveTheOpponentFromConsole(GameFieldPlayer player)
        {
            if (player == GameFieldPlayer.Blue)
            {
                Console.ForegroundColor = ConsoleColor.Blue;

                Console.WriteLine("MALOM!");
                Console.Write("Add meg a koordinánát, ahonnan az ellenfél bábuját el szeretnéd venni (0 és 6 között) (Elvárt formátum: 4,6) (exit: mentés és kilépés): ");
                string input = Console.ReadLine();

                if (input == "exit")
                {
                    Save();
                    isRunning = false;

                    return true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;

                    int i = int.Parse(input.Split(',')[0]);
                    int j = int.Parse(input.Split(',')[1]);

                    return GameTable.RemoveOpponent(player, i, j);
                }
            }

            return false;
        }
        
        public void Load()
        {
            if (File.Exists("game.save"))
            {
                StreamReader streamReader = new StreamReader("game.save", Encoding.UTF8);

                string input = streamReader.ReadLine();

                string[] lines = input.Split(';');

                GameTable = new GameTable();
                Visualize = new Visualize(GameTable);

                for (int i = 0; i < lines.Length - 1; i++)
                {
                    string[] cells = lines[i].Split(',');

                    for (int j = 0; j < cells.Length - 1; j++)
                    {
                        GameTable.Fields[i, j].Player = (GameFieldPlayer)int.Parse(cells[j]);
                    }
                }
                Refresh();
            }
        }

        public void Save()
        {
            string output = "";

            for (int i = 0; i < GameTable.Fields.GetLength(0); i++)
            {
                for (int j = 0; j < GameTable.Fields.GetLength(1); j++)
                {
                    output += $"{(int)GameTable.Fields[i, j].Player},";
                }

                output += ";";
            }

            StreamWriter streamWriter = new StreamWriter("game.save", false, Encoding.UTF8);
            streamWriter.WriteLine(output);
            streamWriter.Close();
        }
    }
}
