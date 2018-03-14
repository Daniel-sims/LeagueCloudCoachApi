using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Utils
{
    public interface ILogging
    {
        void LogEvent(string message);
    }
}
