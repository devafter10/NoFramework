using System;

namespace NoFramework.Core
{
    public enum Problem : int
    {
        Unknown = 0,
        NoProblem = 1,
        StackOverflow = 2,
        ArrayOutOfBound = 3,
        OutOfMemory = 4,
        NullReferenceException = 5
    }
}

