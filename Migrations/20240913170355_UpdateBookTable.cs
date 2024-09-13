using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Collections_CollectionId",
                table: "Book");

            migrationBuilder.DropIndex(
                name: "IX_Book_CollectionId",
                table: "Book");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Collections",
                table: "Collections");

            migrationBuilder.RenameTable(
                name: "Collections",
                newName: "Collection");

            migrationBuilder.RenameColumn(
                name: "CollectionId",
                table: "Book",
                newName: "Price");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Book",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "Discount",
                table: "Book",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ISBN",
                table: "Book",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PublicationDate",
                table: "Book",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "Book",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Sold",
                table: "Book",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Book",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Book",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Book",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Collection",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Collection",
                table: "Collection",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BookCollection",
                columns: table => new
                {
                    BooksId = table.Column<long>(type: "bigint", nullable: false),
                    CollectionsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookCollection", x => new { x.BooksId, x.CollectionsId });
                    table.ForeignKey(
                        name: "FK_BookCollection_Book_BooksId",
                        column: x => x.BooksId,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookCollection_Collection_CollectionsId",
                        column: x => x.CollectionsId,
                        principalTable: "Collection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookCollection_CollectionsId",
                table: "BookCollection",
                column: "CollectionsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookCollection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Collection",
                table: "Collection");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "ISBN",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "PublicationDate",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Sold",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Book");

            migrationBuilder.RenameTable(
                name: "Collection",
                newName: "Collections");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Book",
                newName: "CollectionId");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Collections",
                table: "Collections",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Book_CollectionId",
                table: "Book",
                column: "CollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Collections_CollectionId",
                table: "Book",
                column: "CollectionId",
                principalTable: "Collections",
                principalColumn: "Id");
        }
    }
}
