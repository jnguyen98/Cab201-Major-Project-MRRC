using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MRRCManagement
{
    // Class to print datatable to the console in tabular format
    public static class PrintDataTable
    {
        // The three helper methods below have been adapted and modified from an online source:
        // https://www.codeproject.com/Tips/1147879/Print-DataTable-to-Console-and-more

        #region Print DataTable To Console

        public static void PrintTable(this DataTable dataTable, params string[] columnNames)
        {
            PrintDataRows(dataTable, dataTable.AsEnumerable(), columnNames);
        }

        #endregion

        #region Print Rows of DataTable
        private static void PrintDataRows(DataTable DataTable, IEnumerable<DataRow> DataRows, string[] columnNames)
        {
            // if there are elements within datatable which means the table isn't null, write:
            if (DataTable != null && string.IsNullOrEmpty(DataTable.TableName) == false)
                Console.WriteLine("{0}:", DataTable.TableName);

            // if datatable is null or if there are no data sets or records and either has a header or no header, print to console:
            if (DataTable == null || DataRows.Count() == 0)
            {
                Console.WriteLine("There is no data in the csv file");
                Console.WriteLine();
                return;
            }

            // Get columns of datatable and store into Columns list
            DataColumn[] Columns = GetColumns(DataTable, columnNames);
            // if there are no elements within the columns, do:
            if (Columns.Length == 0)
            {
                Console.WriteLine("There is no data in the csv file");
                Console.WriteLine();
                return;
            }

            int[] Lengths = Columns.Select(c => c.ColumnName.Length).ToArray();

            foreach (DataRow row in DataRows)
            {
                CalculateLengths(row, Columns, Lengths);
            }

            // parts accompanying the borders of the table
            string header = top_left.ToString();
            string separator = middle_left.ToString();
            string footer = bottom_left.ToString();
            string formatHeaders = verticl_bar.ToString();
            string format = verticl_bar.ToString();

            // Formatting the borders
            int k = 0;
            for (; k < Columns.Length - 1; k++)
            {
                string horizontal = new string(horizontal_bar, Lengths[k] + 2);
                header += horizontal + top_center;
                separator += horizontal + middle_center;
                footer += horizontal + bottom_center;

                string cellFormat = string.Format(" {{{0},-{1}}} {2}", k + 1, Lengths[k], verticl_bar);
                formatHeaders += cellFormat;
                format += cellFormat;
            }

            k = Columns.Length - 1;
            if (k >= 0)
            {
                string horizontal = new string(horizontal_bar, Lengths[k] + 2);
                header += horizontal + top_right;
                separator += horizontal + middle_right;
                footer += horizontal + bottom_right;

                string cellFormat = string.Format(" {{{0},-{1}}} {2}", k + 1, Lengths[k], verticl_bar);
                formatHeaders += cellFormat;
                format += cellFormat;
            }

            object[] objects = new object[Columns.Length + 1];

            Console.WriteLine(header);

            objects[0] = string.Empty;
            for (int i = 0; i < Columns.Length; i++)
                objects[i + 1] = Columns[i];
            Console.WriteLine(formatHeaders, objects);

            Console.WriteLine(separator);

            foreach (DataRow row in DataRows)
            {
                for (int i = 0; i < Columns.Length; i++)
                {
                    object obj = row[Columns[i]];

                    string str = null;

                    str = string.Format("{0}", (obj == DBNull.Value || obj == null ? "null" : obj));

                    objects[i + 1] = str;
                }

                Console.WriteLine(format, objects);
            }
            Console.WriteLine(footer);

            Console.WriteLine();
        }
        // Method to grab columns from the Datatable
        private static DataColumn[] GetColumns(DataTable dataTable, string[] columnNames)
        {
            // If column names is not null do:
            if (columnNames != null && columnNames.Length > 0)
            {
                return columnNames.Join(dataTable.Columns.Cast<DataColumn>(), n => n, c => c.ColumnName, (n, c) => c).ToArray();
            }
            // Otherwise:
            else
            {
                return dataTable.Columns.Cast<DataColumn>().ToArray();
            }
        }
        // Method to calculate length of Datatable 
        private static void CalculateLengths(DataRow row, DataColumn[] Columns, int[] Lengths)
        {
            for (int i = 0; i < Columns.Length; i++)
            {
                object Object = row[Columns[i]];

                string String;

                String = string.Format("{0}", (Object == DBNull.Value || Object == null ? "null" : Object));

                if (Lengths[i] < String.Length)
                {
                    Lengths[i] = String.Length;
                }
            }
        }

        #endregion

        #region Border (Customizable)

        public const char ascii_minus = '-';
        public const char ascii_verticl_bar = '|';
        public const char ascii_plus = '+';
        public const char extended_ascii_horizontal_bar = '─';
        public const char extended_ascii_verticl_bar = '│';
        public const char extended_ascii_top_left = '┌';
        public const char extended_ascii_top_center = '┬';
        public const char extended_ascii_top_right = '┐';
        public const char extended_ascii_middle_left = '├';
        public const char extended_ascii_middle_center = '┼';
        public const char extended_ascii_middle_right = '┤';
        public const char extended_ascii_bottom_left = '└';
        public const char extended_ascii_bottom_center = '┴';
        public const char extended_ascii_bottom_right = '┘';

        public static char horizontal_bar;
        public static char verticl_bar;
        public static char top_left;
        public static char top_center;
        public static char top_right;
        public static char middle_left;
        public static char middle_center;
        public static char middle_right;
        public static char bottom_left;
        public static char bottom_center;
        public static char bottom_right;

        // Method printed to tabular view that is clear
        public static void BorderClear()
        {
            horizontal_bar = ' ';
            verticl_bar = ' ';
            top_left = ' ';
            top_center = ' ';
            top_right = ' ';
            middle_left = ' ';
            middle_center = ' ';
            middle_right = ' ';
            bottom_left = ' ';
            bottom_center = ' ';
            bottom_right = ' ';
        }
        // Method printed with asci chars
        public static void BorderASCII()
        {
            horizontal_bar = ascii_minus;
            verticl_bar = ascii_verticl_bar;
            top_left = ascii_plus;
            top_center = ascii_plus;
            top_right = ascii_plus;
            middle_left = ascii_plus;
            middle_center = ascii_plus;
            middle_right = ascii_plus;
            bottom_left = ascii_plus;
            bottom_center = ascii_plus;
            bottom_right = ascii_plus;
        }
        // Method printed with extended asci chars
        public static void BorderExtendedASCII()
        {
            horizontal_bar = extended_ascii_horizontal_bar;
            verticl_bar = extended_ascii_verticl_bar;
            top_left = extended_ascii_top_left;
            top_center = extended_ascii_top_center;
            top_right = extended_ascii_top_right;
            middle_left = extended_ascii_middle_left;
            middle_center = extended_ascii_middle_center;
            middle_right = extended_ascii_middle_right;
            bottom_left = extended_ascii_bottom_left;
            bottom_center = extended_ascii_bottom_center;
            bottom_right = extended_ascii_bottom_right;
        }
        #endregion 
    }
}
