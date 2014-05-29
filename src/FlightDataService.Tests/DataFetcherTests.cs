using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FlightDataService.Tests
{
    [TestClass]
    public class DataFetcherTests
    {
        [TestMethod]
        public async Task GetData()
        {
            var fetcher = new DataDownloader();

            var data = await fetcher.GetData();
        }

        [TestMethod]
        public void ParseData()
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\Data.json");
            string json = File.ReadAllText(path);

            var data = DataParser.Parse(json);
        }
    }
}