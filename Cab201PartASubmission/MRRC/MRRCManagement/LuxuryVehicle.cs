namespace MRRCManagement
{
    // Derived class under the Vehicle class - Luxury vehicle. 3 Attributes are defaulted
    public class LuxuryVehicle : Vehicle
    {
        // This is the constructor of the luxury vehicle class which is invoked when an luxury vehicle object is created.
        // Upon creation, the GPS, sunroof and dailyrate are defaulted
        public LuxuryVehicle(string registration, VehicleGrade grade, string make, string model, int year)
                                : base(registration, grade, make, model, year)
        {
            // Defaults are set for GPS, sunRoof and dailyRate to true, true and 120, respectively.
            GPS = true;
            sunRoof = true;
            dailyRate = 120;
        }
    }

}
