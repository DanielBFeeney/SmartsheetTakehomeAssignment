using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Smartsheet.Api;
using Smartsheet.Api.Models;
using Smartsheet.Api.OAuth;

using SmartsheetTestFramework.Tests.API.Common.Helpers;
namespace SmartsheetTestFramework.Tests.API
{
    [TestClass]
    public class Tests_GetSheet : ApiTestBase
    {
        //ClassInitialize method cannot be inherited. 
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            initializeSmartsheetClient();
        }

        [TestMethod, TestCategory("API_GetSheets"), Description("Creates a simple sheet in the 'sheets' folder then gets it with default (null) params")]
        public void Test_GetSheet001()
        {
            Column columnA = new Column
            {
                Title = "TitleA",
                Primary = false,
                Type = ColumnType.CHECKBOX,
                Symbol = Symbol.STAR
            };

            // Specify properties of the second column
            Column columnB = new Column
            {
                Title = "TitleB",
                Primary = true,
                Type = ColumnType.TEXT_NUMBER
            };

            Column columnC = new Column
            {
                Title = "TitleC",
                Primary = false,
                Type = ColumnType.TEXT_NUMBER,
            };


            // Create sheet in "Sheets" folder (specifying the 2 columns to include in the sheet)
            Sheet newSheet = _smartsheetClient.SheetResources.CreateSheet(new Sheet
            {
                Name = "newsheetTest002",
                Columns = new Column[] { columnA, columnB, columnC }
            }
            );

            long? sheetId = newSheet.Id;
            _testSheetIds.Add((long)sheetId);

            List<SheetLevelInclusion> sheetLevelInclusionList = new List<SheetLevelInclusion>();
            sheetLevelInclusionList.Add(SheetLevelInclusion.COLUMN_TYPE);
            List<long> columnIdList = new List<long>() { 0, 3 };

            //Sheet createdSheet = _smartsheetClient.SheetResources.GetSheet((long)sheetId, null, null, null, null, columnIdList, null, null); ;
            Sheet gotSheet = _smartsheetClient.SheetResources.GetSheet((long)sheetId, null, null, null, null, null, null, null);

            evaluate(
                SheetHelper.CompareForCreateSheets(gotSheet, newSheet),
                "Test_GetSheet001",
                "Test_GetSheet001",
                SheetHelper.DisplayText(gotSheet),
                (SheetHelper.DisplayText(newSheet)));
        }
    }
}
