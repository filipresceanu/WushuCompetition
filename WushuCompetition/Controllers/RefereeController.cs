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

        public RefereeController(IRoundService roundService, IRoundRepository roundRepository)
        {
            _roundService = roundService;
            _roundRepository = roundRepository;
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
        public async Task<ActionResult<RoundDto>> AddPointsInTask([FromBody] PointsDto pointsDto)
        {
            try
            {
                var roundDto =
                    await _roundRepository.AddPointsInRoundNoWinner(pointsDto.RoundId,pointsDto.PointsFirstParticipant,pointsDto.PointsSecondParticipant);

                return Ok(roundDto);
            }
            catch (Exception e)
            {
                return BadRequest("asdad");
            }

        }

    }
}
