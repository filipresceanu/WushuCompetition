using Microsoft.AspNetCore.Mvc;
using WushuCompetition.Repository.Interfaces;
using WushuCompetition.Services.Interfaces;

namespace WushuCompetition.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;
        private readonly IAccountService _accountService;

        public MatchController(IMatchService matchService, IAccountService accountService)
        {
            _matchService = matchService;
            _accountService = accountService;
        }

        [HttpPut("add-participants-matches")]
        public async Task<ActionResult>AddParticipantsInMatches(Guid competitionId)
        {
            try
            {
                await _matchService.HandleParticipantsNumber(competitionId);
                await _accountService.DistributeReferees();
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest("Something Bad");
            }
        }
    }
}
