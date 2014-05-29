using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace FlightDataService
{
    public class UnhandledExceptionLogger : IExceptionLogger
    {
        public void Log(ExceptionLoggerContext context)
        {
            Trace.WriteLine(context.ExceptionContext.Exception.ToString());
        }

        public Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
                {
                    Trace.WriteLine(context.ExceptionContext.Exception.ToString());
                });
        }
    }
}