using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Display
    {
        public int boardWidth { get; private set; }
        public int boardHeight { get; private set; }


        public Display(int boardWidth, int boardHeight)
        {
            this.boardWidth = boardWidth;
            this.boardHeight = boardHeight;
        }

        private void DrawGameBoard()
        {
            Console.SetCursorPosition(0, 0);
            Console.Write(new string('-', boardWidth + 2));

            Console.SetCursorPosition(0, boardHeight + 1);
            Console.Write(new string('-', boardWidth + 2));

            for(int i = 1; i <= boardHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.WriteLine("|");

                Console.SetCursorPosition(boardWidth + 1, i);
                Console.WriteLine("|");
            }
        }

        /*private void RenderPieceRows(List<char[]> pieceRows)
        {
            for (int i = 0; i < pieceRows.Count; i++)
            {
                for(int j = 0; j < pieceRows[i].Length; j++)
                {
                    Console.SetCursorPosition(j, boardHeight - i);

                }
            }
        }

        private void RenderCurrentPiece(Piece currentPiece)
        {
            string[] pieceStrArr = currentPiece.ToStringArray();
            for (int i = 0; i < pieceStrArr.Length; i++)
            {
                Console.SetCursorPosition(currentPiece.X + 1, currentPiece.Y + i + 1);
                Console.WriteLine(pieceStrArr[i]);
            }
        }*/

        public void Render(Piece currentPiece, List<char[]> pieceRows)
        {
            Console.Clear();

            for(int y = 0; y < boardHeight; y++)
            {
                int reverseY = boardHeight - y - 1;
                Console.SetCursorPosition(1, boardHeight - y);
                for (int x = 0; x < boardWidth; x++)
                {
                    bool iteratorInPiece = currentPiece != null && x >= currentPiece.X && x <= currentPiece.X + currentPiece.GetRealPieceWidth() - 1 && reverseY >= currentPiece.Y && reverseY <= currentPiece.Y + currentPiece.GetRealPieceHeight() - 1;
                    bool iteratorInPieceActiveChar = iteratorInPiece && currentPiece.ToCharMatrix()[reverseY - currentPiece.Y][x - currentPiece.X] == '*';
                    if (iteratorInPieceActiveChar)
                    {
                        Console.Write(currentPiece.ToCharMatrix()[reverseY - currentPiece.Y][x - currentPiece.X]);
                    }
                    else if(y < pieceRows.Count)
                    {
                        Console.Write(pieceRows[y][x]);
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
            }
            
            DrawGameBoard();
        }
    }
}
