using MobileClassSchedule.Classes;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;


namespace MobileClassSchedule.Pages;

public partial class AddNotification : ContentPage
{
	int cId;
	
	List<Assessment> assessList;


    public AddNotification(int courseId)
	{
		cId = courseId;
        InitializeComponent();

		List<string> typeList = new List<string>();
		assessList = new List<Assessment>();
		SQL db = new();

		Course core = db.getdbConnection().Table<Course>().FirstOrDefault(c => c.id == cId);

        // fill picker list
		if (core.assessments.Length > 0)
		{
			string[] assessIds = core.assessments.Split(",");

			foreach (string a in assessIds)
			{
				int aId = int.Parse(a);
				Assessment assess = db.getdbConnection().Table<Assessment>().FirstOrDefault(a => a.id == aId);
				typeList.Add(assess.title);
				assessList.Add(assess);

			}

        }
		typeList.Add(core.title);
		typePicker.ItemsSource = typeList;

        // setup validation
        Save.IsEnabled = false;
        Save.BackgroundColor = Color.FromRgb(211, 211, 211);
        typePicker.SelectedIndexChanged += enableSave;
        titleEntry.TextChanged += enableSave;
        messageEntry.TextChanged += enableSave;
		
    }


	private void saveClicked(object sender, EventArgs e)
	{
        
        SQL db = new();
		Assessment assess = new();
		
        Course core = db.getdbConnection().Table<Course>().FirstOrDefault(c => c.id == cId);
		string selected = typePicker.Items[typePicker.SelectedIndex];
		string when = date.Date.ToString("yyyy-MM-dd") + " " + time.Time.ToString();

        // create fill and add notification to db
		NotificationT note = new();

		note.Title = titleEntry.Text;
		note.Message = messageEntry.Text;
		note.Date = when;

		db.getdbConnection().Insert(note);

		if (core.title == selected)
		{
            
			if(core.notifications.Length > 0)
			{
				string a = "," + note.id.ToString();
				core.notifications += a;
			}
			else
			{
				core.notifications = note.id.ToString();
            }
			db.getdbConnection().Update(core);
		}
		else
		{
            
			int index = typePicker.SelectedIndex;
			assess = assessList[index];

            if (assess.notifications.Length > 0)
            {
                string a = "," + note.id.ToString();
                assess.notifications += a;
            }
            else
            {
                assess.notifications = note.id.ToString();
            }
            db.getdbConnection().Update(assess);
        }
        // create notification request and add to local notification center
        NotificationRequest notificationR = new NotificationRequest
        {
            Title = note.Title,
            Description = note.Message,
            NotificationId = note.id,
            BadgeNumber = 1,
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = DateTime.Parse(note.Date)
                //NotifyTime = DateTime.Now.AddSeconds(10)
            },
            Android = new AndroidOptions
            {
                ChannelId = "default"
            }
        };

        LocalNotificationCenter.Current.Show(notificationR);
        Navigation.PushAsync(new Notifications(cId));
        Navigation.RemovePage(this);
    }

    public void cancelClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Notifications(cId)); ;
        Navigation.RemovePage(this);
    }

    private void enableSave(object sender, EventArgs e)
    {
        bool test = !string.IsNullOrWhiteSpace(titleEntry.Text) &&
            !string.IsNullOrWhiteSpace(messageEntry.Text) &&
            typePicker.SelectedItem != null;

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