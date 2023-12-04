using System;
using RestSharp;


namespace ICream.Models

{
    public class HebCalAdapter
    {

        public string Check()
        {
            string Url = $"http://localhost:5036/api/HebCal";

            var client = new RestClient(Url);

            var request = new RestRequest(new Uri(Url), Method.Get);


            RestResponse response = client.Execute(request);

            return response.Content;
        }
        
    }

    public enum Holiday { None, Rosh_Hashana, Yom_Kippur, Succot, Shmini_Atzeret, Hannuka, Tubishvat, Purim, Pesach, Shavouot, Lagbaomer }


}

