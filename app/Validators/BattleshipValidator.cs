using BattleshipApi.Models;
using FluentValidation;

namespace BattleshipApi.Validators
{
    public class BattleshipValidator : AbstractValidator<Battleship>
    {
        public BattleshipValidator() {
            RuleFor(battleship => battleship.RowStart).GreaterThanOrEqualTo(0);
            RuleFor(battleship => battleship.ColStart).GreaterThanOrEqualTo(0);
            RuleFor(battleship => battleship.RowEnd).GreaterThanOrEqualTo(battleship => Math.Max(0, battleship.RowStart));
            RuleFor(battleship => battleship.ColEnd).GreaterThanOrEqualTo(battleship => Math.Max(0, battleship.ColStart));
            RuleFor(battleship => battleship)
                .Must(battleship => 
                    battleship.RowStart == battleship.RowEnd 
                    || battleship.ColStart == battleship.ColEnd
                )
                .WithMessage("Battleship must be of dimension 1 x n (along one row or along one column only)");
        }
    }
}
