using System.Threading.Tasks;

namespace EmployeeArrivalTracker.Services.Data
{
    public interface ISubscriptionService
    {
        void Dispose();
        Task SubscribeAsync();
    }
}