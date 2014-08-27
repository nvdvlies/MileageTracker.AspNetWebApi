using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MileageTracker.Interfaces;
using MileageTracker.Models;
using MileageTracker.ViewModels;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace MileageTracker.Services {

    public class AccountService : IAccountService, IDisposable {
        private readonly IApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(IApplicationDbContext dbContext)
            : this(dbContext, new UserManager<ApplicationUser>(
                                    new UserStore<ApplicationUser>(
                                           (DbContext) dbContext
                                    ))) {
        }

        public AccountService(IApplicationDbContext dbContext, UserManager<ApplicationUser> userManager) {
            _dbContext = dbContext;
            _userManager = userManager;
            _userManager.UserValidator = new UserValidator<ApplicationUser>(_userManager) {
                AllowOnlyAlphanumericUserNames = false
            };
            _userManager.PasswordValidator = new PasswordValidator {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };
        }

        public async Task<ApplicationUser> FindUser(string userName, string password) {
            var user = await _userManager.FindAsync(userName, password);
 
            return user;
        }

        public async Task<IdentityResult> RegisterUser(RegisterViewModel registerViewModel) {
            var user = new ApplicationUser {
                UserName = registerViewModel.UserName
            };

            var result = await _userManager.CreateAsync(user, registerViewModel.Password);
            return result;
        }

        public void Dispose() {
            _userManager.Dispose();
            _dbContext.Dispose();
        }
    }
}