using MRRCManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace MRRC
{
    public class InnerSubInterfaceRentals : SubInterface
    {
        #region Fields
        private string id;
        private string RegistrationRent;
        private string RegoToReturn;
        private string rentalTime;

        private int RentalTime;
        private int CustomerIDRent;
        #endregion

        #region Constructor
        public InnerSubInterfaceRentals(Fleet fleet, CRM crm) : base(fleet, crm) 
        {
          
        }
        #endregion

        #region Validation user input for rentals

        private void CustomerIDValidInput()
        {
            bool IDisValid;
            do
            {
                Console.Write("Please enter the customer ID of the customer to rent: ");
                id = Console.ReadLine();
                IDisValid = int.TryParse(id, out CustomerIDRent);
                if (CustomerIDRent < 0)
                {
                    Console.WriteLine("\nYou entered '{0}', which is not a positive integer", CustomerIDRent);
                    Console.WriteLine("Enter an ID which is a positive integer\n");
                    IDisValid = false;
                }
                else if (!int.TryParse(id, out CustomerIDRent))
                {
                    Console.WriteLine("\nYou entered '{0}', which is not an integer", id);
                    Console.WriteLine("Enter an ID which is an integer\n");
                    IDisValid = false;
                }
                // If there is a match of the customerID within the rentals list, then id is not valid as a customer cannot rent more than 1 vehicle.
                else if (Fleet.Rentals.Any(archivedID => archivedID.Value == CustomerIDRent))
                {
                    Console.WriteLine($"\nCould not rent customer id '{CustomerIDRent}', customer is currently renting a vehicle.\n");
                    IDisValid = false;
                }
                // if the provided customerID to rent does not exist in the customer list, then the id is not valid.
                else if (!Crm.CustomerList.Any(archivedCustomer => archivedCustomer.ID == CustomerIDRent))
                {
                    Console.WriteLine($"\nCould not rent customer id '{CustomerIDRent}', customer id does not exist in the customer list.\n");
                    IDisValid = false;
                }
                

            } while (!IDisValid);
        }

        // Validation check when user enters the registration of a vehicle
        private void RegistrationValidInput()
        {
            bool RegoIsValid;
            do
            {
                Console.Write("Please enter the Registration of the vehicle to be rented*: ");
                RegistrationRent = Console.ReadLine();
                // Perform complex regex match for user input validation
                RegoIsValid = Regex.IsMatch(RegistrationRent, @"^\d{3}[A-Z]{3}$");
                if (!RegoIsValid)
                {
                    Console.WriteLine($"\nYou entered {RegistrationRent}, which is not in the correct format for the vehicle registration");
                    Console.WriteLine("Please enter the registration in the following regex pattern format: ^\\d{3}[A-Z]{3}$\n");
                }
                // If there is not match of the registration within the vehicles list, then rego is not valid as it doesn't exist in the vehicles csv file.
                else if (!Fleet.VehicleList.Any(archivedVehicle => archivedVehicle.registration == RegistrationRent))
                {
                    Console.WriteLine($"\nCould not accept vehicle registration '{RegistrationRent}' as it doesn't exist in the fleet list.\n");
                    RegoIsValid = false;
                }
                else if (Fleet.Rentals.Any(archivedRego => archivedRego.Key == RegistrationRent))
                {
                    Console.WriteLine($"\nCould not accept vehicle registration '{RegistrationRent}', the vehicle is currently being rented.\n");
                    RegoIsValid = false;
                }
            } while (!RegoIsValid);
        }

        // Validation check when user enters the registration of a vehicle
        private void RegoReturnValidInput()
        {
            bool RegoIsValid;
            do
            {
                Console.Write("Please enter the Registration of the vehicle to be returned*: ");
                RegoToReturn = Console.ReadLine();
                // Perform complex regex match for user input validation
                RegoIsValid = Regex.IsMatch(RegoToReturn, @"^\d{3}[A-Z]{3}$");
                if (!RegoIsValid)
                {
                    Console.WriteLine($"\nYou entered {RegoToReturn}, which is not in the correct format for the vehicle registration");
                    Console.WriteLine("Please enter the registration in the following regex pattern format: ^\\d{3}[A-Z]{3}$\n");
                }
                // If there is not match of the registration within the RegoRented list, then rego is not valid as it doesn't exist..
                else if (!Fleet.Rentals.Any(archivedRego => archivedRego.Key == RegoToReturn))
                {
                    Console.WriteLine($"\nCould not return vehicle registration '{RegoToReturn}' as it doesn't exist in the rental list.\n");
                    RegoIsValid = false;
                }
            } while (!RegoIsValid);
        }
        private void RentalTimeValidationInput()
        {
            bool IDisValid;
            do
            {
                Console.Write("Please enter rental time in days: ");
                rentalTime = Console.ReadLine();
                IDisValid = int.TryParse(rentalTime, out RentalTime);
                if (RentalTime < 0)
                {
                    Console.WriteLine($"\nYou entered '{RentalTime}', which is not a positive integer for days");
                    Console.WriteLine("Enter a time in days which is a positive integer\n");
                    IDisValid = false;
                }
                else if (RentalTime > 365)
                {
                    Console.WriteLine($"\nYou entered '{RentalTime}', which exceeds the maximum rental time in days");
                    Console.WriteLine("Enter a time in days which is less than 365 days\n");
                    IDisValid = false;
                }
                else if (!int.TryParse(rentalTime, out RentalTime))
                {
                    Console.WriteLine($"\nYou entered '{rentalTime}', which is not an integer which could be converted to days");
                    Console.WriteLine("Enter a time in days which is an integer\n");
                    IDisValid = false;
                }

            } while (!IDisValid);
        }
        #endregion

        /* Methods applied in Search vehicle menu have been adapted and referenced from the code example in week 9 on Qut Cab201 blackboard 2020.
           Code example file name is: Program.cs
           Link: https://blackboard.qut.edu.au/bbcswebdav/pid-8400584-dt-content-rid-31313832_1/xid-31313832_1 */

        #region Search Vehicle
        // Method for the Search Vehiicle Submenu
        public void SearchVehicleSubMenu(bool clear = true)
        {
            if (clear)
                Console.Clear();
            try
            {
                setUpVehicles(out Fleet); // Set up vehicles fleet
                while (true)
                {
                    Console.WriteLine("### Search Vehicle Submenu ###\n");
                    getQuery(out ArrayList query); // Get Query from user
                    if (query[0].Equals("QUIT"))
                    {
                        // Clear the vehicle list in case user decides to go back to the search submenu
                        Fleet.vehicles.Clear();
                        // Prompt end of menu screen
                        EndOfMenuScreen(() => SubMenu("Rentals"), () => SearchVehicleSubMenu());
                    }
                    else if (query[0].Equals("Show All")) // Show all vehicles if query is blank line
                    {
                        search(Fleet);
                    }
                    else if (query[0].Equals("CLEAR"))
                    {
                        SearchVehicleSubMenu();
                    }
                    else
                    {
                        search(new RPN(query, Fleet), Fleet); // convert query to RPN and search
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n{ex.Message}");
                // Prompt end of menu screen
                EndOfMenuScreen(() => SubMenu("Rentals"), () => SearchVehicleSubMenu(), false, false);
            }
}
        // Method to set up vehicles for the search
        private void setUpVehicles(out Fleet fleetVehicles)
        {
            // Create fleet instance object inside the method
            fleetVehicles = new Fleet(Fleet.FleetFile, Fleet.RentalsFile);
            // Create vehicle list to store vehicles and their attributes as a string
            List<string> vehicles = new List<string>();
            // For each vehicle in unrented vehicle list
            foreach (Vehicle vehicle in fleetVehicles.GetFleet(false))
            {
                // convert attribute list to a string using the join method
                vehicles.Add(string.Format($"{vehicle.registration},{vehicle.grade},{vehicle.make},{vehicle.model},{vehicle.year},{vehicle.numSeats}," +
                                           $"{vehicle.transmission},{vehicle.fuel},{vehicle.GPS},{vehicle.sunRoof},{vehicle.colour}"));
            }

            // For each vehicle, insert the csv string.
            for (int i = 0; i < vehicles.Count; i++)
            {
                fleetVehicles.insertVehicle(vehicles[i]);
            }
        }

        // Method to perform search on the querey
        private void search(RPN rpn, Fleet fleet)
        {
            Console.WriteLine("\n--------------------------------------------- Searching available vehicles ---------------------------------------------");

            // Create new datatable for available vehicles (unrented)
            DataTable AvaiableVehicles = new DataTable();
            // Fill table for available vehicles
            CsvOperations.SearchToTable(fleet, AvaiableVehicles);
            // Show search to table datatable
            CsvOperations.ShowDataTable(AvaiableVehicles, fleet.FleetFile, "ASCII", true);

            Console.Write("\nInfix expression:    ");
            foreach (string token in rpn.InfixTokens)
            {
                Console.Write(token + " ");
            }
            Console.WriteLine();

            Console.Write("Postfix expression:  ");
            foreach (string token in rpn.PostfixTokens)
            {
                Console.Write(token + " ");
            }
            Console.WriteLine();
            Console.WriteLine();

            fleet.search(out string[] result, rpn);

            if (result.Length == 0)
                throw new Exception("No match found");

            List<string> results = new List<string>();
            foreach (string res in result)
            {
                // display vehicle by registration plate (rego)
                Vehicle v;
                for (int i = 0; i < fleet.vehicles.Count; i++)
                {   // go through, find the vehicle by rego
                    v = (Vehicle)fleet.vehicles[i];
                    if (v.registration == res)
                    {
                        results.Add(v.ToCSVstring());
                        break;
                    }
                }
            }

            Console.WriteLine("\n-------------------------------------------------- Query Search Output -------------------------------------------------");
            // Create new datatable for query result for searched vehicles
            DataTable resultTable = new DataTable();
            // fill query result table
            CsvOperations.SearchToTable(fleet, resultTable, results);
            // show datatable
            CsvOperations.ShowDataTable(resultTable, fleet.FleetFile, "Extended", true);
        }
        // method to show all vehicles if query is a blank line
        private void search(Fleet fleet)
        {
            Console.WriteLine("\n-------------------------------------------- Showing all available vehicles --------------------------------------------");
            // Create new datatable for all vehicles (unrented)
            DataTable AllVehicles = new DataTable();
            // Fill table for all vehicles
            CsvOperations.SearchToTable(fleet, AllVehicles, null);
            // Show search to table datatable
            CsvOperations.ShowDataTable(AllVehicles, fleet.FleetFile, "ASCII", true);

        }
        // Method to get the query from user input 
        private string getQuery(out ArrayList query)
        {
            // Accept user query and implement validation
            query = new ArrayList();
            string queryText = "";
            Console.Write("\nEnter your search query, or just hit Enter to quit:  ");
            string temp = Console.ReadLine();
            // checking for blank query
            bool blankQuery = Regex.IsMatch(temp, @"^\s+$+");
            // checking for clear query
            bool clearQuery = Regex.IsMatch(temp.ToUpper(), @"CLEAR");
            if (!blankQuery)
            {
                if (!clearQuery)
                {
                    if (temp.Length > 0)
                    {
                        // separate parenthesis before splitting string
                        for (int i = 0; i < temp.Length; i++)
                        {
                            if (temp[i].Equals('(') || temp[i].Equals(')'))
                            {
                                queryText += " ";
                                queryText += temp[i];
                                queryText += " ";
                            }
                            else
                            {
                                queryText += temp[i];
                            }

                        }

                        queryText = queryText.ToUpper();
                        query.AddRange(queryText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)); // Split query into tokens (default delimiter is space)
                    }
                    else
                    {
                        query.Add("QUIT");
                    }
                }
                else
                {
                    query.Add("CLEAR");
                }
            }
            // if blank query, then show all vehicles
            else
            {
                query.Add("Show All");
            }

            return temp;
        }
        #endregion

        #region Add new rental submenu
        public void AddNewRentalSubMenu(bool clear = true)
        {
            if (clear)
                Console.Clear();

            Console.WriteLine("### Rent Vehicle Submenu ###\n\n");
            CustomerIDValidInput();
            RegistrationValidInput();
            RentalTimeValidationInput();
            double dailyRate = Fleet.GetVehicle(RegistrationRent).dailyRate;
            if (Fleet.RentVehicle(RegistrationRent, CustomerIDRent))
            {
                string days = RentalTime.ToString() == "1" ? "day" : "days";
                double TotalCost = dailyRate * RentalTime;
                Console.WriteLine($"\nSuccessfully added customer '{CustomerIDRent}' renting '{RegistrationRent}'" +
                                  $" for '{RentalTime}' {days} with a total cost of {TotalCost.ToString("C")}");
                Fleet.SaveRentalsToFile();
            }
            else
            {
                Console.WriteLine($"\nCannot add customer '{CustomerIDRent}' with '{RegistrationRent}' to the rental list.\n");
            }
            LastMRRCscreen(() => SubMenu("Rentals"), () => AddNewRentalSubMenu());
        }
        #endregion

        #region Return vehicle submenu
        //Method
        public void ReturnVehicleSubMenu(bool clear = true)
        {
            if (clear)
                Console.Clear();

            Console.WriteLine("### Return Vehicle Submenu ###\n");
            RegoReturnValidInput();
            // Store customer ID to be returned
            int CustomerID = Fleet.ReturnVehicle(RegoToReturn);
            // Return is unsuccessful if returned customer ID is -1
            if (CustomerID == -1)
            {
                Console.WriteLine($"\nReturn Unsuccessful. Vehicle '{RegoToReturn}' is not currently being rented.");
            }
            else
            {
                // Delete Record containing rego and customer id 
                CsvOperations.DeleteRecord("rentals.csv", RegoToReturn, Fleet.RentalsFile, 0);
                // Remove rego and Id from rental dictionary
                foreach (var item in Fleet.Rentals.Where(kvp => kvp.Value == CustomerID).ToList())
                {
                    Fleet.Rentals.Remove(item.Key);
                }
                Console.WriteLine($"\nSuccessfully returned the Vehicle with the registration of '{RegoToReturn}' belonging to customer '{CustomerID.ToString()}'");
            }
            LastMRRCscreen(() => SubMenu("Rentals"), () => ReturnVehicleSubMenu());
        }


        #endregion
    }
}

