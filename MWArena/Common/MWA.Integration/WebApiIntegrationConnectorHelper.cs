using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MWA.Integration;
using MWA.Models;

namespace MWApiIntegrationConnector
{
    public static class WebApiIntegrationConnectorHelper
    {

        public static async void ExtLogin(this WebApiIntegrationConnector waic,string userName, string password)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = waic.ApiUrl;
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response;
            string endp = "token";
            response = await client.GetAsync(endp);
            response.EnsureSuccessStatusCode(); // Throw on error code.
            var exua = response.Content.ReadAsAsync<IEnumerable<ExternalSystemUserAccount>>().Result;
            if (waic.ExtUserAccountInfo == null)
            {
                waic.ExtUserAccountInfo = exua.FirstOrDefault();
            }
            // MwaMainDataGrid.ItemsSource = matches.Select(o => new { o.level, o.matchType, o.mech, o.kills, o.damage }); 
        }
        public static async void ExtLogin()
        {

        }
        public static async void SetExtSystemUserAccountInfo(this WebApiIntegrationConnector waic,string UserName)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = waic.ApiUrl;
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response;
            string endp =  "ExternalSystemUserAccountController" ;
            response = await client.GetAsync(endp);
            response.EnsureSuccessStatusCode(); // Throw on error code.
                var exua =    response.Content.ReadAsAsync<IEnumerable<ExternalSystemUserAccount>>().Result;
            if (waic.ExtUserAccountInfo == null)
            {
                waic.ExtUserAccountInfo = exua.FirstOrDefault();
            }
                // MwaMainDataGrid.ItemsSource = matches.Select(o => new { o.level, o.matchType, o.mech, o.kills, o.damage });
   
        }
 
    }
}
