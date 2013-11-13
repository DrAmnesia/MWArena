using System;
using System.ComponentModel;
using MWA.Models.Annotations;

namespace MWA.Models
{
    /// <summary>
    /// Class UserAppSetting.
    /// </summary>
    public class UserAppSetting : INotifyPropertyChanged 
    {
        private int _userAppSettingId;
        private string _appSettingValue;
        private AppSetting _appSetting;

        /// <summary>
        /// Gets or sets the user application setting identifier.
        /// </summary>
        /// <value>The user application setting identifier.</value>
        public int UserAppSettingId
        {
            get { return _userAppSettingId; }
            set
            {
                if (value == _userAppSettingId) return;
                _userAppSettingId = value;
                OnPropertyChanged("UserAppSettingId");
            }
        }

        public MechWarrior MechWarrior { get; set; }

        /// <summary>
        /// Gets or sets the application setting value.
        /// </summary>
        /// <value>The application setting value.</value>
        public String AppSettingValue
        {
            get { return _appSettingValue; }
            set
            {
                if (value == _appSettingValue) return;
                _appSettingValue = value;
                OnPropertyChanged("AppSettingValue");
            }
        }

        /// <summary>
        /// Gets or sets the application setting.
        /// </summary>
        /// <value>The application setting.</value>
        public AppSetting AppSetting { get { return _appSetting; } set { _appSetting = value; } }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}