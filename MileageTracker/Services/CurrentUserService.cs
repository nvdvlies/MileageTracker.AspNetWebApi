using Microsoft.AspNet.Identity;
using MileageTracker.Interfaces;
using System.Web;

namespace MileageTracker.Services {
    public class CurrentUserService : ICurrentUserService {
        public string UserId {
            get {
                if (HttpContext.Current == null) return null;
                if (HttpContext.Current.User == null) return null;
                if (HttpContext.Current.User.Identity == null) return null;
                return !HttpContext.Current.User.Identity.IsAuthenticated ? null : HttpContext.Current.User.Identity.GetUserId();
            }
        }
    }
}