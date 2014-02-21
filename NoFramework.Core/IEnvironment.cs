using System;

namespace NoFramework.Core
{
    public interface IEnvironment
    {
        Guid ExecutionSignature { get; }

        string Status { get; set; }

        Problem Outcome { get; set; }

        int DonutOwed { get; set; }
    }
}

