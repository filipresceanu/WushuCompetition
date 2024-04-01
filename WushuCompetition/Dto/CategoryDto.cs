using WushuCompetition.Models;

namespace WushuCompetition.Dto
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Sex { get; set; }

        public int LessThanWeight { get; set; }

        public int GraterThanWeight { get; set; }



    }
}
