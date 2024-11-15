using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClassSchedule.Classes
{
     public class Student
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string fName { get; set; }
        public string lName { get; set; }
        public string terms { get; set; }

        // Constructor

        public Student() {
            this.fName = "";
            this.lName = "";
            this.terms = "";
        }
        public Student(string StudentFName, string StudentLName)
        {
            this.fName = StudentFName;
            this.lName = StudentLName;
        }
        public Student(string StudentFName, string StudentLName, string studentsTerms)
        {
            this.fName = StudentFName;
            this.lName = StudentLName;
            this.terms = studentsTerms;
        }
        public Term termLookup(int termId)
        {
            
            SQL dbConnection = new SQL();

            return dbConnection.getdbConnection().Table<Term>().FirstOrDefault(t => t.id == termId, null);
        }
    }
}
