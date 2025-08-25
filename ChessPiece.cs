using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Chess
{
    public enum PieceType
    {
        empty,
        pawn,
        rook,
        knight,
        bishop,
        queen,
        king,
    }
    public class ChessPiece
    {

        private PieceType pieceType;
        public PieceType PieceType
        { 
            get { return pieceType; }
            set { pieceType = value; }
        }
        private bool isWhite;
        public bool IsWhite
        {
            get { return isWhite; }
            set { isWhite = value; }
        }
        public bool FirstMove
        { get; set; }
        public List<int[]> MovesList;
        
        public ChessPiece(PieceType type)
        {
            PieceType = type;
            IsWhite = false;
            FirstMove = true; 
        }
        public ChessPiece()
        {
            PieceType = PieceType.empty;
            IsWhite = false;
        }
    }
    public class ChessBoard
    {
        public ChessPiece[,] board = new ChessPiece[8,8];
        public int BlackScore
        { get; set; }
        public int WhiteScore
        { get; set; }
        internal bool Move(int pieceRow, int pieceColumn, int newRow, int newColumn)
        {
            bool validMove = false;
            for (int i = 0; i < board[pieceRow, pieceColumn].MovesList.Count; i++)
            {
                List<int[]> moveList = board[pieceRow, pieceColumn].MovesList;
                if (Contains.Arrays(moveList[i], new int[] {newRow,newColumn}))
                {
                    validMove = true; 
                }
            }
            if (validMove)
            {
                board[pieceRow, pieceColumn].FirstMove = false;
                board[newRow, newColumn] = board[pieceRow, pieceColumn];
                board[pieceRow, pieceColumn] = new ChessPiece();
            }
            return false;
        }
        internal void RecordMoves()
        {
            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int column = 0; column < board.GetLength(1); column++)
                {
                    ChessPiece focus = board[row, column];
                    focus.MovesList = new List<int[]>();
                    List<int[]> movesEmpty = new List<int[]>();
                    AddScore(focus);
                    switch (focus.PieceType)
                    {
                        case PieceType.pawn:
                            if (focus.IsWhite)
                            {
                                movesEmpty.Add(new int[] { row - 1, column });
                                if (focus.FirstMove) movesEmpty.Add(new int[] { row - 2, column });
                                focus.MovesList.AddRange(Check(movesEmpty, focus.IsWhite));
                                movesEmpty.Clear();
                                movesEmpty.Add(new int[] { row - 1, column - 1 });
                                movesEmpty.Add(new int[] { row - 1, column + 1 });
                                focus.MovesList.AddRange(CheckPawn(movesEmpty, focus.IsWhite));
                                
                            }
                            else
                            {
                                movesEmpty.Add(new int[] { row + 1, column });
                                if (focus.FirstMove) movesEmpty.Add(new int[] { row + 2, column });
                                focus.MovesList.AddRange(Check(movesEmpty, focus.IsWhite));
                                movesEmpty.Clear();
                                movesEmpty.Add(new int[] { row + 1, column - 1 });
                                movesEmpty.Add(new int[] { row + 1, column + 1 });
                                focus.MovesList.AddRange(CheckPawn(movesEmpty, focus.IsWhite));

                            }
                            break;
                        case PieceType.knight:
                            movesEmpty.Add(new int[] { row - 2, column - 1 });
                            movesEmpty.Add(new int[] { row - 2, column + 1 });
                            movesEmpty.Add(new int[] { row + 2, column - 1 });
                            movesEmpty.Add(new int[] { row + 2, column + 1 });
                            movesEmpty.Add(new int[] { row - 1, column - 2 });
                            movesEmpty.Add(new int[] { row - 1, column + 2 });
                            movesEmpty.Add(new int[] { row + 1, column - 2 });
                            movesEmpty.Add(new int[] { row + 1, column + 2 });
                            focus.MovesList.AddRange(CheckKnight(movesEmpty, focus.IsWhite));
                            break;
                        case PieceType.bishop:
                            
                            for(int rowCheck = row, columnCheck = column; rowCheck < 8 && columnCheck < 8; rowCheck++)
                            {
                                if (rowCheck != row && columnCheck != column)
                                { 
                                    movesEmpty.Add(new int[] { rowCheck, columnCheck });
                                }
                                columnCheck++;
                            }
                            for (int rowCheck = row, columnCheck = column; rowCheck > -1 && columnCheck < 8; rowCheck--)
                            {
                                if (rowCheck != row && columnCheck != column)
                                {
                                    movesEmpty.Add(new int[] { rowCheck, columnCheck });
                                }
                                columnCheck++;
                            }
                            for (int rowCheck = row, columnCheck = column; rowCheck < 8 && columnCheck > -1; rowCheck++)
                            {
                                if (rowCheck != row && columnCheck != column)
                                {
                                    movesEmpty.Add(new int[] { rowCheck, columnCheck });
                                }
                                columnCheck--;
                            }
                            for (int rowCheck = row, columnCheck = column; rowCheck > -1 && columnCheck > -1; rowCheck--)
                            {
                                if (rowCheck != row && columnCheck != column)
                                {
                                    movesEmpty.Add(new int[] { rowCheck, columnCheck });
                                }
                                columnCheck--;
                            }
                            focus.MovesList.AddRange(Check(movesEmpty, focus.IsWhite));
                            break;
                        case PieceType.rook:
                            for(int rowCheck = row; rowCheck < 8; rowCheck++)
                            {
                                if (rowCheck != row)
                                {
                                    movesEmpty.Add(new int[] { rowCheck, column });
                                }
                            }
                            focus.MovesList.AddRange(Check(movesEmpty, focus.IsWhite));
                            movesEmpty.Clear();
                            for (int rowCheck = row; rowCheck > -1; rowCheck--)
                            {
                                if (rowCheck != row)
                                {
                                    movesEmpty.Add(new int[] { rowCheck, column });
                                }
                            }
                            focus.MovesList.AddRange(Check(movesEmpty, focus.IsWhite));
                            movesEmpty.Clear();
                            for (int columnCheck = column; columnCheck < 8; columnCheck++)
                            {
                                if (columnCheck != column)
                                {
                                    movesEmpty.Add(new int[] { row, columnCheck });
                                }
                            }
                            focus.MovesList.AddRange(Check(movesEmpty, focus.IsWhite));
                            movesEmpty.Clear();
                            for (int columnCheck = column; columnCheck > -1; columnCheck--)
                            {
                                if (columnCheck != column)
                                {
                                    movesEmpty.Add(new int[] { row, columnCheck });
                                }
                            }
                            focus.MovesList.AddRange(Check(movesEmpty, focus.IsWhite));
                            movesEmpty.Clear();
                            break;
                        case PieceType.queen:
                            for (int rowCheck = row; rowCheck < 8; rowCheck++)
                            {
                                movesEmpty.Add(new int[] { rowCheck, column });
                            }
                            for (int rowCheck = row; rowCheck > -1; rowCheck--)
                            {
                                movesEmpty.Add(new int[] { rowCheck, column });
                            }
                            for (int columnCheck = column; columnCheck < 8; columnCheck++)
                            {
                                movesEmpty.Add(new int[] { row, columnCheck });
                            }
                            for (int columnCheck = column; columnCheck > -1; columnCheck--)
                            {
                                movesEmpty.Add(new int[] { row, columnCheck });
                            }
                            goto case PieceType.bishop;
                        case PieceType.king:

                            break;
                    }
                    movesEmpty.Clear();
                }
            }
        }
        internal void AddScore(ChessPiece input)
        {
            int value = 0;
            switch (input.PieceType)
            {
                case PieceType.pawn:
                    value = 1;
                    break;
                case PieceType.knight:
                    value = 3;
                    break;
                case PieceType.bishop:
                    value = 3;
                    break;
                case PieceType.rook:
                    value = 5;
                    break;
                case PieceType.queen:
                    value = 9;
                    break;
            }
            if (input.IsWhite)
            {
                WhiteScore += value;
            }
            else
            {
                BlackScore += value;
            }
        }
        internal List<int[]> Check(List<int[]> check, bool isWhite)
        {
            for(int i = 0; i < check.Count; i++)
            {
                int row = check[i][0];
                int column = check[i][1];
                if (row < 8 && row > -1 && column < 8 && column > -1)
                {
                    if (board[row, column].PieceType != PieceType.empty)
                    {
                        if (board[row, column].IsWhite == isWhite)
                        { 
                            check.RemoveRange(i, check.Count - i);
                            return check;
                        } 
                        else
                        {
                            check.RemoveRange(i + 1, check.Count - i -1);
                            return check;
                        }
                    }
                }
                else
                {
                    check.RemoveAt(i);
                    i--;
                }
            }
            return check;
        }
        internal List<int[]> CheckKnight(List<int[]> check, bool isWhite)
        {
            for (int i = 0; i < check.Count;)
            {
                int row = check[i][0];
                int column = check[i][1];
            
                if (row < 8 && row > -1 && column < 8 && column > -1)
                {
                    if (board[row, column].PieceType != PieceType.empty)
                    {
                        if (board[row, column].IsWhite == isWhite)
                        {
                            check.RemoveAt(i);
                            continue;
                        }
                    }
                }
                else
                {
                    check.RemoveAt(i);
                    continue;
                }
                i++;
            }
            return check;
        }
        internal List<int[]> CheckPawn(List<int[]>check, bool isWhite)
        {
            for (int i = 0; i < check.Count;)
            {
                int row = check[i][0];
                int column = check[i][1];
                if (row < 8 && row > -1 && column < 8 && column > -1)
                {
                    if (board[row, column].PieceType == PieceType.empty)
                    {
                        check.RemoveAt(i);
                        continue;
                    }
                    if (board[row, column].IsWhite == isWhite)
                    {
                        check.RemoveAt(i);
                        continue;
                    }
                    
                }
                else
                {
                    check.RemoveAt(i);
                    continue;
                }
                i++;
            }
            return check;
        }
        internal void Display()
        {
            Console.Clear();
            Console.WriteLine("\x1b[3J");
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for(int j = 0; j < board.GetLength(0); j++)
                {
                    string printString = "";
                    if (board[i, j].PieceType != PieceType.empty)
                    {
                        printString = (board[i, j].PieceType.ToString()).PadLeft(5).PadRight(6);

                     
                    }
                    else
                    {
                        printString = "      ";
                    }
                    Console.Write($"|  {printString}  |");
                }
                for (int i2 = 0, max = 3; i2 < max; i2++)
                {
                    if (i2 == 0)
                    {
                        Console.WriteLine();
                    }
                    for (int k = 0; k < 8; k++)
                    {
                        string newLine = "";
                        if (i2 == (max / 2))
                        {
                            newLine = "|__________|";
                        }
                        else
                        {
                            newLine = "|          |";
                        }
                            
                        Console.Write($"{newLine}");
                    }
                    Console.WriteLine();
                    
                }
            }
        }
        internal void PlacePieces(ChessPiece[,] board)
        {
            for(int row = 0; row < board.GetLength(1); row++)
            {
                for (int Column = 0; Column < board.GetLength(0); Column++)
                {
                    PieceType type = 0;
                    switch (row)
                    {
                        case 0:
                            switch(Column)
                            {
                                case 0:
                                    type = PieceType.rook;
                                    break;
                                case 1:
                                    type = PieceType.knight;
                                    break;
                                case 2:
                                    type = PieceType.bishop;
                                    break;
                                case 3:
                                    type = PieceType.queen;
                                    break;
                                case 4:
                                    type = PieceType.king;
                                    break;
                                case 5:
                                    goto case 2;
                                case 6:
                                    goto case 1;
                                case 7:
                                    goto case 0;
                            }
                            break;
                        case 1:
                            type = PieceType.pawn;
                            break;
                        case 6:
                            goto case 1;
                        case 7:
                            goto case 0;

                    }
                    
                    board[row, Column] = new ChessPiece(type);
                    if(row > 3)
                    {
                        board[row, Column].IsWhite = true;
                    }
                }
            }
        }
    }

}
