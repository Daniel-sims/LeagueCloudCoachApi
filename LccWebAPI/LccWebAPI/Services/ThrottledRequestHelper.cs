using RiotSharp;
using System;
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
        
        public ThrottledRequestHelper()
        {
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
                if (e.HttpStatusCode == (HttpStatusCode)429)
                {
                    Console.WriteLine("429 received sleeping for 25 seconds.");
                    await Task.Run(() => Thread.Sleep(25 * 1000));
                }

                if (e.HttpStatusCode == (HttpStatusCode)403)
                {
                    Console.WriteLine("403 received sleeping indefinetly.");
                    await Task.Run(() => Thread.Sleep(1000 * 1000));
                }
            }
            catch(Exception e)
            { }

            return await Task.FromResult<T>(null);
        }

        public void SetThrottleRates(int requestsPerTimePeriod, int timePeriod)
        {
            _requestsPerTimePeriod = requestsPerTimePeriod;
            _timePeriod = timePeriod;
        }
    }
}
