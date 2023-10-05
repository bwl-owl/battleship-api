using BattleshipApi.Controllers;
using BattleshipApi.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BattleshipApiUnitTests.Controllers
{
    public class GameControllerUnitTests
    {
        private readonly GameController _sut = new();

        public GameControllerUnitTests()
        {
            GameController.GameBoards = new();
        }

        [Fact]
        public void Create_Game_Board_Should_Create_Board_And_Return_Id()
        {   
            for (var i = 0; i < 10; i++)
            {
                var result = (ObjectResult)_sut.CreateGameBoard().Result;
                var apiResponse = (ApiResponse<int>)(result).Value;

                Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
                Assert.Equal("Success", apiResponse.Message);
                Assert.Equal(i, apiResponse.Result);
            }
        }

        [Theory]
        [MemberData(nameof(AddValidBattleshipsTestData))]
        public void Add_Battleship_Should_Add_Valid_Battleships(int rowStart, int rowEnd, int colStart, int colEnd, TileState[,] expectedBoard)
        {
            _sut.CreateGameBoard();
            var testBattleship = new Battleship()
            {
                RowStart = rowStart,
                RowEnd = rowEnd,
                ColStart = colStart,
                ColEnd = colEnd,
            };

            var result = (ObjectResult)_sut.AddBattleShip(0, testBattleship).Result;
            var apiResponse = (ApiResponse<GameBoard>)(result).Value;

            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(apiResponse.Message, "Success");
            apiResponse.Result.Board.Should().BeEquivalentTo(expectedBoard);
        }

        public static IEnumerable<object[]> AddValidBattleshipsTestData =>
        new List<object[]>
        {
            new object[] { 3, 3, 1, 9, 
                new TileState[,] {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, TileState.UNSUNK, TileState.UNSUNK, TileState.UNSUNK, TileState.UNSUNK, TileState.UNSUNK, TileState.UNSUNK, TileState.UNSUNK, TileState.UNSUNK, TileState.UNSUNK },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            } },
            new object[] { 0, 0, 4, 5, 
                new TileState[,] {
                { 0, 0, 0, 0, TileState.UNSUNK, TileState.UNSUNK, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            } },
            new object[] { 4, 9, 1, 1,
                new TileState[,] {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, TileState.UNSUNK, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, TileState.UNSUNK, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, TileState.UNSUNK, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, TileState.UNSUNK, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, TileState.UNSUNK, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, TileState.UNSUNK, 0, 0, 0, 0, 0, 0, 0, 0 }
            } }
        };

        [Fact]
        public void Add_Battleship_Should_Return_404_For_Nonexistent_Board()
        {
            var result = (ObjectResult)_sut.AddBattleShip(0, new Battleship()).Result;
            var apiResponse = (ApiResponse<GameBoard>)(result).Value;

            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.Equal("Board 0 does not exist", apiResponse.Message);
        }

        [Theory]
        [InlineData(-1, 0, 0, 0)]
        [InlineData(0, 10, 0, 0)]
        [InlineData(1, 1, -1, 0)]
        [InlineData(-1, 1, -1, 0)]
        [InlineData(1, 1, 1, 10)]
        [InlineData(1, 1, 9, 10)]
        public void Add_Battleship_Should_Return_400_For_Out_Of_Bounds_Battleship(int rowStart, int rowEnd, int colStart, int colEnd)
        {
            _sut.CreateGameBoard();
            var testBattleship = new Battleship()
            {
                RowStart = rowStart,
                RowEnd = rowEnd,
                ColStart = colStart,
                ColEnd = colEnd,
            };

            var result = (ObjectResult)_sut.AddBattleShip(0, testBattleship).Result;
            var apiResponse = (ApiResponse<GameBoard>)(result).Value;

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Equal(apiResponse.Message, $"Error adding battleship: row {rowEnd} and {colEnd} out of bounds for board 0");
        }

        [Theory]
        [InlineData(2, 2, 1, 1, 2, 1)]
        [InlineData(2, 2, 9, 9, 2, 9)]
        [InlineData(0, 2, 3, 3, 2, 3)]
        [InlineData(1, 9, 5, 5, 2, 5)]
        public void Add_Battleship_Should_Return_400_When_Conflict_With_Existing_Battleship(int rowStart, int rowEnd, int colStart, int colEnd, int conflictRow, int conflictCol)
        {
            _sut.CreateGameBoard();
            _sut.AddBattleShip(0, new Battleship()
            {
                RowStart = 2,
                RowEnd = 2,
                ColStart = 1,
                ColEnd = 9,
            });
            var testBattleship = new Battleship()
            {
                RowStart = rowStart,
                RowEnd = rowEnd,
                ColStart = colStart,
                ColEnd = colEnd,
            };

            var result = (ObjectResult)_sut.AddBattleShip(0, testBattleship).Result;
            var apiResponse = (ApiResponse<GameBoard>)(result).Value;

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Equal(apiResponse.Message, $"Error adding battleship: existing battleship already occupying board 0 at row {conflictRow} colum {conflictCol}");
        }

        [Theory]
        [InlineData(0, 0, AttackResult.MISS)]
        [InlineData(9, 1, AttackResult.MISS)]
        [InlineData(3, 5, AttackResult.MISS)]
        [InlineData(5, 0, AttackResult.MISS)]
        [InlineData(5, 1, AttackResult.HIT)]
        [InlineData(5, 9, AttackResult.HIT)]
        [InlineData(5, 5, AttackResult.HIT)]
        public void Attack_Should_Return_Result_And_Modify_Board_If_Hit(int row, int col, AttackResult expectedResult)
        {
            _sut.CreateGameBoard();
            _sut.AddBattleShip(0, new Battleship()
            {
                RowStart = 5,
                RowEnd = 5,
                ColStart = 1,
                ColEnd = 9,
            });
            var testAttack = new Attack()
            {
                Row = row,
                Col = col
            };

            var result = (ObjectResult)_sut.Attack(0, testAttack).Result;
            var apiResponse = (ApiResponse<AttackResult>)(result).Value;

            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(expectedResult, apiResponse.Result);
            Assert.Equal(GameController.GameBoards[0].Board[row, col], expectedResult == AttackResult.HIT ? TileState.SUNK : TileState.EMPTY);
        }
    }
}
