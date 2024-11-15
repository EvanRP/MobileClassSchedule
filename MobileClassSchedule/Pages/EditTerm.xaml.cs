using MobileClassSchedule.Classes;
using System.Data.Common;

namespace MobileClassSchedule.Pages;

public partial class EditTerm : ContentPage
{
	Student currentStudent;
    Term currentTerm;
	public EditTerm(Student student, Term term)
	{
        // initialize initialize initialize
        InitializeComponent();
        currentStudent = new();
        currentTerm = new();
        currentTerm = term;
        currentStudent = student;

        // convert date strings to DateTime and set view page objects
        DateTime start = DateTime.Parse(currentTerm.start);
        DateTime end = DateTime.Parse(currentTerm.end);
        TermEntry.Text = currentTerm.name;
        StartDate.Date = start;
        EndDate.Date = end;

        // setup input validation
        TermEntry.TextChanged += enableSave;
        Save.BackgroundColor = Color.FromRgb(65, 105, 225);

    }

    public void SaveClicked(object sender, EventArgs e)
    {
        SQL dbConnection = new SQL();

        // compare dates

        DateTime start = StartDate.Date;
        DateTime end = EndDate.Date;
        int dTCompare = DateTime.Compare(start, end);
        if (dTCompare > 0)
        {
            getError();
            return;
        }

        // update term
        currentTerm.start = StartDate.Date.ToString("yyyy-MM-dd");
        currentTerm.end = EndDate.Date.ToString("yyyy-MM-dd");
        currentTerm.name = TermEntry.Text;

        dbConnection.getdbConnection().Update(currentTerm);
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