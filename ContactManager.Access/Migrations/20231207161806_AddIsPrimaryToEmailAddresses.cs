﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactManagerStarter.Access.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPrimaryToEmailAddresses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrimary",
                table: "EmailAddresses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "EmailAddresses",
                keyColumn: "Id",
                keyValue: new Guid("3a406f64-ad7b-4098-ab01-7e93aae2b851"),
                column: "IsPrimary",
                value: false);

            migrationBuilder.UpdateData(
                table: "EmailAddresses",
                keyColumn: "Id",
                keyValue: new Guid("3ddeb084-5e5d-4eca-b275-e4f6886e04dc"),
                column: "IsPrimary",
                value: true);

            migrationBuilder.UpdateData(
                table: "EmailAddresses",
                keyColumn: "Id",
                keyValue: new Guid("5111f412-a7f4-4169-bb27-632687569ccd"),
                column: "IsPrimary",
                value: true);

            migrationBuilder.UpdateData(
                table: "EmailAddresses",
                keyColumn: "Id",
                keyValue: new Guid("d1a50413-20c0-4972-a351-8be24e1fc939"),
                column: "IsPrimary",
                value: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailAddresses_IsPrimary_ContactId",
                table: "EmailAddresses",
                columns: new[] { "IsPrimary", "ContactId" },
                unique: true,
                filter: "IsPrimary = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmailAddresses_IsPrimary_ContactId",
                table: "EmailAddresses");

            migrationBuilder.DropColumn(
                name: "IsPrimary",
                table: "EmailAddresses");
        }
    }
}
