namespace WushuCompetition.Dto.Identity
{
    public class ResetPasswordDto
    {
        public string UserId { get; set; }
        public string NewPassword { get; set; }
        public string ResetPasswordToken { get; set; }
    }
}
