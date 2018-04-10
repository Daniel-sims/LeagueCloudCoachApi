using System;
using System.Threading.Tasks;

namespace LccWebAPI.Services
{
    public interface IThrottledRequestHelper
    {
        void SetThrottleRates(int requestsPerTimePeriod, int timePeriod);
        Task<T> SendThrottledRequest<T>(Func<Task<T>> action) where T : class;
    }
}
