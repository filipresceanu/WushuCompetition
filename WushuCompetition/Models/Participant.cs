using System.ComponentModel.DataAnnotations.Schema;

namespace WushuCompetition.Models
{
    public class Participant
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Club { get; set; }

        public DateTime BirthDay { get; set; }

        public string Sex { get; set; }

        public int CategoryWeight { get; set; }

        public string Color { get; set; }

        public Guid CategoryId { get; set; }

        public Category Category { get; set; }

        public bool CompeteInNextMatch { get; set; } = true;

        public int calculateAge(DateTime birthDay)
        {
            DateTime Now = DateTime.Now;
            int Years = new DateTime(DateTime.Now.Subtract(birthDay).Ticks).Year - 1;
            return Years;

        }

        public string GetAgeCategory(int age, AgeCategory ageCategory)
        {

            if (age <= ageCategory.LessThanAge && age >= ageCategory.GraterThanAge)
            {
                return ageCategory.Name;
            }
            return null;
        }

        [InverseProperty("CompetitorFirst")]
        public ICollection<Match> MatchesAsFirstCompetitor { get; set; }

        [InverseProperty("CompetitorSecond")]
        public ICollection<Match> MatchesAsSecondCompetitor { get; set; }

        [InverseProperty("ParticipantWinner")]
        public ICollection<Match> MatchesAsWinner { get; set; }


    }
}
