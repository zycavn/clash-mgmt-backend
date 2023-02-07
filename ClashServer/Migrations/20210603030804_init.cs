using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClashServer.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "project",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "clashgroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ClashCode = table.Column<string>(nullable: false),
                    Tolerance = table.Column<string>(nullable: true),
                    ProjectId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clashgroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_clashgroup_project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "clash",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    AssignTo = table.Column<string>(nullable: true),
                    GridLocation = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DateFound = table.Column<string>(nullable: true),
                    ClashPoint = table.Column<string>(nullable: true),
                    ClashImagePath = table.Column<string>(nullable: true),
                    Distance = table.Column<string>(nullable: true),
                    ElementId1 = table.Column<int>(nullable: false),
                    Layer1 = table.Column<string>(nullable: true),
                    ItemPath1 = table.Column<string>(nullable: true),
                    ItemName1 = table.Column<string>(nullable: true),
                    ItemType1 = table.Column<string>(nullable: true),
                    ElementId2 = table.Column<int>(nullable: false),
                    Layer2 = table.Column<string>(nullable: true),
                    ItemPath2 = table.Column<string>(nullable: true),
                    ItemName2 = table.Column<string>(nullable: true),
                    ItemType2 = table.Column<string>(nullable: true),
                    ClashGroupId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clash", x => x.Id);
                    table.ForeignKey(
                        name: "FK_clash_clashgroup_ClashGroupId",
                        column: x => x.ClashGroupId,
                        principalTable: "clashgroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    ClashId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_comment_clash_ClashId",
                        column: x => x.ClashId,
                        principalTable: "clash",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "status",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OldStatus = table.Column<string>(nullable: true),
                    NewStatus = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false),
                    ClashId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_status", x => x.Id);
                    table.ForeignKey(
                        name: "FK_status_clash_ClashId",
                        column: x => x.ClashId,
                        principalTable: "clash",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_clash_ClashGroupId",
                table: "clash",
                column: "ClashGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_clashgroup_ProjectId",
                table: "clashgroup",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_comment_ClashId",
                table: "comment",
                column: "ClashId");

            migrationBuilder.CreateIndex(
                name: "IX_status_ClashId",
                table: "status",
                column: "ClashId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comment");

            migrationBuilder.DropTable(
                name: "status");

            migrationBuilder.DropTable(
                name: "clash");

            migrationBuilder.DropTable(
                name: "clashgroup");

            migrationBuilder.DropTable(
                name: "project");
        }
    }
}
