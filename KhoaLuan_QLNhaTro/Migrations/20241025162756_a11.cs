using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KhoaLuan_QLNhaTro.Migrations
{
    /// <inheritdoc />
    public partial class a11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    idAsset = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.idAsset);
                });

            migrationBuilder.CreateTable(
                name: "Houses",
                columns: table => new
                {
                    idHouse = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    floorNumber = table.Column<int>(type: "int", nullable: false),
                    roomNumber = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Houses", x => x.idHouse);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    idRole = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.idRole);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    idService = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.idService);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    idAccount = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CCCD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    idRole = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.idAccount);
                    table.ForeignKey(
                        name: "FK_Accounts_Roles_idRole",
                        column: x => x.idRole,
                        principalTable: "Roles",
                        principalColumn: "idRole",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Incidents",
                columns: table => new
                {
                    idIncident = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    idAccount = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incidents", x => x.idIncident);
                    table.ForeignKey(
                        name: "FK_Incidents_Accounts_idAccount",
                        column: x => x.idAccount,
                        principalTable: "Accounts",
                        principalColumn: "idAccount",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    idRoom = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Acreage = table.Column<float>(type: "real", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    idHouse = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    idAccount = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.idRoom);
                    table.ForeignKey(
                        name: "FK_Rooms_Accounts_idAccount",
                        column: x => x.idAccount,
                        principalTable: "Accounts",
                        principalColumn: "idAccount",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rooms_Houses_idHouse",
                        column: x => x.idHouse,
                        principalTable: "Houses",
                        principalColumn: "idHouse",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssetRooms",
                columns: table => new
                {
                    idRoom = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    idAsset = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetRooms", x => new { x.idRoom, x.idAsset });
                    table.ForeignKey(
                        name: "FK_AssetRooms_Assets_idAsset",
                        column: x => x.idAsset,
                        principalTable: "Assets",
                        principalColumn: "idAsset",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetRooms_Rooms_idRoom",
                        column: x => x.idRoom,
                        principalTable: "Rooms",
                        principalColumn: "idRoom",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    idBill = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Total = table.Column<float>(type: "real", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    idRoom = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    idAccount = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.idBill);
                    table.ForeignKey(
                        name: "FK_Bills_Accounts_idAccount",
                        column: x => x.idAccount,
                        principalTable: "Accounts",
                        principalColumn: "idAccount",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bills_Rooms_idRoom",
                        column: x => x.idRoom,
                        principalTable: "Rooms",
                        principalColumn: "idRoom",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    idContract = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    idRoom = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    idAccount = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.idContract);
                    table.ForeignKey(
                        name: "FK_Contracts_Accounts_idAccount",
                        column: x => x.idAccount,
                        principalTable: "Accounts",
                        principalColumn: "idAccount",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_Rooms_idRoom",
                        column: x => x.idRoom,
                        principalTable: "Rooms",
                        principalColumn: "idRoom",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoomServices",
                columns: table => new
                {
                    idRoom = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    idService = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomServices", x => new { x.idRoom, x.idService });
                    table.ForeignKey(
                        name: "FK_RoomServices_Rooms_idRoom",
                        column: x => x.idRoom,
                        principalTable: "Rooms",
                        principalColumn: "idRoom",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomServices_Services_idService",
                        column: x => x.idService,
                        principalTable: "Services",
                        principalColumn: "idService",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetailBills",
                columns: table => new
                {
                    idBill = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    idService = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Total = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailBills", x => new { x.idBill, x.idService });
                    table.ForeignKey(
                        name: "FK_DetailBills_Bills_idBill",
                        column: x => x.idBill,
                        principalTable: "Bills",
                        principalColumn: "idBill",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetailBills_Services_idService",
                        column: x => x.idService,
                        principalTable: "Services",
                        principalColumn: "idService",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetailContracts",
                columns: table => new
                {
                    idContract = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    idAsset = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    idService = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailContracts", x => new { x.idContract, x.idService, x.idAsset });
                    table.ForeignKey(
                        name: "FK_DetailContracts_Assets_idAsset",
                        column: x => x.idAsset,
                        principalTable: "Assets",
                        principalColumn: "idAsset",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetailContracts_Contracts_idContract",
                        column: x => x.idContract,
                        principalTable: "Contracts",
                        principalColumn: "idContract",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetailContracts_Services_idService",
                        column: x => x.idService,
                        principalTable: "Services",
                        principalColumn: "idService",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_idRole",
                table: "Accounts",
                column: "idRole");

            migrationBuilder.CreateIndex(
                name: "IX_AssetRooms_idAsset",
                table: "AssetRooms",
                column: "idAsset");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_idAccount",
                table: "Bills",
                column: "idAccount");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_idRoom",
                table: "Bills",
                column: "idRoom");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_idAccount",
                table: "Contracts",
                column: "idAccount");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_idRoom",
                table: "Contracts",
                column: "idRoom");

            migrationBuilder.CreateIndex(
                name: "IX_DetailBills_idService",
                table: "DetailBills",
                column: "idService");

            migrationBuilder.CreateIndex(
                name: "IX_DetailContracts_idAsset",
                table: "DetailContracts",
                column: "idAsset");

            migrationBuilder.CreateIndex(
                name: "IX_DetailContracts_idService",
                table: "DetailContracts",
                column: "idService");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_idAccount",
                table: "Incidents",
                column: "idAccount");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_idAccount",
                table: "Rooms",
                column: "idAccount");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_idHouse",
                table: "Rooms",
                column: "idHouse");

            migrationBuilder.CreateIndex(
                name: "IX_RoomServices_idService",
                table: "RoomServices",
                column: "idService");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetRooms");

            migrationBuilder.DropTable(
                name: "DetailBills");

            migrationBuilder.DropTable(
                name: "DetailContracts");

            migrationBuilder.DropTable(
                name: "Incidents");

            migrationBuilder.DropTable(
                name: "RoomServices");

            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Houses");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
