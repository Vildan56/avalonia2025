using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project2025.Migrations
{
    /// <inheritdoc />
    public partial class InitialDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasNeedsOrOffers = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Length = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PropertyTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Realtors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommissionShare = table.Column<double>(type: "float", nullable: true),
                    HasNeedsOrOffers = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Realtors", x => x.Id);
                    table.CheckConstraint("CK_Realtor_CommissionShare", "CommissionShare BETWEEN 0 AND 100");
                });

            migrationBuilder.CreateTable(
                name: "PropertyObjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    House = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apartment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<float>(type: "real", nullable: false),
                    Longitude = table.Column<float>(type: "real", nullable: false),
                    Floor = table.Column<int>(type: "int", nullable: false),
                    RoomsCount = table.Column<int>(type: "int", nullable: false),
                    Area = table.Column<float>(type: "real", nullable: false),
                    PropertyTypeId = table.Column<int>(type: "int", nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyObjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyObjects_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PropertyObjects_PropertyTypes_PropertyTypeId",
                        column: x => x.PropertyTypeId,
                        principalTable: "PropertyTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Requirements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    House = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinPrice = table.Column<int>(type: "int", nullable: false),
                    MaxPrice = table.Column<int>(type: "int", nullable: false),
                    Apartment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    RealtorId = table.Column<int>(type: "int", nullable: true),
                    PropertyTypeId = table.Column<int>(type: "int", nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requirements", x => x.Id);
                    table.CheckConstraint("CK_Requirement_MaxPrice", "MaxPrice >= 0");
                    table.CheckConstraint("CK_Requirement_MinPrice", "MinPrice >= 0");
                    table.ForeignKey(
                        name: "FK_Requirements_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Requirements_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Requirements_PropertyTypes_PropertyTypeId",
                        column: x => x.PropertyTypeId,
                        principalTable: "PropertyTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Requirements_Realtors_RealtorId",
                        column: x => x.RealtorId,
                        principalTable: "Realtors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    RealtorId = table.Column<int>(type: "int", nullable: false),
                    PropertyObjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offers_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Offers_PropertyObjects_PropertyObjectId",
                        column: x => x.PropertyObjectId,
                        principalTable: "PropertyObjects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Offers_Realtors_RealtorId",
                        column: x => x.RealtorId,
                        principalTable: "Realtors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApartmentRequirements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinFloor = table.Column<int>(type: "int", nullable: false),
                    MaxFloor = table.Column<int>(type: "int", nullable: false),
                    MinArea = table.Column<float>(type: "real", nullable: false),
                    MaxArea = table.Column<float>(type: "real", nullable: false),
                    MinRooms = table.Column<int>(type: "int", nullable: false),
                    MaxRooms = table.Column<int>(type: "int", nullable: false),
                    RequirementId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentRequirements", x => x.Id);
                    table.CheckConstraint("CK_ApartmentRequirement_MaxArea", "MaxArea >= 0");
                    table.CheckConstraint("CK_ApartmentRequirement_MaxFloor", "MaxFloor >= 1");
                    table.CheckConstraint("CK_ApartmentRequirement_MaxRooms", "MaxRooms >= 0");
                    table.CheckConstraint("CK_ApartmentRequirement_MinArea", "MinArea >= 0");
                    table.CheckConstraint("CK_ApartmentRequirement_MinFloor", "MinFloor >= 1");
                    table.CheckConstraint("CK_ApartmentRequirement_MinRooms", "MinRooms >= 0");
                    table.ForeignKey(
                        name: "FK_ApartmentRequirements_Requirements_RequirementId",
                        column: x => x.RequirementId,
                        principalTable: "Requirements",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HouseRequirements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinFloors = table.Column<int>(type: "int", nullable: false),
                    MaxFloors = table.Column<int>(type: "int", nullable: false),
                    MinArea = table.Column<float>(type: "real", nullable: false),
                    MaxArea = table.Column<float>(type: "real", nullable: false),
                    MinRooms = table.Column<int>(type: "int", nullable: false),
                    MaxRooms = table.Column<int>(type: "int", nullable: false),
                    RequirementId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseRequirements", x => x.Id);
                    table.CheckConstraint("CK_HouseRequirement_MaxArea", "MaxArea >= 0");
                    table.CheckConstraint("CK_HouseRequirement_MaxFloors", "MaxFloors >= 1");
                    table.CheckConstraint("CK_HouseRequirement_MaxRooms", "MaxRooms >= 0");
                    table.CheckConstraint("CK_HouseRequirement_MinArea", "MinArea >= 0");
                    table.CheckConstraint("CK_HouseRequirement_MinFloors", "MinFloors >= 1");
                    table.CheckConstraint("CK_HouseRequirement_MinRooms", "MinRooms >= 0");
                    table.ForeignKey(
                        name: "FK_HouseRequirements_Requirements_RequirementId",
                        column: x => x.RequirementId,
                        principalTable: "Requirements",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LandRequirements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinArea = table.Column<float>(type: "real", nullable: false),
                    MaxArea = table.Column<float>(type: "real", nullable: false),
                    RequirementId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandRequirements", x => x.Id);
                    table.CheckConstraint("CK_LandRequirement_MaxArea", "MaxArea >= 0");
                    table.CheckConstraint("CK_LandRequirement_MinArea", "MinArea >= 0");
                    table.ForeignKey(
                        name: "FK_LandRequirements_Requirements_RequirementId",
                        column: x => x.RequirementId,
                        principalTable: "Requirements",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Deals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequirementId = table.Column<int>(type: "int", nullable: false),
                    OfferId = table.Column<int>(type: "int", nullable: false),
                    CompanyDeduction = table.Column<float>(type: "real", nullable: false),
                    BuyerRealtorDeduction = table.Column<float>(type: "real", nullable: false),
                    SellerRealtorDeduction = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deals", x => x.Id);
                    table.CheckConstraint("CK_Deal_BuyerRealtorDeduction", "BuyerRealtorDeduction BETWEEN 0 AND 99");
                    table.CheckConstraint("CK_Deal_SellerRealtorDeduction", "SellerRealtorDeduction BETWEEN 0 AND 99");
                    table.ForeignKey(
                        name: "FK_Deals_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Deals_Requirements_RequirementId",
                        column: x => x.RequirementId,
                        principalTable: "Requirements",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentRequirements_RequirementId",
                table: "ApartmentRequirements",
                column: "RequirementId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deals_OfferId",
                table: "Deals",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_RequirementId",
                table: "Deals",
                column: "RequirementId");

            migrationBuilder.CreateIndex(
                name: "IX_HouseRequirements_RequirementId",
                table: "HouseRequirements",
                column: "RequirementId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LandRequirements_RequirementId",
                table: "LandRequirements",
                column: "RequirementId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offers_ClientId",
                table: "Offers",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_PropertyObjectId",
                table: "Offers",
                column: "PropertyObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_RealtorId",
                table: "Offers",
                column: "RealtorId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyObjects_DistrictId",
                table: "PropertyObjects",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyObjects_PropertyTypeId",
                table: "PropertyObjects",
                column: "PropertyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Requirements_ClientId",
                table: "Requirements",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Requirements_DistrictId",
                table: "Requirements",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Requirements_PropertyTypeId",
                table: "Requirements",
                column: "PropertyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Requirements_RealtorId",
                table: "Requirements",
                column: "RealtorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApartmentRequirements");

            migrationBuilder.DropTable(
                name: "Deals");

            migrationBuilder.DropTable(
                name: "HouseRequirements");

            migrationBuilder.DropTable(
                name: "LandRequirements");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "Requirements");

            migrationBuilder.DropTable(
                name: "PropertyObjects");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Realtors");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "PropertyTypes");
        }
    }
}
