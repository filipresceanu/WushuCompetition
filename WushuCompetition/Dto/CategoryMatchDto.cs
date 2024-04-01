namespace WushuCompetition.Dto
{
    public class CategoryMatchDto
    {
        public string Seniority { get; set; }

        public string Sex { get; set; }

        public int LessThanWeight { get; set; }

        public int GraterThanWeight { get; set; }  

        public List<MatchDto> Matches { get; set; }
    }
}
