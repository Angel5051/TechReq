using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechReq.DAL.Migrations
{
    /// <inheritdoc />
    public partial class bgmj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Service_id",
                table: "Requests",
                newName: "Services_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Services_id",
                table: "Requests",
                newName: "Service_id");
        }
    }
}
