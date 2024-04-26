using System.ComponentModel.DataAnnotations.Schema;
using WushuCompetition.Models;

namespace WushuCompetition.Dto
{
    public class RoundDto
    {
        public Guid Id { get; set; }
        public Guid MatchId { get; set; }
        public int PointParticipantFirst { get; set; }
        public int PointParticipantSecond { get; set; }
        public Guid CompetitorSecondId { get; set; }
        public Guid CompetitorFirstId { get; set;}

    }
}
