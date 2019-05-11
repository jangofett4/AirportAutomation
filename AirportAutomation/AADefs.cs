using System;

namespace AirportAutomation
{
    public class Country
    {
        public int ID;
        public string Name;
    }

    public class City
    {
        public int ID;
        public int CountryID;
        public string Name;
    }

    public class Staff
    {
        public int ID;
        public string Name;
        public string Surname;
        public string Username;
        public string Password;
        public string TC;
        public int AirportID;
    }

    public class Airline
    {
        public int ID;
        public string Name;
    }

    public class Airport
    {
        public int ID;
        public string Name;
        public int CityID;
        public int AdminID;
    }

    public class Plane
    {
        public int ID;
        public int ModelID;
    }

    public class Model
    {
        public int ID;
        public string Name;
        public int Capacity;
        public int Type;
    }

    public class ModelType
    {
        public int ID;
        public string Name;
    }

    public struct Luggage
    {
        public int FlightID;
        public int PassengerID;
    }

    public class Passenger
    {
        public int ID;
        public string Name;
        public string Surname;
        public string TC;
        public int FlightID;
    }

    public class MasterAdmin
    {
        public int ID;
        public string Name;
        public string Surname;
        public string Username;
        public string Password;
        public string TC;
    }

    public class AirportAdmin
    {
        public int ID;
        public string Name;
        public string Surname;
        public string Username;
        public string Password;
        public string TC;
        public int AirportID;
    }

    public class Pilot
    {
        public int ID;
        public string Name;
        public string Surname;
        public string TC;
    }

    public class Flight
    {
        public int ID;
        public int AirlineID;
        public int TakeoffCity;
        public int LandingCity;
        public DateTime TakeoffDate;
        public int PlaneID;
        public int PilotID;
        public int CoPilotID;
    }
}
