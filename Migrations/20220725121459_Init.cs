using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyStudentMCVApp.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    HoursPerWeek = table.Column<int>(type: "int", nullable: true),
                    FeeBase = table.Column<decimal>(type: "decimal(6,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Course__A25C5AA6502681CB", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employee_Role",
                columns: table => new
                {
                    Employee_Id = table.Column<int>(type: "int", nullable: false),
                    Role_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee_Role", x => new { x.Employee_Id, x.Role_Id });
                    table.ForeignKey(
                        name: "FK_ToEmployee",
                        column: x => x.Employee_Id,
                        principalTable: "Employee",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ToRole",
                        column: x => x.Role_Id,
                        principalTable: "Role",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AcademicRecord",
                columns: table => new
                {
                    CourseCode = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    StudentId = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Grade = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Academic__3D052599CA81688F", x => new { x.StudentId, x.CourseCode });
                    table.ForeignKey(
                        name: "FK_AcademicRecord_Course",
                        column: x => x.CourseCode,
                        principalTable: "Course",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_AcademicRecord_Student",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Registration",
                columns: table => new
                {
                    Course_CourseID = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Student_StudentNum = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Registra__92ECCCE9B9BA1538", x => new { x.Course_CourseID, x.Student_StudentNum });
                    table.ForeignKey(
                        name: "FK_Registration_ToCourse",
                        column: x => x.Course_CourseID,
                        principalTable: "Course",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_Registration_ToStudent",
                        column: x => x.Student_StudentNum,
                        principalTable: "Student",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicRecord_CourseCode",
                table: "AcademicRecord",
                column: "CourseCode");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Role_Role_Id",
                table: "Employee_Role",
                column: "Role_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Registration_Student_StudentNum",
                table: "Registration",
                column: "Student_StudentNum");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcademicRecord");

            migrationBuilder.DropTable(
                name: "Employee_Role");

            migrationBuilder.DropTable(
                name: "Registration");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "Student");
        }
    }
}
