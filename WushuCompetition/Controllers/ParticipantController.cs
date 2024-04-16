using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WushuCompetition.Dto;
using WushuCompetition.Services.Interfaces;

namespace WushuCompetition.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantController : ControllerBase
    {
        private readonly IParticipantService _participantService;

        public ParticipantController(IParticipantService participantService)
        {
            _participantService = participantService;
        }

        [HttpDelete("delete-participant")]
        public async Task<ActionResult> Delete(Guid id) 
        {
            try
            {
                await _participantService.DeleteParticipant(id);
                return Ok("success");
            }
            catch 
            {
                return BadRequest("unable to delete participant");
            }
        }

        [HttpPut("add-in-competition/{id}")]
        public async Task<ActionResult> AddParticipantsInCompetition(Guid id,
            ParticipantDto participantDto)
        {
            string result = await _participantService.AddParticipantsInCompetition(id,
                participantDto);
            if (string.IsNullOrEmpty(result))
            {
                return BadRequest("There is no category for this participant");
            }
            return Ok("success");
        }

        [HttpGet("participants-for-specific/{competitionId}")]
        public async Task<ActionResult<IEnumerable<ParticipantDto>>> GetParticipantForCompetition(Guid competitionId)
        {
            var participants = await _participantService.GetParticipantsInCompetitionId(competitionId);
            return Ok(participants);
        }
    }
}
