using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace CarFinder.Models
{
    public class Car //create class to old var of car information
    {
        public int id { get; set; }
        public string make { get; set; }
        public string model_name { get; set; }
        public string model_trim { get; set; }
        public string model_year { get; set; }
        public string body_style { get; set; }
        public string engine_position { get; set; }
        public string engine_cc { get; set; }
        public string engine_num_cyl { get; set; }
        public string engine_type { get; set; }
        public string engine_valves_per_cyl { get; set; }
        public string engine_power_ps { get; set; }
        public string engine_power_rpm { get; set; }
        public string engine_torque_nm { get; set; }
        public string engine_torque_rpm { get; set; }
        public string engine_compression { get; set; }
        public string engine_fuel { get; set; }
        public string top_speed_kph { get; set; }
        public string zero_to_100_kph { get; set; }
        public string drive_type { get; set; }
        public string transmission_type { get; set; }
        public string seats { get; set; }
        public string doors { get; set; }
        public string weight_kg { get; set; }
        public string wheelbase { get; set; }
        public string lkm_hwy { get; set; }
        public string lkm_mixed { get; set; }
        public string lkm_city { get; set; }
        public string fuel_capacity_l { get; set; }
        public string make_display { get; set; }

        public static implicit operator Car(string v)
        {
            throw new NotImplementedException();
        }

        public static implicit operator Car(List<object> v)
        {
            throw new NotImplementedException();
        }
    }
    //To create C# classes that correspond to a JSON result, copy the sample result JSON string,
    //then from the Edit menu in VS, select Paste Special, then choose Paste JSON As Classes.

    public class Recalls
    {
        public int Count { get; set; }
        public string Message { get; set; }
        public RecallItem[] Results { get; set; }
    }

    public class RecallItem
    {
        public string Manufacturer { get; set; }
        public string NHTSACampaignNumber { get; set; }
        public DateTime ResportReceivedDate { get; set; }
        public string Component { get; set; }
        public string Summary { get; set; }
        public string Conequence { get; set; }
        public string Remedy { get; set; }
    }

    public class CarData
    {
        //instance of Car
        //public Car cars { get; set; }
        public List<Car> cars { get; set; } //Make this object of Car a list!!!
        public Recalls recalls { get; set; } //create Object of Recalls
        public string[] imageURLs { get; set; } //create array[] object of imageURLs

    }
    

    //This is our connection
    public class CarsDb : DbContext
    {
        public CarsDb() : base("AzureConnection")
        {

        }

        public static CarsDb Create()
        {
            return new CarsDb();
        }

    }

   
    

}