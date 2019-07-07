using System;
using System.Collections.Generic;
using System.Text;

using SmartsheetTestFramework.Common.Utilities;

using Smartsheet.Api.Models;

namespace SmartsheetTestFramework.Tests.API.Common.Helpers
{
    public static class SheetHelper
    {
        /// <summary>
        /// Returns a string version of a Sheet Object for test logging purposes
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public static string DisplayText(Sheet sheet)
        {
            ToStringHelper toStringHelper = new ToStringHelper();

            toStringHelper.Append("\r\nSheet\r\n");
            toStringHelper.AppendProperty("Id", sheet.Id);
            toStringHelper.AppendProperty("Name", sheet.Name);
            toStringHelper.AppendProperty("Permalink", sheet.Permalink);
            toStringHelper.AppendProperty("AccessLevel", sheet.AccessLevel);

            foreach (Column column in sheet.Columns)
            {
                toStringHelper.Append(ColumnHelper.DisplayText(column));
            }

            return toStringHelper.ToString();
        }
        /// <summary>
        /// Compares the fields in two Sheet objects that are relevant when Creating a sheet with the API
        /// </summary>
        /// <param name="currentSheet"></param>
        /// <param name="expectedSheet"></param>
        /// <returns></returns>
        public static bool CompareForCreateSheets(Sheet currentSheet, Sheet expectedSheet)
        {
            bool result = true;

            result &= ParameterChecker.EqualsIncludeNull(currentSheet.Name, expectedSheet.Name);
            result &= ParameterChecker.EqualsIncludeNull(currentSheet.AccessLevel, expectedSheet.AccessLevel);

            ColumnHelper.CompareForCreateSheets(currentSheet.Columns, expectedSheet.Columns);

            return result;
        }
    }
}
