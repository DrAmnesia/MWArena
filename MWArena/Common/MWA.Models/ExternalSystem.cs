using System.Collections.Generic;
using System.ComponentModel;
using MWA.Models.Annotations;

namespace MWA.Models
{
    /// <summary>
    /// Class ExternalSystem.
    /// </summary>
    public class ExternalSystem : INotifyPropertyChanged 
    {
        private int _externalSystemId;
        private string _externalSystemName;
        private string _externalSystemType;
        private string _externalSystemEndpoint;
        private string _externalSystemIcon;
        private string _externalSystemAbbrv;
        private string _externalSystemDescription;
        private bool _isEnabled;

        public int ExternalSystemId { get { return _externalSystemId; } set
        {
            if (value == _externalSystemId) return;
            _externalSystemId = value;
            OnPropertyChanged("ExternalSystemId");
        }
        }

        public string ExternalSystemName { get { return _externalSystemName; } set
        {
            if (value == _externalSystemName) return;
            _externalSystemName = value;
            OnPropertyChanged("ExternalSystemName");
        }
        }

        /// <summary>
        /// Gets or sets the type of the external system.
        /// </summary>
        /// <value>The type of the external system.</value>
        public string ExternalSystemType
        {
            get { return _externalSystemType; }
            set
            {
                if (value == _externalSystemType) return;
                _externalSystemType = value;
                OnPropertyChanged("ExternalSystemType");
            }
        }

        /// <summary>
        /// Gets or sets the external system endpoint.
        /// </summary>
        /// <value>The external system endpoint.</value>
        public string ExternalSystemEndpoint
        {
            get { return _externalSystemEndpoint; }
            set
            {
                if (value == _externalSystemEndpoint) return;
                _externalSystemEndpoint = value;
                OnPropertyChanged("ExternalSystemEndpoint");
            }
        }

        /// <summary>
        /// Gets or sets the external system icon.
        /// </summary>
        /// <value>The external system icon.</value>
        public string ExternalSystemIcon
        {
            get { return _externalSystemIcon; }
            set
            {
                if (value == _externalSystemIcon) return;
                _externalSystemIcon = value;
                OnPropertyChanged("ExternalSystemIcon");
            }
        }

        /// <summary>
        /// Gets or sets the external system abbrv.
        /// </summary>
        /// <value>The external system abbrv.</value>
        public string ExternalSystemAbbrv
        {
            get { return _externalSystemAbbrv; }
            set
            {
                if (value == _externalSystemAbbrv) return;
                _externalSystemAbbrv = value;
                OnPropertyChanged("ExternalSystemAbbrv");
            }
        }

        /// <summary>
        /// Gets or sets the external system description.
        /// </summary>
        /// <value>The external system description.</value>
        public string ExternalSystemDescription
        {
            get { return _externalSystemDescription; }
            set
            {
                if (value == _externalSystemDescription) return;
                _externalSystemDescription = value;
                OnPropertyChanged("ExternalSystemDescription");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is enabled; otherwise, <c>false</c>.</value>
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (value.Equals(_isEnabled)) return;
                _isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }


        /// <summary>
        /// Gets or sets the user application settings.
        /// </summary>
        /// <value>The user application settings.</value>
        public virtual ICollection<AppSetting> AppSettings { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}