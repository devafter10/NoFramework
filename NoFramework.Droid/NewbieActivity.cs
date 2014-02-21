using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using NoFramework.Core;
using System.Linq;

namespace NoFramework.Droid
{
	[Activity (Label = "NoFramework.Droid", MainLauncher = true)]
    public class NewbieActivity : Activity, INewbie
	{
        private Guid programExecutionSignature = Guid.Empty;

        private readonly int maxProblemValue = Enum.GetValues(typeof(Problem)).Cast<int>().Max();

		private Expert expert;

        Button btnAskForHelp;

        Button btnExeProgram;

        TextView txtStatus;

        TextView txtOutcome;

        TextView txtDonutOwed;

		protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // locate all controls
            btnAskForHelp = FindViewById<Button>(Resource.Id.btnAskForHelp);
            btnExeProgram = FindViewById<Button>(Resource.Id.btnExeProgram);
            txtStatus = FindViewById<TextView>(Resource.Id.txtStatus);
            txtOutcome = FindViewById<TextView>(Resource.Id.txtOutcome);
            txtDonutOwed = FindViewById<TextView>(Resource.Id.txtDonutOwed);

            // irrespective of debugging skills, newbie at least know how to run program
            this.btnExeProgram.Click += (sender, e) => this.ExecuteProgram("Newbie");

            // instantiate concrete class as it's rare we pair a newbie with multiple expert. But say if there's a team of 
            // experts helping the same newbie, then we should define an interface for the experts.
            this.expert = new Expert(this);

            // [C] newbie should at least know how to notify expert and provide
            btnAskForHelp.Click += (sender, e) => this.expert.ProvideHelp();
        }

        #region INewbie implementation

        public void ExecuteProgram(string runner) 
        {
            Toast.MakeText(this, runner + " - running program", ToastLength.Short).Show();

            // always encounter an issue
            Random r = new Random();
            this.Outcome = (Problem)r.Next(maxProblemValue);

            this.programExecutionSignature = Guid.NewGuid();
        }

        #endregion

        #region IEnvironment implementation

        public string Status { get { return this.txtStatus.Text; } set { this.txtStatus.Text = value; } }

        public Problem Outcome 
        { 
            get
            { 
                Problem ret;
                if (!Enum.TryParse<Problem>(this.txtOutcome.Text, out ret))
                {
                    ret = Problem.Unknown;
                }

                return ret;
            }

            set 
            { 
                this.txtOutcome.Text = value.ToString(); 
            } 
        }

        public int DonutOwed 
        { 
            get 
            {
                int ret;
                if (!Int32.TryParse(this.txtDonutOwed.Text, out ret))
                {
                    ret = 0;
                }

                return ret;
            }

            set 
            {
                this.txtDonutOwed.Text = value.ToString();
            }
        }


        public Guid ExecutionSignature
        {
            get
            {
                return this.programExecutionSignature;
            }
        }        

        #endregion
    }
}


