using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;

namespace ArPlaneTrackerClient.Test
{
    [TestClass]
    public class FlightPositionClientTests
    {
        [TestMethod]
        public async Task StartClient_Success()
        {
            var are = new AutoResetEvent(false);

            var client = new FlightPositionClient();
            await client.Start();
            client.NewData += (s, e) =>
            {
                if (e.Count > 0)
                    are.Set();
            };

            client.SetLocation(60, 24, 0);

            are.WaitOne();
        }
    }
}