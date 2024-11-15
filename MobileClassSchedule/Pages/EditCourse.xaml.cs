using MobileClassSchedule.Classes;

namespace MobileClassSchedule.Pages;

public partial class EditCourse : ContentPage
{
	Course currentCourse;
	Student currentStudent;
	Term currentTerm;
	int index;
	Instructor instructor;

    public EditCourse(int courseId, int pageIndex)
	{
		InitializeComponent();
        index = pageIndex;
        currentCourse = new();
        instructor = new();
        SQL db = new SQL();
		currentCourse = db.getdbConnection().Table<Course>().FirstOrDefault(cid => cid.id == courseId);
        int instructorId = currentCourse.id;
        instructor = db.getdbConnection().Table<Instructor>().FirstOrDefault(inst => inst.id == instructorId);
		currentTerm = new();
		string cId = courseId.ToString();
		currentTerm = db.getdbConnection().Table<Term>().FirstOrDefault(ter => ter.courses.Contains(cId));
		currentStudent = new();
		string tId = currentTerm.id.ToString();
        currentStudent = db.getdbConnection().Table<Student>().FirstOrDefault(stu=>stu.terms.Contains(tId));
        
        DateTime start = DateTime.Parse(currentCourse.start);
        DateTime end = DateTime.Parse(currentCourse.end);
		// get values
		string code = currentCourse.code;
		string title = currentCourse.title;
		string fName = instructor.fName;
		string lName = instructor.lName;
		// set page fields 
        CourseCode.Text = code;
		CourseName.Text = title;
		InstructorFName.Text = fName;
		InstructorLName.Text = lName;
        iPhone.Text = instructor.phone;
        iEmail.Text = instructor.email;
		startDate.Date = start;
		endDate.Date = end;

        // set up validation
        Save.BackgroundColor = Color.FromRgb(65, 105, 225);
        CourseCode.TextChanged += enableSave;
        CourseName.TextChanged += enableSave;
        InstructorLName.TextChanged += enableSave;
        InstructorFName.TextChanged += enableSave;
        iPhone.TextChanged += enableSave;
        iEmail.TextChanged += enableSave;

        iPhone.Placeholder = "Only accepts numbers";
        iEmail.Placeholder = "Must have '@' and a '.'";

    }

	public void cancelClicked(object sender, EventArgs e)
	{
		TabbedPage tabbedPage = new AllCourses(currentStudent.id);
		tabbedPage.CurrentPage = tabbedPage.Children[index];
		Navigation.PushAsync(tabbedPage);
		Navigation.RemovePage(this);
	}

	public void saveClicked(object sender, EventArgs e)
    {
		SQL db = new();

        DateTime start = startDate.Date;
        DateTime end = endDate.Date;
        int dTCompare = DateTime.Compare(start, end);
        if (dTCompare > 0)
        {
            getError();
            return;
        }

        // update course and instructor
        currentCourse.code = CourseCode.Text;
		currentCourse.title = CourseName.Text;
		currentCourse.start = startDate.Date.ToString("yyyy-MM-dd");
        currentCourse.end = endDate.Date.ToString("yyyy-MM-dd");
		instructor.fName = InstructorFName.Text;
		instructor.lName = InstructorLName.Text;

        db.getdbConnection().Update(currentCourse);
		db.getdbConnection().Update(instructor);

		TabbedPage tabbedPage = new AllCourses(currentStudent.id);
        tabbedPage.CurrentPage = tabbedPage.Children[index];
        Navigation.PushAsync(tabbedPage);
        Navigation.RemovePage(this);
    }

    private async void getError()
    {
        await DisplayAlert("Oops", "Your choosen End date is before your Start date.", "Ok");
    }

    private void enableSave(object sender, EventArgs e)
    {
        bool test = !string.IsNullOrWhiteSpace(CourseCode.Text) &&
             !string.IsNullOrWhiteSpace(CourseName.Text) &&
             !string.IsNullOrWhiteSpace(InstructorFName.Text) &&
             !string.IsNullOrWhiteSpace(InstructorLName.Text) &&
             !string.IsNullOrWhiteSpace(iPhone.Text) &&
             !string.IsNullOrWhiteSpace(iEmail.Text) &&
             iEmail.Text.Contains("@") &&
             iEmail.Text.Contains(".");


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
        if (test && other)
        {
            Save.IsEnabled = true;
            Save.BackgroundColor = Color.FromRgb(65, 105, 225);
        }
        else
        {
            Save.IsEnabled = false;
            Save.BackgroundColor = Color.FromRgb(211, 211, 211);
        }
    }
}