using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using OfficeOpenXml;
using System.Collections.ObjectModel;
using System.Drawing;
using WushuCompetition.Dto;
using WushuCompetition.Models;
using WushuCompetition.Services;

namespace WushuCompetition.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitionController : ControllerBase
    {
        private readonly ICompetitionService _competitionService;

        public CompetitionController(ICompetitionService competitionService)
        {
            _competitionService = competitionService;
        }

        [HttpPut("add-event")]
        public async Task<ActionResult> AddCompetition(CompetitionDto competition)
        {
            var _competition = new Competition
            {
                Name = competition.Name,
                Date = competition.Date,
                Categories = new Collection<Category>()

            };

            await _competitionService.CreateCompetition(_competition);
            return Ok("Success");
        }

        [HttpGet("get-competition")]
        public async Task<ActionResult<IEnumerable<CompetitionDto>>> GetCompetitions()
        {
            var _competition = await _competitionService.GetCompetitions();
            return Ok(_competition);
        }


    }
}
