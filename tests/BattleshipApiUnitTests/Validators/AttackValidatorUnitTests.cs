using BattleshipApi.Models;
using BattleshipApi.Validators;
using FluentValidation.TestHelper;

namespace BattleshipApiUnitTests.Validators;

public class AttackValidatorUnitTests
{
    private readonly AttackValidator _sut = new();

    [Theory]
    [InlineData(0, 0)]
    [InlineData(5, 5)]
    [InlineData(0, 5)]
    [InlineData(6, 2)]
    [InlineData(9, 9)]
    public void Attack_Validator_Should_Not_Have_Errors_For_Valid_Attacks(int row, int col)
    {
        var testAttack = new Attack()
        {
            Row = row,
            Col = col
        };

        var result = _sut.TestValidate(testAttack);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(-1, 0, "Row")]
    [InlineData(5, -5, "Col")]
    [InlineData(0, -1, "Col")]
    public void Attack_Validator_Should_Have_Errors_For_Attacks_With_One_Invalid_Property(int row, int col, string invalidProperty)
    {
        var testAttack = new Attack()
        {
            Row = row,
            Col = col
        };

        var result = _sut.TestValidate(testAttack);

        result.ShouldHaveValidationErrorFor(invalidProperty).Only();
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(-9, -1)]
    public void Attack_Validator_Should_Have_Errors_For_Attacks_With_Two_Invalid_Properties(int row, int col)
    {
        var testAttack = new Attack()
        {
            Row = row,
            Col = col
        };

        var result = _sut.TestValidate(testAttack);

        result.ShouldHaveValidationErrorFor(testAttack => testAttack.Row);
        result.ShouldHaveValidationErrorFor(testAttack => testAttack.Col);
    }
}