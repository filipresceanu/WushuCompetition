using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WushuCompetition.Dto;
using WushuCompetition.Extensions;
using WushuCompetition.Repository.Interfaces;
using WushuCompetition.Services.Interfaces;

namespace WushuCompetition.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RefereeController : ControllerBase
    {
        private readonly IRoundService _roundService;
        private readonly IRoundRepository _roundRepository;
        private readonly IMatchService _matchService;

        public RefereeController(IRoundService roundService, IRoundRepository roundRepository, IMatchService matchService)
        {
            _roundService = roundService;
            _roundRepository = roundRepository;
            _matchService = matchService;
        }

        [HttpGet]
        [Route("GetRounds")]
        public async Task<ActionResult<IEnumerable<RoundDto>>> GetRounds()
        {
            try
            {
                var refereeId = User.GetUserId();
                var rounds = await _roundService.GetRoundsForSpecificRefereeNoWinner(refereeId);
                return Ok(rounds);
            }
            catch (Exception e)
            {
                return BadRequest("Unable to get rounds");
            }

        }

        [HttpPost]
        [Route("AddPointsInRound")]
        public async Task<ActionResult<RoundDto>> AddPointsInRound([FromBody] PointsDto pointsDto)
        {
            try
            {
                var round = await _roundRepository.AddPointsInRoundNoWinner(pointsDto.RoundId, 
                    pointsDto.PointsFirstParticipant, pointsDto.PointsSecondParticipant);

                var roundDto = await _roundRepository.CalculateWinnerRound(round.Id);
                return Ok(roundDto);
            }
            catch (Exception e)
            {
                return BadRequest("asdad");
            }

        }

        [AllowAnonymous]
        [HttpPut]
        [Route("CalculateWinner")]
        public async Task<ActionResult<MatchResultDto>> CalculateWinnerMatch(Guid matchId)
        {
            try
            {
                var result = await _matchService.CalculateWinnerMatch(matchId);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest("Something Bad");
            }
        }

    }
}
