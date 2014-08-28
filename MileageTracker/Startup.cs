using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using MileageTracker.Infrastructure.Configuration;
using MileageTracker.Infrastructure.Logging;
using MileageTracker.Providers;
using Owin;
using System;
using System.Web.Http;

[assembly: OwinStartup(typeof(MileageTracker.Startup))]
namespace MileageTracker {
    public class Startup {
        public void Configuration(IAppBuilder app) {
            var config = new HttpConfiguration();
            config.EnableCors();

            ApplicationSettingsFactory.InitializeApplicationSettingsFactory(new WebConfigApplicationSettings());
            LoggingFactory.InitializeLogFactory(new Log4NetAdapter());

            //Autofac
            var container = AutofacConfig.RegisterContainer(config);
            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);

            //OAuth
            ConfigureOAuth(app);

            //Routes
            WebApiConfig.Register(config);

            app.UseCors(CorsOptions.AllowAll);

            app.UseWebApi(config);
        }

        private void ConfigureOAuth(IAppBuilder app) {
            var oAuthServerOptions = new OAuthAuthorizationServerOptions {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/account/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new AuthorizationServerProvider()
            };

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
        }
    }
}