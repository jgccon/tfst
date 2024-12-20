using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheFullStackTeam.Infrastructure.Persistence.Sql.Migrations;

/// <inheritdoc />
public partial class AddRefreshToken : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Comments");

        migrationBuilder.DropTable(
            name: "Votes");

        migrationBuilder.DropTable(
            name: "Answers");

        migrationBuilder.DropTable(
            name: "Questions");

        migrationBuilder.CreateTable(
            name: "RefreshTokens",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                AccountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                JwtId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                IsUsed = table.Column<bool>(type: "bit", nullable: false),
                IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                ExpireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                table.ForeignKey(
                    name: "FK_RefreshTokens_Accounts_AccountId",
                    column: x => x.AccountId,
                    principalTable: "Accounts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_RefreshTokens_AccountId",
            table: "RefreshTokens",
            column: "AccountId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "RefreshTokens");

        migrationBuilder.CreateTable(
            name: "Questions",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                Moniker = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Questions", x => x.Id);
                table.ForeignKey(
                    name: "FK_Questions_UserProfiles_AuthorId",
                    column: x => x.AuthorId,
                    principalTable: "UserProfiles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Answers",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                QuestionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                IsAccepted = table.Column<bool>(type: "bit", nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Answers", x => x.Id);
                table.ForeignKey(
                    name: "FK_Answers_Questions_QuestionId",
                    column: x => x.QuestionId,
                    principalTable: "Questions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Answers_UserProfiles_AuthorId",
                    column: x => x.AuthorId,
                    principalTable: "UserProfiles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Comments",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                AnswerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                QuestionId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Comments", x => x.Id);
                table.ForeignKey(
                    name: "FK_Comments_Answers_AnswerId",
                    column: x => x.AnswerId,
                    principalTable: "Answers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Comments_Questions_QuestionId",
                    column: x => x.QuestionId,
                    principalTable: "Questions",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Comments_UserProfiles_AuthorId",
                    column: x => x.AuthorId,
                    principalTable: "UserProfiles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Votes",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                AnswerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                QuestionId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                IsUpvote = table.Column<bool>(type: "bit", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Votes", x => x.Id);
                table.ForeignKey(
                    name: "FK_Votes_Answers_AnswerId",
                    column: x => x.AnswerId,
                    principalTable: "Answers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Votes_Questions_QuestionId",
                    column: x => x.QuestionId,
                    principalTable: "Questions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Votes_UserProfiles_AuthorId",
                    column: x => x.AuthorId,
                    principalTable: "UserProfiles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Answers_AuthorId",
            table: "Answers",
            column: "AuthorId");

        migrationBuilder.CreateIndex(
            name: "IX_Answers_QuestionId",
            table: "Answers",
            column: "QuestionId");

        migrationBuilder.CreateIndex(
            name: "IX_Comments_AnswerId",
            table: "Comments",
            column: "AnswerId");

        migrationBuilder.CreateIndex(
            name: "IX_Comments_AuthorId",
            table: "Comments",
            column: "AuthorId");

        migrationBuilder.CreateIndex(
            name: "IX_Comments_QuestionId",
            table: "Comments",
            column: "QuestionId");

        migrationBuilder.CreateIndex(
            name: "IX_Questions_AuthorId",
            table: "Questions",
            column: "AuthorId");

        migrationBuilder.CreateIndex(
            name: "IX_Questions_Moniker",
            table: "Questions",
            column: "Moniker",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Votes_AnswerId",
            table: "Votes",
            column: "AnswerId");

        migrationBuilder.CreateIndex(
            name: "IX_Votes_AuthorId",
            table: "Votes",
            column: "AuthorId");

        migrationBuilder.CreateIndex(
            name: "IX_Votes_QuestionId",
            table: "Votes",
            column: "QuestionId");
    }
}

