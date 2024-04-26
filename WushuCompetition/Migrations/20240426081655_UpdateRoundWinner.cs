using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WushuCompetition.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoundWinner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Participants_ParticipantWinnerId",
                table: "Rounds");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParticipantWinnerId",
                table: "Rounds",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Participants_ParticipantWinnerId",
                table: "Rounds",
                column: "ParticipantWinnerId",
                principalTable: "Participants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Participants_ParticipantWinnerId",
                table: "Rounds");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParticipantWinnerId",
                table: "Rounds",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Participants_ParticipantWinnerId",
                table: "Rounds",
                column: "ParticipantWinnerId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
