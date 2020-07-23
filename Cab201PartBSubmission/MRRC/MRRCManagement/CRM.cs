using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace MRRCManagement
{
    // This class which provides methods to specifically extract, load, write, delete and save customers to the CRM file
    public class CRM
    {
        public string CrmFile { get; set; }
        public List<Customer> CustomerList = new List<Customer>();

        #region CRM Constructor Operations
        public CRM(string CrmFile)
        {
            if (File.Exists(CrmFile))
            {// If file exist, load files into private variables 
                this.CrmFile = CrmFile;
                // load list of customers
                LoadFromFile();

            }
            // If fleet file doesn't exist, throw  file exception
            else
            {
                throw new FileException($"Error: Specified file path {CrmFile} does not contain customers.csv file");
            }
        }
        #endregion

        #region CRM Standard Class Methods
        // This method adds the provided customer to the customer list if the customer ID doesn’t
        // already exist in the CRM. It returns true if the addition was successful (the customer
        // ID didn’t already exist in the CRM) and false otherwise.
        public bool AddCustomer(Customer customer)
        {
            // Use the Any method on customer list to check if it already contains the CustomerID.
            // If if theres a match, false is returned so the customer will not be added.
            if (CustomerList.Any(archivedCustomer => archivedCustomer.ID == customer.ID))
            {
                return false;
            }
            else
            {
                // if the customer id does not already exist, the customer can be added. return true
                CustomerList.Add(customer);
                return true;
            }
        }

        // This method removes the customer with the provided customer ID from the CRM if they
        // are not currently renting a vehicle. It returns true if the removal was successful,
        // otherwise it returns false.
        public bool RemoveCustomer(int ID, Fleet fleet)
        {
            if (!(fleet.IsRenting(ID)))
            {
                CustomerList.Remove(GetCustomer(ID));
                CsvOperations.DeleteRecord("customers.csv", ID.ToString(), CrmFile, 0);
                return true;
            }
            return false;

        }

        // This method returns the list of current customers.
        public List<Customer> GetCustomers()
        {
            return CustomerList;
        }

        // This method returns the customer who matches the provided ID.
        public Customer GetCustomer(int ID)
        {
            // Using firstordefault method to return the customer that matches the provided id
            // Returns null if there is no customer that matches the provided ID.
            return CustomerList.FirstOrDefault(customer => customer.ID == ID);
        }

        // This method saves the current state of the CRM to file.
        public void SaveToFile()
        {
            // create newfile
            string newFile = "customers.csv";
            // Write field headers at the top of the new file
            string csvFieldHeader = "ID," + "Title," + "FirstName," + "LastName," + "Gender," + "DOB\n";
            File.WriteAllText(newFile, csvFieldHeader);

            // Sort the customer list by ID
            CustomerList = CustomerList.OrderBy(order => order.ID).ToList();
            // For each of the vehicles added into the existing vehicles list, write to the new csv file.
            foreach (Customer customer in CustomerList)
            {
                CsvOperations.WriteToCsv(newFile, customer.ToCSVstring());
            }

            // delete old file
            File.Delete(CrmFile);
            // move new file to old file directory
            File.Move(newFile, CrmFile);
        }

        // This method loads the state of the CRM from file.
        public void LoadFromFile()
        {
            // create customers datatable
            DataTable customers = new DataTable();
            // load list of rentals and store into datable
            CsvOperations.CsvToTable(CrmFile, customers);

            // Create string arrays for each attribute of a customer and store the data from the datable into the string arrays.
            // (Note that string array variables starting with lower case letters need to be converted to their appropriate
            //  Data type. When converted, they will encompass uppercase lettering.
            string[] id = customers.AsEnumerable().Select(row => row.Field<string>("ID")).ToArray();
            string[] Title = customers.AsEnumerable().Select(row => row.Field<string>("Title")).ToArray();
            string[] FirstName = customers.AsEnumerable().Select(row => row.Field<string>("FirstName")).ToArray();
            string[] LastName = customers.AsEnumerable().Select(row => row.Field<string>("LastName")).ToArray();
            string[] gender = customers.AsEnumerable().Select(row => row.Field<string>("Gender")).ToArray();
            string[] dob = customers.AsEnumerable().Select(row => row.Field<string>("DOB")).ToArray();
            //Convert id string arr to int Array
            int[] CustomerID = Array.ConvertAll(id, int.Parse);
            // Convert gender string arr to Enum array
            Customer.Gender[] Gender = gender.Select(s => Enum.Parse(typeof(Customer.Gender), s)).Cast<Customer.Gender>().ToArray();
            // Convert dob string arr to DateTime array
            DateTime[] DOB = Array.ConvertAll(dob, DateTime.Parse);

            // The number of customers in the customers csv file is equal to the number of lines contained within the file.
            // Since ID is a primary field, it will not have missing entries, so we count the size of the ID array
            // to determine the number of customers in the csv file.
            int numCustomers = CustomerID.Length;

            // Using a loop, create a list of customer by instatiating the customer class and add to list.
            // Import values from the arrays above (taken from the csv file) into the constructor of customer class.
            for (int i = 0; i < numCustomers; i++)
            {
                CustomerList.Add(new Customer(CustomerID[i], Title[i], FirstName[i], LastName[i], Gender[i], DOB[i]));
            }
        }
#endregion
    }

}
