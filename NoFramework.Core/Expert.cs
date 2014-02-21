using System;
using System.Collections.Generic;

namespace NoFramework.Core
{
	public class Expert
	{
        HashSet<Guid> helpOffered = new HashSet<Guid>();

        INewbie newbie;

        private const string expertName = "Expert";

        public Expert (INewbie newbie)
		{
            this.newbie = newbie;
		}

        public void ProvideHelp() 
        {
            // get program exe state
            var owed = newbie.DonutOwed;

            // dead beat, ignore
            if (owed >= 10)
            {
                newbie.Status = "Sorry I don't know, I'll run the program for you again";
                newbie.ExecuteProgram(expertName);

                return;
            }

            if (this.helpOffered.Contains(newbie.ExecutionSignature))
            {
                newbie.Status = "Problem should be solved already. Let's run it again";
                newbie.ExecuteProgram(expertName);

                return;
            }

            var problem = newbie.Outcome;
            switch (problem)
            {
                case Problem.ArrayOutOfBound:
                    newbie.Status = "Check array accessor";
                    owed = owed + 10;
                    break;
                
                case Problem.NullReferenceException:
                    newbie.Status = "Do null check";
                    owed = owed + 5;
                    break;

                case Problem.OutOfMemory:
                    newbie.Status = "Call dispose whenever possible";
                    owed = owed + 2;
                    break;

                case Problem.StackOverflow:
                    newbie.Status = "Check your recursion algorithm";
                    owed = owed + 2;
                    break;

                case Problem.Unknown:
                case Problem.NoProblem:
                    newbie.Status = "Not a clue";
                    break;
            }

            // update donut owed to the model
            // update model to persist the donut owed. The model could then persist the info to Database or Cloud.

            // update newbie on the donut owed
            newbie.DonutOwed = owed;

            // record the signature so stay DRY
            helpOffered.Add(newbie.ExecutionSignature);
        }
	}
}

