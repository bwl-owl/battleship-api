namespace BattleshipApi.Models;

public class GameBoard
{
    private const int DEFAULT_BOARD_SIZE = 10;

    public TileState[,] Board { get; init; } 

    public int BoardId { get; init; }

    // TODO: add player
    // public Player Player { get; init; }

    public GameBoard(int boardId, int boardRowSize = DEFAULT_BOARD_SIZE, int boardColumnSize = DEFAULT_BOARD_SIZE) {
        BoardId = boardId;
        Board = new TileState[boardRowSize, boardColumnSize];
    }

    public TileState[,] AddBattleShip(Battleship ship) {
        if (ship.RowStart < 0 || ship.ColStart < 0 || ship.RowEnd >= Board.GetLength(0) || ship.ColEnd >= Board.GetLength(1)) {
            throw new ArgumentException($"Error adding battleship: row {ship.RowEnd} and col {ship.ColEnd} out of bounds for board {BoardId}");
        }
        for (var i = ship.RowStart; i <= ship.RowEnd; i++) {
            for (var j = ship.ColStart; j <= ship.ColEnd; j++) {
                if (Board[i,j] != TileState.EMPTY) {
                    throw new ArgumentException($"Error adding battleship: existing battleship already occupying board {BoardId} at row {i} colum {j}");
                }
                Board[i,j] = TileState.UNSUNK;
            }
        }
        return Board;
    }

    public AttackResult Attack(Attack attack) {
        if (attack.Row >= Board.GetLength(0) || attack.Col >= Board.GetLength(1)) {
            throw new ArgumentException($"Error attempting attack: row {attack.Row} and col{attack.Col} out of bounds for board {BoardId}");
        }
        if (Board[attack.Row, attack.Col] == TileState.SUNK) { 
            throw new ArgumentException($"Error attempting attack: row {attack.Row} and col {attack.Col} on {BoardId} already contains a sunk battleship");
        }
        if (Board[attack.Row, attack.Col] == TileState.UNSUNK) { 
            Board[attack.Row, attack.Col] = TileState.SUNK;
            return AttackResult.HIT;
        }
        return AttackResult.MISS;
    }
}
