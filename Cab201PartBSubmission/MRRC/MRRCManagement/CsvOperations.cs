using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRRCManagement
{
    public class CsvOperations
    {
        #region Border Property
        // A border style string variable for the table view in console
        public static string borderStyle { get; set; }
        #endregion

        #region Convert CSV to table
        // This method extracts data from a csv file and stores it into a datatable.
        public static void CsvToTable(string filePath, DataTable Name)
        {
            // Read file and doing a check to store lines which are not nullorwhitespace into string array
            string[] Lines = File.ReadAllLines(filePath).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            try
            {
                if (Lines.Length > 0)
                {
                    // First line to create the header
                    string firstLine = Lines[0];
                    string[] Fields = firstLine.Split(',');
                    // Each field is added to the data column in the dataTable
                    foreach (string Field in Fields)
                    {
                        Name.Columns.Add(new DataColumn(Field));
                    }

                    // Add rows
                    for (int row = 1; row < Lines.Length; row++)
                    {
                        // Split each line separated with a delimeter ',' into individual elements
                        string[] dataWords = Lines[row].Split(',');

                        // create new row
                        DataRow dataRow = Name.NewRow();
                        int columnIndex = 0;
                        // store data in their respective row and fields (column)
                        foreach (string Field in Fields)
                        {
                            dataRow[Field] = dataWords[columnIndex++];
                        }

                        // add to data table
                        Name.Rows.Add(dataRow);
                    }
                }
            }
            catch (Exception error)
            {
                throw new Exception("Something went wrong converting the csv file to table. Try to provide at least one record.", error);
            }
        }
        #endregion

        #region RentalsToTable
        // This method creates the rental data table for displaying rentals
        public static void RentalsToTable(Fleet fleet, DataTable Name)
        {
            // String array containing all fields
            string[] Fields = { "Registration", "CustomerID", "DailyRate"};

            // Add columns to rentals datatable
            foreach(string field in Fields)
            {
                Name.Columns.Add(new DataColumn(field));
            }

            // list which will contain all the lines
            List<string> totalLines = new List<string>();

            // Loop through dictionary and store all values into total lines, including dailyRate
            foreach(KeyValuePair<string, int> kvp in fleet.Rentals)
            {
                totalLines.Add(kvp.Key + "," + kvp.Value.ToString() + "," + fleet.GetVehicle(kvp.Key).dailyRate.ToString("C"));
            }
            // implement loop to add data lines to the row data
            for(int row = 0; row < totalLines.Count; row++)
            {
                string[] dataWords = totalLines[row].Split(',');

                DataRow dataRow = Name.NewRow();
                int columnIndex = 0;
                foreach (string Field in Fields)
                {
                    dataRow[Field] = dataWords[columnIndex++];
                }
                // add row to datatable
                Name.Rows.Add(dataRow);

            }
            
        }
        #endregion

        #region SearchToTable
        // This method creates the data table for the searched vehicles in the rentals menu
        public static void SearchToTable(Fleet fleet, DataTable Name, List<string> Result = null)
        {
            // String array containing all fields
            string[] Fields = { "Rego", "Grade", "Make", "Model", "Year", "Seats", "Transmission", "Fuel", "GPS", "SunRoof", "Colour"};
            // Add columns to rentals datatable
            foreach (string field in Fields)
            {
                Name.Columns.Add(new DataColumn(field));
            }

            // list which will contain all the lines
            List<string> totalLines = new List<string>();
            // If result is not parsed through or is null, do:
            if (Result == null)
            {     
                // For each vehicle in unrented vehicle list
                foreach (Vehicle vehicle in fleet.GetFleet(false))
                {
                    // convert attribute list to a string using the join method
                    totalLines.Add(string.Join(",", vehicle.GetAttributeList()));
                } 
            }
            // If result is parsed, do:
            else
            {
                totalLines = Result;
            }
            // implement loop to add data lines to the row data
            for (int row = 0; row < totalLines.Count; row++)
            {
                string[] dataWords = totalLines[row].Split(',');

                DataRow dataRow = Name.NewRow();
                int columnIndex = 0;
                foreach (string Field in Fields)
                {
                    dataRow[Field] = dataWords[columnIndex++];
                }
                // add row to datatable
                Name.Rows.Add(dataRow);

            }

        }
        #endregion

        #region Display the data table in a tabular format with border options
        // Method to display datatable to console
        public static void ShowDataTable(DataTable Name, string filePath, string BorderStyle, bool OtherTable = false)
        {
            // If its not the rental table, do csvtotable
            if (!OtherTable)
            {
                CsvToTable(filePath, Name);
            }
            switch (BorderStyle)
            {
                case "Clear":
                    PrintDataTable.BorderClear();
                    break;
                case "ASCII":
                    PrintDataTable.BorderASCII();
                    break;
                case "Extended":
                    PrintDataTable.BorderExtendedASCII();
                    break;
            }
            // Insert new line
            Console.Write("\n");
            // Print table
            Name.PrintTable();
        }
        #endregion

        #region Writing data to a specific csv file 
        // This method uses the streamwriter to write data to a specific file path and file extension
        // which in this case, is "csv".
        public static void WriteToCsv(string filePath, string toCsvString)
        {
            try
            {
                // Write to CSV
                using (StreamWriter file = new StreamWriter(filePath, true))
                {
                    file.WriteLine(toCsvString);
                }
            }
            catch (Exception error)
            {
                throw new Exception("Could not write data to Csv file", error);
            }
        }
        #endregion

        #region Deleting record from a specific csv file
        // This method creates a new file containing the same data as the previous file, excluding the desired record to be deleted
        public static void DeleteRecord(string fileName, string searchTerm, string filePath, int positionOfSearchTerm)
        {
            string newFile = fileName;
            bool deleted = false;
            try
            {
                string[] CsvLines = File.ReadAllLines(filePath);
                string csvFieldHeader = "";
                // Write headers into new file
                if (fileName == "fleet.csv")
                    csvFieldHeader = "Registration," + "Grade," + "Make," + "Model," + "Year," + "NumSeats," + "Transmission," + "Fuel," + "GPS," + "SunRoof," + "DailyRate," + "Colour\n";
                File.WriteAllText(newFile, csvFieldHeader);
                if (fileName == "rentals.csv")
                    csvFieldHeader = "Registration," + "CustomerID\n";
                File.WriteAllText(newFile, csvFieldHeader);
                if (fileName == "customers.csv")
                    csvFieldHeader = "ID," + "Title," + "FirstName," + "LastName," + "Gender," + "DOB\n";
                File.WriteAllText(newFile, csvFieldHeader);
                for (int i = 0; i < CsvLines.Length; i++)
                {
                    string[] fields = CsvLines[i].Split(',');
                    // write contents to new file until match is found
                    if (!(RecordExists(searchTerm, fields, positionOfSearchTerm)) && deleted)
                    { // only delete the first record that matches
                        if (fileName == "fleet.csv")
                        {
                            WriteToCsv(newFile, fields[0] + "," + fields[1] + "," + fields[2] + "," + fields[3] + "," + fields[4] + "," + fields[5]
                                        + "," + fields[6] + "," + fields[7] + "," + fields[8] + "," + fields[9] + "," + fields[10] + "," + fields[11]);
                        }
                        else if (fileName == "rentals.csv")
                        {
                            WriteToCsv(newFile, fields[0] + "," + fields[1]);
                        }
                        else if (fileName == "customers.csv")
                        {
                            WriteToCsv(newFile, fields[0] + "," + fields[1] + "," + fields[2] + "," + fields[3] + "," + fields[4] + "," + fields[5]);
                        }
                    }
                    // when match is found, flag turns true, so the writing of the new file stops.
                    else
                    {
                        deleted = true;
                    }
                }
                File.Delete(filePath);
                File.Move(newFile, filePath);
            }
            catch (Exception error)
            {
                throw new Exception("Could not delete data from Csv file", error);
            };
        }
        #endregion

        #region Boolean checking whether record exists in the csv file
        // This boolean takes in a search term and iterates through all lines of the csv file
        // and checking whether theres a match. Returns true if succesful, false if no match.
        public static bool RecordExists(string searchTerm, string[] record, int positionOfSearchTerm)
        {
            if (record[positionOfSearchTerm].Equals(searchTerm))
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
