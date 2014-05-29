using FlightDataService.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace FlightDataService
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        // TODO: Dependency Injection
        public static Lazy<IDataHandler> DataHandler = new Lazy<IDataHandler>(() => new DataHandler());

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            GlobalConfiguration.Configuration.Services.Add(typeof(IExceptionLogger), new UnhandledExceptionLogger());

            // TODO: Move this to proper place
            DataHandler.Value.UpdatedClientData += Value_UpdatedClientData;

            //GlobalConfiguration.Configuration.DependencyResolver.Register(typeof(IDataHandler),
            //   () => _dataHandler.Value);
        }

        private void Value_UpdatedClientData(object sender, Tuple<string, List<FlightInfo>> e)
        {
            var ctx = GlobalHost.ConnectionManager.GetHubContext("FlightsHub");
            var client = ctx.Clients.Client(e.Item1);
            client.newData(e.Item2.MapToDTO());
        }
    }
}