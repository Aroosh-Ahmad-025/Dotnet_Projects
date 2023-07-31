using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LoginFinal.Migrations
{
    public partial class FirstInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Experience",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    updatedat = table.Column<DateTime>(nullable: true),
                    Deletedat = table.Column<DateTime>(nullable: true),
                    FromDate = table.Column<string>(nullable: true),
                    ToDate = table.Column<string>(nullable: true),
                    Organization = table.Column<string>(nullable: true),
                    Designation = table.Column<string>(nullable: true),
                    Organization_Reference = table.Column<string>(nullable: true),
                    ReferalContact = table.Column<string>(nullable: true),
                    userid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experience", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Contact_No = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    IsActive = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InternalReviews",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sellerreview = table.Column<string>(nullable: true),
                    Stars = table.Column<int>(nullable: true),
                    customerid = table.Column<string>(nullable: true),
                    IsActive = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    Sellerid = table.Column<string>(nullable: true),
                    Orderid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalReviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsRead = table.Column<int>(nullable: true),
                    IsActive = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<string>(nullable: true),
                    DeletedAt = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    Senderid = table.Column<int>(nullable: true),
                    Requestid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Testorders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderTitle = table.Column<string>(nullable: true),
                    OrderDescription = table.Column<string>(nullable: true),
                    IsActive = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    BuyerId = table.Column<int>(nullable: true),
                    SellerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Testorders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(355)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    DOB = table.Column<DateTime>(nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Organization = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    Language = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Experience_From = table.Column<DateTime>(nullable: true),
                    Experience_To = table.Column<DateTime>(nullable: true),
                    Company = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(8)", nullable: true),
                    Status = table.Column<int>(nullable: true),
                    Availability = table.Column<int>(nullable: false),
                    Role = table.Column<int>(nullable: true),
                    IsActive = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    Refferal_Code = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Reffered_By = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    GoogleLink = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    GitHubLink = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    StackOverFlowLink = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    DribbleLink = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    VimeoLink = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    FacebookLink = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    TwitterLink = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Acc_Deactive_Reason = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Stars = table.Column<int>(nullable: true),
                    StartingFrom = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    ConnectionId = table.Column<string>(nullable: true),
                    StripeId = table.Column<string>(nullable: true),
                    Account_Verification_Link = table.Column<string>(nullable: true),
                    Is_Verified = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "butlerprefertime",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<string>(nullable: true),
                    IsActive = table.Column<int>(nullable: false),
                    UpdatedAt = table.Column<string>(nullable: true),
                    DeletedAt = table.Column<string>(nullable: true),
                    PreferTime = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_butlerprefertime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_butlerprefertime_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Education",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DegreeName = table.Column<string>(nullable: true),
                    InstituteName = table.Column<string>(nullable: true),
                    IsActive = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Education", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Education_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Logging",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<int>(nullable: true),
                    SearchKeyword = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    ProfileId = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logging", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logging_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message_Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<int>(nullable: false),
                    IsRead = table.Column<int>(nullable: false),
                    FilePath = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    SenderId = table.Column<int>(nullable: false),
                    RecieverId = table.Column<int>(nullable: false),
                    OrderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Users_RecieverId",
                        column: x => x.RecieverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderReviews",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuyerComment = table.Column<string>(nullable: true),
                    SellerComment = table.Column<string>(nullable: true),
                    Stars = table.Column<int>(nullable: false),
                    FilePath = table.Column<string>(nullable: true),
                    IsActive = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    OrdId = table.Column<int>(nullable: false),
                    CommentId = table.Column<int>(nullable: false),
                    CommentUser = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderReviews_Users_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderReviews_Users_CommentUser",
                        column: x => x.CommentUser,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderTitle = table.Column<string>(nullable: true),
                    OrderDescription = table.Column<string>(nullable: true),
                    Cancellation_Reason = table.Column<string>(nullable: true),
                    Extending_Reason = table.Column<string>(nullable: true),
                    Revision_Reason = table.Column<string>(nullable: true),
                    OrderPrice = table.Column<long>(nullable: false),
                    IsAccepted = table.Column<int>(nullable: false),
                    requestedSessions = table.Column<int>(nullable: false),
                    Charge = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    paidBy = table.Column<int>(nullable: false),
                    IsActive = table.Column<int>(nullable: false),
                    IsDelivered = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    BuyerId = table.Column<int>(nullable: false),
                    SellerId = table.Column<int>(nullable: false),
                    BuyerCommentsCount = table.Column<string>(nullable: true),
                    SellerCommentsCount = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Users_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Refferals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefferalCode = table.Column<string>(nullable: true),
                    IsActive = table.Column<int>(nullable: true),
                    RefferalType = table.Column<int>(nullable: false),
                    ReferredUserType = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    ReferralEndTime = table.Column<DateTime>(nullable: true),
                    RefferedId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refferals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Refferals_Users_RefferedId",
                        column: x => x.RefferedId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Requesthelp",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Isactive = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    budget = table.Column<string>(nullable: true),
                    FromDateTime = table.Column<DateTime>(nullable: true),
                    ToDateTime = table.Column<DateTime>(nullable: true),
                    skills = table.Column<string>(nullable: true),
                    Tags = table.Column<string>(nullable: true),
                    Zipcodes = table.Column<string>(nullable: true),
                    servicetime = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    userid = table.Column<int>(nullable: false),
                    EmailToButlers = table.Column<string>(nullable: true),
                    EmailToCustomer = table.Column<int>(nullable: false),
                    RequestType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requesthelp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requesthelp_Users_userid",
                        column: x => x.userid,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkillName = table.Column<string>(nullable: true),
                    IsActive = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuyerId = table.Column<int>(nullable: false),
                    SellerId = table.Column<int>(nullable: false),
                    IsSubscribed = table.Column<int>(nullable: false),
                    SessionCount = table.Column<int>(nullable: false),
                    subtractedSessions = table.Column<int>(nullable: false),
                    SubscriptionStart = table.Column<DateTime>(nullable: false),
                    SubscriptionEnd = table.Column<DateTime>(nullable: false),
                    SubscriptionPrice = table.Column<long>(nullable: false),
                    SubscriptionCharge = table.Column<string>(nullable: true),
                    subscriptionType = table.Column<int>(nullable: true),
                    IsActive = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Users_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagName = table.Column<string>(nullable: true),
                    IsActive = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserLanguage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<string>(nullable: true),
                    IsActive = table.Column<int>(nullable: false),
                    UpdatedAt = table.Column<int>(nullable: false),
                    language = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLanguage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLanguage_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "preferservicetimes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<string>(nullable: true),
                    IsActive = table.Column<int>(nullable: false),
                    UpdatedAt = table.Column<string>(nullable: true),
                    DeletedAt = table.Column<string>(nullable: true),
                    servicetime = table.Column<string>(nullable: true),
                    requestid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_preferservicetimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_preferservicetimes_Requesthelp_requestid",
                        column: x => x.requestid,
                        principalTable: "Requesthelp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestSkills",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Skillcategory = table.Column<string>(nullable: true),
                    Requested_SkillName = table.Column<string>(nullable: true),
                    subcategoryissue = table.Column<string>(nullable: true),
                    IsActive = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    Requestid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestSkills_Requesthelp_Requestid",
                        column: x => x.Requestid,
                        principalTable: "Requesthelp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestTags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Requested_TagName = table.Column<string>(nullable: true),
                    IsActive = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    Requestid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestTags_Requesthelp_Requestid",
                        column: x => x.Requestid,
                        principalTable: "Requesthelp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_butlerprefertime_UserId",
                table: "butlerprefertime",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Education_UserId",
                table: "Education",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Logging_UserId",
                table: "Logging",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RecieverId",
                table: "Messages",
                column: "RecieverId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderReviews_CommentId",
                table: "OrderReviews",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderReviews_CommentUser",
                table: "OrderReviews",
                column: "CommentUser");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BuyerId",
                table: "Orders",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SellerId",
                table: "Orders",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_preferservicetimes_requestid",
                table: "preferservicetimes",
                column: "requestid");

            migrationBuilder.CreateIndex(
                name: "IX_Refferals_RefferedId",
                table: "Refferals",
                column: "RefferedId");

            migrationBuilder.CreateIndex(
                name: "IX_Requesthelp_userid",
                table: "Requesthelp",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_RequestSkills_Requestid",
                table: "RequestSkills",
                column: "Requestid");

            migrationBuilder.CreateIndex(
                name: "IX_RequestTags_Requestid",
                table: "RequestTags",
                column: "Requestid");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_UserId",
                table: "Skills",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_BuyerId",
                table: "Subscriptions",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_UserId",
                table: "Tags",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLanguage_UserId",
                table: "UserLanguage",
                column: "UserId");


            var GetTableCount = @"Alter PROCEDURE [dbo].[GetTableCount]

                        @TblName varchar(50) = ''

                        AS
                        
								BEGIN     

									  SELECT TotalCount= SUM (row_count) FROM sys.dm_db_partition_stats WHERE object_id = OBJECT_ID('dbo.'+@TblName+'') AND (index_id=0 or index_id=1)
							  
								END";

            var getAllRecords = @"Alter PROCEDURE [dbo].[GetAllRecords]
                        
                        @TblName varchar(50) = '',

						@EntityName varchar(50) ='',

                        @EntityValue varchar(50) = '-1',

						@Join varchar(MAX) ='',

						@IsActive varchar(50) ='0',

                        @row varchar(20) = '0',

                        @ofset varchar(20) = '0',

                        @SortType varchar(50) = 'Id',

                        @SortDirection varchar(50) = '',

						@count int = 0

                        AS
                        
						BEGIN     

                            DECLARE @SQL NVARCHAR(MAX)

                            SELECT @SQL = COALESCE(@SQL +','  ,' ') + name from sys.columns where name not in ('UpdatedAt','DeletedAt') and object_id = (Select id from sysobjects where name = @TblName)


                            --Get Record By Entity Value
                            IF(@EntityValue!='-1')

                                    BEGIN

                                    SELECT @SQL = 'SELECT ' + @SQL + ' FROM ' +@TblName+' WITH (NOLOCK) WHERE '+@EntityName+'=' + @EntityValue + ' AND IsActive <> 0 Order By '+@SortType+' '+@SortDirection+''

									EXEC (@SQL)

									SELECT @count = @@ROWCOUNT

									return @count

                                    END

							

							ELSE IF(@Join != '')

									BEGIN 

										 SELECT @SQL = 'SELECT '+@SQL+' FROM ' +@TblName+' WITH (NOLOCK) '+@Join+''

										 EXEC (@SQL)

									END

                            ELSE IF(@row!='0')
                                    
                                    BEGIN

                                    SELECT @SQL = 'SELECT ' + @SQL + ' FROM '+@TblName+' WITH (NOLOCK) WHERE IsActive <> 0 Order By '+@SortType+' '+@SortDirection+' OFFSET '+@ofset+' ROWS FETCH NEXT '+@row+' ROWS ONLY'
 
									EXEC (@SQL)

                                    END

							ELSE IF(@IsActive!='0')


								BEGIN 
										SELECT @SQL = 'SELECT ' + @SQL + ' FROM '+@TblName+' WITH (NOLOCK) WHERE IsActive ='+@IsActive+''
										
										EXEC (@SQL)
							
								END


                            ELSE

                                    BEGIN 

                                        SELECT @SQL = 'SELECT ' + @SQL + ' FROM '+@TblName+' WITH (NOLOCK) WHERE IsActive <> 0'
										
										EXEC (@SQL)
                                    END



                        END";

            var Insert_Update = @"Alter PROCEDURE [dbo].[InsertOrUpdate]
                    
                     @Id varchar(MAX) = '0',

                     @TblName varchar(MAX)='',

                     @Columns varchar(MAX) = '0',

                     @Values varchar(MAX) = ''

                        AS
                        
                         Declare @insert NVARCHAR(MAX) ='Insert Into '+@TblName+' ('+@Columns+') Values ('+@Values+')'
                        
                         Declare @update NVARCHAR(MAX) ='UPDATE '+@TblName+' SET '+@Columns+' where Id = '+@Id;

						 
                        BEGIN
                  
                    IF(@Id = 0)

                     BEGIN

					
					 EXEC(@insert)
					 
					
                      --EXECUTE sp_executesql @insert
					  SELECT Id= @@IDENTITY

		             END

                ELSE


                BEGIN

				 EXEC(@update)
                    --EXECUTE sp_executesql @update

                END

               
                END";

            migrationBuilder.Sql(GetTableCount);
            migrationBuilder.Sql(Insert_Update);
            migrationBuilder.Sql(getAllRecords);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "butlerprefertime");

            migrationBuilder.DropTable(
                name: "Education");

            migrationBuilder.DropTable(
                name: "Experience");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "InternalReviews");

            migrationBuilder.DropTable(
                name: "Logging");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "OrderReviews");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "preferservicetimes");

            migrationBuilder.DropTable(
                name: "Refferals");

            migrationBuilder.DropTable(
                name: "RequestSkills");

            migrationBuilder.DropTable(
                name: "RequestTags");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Testorders");

            migrationBuilder.DropTable(
                name: "UserLanguage");

            migrationBuilder.DropTable(
                name: "Requesthelp");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
