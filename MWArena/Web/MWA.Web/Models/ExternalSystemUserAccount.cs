using System;
using System.Collections.Generic;
using System.ComponentModel;
using MWA.Models.Annotations;

namespace MWA.Models
{
    /// <summary>
    /// Class ExternalSystemUserAccount.
    /// </summary>
    public class ExternalSystemUserAccount : INotifyPropertyChanged
    {
      

        public int ExternalSystemUserAccountId
        {
            get { return _userExternalSystemAccountId; }
            set
        {
            if (value == _userExternalSystemAccountId) return;
            _userExternalSystemAccountId = value;
            OnPropertyChanged("UserExternalSystemAccountId");
        }
        }

        public MechWarrior MechWarrior { get; set; }

        public string ExternalSystemAccountName { get { return _externalSystemAccountName; } set
        {
            if (value == _externalSystemAccountName) return;
            _externalSystemAccountName = value;
            OnPropertyChanged("ExternalSystemAccountName");
        }
        }


       

        public string ExternalSystemAccountPassword { get { return _externalSystemAccountPassword; } set
        {
            if (value == _externalSystemAccountPassword) return;
            _externalSystemAccountPassword = value;
            OnPropertyChanged("ExternalSystemAccountPassword");
        }
        }

        public bool IsEnabled { get { return _isEnabled; } set
        {
            if (value.Equals(_isEnabled)) return;
            _isEnabled = value;
            OnPropertyChanged("IsEnabled");
        }
        }

        private int _userExternalSystemAccountId;
        private string _externalSystemAccountName;
        private string _externalSystemAccountPassword;
        private bool _isEnabled;
        private ExternalSystem _externalSystem;
      

        /// <summary>
        /// Gets or sets the user application settings.
        /// </summary>
        /// <value>The user application settings.</value>
        public virtual ICollection<UserAppSetting> UserAppSettings { get; set; }

        /// <summary>
        /// Gets or sets the external system.
        /// </summary>
        /// <value>The external system.</value>
        public ExternalSystem ExternalSystem { get { return _externalSystem; } set { _externalSystem = value; } }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}