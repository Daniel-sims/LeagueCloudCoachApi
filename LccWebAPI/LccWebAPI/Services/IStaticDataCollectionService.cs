using System.Threading.Tasks;

namespace LccWebAPI.Services
{
    public interface IStaticDataCollectionService
    {
        Task CollectStaticDataIfNeeded();
    }
}
