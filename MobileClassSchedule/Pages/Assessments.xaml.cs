using MobileClassSchedule.Classes;

namespace MobileClassSchedule.Pages;

public partial class Assessments : ContentPage
{
	int cId;

	public Assessments(int courseId)
	{
		InitializeComponent();
		cId = courseId;
		createView(cId);
	}

	private void createView(int id)
	{
		SQL db = new();

		Course c = new Course();
		c = db.getdbConnection().Table<Course>().FirstOrDefault(c=>c.id == id);
		
		if (c.assessments.Length > 0)
		{
			string[] assessInCourse = c.assessments.Split(",");
			foreach(string s in assessInCourse)
			{
				Assessment assess = new Assessment();
				int aId = int.Parse(s);
				assess = db.getdbConnection().Table<Assessment>().FirstOrDefault(a=>a.id == aId);

				StackLayout stackIt = new();
				SwipeView swipe = new();

				Button button = new Button
                {
					Text = assess.title,
                    Command = new Command(() => assessClicked(assess.id, cId))
                };
				
				stackIt.Children.Add(button);

				SwipeItem edit = new SwipeItem
				{
                    Text = "Edit",
                    BackgroundColor = Colors.LightBlue,
                    Command = new Command(() =>editClicked(assess.id, cId, assess.type[0]))
                };

				SwipeItem delete = new SwipeItem
				{
                    Text = "Delete",
                    BackgroundColor = Colors.LightPink,
                    Command = new Command(() => deleteClicked(assess.id))
                };
				swipe.RightItems = new SwipeItems { delete };
				swipe.LeftItems = new SwipeItems { edit };
				swipe.Content = stackIt;

				stackEm.Children.Add(swipe);

			}
		}
	}
	private async void addClicked(object sender, EventArgs e)
	{
		SQL db = new();

		Course c = new Course();

		c = db.getdbConnection().Table<Course>().FirstOrDefault(c => c.id == cId);
		string[] assess = c.assessments.Split(",");
		

		// figure out what assessments course already has
		// and if it can have another
		if (assess.Length < 2)
		{
			int i = assess.Length;
            if (!int.TryParse(assess[0], out _))
            {
                await Navigation.PushAsync(new AddAssessment(cId, 'n'));
                Navigation.RemovePage(this);
            }
			else
			{
				
                Assessment assessment = new Assessment();
                int aId = int.Parse(assess[0]);
                assessment = db.getdbConnection().Table<Assessment>().FirstOrDefault(a => a.id == aId);

				char thing = assessment.type[0];

                if (assessment.type[0] == 'O')
                {
                    // check if objective
                    await Navigation.PushAsync(new AddAssessment(cId, 'O'));
                    Navigation.RemovePage(this);
                }
                else if (assessment.type[0] == 'P')
                {
                    // check if preformance
                    await Navigation.PushAsync(new AddAssessment(cId, 'P'));
                    Navigation.RemovePage(this);
                }
            }
            
		}
		else
		{
			// pop up because there is already two assessments

			await DisplayAlert("Oops", "You can only have two assessments!\n\nOne Performance and one Objective", "Ok");
		}
	}
	private void assessClicked(int assessId, int courseId)
	{

	}
	private void editClicked(int assessId, int courseId, char assessType)
	{
		Navigation.PushAsync(new EditAssessment(courseId, assessType, assessId));
		Navigation.RemovePage(this);
	}
	private void deleteClicked(int assessId)
	{
		SQL db = new();
		Course c = db.getdbConnection().Table<Course>().FirstOrDefault(c => c.id == cId);
		Assessment assess = db.getdbConnection().Table<Assessment>().FirstOrDefault(a=>a.id==assessId);

		string[] cAssess = c.assessments.Split(',');
		c.assessments = "";
		foreach(string s in cAssess)
		{
			if(int.Parse(s) != assess.id)
			{
				if(c.assessments.Length == 0)
				{
					c.assessments = s;
				}
				else
				{
					string a = "," + s;
					c.assessments += a;
				}
			}
		}

		db.getdbConnection().Delete(assess);
		db.getdbConnection().Update(c);

		Navigation.PushAsync(new Assessments(cId));
		Navigation.RemovePage(this);
	}

}