using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class DAOclass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Author_AuthorId",
                table: "Book");

            migrationBuilder.DropForeignKey(
                name: "FK_Book_BookCategory_BookCategoryId",
                table: "Book");

            migrationBuilder.DropIndex(
                name: "IX_Book_AuthorId",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Book");

            migrationBuilder.RenameColumn(
                name: "BookCategoryId",
                table: "Book",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Book_BookCategoryId",
                table: "Book",
                newName: "IX_Book_CategoryId");

            migrationBuilder.CreateTable(
                name: "AuthorBook",
                columns: table => new
                {
                    AuthorsId = table.Column<long>(type: "bigint", nullable: false),
                    BooksId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorBook", x => new { x.AuthorsId, x.BooksId });
                    table.ForeignKey(
                        name: "FK_AuthorBook_Author_AuthorsId",
                        column: x => x.AuthorsId,
                        principalTable: "Author",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorBook_Book_BooksId",
                        column: x => x.BooksId,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorBook_BooksId",
                table: "AuthorBook",
                column: "BooksId");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_BookCategory_CategoryId",
                table: "Book",
                column: "CategoryId",
                principalTable: "BookCategory",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_BookCategory_CategoryId",
                table: "Book");

            migrationBuilder.DropTable(
                name: "AuthorBook");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Book",
                newName: "BookCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Book_CategoryId",
                table: "Book",
                newName: "IX_Book_BookCategoryId");

            migrationBuilder.AddColumn<long>(
                name: "AuthorId",
                table: "Book",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Book_AuthorId",
                table: "Book",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Author_AuthorId",
                table: "Book",
                column: "AuthorId",
                principalTable: "Author",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_BookCategory_BookCategoryId",
                table: "Book",
                column: "BookCategoryId",
                principalTable: "BookCategory",
                principalColumn: "Id");
        }
    }
}
