using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MileageTracker.Infrastructure.Configuration;
using MileageTracker.Infrastructure.Logging;
using MileageTracker.Interfaces;
using MileageTracker.Models;
using MileageTracker.Repositories;
using MileageTracker.Services;
using System.Reflection;
using System.Web.Http;

namespace MileageTracker {
    public class AutofacConfig {
        public static IContainer RegisterContainer(HttpConfiguration config) {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            //Settings
            builder.RegisterType<WebConfigApplicationSettings>().As<IApplicationSettings>().InstancePerRequest();
            //Logger
            builder.RegisterType<Log4NetAdapter>().As<ILogger>().InstancePerRequest();
            //EF
            builder.RegisterType<ApplicationDbContext>().As<IApplicationDbContext>().InstancePerRequest();
            builder.RegisterType<IdentityDbContext<ApplicationUser>>();
            //Usermanager
            //builder.RegisterInstance(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())));
            //Services
            builder.RegisterType<CurrentUserService>().As<ICurrentUserService>().InstancePerRequest();
            builder.RegisterType<AccountService>().As<IAccountService>().InstancePerRequest();
            builder.RegisterType<TripService>().As<ITripService>().InstancePerRequest();
            builder.RegisterType<AddressService>().As<IAddressService>().InstancePerRequest();
            builder.RegisterType<CarService>().As<ICarService>().InstancePerRequest();
            builder.RegisterType<GoogleDistanceCalculatorService>().As<IDistanceCalculatorService>().InstancePerRequest();

            var container = builder.Build();
            var resolver = new AutofacWebApiDependencyResolver(container);

            config.DependencyResolver = resolver;

            return container;
        }
    }
}