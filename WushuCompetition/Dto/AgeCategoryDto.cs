using WushuCompetition.Models;

namespace WushuCompetition.Dto
{
    public class AgeCategoryDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int LessThanAge { get; set; }

        public int GraterThanAge { get; set; }

        
    }
}
