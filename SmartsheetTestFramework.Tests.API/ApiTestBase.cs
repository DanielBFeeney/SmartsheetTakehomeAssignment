using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smartsheet.Api;
using SmartsheetTestFramework.Tests.Common;

namespace SmartsheetTestFramework.Tests.API
{
    public class ApiTestBase : UnitTestBase
    {
        protected static SmartsheetClient _smartsheetClient = null;
        protected static List<long> _testSheetIds = new List<long>();

        #region Initializations/Cleanup

        //private static Random randSeed = new Random();
        //private static Random rand = new Random(
        //    randSeed.Next(99999));

        [TestInitialize]
        public void MyTestInitialize()
        {
            initialize();
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            foreach (long id in _testSheetIds)
            {
                _smartsheetClient.SheetResources.DeleteSheet(id);
            }

            _testSheetIds.Clear();
        }

        #endregion

        #region Methods
        protected static void initializeSmartsheetClient()
        {
            // Get API access token from App.config file or environment
            string accessToken = ConfigurationManager.AppSettings["AccessToken"];
            if (string.IsNullOrEmpty(accessToken))
                accessToken = Environment.GetEnvironmentVariable("SMARTSHEET_ACCESS_TOKEN");
            if (string.IsNullOrEmpty(accessToken))
                throw new Exception("Must set API access token in App.config file");

            // Initialize client
            _smartsheetClient = new SmartsheetBuilder().SetAccessToken(accessToken).Build();
        }
        #endregion
    }
}
