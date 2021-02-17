using System;

namespace Malom
{
    class Point
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    class PossibleMill
    {
        public Point[] Points { get; set; }

        public PossibleMill(Point[] points)
        {
            Points = points; 
        }
    }

    class GameTable
    {
        private static Random rnd = new Random();

        public static PossibleMill[] possibleMills = new PossibleMill[]
        {
            new PossibleMill(new Point[] { new Point(0, 0), new Point(0, 3), new Point(0, 6)}),
            new PossibleMill(new Point[] { new Point(1, 1), new Point(1, 3), new Point(1, 5)}),
            new PossibleMill(new Point[] { new Point(2, 2), new Point(2, 3), new Point(2, 4)}),
            new PossibleMill(new Point[] { new Point(3, 0), new Point(3, 1), new Point(3, 2)}),
            new PossibleMill(new Point[] { new Point(3, 4), new Point(3, 5), new Point(3, 6)}),
            new PossibleMill(new Point[] { new Point(4, 2), new Point(4, 3), new Point(4, 4)}),
            new PossibleMill(new Point[] { new Point(5, 1), new Point(5, 3), new Point(5, 5)}),
            new PossibleMill(new Point[] { new Point(6, 0), new Point(6, 3), new Point(6, 6)}),
            new PossibleMill(new Point[] { new Point(0, 0), new Point(3, 0), new Point(6, 0)}),
            new PossibleMill(new Point[] { new Point(1, 1), new Point(3, 1), new Point(5, 1)}),
            new PossibleMill(new Point[] { new Point(2, 2), new Point(3, 2), new Point(4, 2)}),
            new PossibleMill(new Point[] { new Point(0, 3), new Point(1, 3), new Point(2, 3)}),
            new PossibleMill(new Point[] { new Point(4, 3), new Point(5, 3), new Point(6, 3)}),
            new PossibleMill(new Point[] { new Point(2, 4), new Point(3, 4), new Point(4, 4)}),
            new PossibleMill(new Point[] { new Point(1, 5), new Point(3, 5), new Point(5, 5)}),
            new PossibleMill(new Point[] { new Point(0, 6), new Point(3, 6), new Point(6, 6)}),
        };

        private bool steppedToMill;

        public GameTableField[,] Fields { get; set; }

        public GameTable()
        {
            int[,] template = new int[,]
            {
                { 1, 0, 0, 1, 0, 0, 1 },
                { 0, 1, 0, 1, 0, 1, 0 },
                { 0, 0, 1, 1, 1, 0, 0 },
                { 1, 1, 1, 0, 1, 1, 1 },
                { 0, 0, 1, 1, 1, 0, 0 },
                { 0, 1, 0, 1, 0, 1, 0 },
                { 1, 0, 0, 1, 0, 0, 1 },
            };

            Fields = new GameTableField[template.GetLength(0), template.GetLength(1)];

            for (int i = 0; i < Fields.GetLength(0); i++)
            {
                for (int j = 0; j < Fields.GetLength(1); j++)
                {
                    if (template[i, j] == 1)
                    {
                        Fields[i, j] = new GameTableField { Player = GameFieldPlayer.None, Useable = true };
                    }
                    else
                    {
                        Fields[i, j] = new GameTableField { Player = GameFieldPlayer.None, Useable = false };
                    }
                }
            }
        }

        internal bool SteppedToMill(GameFieldPlayer blue)
        {
            return steppedToMill;
        }

        public int GetNumberOfFields(GameFieldPlayer player)
        {
            int num = 0;

            for (int i = 0; i < Fields.GetLength(0); i++)
            {
                for (int j = 0; j < Fields.GetLength(1); j++)
                {
                    if (Fields[i,j].Player == player)
                    {
                        num++;
                    }
                }
            }

            return num;
        }

        public GameFieldPlayer GetPlayerType(int i, int j)
        {
            if (IsUseable(i, j))
            {
                return Fields[i, j].Player;
            }

            return GameFieldPlayer.None;
        }

        public void SetPlayer(GameFieldPlayer player, int i, int j)
        {
            Fields[i, j].Player = player;
            steppedToMill = IsInMill(player, i, j);
        }

        public bool MovePlayer(GameFieldPlayer player, int i, int j, Direction direction)
        {
            int newCoordinate;

            if (direction == Direction.Up)
            {
                newCoordinate = i;

                while (newCoordinate >= 0 && !IsFree(newCoordinate, j))
                {
                    newCoordinate--;
                }
            }
            else if (direction == Direction.Down)
            {
                newCoordinate = i;

                while (newCoordinate < Fields.GetLength(0) && !IsFree(newCoordinate, j))
                {
                    newCoordinate++;
                }
            }
            else if (direction == Direction.Right)
            {
                newCoordinate = j;

                while (newCoordinate < Fields.GetLength(1) && !IsFree(i, newCoordinate))
                {
                    newCoordinate++;
                }
            }
            else //if (direction == Direction.Left)
            {
                newCoordinate = j;

                while (newCoordinate >= 0 && !IsFree(i, newCoordinate))
                {
                    newCoordinate--;
                }
            }

            if (0 <= newCoordinate && newCoordinate < Fields.GetLength(0))
            {
                if ((direction == Direction.Down || direction == Direction.Up) && IsFree(newCoordinate, j))
                {
                    Fields[i, j].Player = GameFieldPlayer.None;
                    Fields[newCoordinate, j].Player = player;

                    steppedToMill = IsInMill(player, newCoordinate, j);
                }
                else
                {
                    Fields[i, j].Player = GameFieldPlayer.None;
                    Fields[i, newCoordinate].Player = player;

                    steppedToMill = IsInMill(player, i, newCoordinate);
                }

                return true;
            }

            return false;
        }

        public bool MovePlayer(GameFieldPlayer player, int iFrom, int jFrom, int iTo, int jTo)
        {
            if (IsFree(iTo, jTo))
            {
                Fields[iFrom, jFrom].Player = GameFieldPlayer.None;
                Fields[iTo, jTo].Player = player;

                return true;
            }

            return false;
        }

        public bool IsFree(int i, int j)
        {
            return IsUseable(i, j) && Fields[i, j].Player == GameFieldPlayer.None;
        }

        public bool IsUseable(int i, int j)
        {
            //Helyes indexelés?
            if (ValidCoordinates(i, j))
            {
                return Fields[i, j].Useable;
            }

            //Kivétel elkerülése
            return false;
        }

        internal bool RemoveOpponent(GameFieldPlayer player, int i, int j)
        {
            GameFieldPlayer opponent = GameFieldPlayer.Red;

            if (player == GameFieldPlayer.Red)
            {
                opponent = GameFieldPlayer.Blue;
            }

            if (IsUseable(i, j) && Fields[i, j].Player == opponent && (AllInMill(player) || !IsInMill(opponent, i, j)))
            {
                Fields[i, j].Player = GameFieldPlayer.None;

                return true;
            }

            return false;
        }

        private bool ValidCoordinates(int i, int j)
        {
            return 0 <= i && i < Fields.GetLength(0) && 0 <= j && j < Fields.GetLength(1);
        }

        public bool HasMill(GameFieldPlayer player)
        {
            foreach (PossibleMill possibleMill in possibleMills)
            {
                if (Fields[possibleMill.Points[0].X, possibleMill.Points[0].Y].Player == player &&
                    Fields[possibleMill.Points[1].X, possibleMill.Points[1].Y].Player == player &&
                    Fields[possibleMill.Points[2].X, possibleMill.Points[2].Y].Player == player)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsInMill(GameFieldPlayer player, int i, int j)
        {
            foreach (PossibleMill possibleMill in possibleMills)
            {
                if (Fields[possibleMill.Points[0].X, possibleMill.Points[0].Y].Player == player &&
                    Fields[possibleMill.Points[1].X, possibleMill.Points[1].Y].Player == player &&
                    Fields[possibleMill.Points[2].X, possibleMill.Points[2].Y].Player == player &&
                    ((possibleMill.Points[0].X == i && possibleMill.Points[0].Y == j) ||
                    (possibleMill.Points[1].X == i && possibleMill.Points[1].Y == j) ||
                    (possibleMill.Points[2].X == i && possibleMill.Points[2].Y == j)
                    ))
                {
                    return true;
                }
            }

            return false;
        }

        public bool AllInMill(GameFieldPlayer player)
        {
            for (int i = 0; i < Fields.GetLength(0); i++)
            {
                for (int j = 0; j < Fields.GetLength(1); j++)
                {
                    if (!IsInMill(player, i, j))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool PossibleToMakeMill(GameFieldPlayer player)
        {
            foreach (PossibleMill possibleMill in possibleMills)
            {
                if (!(Fields[possibleMill.Points[0].X, possibleMill.Points[0].Y].Player == player && Fields[possibleMill.Points[1].X, possibleMill.Points[1].Y].Player == player && Fields[possibleMill.Points[2].X, possibleMill.Points[2].Y].Player == player))
                {
                    if ((Fields[possibleMill.Points[0].X, possibleMill.Points[0].Y].Player == player && Fields[possibleMill.Points[1].X, possibleMill.Points[1].Y].Player == player) ||
                    (Fields[possibleMill.Points[0].X, possibleMill.Points[0].Y].Player == player && Fields[possibleMill.Points[2].X, possibleMill.Points[2].Y].Player == player) ||
                    (Fields[possibleMill.Points[1].X, possibleMill.Points[1].Y].Player == player && Fields[possibleMill.Points[2].X, possibleMill.Points[2].Y].Player == player))
                    {
                        return true;
                    }
                }
                
            }

            return false;
        }

        public void MoveCPU(GameFieldPlayer player)
        {
            bool done = false;

            while (!done)
            {
                int i = rnd.Next(0, Fields.GetLength(0));
                int j = rnd.Next(0, Fields.GetLength(1));

                while (Fields[i, j].Player != player)
                {
                    i = rnd.Next(0, Fields.GetLength(0));
                    j = rnd.Next(0, Fields.GetLength(1));
                }

                Direction direction;
                int attemptions = 0;

                do
                {
                    direction = (Direction)attemptions++;
                } while (!MovePlayer(player, i, j, direction) && attemptions < 5);

                if (attemptions < 5)
                {
                    done = true;
                }
            }
        }

        public void RemoveOpponentCPU()
        {
            GameFieldPlayer cpu = GameFieldPlayer.Red;
            GameFieldPlayer opponent = GameFieldPlayer.Blue;

            int i = rnd.Next(0, Fields.GetLength(0));
            int j = rnd.Next(0, Fields.GetLength(1));

            while (Fields[i, j].Player != opponent && (!IsInMill(opponent, i, j) || AllInMill(opponent)))
            {
                i = rnd.Next(0, Fields.GetLength(0));
                j = rnd.Next(0, Fields.GetLength(1));
            }

            RemoveOpponent(cpu, i, j);
        }
    }
}
