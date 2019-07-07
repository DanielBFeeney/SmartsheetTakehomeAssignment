using System;
using System.Collections.Generic;
using System.Text;

using SmartsheetTestFramework.Common.Utilities;

using Smartsheet.Api.Models;

namespace SmartsheetTestFramework.Tests.API.Common.Helpers
{
    public static class ColumnHelper
    {
        /// <summary>
        /// Returns a string version of a Column Object for test logging purposes
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public static string DisplayText(Column column)
        {
            ToStringHelper toStringHelper = new ToStringHelper();

            toStringHelper.Append("\r\nColumn\r\n");
            toStringHelper.AppendProperty("Id", column.Id);
            toStringHelper.AppendProperty("Version", column.Version);
            toStringHelper.AppendProperty("Index", column.Index);
            toStringHelper.AppendProperty("Primary", column.Primary);
            toStringHelper.AppendProperty("Title", column.Title);
            toStringHelper.AppendProperty("Type", column.Type);
            toStringHelper.AppendProperty("Validation", column.Validation);
            toStringHelper.AppendProperty("Symbol", column.Symbol);
            toStringHelper.AppendProperty("AutoNumberFormat", column.AutoNumberFormat);

            return toStringHelper.ToString();
        }
        /// <summary>
        /// Compares the two Column Lists that are relevant when Creating a sheet with the API
        /// </summary>
        /// <param name="currentColumns"></param>
        /// <param name="expectedColumns"></param>
        /// <returns></returns>
        public static bool CompareForCreateSheets(IList<Column> currentColumns, IList<Column> expectedColumns)
        {
            bool result = true;

            if (null != currentColumns && null != expectedColumns)
            {
                result &= (currentColumns.Count == expectedColumns.Count);
            }

            foreach (Column expectedColumn in expectedColumns)
            {
                bool foundCol = false;
                foreach (Column currentColumn in currentColumns)
                {
                    foundCol &= ParameterChecker.EqualsIncludeNull(currentColumn.Title, currentColumn.Title);
                    foundCol &= ParameterChecker.EqualsIncludeNull(currentColumn.Type, currentColumn.Type);
                    foundCol &= ParameterChecker.EqualsIncludeNull(currentColumn.Index, currentColumn.Index);
                    foundCol &= ParameterChecker.EqualsIncludeNull(currentColumn.Primary, currentColumn.Primary);
                    foundCol &= ParameterChecker.EqualsIncludeNull(currentColumn.Symbol, currentColumn.Symbol);
                    foundCol &= ParameterChecker.EqualsIncludeNull(currentColumn.AutoNumberFormat, currentColumn.AutoNumberFormat);
                }
                result &= foundCol;
            }

            return result;
        }
    }
}
