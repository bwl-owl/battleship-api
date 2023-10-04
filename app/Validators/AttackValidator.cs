using BattleshipApi.Models;
using FluentValidation;

namespace BattleshipApi.Validators
{
    public class AttackValidator : AbstractValidator<Attack>
    {
        public AttackValidator() {
            RuleFor(attack => attack.Row).GreaterThanOrEqualTo(0);
            RuleFor(attack => attack.Col).GreaterThanOrEqualTo(0);
        }
    }
}
