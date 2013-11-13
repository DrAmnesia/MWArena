// ***********************************************************************
// Assembly         :  MWA.Web
// Author           :  D R Bowden
// Created          : 201311121354 
//
// Last Modified By : D R Bowden
// Last Modified On : 201311120000
// ***********************************************************************
// <copyright file="MechWarriorInfo.cs" company="Allscripts">
//     Copyright (c) Allscripts. All rights reserved.
// </copyright>
// <summary>MwArena MWA.Web MechWarriorInfo.cs </summary>
// ***********************************************************************

using System.Collections.Generic;
using System.ComponentModel;
using MWA.Models;
using MWA.Models.Annotations;

namespace MWA.Models
{

    /// <summary> MWA.Web.Models.MechWarriorInfo class implements the <see cref="MWA.Web.Models.IMechWarriorInfo">MWA.Web.Models.IMechWarriorInfo</see> interface</summary>
    public class MechWarriorInfo :INotifyPropertyChanged // IMechWarriorInfo
         
    {
        private string _factionName;
        private string _regimentName;
        private string _battalionName;
        private string _companyName;
        private string _lanceName;
        private string _rankName;
        private string _imageUrl;
        private string _confirmationToken;
        private bool _isConfirmed;
        private string _email;
        private List<UserPref> _userPrefs;

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="MechWarriorInfo">MWA.Web.Models.MechWarriorInfo</see> with no parameters.
        /// </summary>
        public MechWarriorInfo()
        {
            
        }

        #endregion

        #region IMechWarriorInfo Members

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the faction.
        /// </summary>
        /// <value>The name of the faction.</value>
        public string FactionName
        {
            get { return _factionName; }
            set
            {
                if (value == _factionName) return;
                _factionName = value;
                OnPropertyChanged("FactionName");
            }
        }


        /// <summary>
        /// Gets or sets the name of the regiment.
        /// </summary>
        /// <value>The name of the regiment.</value>
        public string RegimentName
        {
            get { return _regimentName; }
            set
            {
                if (value == _regimentName) return;
                _regimentName = value;
                OnPropertyChanged("RegimentName");
            }
        }


        /// <summary>
        /// Gets or sets the name of the battalion.
        /// </summary>
        /// <value>The name of the battalion.</value>
        public string BattalionName
        {
            get { return _battalionName; }
            set
            {
                if (value == _battalionName) return;
                _battalionName = value;
                OnPropertyChanged("BattalionName");
            }
        }


        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>The name of the company.</value>
        public string CompanyName
        {
            get { return _companyName; }
            set
            {
                if (value == _companyName) return;
                _companyName = value;
                OnPropertyChanged("CompanyName");
            }
        }


        /// <summary>
        /// Gets or sets the name of the lance.
        /// </summary>
        /// <value>The name of the lance.</value>
        public string LanceName
        {
            get { return _lanceName; }
            set
            {
                if (value == _lanceName) return;
                _lanceName = value;
                OnPropertyChanged("LanceName");
            }
        }


        /// <summary>
        /// Gets or sets the name of the rank.
        /// </summary>
        /// <value>The name of the rank.</value>
        public string RankName
        {
            get { return _rankName; }
            set
            {
                if (value == _rankName) return;
                _rankName = value;
                OnPropertyChanged("RankName");
            }
        }


        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>The image URL.</value>
        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                if (value == _imageUrl) return;
                _imageUrl = value;
                OnPropertyChanged("ImageUrl");
            }
        }



        public List<UserPref> UserPrefs
        {
            get { return _userPrefs; }
            set
            {
                if (Equals(value, _userPrefs)) return;
                _userPrefs = value;
                OnPropertyChanged("UserPrefs");
            }
        }

        #endregion

        #region Methods
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Extensibility Points - Events, Partial Methods and Virtual Methods

        #endregion

        #region Helpers - Protected and Private

        #endregion

        #region Private

        #endregion
    }
}