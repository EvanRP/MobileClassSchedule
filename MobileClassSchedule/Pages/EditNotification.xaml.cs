using MobileClassSchedule.Classes;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;


namespace MobileClassSchedule.Pages;

public partial class EditNotification : ContentPage
{
	int noteId;
	int cId;

	public EditNotification(int noteificationId, int courseId)
	{
		noteId = noteificationId;
		
		cId = courseId;

		InitializeComponent();

		SQL db = new();
		NotificationT note = new();
		note = db.getdbConnection().Table<NotificationT>().FirstOrDefault(n=>n.id == noteId);

        // fill the fields

		titleEntry.Text = note.Title;
		messageEntry.Text = note.Message;
		string[] dtStrings = note.Date.Split(" ");
		date.Date = DateTime.Parse(dtStrings[0]);
		time.Time = TimeSpan.Parse(dtStrings[1]);

       // setup input validation
        Save.BackgroundColor = Color.FromRgb(65, 105, 225);

        titleEntry.TextChanged += enableSave;
        messageEntry.TextChanged += enableSave;
    }

    public void saveClicked(object sender, EventArgs e)
    {
		SQL db = new();

        // remove notification id form local notification center and update notification 

        LocalNotificationCenter.Current.Cancel(noteId);
		NotificationT newNote = db.getdbConnection().Table<NotificationT>().FirstOrDefault(n => n.id == noteId);
		newNote.Title = titleEntry.Text;
		newNote.Message = messageEntry.Text;
		string dtString = date.Date.ToString("yyyy-MM-dd") + " " + time.Time.ToString();

        newNote.Date = dtString;

        // create notification request 
        NotificationRequest note = new NotificationRequest
		{
            Title = newNote.Title,
            Description = newNote.Message,
            NotificationId = newNote.id,
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = DateTime.Parse(newNote.Date)
            },
			Android = new AndroidOptions
			{
				ChannelId = "default"
			}
        };

        // add notification to db and notification request to local notification center
		db.getdbConnection().Update(newNote);
		LocalNotificationCenter.Current.Show(note);

        Navigation.PushAsync(new Notifications(cId));
        Navigation.RemovePage(this);
    }

    public void cancelClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Notifications(cId));
        Navigation.RemovePage(this);
    }

    private void enableSave(object sender, EventArgs e)
    {
        bool test = !string.IsNullOrWhiteSpace(titleEntry.Text) &&
            !String.IsNullOrWhiteSpace(messageEntry.Text);

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