namespace WushuCompetition.Models
{
    public class AgeCategory
    {
        public Guid Id { get; set; }

        public string Name { get; set; } 

        public int LessThanAge { get; set; }

        public int GraterThanAge { get; set; }

        public ICollection<Category> Categories { get; set; }
    }
}
