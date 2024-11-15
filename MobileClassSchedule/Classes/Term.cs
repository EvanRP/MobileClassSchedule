using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClassSchedule.Classes
{
    public class Term
    {
        // Properties

        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string name { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string courses { get; set; }
        public bool isCurrent { get; set; }


        // Constructor
        public Term() {
            this.name = "";
            this.start = "";
            this.end = "";
            this.courses = "";

        }
        public Term(string termName, string termStart, string termEnd)
        {
            this.name = termName;
            this.start = termStart;
            this.end = termEnd;
            
        }
        public Term(string termName, string termStart, string termEnd, string termCourse)
        {
            this.name = termName;
            this.start = termStart;
            this.end = termEnd;
            this.courses = termCourse;
        }

        public Course courseLookup(int courseId)
        {
            //Term term;
            SQL dbConnection = new SQL();

            return dbConnection.getdbConnection().Table<Course>().FirstOrDefault(c => c.id == courseId, null);
        }
    }
}
