using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Services
{
    public interface IThrottledRequestHelper
    {
        Task<T> SendThrottledRequest<T>(Func<Task<T>> action);
    }
}
