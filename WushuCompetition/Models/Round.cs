using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WushuCompetition.Models
{
    public class Round
    {
        [Key]
        public Guid Id { get; set; }
        public Guid MatchId { get; set; }
        public Match Match { get; set; }
        [ForeignKey("CompetitorFirstId")]
        public Participant CompetitorFirst { get; set; }
        public Guid CompetitorFirstId { get; set; }
        public int PointParticipantFirst { get; set; }
        [ForeignKey("CompetitorSecondId")]
        public Participant CompetitorSecond { get; set; }
        public int PointParticipantSecond { get; set; }
        public Guid CompetitorSecondId { get; set; }

        [ForeignKey("ParticipantWinnerId")]
        public Participant ParticipantWinner { get; set; }
        public Guid? ParticipantWinnerId { get; set; }

    }
}
