using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MWA.Models;

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
                    this.connectorSource = value.ToString();
                    NotifyPropertyChanged();
                }
            }
        }


        public MWA.Models.ExternalSystem ExtSystemInformation
        {
            get { return _extSystemInformation; }
            set { _extSystemInformation = value; }
        }

        public MWA.Models.ExternalSystemUserAccount ExtUserAccountInfo
        {
            get { return _extUserAccountIfo; }
            set { _extUserAccountIfo = value; }
        }


        private ExternalSystem _extSystemInformation;
        private ExternalSystemUserAccount _extUserAccountIfo;

     

    }
}
