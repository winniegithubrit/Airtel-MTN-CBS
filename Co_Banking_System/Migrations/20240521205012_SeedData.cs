using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Co_Banking_System.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TransactionStatuses",
                columns: new[] { "StatusId", "Description", "StatusCode" },
                values: new object[,]
                {
                    { 1, "Transaction is pending", "Pending" },
                    { 2, "Transaction is completed", "Completed" },
                    { 3, "Transaction has failed", "Failed" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "LastName", "PasswordHash", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 5, 21, 20, 50, 11, 400, DateTimeKind.Utc).AddTicks(7332), "wendy.jomo@gmail.com", "Wendy", "Jomo", "password", "0797594751" },
                    { 2, new DateTime(2024, 5, 21, 20, 50, 11, 400, DateTimeKind.Utc).AddTicks(7335), "lexxnjiru@gmail.com", "Lexxie", "Wanjiru", "lexx", "0797594745" },
                    { 3, new DateTime(2024, 5, 21, 20, 50, 11, 400, DateTimeKind.Utc).AddTicks(7337), "johnchi@gmail.com", "Johnson", "Njichi", "john", "0797794745" },
                    { 4, new DateTime(2024, 5, 21, 20, 50, 11, 400, DateTimeKind.Utc).AddTicks(7338), "kamawe@gmail.com", "Wesley", "Kamau", "kama", "0700594745" }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "TransactionId", "Amount", "CreatedAt", "Reference", "StatusId", "UserId" },
                values: new object[,]
                {
                    { 1, 100.00m, new DateTime(2024, 5, 21, 20, 50, 11, 400, DateTimeKind.Utc).AddTicks(7381), "TXN10001", 2, 1 },
                    { 2, 200.00m, new DateTime(2024, 5, 21, 20, 50, 11, 400, DateTimeKind.Utc).AddTicks(7384), "TXN10002", 1, 1 },
                    { 3, 300.00m, new DateTime(2024, 5, 21, 20, 50, 11, 400, DateTimeKind.Utc).AddTicks(7386), "TXN10003", 3, 1 },
                    { 4, 400.00m, new DateTime(2024, 5, 21, 20, 50, 11, 400, DateTimeKind.Utc).AddTicks(7388), "TXN10004", 2, 1 }
                });

            migrationBuilder.InsertData(
                table: "AdditionalInfos",
                columns: new[] { "AdditionalInfoId", "InfoKey", "InfoValue", "TransactionId" },
                values: new object[,]
                {
                    { 1, "Note", "Payment for services", 1 },
                    { 2, "Note", "Pending payment", 2 },
                    { 3, "Note", "Failed payment", 3 },
                    { 4, "Note", "Successful payment", 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AdditionalInfos",
                keyColumn: "AdditionalInfoId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AdditionalInfos",
                keyColumn: "AdditionalInfoId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AdditionalInfos",
                keyColumn: "AdditionalInfoId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AdditionalInfos",
                keyColumn: "AdditionalInfoId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TransactionStatuses",
                keyColumn: "StatusId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TransactionStatuses",
                keyColumn: "StatusId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TransactionStatuses",
                keyColumn: "StatusId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
