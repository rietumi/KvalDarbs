using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KvalDarbsCore.Data.Migrations
{
    public partial class RemovedUnnecessaryObjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Errors");

            migrationBuilder.DropTable(
                name: "SystemErrors");

            migrationBuilder.DropTable(
                name: "TrainingTasks");

            migrationBuilder.DropColumn(
                name: "Repetiton",
                table: "Tasks");

            migrationBuilder.AddColumn<int>(
                name: "ExerciseId",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Repetition",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrainingId",
                table: "Tasks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ExerciseId",
                table: "Tasks",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TrainingId",
                table: "Tasks",
                column: "TrainingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Exercises_ExerciseId",
                table: "Tasks",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Trainings_TrainingId",
                table: "Tasks",
                column: "TrainingId",
                principalTable: "Trainings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Exercises_ExerciseId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Trainings_TrainingId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_ExerciseId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_TrainingId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ExerciseId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Repetition",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TrainingId",
                table: "Tasks");

            migrationBuilder.AddColumn<string>(
                name: "Repetiton",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Errors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Errors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemErrors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemErrors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingTasks",
                columns: table => new
                {
                    TrainingId = table.Column<int>(type: "int", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingTasks", x => new { x.TrainingId, x.TaskId });
                    table.ForeignKey(
                        name: "FK_TrainingTasks_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingTasks_Trainings_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "Trainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingTasks_TaskId",
                table: "TrainingTasks",
                column: "TaskId");
        }
    }
}
