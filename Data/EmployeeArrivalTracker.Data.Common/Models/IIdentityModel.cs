namespace EmployeeArrivalTracker.Data.Common.Models
{
    public interface IIdentityModel<TKey>
    {
        TKey Id { get; set; }
    }
}