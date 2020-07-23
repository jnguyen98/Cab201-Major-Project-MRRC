using MRRCManagement;
using System;
using System.Data;

namespace MRRC
{
    // This derived submenu interface class contains border options for displaying customers, vehicles and rentals
    public class InnerSubInterfaceBorder : SubInterface
    {
        #region Constructor
        public InnerSubInterfaceBorder(Fleet fleet, CRM crm) : base(fleet, crm) { }
        #endregion

        #region Main border and Table submenu interface
        // This method handles the displaying fleet, customer and rental files to the console
        public void BorderSubmenu(string option, bool clear = true)
        {
            // If clear is true, clear the console screen buffer
            if (clear)
                Console.Clear();

            BorderSubmenuDisplay(option);
            switch (inputkey.Key)
            {
                case A_Pressed:
                    // Set to border to extended Ascii char
                    CsvOperations.borderStyle = "Extended";
                    // Display table
                    DisplayTable(option);
                    LastMRRCscreen(() => SubMenu(option), () => BorderSubmenu(option));
                    break;
                case B_Pressed:
                    // Set to border to Ascii char
                    CsvOperations.borderStyle = "ASCII";
                    // Display table
                    DisplayTable(option);
                    LastMRRCscreen(() => SubMenu(option), () => BorderSubmenu(option));
                    break;
                case C_Pressed:
                    // Set to border to clear
                    CsvOperations.borderStyle = "Clear";
                    // Display table
                    DisplayTable(option);
                    LastMRRCscreen(() => SubMenu(option), () => BorderSubmenu(option));
                    break;
                case Backspace_Pressed: // return to the submenu interface
                    SubMenu(option);
                    break;
                case Escape_Pressed: // prompt quit message
                    QuitMessage();
                    break;
                case H_Pressed: // go MRRC home
                    MRRCInterface();
                    break;
                case Q_Pressed: // go to parent menu
                    SubMenu(option);
                    break;
                default: // if any key other than above listed is pressed, do:
                    Console.Clear();
                    Console.WriteLine("You Pressed '{0}', Please enter a valid option\n", inputkey.Key.ToString());
                    BorderSubmenu(option, false);
                    break;
            }
        }
        #endregion

        #region Helper Methods for displaying Submenu and Table to the console
        // Display of border submenu screen
        private void BorderSubmenuDisplay(string optionChoice)
        {
            string innerSubMenu = "a) Display " + optionChoice + " Table with ExtendedASCII border" + "\nb) Display " + optionChoice + " Table with ASCII border"
                       + "\nc) Display " + optionChoice + " Table with Clear border";
            Console.WriteLine("### Table and Border Submenu ###\n");
            Console.WriteLine("Please enter a character from the options below:\n");
            Console.Write(innerSubMenu + "\n\n");
            // Read the next keyboard char input from the user
            inputkey = Console.ReadKey();
        }

        // This methods displays the data table to the console, where it sources the csv files from the specified locations
        private void DisplayTable(string option)
        {
            DataTable rentals = new DataTable();
            DataTable fleet = new DataTable();
            DataTable customers = new DataTable();
            switch (option)
            {
                case "Customer":
                    CsvOperations.ShowDataTable(customers, Crm.CrmFile, CsvOperations.borderStyle);
                    break;
                case "Fleet":
                    CsvOperations.ShowDataTable(fleet, Fleet.FleetFile, CsvOperations.borderStyle);
                    break;
                case "Rentals":
                    CsvOperations.RentalsToTable(Fleet, rentals);
                    CsvOperations.ShowDataTable(rentals, Fleet.RentalsFile, CsvOperations.borderStyle, true);
                    break;
            }
        }
        #endregion
    }

}
