using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GW2Stuff;

namespace GW2Stuff
{
	public static class OverlaySettings
	{
		public static event EventHandler settingsUpdated = delegate {};

		// Active events
		public static FontSettings activeEventTitleFont;
		public static FontSettings activeEventStatusFont;
		public static FontSettings activeEventTimerFont;
		public static String activeEventBackgroundImage;
		public static double activeEventBackgroundOpacity;
		public static Thickness activeEventMargins;

		// Window events
		public static FontSettings windowEventTitleFont;
		public static FontSettings windowEventStatusFont;
		public static FontSettings windowEventTimerFont;
		public static String windowEventBackgroundImage;
		public static double windowEventBackgroundOpacity;
		public static Thickness windowEventMargins;

		// Inactive events
		public static FontSettings inactiveEventTitleFont;
		public static FontSettings inactiveEventStatusFont;
		public static FontSettings inactiveEventTimerFont;
		public static String inactiveEventBackgroundImage;
		public static double inactiveEventBackgroundOpacity;
		public static Thickness inactiveEventMargins;

		// Misc
		public static bool clickThroughInGame;
		public static String dataServer;
		public static int defaultWorldId;
		public static bool hideTitleInGame;
		public static String language;

		public static void loadSettings(bool defaults = false)
		{
			/* Active events */

			if ((GW2Stuff.Properties.Settings.Default.activeEventTitleFont == null) || defaults)
			{
				FontSettings defaultFont = new FontSettings();
				defaultFont.fontFace = "Segoe UI";
				defaultFont.fontSize = 14;
				defaultFont.fontBold = true;
				defaultFont.fontColour = 0x000000ff;
				defaultFont.shadowColour = 0x00000000;
				activeEventTitleFont = defaultFont;
			}
			else
			{
				activeEventTitleFont = Properties.Settings.Default.activeEventTitleFont.clone();
			}
			
			if ((Properties.Settings.Default.activeEventStatusFont == null) || defaults)
			{
				FontSettings defaultFont = new FontSettings();
				defaultFont.fontFace = "Segoe UI";
				defaultFont.fontSize = 12;
				defaultFont.fontBold = true;
				defaultFont.fontColour = 0x000000ff;
				defaultFont.shadowColour = 0x00000000;
				activeEventStatusFont = defaultFont;
			}
			else
			{
				activeEventStatusFont = Properties.Settings.Default.activeEventStatusFont.clone();
			}
			
			if ((Properties.Settings.Default.activeEventTimerFont == null) || defaults)
			{
				FontSettings defaultFont = new FontSettings();
				defaultFont.fontFace = "Segoe UI";
				defaultFont.fontSize = 12;
				defaultFont.fontBold = true;
				defaultFont.fontColour = 0x000000ff;
				defaultFont.shadowColour = 0x00000000;
				activeEventTimerFont = defaultFont;
			}
			else
			{
				activeEventTimerFont = Properties.Settings.Default.activeEventTimerFont.clone();
			}

			if (defaults)
			{
				activeEventBackgroundImage = "active";
				activeEventBackgroundOpacity = 1.0;
				activeEventMargins = new Thickness(10, 5, 10, 20);
			}
			else
			{
				activeEventBackgroundImage = Properties.Settings.Default.activeEventBackgroundImage;
				activeEventBackgroundOpacity = Properties.Settings.Default.activeEventBackgroundOpacity;
				activeEventMargins = Properties.Settings.Default.activeEventMargins;
			}

			/* Window events */

			if ((Properties.Settings.Default.windowEventTitleFont == null) || defaults)
			{
				FontSettings defaultFont = new FontSettings();
				defaultFont.fontFace = "Segoe UI";
				defaultFont.fontSize = 14;
				defaultFont.fontBold = true;
				defaultFont.fontColour = 0xffffffff;
				defaultFont.shadowColour = 0x000000ff;
				windowEventTitleFont = defaultFont;
			}
			else
			{
				windowEventTitleFont = Properties.Settings.Default.windowEventTitleFont.clone();
			}
			
			if ((Properties.Settings.Default.windowEventStatusFont == null) || defaults)
			{
				FontSettings defaultFont = new FontSettings();
				defaultFont.fontFace = "Segoe UI";
				defaultFont.fontSize = 12;
				defaultFont.fontBold = true;
				defaultFont.fontColour = 0xffffffff;
				defaultFont.shadowColour = 0x000000ff;
				windowEventStatusFont = defaultFont;
			}
			else
			{
				windowEventStatusFont = Properties.Settings.Default.windowEventStatusFont.clone();
			}
			
			if ((Properties.Settings.Default.windowEventTimerFont == null) || defaults)
			{
				FontSettings defaultFont = new FontSettings();
				defaultFont.fontFace = "Segoe UI";
				defaultFont.fontSize = 12;
				defaultFont.fontBold = true;
				defaultFont.fontColour = 0xffff00ff;
				defaultFont.shadowColour = 0x000000ff;
				windowEventTimerFont = defaultFont;
			}
			else
			{
				windowEventTimerFont = Properties.Settings.Default.windowEventTimerFont.clone();
			}

			if (defaults)
			{
				windowEventBackgroundImage = "inactive";
				windowEventBackgroundOpacity = 1.0;
				windowEventMargins = new Thickness(10, 5, 10, 20);
			}
			else
			{
				windowEventBackgroundImage = Properties.Settings.Default.windowEventBackgroundImage;
				windowEventBackgroundOpacity = Properties.Settings.Default.windowEventBackgroundOpacity;
				windowEventMargins = Properties.Settings.Default.windowEventMargins;
			}

			/* Inactive events */

			if ((Properties.Settings.Default.inactiveEventTitleFont == null) || defaults)
			{
				FontSettings defaultFont = new FontSettings();
				defaultFont.fontFace = "Segoe UI";
				defaultFont.fontSize = 14;
				defaultFont.fontBold = true;
				defaultFont.fontColour = 0xffffffff;
				defaultFont.shadowColour = 0x000000ff;
				inactiveEventTitleFont = defaultFont;
			}
			else
			{
				inactiveEventTitleFont = Properties.Settings.Default.inactiveEventTitleFont.clone();
			}
			
			if ((Properties.Settings.Default.inactiveEventStatusFont == null) || defaults)
			{
				FontSettings defaultFont = new FontSettings();
				defaultFont.fontFace = "Segoe UI";
				defaultFont.fontSize = 12;
				defaultFont.fontBold = true;
				defaultFont.fontColour = 0xffffffff;
				defaultFont.shadowColour = 0x000000ff;
				inactiveEventStatusFont = defaultFont;
			}
			else
			{
				inactiveEventStatusFont = Properties.Settings.Default.inactiveEventStatusFont.clone();
			}
			
			if ((Properties.Settings.Default.inactiveEventTimerFont == null) || defaults)
			{
				FontSettings defaultFont = new FontSettings();
				defaultFont.fontFace = "Segoe UI";
				defaultFont.fontSize = 12;
				defaultFont.fontBold = true;
				defaultFont.fontColour = 0xffffffff;
				defaultFont.shadowColour = 0x000000ff;
				inactiveEventTimerFont = defaultFont;
			}
			else
			{
				inactiveEventTimerFont = Properties.Settings.Default.inactiveEventTimerFont.clone();
			}

			if (defaults)
			{
				inactiveEventBackgroundImage = "inactive";
				inactiveEventBackgroundOpacity = 0.6;
				inactiveEventMargins = new Thickness(10, 5, 10, 20);
			}
			else
			{
				inactiveEventBackgroundImage = Properties.Settings.Default.inactiveEventBackgroundImage;
				inactiveEventBackgroundOpacity = Properties.Settings.Default.inactiveEventBackgroundOpacity;
				inactiveEventMargins = Properties.Settings.Default.inactiveEventMargins;
			}

			/* Misc */

			if (defaults)
			{
				clickThroughInGame = true;
				dataServer = "jasper.gw2stuff.com";
				defaultWorldId = 0;
				hideTitleInGame = false;
				language = "en";
			}
			else
			{
				clickThroughInGame = Properties.Settings.Default.clickThroughInGame;
				dataServer = "jasper.gw2stuff.com";
				//dataServer = Properties.Settings.Default.dataServer;
				defaultWorldId = Properties.Settings.Default.defaultWorldId;
				hideTitleInGame = Properties.Settings.Default.hideTitleInGame;
				language = Properties.Settings.Default.language;
			}
		}

		public static void saveSettings()
		{
			Properties.Settings.Default.activeEventTitleFont = activeEventTitleFont;
			Properties.Settings.Default.activeEventStatusFont = activeEventStatusFont;
			Properties.Settings.Default.activeEventTimerFont = activeEventTimerFont;
			Properties.Settings.Default.activeEventBackgroundImage = activeEventBackgroundImage;
			Properties.Settings.Default.activeEventBackgroundOpacity = activeEventBackgroundOpacity;
			Properties.Settings.Default.activeEventMargins = activeEventMargins;
			
			Properties.Settings.Default.windowEventTitleFont = windowEventTitleFont;
			Properties.Settings.Default.windowEventStatusFont = windowEventStatusFont;
			Properties.Settings.Default.windowEventTimerFont = windowEventTimerFont;
			Properties.Settings.Default.windowEventBackgroundImage = windowEventBackgroundImage;
			Properties.Settings.Default.windowEventBackgroundOpacity = windowEventBackgroundOpacity;
			Properties.Settings.Default.windowEventMargins = windowEventMargins;
			
			Properties.Settings.Default.inactiveEventTitleFont = inactiveEventTitleFont;
			Properties.Settings.Default.inactiveEventStatusFont = inactiveEventStatusFont;
			Properties.Settings.Default.inactiveEventTimerFont = inactiveEventTimerFont;
			Properties.Settings.Default.inactiveEventBackgroundImage = inactiveEventBackgroundImage;
			Properties.Settings.Default.inactiveEventBackgroundOpacity = inactiveEventBackgroundOpacity;
			Properties.Settings.Default.inactiveEventMargins = inactiveEventMargins;

			Properties.Settings.Default.clickThroughInGame = clickThroughInGame;
			//Properties.Settings.Default.dataServer = dataServer;
			Properties.Settings.Default.defaultWorldId = defaultWorldId;
			Properties.Settings.Default.hideTitleInGame = hideTitleInGame;
			Properties.Settings.Default.language = language;

			Properties.Settings.Default.Save();
		}

		public static void raiseUpdate()
		{
			settingsUpdated(null, EventArgs.Empty);
		}
	}
}
