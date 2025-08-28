using static System.Net.Mime.MediaTypeNames;

namespace Chess
{
    internal class Program
    {
        static void Main(string[] args)
        {

            ChessBoard Board = new ChessBoard();
            Board.PlacePieces(Board.board);
            Board.Display();
            Board.RecordMoves();
            ChessEngine Black = new ChessEngine();
            Node test = new Node(Board);
            //Black.Calculate(test);
            //Black.Trim(test);
            //Black.depth += 3;
            //Black.ReCalculate(test);
            //Console.WriteLine();


            while (true)
            {
                Board.RecordMoves();
                string ChosenPiece = Console.ReadLine();
                
                
                if (Char.ToUpper(ChosenPiece[0]) >= 'A' && Char.ToUpper(ChosenPiece[0]) <= 'H' && ChosenPiece[1] >= '1' && ChosenPiece[1] <= '8')
                {
                    int column = (int)Char.ToUpper(ChosenPiece[0]) - 65;
                    int row = (8 - (ChosenPiece[1] - '0'));
                   // Console.WriteLine(column);
                  //  Console.WriteLine(row);
                    if (Board.board[row, column].IsWhite)
                    {
                        string moveTo = Console.ReadLine();
                        int moveColumn = (int)Char.ToUpper(moveTo[0]) - 65;
                        int moveRow = (8 - (moveTo[1] - '0'));
                        if (Board.Move(row, column, moveRow, moveColumn))
                        {
                            Board.Display();
                            Board.RecordMoves();
                            //Thread.Sleep(500);
                            Board.board = MoveAi(Board);
                            Board.Display();

                        }
                    }
                }
            }

        }
        static ChessPiece[,] MoveAi(ChessBoard Board)
        {
            ChessEngine Black = new ChessEngine();
            Node test = new Node(Board);
            Black.Calculate(test);
            Black.depth += 2;
            Black.ReCalculate(test);
            Black.depth += 2;
            return Black.headBoard.childBoards[0].data.CopyBoard();
        }
    }
}
