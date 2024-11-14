using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KhoaLuan_QLNhaTro.Migrations
{
    /// <inheritdoc />
    public partial class _999 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Accounts_AccountId",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Accounts_AccountId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailContracts_Assets_AssetId",
                table: "DetailContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailContracts_Contracts_ContractId",
                table: "DetailContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailContracts_Services_ServiceId",
                table: "DetailContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Details_Bills_BillId",
                table: "Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Details_Services_ServiceId",
                table: "Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_Accounts_AccountId",
                table: "Incidents");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Accounts_AccountId",
                table: "Rooms");

            migrationBuilder.DropTable(
                name: "AssetRooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DetailContracts",
                table: "DetailContracts");

            migrationBuilder.DropIndex(
                name: "IX_DetailContracts_ServiceId",
                table: "DetailContracts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Details",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "DetailContracts");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "CCCD",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Dob",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Accounts");

            migrationBuilder.RenameTable(
                name: "Details",
                newName: "DetailBills");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Rooms",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_AccountId",
                table: "Rooms",
                newName: "IX_Rooms_UserId");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Incidents",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Incidents_AccountId",
                table: "Incidents",
                newName: "IX_Incidents_UserId");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Contracts",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_AccountId",
                table: "Contracts",
                newName: "IX_Contracts_UserId");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Bills",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Bills_AccountId",
                table: "Bills",
                newName: "IX_Bills_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Details_ServiceId",
                table: "DetailBills",
                newName: "IX_DetailBills_ServiceId");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "Bills",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "RoomId",
                table: "Assets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetailContracts",
                table: "DetailContracts",
                columns: new[] { "ContractId", "AssetId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetailBills",
                table: "DetailBills",
                columns: new[] { "BillId", "ServiceId" });

            migrationBuilder.CreateTable(
                name: "IncidentRooms",
                columns: table => new
                {
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IncidentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentRooms", x => new { x.IncidentId, x.RoomId });
                    table.ForeignKey(
                        name: "FK_IncidentRooms_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncidentRooms_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<float>(type: "real", nullable: false),
                    CCCD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_RoomId",
                table: "Assets",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentRooms_RoomId",
                table: "IncidentRooms",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AccountId",
                table: "Users",
                column: "AccountId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Rooms_RoomId",
                table: "Assets",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Users_UserId",
                table: "Bills",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Users_UserId",
                table: "Contracts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailBills_Bills_BillId",
                table: "DetailBills",
                column: "BillId",
                principalTable: "Bills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailBills_Services_ServiceId",
                table: "DetailBills",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailContracts_Assets_AssetId",
                table: "DetailContracts",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailContracts_Contracts_ContractId",
                table: "DetailContracts",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_Users_UserId",
                table: "Incidents",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Users_UserId",
                table: "Rooms",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Rooms_RoomId",
                table: "Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Users_UserId",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Users_UserId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailBills_Bills_BillId",
                table: "DetailBills");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailBills_Services_ServiceId",
                table: "DetailBills");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailContracts_Assets_AssetId",
                table: "DetailContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailContracts_Contracts_ContractId",
                table: "DetailContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_Users_UserId",
                table: "Incidents");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Users_UserId",
                table: "Rooms");

            migrationBuilder.DropTable(
                name: "IncidentRooms");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DetailContracts",
                table: "DetailContracts");

            migrationBuilder.DropIndex(
                name: "IX_Assets_RoomId",
                table: "Assets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DetailBills",
                table: "DetailBills");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Assets");

            migrationBuilder.RenameTable(
                name: "DetailBills",
                newName: "Details");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Rooms",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_UserId",
                table: "Rooms",
                newName: "IX_Rooms_AccountId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Incidents",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Incidents_UserId",
                table: "Incidents",
                newName: "IX_Incidents_AccountId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Contracts",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_UserId",
                table: "Contracts",
                newName: "IX_Contracts_AccountId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Bills",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Bills_UserId",
                table: "Bills",
                newName: "IX_Bills_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_DetailBills_ServiceId",
                table: "Details",
                newName: "IX_Details_ServiceId");

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceId",
                table: "DetailContracts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CCCD",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Dob",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "Name",
                table: "Accounts",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetailContracts",
                table: "DetailContracts",
                columns: new[] { "ContractId", "AssetId", "ServiceId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Details",
                table: "Details",
                columns: new[] { "BillId", "ServiceId" });

            migrationBuilder.CreateTable(
                name: "AssetRooms",
                columns: table => new
                {
                    AssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetRooms", x => new { x.AssetId, x.RoomId });
                    table.ForeignKey(
                        name: "FK_AssetRooms_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetRooms_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetailContracts_ServiceId",
                table: "DetailContracts",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetRooms_RoomId",
                table: "AssetRooms",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Accounts_AccountId",
                table: "Bills",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Accounts_AccountId",
                table: "Contracts",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailContracts_Assets_AssetId",
                table: "DetailContracts",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailContracts_Contracts_ContractId",
                table: "DetailContracts",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailContracts_Services_ServiceId",
                table: "DetailContracts",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Details_Bills_BillId",
                table: "Details",
                column: "BillId",
                principalTable: "Bills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Details_Services_ServiceId",
                table: "Details",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_Accounts_AccountId",
                table: "Incidents",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Accounts_AccountId",
                table: "Rooms",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
