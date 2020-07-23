namespace MRRCManagement
{
    // Derived class under the Vehicle class - Commerical vehicle. 2 Attributes are defaulted
    public class CommercialVehicle : Vehicle
    {
        // This is the constructor of the commercial vehicle class which is invoked when an commercial vehicle object is created.
        // Upon creation, the fuel and dailyrate are defaulted
        public CommercialVehicle(string registration, VehicleGrade grade, string make, string model, int year)
                                : base(registration, grade, make, model, year)
        {
            // Set defaults of fuel and dailyrate to dieseltype and 130, respectively.
            fuel = FuelType.Diesel;
            dailyRate = 130;
        }
    }
}
