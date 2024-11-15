using MobileClassSchedule.Classes;
using MobileClassSchedule.Pages;
using System.Data.Common;

namespace MobileClassSchedule
{
    public partial class App : Application
    {
        public App()
        {
            // string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "mobile.db");

            InitializeComponent();
            requestPermissions();
            SQL db = new SQL();

            Student student = db.getdbConnection().Table<Student>().FirstOrDefault(s => s.fName == "Tester", null);
            int id = student.id;
            if (student != null)
            {

                var main = new MainPage(student.id);
                MainPage = new NavigationPage(main);
            }
        }
    }
}
