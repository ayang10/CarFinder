using System;
using System.Net.Http;
using System.Web.Http;
using CarFinder.Models;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Net;
using System.Collections.Generic;
using System.IO;

namespace CarFinder.Controllers
{
    

    public class CarsController : ApiController  //create class to inherit Apicontroller
    {
        // Database connection
        CarsDb db = new CarsDb();
        
        /// <summary>
        /// This action retrieves a list of available car model years.
        /// </summary>
        /// <returns>IEnumerable list of years</returns>

        // GET : api/Cars/Years
        [ActionName("Years")]
        public async Task<IHttpActionResult> GetYears()
        {
            // go and get the data
            var retval = await db.Database.SqlQuery<string>("EXEC GetYears").ToListAsync();
            if (retval != null)
                return Ok(retval);
            else
            return NotFound();
        }

        /// <summary>
        /// This action retrieves a list of available car years, makes
        /// </summary>
        /// <param name="year"></param>
        /// <returns>IEnumerable list of years and makes</returns>

        // GET : api/Cars/Makes
        [ActionName("Makes")]
        public async Task<IHttpActionResult> GetMakes(string year)
        {
            // go and get the data
            var retval = await db.Database.SqlQuery<string>("EXEC GetMakesByYear @year",
                new SqlParameter("year", year)).ToListAsync();

            if (retval != null)
                return Ok(retval);
            else
                return NotFound();
        }

        /// <summary>
        /// This action retrieves a list of available car years, makes, models
        /// </summary>
        /// <param name="year"></param>
        /// <param name="make"></param>
        /// <returns>IEnumerable list of years, makes, and models</returns>

        // GET : api/Cars/Models
        [ActionName("Models")]
        public async Task<IHttpActionResult> GetModels(string year, string make)
        {
            // go and get the data
            var retval = await db.Database.SqlQuery<string>("EXEC GetModelYearAndMake @year, @make",
                new SqlParameter("year", year), 
                new SqlParameter("make", make)).ToListAsync();
            if (retval != null)
                return Ok(retval);
            else
                return NotFound();
        }

        /// <summary>
        /// This action retrieves a list of available car years, makes, models, trims
        /// </summary>
        /// <param name="year"></param>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <returns>IEnumerable list of years, makes, models, and trims</returns>

        // GET : api/Cars/Trim
        [ActionName("Trims")]
        public async Task<IHttpActionResult> GetTrims(string year, string make, string model)
        {
            // go and get the data
            var retval = await db.Database.SqlQuery<string>("EXEC GetTrimsByYearMakeAndModel @year, @make, @model",
                new SqlParameter("year", year), 
                new SqlParameter("make", make), 
                new SqlParameter("model", model)).ToListAsync();
            if (retval != null)
                return Ok(retval);
            else
                return NotFound();
        }

        /// <summary>
        /// This action retrieves a list of car available in years, makes, models, and trims
        /// </summary>
        /// <param name="year"></param>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <param name="trim"></param>
        /// <returns>Returns a list of car objects</returns>

        [ActionName("Cars")]
        public async Task<IHttpActionResult> GetMatchingYearMakeModelAndTrim(string year, string make, string model)
        {
            var carData = new CarData();

            carData.cars = await db.Database.SqlQuery<Car>("EXEC GetMatchingYearMakeModelAndTrim @year, @make, @model",
                new SqlParameter("year", year),
                new SqlParameter("make", make),
                new SqlParameter("model", model)).ToListAsync();
            carData.recalls = GetRecalls(year, make, model);

           
            return Ok(carData);

        }




        // GET : api/Cars/Matching
        [ActionName("Cars")]
        public async Task<IHttpActionResult> GetMatchingYearMakeModelAndTrim(string year, string make, string model, string trim)
        {
            var carData = new CarData();

            //execute fomr database, year, make, model, trim
            carData.cars = await db.Database.SqlQuery<Car>("EXEC GetMatchingYearMakeModelAndTrim @year, @make, @model, @trim",
                new SqlParameter("year", year), 
                new SqlParameter("make", make), 
                new SqlParameter("model", model), 
                new SqlParameter("trim", trim)).ToListAsync();

            carData.recalls = GetRecalls(year, make, model);
            //string imageURLs
            carData.imageURLs = GetImages(year, make, model, trim);
                return Ok(carData);
            
        }
        
       

        //This private class will get year, make, model recalls
        private Recalls GetRecalls(string year, string make, string model)
        {
            HttpResponseMessage response;
            Recalls recalls;

            //HttpClient (and certain other web request objects) are not necessarily disposable 
            //objects, unlike HttpResponseMessage, which is. A best practice is to wrap such
            //resources within a using statment to allow early and determined clearnup.


            using (var client = new HttpClient())
            {
                // 1) establish base client address
                client.BaseAddress = new Uri("http://www.nhtsa.gov/");

                try
                {
                    // 2) make request to specific URL on the client
                    response = client.GetAsync("webapi/api/Recalls/vehicle/modelyear/" + year + "/make/" + make + "/model/" + model + "?format=json").Result;

                    // 3) construct a Recalls object from the resulting JSON data
                    recalls = response.Content.ReadAsAsync<Recalls>().Result;

                    //recalls = response.Content.ReadAsStringAsync();
                }
                catch (Exception)
                {
                    //return InternalServerError(e);
                    return null;
                }

            }
            return recalls;

        }

        /* This class will get images for year, make, and model */
        private string[] GetImages(string year, string make, string model)
        {
            // This is the query - or you could get it from args.
            string query = year + " " + make + " " + model;

            // Create a Bing container.
            string rootUri = "https://api.datamarket.azure.com/Bing/Search";

            var bingContainer = new Bing.BingSearchContainer(new Uri(rootUri));

            // Replace this value with your account key.
            var accountKey = "B7fUsZ3RushD0tSHsGmJfD2UywS5m4cpIlJ9v5Uca/M=";

            // Configure bingContainer to use your credentials.
            bingContainer.Credentials = new NetworkCredential("accountKey", accountKey);

            // Build the query.
            var imageQuery = bingContainer.Image(query, null, null, null, null, null, null);

            imageQuery = imageQuery.AddQueryOption("$top", 5);
            var imageResults = imageQuery.Execute();

            //extract the properties need for the images
            List<string> images = new List<string>();

            //for each results in this collection
            foreach (var result in imageResults)
            {
                //insert new images
                //if else statement to handle img error
                images.Add(result.MediaUrl);


            }
            //matching and converting into strings.
            return images.ToArray();
        }

        /* This class will get images for year, make, and model */
        private string[] GetImages(string year, string make, string model, string trim)
        {
            // This is the query - or you could get it from args.
            string query = year + " " + make + " " + model + " " + trim;

            // Create a Bing container.
            string rootUri = "https://api.datamarket.azure.com/Bing/Search";

            var bingContainer = new Bing.BingSearchContainer(new Uri(rootUri));

            // Replace this value with your account key.
            var accountKey = "B7fUsZ3RushD0tSHsGmJfD2UywS5m4cpIlJ9v5Uca/M=";

            // Configure bingContainer to use your credentials.
            bingContainer.Credentials = new NetworkCredential("accountKey", accountKey);

            // Build the query.
            var imageQuery = bingContainer.Image(query, null, null, null, null, null, null);

            imageQuery = imageQuery.AddQueryOption("$top", 5);
            var imageResults = imageQuery.Execute();

            //extract the properties need for the images
            List<string> images = new List<string>();

           
            //for each results in this collection
            foreach (var result in imageResults)
            {
                if (UrlCtrl.IsUrl(result.MediaUrl))
                {
                    images.Add(result.MediaUrl);
                  
                }
                else
                {
                    continue;
                }
                
            }
            //matching and converting into strings.
            return images.ToArray();
        }

      public static class UrlCtrl
        {
            public static bool IsUrl(string path)
            {
                HttpWebResponse response = null;
                var request = (HttpWebRequest)WebRequest.Create(path);
                request.Method = "HEAD";
                bool result = true;

                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch (WebException ex)
                {
                    /*A WebException will be thrown if the status of the response is not '200 OK' */
                    result = false;
                }
                finally
                {
                    // Don't forget to close your response.
                    if (response != null)
                    {
                        response.Close();
                    }
                }
                return result;
            }
        }



    }
}




//Next we need to collect recall information from the NHTSA database using their API
//http://www.nhtsa.gov/webapi/Default.aspx?Recalls/API/83
//The documentation at this site is an example of what you can expect when interacting with
//third party APIs - not necessarily very good, not always accurate, but usually decipherable.




