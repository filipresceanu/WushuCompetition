using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WushuCompetition.Migrations
{
    public partial class UpdateParticipants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CompeteInNextMatch",
                table: "Participants",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Referee",
                table: "Matches",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompeteInNextMatch",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "Referee",
                table: "Matches");
        }
    }
}
