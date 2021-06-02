using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Piece
    {

        enum PieceRotation
        {
            UP,
            RIGHT,
            DOWN,
            LEFT
        }
        private static readonly int enumLength = Enum.GetValues(typeof(PieceRotation)).Length;

        public enum PieceMovement
        {
            UP,
            DOWN,
            LEFT,
            RIGHT
        }


        private static readonly char[][][] pieceShapes = new char[][][] { new char[][] {
                                                                                 new char[] { '*', ' ', ' ' }, 
                                                                                 new char[] { '*', '*', '*' },
                                                                                 new char[] { '-', '-', '-' }},
                                                               new char[][] { 
                                                                                 new char[] { ' ', '*', ' ' }, 
                                                                                 new char[] { '*', '*', '*' },
                                                                                 new char[] { '-', '-', '-' }},
                                                               new char[][] {
                                                                                 new char[] { '*', '-' }, 
                                                                                 new char[] { '*', '-' } }, 
                                                               new char[][] { 
                                                                                 new char[] { '*', '-', '-' },
                                                                                 new char[] { '*', '-', '-' },
                                                                                 new char[] { '*', '-', '-' }}, 
                                                               new char[][] {
                                                                                 new char[] { ' ', '*', '-' },
                                                                                 new char[] { '*', '*', '-' },
                                                                                 new char[] { '*', ' ', '-' } }, 
                                                               new char[][] {
                                                                                 new char[] { '*', '-', '-', '-' },
                                                                                 new char[] { '*', '-', '-', '-' },
                                                                                 new char[] { '*', '-', '-', '-' },
                                                                                 new char[] { '*', '-', '-', '-' }} }; //Each 'new char[]' means new row. All piece matrices need to be the same length on both sides to prevent from errors
        private char[][] pieceShape { get; set; } //The char matrix representing the shape of this piece

        public int X { get; set; }
        public int Y { get; set; }

        private PieceRotation rotation;


        public Piece()
        {
            X = 0;
            Y = 0;

            pieceShape = pieceShapes[new Random().Next(pieceShapes.GetLength(0))];
            rotation = 0;
        }

        private string tempStr; //I created tempStr and tempMx to optimize the conversion process a bit
        public override string ToString()
        {
            if (tempStr != null)
                return tempStr;

            StringBuilder sb = new StringBuilder();
            switch (rotation)
            {
                case PieceRotation.UP: 
                    
                    for(int i = 0; i < pieceShape.GetLength(0); i++)
                    {
                        for(int j = 0; j < pieceShape[i].GetLength(0); j++)
                        {
                            sb.Append(pieceShape[i][j]);
                        }

                        if(i < pieceShape.GetLength(0) - 1)
                            sb.Append("\n");
                    }

                    break;

                case PieceRotation.DOWN:

                    for (int i = pieceShape.GetLength(0) - 1; i >= 0; i--)
                    {
                        for (int j = pieceShape[i].GetLength(0) - 1; j >= 0; j--)
                        {
                            sb.Append(pieceShape[i][j]);
                        }

                        if (i > 0)
                            sb.Append("\n");
                    }

                    break;

                case PieceRotation.LEFT:

                    for (int i = 0; i < pieceShape.GetLength(0); i++)
                    {
                        for (int j = 0; j < pieceShape[i].GetLength(0); j++)
                        {
                            sb.Append(pieceShape[j][i]);
                        }

                        if (i < pieceShape[i].GetLength(0) - 1)
                            sb.Append("\n");
                    }

                    break;
                
                case PieceRotation.RIGHT:

                    for (int i = pieceShape.GetLength(0) - 1; i >= 0; i--)
                    {
                        for (int j = pieceShape[i].GetLength(0) - 1; j >= 0; j--)
                        {
                            sb.Append(pieceShape[j][i]);
                        }

                        if (i > 0)
                            sb.Append("\n");
                    }

                    break;
            }

            sb.Replace("-", "");

            while(sb[0] == '\n')
            {
                sb.Remove(0, 1);
            }

            while (sb[sb.Length - 1] == '\n')
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return tempStr = sb.ToString();
        }

        public string[] ToStringArray()
        {
            return ToString().Split('\n');
        }

        private char[][] tempMx;
        public char[][] ToCharMatrix()
        {
            if (tempMx != null)
                return tempMx;


            string renderStr = ToString();
            char[][] matrix = renderStr.Split('\n').Select(x => x.ToCharArray()).ToArray();
            tempMx = matrix;

            return matrix;
        }

        public void Rotate(bool right)
        {
            if (right)
            {
                if ((int)++rotation >= enumLength)
                    rotation = 0;
            }
            else
            {
                if ((int)--rotation < 0)
                    rotation = (PieceRotation)enumLength;
            }

            tempStr = null;
            tempMx = null;
        }

        public void MovePiece(PieceMovement direction)
        {
            switch (direction)
            {
                case PieceMovement.UP: Y--; break; //UP on the screen
                case PieceMovement.DOWN: Y++; break; //DOWN on the screen
                case PieceMovement.LEFT: X--; break;
                case PieceMovement.RIGHT: X++; break;
            }
        }

        public int GetRealPieceHeight()
        {
            int height = 1;
            for (int y = 0; y < ToCharMatrix().Length; y++)
            {
                for (int x = 0; x < ToCharMatrix()[y].Length; x++)
                {
                    if (ToCharMatrix()[y][x] == '*' && y + 1 > height)
                    {
                        height = y + 1;
                    }
                }
            }

            return height;
        }

        public int GetRealPieceWidth()
        {
            int width = 1;
            for (int y = 0; y < ToCharMatrix().Length; y++)
            {
                for (int x = 0; x < ToCharMatrix()[y].Length; x++)
                {
                    if (ToCharMatrix()[y][x] == '*' && x + 1 > width)
                    {
                        width = x + 1;
                    }
                }
            }

            return width;
        }

    }
}
