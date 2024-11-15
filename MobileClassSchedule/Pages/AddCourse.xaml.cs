using MobileClassSchedule.Classes;

namespace MobileClassSchedule.Pages;

public partial class AddCourse : ContentPage
{
	
	Term term;
	Student student;
	int index;
	public AddCourse(int studentId, int termId, int pageIndex)
	{
		InitializeComponent();
		term = new();
		SQL db = new();
		
		//get student and term
        student = new Student();
        student = db.getdbConnection().Table<Student>().FirstOrDefault(s => s.id == studentId);
        term = db.getdbConnection().Table<Term>().FirstOrDefault(t=>t.id == termId);
		index = pageIndex;

		// setup input validation
		iPhone.Placeholder = "Only accepts numbers";
		iEmail.Placeholder = "Must have '@' and a '.'";
        Save.IsEnabled = false;
		Save.BackgroundColor = Color.FromRgb(211,211,211);
        CourseCode.TextChanged += enableSave;
		CourseName.TextChanged += enableSave;
		InstructorLName.TextChanged += enableSave;
		InstructorFName.TextChanged += enableSave;
		iPhone.TextChanged += enableSave;
		iEmail.TextChanged += enableSave;
        
    }

	public void cancelClicked(object sender, EventArgs e)
    {
		SQL db = new();

		// return to proper tab
		TabbedPage tab = new AllCourses(student.id);
		tab.CurrentPage = tab.Children[index];

        Navigation.PushAsync(tab);
		Navigation.RemovePage(this);
	}
	public void saveClicked(object sender, EventArgs e)
    {
		SQL db = new();

		// create instructor

		Instructor newInstructor = new Instructor(InstructorFName.Text, InstructorLName.Text, iEmail.Text, iPhone.Text);
        db.getdbConnection().Insert(newInstructor);
        int iId = newInstructor.id;
		
		//check dates

        DateTime start = startDate.Date;
		DateTime end = endDate.Date;
        int dTCompare = DateTime.Compare(start, end);
        if (dTCompare > 0)
        {
            getError();
            return;
        }

		// create new course

        Course newCourse = new Course(CourseName.Text, CourseCode.Text, iId.ToString(), start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd"));
        
		db.getdbConnection().Insert(newCourse);


		// insert course Id into term
		if(term.courses.Length > 0)
		{
			string id = ","+newCourse.id.ToString();
			term.courses += id;
		}
		else
		{
			term.courses = newCourse.id.ToString();
		}
		db.getdbConnection().Update(term);

		TabbedPage tab = new AllCourses(student.id);
		tab.CurrentPage = tab.Children[index];

		Navigation.PushAsync(tab);
        Navigation.RemovePage(this);
    }

    private async void getError()
    {
        await DisplayAlert("Oops", "Your choosen End date is before your Start date.", "Ok");
    }
    private void enableSave(object sender, EventArgs e)
    {
		// check if required fields are full and if email is in a proper format
		bool test = !string.IsNullOrWhiteSpace(CourseCode.Text) &&
			!string.IsNullOrWhiteSpace(CourseName.Text) &&
			!string.IsNullOrWhiteSpace(InstructorFName.Text) &&
			!string.IsNullOrWhiteSpace(InstructorLName.Text)&&
			!string.IsNullOrWhiteSpace(iPhone.Text) &&
			!string.IsNullOrWhiteSpace(iEmail.Text)&&
			iEmail.Text.Contains("@")&&
			iEmail.Text.Contains(".");

		// input validation for phone number
		bool other = false;
		if (!string.IsNullOrWhiteSpace(iPhone.Text))
		{
            string[] ph = iPhone.Text.Split('-');

            foreach (string p in ph)
            {
                if (double.TryParse(p, out _))
                {
                    other = true;
                }
                else
                {
                    other = false;
                    break;
                }
            }
        }
		
		// enable save if fields are filled properly
        if (test && other)
        {
            Save.IsEnabled = true;
            Save.BackgroundColor = Color.FromRgb(65,105,225);
        }
        else
        {
            Save.IsEnabled = false;
            Save.BackgroundColor = Color.FromRgb(211, 211, 211);
        }
    }
}