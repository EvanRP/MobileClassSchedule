using MobileClassSchedule.Classes;
using Microsoft.Maui.ApplicationModel.Communication;

namespace MobileClassSchedule.Pages;

public partial class CoursePage : ContentPage
{
	int sId;
	Course currentCourse;
	int allCoursesPage;
	int currentTermId;
	public CoursePage(int courseId, int returnPage, int studentId, int termId)
	{
		InitializeComponent();
		allCoursesPage = returnPage;
        sId = studentId;
		currentTermId = termId;
        SQL db = new();
		currentCourse = new Course();
		currentCourse = db.getdbConnection().Table<Course>().FirstOrDefault(c=>c.id == courseId);
		Instructor instruct = new();
		int iId = int.Parse(currentCourse.instructor);
		
        instruct = db.getdbConnection().Table<Instructor>().FirstOrDefault(inst=>inst.id == iId);

		// fill course status picker
		List<string>pickerlist = new();
		pickerlist.Add("enrolled");
		pickerlist.Add("completed");
		pickerlist.Add("dropped");
		pickerlist.Add("failed");
		status.ItemsSource = pickerlist;

        // concatenate instructors name
        string iName = instruct.fName;
		iName += " ";
		iName += instruct.lName;

		// set page title to course code and name
		this.Title = currentCourse.code + " " + currentCourse.title;
        Save.BackgroundColor = Color.FromRgb(65, 105, 225);

		// setup validation
		instructorName.TextChanged += enableSave;
		instructorEmail.TextChanged += enableSave;
		instructorPhone.TextChanged += enableSave;

        instructorPhone.Placeholder = "Only accepts numbers";
        instructorEmail.Placeholder = "Must have '@' and a '.'";


  //      if (instruct.email.Length == 0 && instruct.fName.Length > 0)
		//{
		//	string eMail = "";
		//	eMail += instruct.fName[0];
		//	eMail += instruct.lName;
		//	eMail += "@School.edu";
		//	instruct.email = eMail;
		//}
		//else if (instruct.email.Length == 0 && instruct.fName.Length !> 0)
  //          {
  //              string eMail = "";

  //              eMail += instruct.lName;
  //              eMail += "@School.edu";
  //              instruct.email = eMail;
  //          }
		
		//if (instruct.phone.Length == 0)
		//{
  //          instruct.phone = "Enter phone #";
		//}

		// fill the fields
		if (currentCourse.status.Length > 0)
		{
			status.SelectedIndex = status.Items.IndexOf(currentCourse.status);
		}
		instructorName.Text = iName;
		instructorPhone.Text = instruct.phone;
        instructorEmail.Text = instruct.email;
		Notes.Text = currentCourse.notes;
		courseIdLabel.Text = currentCourse.id.ToString();
        
    }

	public void saveClicked(object sender, EventArgs e)
	{
		
		// Get varaiables 
		string[] name = instructorName.Text.Split(' ');
		string fname = name[0];
		string lname = name[1];
		SQL db = new();
		Instructor instruct = new();
		Course course = new();

		// compare dates
		DateTime start = startDate.Date;
		DateTime end = endDate.Date;
        int dTCompare = DateTime.Compare(start, end);
        if (dTCompare > 0)
        {
            getError();
            return;
        }

        int cid = int.Parse(courseIdLabel.Text);
		course = db.getdbConnection().Table<Course>().FirstOrDefault(c=>c.id == cid);
		int iId = int.Parse(course.instructor);
		instruct = db.getdbConnection().Table<Instructor>().FirstOrDefault(ins => ins.id == iId);

        if (status.SelectedItem != null)
        {
            string statusSelected = status.Items[status.SelectedIndex];
            course.status = statusSelected;
        }
        // set new values


        course.start = startDate.Date.ToString("yyyy-MM-dd");
		course.end = endDate.Date.ToString("yyyy-MM-dd");
		
		instruct.fName = name[0];
		instruct.lName = name[1];
		instruct.email = instructorEmail.Text;
		instruct.phone = instructorPhone.Text;
		course.notes = Notes.Text;

		// update DB
		db.getdbConnection().Update(instruct);
		db.getdbConnection().Update(course);
		TabbedPage allCourses = new AllCourses(sId);
		allCourses.CurrentPage = allCourses.Children[allCoursesPage];
		
		
	}
	public async void deleteClicked(object sender, EventArgs e)
	{
		// get conformation

		bool result = await DisplayAlert("Confimation", "Are you sure you want to delete this course?", "Yes", "No");
		if (!result)
		{
			return;
		}

        SQL db = new();
		Term term = db.getdbConnection().Table<Term>().FirstOrDefault(t => t.id == currentTermId);
		Course course = currentCourse;
        
		// remove course id from term
		string[] courseIds = term.courses.Split(',');
		term.courses = "";
		foreach(string s in courseIds)
		{
			if (s != course.id.ToString())
			{
                if (term.courses.Length == 0)
                {
                    term.courses = s;
                }
				else
				{
					string newS = "," + s;
					term.courses += newS;
				}
            }
		}
		// delete course from db and update term
		db.getdbConnection().Delete(course);
		db.getdbConnection().Update(term);

		TabbedPage allCourses = new AllCourses(sId);
		allCourses.CurrentPage = allCourses.Children[allCoursesPage];
		await Navigation.PushAsync(allCourses);
		Navigation.RemovePage(this);

    }
	public void assessmentClicked(object sender, EventArgs e)
	{
	
		Navigation.PushAsync(new Assessments(currentCourse.id));
	}
	public void notificationClicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new Notifications(currentCourse.id));
	}
	public async void shareClicked(object sender, EventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(Notes.Text))
		{
			await Share.RequestAsync(new ShareTextRequest
			{
				Text = Notes.Text,
				Title = "Share Notes"
			});
		}
		else
		{
			await DisplayAlert("Error", "Notes are empty.", "Ok");
		}
	}

    private async void getError()
    {
        await DisplayAlert("Oops", "Your choosen End date is before your Start date.", "Ok");
    }

	private void enableSave(object sender, EventArgs e)
	{
		bool result = !string.IsNullOrWhiteSpace(instructorName.Text)&&
			instructorName.Text.Contains(" ")&&
			!string.IsNullOrWhiteSpace(instructorEmail.Text)&&
			instructorEmail.Text.Contains("@")&&
			instructorEmail.Text.Contains(".");
		
		
		bool other = false;
        if (!string.IsNullOrWhiteSpace(instructorPhone.Text))
        {
            string[] ph = instructorPhone.Text.Split('-');

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


        if (result && other)
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