using MobileClassSchedule.Classes;

namespace MobileClassSchedule.Pages;

public partial class EditAssessment : ContentPage
{
    int cId;
    char atype;
    int aId;
    public EditAssessment(int courseId, char assessType, int assessID)
	{
		InitializeComponent();
        SQL db = new();

        cId = courseId;
        atype = assessType;
        aId = assessID;
        List<string> pickerList = new List<string>();
        Assessment assess = new Assessment();
        Course c = db.getdbConnection().Table<Course>().FirstOrDefault(c=>c.id==cId);
        assess = db.getdbConnection().Table<Assessment>().FirstOrDefault(a => a.id == aId);

        // fill picker based on what assessments course already has

        if(c.assessments.Length < 2)
        {
            pickerList.Add("Performance");
            pickerList.Add("Objective");
        }
        else
        {
            if (atype == 'O')
            {
                pickerList.Add("Objective");
            }
            else if (atype == 'P')
            {
                pickerList.Add("Performance");
            }
        }

        // fill the fields

        selectorType.ItemsSource = pickerList;
        int index = pickerList.IndexOf(assess.type);
        selectorType.SelectedItem = selectorType.Items[index];
        title.Text = assess.title;
        due.Date = DateTime.Parse(assess.dueDate);

        // setup validation
        Save.BackgroundColor = Color.FromRgb(65, 105, 225);
        title.TextChanged += enableSave;
        selectorType.SelectedIndexChanged += enableSave;

    }

    private void saveClicked(object sender, EventArgs e)
    {
        SQL db = new SQL();
        Assessment assess = db.getdbConnection().Table<Assessment>().FirstOrDefault(a => a.id == aId);

        // update assessment

        assess.title = title.Text;
        assess.type = selectorType.Items[selectorType.SelectedIndex];
       
        assess.dueDate = due.Date.ToString("yyyy-MM-dd");

        db.getdbConnection().Update(assess);
        
        
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
            selectorType.SelectedItem != null;

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