using LccWebAPI.Utils;
using RiotSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace LccWebAPI.Services
{
    public class ThrottledRequestHelper : IThrottledRequestHelper
    {
        private int _timePeriod;
        private int _requestsPerTimePeriod;
        
        private int DelayBetweenRequestsAsSeconds { get { return (1000 * _timePeriod) / _requestsPerTimePeriod; } }

        private readonly ILogging _logging;

        public ThrottledRequestHelper(ILogging logging)
        {
            _logging = logging;
            _timePeriod = 120;
            _requestsPerTimePeriod = 90;
        }

        public async Task<T> SendThrottledRequest<T>(Func<Task<T>> action) where T : class
        {
            try
            {
                await Task.Run(() => Thread.Sleep(DelayBetweenRequestsAsSeconds));
                return await action();
            }
            catch (RiotSharpException e)
            {
                //Leaving this one for now as it's generally important exceptions being logged here but will replace
                _logging.LogEvent("######RiotSharpException encountered when sending throttle request - " + e.Message + ".");
                if (e.HttpStatusCode == (HttpStatusCode)429)
                {
                    _logging.LogEvent("Sleeping for 50 seconds.");
                    await Task.Run(() => Thread.Sleep(50 * 1000));
                }
            }
            catch(Exception e)
            {
                _logging.LogEvent("######Exception encountered when trying to send throttled request - " + e.Message + ". Action: " + action.ToString());
            }


            return await Task.FromResult<T>(null);
        }

        public void SetThrottleRates(int requestsPerTimePeriod, int timePeriod)
        {
            _requestsPerTimePeriod = requestsPerTimePeriod;
            _timePeriod = timePeriod;
        }
    }
}
