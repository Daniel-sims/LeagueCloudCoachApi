using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Utils
{
    public class Logging : ILogging
    {
        private int LogCount = 0;

        public void LogEvent(string message)
        {
            ++LogCount;
            Console.WriteLine(LogCount + ". " + message);
        }
    }
}
