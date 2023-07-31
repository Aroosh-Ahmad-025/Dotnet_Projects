using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MilkManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Packets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalQuantity = table.Column<double>(type: "float", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    Salary = table.Column<double>(type: "float", nullable: true),
                    ContactNo = table.Column<string>(name: "Contact_No", type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRegular = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SoldPackets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    PricePerPacket = table.Column<int>(type: "int", nullable: false),
                    PickupLocation = table.Column<string>(name: "Pickup_Location", type: "nvarchar(max)", nullable: true),
                    DropOffLocation = table.Column<string>(name: "DropOff_Location", type: "nvarchar(max)", nullable: true),
                    TotalDistance = table.Column<double>(name: "Total_Distance", type: "float", nullable: true),
                    PickedBy = table.Column<int>(type: "int", nullable: false),
                    SoldTo = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SoldDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SoldDateString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PacketsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoldPackets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SoldPackets_Packets_PacketsId",
                        column: x => x.PacketsId,
                        principalTable: "Packets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SoldPackets_Users_PickedBy",
                        column: x => x.PickedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SoldPackets_Users_SoldTo",
                        column: x => x.SoldTo,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SoldPackets_PacketsId",
                table: "SoldPackets",
                column: "PacketsId");

            migrationBuilder.CreateIndex(
                name: "IX_SoldPackets_PickedBy",
                table: "SoldPackets",
                column: "PickedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SoldPackets_SoldTo",
                table: "SoldPackets",
                column: "SoldTo");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SoldPackets");

            migrationBuilder.DropTable(
                name: "Packets");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
