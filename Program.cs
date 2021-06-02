using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tetris
{
    class Program
    {
        static Display display;
        static List<char[]> pieceRows; //LinkedList would be much better here, but it doesn't allow using indexers, which I actually need a lot!

        static Piece currentPiece/*, nextPiece*/;

        static int speed = 1000;


        static void Main(string[] args)
        {
            display = new Display(8, 8);

            pieceRows = new List<char[]>(display.boardHeight);

            checkStr = new string('*', display.boardWidth);

            Stopwatch controlTime = new Stopwatch();
            while (true)
            {
                if (currentPiece == null)
                {
                    currentPiece = new Piece();
                    display.Render(currentPiece, pieceRows);
                }
                
                if(IsPieceCollidingWithOthers(0, 0) && currentPiece.Y == 0)
                {
                    GameOver();
                    continue;
                }

                controlTime.Start();
                while(controlTime.ElapsedMilliseconds < speed)
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKey key = Console.ReadKey(true).Key;
                        

                        if(key == ConsoleKey.A && !IsPieceCollidingWithOthersOrBorders(-1, 0))
                        {
                            currentPiece.MovePiece(Piece.PieceMovement.LEFT);
                            display.Render(currentPiece, pieceRows);
                        }
                        else if(key == ConsoleKey.D && !IsPieceCollidingWithOthersOrBorders(1, 0))
                        {
                            currentPiece.MovePiece(Piece.PieceMovement.RIGHT);
                            display.Render(currentPiece, pieceRows);
                        }
                        else if(key == ConsoleKey.S)
                        {
                            StickPieceDown();
                            break;
                        }
                        else if(key == ConsoleKey.W)
                        {
                            currentPiece.Rotate(true);

                            while (isPieceCollidingWithLeftBorder(0))
                            {
                                currentPiece.MovePiece(Piece.PieceMovement.RIGHT);
                            }

                            while (isPieceCollidingWithRightBorder(0))
                            {
                                currentPiece.MovePiece(Piece.PieceMovement.LEFT);
                            }

                            while (isPieceCollidingWithBottom(0))
                            {
                                currentPiece.MovePiece(Piece.PieceMovement.UP);
                            }


                            if (IsPieceCollidingWithOthers(0, 0))
                            {
                                currentPiece.Rotate(false);
                                continue;
                            }

                            display.Render(currentPiece, pieceRows);
                        }
                    }
                }
                controlTime.Reset();

                if (currentPiece == null)
                    continue;

                if (IsPieceCollidingWithOthersOrBorders(0, 1))
                {
                    StickPieceDown();
                }
                else
                {
                    currentPiece.MovePiece(Piece.PieceMovement.DOWN);
                    display.Render(currentPiece, pieceRows);
                }
            }
        }

        static void GameOver()
        {
            if(Console.KeyAvailable)
                Console.ReadKey(true); //To prevent from unwanted restarts

            string gameOver = "Game Over!";
            Console.SetCursorPosition(display.boardWidth / 2 + 1 - gameOver.Length / 2, display.boardHeight / 2 + 1);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(gameOver);

            Console.ReadKey(true);

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;


            currentPiece = null;
            pieceRows.Clear();
        }

        static bool isPieceCollidingWithBottom(int yOffset)
        {
            int pieceOffY = currentPiece.Y + yOffset;
            return pieceOffY + currentPiece.GetRealPieceHeight() - 1 >= display.boardHeight;
        }

        static bool isPieceCollidingWithLeftBorder(int xOffset)
        {
            int pieceOffX = currentPiece.X + xOffset;
            return pieceOffX < 0;
        }

        static bool isPieceCollidingWithRightBorder(int xOffset)
        {
            int pieceOffX = currentPiece.X + xOffset;
            return pieceOffX + currentPiece.GetRealPieceWidth() - 1 >= display.boardWidth;
        }

        static bool IsPieceCollidingWithOthers(int xOffset, int yOffset)
        {
            char[][] pieceMatrix = currentPiece.ToCharMatrix();
            /*if (currentPiece.GetRealPieceHeight() + pieceOffY < pieceRows.Count)
                return false;*/

            int pieceOffY = currentPiece.Y + yOffset;
            int pieceOffX = currentPiece.X + xOffset;
            for (int y = 0; y < pieceMatrix.Length; y++)
            {
                int piecePartY = pieceOffY + y;
                for (int x = 0; x < pieceMatrix[y].Length; x++)
                {
                    if (pieceMatrix[y][x] != '*')
                        continue;

                    int piecePartX = pieceOffX + x;


                    if (display.boardHeight - piecePartY <= pieceRows.Count && (pieceRows.Count != 0 && pieceRows[display.boardHeight - piecePartY - 1][piecePartX] == '*' || pieceRows.Count == 0))
                        return true;
                }
            }

            return false;
        }

        static bool IsPieceCollidingWithOthersOrBorders(int xOffset, int yOffset)
        {
            return isPieceCollidingWithBottom(yOffset) || isPieceCollidingWithLeftBorder(xOffset) || isPieceCollidingWithRightBorder(xOffset) || IsPieceCollidingWithOthers(xOffset, yOffset);
        }

        static void StickPieceDown()
        {
            while(!IsPieceCollidingWithOthersOrBorders(0, 1))
            {
                currentPiece.MovePiece(Piece.PieceMovement.DOWN);
            }


            for(int i = display.boardHeight - currentPiece.Y - pieceRows.Count; i >= 0; i--)
            {
                AddRow();
            }

            char[][] tempMx = currentPiece.ToCharMatrix();
            for (int y = 0; y < tempMx.Length; y++)
            {
                int piecePartY = currentPiece.Y + y;
                for(int x = 0; x < tempMx[y].Length; x++)
                {
                    if (tempMx[y][x] == ' ')
                        continue;

                    int piecePartX = currentPiece.X + x;
                    pieceRows[display.boardHeight - (piecePartY + 1)][piecePartX] = tempMx[y][x];
                }
            }

            currentPiece = null;

            RemoveFilledRows();
        }

        static string checkStr;
        static void RemoveFilledRows()
        {
            for(int y = 0; y < pieceRows.Count; y++)
            {
                if(new string(pieceRows[y]) == checkStr)
                {
                    pieceRows.RemoveAt(y);
                    y--;
                }
            }

            display.Render(currentPiece, pieceRows);
            Thread.Sleep(speed);
        }

        static void AddRow()
        {
            pieceRows.Add(new char[display.boardWidth]);
            for(int i = 0; i < display.boardWidth; i++)
            {
                pieceRows.Last()[i] = ' ';
            }
        }
    }
}
