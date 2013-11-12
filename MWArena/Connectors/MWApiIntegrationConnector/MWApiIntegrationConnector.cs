using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MWA.Models;

namespace MWA.Integration
{
    public class MWApiIntegrationConnector: WebApiIntegrationConnector 
    {

        //todo: add ITrulyObservableCollection<INotifyPropertyChanged> for data results
        //add ext i
        private bool debug = false;
        

        public MWApiIntegrationConnector()
            : base("")
        {

        }

        public MWApiIntegrationConnector(string name)
            : base(name)
        {

        }
        public MWApiIntegrationConnector(string name, Uri ApiUrl)
            : base(name, ApiUrl)
        {

        }



        public override async Task<bool> ConnectionAction()
        {
            // load users Connector Settings
            HttpClient client = new HttpClient();
            client.BaseAddress = this.ApiUrl;
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response;

            string endp = "ExternalSystemUserAccountController";
            response =  await client.GetAsync(endp);
            response.EnsureSuccessStatusCode(); // Throw on error code.
            var exua = response.Content.ReadAsAsync<IEnumerable<ExternalSystemUserAccount>>().Result;
            if (this.ExtUserAccountInfo == null)
            {
                this.ExtUserAccountInfo = exua.FirstOrDefault();
                return true;
            }
            return false;

            /*
            HttpClient client = new HttpClient();
            client.BaseAddress = this.ApiUrl;
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response;
            try
            {
                string endp = String.Format("mwoamatchmetric/{0}", ExtUserAccountIfo.ExternalSystemAccountName);
                response = await client.GetAsync(endp);
                response.EnsureSuccessStatusCode(); // Throw on error code.
                var matches = await response.Content.ReadAsAsync<IEnumerable<MwoAMatchMetric>>();
               // MwaMainDataGrid.ItemsSource = matches.Select(o => new { o.level, o.matchType, o.mech, o.kills, o.damage });

            }
            catch (Newtonsoft.Json.JsonException jEx)
            {
                // This exception indicates a problem deserializing the request body.
               // InfoBlock.Text = "ERROR:" + jEx.Message;
               // mwApiConn.ConnectionState = ConnState.ERROR;
            }
            catch (HttpRequestException ex)
            {
                //InfoBlock.Text = "ERROR:" + ex.Message;
               // mwApiConn.ConnectionState = ConnState.ERROR;
            }
            finally
            {
               // mwApiConn.ConnectionState = ConnState.CONNECTED;
               // tbMWApiStatus.Fill = (mwApiConn.IsConnected) ? System.Windows.Media.Brushes.LimeGreen : (mwApiConn.ConnectionState == ConnState.ERROR) ? System.Windows.Media.Brushes.Red : System.Windows.Media.Brushes.DimGray; ;

            }
          * */
        }

        public async void LinkLogwarriorAndMWA( MWApiIntegrationConnector waic, string UserName)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = waic.ApiUrl;
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response;

            string endp = "ExternalSystemUserAccountController";
            response = await client.GetAsync(endp);
            response.EnsureSuccessStatusCode(); // Throw on error code.
            waic.ExtUserAccountInfo =   response.Content.ReadAsAsync<IEnumerable<ExternalSystemUserAccount>>().Result.FirstOrDefault();
            // MwaMainDataGrid.ItemsSource = matches.Select(o => new { o.level, o.matchType, o.mech, o.kills, o.damage });

        }
    }
}
