using System.ComponentModel.DataAnnotations;

namespace BattleshipApi.Models;

public class Battleship
{
    public int RowStart { get; init; }

    public int RowEnd { get; init; } 

    public int ColStart { get; init; } 

    public int ColEnd { get; init; } 
}
