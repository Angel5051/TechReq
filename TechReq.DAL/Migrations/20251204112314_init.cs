using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechReq.DAL.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    IsExpress = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Assigned_staff_id = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    User_id = table.Column<Guid>(type: "uuid", nullable: false),
                    Services_id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersDbId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requests_Users_UsersDbId",
                        column: x => x.UsersDbId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RequestFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FilePath = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    Request_id = table.Column<int>(type: "integer", nullable: false),
                    RequestDbId = table.Column<Guid>(type: "uuid", nullable: true),
                    ServiceDbId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestFiles_Requests_RequestDbId",
                        column: x => x.RequestDbId,
                        principalTable: "Requests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RequestFiles_Services_ServiceDbId",
                        column: x => x.ServiceDbId,
                        principalTable: "Services",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestFiles_RequestDbId",
                table: "RequestFiles",
                column: "RequestDbId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestFiles_ServiceDbId",
                table: "RequestFiles",
                column: "ServiceDbId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_UsersDbId",
                table: "Requests",
                column: "UsersDbId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestFiles");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
