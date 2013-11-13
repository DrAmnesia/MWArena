using System.Collections.Generic;
using MWA.Models;

namespace MWA.Web.Models
{
    public interface IMechWarriorInfo
    {
        /// <summary>
        /// Gets or sets the name of the faction.
        /// </summary>
        /// <value>The name of the faction.</value>
        string FactionName { get; set; }

        /// <summary>
        /// Gets or sets the name of the regiment.
        /// </summary>
        /// <value>The name of the regiment.</value>
        string RegimentName { get; set; }

        /// <summary>
        /// Gets or sets the name of the battalion.
        /// </summary>
        /// <value>The name of the battalion.</value>
        string BattalionName { get; set; }

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>The name of the company.</value>
        string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the name of the lance.
        /// </summary>
        /// <value>The name of the lance.</value>
        string LanceName { get; set; }

        /// <summary>
        /// Gets or sets the name of the rank.
        /// </summary>
        /// <value>The name of the rank.</value>
        string RankName { get; set; }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>The image URL.</value>
        string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the confirmation token.
        /// </summary>
        /// <value>The confirmation token.</value>
        string ConfirmationToken { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is confirmed.
        /// </summary>
        /// <value><c>true</c> if this instance is confirmed; otherwise, <c>false</c>.</value>
        bool IsConfirmed { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        string Email { get; set; }

        List<UserPref> UserPrefs { get; set; }
    }
}