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
            //ChessEngine Black = new ChessEngine();
            //Node test = new Node(Board);
            //Black.Calculate(test);    
            //Black.Trim(test);
            //Black.depth += 2;
            //Black.ReCalculate(test);
            //Console.WriteLine();


            while (true)
            {
                string ChosenPiece = Console.ReadLine();
                int column = (int)ChosenPiece[0] - 65;
                int row = (8 - (ChosenPiece[1] - '0'));
                string moveTo = Console.ReadLine();
                int moveColumn = (int)moveTo[0] - 65;
                int moveRow = (8 - (moveTo[1] - '0'));
                Board.Move(row, column, moveRow, moveColumn);
                Board.Display();
                Board.RecordMoves();
                Board.board = MoveAi(Board);
                Board.Display();
            }

        }
        static ChessPiece[,] MoveAi(ChessBoard Board)
        {
            ChessEngine Black = new ChessEngine();
            Node test = new Node(Board);
            Black.Calculate(test);
            Black.Trim(test);
            return Black.headBoard.childBoards[0].data.CopyBoard();
        }
    }
}
