using System;
using System.Collections;
using System.Collections.Generic;

namespace MRRCManagement
{
    // This Vehicle class contains enumeration type constants, Constructors + overloading, properties and methods  
    // that will be useful as a template for performing operations on fleet csv file.
    public class Vehicle
    {
        #region Enums
        // Defined public Vehicle Grade Enum
        public enum VehicleGrade
        {
            Economy,
            Family,
            Luxury,
            Commercial,
        }
        // Defined public TransmissionType Enum
        public enum TransmissionType
        {
            Manual,
            Automatic,
        }
        // Defined public FuelType Enum
        public enum FuelType
        {
            Petrol,
            Diesel,
        }

        #endregion

        #region Properties
        // Get; Set; Propeties for vehicle modification
        public string registration { get; set; }
        public VehicleGrade grade { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public int year { get; set; }
        public int numSeats { get; set; }
        public TransmissionType transmission { get; set; }
        public FuelType fuel { get; set; }
        public bool GPS { get; set; }
        public bool sunRoof { get; set; }
        public double dailyRate { get; set; }
        public string colour { get; set; }
        #endregion

        #region SearchField bool
        private bool SearchUsed = false;
        #endregion

        #region Constructors
        // This constructor provides values for all parameters of the vehicle.
        public Vehicle(string registration, VehicleGrade grade, string make, string model,
                        int year, int numSeats, TransmissionType transmission, FuelType fuel,
                        bool GPS, bool sunRoof, double dailyRate, string colour)
        {
            SearchUsed = false;
            this.registration = registration;
            this.grade = grade;
            this.make = make;
            this.model = model;
            this.year = year;
            this.numSeats = numSeats;
            this.transmission = transmission;
            this.fuel = fuel;
            this.GPS = GPS;
            this.sunRoof = sunRoof;
            this.dailyRate = dailyRate;
            this.colour = colour;
        }

        // This constructor provides only the mandatory parameters of the vehicle. Others are
        // set based on the defaults of each class.
        public Vehicle(string registration, VehicleGrade grade, string make, string model, int year)
        {
            SearchUsed = false;
            this.registration = registration;
            this.grade = grade;
            this.make = make;
            this.model = model;
            this.year = year;
            numSeats = 4;
            transmission = TransmissionType.Manual;
            fuel = FuelType.Petrol;
            GPS = false;
            sunRoof = false;
            dailyRate = 50;
            colour = "Black";
        }

        public Vehicle(string vehicleCSV)
        {
            SearchUsed = true;
            // vehicle constructor from CSV string
            ArrayList values = new ArrayList();
            values.AddRange(vehicleCSV.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            registration = (string)values[0];
            grade = (VehicleGrade)Enum.Parse(typeof(VehicleGrade), (string)values[1]);
            make = (string)values[2];
            model = (string)values[3];
            year = int.Parse((string)values[4]);
            numSeats = int.Parse((string)values[5]);
            transmission = (TransmissionType)Enum.Parse(typeof(TransmissionType), (string)values[6]);
            fuel = (FuelType)Enum.Parse(typeof(FuelType), (string)values[7]);
            GPS = bool.Parse((string)values[8]);
            sunRoof = bool.Parse((string)values[9]);
            colour = (string)values[10];
        }
        #endregion

        #region Vehicle Methods
        // This method should return a CSV representation of the vehicle that is consistent
        // with the provided data files.
        public string ToCSVstring()
        {
            if (!SearchUsed)
            {
                return registration + "," + grade + "," + make + "," + model + "," + year + "," + numSeats + "," +
                       transmission + "," + fuel + "," + GPS + "," + sunRoof + "," + dailyRate + "," + colour;
            }
            else
            {
                return registration + "," + grade + "," + make + "," + model + "," + year + "," + numSeats + " Seater," +
                      transmission + "," + fuel + "," + (GPS.ToString() == "True" ? "GPS" : " ") + "," + (sunRoof.ToString() == "True" ? "sunroof" : " ") + "," + colour;
            }
        }

        // This method should return a string representation of the attributes of the vehicle.
        public override string ToString()
        {
            if (!SearchUsed)
            {
                return "<" + registration + ">," + "<" + grade + ">," + "<" + make + ">," + "<" + model + ">," + "<" + year + ">," + "<" + numSeats + ">," +
                       "<" + transmission + ">," + "<" + fuel + ">," + "<" + GPS + ">," + "<" + sunRoof + ">," + "<" + dailyRate + ">," + "<" + colour + ">";
            }
            else
            {
                return "<" + registration + ">," + "<" + grade + ">," + "<" + make + ">," + "<" + model + ">," + "<" + year + ">," + "<" + numSeats + ">," +
                       "<" + transmission + ">," + "<" + fuel + ">," + "<" + GPS + ">," + "<" + sunRoof + ">," + "<" + colour + ">";
            }
        }

        // This method should return a list of strings which represent each attribute. Values
        // should be made to be unique (e.g. numSeats should not be written as “4” but as “4
        // Seater”, sunroof should not be written as “True” but as “sunroof” or with no string
        // added if there is no sunroof). Vehicle rego, grade, make, model, year, transmission
        // type, fuel type, and colour can all be assumed to not overlap (i.e. if the make
        // “Mazda” exists, “Mazda” will not exist in other attributes). You do not need to
        // maintain this restriction, only assume it is true. You do not need to add the daily
        // rate to this list.
        public List<string> GetAttributeList()
        {
            List<string> attributes = new List<string>();

            attributes.Add(registration.ToString());
            attributes.Add(grade.ToString());
            attributes.Add(make.ToString());
            attributes.Add(model.ToString());
            attributes.Add(year.ToString());
            attributes.Add(numSeats.ToString() + " Seater");
            attributes.Add(transmission.ToString());
            attributes.Add(fuel.ToString());
            // Only add GPS if its true
            if (GPS)
            {
                attributes.Add("GPS");
            }
            else
            {
                attributes.Add(" ");
            }
            // Add SunRoof if true
            if (sunRoof)
            {
                attributes.Add("sunroof");
            }
            else
            {
                attributes.Add(" ");
            }

            attributes.Add(colour.ToString());

            return attributes;
        }
        #endregion
    }
}
