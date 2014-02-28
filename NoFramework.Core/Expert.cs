using System;
using System.Collections.Generic;
using ReactiveUI;
using System.Reactive.Linq;

namespace NoFramework.Core
{
    public class Expert : ReactiveObject
	{
        HashSet<Guid> helpOffered = new HashSet<Guid>();

        private ReactiveCommand provideHelpCommand = new ReactiveCommand();
        public ReactiveCommand ProvideHelpCommand { get { return this.provideHelpCommand; } }

        private Guid signature;
        public Guid Signature 
        {
            get { return this.signature; }
            set { this.RaiseAndSetIfChanged(ref this.signature, value); }
        }

        private string status;
        public string Status 
        {
            get { return this.status; }
            set { this.RaiseAndSetIfChanged(ref this.status, value); }
        }

        private int donutOwed;
        public int DonutOwed 
        {
            get { return this.donutOwed; }
            set { this.RaiseAndSetIfChanged(ref this.donutOwed, value); }
        }

        private Problem outcome;
        public Problem Outcome
        {
            get { return this.outcome; }
            set { this.RaiseAndSetIfChanged(ref this.outcome, value); }
        }

        private INewbie view;

        public Expert (INewbie view = null)
		{
            this.view = view;

            var problemsWorthSolving = this.ProvideHelpCommand
                .Where(_ => this.Outcome == Problem.OutOfMemory || this.Outcome == Problem.StackOverflow)
                .Where(_ => !this.helpOffered.Contains(this.Signature));

            var problemsWeCanSolve = problemsWorthSolving
                .Where(_ => this.DonutOwed < 10)
                .Subscribe(_ => this.ProvideHelp());

            var problemsWeCanSolveButDoNotWantTo = problemsWorthSolving
                .Where(_ => this.DonutOwed >= 10)
                .Subscribe(_ => this.ProvideNoHelp());
		}

        public void ProvideNoHelp() 
        {
            this.Status = "Sorry don't feel like solving your problem";

            // record the signature so stay DRY
            helpOffered.Add(this.Signature);

            this.view.ShowMessage("Get Lost Deadbeat");
        }

        public void ProvideHelp() 
        {
            // get program exe state
            var owed = this.DonutOwed;

            string newStatus = string.Empty;
            int donutDelta = 0;
            var problem = this.Outcome;
            switch (problem)
            {
                case Problem.OutOfMemory:
                    newStatus = "Call dispose whenever possible";
                    donutDelta = 2;
                    break;

                case Problem.StackOverflow:
                    newStatus = "Check your recursion algorithm";
                    donutDelta = 4;
                    break;

                default:
                    newStatus = "This Shouldn't happen";
                    donutDelta = 0;
                    break;
            }

            this.Status = newStatus;
            this.DonutOwed = this.DonutOwed + donutDelta;

            // update donut owed to the model
            // update model to persist the donut owed. The model could then persist the info to Database or Cloud.

            // record the signature so stay DRY
            helpOffered.Add(this.Signature);

            this.view.ShowMessage("Help Provided");
        }
	}
}

