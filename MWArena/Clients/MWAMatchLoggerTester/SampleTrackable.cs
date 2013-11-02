// ***********************************************************************
// Assembly         :  CommonLibrarySamplesWinformApp
// Author           :  D R Bowden
// Created          : 201310301827 
//
// Last Modified By : D R Bowden
// Last Modified On : 201310300000
// ***********************************************************************
// <copyright file="SampleTrackable.cs" company="Allscripts">
//     Copyright (c) Allscripts. All rights reserved.
// </copyright>
// <summary>CommonLibraryWinform_nugettest CommonLibrarySamplesWinformApp SampleTrackable.cs </summary>
// ***********************************************************************

#region Default NameSpaces

using System;
using System.Collections.Generic;
using Common;
using Common.Providers;

#endregion

namespace MWA.MatchLoggerTester
{

    /// <summary> Common.Examples.SampleTrackable class implements the <see cref="Common.Examples.ISampleTrackable">Common.Examples.ISampleTrackable</see> interface</summary>
    ///<remarks> This is an ITrackable class which carries it's own <see cref="Common.Status">Common.Status</see> and Tracker Guid for transaction logging</remarks>
    public class SampleTrackable : ISampleTrackable
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Common.ITrackable">Common.ITrackable</see> class.</summary>
        public SampleTrackable() : this(Guid.NewGuid(), null) { }

        /// <summary>Initializes a new instance of the <see cref="Common.ITrackable">Common.ITrackable</see> class.</summary>
        public SampleTrackable(Guid tracker, Status status)
        {
            _extdata = new Dictionary<String, String>();
            _status = status ?? new Status();
            _tracker = tracker;
            
        }

        #endregion

        #region Properties

        #region ISampleTrackable Properties

        /// <summary>Returns true if the ITrackable.Status.StatusCode is less than 250 or greater than 999.</summary>
        public bool IsStatusSuccess { get { return (_status.StatusCode < 255 || _status.StatusCode > 999); } }

        /// <summary>Returns true if the ITrackable.Status.StatusCode is greater than 499 and less than 1000.</summary>
        public bool IsStatusError { get { return (_status.StatusCode < 1000 || _status.StatusCode > 499); } }

        /// <summary>A name/value collection that can be published using ISampleTrackable.PublishStatus</summary>
        public IDictionary<String, String> ExtData { get; set; }

        #endregion

        #endregion

        #region Methods

        #region ISampleTrackable Methods

        /// <summary>Initializes the <see cref="Common.ITrackable">Common.ITrackable</see> properties.</summary>
        public void Init(Guid tracker, Status status)
        {
            _status = status;
            _tracker = tracker;
        }

        /// <summary>A helper function that passes the  ISampleTrackable.Tracker and ISampleTrackable.ExtData to LogStatus.</summary>
        public void PublishStatus() { _status.ToAuditLog(_tracker, _extdata); }

        #region ITrackable Methods

        /// <summary>Provides a current reference to the object's internal or private Common.Status field</summary>
        /// <value>The status.</value>
        /// <returns>a Common Status</returns>
        public Status Status { get { return _status; } set { _status = value; } }

        /// <summary>Gets or sets a Unigue Instance Identifier for tracking the objects activities.</summary>
        /// <value>The tracker.</value>
        /// <returns>a Guid</returns>
        public Guid Tracker { get { return _tracker; } set { _tracker = value; } }

        #endregion

        #endregion

        #endregion

        #region Extensibility Points - Events, Partial Methods and Virtual Methods

        #endregion

        #region Helpers - Protected and Private

        #endregion

        #region Private

        private Status _status;
        private Guid _tracker;
        private Dictionary<String, String> _extdata;

        #endregion
    }


    /// <summary> Common.Examples.ISampleTrackable Interface implements the <see cref="Common.ITrackable">Common.ITrackable</see> interface</summary>
    ///<remarks> This is an ITrackable Interface which requires it's own <see cref="Common.Status">Common.Status</see> and Tracker Guid for transaction logging</remarks>
    public interface ISampleTrackable : ITrackable
    {
        #region ISampleTrackable Methods

        /// <summary>Initializes the <see cref="Common.ITrackable">Common.ITrackable</see> properties.</summary>
        void Init(Guid tracker, Status status);

        /// <summary>A helper function that passes the  ISampleTrackable.Tracker and ISampleTrackable.ExtData to LogStatus.</summary>
        void PublishStatus();

        #endregion

        #region ISampleTrackable Properties

        /// <summary>A name/value collection that can be published using ISampleTrackable.PublishStatus</summary>
        IDictionary<String, String> ExtData { get; set; }

        /// <summary>Returns true if the ITrackable.Status.StatusCode is less than 250 or greater than 999.</summary>
        bool IsStatusSuccess { get; }

        /// <summary>Returns true if the ITrackable.Status.StatusCode is greater than 499 and less than 1000.</summary>
        bool IsStatusError { get; }

        #endregion

    }
}