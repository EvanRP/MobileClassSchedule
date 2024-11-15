using MobileClassSchedule.Classes;
namespace MobileClassSchedule.Pages;

public partial class AddAssessment : ContentPage
{
	int cId;
	char atype;
	public AddAssessment(int courseId, char assessType)
	{
        InitializeComponent();
        cId = courseId;
		atype = assessType;
        List<string> pickerList = new List<string>();

		// fill list based on what assessment course currently has 
        if (atype == 'n')
		{

			pickerList.Add("Objective");
            pickerList.Add("Performance");
        }
		else if(atype == 'O')
		{
            pickerList.Add("Performance");
        }
		else if ( atype == 'P')
		{
            pickerList.Add("Objective");
        }

        type.ItemsSource = pickerList;

		// setup validation
		Save.IsEnabled = false;
        Save.BackgroundColor = Color.FromRgb(211, 211, 211);
        title.TextChanged += enableSave;
		type.SelectedIndexChanged += enableSave;
    }

	private void saveClicked(object sender, EventArgs e)
	{
		SQL db = new SQL();

		// create assessment and add to db
		Assessment assess = new Assessment();
		assess.title = title.Text;
        assess.type = type.Items[type.SelectedIndex];
		assess.dueDate = due.Date.ToString("yyyy-MM-dd");

		db.getdbConnection().Insert(assess);
		Course core = new();
		core = db.getdbConnection().Table<Course>().FirstOrDefault(core=>core.id == cId);

		// add assessment id to course
		if (core.assessments.Length > 0)
		{
			string a = "," + assess.id.ToString();
			core.assessments += a;
		}
		else
		{
            core.assessments = assess.id.ToString();

        }
		// update course
		db.getdbConnection().Update(core);
		Navigation.PushAsync(new Assessments(cId));
		Navigation.RemovePage(this);
    }
	private void cancelClicked(object sender, EventArgs e) 
	{
		Navigation.PushAsync(new Assessments(cId));
		Navigation.RemovePage(this);
	}
    private void enableSave(object sender, EventArgs e)
    {
		bool test = !string.IsNullOrWhiteSpace(title.Text) &&
			type.SelectedItem != null;
            
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