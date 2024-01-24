using DotNetExcptionHandling.Models;

namespace DotNetExcptionHandling.Interfaces
{
    public interface IDriverService
    {
        Task<Driver?> GetDriverAsync(int id);
        Task<Driver> AddDriverAsync(Driver model);
        Task<Driver> UpdateDriverAsync(Driver model);
        Task<bool> DeleteDriver(int id);
        Task<IEnumerable<Driver>> GetDriversAsync();
    }
}
