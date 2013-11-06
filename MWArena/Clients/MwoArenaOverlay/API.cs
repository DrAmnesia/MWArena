using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GW2Stuff
{
	public static class GW2StuffAPI
	{
		private static int apiTimeOffset = 0;
		private static DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		
		public static DateTime unixToDate(int timestamp)
		{
			return unixEpoch.AddSeconds(timestamp);
		}

		public static int dateToUnix(DateTime dateTime)
		{
			return (int)((dateTime.ToUniversalTime() - unixEpoch).TotalSeconds);
		}

		public static void setCurrentTime(int remoteTimestamp)
		{
			apiTimeOffset = remoteTimestamp - dateToUnix(DateTime.UtcNow);
		}

		public static int getCurrentTime()
		{
			return dateToUnix(DateTime.UtcNow) + apiTimeOffset;
		}
	}

	[DataContract]
	public class EventsRequestResponse
	{
		[DataMember] public List<EventInformation> events;
		[DataMember] public int currentTime;
		[DataMember] public int dayStart;
	}

	[DataContract]
	public class EventInformation
	{
		[DataMember] public string id;
		[DataMember] public string name;
		[DataMember] public string eventType;
		[DataMember] public int timestamp;
		[DataMember] public int stateIndex;
		[DataMember] public string stateType;
		[DataMember] public string stateName;
		[DataMember] public string mapName;
		[DataMember] public string windowType;
		[DataMember] public int minWindow;
		[DataMember] public int maxWindow;
	}

	[DataContract]
	public class WorldInformation: IComparable<WorldInformation>
	{
		[DataMember] public int worldId;
		[DataMember] public String region;
		[DataMember] public String language;
		[DataMember] public String slug;
		[DataMember] public String name;

		public int CompareTo(WorldInformation other)
		{
			int r = region.CompareTo(other.region);
			return (r == 0) ? name.CompareTo(other.name) : r;
		}

		public override String ToString()
		{
			return region.ToUpper() + " - " + name;
		}
	}

	[DataContract]
	public class StartupInformation
	{
		[DataMember] public int minVersion;
		[DataMember] public List<WorldInformation> worlds;
	}
}
