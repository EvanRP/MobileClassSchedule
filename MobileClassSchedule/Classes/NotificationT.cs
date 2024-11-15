using SQLite;

namespace MobileClassSchedule.Classes
{
    public class NotificationT
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Date { get; set; }


        public NotificationT()
        {
            this.Title = "";
            this.Message = "";
            this.Date = "";
        }

        public NotificationT(string nTitle, string nMessage, string nDate)
        {
            this.Title = nTitle;
            this.Message = nMessage;
            this.Date = nDate;
        }
    }
}
