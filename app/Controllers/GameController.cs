using BattleshipApi.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BattleshipApi.Controllers;

[ApiController]
[Route("api/[controller]/")]
public class GameController : ControllerBase
{
    // dummy DB
    public static List<GameBoard> GameBoards = new();

    [HttpPost("boards")]
    [SwaggerOperation(Summary = "Creates a new game board and returns the created board's id")]
    public ActionResult<ApiResponse<int>> CreateGameBoard()
    {
        var boardId = GameBoards.Count;
        var board = new GameBoard(boardId);
        GameBoards.Add(board);
        return Ok(new ApiResponse<int>() {
            Result = boardId,
            Message = "Success"
        });
    }

    [HttpPost("boards/{boardId:int}/ships")]
    [SwaggerOperation(Summary = "Adds a battleship to the board")]
    public ActionResult<ApiResponse<GameBoard>> AddBattleShip(int boardId, [FromBody]Battleship ship)
    {
        if (boardId >= GameBoards.Count)
        {
            return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<GameBoard>()
            {
                Message = $"Board {boardId} does not exist"
            });
        }

        // TODO: exception middleware to keep controllers DRY
        try {
            GameBoards[boardId].AddBattleShip(ship);
        }
        catch (ArgumentException exception)
        {
            return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<GameBoard>()
            {
                Message = exception.Message
            });
        }
        catch (Exception exception) {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<GameBoard>()
            {
                Message = exception.Message
            });
        }
        
        return Ok(new ApiResponse<GameBoard>() {
            Result = GameBoards[boardId],
            Message = "Success"
        });
    }

    [HttpPost("boards/{boardId:int}/attack")]
    [SwaggerOperation(Summary = "Attemps an attack on the board and returns the attack result")]
    public ActionResult<ApiResponse<AttackResult>> Attack(int boardId, [FromBody]Attack attack)
    {
        if (boardId >= GameBoards.Count)
        {
            return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<AttackResult>()
            {
                Message = $"Board {boardId} does not exist"
            });
        }

        try {
           return Ok(new ApiResponse<AttackResult>() {
                Result = GameBoards[boardId].Attack(attack)
           });
        }
        catch (ArgumentException exception)
        {
            return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<AttackResult>()
            {
                Message = exception.Message
            });
        }
        catch (Exception exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<AttackResult>()
            {
                Message = exception.Message
            });
        }
    }
}
