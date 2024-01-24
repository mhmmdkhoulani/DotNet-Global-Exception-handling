using DotNetExcptionHandling.Data;
using DotNetExcptionHandling.Exceptions;
using DotNetExcptionHandling.Interfaces;
using DotNetExcptionHandling.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetExcptionHandling.Services
{
    public class DriverService : IDriverService
    {
        private readonly AppDBContext _db;

        public DriverService(AppDBContext db)
        {
            _db = db;
        }

        public async Task<Driver> AddDriverAsync(Driver model)
        {
            await _db.Drivers.AddAsync(model);
            await _db.SaveChangesAsync();

            return model;
        }

        public async Task<bool> DeleteDriver(int id)
        {
            var driver = await _db.Drivers.FindAsync(id);
            if (driver is null)
                throw new NotFoundException($"Driver with id {id} not found!");

            _db.Drivers.Remove(driver);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<Driver?> GetDriverAsync(int id)
        {
            var driver = await _db.Drivers.FindAsync(id);
            if (driver is null)
                throw new NotFoundException($"Driver with id {id} not found!");

            return driver;
        }

        public async Task<IEnumerable<Driver>> GetDriversAsync()
        {
            var drivers = await _db.Drivers.ToListAsync(); 
            return drivers;
        }

        public async Task<Driver> UpdateDriverAsync(Driver model)
        {
            var driver = await _db.Drivers.FindAsync(model.Id);
            if (driver is null)
                throw new NotFoundException($"Driver with id {model.Id} not found!");
            driver.DriverNumber = model.DriverNumber;
            driver.Name = model.Name;
            await _db.SaveChangesAsync();
            return driver;

        }
    }
}
