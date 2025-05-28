using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatSphere.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddedAdminFields : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "IsBanned",
            table: "Users",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "IsActive",
            table: "Rooms",
            type: "bit",
            nullable: false,
            defaultValue: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsBanned",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "IsActive",
            table: "Rooms");
    }
}
