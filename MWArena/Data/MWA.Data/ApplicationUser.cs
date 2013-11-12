// ***********************************************************************
// Assembly         : MWA.Web
// Author           : Dev
// Created          : 11-09-2013
//
// Last Modified By : Dev
// Last Modified On : 11-09-2013
// ***********************************************************************
// <copyright file="ApplicationUser.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using System;
using Microsoft.AspNet.Identity.EntityFramework;

/// <summary>
/// The Models namespace.
/// </summary>
namespace MWA.Models
{



    /// <summary>
    /// Class ApplicationUser.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {

        /// <summary>
        /// Gets or sets the name of the faction.
        /// </summary>
        /// <value>The name of the faction.</value>
        public string FactionName { get; set; }


        /// <summary>
        /// Gets or sets the name of the regiment.
        /// </summary>
        /// <value>The name of the regiment.</value>
        public string RegimentName { get; set; }


        /// <summary>
        /// Gets or sets the name of the battalion.
        /// </summary>
        /// <value>The name of the battalion.</value>
        public string BattalionName { get; set; }


        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>The name of the company.</value>
        public string CompanyName { get; set; }


        /// <summary>
        /// Gets or sets the name of the lance.
        /// </summary>
        /// <value>The name of the lance.</value>
        public string LanceName { get; set; }


        /// <summary>
        /// Gets or sets the name of the rank.
        /// </summary>
        /// <value>The name of the rank.</value>
        public string RankName { get; set; }


        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>The image URL.</value>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the confirmation token.
        /// </summary>
        /// <value>The confirmation token.</value>
        public string ConfirmationToken { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is confirmed.
        /// </summary>
        /// <value><c>true</c> if this instance is confirmed; otherwise, <c>false</c>.</value>
        public bool IsConfirmed { get; set; }
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the user prefs.
        /// </summary>
        /// <value>The user prefs.</value>
        public virtual ICollection<UserPref> UserPrefs { get; set; }


    }
}
