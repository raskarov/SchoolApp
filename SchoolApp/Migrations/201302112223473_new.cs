namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _new : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Attendances", "Student_StudentID", "dbo.Students");
            DropForeignKey("dbo.Attendances", "Course_CourseID", "dbo.Courses");
            DropForeignKey("dbo.Attendances", "Teacher_TeacherID", "dbo.Teachers");
            DropForeignKey("dbo.Students", "Course_CourseID", "dbo.Courses");
            DropForeignKey("dbo.TeacherCourse", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.TeacherCourse", "TeacherId", "dbo.Teachers");
            DropIndex("dbo.Attendances", new[] { "Student_StudentID" });
            DropIndex("dbo.Attendances", new[] { "Course_CourseID" });
            DropIndex("dbo.Attendances", new[] { "Teacher_TeacherID" });
            DropIndex("dbo.Students", new[] { "Course_CourseID" });
            DropIndex("dbo.TeacherCourse", new[] { "CourseId" });
            DropIndex("dbo.TeacherCourse", new[] { "TeacherId" });
            CreateTable(
                "dbo.Classrooms",
                c => new
                    {
                        ClassroomID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Location = c.String(),
                        Comments = c.String(),
                    })
                .PrimaryKey(t => t.ClassroomID);
            
            DropTable("dbo.Courses");
            DropTable("dbo.Teachers");
            DropTable("dbo.Attendances");
            DropTable("dbo.Students");
            DropTable("dbo.TeacherCourse");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TeacherCourse",
                c => new
                    {
                        CourseId = c.Int(nullable: false),
                        TeacherId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CourseId, t.TeacherId });
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StudentID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 40),
                        LastName = c.String(nullable: false, maxLength: 40),
                        Course_CourseID = c.Int(),
                    })
                .PrimaryKey(t => t.StudentID);
            
            CreateTable(
                "dbo.Attendances",
                c => new
                    {
                        AttendanceID = c.Int(nullable: false, identity: true),
                        AttendanceType = c.Int(nullable: false),
                        AttendanceTaken = c.DateTime(nullable: false),
                        Student_StudentID = c.Int(),
                        Course_CourseID = c.Int(),
                        Teacher_TeacherID = c.Int(),
                    })
                .PrimaryKey(t => t.AttendanceID);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        TeacherID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 40),
                        LastName = c.String(nullable: false, maxLength: 40),
                    })
                .PrimaryKey(t => t.TeacherID);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        CourseID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        RunTimes = c.String(),
                    })
                .PrimaryKey(t => t.CourseID);
            
            DropTable("dbo.Classrooms");
            CreateIndex("dbo.TeacherCourse", "TeacherId");
            CreateIndex("dbo.TeacherCourse", "CourseId");
            CreateIndex("dbo.Students", "Course_CourseID");
            CreateIndex("dbo.Attendances", "Teacher_TeacherID");
            CreateIndex("dbo.Attendances", "Course_CourseID");
            CreateIndex("dbo.Attendances", "Student_StudentID");
            AddForeignKey("dbo.TeacherCourse", "TeacherId", "dbo.Teachers", "TeacherID", cascadeDelete: true);
            AddForeignKey("dbo.TeacherCourse", "CourseId", "dbo.Courses", "CourseID", cascadeDelete: true);
            AddForeignKey("dbo.Students", "Course_CourseID", "dbo.Courses", "CourseID");
            AddForeignKey("dbo.Attendances", "Teacher_TeacherID", "dbo.Teachers", "TeacherID");
            AddForeignKey("dbo.Attendances", "Course_CourseID", "dbo.Courses", "CourseID");
            AddForeignKey("dbo.Attendances", "Student_StudentID", "dbo.Students", "StudentID");
        }
    }
}
