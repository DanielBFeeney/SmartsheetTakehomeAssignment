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
    public class Tests_CreateSheet : ApiTestBase
    {
        //ClassInitialize method cannot be inherited. 
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            initializeSmartsheetClient();
        }

        [TestMethod, TestCategory("API_CreateSheets"), Description("Creates a simple sheet in the 'sheets' folder")]
        public void Test_CreateSheet001()
        {
            //Create Columns
            Column columnA = new Column
            {
                Title = "TitleA",
                Primary = false,
                Type = ColumnType.CHECKBOX,
                Symbol = Symbol.STAR
            };

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
                Type = ColumnType.PICKLIST,
                Symbol = Symbol.PROGRESS
            };


            // Create sheet in "Sheets" folder
            Sheet newSheet = _smartsheetClient.SheetResources.CreateSheet(new Sheet
            {
                Name = "newsheetTest001",
                Columns = new Column[] { columnA, columnB, columnC }
            }
            );

            //Add new sheet id to list for TestCleanup
            _testSheetIds.Add((long)newSheet.Id);

            // Manually creating a sheet with expected values to compare. 
            Sheet expectedSheet = new Sheet
            {
                Name = "newsheetTest001",
                AccessLevel = AccessLevel.OWNER,
                Columns = new Column[] { columnA, columnB, columnC }
            };

            //Evaluating that the new and expected sheets match
            evaluate(
                SheetHelper.CompareForCreateSheets(newSheet, expectedSheet),
                "Test_CreateSheet001",
                "Test_CreateSheet001",
                 SheetHelper.DisplayText(newSheet),
                 SheetHelper.DisplayText(expectedSheet));
        }

        [TestMethod, TestCategory("API_CreateSheets"), Description("Verifies sheet name may not be unique")]
        public void Test_CreateSheet002()
        {
            //Create Columns
            Column columnA = new Column
            {
                Title = "TitleA",
                Primary = false,
                Type = ColumnType.CHECKBOX,
                Symbol = Symbol.STAR
            };

            Column columnB = new Column
            {
                Title = "TitleB",
                Primary = true,
                Type = ColumnType.TEXT_NUMBER
            };


            // Create sheet in "Sheets" folder 
            Sheet newSheet = _smartsheetClient.SheetResources.CreateSheet(new Sheet
            {
                Name = "newsheetTest002",
                Columns = new Column[] { columnA, columnB }
            }
            );
            
            //Add new sheet id to list for TestCleanup
            _testSheetIds.Add((long)newSheet.Id);

            Sheet newSheet2 = _smartsheetClient.SheetResources.CreateSheet(new Sheet
            {
                Name = "newsheetTest002",
                Columns = new Column[] { columnA, columnB }
            }
            );

            //Add new sheet id to list for TestCleanup
            _testSheetIds.Add((long)newSheet2.Id);

            evaluate(
                SheetHelper.CompareForCreateSheets(newSheet, newSheet2),
                "Test_CreateSheet002",
                "Test_CreateSheet002",
                SheetHelper.DisplayText(newSheet),
                (SheetHelper.DisplayText(newSheet2)));
        }

        [TestMethod, TestCategory("API_CreateSheets"), Description("Valdates text column can not have a star symbol")]
        public void Test_CreateSheet003()
        {
            Column columnA = new Column
            {
                Title = "TitleA",
                Primary = true,
                Type = ColumnType.TEXT_NUMBER,
                Symbol = Symbol.STAR
            };

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
                Type = ColumnType.PICKLIST,
                Symbol = Symbol.PROGRESS
            };

            //try to create sheet and catch any exceptions
            Exception exception = null;
            try
            {
                Sheet newSheet = _smartsheetClient.SheetResources.CreateSheet(new Sheet
                {
                    Name = "newsheetTest001",
                    Columns = new Column[] { columnA, columnB, columnC }
                }
                );
            }
            catch (Exception e)
            {
                exception = e;
            }

            // Validate and exception was thrown and that the message is correct
            evaluate(
                null != exception,
                "Test_CreateSheet003",
                "No exception found",
                "",
                "");

            evaluate(
                exception.Message.Equals("Column type of TEXT_NUMBER does not support symbol of type STAR."),
                "Test_CreateSheet003",
                "Exception message not as expected",
                exception.Message,
                "");
        }

        [TestMethod, TestCategory("API_CreateSheets"), Description("Valdates only one column can be primary")]
        public void Test_CreateSheet004()
        {
            // Create Columns - two are primary
            Column columnA = new Column
            {
                Title = "TitleA",
                Primary = true,
                Type = ColumnType.TEXT_NUMBER,
            };

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
                Type = ColumnType.PICKLIST,
                Symbol = Symbol.PROGRESS
            };

            // Attempt to create sheet
            Exception exception = null;
            try
            {
                // Create sheet in "Sheets" folder
                Sheet newSheet = _smartsheetClient.SheetResources.CreateSheet(new Sheet
                {
                    Name = "newsheetTest001",
                    Columns = new Column[] { columnA, columnB, columnC }
                }
                );
            }
            catch (Exception e)
            {
                exception = e;
            }

            // Validate and exception was thrown and that the message is correct
            evaluate(
                null != exception,
                "Test_CreateSheet004",
                "No exception found",
                "",
                "");

            evaluate(
                exception.Message.Equals("One and only one column must be primary."),
                "Test_CreateSheet004",
                "Exception message not as expected",
                exception.Message,
                "");
        }

        [TestMethod, TestCategory("API_CreateSheets"), Description("Valdates sheet must have one primary column")]
        public void Test_CreateSheet005()
        {
            // Create columns - all not primary
            Column columnA = new Column
            {
                Title = "TitleA",
                Primary = false,
                Type = ColumnType.TEXT_NUMBER,
            };

            Column columnB = new Column
            {
                Title = "TitleB",
                Primary = false,
                Type = ColumnType.TEXT_NUMBER
            };

            Column columnC = new Column
            {
                Title = "TitleC",
                Primary = false,
                Type = ColumnType.PICKLIST,
                Symbol = Symbol.PROGRESS
            };

            Exception exception = null;

            // Try to create sheet in "Sheets" folder
            try
            {
                Sheet newSheet = _smartsheetClient.SheetResources.CreateSheet(new Sheet
                {
                    Name = "newsheetTest001",
                    Columns = new Column[] { columnA, columnB, columnC }
                }
                );
            }
            catch (Exception e)
            {
                exception = e;
            }

            // Validate and exception was thrown and that the message is correct
            evaluate(
                null != exception,
                "Test_CreateSheet005",
                "No exception found",
                "",
                "");

            evaluate(
                exception.Message.Equals("One and only one column must be primary."),
                "Test_CreateSheet005",
                "Exception message not as expected",
                exception.Message,
                "");
        }

        [TestMethod, TestCategory("API_CreateSheets"), Description("Test designed to fail to demonlsrate logging")]
        public void Test_CreateSheet006()
        {
            //Create Columns
            Column columnA = new Column
            {
                Title = "TitleA",
                Primary = false,
                Type = ColumnType.CHECKBOX,
                Symbol = Symbol.STAR
            };

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
                Type = ColumnType.PICKLIST,
                Symbol = Symbol.PROGRESS
            };


            // Create sheet in "Sheets" folder
            Sheet newSheet = _smartsheetClient.SheetResources.CreateSheet(new Sheet
            {
                Name = "newsheetTest001",
                Columns = new Column[] { columnA, columnB, columnC }
            }
            );

            //Add new sheet id to list for TestCleanup
            _testSheetIds.Add((long)newSheet.Id);

            // Manually creating a sheet with expected values to compare. 
            Sheet expectedSheet = new Sheet
            {
                Name = "newsheetTest001",
                AccessLevel = AccessLevel.OWNER,
                Columns = new Column[] { columnA, columnB, columnC }
            };

            //Force Failure
            evaluate(
                false,
                "Test_CreateSheet006",
                "Test_CreateSheet006",
                 SheetHelper.DisplayText(newSheet),
                 SheetHelper.DisplayText(expectedSheet));
        }
    }
}
