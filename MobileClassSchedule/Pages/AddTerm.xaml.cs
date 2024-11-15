namespace MobileClassSchedule.Pages;
using MobileClassSchedule.Classes;
public partial class AddTerm : ContentPage
{
    public Student currentStudent { get; set; }
    SQL dbConnection = new();
    public AddTerm(Student student)
	{
        InitializeComponent();
        
        currentStudent = new Student();
        currentStudent = student;
        

        // setup input validation
        Save.IsEnabled = false;
        Save.BackgroundColor = Color.FromRgb(211, 211, 211);
        TermEntry.TextChanged += enableSave;
    }

    public void SaveClicked(object sender, EventArgs e)
    {
        // check dates

        DateTime start = StartDate.Date;
        DateTime end = EndDate.Date;
        int dTCompare = DateTime.Compare(start, end);
        if (dTCompare > 0)
        {
            getError();
            return;
        }

        // create term

        Term newTerm = new(TermEntry.Text, start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd"));
        dbConnection.getdbConnection().Insert(newTerm);
        string id = newTerm.id.ToString();

        // add term id to student
        if (currentStudent.terms.Length > 0)
        {
            id = "," + id;
        }
        currentStudent.terms += id;
        dbConnection.getdbConnection().Update(currentStudent);
       
        var main = new MainPage(currentStudent.id);
        
        Navigation.PushAsync(main);
        Navigation.RemovePage(this);
    }
    public void CancelClicked(object sender, EventArgs e)
    {
        var main = new MainPage(currentStudent.id);
        
        Navigation.PushAsync(main);
        Navigation.RemovePage(this);
    }

    private async void getError()
    {
        await DisplayAlert("Oops", "Your choosen End date is before your Start date.", "Ok");
    }
    private void enableSave(object sender, EventArgs e)
    {
        bool test = !string.IsNullOrWhiteSpace(TermEntry.Text);
        if (test)
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