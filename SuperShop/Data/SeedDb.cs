using Microsoft.AspNetCore.Identity;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private Random _random;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            var user = await _userHelper.GetUserByEmailAsync("gabriel@gmail.com");

            if(user == null)
            {
                user = new User
                {
                    FirstName = "Gabriel",
                    LastName = "Costa",
                    Email = "gabriel@gmail.com",
                    UserName = "gabriel@gmail.com",
                    PhoneNumber = "22323232232"
                };

                var result = await _userHelper.AddUserAsync(user, "123456");

                if(result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user, in seeder class");
                }
            }

            if(!_context.Products.Any())
            {
                AddProduct("Iphone x", user);
                AddProduct("Magic Mouse", user);
                AddProduct("iWhatch Series 4", user);
                AddProduct("iPad Mini", user);

                await _context.SaveChangesAsync();
            }
        }

        private void AddProduct(string name, User user)
        {
            _context.Products.Add(new Product {
                Name = name,
                Price = _random.Next(1000),
                IsAvailabe = true,
                Stock = _random.Next(100),
                User = user
            });
        }
    }
}
