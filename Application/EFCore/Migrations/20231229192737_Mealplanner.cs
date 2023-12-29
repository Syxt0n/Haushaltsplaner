using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class Mealplanner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mealplans",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    weeknumber = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mealplans", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mealtypes",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mealtypes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "meals",
                schema: "main",
                columns: table => new
                {
                    id_mealplan1 = table.Column<Guid>(type: "uuid", nullable: false),
                    id_food = table.Column<Guid>(type: "uuid", nullable: false),
                    id_person = table.Column<Guid>(type: "uuid", nullable: false),
                    id_mealtype = table.Column<Guid>(type: "uuid", nullable: false),
                    day = table.Column<int>(type: "int", nullable: false),
                    id_chore = table.Column<Guid>(type: "uuid", nullable: false),
                    id_mealplan = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_meals", x => new { x.id_mealplan1, x.id_food, x.id_person, x.id_mealtype, x.day });
                    table.ForeignKey(
                        name: "FK_meals_food_id_person",
                        column: x => x.id_person,
                        principalSchema: "main",
                        principalTable: "food",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_meals_mealplans_id_mealplan1",
                        column: x => x.id_mealplan1,
                        principalSchema: "main",
                        principalTable: "mealplans",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_meals_mealtypes_id_chore",
                        column: x => x.id_chore,
                        principalSchema: "main",
                        principalTable: "mealtypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_meals_persons_id_person",
                        column: x => x.id_person,
                        principalSchema: "main",
                        principalTable: "persons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_assignments_id_chore",
                schema: "main",
                table: "assignments",
                column: "id_chore");

            migrationBuilder.CreateIndex(
                name: "IX_assignments_id_person",
                schema: "main",
                table: "assignments",
                column: "id_person");

            migrationBuilder.CreateIndex(
                name: "IX_meals_id_chore",
                schema: "main",
                table: "meals",
                column: "id_chore");

            migrationBuilder.CreateIndex(
                name: "IX_meals_id_person",
                schema: "main",
                table: "meals",
                column: "id_person");

            migrationBuilder.AddForeignKey(
                name: "FK_assignments_chores_id_chore",
                schema: "main",
                table: "assignments",
                column: "id_chore",
                principalSchema: "main",
                principalTable: "chores",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_assignments_persons_id_person",
                schema: "main",
                table: "assignments",
                column: "id_person",
                principalSchema: "main",
                principalTable: "persons",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_assignments_chores_id_chore",
                schema: "main",
                table: "assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_assignments_persons_id_person",
                schema: "main",
                table: "assignments");

            migrationBuilder.DropTable(
                name: "meals",
                schema: "main");

            migrationBuilder.DropTable(
                name: "mealplans",
                schema: "main");

            migrationBuilder.DropTable(
                name: "mealtypes",
                schema: "main");

            migrationBuilder.DropIndex(
                name: "IX_assignments_id_chore",
                schema: "main",
                table: "assignments");

            migrationBuilder.DropIndex(
                name: "IX_assignments_id_person",
                schema: "main",
                table: "assignments");
        }
    }
}
