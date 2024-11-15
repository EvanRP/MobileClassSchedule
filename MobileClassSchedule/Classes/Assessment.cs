using SQLite;

namespace MobileClassSchedule.Classes
{
    internal class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string dueDate { get; set; }
        public string notifications { get; set; }

        public Assessment()
        {
            this.title = "";
            this.type = "";
            this.dueDate = "";
            this.notifications = "";

        }
        public Assessment(string aTitle, string atype, string adueDate)
        {
            this.title = aTitle;
            this.type = atype;
            this.dueDate = adueDate;

        }
        public Assessment(string aTitle, string atyoe, string adueDate, string anotification)
        {
            this.title = aTitle;
            this.type = atyoe;
            this.dueDate = adueDate;
            this.notifications = anotification;

        }
    }
}
