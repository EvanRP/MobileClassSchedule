using MobileClassSchedule.Classes;
using Plugin.LocalNotification;

namespace MobileClassSchedule.Pages;

public partial class Notifications : ContentPage
{
    int id;

    public Notifications(int courseId)
    {
        InitializeComponent();
        id = courseId;
        SQL db = new();
        Course core = db.getdbConnection().Table<Course>().FirstOrDefault(c => c.id == id);

        // create page items for course notification
        if(core.notifications.Length > 0)
        {
            string[] coreNotes = core.notifications.Split(",");
            Label label = new Label
            {
                Text = core.title + " Notifications:"
            };
            StackEm.Children.Add(label);
            createPage(coreNotes, 'c', core.id);
        }

        // create page items for each assessment
        if (core.assessments.Length > 0)
        {
            string[] coreAssess = core.assessments.Split(",");
            foreach (string s in coreAssess)
            {
                int aId = int.Parse(s);
                Assessment assess = db.getdbConnection().Table<Assessment>().FirstOrDefault(a=>a.id == aId);
                if (assess.notifications.Length > 0) 
                {
                    Label label = new Label
                    {
                        Text = assess.title + " Notifications:"
                    };
                    StackEm.Children.Add(label);
                    string[] assessNotes = assess.notifications.Split(",");
                    createPage(assessNotes, 'a', assess.id);
                } 
            }
        }
    }

    public void addClicked(object sender, EventArgs e)
    {
        ContentPage newNote = new AddNotification(id);
        Navigation.PushAsync(newNote);
        Navigation.RemovePage(this);
    }

    private void createPage(string[] notes, char type, int caId)
    {
        SQL db = new();

        foreach (string s in notes)
        {
            SwipeView swipe = new SwipeView();
            int newS = int.Parse(s);
            NotificationT no = new();
            no = db.getdbConnection().Table<NotificationT>().FirstOrDefault(n => n.id == newS);
            StackLayout stackIt = new StackLayout();

            Button notification = new Button()
            {
                Text = no.Title
            };

            SwipeItem deleteSwipe = new SwipeItem
            {
                Text = "Delete",
                BackgroundColor = Colors.LightPink,
                Command = new Command(() => deleteClicked(no.id, type, caId))

            };
            SwipeItem editSwipe = new SwipeItem
            {
                Text = "Edit",
                BackgroundColor = Colors.LightBlue,
                Command = new Command(() => editClicked(no.id))

            };
            swipe.RightItems.Add(deleteSwipe);
            swipe.LeftItems.Add(editSwipe);
            stackIt.Children.Add(notification);
            swipe.Content = stackIt;
            StackEm.Children.Add(swipe);

        }
    }

    public void editClicked(int nId)
    {
        Navigation.PushAsync(new EditNotification(nId, id));
        Navigation.RemovePage(this);
    }

    public void deleteClicked(int nId, char cType, int caId)
    {
        SQL db = new();

        LocalNotificationCenter.Current.Cancel(nId);

        if (cType == 'c')
        {
            Course core = db.getdbConnection().Table<Course>().First(c => c.id == caId);
            string[] notes = core.notifications.Split(",");
            
            NotificationT no = db.getdbConnection().Table<NotificationT>().FirstOrDefault(n => n.id == nId);
            if (core.notifications.Length > 0)
            {
                core.notifications = "";
                foreach (string n in notes)
                {
                    if (n != nId.ToString())
                    {
                        if (core.notifications.Length > 0)
                        {
                            string a = "," + n;
                            core.notifications += a;
                        }
                        else
                        {
                            core.notifications = n;
                        }
                    }
                }
                db.getdbConnection().Update(core);
                db.getdbConnection().Delete(no);
            }

        }
        else if (cType == 'a')
        {
            Assessment assess = db.getdbConnection().Table<Assessment>().FirstOrDefault(a => a.id == caId);
            NotificationT no = db.getdbConnection().Table<NotificationT>().FirstOrDefault(n => n.id == nId);
            string[] notes = assess.notifications.Split(",");
            

            if (assess.notifications.Length > 0)
            {
                assess.notifications = "";
                foreach (string n in notes)
                {
                    if (n != nId.ToString())
                    {
                        if (assess.notifications.Length > 0)
                        {
                            string a = "," + n;
                            assess.notifications += a;
                        }
                        else
                        {
                            assess.notifications = n;
                        }
                    }
                }
                db.getdbConnection().Update(assess);
                db.getdbConnection().Delete(no);
            }
        }
        Navigation.PushAsync(new Notifications(id));
        Navigation.RemovePage(this);
    }
}

