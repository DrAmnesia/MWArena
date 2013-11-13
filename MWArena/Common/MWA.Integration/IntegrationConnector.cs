using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MWA.Integration
{
    public class IntegrationConnector : IIntegrationConnector
    {
        public Func<bool> ConnectCommand { get;set; } 
        public IntegrationConnector()
        {
            connectionState = ConnState.NOTINITIALIZED;
            tokenSource = new CancellationTokenSource();
            ctoken = tokenSource.Token;
        }
        public IntegrationConnector(String connectorName, String connectorSourcePath)
            : this(connectorName)
        {
            connectorSource = connectorSourcePath;
        }
        public IntegrationConnector(string connectorName)
            : this()
        {
            name = connectorName;
        }


        public bool HasAsyncConnectionAction
        {
            get { return _hasAsyncConnectionAction; }
            set { _hasAsyncConnectionAction = value; }
        }

        public virtual void Disconnect()
        {
            tokenSource.Cancel();
            this.IsActive = false;
            this.ConnectionState = ConnState.DISCONNECTED;
        }
        public virtual void Connect()
        {
            var dueTime = TimeSpan.FromSeconds(5);
            var interval = TimeSpan.FromSeconds(5);
            ctoken = new CancellationToken();
        }
        public virtual void ConnectAndRefreshEvery(int refInterval)
        {
            var dueTime = TimeSpan.FromSeconds(refInterval);
            var interval = TimeSpan.FromSeconds(refInterval);
            DoPeriodicWorkAsync(dueTime, interval, ctoken).ConfigureAwait(false);
        }
        public string Name
        {
            get { return name; }
            set
            {
                if (value != this.name)
                {
                    this.name = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public String ConnectorSource
        {
            get { return connectorSource; }
            set
            {
                if (value != this.connectorSource)
                {
                    this.connectorSource = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                if (value != this.isConnected)
                {
                    this.isConnected = value;
                    NotifyPropertyChanged();
                }
            }
        }
        /// <summary> Gets or sets a value indicating whether this instance is active.</summary>
        /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        /// <remarks>Setting this value to false will cancel refreshing process.</remarks>
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                if (value != this.isActive)
                {
                    this.isActive = value;
                    if (!this.isActive)
                    {
                       //tokenSource.Cancel();
                    }
                    NotifyPropertyChanged();
                }
            }
        }
        public bool HasRefreshRequest
        {
            get { return hasRefreshRequest; }
            set
            {
                if (value != this.hasRefreshRequest)
                {
                    this.hasRefreshRequest = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public ConnState ConnectionState
        {
            get { return connectionState; }
            set
            {
                if (value != this.connectionState)
                {
                    this.connectionState = value;
                    NotifyPropertyChanged();
                }
            }
        }
  /*      public Task<bool> connectionAction()
        {

            return Task.Factory.StartNew(() =>
            {
                //do a lot of work without awaiting
                //any other Task
                return  ConnectionAction();
            });
       }*/ 
        public virtual async Task<bool> ConnectionAction()
        {
            Thread.Sleep(2000);
            return true;
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
                    var t = await Task.Run(() => this.ConnectionAction()).ContinueWith((o) => UpdateRefreshStatus(o), token, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext());
                    await Task.Delay(dueTime, token);
                }
            }
        }

        protected Task UpdateRefreshStatus(Task<bool> t)
        {
            tokenSource.Token.ThrowIfCancellationRequested();

            this.HasRefreshRequest = false;
            if (t.Result == true)
            {

                this.ConnectionState = ConnState.CONNECTED;
                this.IsConnected = true;
            }
            else
            {

                this.ConnectionState = ConnState.ERROR;
                this.IsConnected = false;

            }
            this.IsActive = false;
            return t;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        // This method is called by the Set accessor of each property. 
        // The CallerMemberName attribute that is applied to the optional propertyName 
        // parameter causes the property name of the caller to be substituted as an argument. 
        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected bool hasRefreshRequest = false;
        protected bool isActive = false;
        protected ConnState connectionState = ConnState.NOTINITIALIZED;
        protected bool isConnected = false;
        protected Uri apiUrl;
        protected string name;
        protected CancellationToken ctoken;
        protected CancellationTokenSource tokenSource;
        protected string connectorSource;
        private bool _hasAsyncConnectionAction;
    }
}
