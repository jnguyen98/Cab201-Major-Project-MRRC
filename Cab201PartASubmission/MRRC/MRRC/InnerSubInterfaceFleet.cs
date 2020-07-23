using MRRCManagement;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace MRRC
{

    // This derived submenu interface class contains Fleet options for addition, modification and deletion of Vehicles
    public class InnerSubInterfaceFleet : SubInterface
    {
        #region Fields
        // String variables to read inputs from user in the vehicle menu
        private string registration;
        private string grade;
        private string make;
        private string model;
        private string year;
        private string numSeats;
        private string transmission;
        private string fuel;
        private string gps;
        private string sunRoof;
        private string dailyRate;
        private string colour;
        private string regoToModify;
        private string regoToDelete;
        // The 6 variables below will undergo appropriate validation testing. They will be parsed into
        // the out parameters of the tryparse methods
        private Vehicle.VehicleGrade Grade;
        private Vehicle.TransmissionType Transmission;
        private Vehicle.FuelType Fuel;
        private int NumSeats;
        private bool GPS;
        private bool SunRoof;
        private double DailyRate;
        #endregion

        #region Constructor
        public InnerSubInterfaceFleet(Fleet fleet, CRM crm) : base(fleet, crm) { }
        #endregion

        #region Validation user input for fleet
        // Validation check when user enters the registration of a vehicle
        private void RegistrationValidInput()
        {
            bool RegoIsValid;
            do
            {
                Console.Write("Registration*: ");
                registration = Console.ReadLine();
                // Perform complex regex match for user input validation
                RegoIsValid = Regex.IsMatch(registration, @"^\d{3}[A-Z]{3}$");
                if (!RegoIsValid)
                {
                    Console.WriteLine("\nYou entered {0}, which is not in the correct format for the vehicle registration", registration);
                    Console.WriteLine("Please enter the registration in the following regex pattern format: ^\\d{3}[A-Z]{3}$\n");
                }
                // If theres a sequence match of the registration within the fleet vehicle list, then rego is not valid as it already exists.
                else if (Fleet.VehicleList.Any(archivedVehicle => archivedVehicle.registration == registration))
                {
                    Console.WriteLine("\nCould not accept vehicle registration '{0}' as it already exists in the fleet list.\n",
                                registration);
                    RegoIsValid = false;
                }
            } while (!RegoIsValid);
        }

        // Validation check when user enters the grade of a vehicle
        private void GradeValidInput()
        {
            bool GradeIsValid;
            do
            {
                Console.Write("Grade*: ");
                grade = Console.ReadLine();
                GradeIsValid = Enum.TryParse(grade, true, out Grade);

                if (!GradeIsValid)
                {
                    Console.WriteLine("\nYou entered {0}, which is not a member of the Vehicle Grade Enum", grade);
                    Console.WriteLine("Please enter a defined grade in the following format: Economy, Family, Luxury, Commercial\n");
                }
            } while (!GradeIsValid);
        }

        // Validation check when user enters the year of a vehicle
        private void YearValidInput()
        {
            bool YearIsValid;
            do
            {
                Console.Write("Year*: ");
                year = Console.ReadLine();
                YearIsValid = Regex.IsMatch(year, @"^\d{4}");
                if (!YearIsValid)
                {
                    Console.WriteLine("\nYou entered {0}, which is not in the correct format for the vehicle year", year);
                    Console.WriteLine("Please enter the year in the following regex pattern format: ^\\d{4}\n");
                }
                else
                {
                    int dateA = int.Parse(year);
                    int dateB = int.Parse(DateTime.Now.Year.ToString());
                    if (!(dateA <= dateB))
                    {
                        Console.WriteLine("\nYou entered {0}, which not a valid year.", dateA);
                        Console.WriteLine("Please enter a year which is less than or equal to {0}\n", dateB);
                        YearIsValid = false;
                    }
                }
            } while (!YearIsValid);
        }

        // Validation check when user enters the number of seat of a vehicle
        private void NumSeatsValidInput()
        {
            bool NumSeatsIsValid;
            do
            {
                Console.Write("NumSeats*: ");
                numSeats = Console.ReadLine();
                NumSeatsIsValid = int.TryParse(numSeats, out NumSeats);

                if (NumSeats < 0)
                {
                    Console.WriteLine("\nYou entered '{0}', which is not a positive integer", NumSeats);
                    Console.WriteLine("Enter a number of seats which is a positive integer\n");
                    NumSeatsIsValid = false;
                }
                else if (NumSeats > 8)
                {
                    Console.WriteLine("\nYou entered '{0}', which is greater than the maximum number of seats allowed", NumSeats);
                    Console.WriteLine("Enter a valid number of seats which is between 0 and 8\n");
                    NumSeatsIsValid = false;
                }
                else if (!int.TryParse(numSeats, out NumSeats))
                {
                    Console.WriteLine("\nYou entered '{0}', which is not an integer", NumSeats);
                    Console.WriteLine("Enter a number of seats which is an integer\n");
                    NumSeatsIsValid = false;
                }
            } while (!NumSeatsIsValid);
        }

        // Validation check when user enters the transmissiontype of a vehicle
        private void TransmissionValidInput()
        {
            bool TransmissionIsValid;
            do
            {
                Console.Write("Transmission*: ");
                transmission = Console.ReadLine();
                TransmissionIsValid = Enum.TryParse(transmission, true, out Transmission);

                if (!TransmissionIsValid)
                {
                    Console.WriteLine("\nYou entered {0}, which is not a member of the Vehicle TransmissionType Enum", transmission);
                    Console.WriteLine("Please enter a defined TransmissionType in the following format: Automatic, Manual\n");
                }
            } while (!TransmissionIsValid);
        }

        // Validation check when user enters the fueltype of a vehicle
        private void FuelValidInput()
        {
            bool FuelIsValid;
            do
            {
                Console.Write("Fuel*: ");
                fuel = Console.ReadLine();
                FuelIsValid = Enum.TryParse(fuel, true, out Fuel);

                if (!FuelIsValid)
                {
                    Console.WriteLine("\nYou entered {0}, which is not a member of the Vehicle FuelType Enum", fuel);
                    Console.WriteLine("Please enter a defined FuelType in the following format: Petrol, Diesel\n");
                }
            } while (!FuelIsValid);

        }

        // This method tests whether the user inputs a value that can be parsed as bool type (for GPS and SunRoof)
        private void BoolValidInput(string option)
        {
            if (option == "GPS")
            {
                bool GpsIsValid;
                do
                {
                    Console.Write("GPS*: ");
                    gps = Console.ReadLine();
                    GpsIsValid = bool.TryParse(gps, out GPS);

                    if (!GpsIsValid)
                    {
                        Console.WriteLine("\nYou entered {0}, which is not a valid value to be converted to a bool", gps);
                        Console.WriteLine("Please enter a defined boolean type in the following format: True, False\n");
                    }
                } while (!GpsIsValid);
            }
            if (option == "SunRoof")
            {
                bool SunRoofIsValid;
                do
                {
                    Console.Write("SunRoof*: ");
                    sunRoof = Console.ReadLine();
                    SunRoofIsValid = bool.TryParse(sunRoof, out SunRoof);

                    if (!SunRoofIsValid)
                    {
                        Console.WriteLine("\nYou entered {0}, which is not a valid value to be converted to a bool", sunRoof);
                        Console.WriteLine("Please enter a defined boolean type in the following format: True, False\n");
                    }
                } while (!SunRoofIsValid);
            }
        }

        // Validation check when user enters the daily rate of a vehicle
        private void DailyRateValidInput()
        {
            bool DailyRateIsValid;
            do
            {
                Console.Write("DailyRate*: ");
                dailyRate = Console.ReadLine();
                DailyRateIsValid = double.TryParse(dailyRate, out DailyRate);

                if (DailyRate < 0)
                {
                    Console.WriteLine("\nYou entered '{0}', which is not a positive double numerical type", dailyRate);
                    Console.WriteLine("Enter a daily rate which is a positive double numerical type\n");
                    DailyRateIsValid = false;
                }
                else if (!double.TryParse(dailyRate, out DailyRate))
                {
                    Console.WriteLine("\nYou entered '{0}', which is not an double numerical type", dailyRate);
                    Console.WriteLine("Enter a daily rate which is a double numerical type\n");
                    DailyRateIsValid = false;
                }
            } while (!DailyRateIsValid);

        }
        // Validation check on the registration when user wants to modify a vehicle
        private void ModifyRegoValidInput()
        {
            bool RegoIsValid;
            do
            {
                Console.Write("Please enter the registration of the vehicle to be modified: ");
                regoToModify = Console.ReadLine();
                // Perform complex regex match for user input validation
                RegoIsValid = Regex.IsMatch(regoToModify, @"^\d{3}[A-Z]{3}$");
                if (!RegoIsValid)
                {
                    Console.WriteLine("\nYou entered {0}, which is not in the correct format for the vehicle registration", regoToModify);
                    Console.WriteLine("Please enter the registration in the following regex pattern format: ^\\d{3}[A-Z]{3}$\n");
                }
                // Ensure that the registration of the vehicle exist in the vehicle list to modify the vehicle
                else if (!Fleet.VehicleList.Any(archivedVehicle => archivedVehicle.registration == regoToModify))
                {
                    Console.WriteLine("\nCould not modify vehicle registration '{0}' as it does not exist in the fleet list.\n",
                                regoToModify);
                    RegoIsValid = false;
                }
                else if (Fleet.IsRented(regoToModify))
                {
                    Console.WriteLine("\nCould not modify vehicle registration '{0}' as it is currently being rented. Try another vehicle.\n",
                                regoToModify);
                    RegoIsValid = false;
                }
            } while (!RegoIsValid);
        }
        // Validation check on the registration when user wants to delete a vehicle
        private void DeleteRegoValidInput()
        {
            bool RegoIsValid;
            do
            {
                Console.Write("Please enter the registration of the vehicle to be deleted: ");
                regoToDelete = Console.ReadLine();
                // Perform complex regex match for user input validation
                RegoIsValid = Regex.IsMatch(regoToDelete, @"^\d{3}[A-Z]{3}$");
                if (!RegoIsValid)
                {
                    Console.WriteLine("\nYou entered {0}, which is not in the correct format for the vehicle registration", regoToDelete);
                    Console.WriteLine("Please enter the registration in the following regex pattern format: ^\\d{3}[A-Z]{3}$\n");
                }
                // Ensure that the registration of the vehicle exist in the vehicle list to delete the vehicle 
                else if (!Fleet.VehicleList.Any(archivedVehicle => archivedVehicle.registration == regoToDelete))
                {
                    Console.WriteLine("\nCould not delete vehicle registration '{0}' as it does not exist in the fleet list.\n",
                                regoToDelete);
                    RegoIsValid = false;
                }
            } while (!RegoIsValid);
        }
        #endregion

        #region Add new vehicle Main Interface
        // Method that handles the interface of new vehicle, which includes the operation of adding vehicle to fleet.csv.
        public void NewVehicleSubMenu(bool clear = true)
        {
            if (clear)
                Console.Clear();

            Console.WriteLine("### New Vehicle Submenu ###\n");
            string subMenu = "a) Standard" + "\nb) Economy "
                       + "\nc) Family" + "\nd) Luxury"
                       + "\ne) Commercial";
            Console.WriteLine("Please enter a character from the options below:\n");
            Console.WriteLine(subMenu);
            inputkey = Console.ReadKey();
            switch (inputkey.Key)
            {
                case A_Pressed:
                    NewVehicleInnerSubMenu("Standard", () => AddDefaultVehicle(), () => AddFullVehicle());
                    break;
                case B_Pressed:
                    NewVehicleInnerSubMenu("Economy", () => AddDefaultVehicle("Economy", true, true), () => AddFullVehicle("Economy", true, true));
                    break;
                case C_Pressed:
                    NewVehicleInnerSubMenu("Family", () => AddDefaultVehicle("Family", true, true), () => AddFullVehicle("Family", true, true));
                    break;
                case D_Pressed:
                    NewVehicleInnerSubMenu("Luxury", () => AddDefaultVehicle("Luxury", true, true), () => AddFullVehicle("Luxury", true, true));
                    break;
                case E_Pressed:
                    NewVehicleInnerSubMenu("Commercial", () => AddDefaultVehicle("Commercial", true, true), () => AddFullVehicle("Commercial", true, true));
                    break;
                case H_Pressed:
                    MRRCInterface();
                    break;
                case Backspace_Pressed:
                    SubMenu("Fleet");
                    break;
                case Escape_Pressed:
                    QuitMessage();
                    break;
                default: // if any key other than above listed is pressed, do:
                    Console.Clear();
                    Console.WriteLine("You Pressed '{0}', Please enter a valid option\n", inputkey.Key.ToString());
                    NewVehicleSubMenu(false);
                    break;
            }
        }
        #endregion

        #region Add new vehicle Sub-interface
        // Method thats Handles the next interface and operations after the New vehicle sub menu
        private void NewVehicleInnerSubMenu(string Grade, Action DefaultVehicle, Action FullVehicle, bool clear = true)
        {
            if (clear)
                Console.Clear();

            Console.WriteLine("### New " + Grade + " Vehicle Submenu ###\n");
            string subMenu = "a) Default " + Grade + " Vehicle" + "\nb) Full " + Grade + " Vehicle";
            Console.WriteLine("Please enter a character from the options below:\n");
            Console.WriteLine(subMenu);
            // Read next user input
            inputkey = Console.ReadKey();
            switch (inputkey.Key)
            {
                case A_Pressed:
                    DefaultVehicle();
                    break;
                case B_Pressed:
                    FullVehicle();
                    break;
                case Backspace_Pressed:
                    NewVehicleSubMenu();
                    break;
                case Q_Pressed:
                    SubMenu("Fleet");
                    break;
                case H_Pressed:
                    MRRCInterface();
                    break;
                case Escape_Pressed:
                    QuitMessage();
                    break;
                default: // if any key other than above listed is pressed, do:
                    Console.Clear();
                    Console.WriteLine("You Pressed '{0}', Please enter a valid option\n", inputkey.Key.ToString());
                    NewVehicleInnerSubMenu(Grade, DefaultVehicle, FullVehicle, false);
                    break;
            }
        }

        // This Method will determine which header and vehicle grade will be chosen in the 
        // NewVehicle methods below.
        private void VehicleGradeOption(string FullOrDefault, bool GradeProvided = false, string GradeOption = "None")
        {
            if (GradeProvided == false && GradeOption == "None")
            {
                Console.WriteLine("### New " + FullOrDefault + " Vehicle Submenu ###\n");
            }
            else if (GradeProvided == true && GradeOption == "Commercial")
            {
                Console.WriteLine("### New " + FullOrDefault + " Commercial Vehicle Submenu ###\n");
                Grade = Vehicle.VehicleGrade.Commercial;
            }
            else if (GradeProvided == true && GradeOption == "Family")
            {
                Console.WriteLine("### New " + FullOrDefault + " Family Vehicle Submenu ###\n");
                Grade = Vehicle.VehicleGrade.Family;
            }
            else if (GradeProvided == true && GradeOption == "Economy")
            {
                Console.WriteLine("### New " + FullOrDefault + " Economy Vehicle Submenu ###\n");
                Grade = Vehicle.VehicleGrade.Economy;
            }
            else if (GradeProvided == true && GradeOption == "Luxury")
            {
                Console.WriteLine("### New " + FullOrDefault + " Economy Vehicle Submenu ###\n");
                Grade = Vehicle.VehicleGrade.Luxury;
            }
        }
        // Method which reads and validates user input and adds the default vehicle to the fleet csv file
        private void AddDefaultVehicle(string GradeOption = "None", bool clear = true, bool GradeProvided = false)
        {
            if (clear)
                Console.Clear();
            VehicleGradeOption("Default", GradeProvided, GradeOption);
            Console.WriteLine("Please fill the following fields (fields with * are required)\n");
            RegistrationValidInput();
            // If the grade of the vehicle has not been pre-selected (i.e. commercial), then ask for the grade
            if (!GradeProvided)
            {
                GradeValidInput();
            }
            Console.Write("Make*: ");
            make = StringToTitleCase(Console.ReadLine());
            Console.Write("Model*: ");
            model = StringToTitleCase(Console.ReadLine());
            YearValidInput();
            // If the grade is provided, create and instantiate vehicle object from the othervehicles class.
            if (GradeProvided)
            {
                // Assign initial value to new vehicle as null
                Vehicle newVehicle = null;

                // Depending on the grade option, re-assign newvhicle instance by instatiating the grade class
                if (GradeOption == "Economy")
                {
                    newVehicle = new EconomyVehicle(registration, Grade, make, model, int.Parse(year));
                }
                else if (GradeOption == "Family")
                {
                    newVehicle = new FamilyVehicle(registration, Grade, make, model, int.Parse(year));
                }
                else if (GradeOption == "Luxury")
                {
                    newVehicle = new LuxuryVehicle(registration, Grade, make, model, int.Parse(year));
                }
                else if (GradeOption == "Commercial")
                {
                    newVehicle = new CommercialVehicle(registration, Grade, make, model, int.Parse(year));
                }
                // Add the new vehicle providing that registration doesnt already exist
                if (Fleet.AddVehicle(newVehicle))
                {
                    Console.Write("\nSuccessfully added new vehicle '{0} - {1} {2} {3}' and added to vehicle list", registration, grade, model, make);
                    Fleet.SaveVehiclesToFile();
                }
                // If user presses any key, prompt the end of the menu
                LastMRRCscreen(() => SubMenu("Fleet"), () => NewVehicleSubMenu());
            }
            // Otherwise, create and instatiate standard vehicle object from the vehicles class 
            else
            {
                Vehicle newVehicle = new Vehicle(registration, Grade, make, model, int.Parse(year));
                // Add the new vehicle providing that registration doesnt already exist
                if (Fleet.AddVehicle(newVehicle))
                {
                    Console.Write("\nSuccessfully added new vehicle '{0} - {1} {2} {3}' and added to vehicle list", registration, grade, model, make);
                    Fleet.SaveVehiclesToFile();
                }
                // If user presses any key, prompt the end of the menu
                LastMRRCscreen(() => SubMenu("Fleet"), () => NewVehicleInnerSubMenu(GradeOption, () => AddDefaultVehicle(GradeOption), () => AddDefaultVehicle(GradeOption)));

            }
        }
        // Method which reads and validates user input and adds the full vehicle to the fleet csv file
        private void AddFullVehicle(string GradeOption = "None", bool clear = true, bool GradeProvided = false)
        {
            if (clear)
                Console.Clear();

            VehicleGradeOption("Full", GradeProvided, GradeOption);
            Console.WriteLine("Please fill the following fields (fields with * are required)\n");
            RegistrationValidInput();
            // If the grade of the vehicle has not been pre-selected (i.e. commercial), then ask for the grade
            if (!GradeProvided)
            {
                GradeValidInput();
            }
            Console.Write("Make*: ");
            make = StringToTitleCase(Console.ReadLine());
            Console.Write("Model*: ");
            model = StringToTitleCase(Console.ReadLine());
            YearValidInput();
            NumSeatsValidInput();
            TransmissionValidInput();
            FuelValidInput();
            BoolValidInput("GPS");
            BoolValidInput("SunRoof");
            DailyRateValidInput();
            Console.Write("Colour*: ");
            colour = StringToTitleCase(Console.ReadLine());

            // Create new vehicle object and instatitate it with required fields.
            Vehicle newVehicle = new Vehicle(registration, Grade, make, model, int.Parse(year), NumSeats, Transmission, Fuel, GPS, SunRoof, DailyRate, colour);
            // Add the new vehicle providing that registration doesnt already exist
            if (Fleet.AddVehicle(newVehicle))
            {
                Console.Write("\nSuccessfully added new vehicle '{0} - {1} {2} {3}' and added to vehicle list", registration, grade, model, make);
                Fleet.SaveVehiclesToFile();
            }
            // If user presses any key, prompt the end of the menu
            LastMRRCscreen(() => SubMenu("Fleet"), () => NewVehicleInnerSubMenu(GradeOption, () => AddDefaultVehicle(GradeOption), () => AddDefaultVehicle(GradeOption)));
        }
        #endregion

        #region Modify vehicle submenu interface
        // Main method interface to do modify operations on vehicle
        public void ModifyVehicleSubmenu(bool clear = true)
        {
            if (clear)
                Console.Clear();

            Console.WriteLine("### Modify Vehicle Submenu ###\n");
            // Require the operator to enter the vehicle registration to be modified
            ModifyRegoValidInput();
            // Save the changed ID to -1 in case customer decideds to retain the same ID in the modification menu.
            Crm.SaveToFile();
            string subMenu = "a) Modify Default Vehicle Fields" + "\nb) Modify All Vehicle Fields";
            Console.WriteLine("\nPlease enter a character from the options below:\n");
            Console.WriteLine(subMenu);
            // Return the vehicle which matches the provided registration to modify and assign to vehicle object
            Vehicle vehicle = Fleet.GetVehicle(regoToModify);
            // Wait for user to input the next key
            inputkey = Console.ReadKey();
            switch (inputkey.Key)
            {
                case A_Pressed:
                    ModifyVehicle(vehicle);
                    break;
                case B_Pressed:
                    ModifyVehicle(vehicle, true);
                    break;
                case H_Pressed:
                    MRRCInterface();
                    break;
                case Backspace_Pressed:
                    ModifyVehicleSubmenu();
                    break;
                case Escape_Pressed:
                    QuitMessage();
                    break;
                case Q_Pressed:
                    SubMenu("Fleet");
                    break;
                default: // if any key other than above listed is pressed, do:
                    Console.Clear();
                    Console.WriteLine("You Pressed '{0}', Please enter a valid option\n", inputkey.Key.ToString());
                    ModifyVehicleSubmenu(false);
                    break;
            }

        }
        // Method to modify the vehicle fields, either with default or full fields
        private void ModifyVehicle(Vehicle vehicle, bool fullfields = false)
        {
            Console.Clear();

            string subMenu;
            if (fullfields)
                subMenu = "### Modify Full Vehicle Submenu ###\n";
            else
                subMenu = "### Modify Default Vehicle Submenu ###\n";

            Console.WriteLine(subMenu);
            Console.WriteLine("Please fill the following fields to modify (fields with * are required)\n");
            // Set the Registration to 000NUL (hinting null), in case the registration number wants to be retained and only the other fields
            // are to be modified. We set the Rego to null so that the validation doesnt prompt an error that registration wanting 
            // to be retained already exists. If the Registration is retained, there is still unique registrations in the list
            vehicle.registration = "000NUL";
            // Save the changed registration value in case operator decideds to retain the same Registration in the modification menu.
            Fleet.SaveVehiclesToFile();
            // Mandatory attributes for default
            RegistrationValidInput();
            vehicle.registration = registration;
            GradeValidInput();
            vehicle.grade = Grade;
            Console.Write("Make*: ");
            make = StringToTitleCase(Console.ReadLine());
            vehicle.make = make;
            Console.Write("Model*: ");
            model = StringToTitleCase(Console.ReadLine());
            vehicle.model = model;
            YearValidInput();
            vehicle.year = int.Parse(year);
            // If full fields is true, modify the full vehicle including non-mandatory attributes
            if (fullfields)
            {
                NumSeatsValidInput();
                vehicle.numSeats = NumSeats;
                TransmissionValidInput();
                vehicle.transmission = Transmission;
                FuelValidInput();
                vehicle.fuel = Fuel;
                BoolValidInput("GPS");
                vehicle.GPS = GPS;
                BoolValidInput("SunRoof");
                vehicle.sunRoof = SunRoof;
                DailyRateValidInput();
                vehicle.dailyRate = DailyRate;
                Console.Write("Colour*: ");
                colour = StringToTitleCase(Console.ReadLine());
                vehicle.colour = colour;
            }
            Console.Write("\nSuccessfully modified vehicle '{0}' to vehicle '{1} {2} {3} {4}'",
                           regoToModify, registration, make, model, year);
            // Save the new modified vehicle to the file
            Fleet.SaveVehiclesToFile();
            LastMRRCscreen(() => SubMenu("Fleet"), () => ModifyVehicleSubmenu());
        }
        #endregion

        #region Remove vehicle submenu interface
        // Method to remove a vehicle from the fleet list
        public void RemoveVehicleSubmenu(bool clear = true)
        {
            if (clear)
                Console.Clear();

            Console.WriteLine("### Remove Vehicle Submenu ###\n");
            DeleteRegoValidInput();

            // deletion is successful if vehicle registration is not renting a vehicle
            if (Fleet.RemoveVehicle(regoToDelete))
            {
                Console.WriteLine("\nSuccessfully deleted the Vehicle with the registration of '{0}'", regoToDelete);
            }
            else
            {
                Console.WriteLine("\nDeletion Unsuccessful. Vehicle '{0}' is still renting a vehicle", regoToDelete);
            }
            LastMRRCscreen(() => SubMenu("Fleet"), () => RemoveVehicleSubmenu());
        }
        #endregion
    }

}
