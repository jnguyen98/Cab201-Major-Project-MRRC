using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace MRRCManagement
{
    // This class which provides methods to specifically extract, load, write, delete and save vehicles to the fleet file
    public class Fleet
    {
        #region Fields 
        public string FleetFile { get; set; }
        public string RentalsFile { get; set; }
        private string[] RentalsCsvLines;
        public List<Vehicle> VehicleList = new List<Vehicle>();
        public List<string> RentalListRego = new List<string>();
        public List<int> RentalListID = new List<int>();
        public List<double> DailyCost = new List<double>();

        // For search functionality
        public ArrayList vehicles = new ArrayList();
        public List<string> VehicleAttributes = new List<string>();
        public SortedList attributeSets;
        #endregion

        /* Methods applied for Search Functionality have been adapted and referenced from the code example in week 9 on Qut Cab201 blackboard 2020.
           Code example file name is: Program.cs
           Link: https://blackboard.qut.edu.au/bbcswebdav/pid-8400584-dt-content-rid-31313832_1/xid-31313832_1 */

        #region Search Functionality Methods For Fleet
        // method to insert a vehicle
        public void insertVehicle(string vehicleCSV)
        {
            int idx;
            HashSet<string> hs;
            Vehicle V = new Vehicle(vehicleCSV);
            vehicles.Add(V);
            idx = attributeSets.IndexOfKey(V.registration.ToUpper());
            if (idx >= 0)
            {   // here if registration set found
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // get set
                hs.Add(V.registration); // add to set
                attributeSets.SetByIndex(idx, hs);  // save set (replaces old set)
            }
            idx = attributeSets.IndexOfKey(V.grade.ToString().ToUpper());
            if (idx >= 0)
            {   // here if grade set found
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // get set
                hs.Add(V.registration); // add to set
                attributeSets.SetByIndex(idx, hs);  // save set (replaces old set)
            }
            idx = attributeSets.IndexOfKey(V.make.ToUpper());
            if (idx >= 0)
            {   // here if Make set found
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // get set
                hs.Add(V.registration);// add to set
                attributeSets.SetByIndex(idx, hs);  // save set (replaces old set)
            }
            idx = attributeSets.IndexOfKey(V.model.Replace(" ", "-").ToUpper());
            if (idx >= 0)
            {   // here if model set found
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // get set
                hs.Add(V.registration);// add to set
                attributeSets.SetByIndex(idx, hs);  // save set (replaces old set)
            }
            idx = attributeSets.IndexOfKey(V.year.ToString().ToUpper());
            if (idx >= 0)
            {   // here if year set found
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // get set
                hs.Add(V.registration);// add to set
                attributeSets.SetByIndex(idx, hs);  // save set (replaces old set)
            }
            idx = attributeSets.IndexOfKey(V.numSeats.ToString() + "-SEATER");
            if (idx >= 0)
            {   // here if numSeats set found
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // get set
                hs.Add(V.registration);// add to set
                attributeSets.SetByIndex(idx, hs);  // save set (replaces old set)
            }
            idx = attributeSets.IndexOfKey(V.transmission.ToString().ToUpper());
            if (idx >= 0)
            {   // here if transmission set found
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // get set
                hs.Add(V.registration);// add to set
                attributeSets.SetByIndex(idx, hs);  // save set (replaces old set)
            }
            idx = attributeSets.IndexOfKey(V.fuel.ToString().ToUpper());
            if (idx >= 0)
            {   // here if fuel set found
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // get set
                hs.Add(V.registration);// add to set
                attributeSets.SetByIndex(idx, hs);  // save set (replaces old set)
            }
            idx = attributeSets.IndexOfKey(V.GPS.ToString().ToUpper() == "TRUE" ? "GPS" : "-");
            if (idx >= 0)
            {   // here if gps set found
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // get set
                hs.Add(V.registration);// add to set
                attributeSets.SetByIndex(idx, hs);  // save set (replaces old set)
            }
            idx = attributeSets.IndexOfKey(V.sunRoof.ToString().ToUpper() == "TRUE" ? "SUNROOF" : "-");
            if (idx >= 0)
            {   // here if sunroof set found
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // get set
                hs.Add(V.registration);// add to set
                attributeSets.SetByIndex(idx, hs);  // save set (replaces old set)
            }
            idx = attributeSets.IndexOfKey(V.colour.ToUpper());
            if (idx >= 0)
            {   // here if colour set found
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // get set
                hs.Add(V.registration);// add to set
                attributeSets.SetByIndex(idx, hs);  // save set (replaces old set)
            }
        }

        public void search(out string[] result, RPN rpn)
        {
            // Create and instantiate a new empty Stack for result sets.
            Stack setStack = new Stack();
            HashSet<string> hs1;
            HashSet<string> hs2;
            HashSet<string> hs;
            int idx;
            String[] temp = new string[] { };
            for (int i = 0; i < rpn.PostfixTokens.Count; i++)
            {
                if (rpn.PostfixTokens[i].Equals("AND"))
                {
                    // pop two sets off the stack and apply Intersect, push back result
                    hs1 = (HashSet<string>)setStack.Pop();
                    hs2 = (HashSet<string>)setStack.Pop();
                    temp = hs1.ToArray<string>(); // copy the elements of the set hs1
                    hs = new HashSet<string>(temp); // make a deep copy of hs1
                    hs.IntersectWith(hs2);// apply the Intersect to the new set
                    setStack.Push(hs); // push a reference to a new set
                }
                else if (rpn.PostfixTokens[i].Equals("OR"))
                {
                    // pop two sets off the stack and apply Union
                    hs1 = (HashSet<string>)setStack.Pop();
                    hs2 = (HashSet<string>)setStack.Pop();
                    temp = hs1.ToArray<string>(); // copy the elements of the set hs1
                    hs = new HashSet<string>(temp); // make a deep copy of hs1
                    hs.UnionWith(hs2); // apply the Union to the new set
                    setStack.Push(hs); // push a reference to a new set
                }
                else
                {
                    // here if an operand
                    idx = attributeSets.IndexOfKey(rpn.PostfixTokens[i]); // identify attribute set
                    if (idx >= 0)
                    {
                        hs1 = (HashSet<string>)attributeSets.GetByIndex(idx);
                        setStack.Push(hs1); // note: pushing a reference, not the actual set
                    }
                    else
                    {
                        throw new FormatException("Invalid attribute" + rpn.PostfixTokens[i]);
                    }
                }
            }
            if (setStack.Count == 1)
            {
                //hs1 = (HashSet<string>)attributeSets.GetByIndex(1);
                hs1 = (HashSet<string>)setStack.Pop();
                result = hs1.ToList().ToArray();
            }
            else
            {
                throw new Exception("Invalid query");
            }
        }
        #endregion

        #region Fleet Constructor Operations
        public Fleet(string FleetFile, string RentalsFile)
        {
            if (File.Exists(FleetFile))
            {// If file exist, load files into private variables 
                this.FleetFile = FleetFile;
                // load list ofvehicles and rentals
                LoadVehiclesFromFile();
                // Create new list containing all vehicle attributes (has duplicates)
                List<string> AllVehicleAttributes = new List<string>();
                foreach (Vehicle vehicle in GetFleet(false))
                {
                    string removedBlanks = string.Join(",", vehicle.GetAttributeList()).Replace(" ", "-").Replace("-,", "").ToUpper();
                    AllVehicleAttributes.AddRange(removedBlanks.Split(','));
                }
                //Remove all duplicate values out from the vehicleattribute list and store into list
                VehicleAttributes = AllVehicleAttributes.Distinct().ToList();
                attributeSets = new SortedList();
                // Perform a loop on each attribute within the vehicles and initialise the empty sets
                foreach(string attribute in VehicleAttributes)
                {
                    attributeSets.Add(attribute, new HashSet<string>());
                }
            }
            // If fleet file doesn't exist, create two files
            else
            {
                throw new Exception($"Error: Specified file path {FleetFile} does not contain fleet.csv file");
            }
            if (File.Exists(RentalsFile))
            {
                this.RentalsFile = RentalsFile;
                LoadRentalsFromFile();
            }
            else
            {
                throw new Exception($"Error: Specified file path {RentalsFile} does not contain rentals.csv file");
            }
        }
        #endregion

        #region Standard Class Methods For fleet
        // Adds the provided vehicle to the fleet assuming the vehicle registration does not
        // already exist. It returns true if the add was successful (the registration did not
        // already exist in the fleet), and false otherwise.
        public bool AddVehicle(Vehicle vehicle)
        {
            // Use the Any method on Vehicle list to check if it already contains the registration.
            // If theres a match, false so that the vehicle cannot be added, since rego already exists.
            if (VehicleList.Any(archivedVehicle => archivedVehicle.registration == vehicle.registration))
            {
                return false;
            }
            else
            {   // if vehicle registration does not exist in the list, add the vehicle to the vehicle list
                VehicleList.Add(vehicle);
                return true;
            }
        }

        //This method removes the vehicle with the provided rego from the fleet if it is not
        //currently rented.it returns true if the removal was successful and false otherwise.
        public bool RemoveVehicle(string registration)
        {
            // if registration of the vehicle is not rented, delete the vehicle record relative to the fleet file
            if (!IsRented(registration))
            {
                CsvOperations.DeleteRecord("fleet.csv", registration, FleetFile, 0);
                VehicleList.Remove(GetVehicle(registration));
                return true;
            }
            return false;
        }

        //This method returns the fleet(as a list of Vehicles).
        public List<Vehicle> GetFleet()
        {
            //return the vehicle list
            return VehicleList;
        }

        // This method returns a subset of vehicles in the fleet depending on whether they are
        // currently rented. If rented is true, this method returns all rented vehicles. If it
        // false, this method returns all not yet rented vehicles.
        public List<Vehicle> GetFleet(bool rented)
        {   // Create two lists with rented and unrented vehicles
            List<Vehicle> RentedVehicles = new List<Vehicle>();
            List<Vehicle> UnrentedVehicles = new List<Vehicle>();
            // Iterate through the vehicle list and check for vehicles which are rented
            foreach (Vehicle vehicle in VehicleList)
            {
                // If rented store in rented list
                if (IsRented(vehicle.registration))
                {
                    RentedVehicles.Add(vehicle);
                }
                // Else, store in unrented list
                else
                {
                    UnrentedVehicles.Add(vehicle);
                }
            }
            // If function bool parameter is true, return a list of the rented vehicles
            if (rented)
            {
                return RentedVehicles;
            }
            else // If function bool parameter is false, return a list of the unrented vehicles
            {
                return UnrentedVehicles;
            }
        }

        // This method returns the vehicle that matches the provided registration.
        public Vehicle GetVehicle(string registration)
        {
            // Using firstordefault method to return the vehicle that matches the provided rego
            // Returns null if there is no vehicle that matches the provided rego.
            return VehicleList.FirstOrDefault(vehicle => vehicle.registration == registration);
        }

        // This method returns whether the vehicle with the provided registration is currently
        // being rented.
        public bool IsRented(string registration)
        {
            // Use the Any method on RentalRego list to check if it already contains the registration.
            // If if theres a match, true is returned which means the provided registation is rented.
            if (RentalListRego.Any(archivedRegistration => archivedRegistration == registration))
            {
                return true;
            }
            return false;
        }

        // This method returns whether the customer with the provided customer ID is currently
        // renting a vehicle.
        public bool IsRenting(int customerID)
        {
            // Use the Any method on RentalID list to check if it already contains the CustomerID.
            // If if theres a match, true is returned which means customer is renting.
            if (RentalListID.Any(archivedCustomerID => archivedCustomerID == customerID))
            {
                return true;
            }
            return false;
        }

        // This method returns the customer ID of the current renter of the vehicle. If it is
        // rented by no one, it returns -1. Technically this method can replace IsRented.
        public int RentedBy(string registration)
        {
            // Use for loop to iterate through every line of the rentals csv file
            for (int i = 0; i < RentalsCsvLines.Length; i++)
            {
                // Split fields separated by a comma into indidual elements and store into array
                string[] fields = RentalsCsvLines[i].Split(',');
                // If the registration exists in the rentals, return customerID.
                // Note that, registration and customer ID is located in position 0 and 1 of the rentals file, respectively.
                int regoCsvPos = 0, customerIDPos = 1;
                if (CsvOperations.RecordExists(registration, fields, regoCsvPos))
                {
                    return int.Parse(CsvOperations.ReadRecordAttribute(registration, RentalsFile, 0, customerIDPos));
                }
            }
            return -1;
        }

        // This method rents the vehicle with the provided registration to the customer with
        // the provided ID, if the vehicle is not currently being rented. It returns true if
        // the rental was possible, and false otherwise.
        public bool RentVehicle(string registration, int customerID, double DailyCost)
        {
            // if the vehicle is not currently rented (no rego in rentals file), rent the vehicle by writing into csv
            if (!(IsRented(registration)))
            {
                RentalListID.Add(customerID);
                RentalListRego.Add(registration);
                this.DailyCost.Add(DailyCost);
                return true;
            }
            return false;

        }

        //// This method returns a vehicle. If the return is successful (it was currently being
        //// rented) it returns the customer ID of the customer who was renting it, otherwise it
        //// returns -1.
        public int ReturnVehicle(string registration)
        {
            if(IsRented(registration))
            {
                return RentedBy(registration);
            }
            return -1;
        }

        // This method saves the current list of vehicles to file.
        public void SaveVehiclesToFile()
        {
            //Create new file
            string newFile = "fleet.csv";
            //Write the header fields at the top of the new file
            string csvFieldHeader = "Registration," + "Grade," + "Make," + "Model," + "Year," + "NumSeats," + "Transmission,"
                                    + "Fuel," + "GPS," + "SunRoof," + "DailyRate," + "Colour\n";
            File.WriteAllText(newFile, csvFieldHeader);

            // For each of the vehicles added into the unsaved vehicles list, write to the fleet csv file.
            foreach (Vehicle vehicle in VehicleList)
            {
                CsvOperations.WriteToCsv(newFile, vehicle.ToCSVstring());
            }

            // delete old file
            File.Delete(FleetFile);
            // move new file to old file directory
            File.Move(newFile, FleetFile);
        }
        // This method saves the current list of rentals to file.
        public void SaveRentalsToFile()
        {
            // Creat new file
            string newFile = "rentals.csv";
            //Write the header fields at the top of the new file
            string csvFieldHeader = "Registration," + "CustomerID\n";
            File.WriteAllText(newFile, csvFieldHeader);

            // The rego and id lists are zipped into a parallel list, where it will be used in a foreach loop
            // to write into csv file
            var parallelList = RentalListRego.Zip(RentalListID, (rego, id) => new { Rego = rego, ID = id });
            foreach (var element in parallelList)
            {
                CsvOperations.WriteToCsv(newFile, element.Rego + "," + element.ID.ToString());
            }
            // delete old file
            File.Delete(RentalsFile);
            // move new file to old file directory
            File.Move(newFile, RentalsFile);
        }

        // this method loads the list of vehicles from the file.
        private void LoadVehiclesFromFile()
        {
            // Create datatable for fleet
            DataTable fleet = new DataTable();
            // load list of vehicles and store into datatable
            CsvOperations.CsvToTable(FleetFile, fleet);
            // Create string arrays for each attribute of a vehicle and store the data from the datable into the string arrays.
            // (Note that string array variables starting with lower case letters need to be converted to their appropriate
            //  Data type. When converted, they will encompass uppercase lettering.
            string[] Registration = fleet.AsEnumerable().Select(row => row.Field<string>("Registration")).ToArray();
            string[] grade = fleet.AsEnumerable().Select(row => row.Field<string>("Grade")).ToArray();
            string[] Make = fleet.AsEnumerable().Select(row => row.Field<string>("Make")).ToArray();
            string[] Model = fleet.AsEnumerable().Select(row => row.Field<string>("Model")).ToArray();
            string[] year = fleet.AsEnumerable().Select(row => row.Field<string>("Year")).ToArray();
            string[] numSeats = fleet.AsEnumerable().Select(row => row.Field<string>("NumSeats")).ToArray();
            string[] transmission = fleet.AsEnumerable().Select(row => row.Field<string>("Transmission")).ToArray();
            string[] fuel = fleet.AsEnumerable().Select(row => row.Field<string>("Fuel")).ToArray();
            string[] gps = fleet.AsEnumerable().Select(row => row.Field<string>("GPS")).ToArray();
            string[] sunRoof = fleet.AsEnumerable().Select(row => row.Field<string>("SunRoof")).ToArray();
            string[] dailyRate = fleet.AsEnumerable().Select(row => row.Field<string>("DailyRate")).ToArray();
            string[] Colour = fleet.AsEnumerable().Select(row => row.Field<string>("Colour")).ToArray();

            //Convert string arrays of grade, fuel and transmission into enum arrays
            Vehicle.VehicleGrade[] Grade = grade.Select(s => Enum.Parse(typeof(Vehicle.VehicleGrade), s)).Cast<Vehicle.VehicleGrade>().ToArray();
            Vehicle.TransmissionType[] Transmission = transmission.Select(s => Enum.Parse(typeof(Vehicle.TransmissionType), s)).Cast<Vehicle.TransmissionType>().ToArray();
            Vehicle.FuelType[] Fuel = fuel.Select(s => Enum.Parse(typeof(Vehicle.FuelType), s)).Cast<Vehicle.FuelType>().ToArray();
            // Convert string arrays of year and numSeats & dailyRate into type int and double, respectively.
            int[] Year = Array.ConvertAll(year, int.Parse);
            int[] NumSeats = Array.ConvertAll(numSeats, int.Parse);
            double[] DailyRate = Array.ConvertAll(dailyRate, double.Parse);
            // Convert string arrays of gps and sunRoof to boolean datatype 
            bool[] GPS = Array.ConvertAll(gps, bool.Parse);
            bool[] SunRoof = Array.ConvertAll(sunRoof, bool.Parse);

            // The number of vehicles in the fleet csv file is equal to the number of lines contained within the file.
            // Since registration is a mandatory field, it will not have missing entries, so we count the size of the registration array
            // to determine the number of vehicles in the csv file.
            int numVehicles = Registration.Length;
            // Using a loop, create a list of vehicles by instatiating the vehicle class. 
            // Import values from the arrays above (taken from the csv file) into the constructor of vehicle class.
            for (int i = 0; i < numVehicles; i++)
            {
                VehicleList.Add(new Vehicle(Registration[i], Grade[i], Make[i], Model[i], Year[i], NumSeats[i],
                                          Transmission[i], Fuel[i], GPS[i], SunRoof[i], DailyRate[i], Colour[i]));
            }

        }
        // this method loads the list of rentals from the file.
        private void LoadRentalsFromFile()
        {
            // Load all and store the rental csv lines into string array
            RentalsCsvLines = File.ReadAllLines(RentalsFile);
            // Create datatable for rentals
            DataTable rentals = new DataTable();
            // load list of rentals and store into datable
            CsvOperations.CsvToTable(RentalsFile, rentals);
            // Store datatable fields in string arrays: registration and CustomerID
            string[] Registration = rentals.AsEnumerable().Select(row => row.Field<string>("Registration")).ToArray();
            string[] customerID = rentals.AsEnumerable().Select(row => row.Field<string>("CustomerID")).ToArray();
            
            // Convert CustomerID to int array
            int[] CustomerID = Array.ConvertAll(customerID, int.Parse);

            for(int i = 0; i < customerID.Length; i++)
            {
                RentalListRego.Add(Registration[i]);
                RentalListID.Add(CustomerID[i]);
            }

            foreach(string rego in Registration)
            {
                DailyCost.Add(GetVehicle(rego).dailyRate);
            }
        }
        #endregion
    }
}
