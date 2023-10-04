using System.ComponentModel.DataAnnotations;

namespace BattleshipApi.Models;

public class Attack
{
    public int Row { get; init; }

    public int Col { get; init; }
}
