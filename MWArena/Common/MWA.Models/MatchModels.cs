// ***********************************************************************
// Assembly         : MWA.Models
// Author           : Dev
// Created          : 11-01-2013
//
// Last Modified By : Dev
// Last Modified On : 11-04-2013
// ***********************************************************************
// <copyright file="MatchModels.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The Models namespace.
/// </summary>
namespace MWA.Models
{


    /// <summary>
    /// Class MwoAMatchMetric
    /// </summary>
    public class MwoAMatchMetric
    {
        /// <summary>
        /// Gets or sets the mwo a match metric identifier.
        /// </summary>
        /// <value>The mwo a match metric identifier.</value>
        public int MwoAMatchMetricId { get; set; }

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>The time.</value>
        public string time { get; set; }
        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>The level.</value>
        public string level { get; set; }
        /// <summary>
        /// Gets or sets the victory.
        /// </summary>
        /// <value>The victory.</value>
        public int victory { get; set; }
        /// <summary>
        /// Gets or sets the type of the victory.
        /// </summary>
        /// <value>The type of the victory.</value>
        public string victoryType { get; set; }
        /// <summary>
        /// Gets or sets the type of the match.
        /// </summary>
        /// <value>The type of the match.</value>
        public string matchType { get; set; }
        /// <summary>
        /// Gets or sets the team.
        /// </summary>
        /// <value>The team.</value>
        public string team { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string name { get; set; }
        /// <summary>
        /// Gets or sets the mech.
        /// </summary>
        /// <value>The mech.</value>
        public string mech { get; set; }
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public int status { get; set; }
        /// <summary>
        /// Gets or sets the matchscore.
        /// </summary>
        /// <value>The matchscore.</value>
        public int matchscore { get; set; }
        /// <summary>
        /// Gets or sets the kills.
        /// </summary>
        /// <value>The kills.</value>
        public int kills { get; set; }
        /// <summary>
        /// Gets or sets the assists.
        /// </summary>
        /// <value>The assists.</value>
        public int assists { get; set; }
        /// <summary>
        /// Gets or sets the damage.
        /// </summary>
        /// <value>The damage.</value>
        public int damage { get; set; }
        /// <summary>
        /// Gets or sets the ping.
        /// </summary>
        /// <value>The ping.</value>
        public int ping { get; set; }
        /// <summary>
        /// Gets or sets the lance.
        /// </summary>
        /// <value>The lance.</value>
        public int lance { get; set; }
        /// <summary>
        /// Gets or sets the name of the association.
        /// </summary>
        /// <value>The name of the association.</value>
        public string AssociationName { get; set; }
        /// <summary>
        /// Gets or sets the association identifier.
        /// </summary>
        /// <value>The association identifier.</value>
        public int AssociationId { get; set; }
        /// <summary>
        /// Gets or sets the publish flag.
        /// </summary>
        /// <value>The publish flag.</value>
        public int PublishFlag { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [publishing user name].
        /// </summary>
        /// <value><c>true</c> if [publishing user name]; otherwise, <c>false</c>.</value>
        public bool PublishingUserName { get; set; }
        /// <summary>
        /// Gets or sets the match hash.
        /// </summary>
        /// <value>The match hash.</value>
        public string MatchHash { get; set; }
        /// <summary>
        /// Gets or sets the match drop.
        /// </summary>
        /// <value>The match drop.</value>
        public MatchDrop MatchDrop { get; set; }
    }

    /// <summary>
    /// Class vwMatchMetric.
    /// </summary>
    public partial class vwMatchMetric
    {
        /// <summary>
        /// Gets or sets the vw match metric identifier.
        /// </summary>
        /// <value>The vw match metric identifier.</value>
        public Nullable<int> vwMatchMetricId { get; set; }
        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>The time.</value>
        public string time { get; set; }
        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>The level.</value>
        public string level { get; set; }
        /// <summary>
        /// Gets or sets the victory.
        /// </summary>
        /// <value>The victory.</value>
        public int victory { get; set; }
        /// <summary>
        /// Gets or sets the type of the victory.
        /// </summary>
        /// <value>The type of the victory.</value>
        public string victoryType { get; set; }
        /// <summary>
        /// Gets or sets the type of the match.
        /// </summary>
        /// <value>The type of the match.</value>
        public string matchType { get; set; }
        /// <summary>
        /// Gets or sets the team.
        /// </summary>
        /// <value>The team.</value>
        public string team { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string name { get; set; }
        /// <summary>
        /// Gets or sets the mech.
        /// </summary>
        /// <value>The mech.</value>
        public string mech { get; set; }
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public int status { get; set; }
        /// <summary>
        /// Gets or sets the matchscore.
        /// </summary>
        /// <value>The matchscore.</value>
        public int matchscore { get; set; }
        /// <summary>
        /// Gets or sets the kills.
        /// </summary>
        /// <value>The kills.</value>
        public int kills { get; set; }
        /// <summary>
        /// Gets or sets the assists.
        /// </summary>
        /// <value>The assists.</value>
        public int assists { get; set; }
        /// <summary>
        /// Gets or sets the damage.
        /// </summary>
        /// <value>The damage.</value>
        public int damage { get; set; }
        /// <summary>
        /// Gets or sets the ping.
        /// </summary>
        /// <value>The ping.</value>
        public int ping { get; set; }
        /// <summary>
        /// Gets or sets the lance.
        /// </summary>
        /// <value>The lance.</value>
        public int lance { get; set; }
        /// <summary>
        /// Gets or sets the name of the association.
        /// </summary>
        /// <value>The name of the association.</value>
        public string AssociationName { get; set; }
        /// <summary>
        /// Gets or sets the association identifier.
        /// </summary>
        /// <value>The association identifier.</value>
        public int AssociationId { get; set; }
        /// <summary>
        /// Gets or sets the publish flag.
        /// </summary>
        /// <value>The publish flag.</value>
        public int PublishFlag { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [publishing user name].
        /// </summary>
        /// <value><c>true</c> if [publishing user name]; otherwise, <c>false</c>.</value>
        public bool PublishingUserName { get; set; }
        /// <summary>
        /// Gets or sets the match hash.
        /// </summary>
        /// <value>The match hash.</value>
        public string MatchHash { get; set; }
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public int UserId { get; set; }
        /// <summary>
        /// Gets or sets the pilot image.
        /// </summary>
        /// <value>The pilot image.</value>
        public string PilotImage { get; set; }
        /// <summary>
        /// Gets or sets the name of the lance.
        /// </summary>
        /// <value>The name of the lance.</value>
        public string LanceName { get; set; }
        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>The name of the company.</value>
        public string CompanyName { get; set; }
        /// <summary>
        /// Gets or sets the name of the faction.
        /// </summary>
        /// <value>The name of the faction.</value>
        public string FactionName { get; set; }
        /// <summary>
        /// Gets or sets the name of the chassis.
        /// </summary>
        /// <value>The name of the chassis.</value>
        public string ChassisName { get; set; }
        /// <summary>
        /// Gets or sets the name of the variant.
        /// </summary>
        /// <value>The name of the variant.</value>
        public string VariantName { get; set; }
        /// <summary>
        /// Gets or sets the tonnage.
        /// </summary>
        /// <value>The tonnage.</value>
        public int Tonnage { get; set; }
        /// <summary>
        /// Gets or sets the weight class.
        /// </summary>
        /// <value>The weight class.</value>
        public string WeightClass { get; set; }
        /// <summary>
        /// Gets or sets the chassis identifier.
        /// </summary>
        /// <value>The chassis identifier.</value>
        public int ChassisId { get; set; }
        /// <summary>
        /// Gets or sets the name of the rank.
        /// </summary>
        /// <value>The name of the rank.</value>
        public string RankName { get; set; }

    }

    /// <summary>
    /// Class DropDeck.
    /// </summary>
    public class DropDeck
    {

        /// <summary>
        /// Gets or sets the drop deck identifier.
        /// </summary>
        /// <value>The drop deck identifier.</value>
        public int DropDeckId { get; set; }
        /// <summary>
        /// Gets or sets the mech names.
        /// </summary>
        /// <value>The mech names.</value>
        public List<string> MechNames { get; set; }

        /// <summary>
        /// Gets or sets the drop chassis.
        /// </summary>
        /// <value>The drop chassis.</value>
        public List<Chassis> DropChassis { get; set; }

        /// <summary>
        /// Gets or sets the tonnage.
        /// </summary>
        /// <value>The tonnage.</value>
        public int Tonnage { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is friendly.
        /// </summary>
        /// <value><c>true</c> if this instance is friendly; otherwise, <c>false</c>.</value>
        public bool IsFriendly { get; set; }
    }

    /// <summary>
    /// Class DropDeck12.
    /// </summary>
    public class DropDeck12
    {
        /// <summary>
        /// Gets or sets the drop deck12 identifier.
        /// </summary>
        /// <value>The drop deck12 identifier.</value>
        public int DropDeck12Id { get; set; }
        /// <summary>
        /// Gets or sets the mech count.
        /// </summary>
        /// <value>The mech count.</value>
        public int MechCount { get; set; }
        /// <summary>
        /// Gets or sets the tonnage.
        /// </summary>
        /// <value>The tonnage.</value>
        public int Tonnage { get; set; }

        /// <summary>
        /// Gets or sets the mech name1.
        /// </summary>
        /// <value>The mech name1.</value>
        public string MechName1 { get; set; }
        /// <summary>
        /// Gets or sets the mech name2.
        /// </summary>
        /// <value>The mech name2.</value>
        public string MechName2 { get; set; }
        /// <summary>
        /// Gets or sets the mech name3.
        /// </summary>
        /// <value>The mech name3.</value>
        public string MechName3 { get; set; }
        /// <summary>
        /// Gets or sets the mech name4.
        /// </summary>
        /// <value>The mech name4.</value>
        public string MechName4 { get; set; }
        /// <summary>
        /// Gets or sets the mech name5.
        /// </summary>
        /// <value>The mech name5.</value>
        public string MechName5 { get; set; }
        /// <summary>
        /// Gets or sets the mech name6.
        /// </summary>
        /// <value>The mech name6.</value>
        public string MechName6 { get; set; }
        /// <summary>
        /// Gets or sets the mech name7.
        /// </summary>
        /// <value>The mech name7.</value>
        public string MechName7 { get; set; }
        /// <summary>
        /// Gets or sets the mech name8.
        /// </summary>
        /// <value>The mech name8.</value>
        public string MechName8 { get; set; }
        /// <summary>
        /// Gets or sets the mech name9.
        /// </summary>
        /// <value>The mech name9.</value>
        public string MechName9 { get; set; }
        /// <summary>
        /// Gets or sets the mech name10.
        /// </summary>
        /// <value>The mech name10.</value>
        public string MechName10 { get; set; }
        /// <summary>
        /// Gets or sets the mech name11.
        /// </summary>
        /// <value>The mech name11.</value>
        public string MechName11 { get; set; }
        /// <summary>
        /// Gets or sets the mech name12.
        /// </summary>
        /// <value>The mech name12.</value>
        public string MechName12 { get; set; }

        /// <summary>
        /// Gets or sets the chassis id1.
        /// </summary>
        /// <value>The chassis id1.</value>
        public int? ChassisId1 { get; set; }
        /// <summary>
        /// Gets or sets the chassis id2.
        /// </summary>
        /// <value>The chassis id2.</value>
        public int? ChassisId2 { get; set; }
        /// <summary>
        /// Gets or sets the chassis id3.
        /// </summary>
        /// <value>The chassis id3.</value>
        public int? ChassisId3 { get; set; }
        /// <summary>
        /// Gets or sets the chassis id4.
        /// </summary>
        /// <value>The chassis id4.</value>
        public int? ChassisId4 { get; set; }
        /// <summary>
        /// Gets or sets the chassis id5.
        /// </summary>
        /// <value>The chassis id5.</value>
        public int? ChassisId5 { get; set; }
        /// <summary>
        /// Gets or sets the chassis id6.
        /// </summary>
        /// <value>The chassis id6.</value>
        public int? ChassisId6 { get; set; }
        /// <summary>
        /// Gets or sets the chassis id7.
        /// </summary>
        /// <value>The chassis id7.</value>
        public int? ChassisId7 { get; set; }
        /// <summary>
        /// Gets or sets the chassis id8.
        /// </summary>
        /// <value>The chassis id8.</value>
        public int? ChassisId8 { get; set; }
        /// <summary>
        /// Gets or sets the chassis id9.
        /// </summary>
        /// <value>The chassis id9.</value>
        public int? ChassisId9 { get; set; }
        /// <summary>
        /// Gets or sets the chassis id10.
        /// </summary>
        /// <value>The chassis id10.</value>
        public int? ChassisId10 { get; set; }
        /// <summary>
        /// Gets or sets the chassis id11.
        /// </summary>
        /// <value>The chassis id11.</value>
        public int? ChassisId11 { get; set; }
        /// <summary>
        /// Gets or sets the chassis id12.
        /// </summary>
        /// <value>The chassis id12.</value>
        public int? ChassisId12 { get; set; }

        /// <summary>
        /// Gets or sets the chassis1.
        /// </summary>
        /// <value>The chassis1.</value>
        public Chassis Chassis1 { get; set; }
        /// <summary>
        /// Gets or sets the chassis2.
        /// </summary>
        /// <value>The chassis2.</value>
        public Chassis Chassis2 { get; set; }
        /// <summary>
        /// Gets or sets the chassis3.
        /// </summary>
        /// <value>The chassis3.</value>
        public Chassis Chassis3 { get; set; }
        /// <summary>
        /// Gets or sets the chassis4.
        /// </summary>
        /// <value>The chassis4.</value>
        public Chassis Chassis4 { get; set; }
        /// <summary>
        /// Gets or sets the chassis5.
        /// </summary>
        /// <value>The chassis5.</value>
        public Chassis Chassis5 { get; set; }
        /// <summary>
        /// Gets or sets the chassis6.
        /// </summary>
        /// <value>The chassis6.</value>
        public Chassis Chassis6 { get; set; }
        /// <summary>
        /// Gets or sets the chassis7.
        /// </summary>
        /// <value>The chassis7.</value>
        public Chassis Chassis7 { get; set; }
        /// <summary>
        /// Gets or sets the chassis8.
        /// </summary>
        /// <value>The chassis8.</value>
        public Chassis Chassis8 { get; set; }
        /// <summary>
        /// Gets or sets the chassis9.
        /// </summary>
        /// <value>The chassis9.</value>
        public Chassis Chassis9 { get; set; }
        /// <summary>
        /// Gets or sets the chassis10.
        /// </summary>
        /// <value>The chassis10.</value>
        public Chassis Chassis10 { get; set; }
        /// <summary>
        /// Gets or sets the chassis11.
        /// </summary>
        /// <value>The chassis11.</value>
        public Chassis Chassis11 { get; set; }
        /// <summary>
        /// Gets or sets the chassis12.
        /// </summary>
        /// <value>The chassis12.</value>
        public Chassis Chassis12 { get; set; }
    }

    /// <summary>
    /// Class MatchDrop.
    /// </summary>
    public class MatchDrop
    {
        /// <summary>
        /// Gets or sets the match drop identifier.
        /// </summary>
        /// <value>The match drop identifier.</value>
        public int MatchDropId { get; set; }
        /// <summary>
        /// Gets or sets the match hash.
        /// </summary>
        /// <value>The match hash.</value>
        public string MatchHash { get; set; }
        /// <summary>
        /// Gets or sets the name of the association.
        /// </summary>
        /// <value>The name of the association.</value>
        public string AssociationName { get; set; }
        /// <summary>
        /// Gets or sets the association identifier.
        /// </summary>
        /// <value>The association identifier.</value>
        public int AssociationId { get; set; }

        /// <summary>
        /// Gets or sets the friendly drop deck12 identifier.
        /// </summary>
        /// <value>The friendly drop deck12 identifier.</value>
        public int FriendlyDropDeck12Id { get; set; }

        /// <summary>
        /// Gets or sets the enemy drop deck12 identifier.
        /// </summary>
        /// <value>The enemy drop deck12 identifier.</value>
        public int EnemyDropDeck12Id { get; set; }

        /// <summary>
        /// Gets or sets the friendly drop deck12.
        /// </summary>
        /// <value>The friendly drop deck12.</value>
        public DropDeck12 FriendlyDropDeck12 { get; set; }
        /// <summary>
        /// Gets or sets the enemy drop deck12.
        /// </summary>
        /// <value>The enemy drop deck12.</value>
        public DropDeck12 EnemyDropDeck12 { get; set; }
        /// <summary>
        /// Gets or sets the association.
        /// </summary>
        /// <value>The association.</value>
        public Association Association { get; set; }

        public Map Map { get; set; }
    }

    /// <summary>
    /// Class MWMatch.
    /// </summary>
    public class MWMatch
    {
        /// <summary>
        /// Gets or sets the match drop identifier.
        /// </summary>
        /// <value>The match drop identifier.</value>
        public int MatchDropId { get; set; }
        /// <summary>
        /// Gets or sets the match hash.
        /// </summary>
        /// <value>The match hash.</value>
        public string MatchHash { get; set; }
        /// <summary>
        /// Gets or sets the name of the association.
        /// </summary>
        /// <value>The name of the association.</value>
        public string AssociationName { get; set; }
        /// <summary>
        /// Gets or sets the association identifier.
        /// </summary>
        /// <value>The association identifier.</value>
        public int AssociationId { get; set; }

        /// <summary>
        /// Gets or sets the drop decks.
        /// </summary>
        /// <value>The drop decks.</value>
        public List<DropDeck> DropDecks { get; set; }

        /// <summary>
        /// Gets or sets the association.
        /// </summary>
        /// <value>The association.</value>
        public Association Association { get; set; }
    }

    /// <summary>
    /// Class Association.
    /// </summary>
    public class Association
    {
        /// <summary>
        /// Gets or sets the association identifier.
        /// </summary>
        /// <value>The association identifier.</value>
        public int AssociationId { get; set; }
        /// <summary>
        /// Gets or sets the name of the association.
        /// </summary>
        /// <value>The name of the association.</value>
        public string AssociationName { get; set; }
    }

    /// <summary>
    /// Class Chassis
    /// </summary>
    public class Chassis
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>

        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the name of the chassis.
        /// </summary>
        /// <value>The name of the chassis.</value>
        public string ChassisName { get; set; }
        /// <summary>
        /// Gets or sets the name of the variant.
        /// </summary>
        /// <value>The name of the variant.</value>
        public string VariantName { get; set; }
        /// <summary>
        /// Gets or sets the tonnage.
        /// </summary>
        /// <value>The tonnage.</value>
        public int Tonnage { get; set; }
        /// <summary>
        /// Gets or sets the weight class.
        /// </summary>
        /// <value>The weight class.</value>
        public string WeightClass { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is hero.
        /// </summary>
        /// <value><c>true</c> if this instance is hero; otherwise, <c>false</c>.</value>
        public bool IsHero { get; set; }
        /// <summary>
        /// Gets or sets the def image URL.
        /// </summary>
        /// <value>The def image URL.</value>
        public string DefImageUrl { get; set; }
        /// <summary>
        /// Gets or sets the altername name of the chassis.
        /// </summary>
        /// <value>The alternate name of the chassis.</value>
        public string AltName { get; set; }
        /// <summary>
        /// Gets or sets the altername name of the chassis.
        /// </summary>
        /// <value>The alternate name of the chassis.</value>
        public string AltName2 { get; set; }

        /// <summary>
        /// Gets or sets the name of the base variant.
        /// </summary>
        /// <value>The name of the base variant.</value>
        public string BaseVariantName { get; set; }
        /// <summary>
        /// Gets or sets the base variant identifier.
        /// </summary>
        /// <value>The base variant identifier.</value>
        public int BaseVariantId { get; set; }

    }

    public class Map
    {
        public int MapId { get; set; }
        public string MapName { get; set; }
        public string MapIconUrl { get; set; }
        public string MapImageUrl { get; set; }
        public string MapAltName1 { get; set; }
        public string MapAltName2 { get; set; }

    }

    public class ApiConnTest
    {
        public string UserName { get; set; }
    }

}



