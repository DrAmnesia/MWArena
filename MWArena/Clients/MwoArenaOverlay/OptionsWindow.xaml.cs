using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
 

namespace GW2Stuff
{
	/// <summary>
	/// Interaction logic for OptionsWindow.xaml
	/// </summary>
	public partial class OptionsWindow : Window
	{
		public OptionsWindow()
		{
			InitializeComponent();
		}

		public void refresh()
		{
			activeEventTitleFont.fontSettings = OverlaySettings.activeEventTitleFont;
			activeEventStatusFont.fontSettings = OverlaySettings.activeEventStatusFont;
			activeEventTimerFont.fontSettings = OverlaySettings.activeEventTimerFont;
			activeEventBackgroundImage.SelectedItem = OverlaySettings.activeEventBackgroundImage;
			activeEventBackgroundOpacity.Value = OverlaySettings.activeEventBackgroundOpacity;
			activeEventMargins.margins = OverlaySettings.activeEventMargins;
			
			windowEventTitleFont.fontSettings = OverlaySettings.windowEventTitleFont;
			windowEventStatusFont.fontSettings = OverlaySettings.windowEventStatusFont;
			windowEventTimerFont.fontSettings = OverlaySettings.windowEventTimerFont;
			windowEventBackgroundImage.SelectedItem = OverlaySettings.windowEventBackgroundImage;
			windowEventBackgroundOpacity.Value = OverlaySettings.windowEventBackgroundOpacity;
			windowEventMargins.margins = OverlaySettings.windowEventMargins;
			
			inactiveEventTitleFont.fontSettings = OverlaySettings.inactiveEventTitleFont;
			inactiveEventStatusFont.fontSettings = OverlaySettings.inactiveEventStatusFont;
			inactiveEventTimerFont.fontSettings = OverlaySettings.inactiveEventTimerFont;
			inactiveEventBackgroundImage.SelectedItem = OverlaySettings.inactiveEventBackgroundImage;
			inactiveEventBackgroundOpacity.Value = OverlaySettings.inactiveEventBackgroundOpacity;
			inactiveEventMargins.margins = OverlaySettings.inactiveEventMargins;

			clickThroughInGame.IsChecked = OverlaySettings.clickThroughInGame;
			dataServer.SelectedItem = OverlaySettings.dataServer;
			defaultWorld.SelectedItem = null;
			hideTitleInGame.IsChecked = OverlaySettings.hideTitleInGame;

			foreach (WorldInformation world in defaultWorld.Items)
			{
				if (world.worldId == OverlaySettings.defaultWorldId)
				{
					defaultWorld.SelectedItem = world;
					break;
				}
			}
		}

		private bool deploy()
		{
			/* Error checking */

			// Active
			FontSettings tempActiveEventTitleFont = activeEventTitleFont.fontSettings;
			if (tempActiveEventTitleFont == null)
			{
				MessageBox.Show("Invalid font setup - Active event title");
				return false;
			}
			FontSettings tempActiveEventStatusFont = activeEventStatusFont.fontSettings;
			if (tempActiveEventStatusFont == null)
			{
				MessageBox.Show("Invalid font setup - Active event status");
				return false;
			}
			FontSettings tempActiveEventTimerFont = activeEventTimerFont.fontSettings;
			if (tempActiveEventTimerFont == null)
			{
				MessageBox.Show("Invalid font setup - Active event timer");
				return false;
			}
			
			// Window
			FontSettings tempWindowEventTitleFont = windowEventTitleFont.fontSettings;
			if (tempWindowEventTitleFont == null)
			{
				MessageBox.Show("Invalid font setup - In-Window event title");
				return false;
			}
			FontSettings tempWindowEventStatusFont = windowEventStatusFont.fontSettings;
			if (tempWindowEventStatusFont == null)
			{
				MessageBox.Show("Invalid font setup - In-Window event status");
				return false;
			}
			FontSettings tempWindowEventTimerFont = windowEventTimerFont.fontSettings;
			if (tempWindowEventTimerFont == null)
			{
				MessageBox.Show("Invalid font setup - In-Window event timer");
				return false;
			}
			
			// Inactive
			FontSettings tempInactiveEventTitleFont = inactiveEventTitleFont.fontSettings;
			if (tempInactiveEventTitleFont == null)
			{
				MessageBox.Show("Invalid font setup - Inactive event title");
				return false;
			}
			FontSettings tempInactiveEventStatusFont = inactiveEventStatusFont.fontSettings;
			if (tempInactiveEventStatusFont == null)
			{
				MessageBox.Show("Invalid font setup - Inactive event status");
				return false;
			}
			FontSettings tempInactiveEventTimerFont = inactiveEventTimerFont.fontSettings;
			if (tempInactiveEventTimerFont == null)
			{
				MessageBox.Show("Invalid font setup - Inactive event timer");
				return false;
			}

			/* Deploy new settings */

			String tempActiveEventBackgroundImage = (String)activeEventBackgroundImage.SelectedItem;
			OverlaySettings.activeEventTitleFont = tempActiveEventTitleFont;
			OverlaySettings.activeEventStatusFont = tempActiveEventStatusFont;
			OverlaySettings.activeEventTimerFont = tempActiveEventTimerFont;
			OverlaySettings.activeEventBackgroundImage = (String)activeEventBackgroundImage.SelectedItem;
			OverlaySettings.activeEventBackgroundOpacity = activeEventBackgroundOpacity.Value;
			OverlaySettings.activeEventMargins = activeEventMargins.margins;

			String tempWindowEventBackgroundImage = (String)windowEventBackgroundImage.SelectedItem;
			OverlaySettings.windowEventTitleFont = tempWindowEventTitleFont;
			OverlaySettings.windowEventStatusFont = tempWindowEventStatusFont;
			OverlaySettings.windowEventTimerFont = tempWindowEventTimerFont;
			OverlaySettings.windowEventBackgroundImage = (String)windowEventBackgroundImage.SelectedItem;
			OverlaySettings.windowEventBackgroundOpacity = windowEventBackgroundOpacity.Value;
			OverlaySettings.windowEventMargins = windowEventMargins.margins;

			String tempIninactiveEventBackgroundImage = (String)inactiveEventBackgroundImage.SelectedItem;
			OverlaySettings.inactiveEventTitleFont = tempInactiveEventTitleFont;
			OverlaySettings.inactiveEventStatusFont = tempInactiveEventStatusFont;
			OverlaySettings.inactiveEventTimerFont = tempInactiveEventTimerFont;
			OverlaySettings.inactiveEventBackgroundImage = (String)inactiveEventBackgroundImage.SelectedItem;
			OverlaySettings.inactiveEventBackgroundOpacity = inactiveEventBackgroundOpacity.Value;
			OverlaySettings.inactiveEventMargins = inactiveEventMargins.margins;

			OverlaySettings.clickThroughInGame = clickThroughInGame.IsChecked.Value;
			OverlaySettings.dataServer = (String)dataServer.SelectedItem;
			OverlaySettings.defaultWorldId = (defaultWorld.SelectedItem == null) ? 0 : ((WorldInformation)defaultWorld.SelectedItem).worldId;
			OverlaySettings.hideTitleInGame = hideTitleInGame.IsChecked.Value;

			OverlaySettings.raiseUpdate();
			return true;
		}

		private void applyButton_Click(object sender, RoutedEventArgs e)
		{
			deploy();
		}

		private void saveButton_Click(object sender, RoutedEventArgs e)
		{
			if (deploy())
			{
				OverlaySettings.saveSettings();
				Hide();
			}
		}

		private void cancelButton_Click(object sender, RoutedEventArgs e)
		{
			OverlaySettings.loadSettings(false);
			refresh();
			deploy();
			Hide();
		}

		private void defaultsButton_Click(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show("Are you sure you want to reset to defaults?", "GW2Stuff Overlay", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
			{
				OverlaySettings.loadSettings(true);
				refresh();
				deploy();
			}
		}

		private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
			if (deploy())
			{
				OverlaySettings.saveSettings();
				Hide();
			}
		}

		private void clearDefaultWorld_Click(object sender, RoutedEventArgs e)
		{
			defaultWorld.SelectedItem = null;
		}
	}
}
