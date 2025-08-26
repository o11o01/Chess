using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Chess
{
    internal class ChessEngine
    {
        bool isWhite = false; 
        bool isWhiteTemp = false;
        public Node headBoard;
        public int depth = 4;
        int width = 3;
        public void NextMove(Node board)
        {
            for (int row = 0; row < board.data.board.GetLength(0); row++)
            {
                for (int column = 0; column < board.data.board.GetLength(1); column++)
                {
                    if (board.data.board[row, column].IsWhite == Convert.ToBoolean(board.Depth % 2) && board.data.board[row, column].PieceType != PieceType.empty)
                    {
                        ChessPiece FocusedPiece = board.data.board[row, column];
                        if (FocusedPiece.MovesList.Count > 0)
                        {
                            for (int i = 0; i < board.data.board[row, column].MovesList.Count; i++)
                            {
                                     
                                int newRow = FocusedPiece.MovesList[i][0];
                                int newColumn = FocusedPiece.MovesList[i][1];

                                board.childBoards.Add(new Node(new ChessBoard()));

                               
                                
                                board.childBoards.Last().data.board = board.data.CopyBoard();
                                if(board.childBoards.Last().data.Move(row, column, newRow, newColumn))
                                {
                                    board.childBoards.Last().Depth = board.Depth + 1;
                                    board.childBoards.Last().data.RecordMoves();
                                   // board.childBoards.Last().Parent = board;
                                    board.childBoards.Last().Score = board.childBoards.Last().data.WhiteScore - board.childBoards.Last().data.BlackScore;
                                    //board.childBoards.Last().data.Display();
                                }
                                else
                                {
                                    board.childBoards.Remove(board.childBoards.Last());
                                }
   
                            }
                        }
                    }
                }
            }

        }
        public void Calculate(Node board)
        {
            
            if(headBoard == null)
            {
                headBoard = board;
                
            }
            NextMove(board);
            for (int i = 0; i < board.childBoards.Count; i++ )
            {
                    if (board.Depth < depth)
                    {
                        Calculate(board.childBoards[i]);
                    }
            }
            Trim(board);
        }
        public void Trim(Node board)
        {
            board.childBoards.Sort();
            if (board.childBoards.Count != 0)
            {
                if (board.Depth % 2 == 1)
                {
                    board.Score = board.childBoards.Last().Score;
                    board.childBoards.RemoveRange(0, board.childBoards.Count - width);
                }
                else
                {
                    board.Score = board.childBoards[0].Score;
                    board.childBoards.RemoveRange(width, board.childBoards.Count - width);
                }
            }
        }
        public void ReCalculate(Node board)
        {
            for(int i = 0; i <board.childBoards.Count; i++)
            {
                ReCalculate(board.childBoards[i]);
            }
            if(board.childBoards.Count == 0)
            {
                Calculate(board);
            }
        }
    }
    public class Node : IComparable<Node>
    {
        public ChessBoard data;
        public Node Parent;
        public  List<Node> childBoards = new List<Node>();
        public int Depth;
        public int Score;
        public Node(ChessBoard Board)
        {
            data = Board; 
        }
        public int CompareTo(Node other)
        {
            return this.Score.CompareTo(other.Score);

        }
    }

}
