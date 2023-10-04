using BattleshipApi.Models;
using BattleshipApi.Validators;
using FluentValidation.TestHelper;

namespace BattleshipApiUnitTests.Validators;

public class BattleshipValidatorUnitTests
{
    private readonly BattleshipValidator _sut = new();

    [Theory]
    [InlineData(0, 0, 0, 0)]
    [InlineData(0, 0, 1, 1)]
    [InlineData(0, 1, 0, 0)]
    [InlineData(1, 5, 2, 2)]
    [InlineData(8, 8, 0, 10)]
    [InlineData(3, 3, 3, 3)]
    [InlineData(7, 8, 8, 8)]
    [InlineData(0, 9, 0, 0)]
    [InlineData(4, 4, 0, 9)]
    public void Battleship_Validator_Should_Not_Have_Errors_For_Valid_Battleships(int rowStart, int rowEnd, int colStart, int colEnd)
    {
        var testBattleship = new Battleship() { 
            RowStart = rowStart, RowEnd = rowEnd, ColStart = colStart, ColEnd = colEnd 
        };

        var result = _sut.TestValidate(testBattleship);

        result.ShouldNotHaveAnyValidationErrors();
    }


    // TODO: add assertions for messages
    [Theory]
    [InlineData(-1, 0, 0, 0, "RowStart")]
    [InlineData(-1, 0, 1, 1, "RowStart")]
    [InlineData(0, -1, 2, 2, "RowEnd")]
    [InlineData(2, 1, 0, 0, "RowEnd")]
    [InlineData(0, 0, -1, 1, "ColStart")]
    [InlineData(0, 0, 2, 1, "ColEnd")]
    public void Battleship_Validator_Should_Have_Errors_For_Battleships_With_One_Invalid_Property(int rowStart, int rowEnd, int colStart, int colEnd, string invalidProperty)
    {
        var testBattleship = new Battleship()
        {
            RowStart = rowStart,
            RowEnd = rowEnd,
            ColStart = colStart,
            ColEnd = colEnd
        };

        var result = _sut.TestValidate(testBattleship);

        result.ShouldHaveValidationErrorFor(invalidProperty).Only();
    }

    [Theory]
    [InlineData(-1, 0, -1, -1, "RowStart,ColStart,ColEnd")]
    [InlineData(-1, -1, 2, 1, "RowStart,RowEnd,ColEnd")]
    [InlineData(0, 0, -1, -2, "ColStart,ColEnd")]
    [InlineData(2, 1, -2, -2, "RowEnd,ColStart,ColEnd")]
    public void Battleship_Validator_Should_Have_Errors_For_Battleships_With_Multiple_Invalid_Properties(int rowStart, int rowEnd, int colStart, int colEnd, string expectedInvalidPropertiesString)
    {
        var testBattleship = new Battleship()
        {
            RowStart = rowStart,
            RowEnd = rowEnd,
            ColStart = colStart,
            ColEnd = colEnd
        };

        var result = _sut.TestValidate(testBattleship);

        var expectedInvalidProperties = expectedInvalidPropertiesString.Split(",");
        Assert.Equal(expectedInvalidProperties.Length, result.Errors.Count);
        foreach (var property in expectedInvalidProperties)
        {
            result.ShouldHaveValidationErrorFor(property);
        }
    }

    [Theory]
    [InlineData(0, 1, 0, 1)]
    [InlineData(2, 3, 5, 6)]
    [InlineData(0, -1, 1, 2)]
    public void Battleship_Validator_Should_Have_Errors_For_Battleships_Spanning_More_Than_One_Dimension(int rowStart, int rowEnd, int colStart, int colEnd)
    {
        var testBattleship = new Battleship()
        {
            RowStart = rowStart,
            RowEnd = rowEnd,
            ColStart = colStart,
            ColEnd = colEnd
        };

        var result = _sut.TestValidate(testBattleship);

        Assert.True(result.Errors.Exists(error => error.ErrorMessage == "Battleship must be of dimension 1 x n (along one row or along one column only)"));
    }
}