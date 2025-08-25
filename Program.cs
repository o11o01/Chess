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
            Black.NextMove(test);
            Black.Calculate(test);
            Console.WriteLine("Hello World");
        }
    }
}
