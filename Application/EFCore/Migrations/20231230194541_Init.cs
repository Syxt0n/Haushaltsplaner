using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "main");

            migrationBuilder.CreateTable(
                name: "calendars",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    activeCulture = table.Column<string>(type: "text", nullable: false),
                    activeWeek = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_calendars", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "choreplans",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    weeknumber = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_choreplans", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "chores",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chores", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "food",
                schema: "main",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_food", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "items",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UUID", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_items", x => x.id);
                });

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
                name: "persons",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    displayname = table.Column<string>(type: "text", nullable: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persons", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "shoppinglists",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shoppinglists", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TimeRange = table.Column<TimeSpan>(type: "interval", nullable: false),
                    ReminderInMinutes = table.Column<List<int>>(type: "integer[]", nullable: false),
                    Done = table.Column<bool>(type: "boolean", nullable: false),
                    id_calendar = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_calendars_id_calendar",
                        column: x => x.id_calendar,
                        principalSchema: "main",
                        principalTable: "calendars",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ingredients",
                schema: "main",
                columns: table => new
                {
                    id_food = table.Column<Guid>(type: "uuid", nullable: false),
                    id_item = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ingredients", x => new { x.id_food, x.id_item });
                    table.ForeignKey(
                        name: "FK_ingredients_food_id_food",
                        column: x => x.id_food,
                        principalSchema: "main",
                        principalTable: "food",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ingredients_items_id_item",
                        column: x => x.id_item,
                        principalSchema: "main",
                        principalTable: "items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "assignments",
                schema: "main",
                columns: table => new
                {
                    id_choreplan = table.Column<Guid>(type: "uuid", nullable: false),
                    id_person = table.Column<Guid>(type: "uuid", nullable: false),
                    id_chore = table.Column<Guid>(type: "uuid", nullable: false),
                    day = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assignments", x => new { x.id_choreplan, x.id_person, x.id_chore, x.day });
                    table.ForeignKey(
                        name: "FK_assignments_choreplans_id_choreplan",
                        column: x => x.id_choreplan,
                        principalSchema: "main",
                        principalTable: "choreplans",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_assignments_chores_id_chore",
                        column: x => x.id_chore,
                        principalSchema: "main",
                        principalTable: "chores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_assignments_persons_id_person",
                        column: x => x.id_person,
                        principalSchema: "main",
                        principalTable: "persons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "meals",
                schema: "main",
                columns: table => new
                {
                    id_mealplan = table.Column<Guid>(type: "uuid", nullable: false),
                    id_food = table.Column<Guid>(type: "uuid", nullable: false),
                    id_person = table.Column<Guid>(type: "uuid", nullable: false),
                    id_mealtype = table.Column<Guid>(type: "uuid", nullable: false),
                    day = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_meals", x => new { x.id_mealplan, x.id_food, x.id_person, x.id_mealtype, x.day });
                    table.ForeignKey(
                        name: "FK_meals_food_id_food",
                        column: x => x.id_food,
                        principalSchema: "main",
                        principalTable: "food",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_meals_mealplans_id_mealplan",
                        column: x => x.id_mealplan,
                        principalSchema: "main",
                        principalTable: "mealplans",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_meals_mealtypes_id_mealtype",
                        column: x => x.id_mealtype,
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

            migrationBuilder.CreateTable(
                name: "articles",
                schema: "main",
                columns: table => new
                {
                    id_shoppinglist = table.Column<Guid>(type: "uuid", nullable: false),
                    id_item = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articles", x => new { x.id_shoppinglist, x.id_item });
                    table.ForeignKey(
                        name: "FK_articles_items_id_item",
                        column: x => x.id_item,
                        principalSchema: "main",
                        principalTable: "items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_articles_shoppinglists_id_shoppinglist",
                        column: x => x.id_shoppinglist,
                        principalSchema: "main",
                        principalTable: "shoppinglists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_id_calendar",
                table: "Appointments",
                column: "id_calendar");

            migrationBuilder.CreateIndex(
                name: "IX_articles_id_item",
                schema: "main",
                table: "articles",
                column: "id_item");

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
                name: "IX_ingredients_id_item",
                schema: "main",
                table: "ingredients",
                column: "id_item");

            migrationBuilder.CreateIndex(
                name: "IX_meals_id_food",
                schema: "main",
                table: "meals",
                column: "id_food");

            migrationBuilder.CreateIndex(
                name: "IX_meals_id_mealtype",
                schema: "main",
                table: "meals",
                column: "id_mealtype");

            migrationBuilder.CreateIndex(
                name: "IX_meals_id_person",
                schema: "main",
                table: "meals",
                column: "id_person");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "articles",
                schema: "main");

            migrationBuilder.DropTable(
                name: "assignments",
                schema: "main");

            migrationBuilder.DropTable(
                name: "ingredients",
                schema: "main");

            migrationBuilder.DropTable(
                name: "meals",
                schema: "main");

            migrationBuilder.DropTable(
                name: "calendars",
                schema: "main");

            migrationBuilder.DropTable(
                name: "shoppinglists",
                schema: "main");

            migrationBuilder.DropTable(
                name: "choreplans",
                schema: "main");

            migrationBuilder.DropTable(
                name: "chores",
                schema: "main");

            migrationBuilder.DropTable(
                name: "items",
                schema: "main");

            migrationBuilder.DropTable(
                name: "food",
                schema: "main");

            migrationBuilder.DropTable(
                name: "mealplans",
                schema: "main");

            migrationBuilder.DropTable(
                name: "mealtypes",
                schema: "main");

            migrationBuilder.DropTable(
                name: "persons",
                schema: "main");
        }
    }
}
