using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WushuCompetition.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoundParticipants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CompetitorFirstId",
                table: "Rounds",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CompetitorSecondId",
                table: "Rounds",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_CompetitorFirstId",
                table: "Rounds",
                column: "CompetitorFirstId");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_CompetitorSecondId",
                table: "Rounds",
                column: "CompetitorSecondId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Participants_CompetitorFirstId",
                table: "Rounds",
                column: "CompetitorFirstId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Participants_CompetitorSecondId",
                table: "Rounds",
                column: "CompetitorSecondId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Participants_CompetitorFirstId",
                table: "Rounds");

            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Participants_CompetitorSecondId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_CompetitorFirstId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_CompetitorSecondId",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "CompetitorFirstId",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "CompetitorSecondId",
                table: "Rounds");
        }
    }
}
