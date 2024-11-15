using SQLite;


namespace MobileClassSchedule.Classes
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string title { get; set; }
        public string code { get; set; }    
        public string instructor { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string notes { get; set; }
        public string notifications { get; set; }
        public string status { get; set; }
        //public string dueDate { get; set; }
        public string assessments { get; set; }

        public Course() 
        {
            this.title = "";
            this.instructor = "";
            this.end = "";
            this.notes = "";
            this.notifications = "";
            this.status = "";
            this.start = "";
            this.code = "";
            //this.dueDate = "";
            this.assessments = "";
        }
        public Course(string courseTitle, string courseCode, string instructorId ,string start, string end)
        {
            this.title = courseTitle;
            this.code = courseCode;
            this.instructor = instructorId;
            this.start = start;
            this.end = end;
            this.notes = "";
            this.notifications = "";
            this.status = "";
            //this.dueDate = "";
            this.assessments = "";
        }


        public Course(string courseTitle, string courseCode, string instructorId, string startDate, string endDate, string notes, string notifications, string status)
        {
            this.title = courseTitle;
            this.code = courseCode;
            this.instructor = instructorId;
            this.end = endDate;
            this.notes = notes;
            this.notifications = notifications;
            this.status = status;
            this.start = startDate;
            //this.dueDate = "";
            this.assessments = "";
        }

        public Instructor GetInstructor(int instructorId)
        {
            SQL dbConnection = new SQL();

            return dbConnection.getdbConnection().Table<Instructor>().FirstOrDefault(i => i.id == instructorId, null);
        }
        public NotificationT GetNotification(int NotificationId)
        {
            SQL dbConnection = new SQL();

            return dbConnection.getdbConnection().Table<NotificationT>().FirstOrDefault(n => n.id == NotificationId, null);
        }
    }
}
