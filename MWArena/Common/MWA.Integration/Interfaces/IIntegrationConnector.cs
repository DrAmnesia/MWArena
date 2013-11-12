using System;
using System.ComponentModel;

namespace MWA.Integration
{
    public interface IIntegrationConnector:INotifyPropertyChanged
    {
        void Disconnect();
        void Connect();
        void ConnectAndRefreshEvery(int refInterval);
        String Name { get; set; }
        String ConnectorSource { get; set; }
        bool IsConnected { get; set; }

        /// <summary> Gets or sets a value indicating whether this instance is active.</summary>
        /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        /// <remarks>Setting this value to false will cancel refreshing process.</remarks>
        bool IsActive { get; set; }

        bool HasRefreshRequest { get; set; }
        ConnState ConnectionState { get; set; }
        event PropertyChangedEventHandler PropertyChanged;
    }
}