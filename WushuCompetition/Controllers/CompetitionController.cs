using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using OfficeOpenXml;
using System.Collections.ObjectModel;
using System.Drawing;
using WushuCompetition.Dto;
using WushuCompetition.Models;
using WushuCompetition.Services.Interfaces;

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
        public async Task<ActionResult> AddCompetition(CompetitionDto competitionDto)
        {
            var competition = new Competition
            {
                Name = competitionDto.Name,
                Date = competitionDto.Date,
                Categories = new Collection<Category>()

            };

            await _competitionService.CreateCompetition(competition);
            return Ok("Success");
        }

        [HttpGet("get-competitions")]
        public async Task<ActionResult<IEnumerable<CompetitionDto>>> GetCompetitions()
        {
            var competitions = await _competitionService.GetCompetitions();
            return Ok(competitions);
        }


    }
}
