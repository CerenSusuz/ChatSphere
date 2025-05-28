using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatSphere.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddRoomEntity : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<Guid>(
            name: "RoomId",
            table: "ChatMessages",
            type: "uniqueidentifier",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AddColumn<bool>(
            name: "IsMine",
            table: "ChatMessages",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.CreateTable(
            name: "Rooms",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Rooms", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ChatMessages_RoomId",
            table: "ChatMessages",
            column: "RoomId");

        migrationBuilder.AddForeignKey(
            name: "FK_ChatMessages_Rooms_RoomId",
            table: "ChatMessages",
            column: "RoomId",
            principalTable: "Rooms",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ChatMessages_Rooms_RoomId",
            table: "ChatMessages");

        migrationBuilder.DropTable(
            name: "Rooms");

        migrationBuilder.DropIndex(
            name: "IX_ChatMessages_RoomId",
            table: "ChatMessages");

        migrationBuilder.DropColumn(
            name: "IsMine",
            table: "ChatMessages");

        migrationBuilder.AlterColumn<string>(
            name: "RoomId",
            table: "ChatMessages",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier");
    }
}
