using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.System.Migration.SQLite.Migrations
{
    /// <inheritdoc />
    public partial class nitialreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()"),
                    update_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    email = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    password_hash = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    first_name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    last_name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    role = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "Student"),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()"),
                    update_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "course",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    title = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    description = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    category_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    instructor_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    is_published = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()"),
                    update_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course", x => x.id);
                    table.ForeignKey(
                        name: "FK_course_category_category_id",
                        column: x => x.category_id,
                        principalTable: "category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_user_instructor_id",
                        column: x => x.instructor_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "assignment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    title = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    description = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    enrollement_date = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValue: new DateTime(2025, 8, 5, 20, 43, 8, 713, DateTimeKind.Utc).AddTicks(9408)),
                    course_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    assignment_type = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "Text"),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()"),
                    update_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assignment", x => x.id);
                    table.ForeignKey(
                        name: "FK_assignment_course_course_id",
                        column: x => x.course_id,
                        principalTable: "course",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "enrollement",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    enrollement_date = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValue: new DateTime(2025, 8, 5, 20, 43, 8, 741, DateTimeKind.Utc).AddTicks(9541)),
                    is_completed = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    student_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    course_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()"),
                    update_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enrollement", x => x.id);
                    table.ForeignKey(
                        name: "FK_enrollement_course_course_id",
                        column: x => x.course_id,
                        principalTable: "course",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_enrollement_user_student_id",
                        column: x => x.student_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lesson",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    title = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    content = table.Column<string>(type: "TEXT", nullable: false),
                    course_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    order = table.Column<int>(type: "INTEGER", nullable: false),
                    lesson_type = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "Text"),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()"),
                    update_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lesson", x => x.id);
                    table.ForeignKey(
                        name: "FK_lesson_course_course_id",
                        column: x => x.course_id,
                        principalTable: "course",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "submission",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    answer_text = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    file_path = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    grade = table.Column<byte>(type: "INTEGER", nullable: false),
                    feedback = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    submitted_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValue: new DateTime(2025, 8, 5, 20, 43, 8, 732, DateTimeKind.Utc).AddTicks(3114)),
                    assignment_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    student_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()"),
                    update_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_submission", x => x.id);
                    table.ForeignKey(
                        name: "FK_submission_assignment_assignment_id",
                        column: x => x.assignment_id,
                        principalTable: "assignment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_submission_user_student_id",
                        column: x => x.student_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "test_question",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    question_text = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    question_type = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "Single Choice"),
                    assignment_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()"),
                    update_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_test_question", x => x.id);
                    table.ForeignKey(
                        name: "FK_test_question_assignment_assignment_id",
                        column: x => x.assignment_id,
                        principalTable: "assignment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "test_answer_option",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    option_text = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    is_correct = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    test_question_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()"),
                    update_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_test_answer_option", x => x.id);
                    table.ForeignKey(
                        name: "FK_test_answer_option_test_question_test_question_id",
                        column: x => x.test_question_id,
                        principalTable: "test_question",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "test_submission",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    test_question_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    submission_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    selected_option_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()"),
                    update_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_test_submission", x => x.id);
                    table.ForeignKey(
                        name: "FK_test_submission_submission_submission_id",
                        column: x => x.submission_id,
                        principalTable: "submission",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_test_submission_test_answer_option_selected_option_id",
                        column: x => x.selected_option_id,
                        principalTable: "test_answer_option",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_test_submission_test_question_test_question_id",
                        column: x => x.test_question_id,
                        principalTable: "test_question",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_assignment_course_id",
                table: "assignment",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_assignment_title",
                table: "assignment",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_category_name",
                table: "category",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_course_category_id",
                table: "course",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_instructor_id",
                table: "course",
                column: "instructor_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_title",
                table: "course",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_enrollement_course_id",
                table: "enrollement",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_enrollement_student_id",
                table: "enrollement",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_course_id",
                table: "lesson",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_title",
                table: "lesson",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_submission_assignment_id",
                table: "submission",
                column: "assignment_id");

            migrationBuilder.CreateIndex(
                name: "IX_submission_student_id",
                table: "submission",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_test_answer_option_test_question_id",
                table: "test_answer_option",
                column: "test_question_id");

            migrationBuilder.CreateIndex(
                name: "IX_test_question_assignment_id",
                table: "test_question",
                column: "assignment_id");

            migrationBuilder.CreateIndex(
                name: "IX_test_submission_selected_option_id",
                table: "test_submission",
                column: "selected_option_id");

            migrationBuilder.CreateIndex(
                name: "IX_test_submission_submission_id",
                table: "test_submission",
                column: "submission_id");

            migrationBuilder.CreateIndex(
                name: "IX_test_submission_test_question_id",
                table: "test_submission",
                column: "test_question_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_email",
                table: "user",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "enrollement");

            migrationBuilder.DropTable(
                name: "lesson");

            migrationBuilder.DropTable(
                name: "test_submission");

            migrationBuilder.DropTable(
                name: "submission");

            migrationBuilder.DropTable(
                name: "test_answer_option");

            migrationBuilder.DropTable(
                name: "test_question");

            migrationBuilder.DropTable(
                name: "assignment");

            migrationBuilder.DropTable(
                name: "course");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
