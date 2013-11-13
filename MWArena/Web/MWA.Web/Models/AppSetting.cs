using System.ComponentModel;
using MWA.Models.Annotations;

namespace MWA.Models
{
    /// <summary>
    /// Class AppSetting.
    /// </summary>
    public class AppSetting : INotifyPropertyChanged 
    {
        private int _appSettingId;
        private int _appSettingPrefix;
        private int _appSettingName;
        private int _isEnabled;

        /// <summary>
        /// Gets or sets the application setting identifier.
        /// </summary>
        /// <value>The application setting identifier.</value>
        public int AppSettingId
        {
            get { return _appSettingId; }
            set
            {
                if (value == _appSettingId) return;
                _appSettingId = value;
                OnPropertyChanged("AppSettingId");
            }
        }

        /// <summary>
        /// Gets or sets the application setting prefix.
        /// </summary>
        /// <value>The application setting prefix.</value>
        public int AppSettingPrefix
        {
            get { return _appSettingPrefix; }
            set
            {
                if (value == _appSettingPrefix) return;
                _appSettingPrefix = value;
                OnPropertyChanged("AppSettingPrefix");
            }
        }

        /// <summary>
        /// Gets or sets the name of the application setting.
        /// </summary>
        /// <value>The name of the application setting.</value>
        public int AppSettingName
        {
            get { return _appSettingName; }
            set
            {
                if (value == _appSettingName) return;
                _appSettingName = value;
                OnPropertyChanged("AppSettingName");
            }
        }

        /// <summary>
        /// Gets or sets the is enabled.
        /// </summary>
        /// <value>The is enabled.</value>
        public int IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (value == _isEnabled) return;
                _isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        public ExternalSystem ExternalSystem { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}