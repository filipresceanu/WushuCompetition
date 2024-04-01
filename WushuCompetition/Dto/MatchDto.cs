namespace WushuCompetition.Dto
{
    public class MatchDto
    {
        public int MatchNumber { get; set; }

        public string ParticipantFirstName { get; set; }

        public string ParticipantSecondName { get; set; }

        public int FirstParticipantWeight { get; set; }

        public int SecondParticipantWeight { get; set;}

        public string WinnerMatch { get; set; }
    }
}
