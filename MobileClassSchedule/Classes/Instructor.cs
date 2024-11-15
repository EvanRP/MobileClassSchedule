using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClassSchedule.Classes
{
    public class Instructor
    {
        // Properties

        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string fName { get; set; }
        public string lName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }

       

        // Constructor

        public Instructor() {
            this.fName = "";
            this.lName = "";
            this.email = "";
            this.phone = ""; 
        }
        public Instructor(string instructorFName, string instructorLName)
        {
            this.fName = instructorFName;
            this.lName = instructorLName;
            this.email = "";
            this.phone = "";

        }
        public Instructor(string instructorFName, string instructorLName, string instructorEmail, string instructorPhone)
        {
            this.fName = instructorFName;
            this.lName = instructorLName;
            this.email = instructorEmail;
            this.phone = instructorPhone;
        }
    }
}
