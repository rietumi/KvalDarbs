using Microsoft.EntityFrameworkCore.Migrations;

namespace KvalDarbsCore.Data.Migrations
{
    public partial class AddManyToManyTrainingTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Trainings_TrainingId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_TrainingId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TrainingId",
                table: "Tasks");

            migrationBuilder.CreateTable(
                name: "TrainingTasks",
                columns: table => new
                {
                    TrainingId = table.Column<int>(nullable: false),
                    TaskId = table.Column<int>(nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingTasks");

            migrationBuilder.AddColumn<int>(
                name: "TrainingId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TrainingId",
                table: "Tasks",
                column: "TrainingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Trainings_TrainingId",
                table: "Tasks",
                column: "TrainingId",
                principalTable: "Trainings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
