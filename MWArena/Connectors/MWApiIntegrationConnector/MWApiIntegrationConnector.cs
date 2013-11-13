using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using MWA.Models;
using Newtonsoft.Json.Linq;

namespace MWA.Integration
{
    public class MWApiIntegrationConnector: WebApiIntegrationConnector 
    {

        //todo: add ITrulyObservableCollection<INotifyPropertyChanged> for data results
        //add ext i
        private bool debug = false;
        private AuthenticationHeaderValue _authHeader;
        public IEnumerable<MwoAMatchMetric> MM { get; set; }
        public Func<ItemsControl,Task<bool>> ConnectCommand { get; set; }
        public ItemsControl ViewControl { get; set; }

        public MWApiIntegrationConnector()
            : base("")
        {
            _authHeader = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer","");
        }

        public MWApiIntegrationConnector(string name)
            : base(name)
        {
        
        }
        public MWApiIntegrationConnector(string name, Uri ApiUrl)
            : base(name, ApiUrl)
        {

        }
        public virtual async Task<bool> ConnectionAction()
        {
            //var b= ConnectCommand(ViewControl);
            var b = await GetMatches( );
            return b;
        }

        public virtual void ConnectAndRefreshEvery(int refInterval)
        {
            var dueTime = TimeSpan.FromSeconds(refInterval);
            var interval = TimeSpan.FromSeconds(refInterval);
            DoPeriodicWorkAsync(dueTime, interval, ctoken);
        }

        protected virtual async Task DoPeriodicWorkAsync(TimeSpan dueTime,
                                       TimeSpan interval,
                                       CancellationToken token)
        {
            // Initial wait time before we begin the periodic loop.
            if (dueTime > TimeSpan.Zero)
                await Task.Delay(dueTime, token);

            // Repeat this loop until cancelled.
            while (!token.IsCancellationRequested)
            {

                // Wait to repeat again.
                if (interval > TimeSpan.Zero)
                {
                    this.HasRefreshRequest = true;
                    this.ConnectionState = ConnState.CONNECTING;
                    this.IsActive = true;
                    tokenSource.Token.ThrowIfCancellationRequested();
                    // run the Action that is meant to fire when we refresh, then update the Connector Properties, then wait
                    try
                    {
                    var t = await GetMatches();//Task.Run(() => this.GetMatches());//.ContinueWith((o) => UpdateRefreshStatus(o), token, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext());
                    await Task.Delay(dueTime, token);
                    }
                    catch (Exception ex)
                    {
                        var e = ex;
                        throw;
                    }
                    
                }
            }
        }
        public AuthenticationHeaderValue AuthHeader { get { return _authHeader; } set { _authHeader = value; } }

        public async Task<bool> GetMatches()
        {
             
            // load users Connector Settings
            HttpClient client = new HttpClient();
            client.BaseAddress = this.ApiUrl;
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response;
            client.DefaultRequestHeaders.Authorization = AuthHeader;
            string endp = String.Format("{0}{1}", ApiUrl, "MwoAMatchMetric");

            response = client.GetAsync(endp).Result;

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Authentication failed");
            }
            else
            {
                var mm = response.Content.ReadAsAsync<IEnumerable<MwoAMatchMetric>>().Result;
                if (mm.Any())
                {
                    ViewControl.ItemsSource = mm;
                    return true;
                }
               
            }
 
           
            return false;

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
