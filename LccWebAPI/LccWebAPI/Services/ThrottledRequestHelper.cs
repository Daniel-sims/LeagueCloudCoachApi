using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LccWebAPI.Services
{
    public class ThrottledRequestHelper : IThrottledRequestHelper
    {
        private readonly int _timePeriod;
        private readonly int _requestsPerTimePeriod;
        
        private int MsDelayBetweenRequests { get { return _timePeriod / _requestsPerTimePeriod * 1000; } }

        public ThrottledRequestHelper(int requestsPerTimePeriod = 100, int timePeriod = 120)
        {
            _requestsPerTimePeriod = requestsPerTimePeriod;
            _timePeriod = timePeriod;
        }

        public async Task<T> SendThrottledRequest<T>(Func<Task<T>> action)
        {
            await Task.Run(() => Thread.Sleep(MsDelayBetweenRequests));
            return await action();
        }

    }
}
