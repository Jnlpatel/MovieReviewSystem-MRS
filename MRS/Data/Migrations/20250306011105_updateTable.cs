using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MRS.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenres_Genres_GenreId",
                table: "MovieGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenres_Movies_MovieId",
                table: "MovieGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_UserId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Movies_MovieId",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Reviews",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "Reviews",
                newName: "MovieID");

            migrationBuilder.RenameColumn(
                name: "ReviewId",
                table: "Reviews",
                newName: "ReviewID");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                newName: "IX_Reviews_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_MovieId",
                table: "Reviews",
                newName: "IX_Reviews_MovieID");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "Movies",
                newName: "MovieID");

            migrationBuilder.RenameColumn(
                name: "GenreId",
                table: "MovieGenres",
                newName: "GenreID");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "MovieGenres",
                newName: "MovieID");

            migrationBuilder.RenameIndex(
                name: "IX_MovieGenres_GenreId",
                table: "MovieGenres",
                newName: "IX_MovieGenres_GenreID");

            migrationBuilder.RenameColumn(
                name: "GenreId",
                table: "Genres",
                newName: "GenreID");

            migrationBuilder.AddColumn<string>(
                name: "ReviewText",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Director",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenres_Genres_GenreID",
                table: "MovieGenres",
                column: "GenreID",
                principalTable: "Genres",
                principalColumn: "GenreID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenres_Movies_MovieID",
                table: "MovieGenres",
                column: "MovieID",
                principalTable: "Movies",
                principalColumn: "MovieID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_UserID",
                table: "Reviews",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Movies_MovieID",
                table: "Reviews",
                column: "MovieID",
                principalTable: "Movies",
                principalColumn: "MovieID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenres_Genres_GenreID",
                table: "MovieGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenres_Movies_MovieID",
                table: "MovieGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_UserID",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Movies_MovieID",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ReviewText",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Director",
                table: "Movies");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Reviews",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "MovieID",
                table: "Reviews",
                newName: "MovieId");

            migrationBuilder.RenameColumn(
                name: "ReviewID",
                table: "Reviews",
                newName: "ReviewId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_UserID",
                table: "Reviews",
                newName: "IX_Reviews_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_MovieID",
                table: "Reviews",
                newName: "IX_Reviews_MovieId");

            migrationBuilder.RenameColumn(
                name: "MovieID",
                table: "Movies",
                newName: "MovieId");

            migrationBuilder.RenameColumn(
                name: "GenreID",
                table: "MovieGenres",
                newName: "GenreId");

            migrationBuilder.RenameColumn(
                name: "MovieID",
                table: "MovieGenres",
                newName: "MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieGenres_GenreID",
                table: "MovieGenres",
                newName: "IX_MovieGenres_GenreId");

            migrationBuilder.RenameColumn(
                name: "GenreID",
                table: "Genres",
                newName: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenres_Genres_GenreId",
                table: "MovieGenres",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "GenreId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenres_Movies_MovieId",
                table: "MovieGenres",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_UserId",
                table: "Reviews",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Movies_MovieId",
                table: "Reviews",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
