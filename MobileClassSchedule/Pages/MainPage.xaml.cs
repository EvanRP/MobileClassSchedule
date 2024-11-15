using MobileClassSchedule.Classes;

namespace MobileClassSchedule.Pages;

public partial class MainPage : ContentPage
{
	Student student;
	List<Button> termsBs;
	List<Term> terms;

    public MainPage(int studentId)
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        // create db connection
        SQL dbConnection = new SQL();
        student = new Student();
		student = lookupStudent(studentId);

        // view for buttons that need a slide view
        StackLayout termButtons = new StackLayout();

		if (student.terms.Length > 0)
		{
			// Parse term csv string
			string[] section = student.terms.Split(',');
			// create a button for each term and add to list
			termsBs = new List<Button>();
			terms = new List<Term>();
			foreach (string s in section)
			{
                Term t = student.termLookup(int.Parse(s));
				terms.Add(t);
				Button button = new Button
				{
					Text = t.name,
                    //Padding = 20
                    Command = new Command(() => termClicked(terms.IndexOf(t)))
                };
				//button.Clicked += termClicked;
                // swipe items created
                SwipeItem deleteSwipe = new SwipeItem
                {
                    Text = "Delete",
                    BackgroundColor = Colors.LightPink,
					Command = new Command(() => deleteClicked(terms.IndexOf(t)))
                };
                SwipeItem editSwipe = new SwipeItem
                {
                    Text = "Edit",
                    BackgroundColor = Colors.LightBlue,
                    Command = new Command(() => editClicked(terms.IndexOf(t)))
                };
				// create new stack layout to be the swipe views content and add button to it
				StackLayout stackit = new();
				stackit.Children.Add(button);

				// create swipe view and left and right item also content 
                SwipeView swipeView = new();
                swipeView.RightItems = new SwipeItems { deleteSwipe };
				swipeView.LeftItems = new SwipeItems { editSwipe };
				swipeView.Content = stackit;

				// add button to list
               // termsBs.Add(button);
                
				// add swipe view to layout on mainpage
                termStackView.Children.Add(swipeView);
			}
        }
	}
	private void editClicked(int index)
	{
        Term term = terms[index];
		Navigation.PushAsync(new EditTerm(student, term));
		Navigation.RemovePage(this);
    }
	private void deleteClicked(int indexOfTerm)
	{
		
		Term term = new();
		term = terms[indexOfTerm];
		int termId = term.id;
		SQL db = new();
		db.getdbConnection().Delete(term);
		//termsBs.Remove(button);
		terms.Remove(term);
        // remove term id for student termid string
        string[] section = student.terms.Split(',');
		int[] ids = new int[section.Length - 1];
		student.terms = "";
		int index = 0;
		foreach(string s in section)
		{
			int i = int.Parse(s);
			if (i != termId)
			{
				ids[index] = i;
				index++;
			}
		}
		for(int x = 0; x < ids.Length; x++)
		{
			if (x == 0)
			{
				student.terms = ids[x].ToString();
			}
			else
			{
				string y = "," + ids[x].ToString();
				student.terms += y;
			}
		}
		db.getdbConnection().Update(student);
		Navigation.PushAsync(new MainPage(student.id));
		Navigation.RemovePage(this);
    }

	private void AddTermClicked(object sender, EventArgs e)
	{
		var termPage = new AddTerm(student);
		termPage.currentStudent = student;
        
        Navigation.PushAsync(termPage);
        Navigation.RemovePage(this);
    }

	public Student lookupStudent(int id)
	{
		SQL db = new SQL();
		
		return db.getdbConnection().Table<Student>().FirstOrDefault(s => s.id == id, null);
    }
	private void termClicked(int index)
	{
		//Term term = terms.FirstOrDefault(ter=>ter.id == t);
		Term term = terms[index];
		TabbedPage tabPage = new AllCourses(student.id);
        tabPage.CurrentPage = tabPage.Children[index];
		Navigation.PushAsync(tabPage);
        Navigation.RemovePage(this);
    }
}

