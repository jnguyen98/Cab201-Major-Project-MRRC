using MRRCManagement;
using System;
using System.Data;
using System.Globalization;
using System.Threading;

namespace MRRC
{
    // This class is the Main Sub menu interface of MRRC which contains all child interfaces (i.e., border and table menu)
    public class SubInterface : MainInterface
    {
        #region Constructor
        public SubInterface(Fleet fleet, CRM crm) : base(fleet, crm) { }
        #endregion

        #region Main Submenu Interface
        // Control flow of submenu 
        public void SubMenu(string option, bool clear = true)
        {
            // Prompt submenu screen with options and clear the previous screen buffer
            SubMenuOptions(option, clear);
            // Create object instances of the inner submenus
            InnerSubInterfaceBorder border = new InnerSubInterfaceBorder(Fleet, Crm);
            InnerSubInterfaceCRM crm = new InnerSubInterfaceCRM(Fleet, Crm);
            InnerSubInterfaceFleet fleet = new InnerSubInterfaceFleet(Fleet, Crm);
            InnerSubInterfaceRentals rentals = new InnerSubInterfaceRentals(Fleet, Crm);
            switch (inputkey.Key)
            {

                case A_Pressed: // Display Table depending on option
                    border.BorderSubmenu(option);
                    break;
                case B_Pressed:// Add option
                    switch (option)
                    {
                        case "Customer": // New customer
                            crm.NewCustomerSubmenu();
                            break;
                        case "Fleet": // New vehicle
                            fleet.NewVehicleSubMenu();
                            break;
                        case "Rentals": // Search vehicle
                            rentals.SearchVehicleSubMenu();
                            MRRCInterface(false);
                            break;
                    }
                    break;
                case C_Pressed: // Modify option
                    switch (option)
                    {
                        case "Customer": // modify customer
                            crm.ModifyCustomerSubmenu();
                            break;
                        case "Fleet": // modify vehicle
                            fleet.ModifyVehicleSubmenu();
                            break;
                        case "Rentals":
                            rentals.AddNewRentalSubMenu();
                            break;
                    }
                    break;
                case D_Pressed: // Remove option
                    switch (option)
                    {
                        case "Customer": // Delete customer
                            crm.RemoveCustomerSubmenu();
                            break;
                        case "Fleet": // Delete vehicle
                            fleet.RemoveVehicleSubmenu();
                            break;
                        case "Rentals": // Just go back to home since this section is for part B
                            rentals.ReturnVehicleSubMenu();
                            break;
                    }
                    break;
                case Backspace_Pressed: // return to the main menu interface
                    MRRCInterface();
                    break;
                case Escape_Pressed: // Quit program
                    QuitMessage();
                    break;
                case H_Pressed: // Return to home
                    MRRCInterface();
                    break;
                case Q_Pressed: // go to parent menu
                    MRRCInterface();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("You Pressed '{0}', Please enter a valid option\n", inputkey.Key.ToString());
                    // lets not clear the above message and the submenu will re-prompt again.
                    SubMenu(option, false);
                    break;
            }
        }
        #endregion

        #region SubMenu Screen Display Options
        // Display Submenu options
        private void SubMenuOptions(string option, bool clear = true)
        {   // Clear the screen buffer if clear is true, then prompt submenu options
            if (clear)
                Console.Clear();

            // String to store submenu options
            string SubMenu;
            switch (option)
            {
                //Submenu for customer
                case "Customer":
                    SubMenu = "a) Display Customers" + "\nb) New Customer"
                       + "\nc) Modify Customer" + "\nd) Delete Customer";
                    Console.WriteLine("### Customer Management Submenu ###\n");
                    Console.WriteLine("Please enter a character from the options below:\n");
                    Console.Write(SubMenu);
                    // Read the next keyboard char input from the user
                    inputkey = Console.ReadKey();
                    break;
                //Submenu for Fleet
                case "Fleet":
                    SubMenu = "a) Display Fleet" + "\nb) New Vehicle"
                        + "\nc) Modify Vehicle" + "\nd) Delete Vehicle";
                    Console.WriteLine("### Fleet Management Submenu ###\n");
                    Console.WriteLine("Please enter a character from the options below:\n");
                    Console.Write(SubMenu);
                    inputkey = Console.ReadKey();
                    break;
                //Submenu for Rental
                case "Rentals":
                    SubMenu = "a) Display Rentals" + "\nb) Search Vehicles"
                       + "\nc) Rent Vehicle" + "\nd) Return Vehicle";

                    Console.WriteLine("### Rental Management Submenu ###\n");
                    Console.WriteLine("Please enter a character from the options below:\n");
                    Console.Write(SubMenu);
                    inputkey = Console.ReadKey();
                    break;
            }//end switch for submenu
        }
        #endregion

        #region Extra Helper Methods to be inherited in derived classes
        // Apply the last screen operations when the user reaches the end of the MRRC interface
        protected void LastMRRCscreen(Action parentMenu, Action subMenu, bool applyinputkey = true)
        {
            // Wait for user to press a key at the last screen and clear buffer
            if (applyinputkey)
                inputkey = Console.ReadKey();

            switch (inputkey.Key)
            {
                case Backspace_Pressed:
                    // go back to previous menu
                    subMenu();
                    break;
                case Escape_Pressed: // Prompt quit message if esc
                    QuitMessage();
                    break;
                case H_Pressed: // go to MRRC home
                    MRRCInterface();
                    break;
                case Q_Pressed: // Go to parent menu
                    parentMenu();
                    break;
                // If any other key other than backspace or escape is pressed, prompt end of menu screen
                default:
                    EndOfMenuScreen(parentMenu, subMenu);
                    break;
            }
        }

        // Function when user has any pressed key at the last screen on the MRRC interface
        protected void EndOfMenuScreen(Action parentMenu, Action subMenu, bool clear = true, bool showHeader = true)
        {// If clear is true clear the screen buffer and prompt new screen
            if (clear)
                Console.Clear();
            if (showHeader)
                Console.WriteLine("### End of Mates-Rates Rent-a-Car Operation Menu ###");

            Console.WriteLine("\nPress H go to the main menu home, Q to parent menu, Esc to escape, Backspace to go back");
            inputkey = Console.ReadKey();
            switch (inputkey.Key)
            {
                case H_Pressed:
                    MRRCInterface();
                    break;
                case Backspace_Pressed:
                    // Go back to the last screen and set false to not apply read input key 
                    LastMRRCscreen(parentMenu, subMenu, false);
                    break;
                case Escape_Pressed:
                    QuitMessage();
                    break;
                case Q_Pressed:
                    // Go back to the parent screen and set false to not apply read input key 
                    LastMRRCscreen(parentMenu, subMenu, false);
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("You pressed '{0}', please enter a valid key\n", inputkey.Key.ToString());
                    EndOfMenuScreen(parentMenu, subMenu, false);
                    break;
            }
        }

        // This method converts validation input in the format of title case where the first letter 
        // of the string input is uppercase and the latter being lowercase.
        public string StringToTitleCase(string inputString)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            string TitleCase = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(inputString.ToLower());

            return TitleCase;
        }
        #endregion
    }

}
