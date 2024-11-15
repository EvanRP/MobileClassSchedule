using SQLite;

namespace MobileClassSchedule.Classes
{
    public class SQL
    {
        
        readonly SQLiteConnection db;
        
        public static string dbPath =>
        Path.Combine(FileSystem.AppDataDirectory, "mobile.db");


        public SQLiteConnection getdbConnection(){ return db; }

        public SQL()
        {

            //this.dbPath = path;
            db = new SQLiteConnection(dbPath);
            createTable("Student");
            createTable("Term");
            createTable("Instructor");
            createTable("Course");
            createTable("Notification");
            createTable("Assessment");
        }

        public bool tableExists(string tableName)
        {
            var table = db.GetTableInfo(tableName);

            return table.Count > 0;
        }

        public void createTable(string tableName)
        {

            if (!tableExists(tableName))
            {
                switch (tableName)
                {
                    case "Student":
                        db.CreateTable<Student>();
                        Student currentStudent = new("Tester", "Testing", "1");
                        db.Insert(currentStudent);
                        break;
                    case "Course":
                        db.CreateTable<Course>();
                        Course course = new("Basic Truths", "C101", "1", "2023-12-05", "2023-06-05", "Some Notes","", "Enrolled");
                        db.Insert(course);
                        break;
                    case "Instructor":
                        db.CreateTable<Instructor>();
                        Instructor instructor = new("Anika", "Patel", "anika.patel@strimeuniversity.edu", "555-123-4567");
                        db.Insert(instructor);
                        break;
                    case "Term":
                        db.CreateTable<Term>();
                        Term term = new("Term 1", "2023-12-01", "2024-06-01", "1");
                        db.Insert(term);
                        break;
                    case "Notification":
                        db.CreateTable<NotificationT>();
                        break;
                    case "Assessment":
                        db.CreateTable<Assessment>();
                        break;
                }

            }
        }
    }
}
