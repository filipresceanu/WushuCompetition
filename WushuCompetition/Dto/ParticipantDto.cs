using WushuCompetition.Models;

namespace WushuCompetition.Dto
{
    public class ParticipantDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Club { get; set; }

        public DateTime BirthDay { get; set; }

        public string Sex { get; set; }

        public int CategoryWeight { get; set; }

       
    }
}
