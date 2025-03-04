using Bulky.DataAccess.Data;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;

namespace Bulky.DataAccess.DbInitializer;

public class DbInitializer(
    ApplicationDbContext dbContext,
    UserManager<IdentityUser> userManager,
    RoleManager<IdentityRole> roleManager)
    : IDbInitializer
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly  UserManager<IdentityUser> _userManager = userManager;
    private readonly  RoleManager<IdentityRole> _roleManager = roleManager;

    public void Initialize()
    {
        try
        {
            if (_dbContext.Database.GetPendingMigrations().Any())
            {
                _dbContext.Database.Migrate();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        if (!_roleManager.RoleExistsAsync(SD.RoleCustomer).GetAwaiter().GetResult())
        {
            _roleManager.CreateAsync(new IdentityRole(SD.RoleCustomer)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.RoleAdmin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.RoleCustomer)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.RoleEmployee)).GetAwaiter().GetResult();
            
            //if roles are not created, then we will create admin user as well
            _userManager.CreateAsync(new ApplicationUser {
                UserName = "admin@dotnetmastery.com",
                Email = "admin@dotnetmastery.com",
                Name = "Bhrugen Patel",
                PhoneNumber = "1112223333",
                StreetAddress = "test 123 Ave",
                State = "IL",
                PostalCode = "23422",
                City = "Chicago"
            }, "Admin123*").GetAwaiter().GetResult();

            ApplicationUser user =
                _dbContext.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@dotnetmastery.com");
            _userManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();    
        }

        return;
    }
}