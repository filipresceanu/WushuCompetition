namespace WushuCompetition.Dto
{
    public class MatchDto
    {
        public Guid Id { get; set; }
        public string ParticipantFirstName { get; set; }
        public string ParticipantSecondName { get; set; }
        public string Referee { get; set; }
        public string WinnerMatch { get; set; }
    }
}
