namespace EmployeeArrivalTracker.Data.Common.Hubs
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR;

    public class EmployeesHub : Hub
    {
        public const string MethodName = "ReceiveArrivals";

        public async Task SendArrivals(string result)
        {
            await this.Clients.All.SendAsync(MethodName, result);
        }
    }
}