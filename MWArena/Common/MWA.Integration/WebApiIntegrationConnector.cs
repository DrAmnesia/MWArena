using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWA.Integration
{
    public class WebApiIntegrationConnector : IntegrationConnector
    {
        private bool debug = false;
        private Uri apiUrl;

        public WebApiIntegrationConnector()
            : base("")
        {

        }

        public WebApiIntegrationConnector(string name)
            : base(name)
        {

        }
        public WebApiIntegrationConnector(string name, Uri ApiUrl)
            : base(name, ApiUrl.ToString())
        {

        }
        public Uri ApiUrl
        {
            get { return apiUrl; }
            set
            {
                if (value != this.apiUrl)
                {
                    this.apiUrl = value;
                    NotifyPropertyChanged();
                }
            }
        }

    }
}
