using MobileClassSchedule.Classes;
using Microsoft.Maui.Controls;
using System.Security.Cryptography.X509Certificates;


namespace MobileClassSchedule.Pages;

public partial class AllCourses : TabbedPage
{
	Student currentStudent;
	List<Button> buttons;
	List<Course> coursesList;
	public AllCourses(int student)
	{
        InitializeComponent();
        
        SQL db = new();
		currentStudent = new();
		currentStudent = db.getdbConnection().Table<Student>().FirstOrDefault(s=>s.id == student);
		buttons = new();
		coursesList = new();

		// Get list of terms for tabs 
		List<Term> terms = new List<Term>();
		string[] termsArray = currentStudent.terms.Split(',');
		foreach(string t in termsArray)
		{
			terms.Add(currentStudent.termLookup(int.Parse(t)));
		}

		//Create tabs
		List<ContentPage> contentPages = new();
		foreach (Term t in  terms)
		{
			contentPages.Add(makeContentPage(t));
		}
        int index = 0;
		foreach(ContentPage contentP in contentPages)
		{
			this.Children.Add(contentP);
			//Label label = contentP.FindByName<Label>("termId");
			//int i = int.Parse(label.Text);
			//Term t = terms.FirstOrDefault(t=>t.id == i);
			this.Children[this.Children.Count - 1].Title = terms[index].name;
            index++;
		}
	}

	public List<Course> getClasses(Term t)
    {
        List<Course> courses = new List<Course>();
        if (t.courses.Length > 0)
        {
            string[] cid = t.courses.Split(',');
            foreach (string c in cid)
            {
                courses.Add(t.courseLookup(int.Parse(c)));
            }
        }
		else
		{
			return null;
		}
        return courses;
    }
	public ContentPage makeContentPage(Term t)
	{
		ContentPage contentPages = new();
        List<Course> courses = new();
        StackLayout stackEm = new();
        stackEm.Spacing = 10;
        stackEm.Margin = 10;
        ContentPage c = new ContentPage()
        {
            Title = t.name,
            Content = stackEm,
        };
        Label termDates = new Label
        {
            Text = t.start + "  —  " + t.end,
            HorizontalOptions  = LayoutOptions.Center
        };
        stackEm.Children.Add(termDates);
        NavigationPage.SetHasNavigationBar(c, false);

        if (t.courses.Length > 0)
		{
            
            courses = getClasses(t);

            foreach (Course core in courses)
            {
                NavigationPage.SetHasNavigationBar(this, false);
                // Button Created
                Button button = new();
                button.Text = core.title;
                button.Command = new Command(() => courseClicked(core.id, t.id));

                // Swipe Items created
                SwipeItem deleteSwipe = new SwipeItem
                {
                    Text = "Delete",
                    BackgroundColor = Colors.LightPink,
                    Command = new Command(() => deleteClicked(t.id, button))
                };
                SwipeItem editSwipe = new SwipeItem
                {
                    Text = "Edit",
                    BackgroundColor = Colors.LightBlue,
                    Command = new Command(() => editClicked(button))
                };

                // create swipe view and add to page
                SwipeView swipe = new();
                swipe.RightItems.Add(deleteSwipe);
                swipe.LeftItems.Add(editSwipe);
                StackLayout stackIt = new();
                stackIt.Add(button);
                swipe.Content = stackIt;
                stackEm.Children.Add(swipe);

                // fill list of buttons ans courses
                buttons.Add(button);
                coursesList.Add(core);
            }
        }
        
        Label termId = new Label()
        {
            Text = t.id.ToString(),
            IsVisible = false
        };
        Button addButton = new Button()
        {
            Text = "Add Course",
            Command = new Command(() => addButtonClicked(termId.Text)),
            Background = Colors.RoyalBlue

        };
        Button returnHome = new Button()
        {
            Text = "Home",
            Command = new Command(() => homeClicked()),
            Background = Colors.RoyalBlue
        };
		
		stackEm.Children.Add(addButton);
		stackEm.Children.Add(termId);
        stackEm.Children.Add(returnHome);
        c.Content = stackEm;
        c.Title = t.name;
        return c;
	}
	
	public void deleteClicked(int termId, Button button)
	{
        SQL db = new();
		int courseIndex = buttons.IndexOf(button);
        Term term = db.getdbConnection().Table<Term>().FirstOrDefault(t => t.id == termId);
        Course course = coursesList[courseIndex];
        string[] courseString = term.courses.Split(',');
        term.courses = "";
        foreach (string s in courseString)
        {
            if (int.Parse(s) != course.id)
            {
                if (term.courses.Length > 0)
                {
                    string newId = ","+ s;
                    term.courses += newId;
                }
                else
                {
                     term.courses = s;
                }
            }
        }
        db.getdbConnection().Update(term);
        db.getdbConnection().Delete(course);
        Page currentPage = this.CurrentPage;
        int x = this.Children.IndexOf(currentPage);
        TabbedPage tab = new AllCourses(currentStudent.id);
        tab.CurrentPage = tab.Children[x];
        Navigation.PushAsync(tab);
        Navigation.RemovePage(this);

    }
	public void editClicked(Button button)
	{
        Page currentPage = this.CurrentPage;
        int x = this.Children.IndexOf(currentPage);
        int i = buttons.IndexOf(button);
		Navigation.PushAsync(new EditCourse(coursesList[i].id, x));
		Navigation.RemovePage(this);
    }
	public void addButtonClicked(string s)
	{

        Page currentPage = this.CurrentPage;
        int x = this.Children.IndexOf(currentPage);
        Navigation.PushAsync(new AddCourse(currentStudent.id, int.Parse(s), x));
		Navigation.RemovePage(this);

    }
    public void courseClicked(int courseId, int termId)
    {
        int index = this.Children.IndexOf(this.CurrentPage);
        Navigation.PushAsync(new CoursePage(courseId, index, currentStudent.id, termId));

    }
    public void homeClicked()
    {
        Navigation.PushAsync(new MainPage(currentStudent.id));
        Navigation.RemovePage(this);
    }

}