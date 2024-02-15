using Microsoft.EntityFrameworkCore.Migrations;

namespace Article_Project.DataLayer.Migrations
{
    public partial class Update_Configure_Comment_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ReplyComment",
                table: "Comments",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ReplyComment",
                table: "Comments",
                column: "ReplyComment");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_ReplyComment",
                table: "Comments",
                column: "ReplyComment",
                principalTable: "Comments",
                principalColumn: "CommentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_ReplyComment",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ReplyComment",
                table: "Comments");

            migrationBuilder.AlterColumn<long>(
                name: "ReplyComment",
                table: "Comments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
