using Microsoft.AspNet.Identity;
using MileageTracker.Models;
using MileageTracker.ViewModels;
using System.Threading.Tasks;

namespace MileageTracker.Interfaces {
    public interface IAccountService {
        Task<ApplicationUser> FindUser(string userName, string password);
        Task<IdentityResult> RegisterUser(RegisterViewModel registerViewModel);
    }
}
