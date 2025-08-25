using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    internal class ChessEngine
    {
        bool isWhite = false; 
        bool isWhiteTemp = false;
        int depth = 5;
        public void NextMove(Node board)
        {
            board.data.RecordMoves();
   
            for (int row = 0; row < board.data.board.GetLength(0); row++)
            {
                for (int column = 0; column < board.data.board.GetLength(1); column++)
                {
                    if (board.data.board[row, column].IsWhite == isWhiteTemp && board.data.board[row, column].PieceType != PieceType.empty)
                    {
                        ChessPiece FocusedPiece = board.data.board[row, column];
                        if (FocusedPiece.MovesList.Count > 0)
                        {
                            for (int i = 0; i < board.data.board[row, column].MovesList.Count; i++)
                            { 

                                    int newRow = FocusedPiece.MovesList[i][0];
                                    int newColumn = FocusedPiece.MovesList[i][1];

                                    board.childBoards.Add(new Node(new ChessBoard()));

                                    Node workingNode = board.childBoards.Last();
                                    workingNode.data = new ChessBoard();
                                    workingNode.data.board = (ChessPiece[,])board.data.board.Clone();
                                    workingNode.data.Move(row, column, newRow, newColumn);
                                   // workingNode.data.Display();
   
                            }
                        }
                    }
                }
            }

        }
        Node headBoard;
        public void Prune()
        {
        }
        public int[,] Calculate(Node input, int currentDepth = 0)
        {
            currentDepth++;
            isWhiteTemp = !isWhiteTemp;
            if (headBoard == null)
            {
                headBoard = input;
            }
            for (int i = 0; i < input.childBoards.Count; i++)
            {
                NextMove(input.childBoards[i]);
                if (currentDepth < depth)
                { 
                    for (int j = 0; j < input.childBoards[i].childBoards.Count; j++)
                    {
                        Calculate(input.childBoards[i], currentDepth);
                    }
                }
            }

            
        }
    }
    public class Node
    {
        public ChessBoard data;
        public  List<Node> childBoards = new List<Node>();

        public Node(ChessBoard Board)
        {
            data = Board; 
        }
    }
}
