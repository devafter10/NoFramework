using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using NoFramework.Core;
using System.Linq;
using ReactiveUI.Android;
using ReactiveUI;

namespace NoFramework.Droid
{
	[Activity (Label = "NoFramework.Droid", MainLauncher = true)]
    public class NewbieActivity : ReactiveActivity, IViewFor<Expert>, INewbie
	{
        private readonly int maxProblemValue = Enum.GetValues(typeof(Problem)).Cast<int>().Max();

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
            this.btnAskForHelp = FindViewById<Button>(Resource.Id.btnAskForHelp);
            this.btnExeProgram = FindViewById<Button>(Resource.Id.btnExeProgram);
            this.txtStatus = FindViewById<TextView>(Resource.Id.txtStatus);
            this.txtOutcome = FindViewById<TextView>(Resource.Id.txtOutcome);
            this.txtDonutOwed = FindViewById<TextView>(Resource.Id.txtDonutOwed);

            // irrespective of debugging skills, newbie at least know how to run program
            this.btnExeProgram.Click += (sender, e) => this.ExecuteProgram();

            // instantiate concrete class as it's rare we pair a newbie with multiple expert. But say if there's a team of 
            // experts helping the same newbie, then we should define an interface for the experts.
            this.ViewModel = new Expert(this);

            this.Bind(this.ViewModel, vm => vm.DonutOwed, v => v.txtDonutOwed.Text);
            this.Bind(this.ViewModel, vm => vm.Status, v => v.txtStatus.Text);
            this.Bind(this.ViewModel, vm => vm.Outcome, v => v.txtOutcome.Text);

            this.BindCommand(this.ViewModel, vm => vm.ProvideHelpCommand, view => view.btnAskForHelp);
        }


        #region IViewFor implementation

        public Expert ViewModel { get; set; }

        object IViewFor.ViewModel
        {
            get { return this.ViewModel; }
            set { ViewModel = (Expert)value; }
        }

        #endregion

        #region INewbie implementation

        public void ExecuteProgram() 
        {
            // always encounter an issue
            Random r = new Random();
            this.txtOutcome.Text = ((Problem)r.Next(maxProblemValue)).ToString();

            this.ViewModel.Signature = Guid.NewGuid();
        }

        public void ShowMessage(string msg) 
        {
            Toast.MakeText(this, msg, ToastLength.Short).Show();
        }

        #endregion

      }
}


