using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
 

namespace GW2Stuff
{
	public class EventItemComparer: IComparer
	{
		private int currentTimestamp;

		public void updateCurrentTimestamp()
		{
			currentTimestamp = GW2StuffAPI.getCurrentTime();
		}

		// Recent events come first, so that they can be clicked easier
		private int recentCompare(EventInformation a, EventInformation b)
		{
			bool aRecent = (a.stateIndex == 0) && (a.timestamp > (currentTimestamp - 60));
			bool bRecent = (b.stateIndex == 0) && (b.timestamp > (currentTimestamp - 60));

			return (aRecent == bRecent) ? 0 : (aRecent ? -1 : 1);
		}

		// Active events at the top
		private int stateCompare(EventInformation a, EventInformation b)
		{
			if ((a.stateIndex == 0) == (b.stateIndex == 0))
			{
				return 0;
			}
			else
			{
				return (a.stateIndex == 0) ? 1 : -1;
			}
		}

		// Those with countdowns come before those without
		private int countdownCompare(EventInformation a, EventInformation b)
		{
			if ((a.windowType == "countdown") && (b.windowType == "countdown"))
			{
				int aMin = a.timestamp + a.minWindow;
				int bMin = b.timestamp + b.minWindow;
				return (aMin == bMin) ? 0 : ((aMin > bMin) ? 1 : -1);
			}
			else if (a.windowType == "countdown")
			{
				return -1;
			}
			else if (b.windowType == "countdown")
			{
				return 1;
			}
			else
			{
				return 0;
			}
		}

		// Alphabetical as the last resort
		private int nameCompare(EventInformation a, EventInformation b)
		{
			return a.name.CompareTo(b.name);
		}

		int IComparer.Compare(Object a, Object b)
		{
			EventInformation ei1 = ((EventItem)a).eventInfo;
			EventInformation ei2 = ((EventItem)b).eventInfo;

			int c = recentCompare(ei1, ei2);
			if (c != 0) return c;
			c = stateCompare(ei1, ei2);
			if (c != 0) return c;
			c = countdownCompare(ei1, ei2);
			if (c != 0) return c;
			return nameCompare(ei1, ei2);
		}
	}

	/// <summary>
	/// Interaction logic for EventItem.xaml
	/// </summary>
	public partial class EventItem: UserControl
	{
		private bool activeState = false;
		private string _displayMode = null;
		private bool _completed = false;
		private EventInformation _eventInfo;
		private bool hooked = false;

		public bool completed
		{
			get
			{
				return _completed;
			}
			set
			{
				_completed = value;
				eventCompleted.Visibility = _completed ? Visibility.Visible : Visibility.Hidden;
			}
		}

		public String displayMode
		{
			get
			{
				return _displayMode;
			}
		}

		public EventInformation eventInfo
		{
			set
			{
				_eventInfo = value;
				eventTitle.Text = value.name;
				eventStatus.Text = value.stateName;

				switch (value.stateType)
				{
				case "boss":
				case "pre":
					activeState = true;
					break;
				default:
					activeState = false;
					break;
				}

				tick();
			}
			get
			{
				return _eventInfo;
			}
		}

		public EventItem()
		{
			InitializeComponent();
		}

		private String diffToTimeDisplay(int diff)
		{
			bool negative = (diff < 0);
			if (negative) diff = -diff;
			int seconds = diff % 60;
			diff = (diff - seconds) / 60;
			int minutes = diff % 60;
			diff = (diff - minutes) / 60;
			int hours = diff;

			String timeDisplay = String.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
			return negative ? ("-" + timeDisplay) : timeDisplay;
		}

		public void tick()
		{
			int currentTimestamp = GW2StuffAPI.getCurrentTime();

			String newDisplayMode = activeState ? "active" : "inactive";

			switch(_eventInfo.windowType)
			{
			case "countdown":
				{
					int min = _eventInfo.timestamp + _eventInfo.minWindow;
					int max = _eventInfo.timestamp + _eventInfo.maxWindow;
					if (max < min) max = min;
					int diff;

					if (currentTimestamp > min)
					{
						diff = max - currentTimestamp;
						if (!activeState) newDisplayMode = "window";
					}
					else
					{
						diff = min - currentTimestamp;
					}

					eventTimer.Text = diffToTimeDisplay(diff);
				}
				break;
			case "since":
				{
					int diff = currentTimestamp - _eventInfo.timestamp;
					eventTimer.Text = diffToTimeDisplay(diff);
				}
				break;
			default:
				eventTimer.Text = "";
				break;
			}

			if (_displayMode != newDisplayMode)
			{
				_displayMode = newDisplayMode;
				updateDisplayState();
			}
		}

		private Color convertColour(UInt32 value)
		{
			UInt32 r = (value & 0xff000000) >> 24;
			UInt32 g = (value & 0x00ff0000) >> 16;
			UInt32 b = (value & 0x0000ff00) >> 8;
			UInt32 a = (value & 0x000000ff);
			return Color.FromArgb((byte)a, (byte)r, (byte)g, (byte)b);
		}

		private void updateText(TextBlock text, DropShadowEffect shadow, FontSettings font)
		{
			text.FontFamily = new FontFamily(font.fontFace);
			text.FontSize = (double)font.fontSize;
			text.FontWeight = font.fontBold ? FontWeights.Bold : FontWeights.Normal;
			text.FontStyle = font.fontItalic ? FontStyles.Italic : FontStyles.Normal;
			text.TextDecorations = font.fontUnderline ? TextDecorations.Underline : null;
			text.Foreground = new SolidColorBrush(convertColour(font.fontColour));
			shadow.Color = convertColour(font.shadowColour);
			shadow.Opacity = (double)shadow.Color.A / 255d;
		}

		private void updateDisplayState()
		{
			switch (_displayMode)
			{
			case "active":
				updateText(eventTitle, eventTitleShadow, OverlaySettings.activeEventTitleFont);
				updateText(eventStatus, eventStatusShadow, OverlaySettings.activeEventStatusFont);
				updateText(eventTimer, eventTimerShadow, OverlaySettings.activeEventTimerFont);
				Background = new ImageBrush((ImageSource)Resources["event-" + OverlaySettings.activeEventBackgroundImage]);
				Background.Opacity = OverlaySettings.activeEventBackgroundOpacity;
				contentWrapper.Margin = OverlaySettings.activeEventMargins;
				break;
			case "window":
				updateText(eventTitle, eventTitleShadow, OverlaySettings.windowEventTitleFont);
				updateText(eventStatus, eventStatusShadow, OverlaySettings.windowEventStatusFont);
				updateText(eventTimer, eventTimerShadow, OverlaySettings.windowEventTimerFont);
				Background = new ImageBrush((ImageSource)Resources["event-" + OverlaySettings.windowEventBackgroundImage]);
				Background.Opacity = OverlaySettings.windowEventBackgroundOpacity;
				contentWrapper.Margin = OverlaySettings.windowEventMargins;
				break;
			case "inactive":
				updateText(eventTitle, eventTitleShadow, OverlaySettings.inactiveEventTitleFont);
				updateText(eventStatus, eventStatusShadow, OverlaySettings.inactiveEventStatusFont);
				updateText(eventTimer, eventTimerShadow, OverlaySettings.inactiveEventTimerFont);
				Background = new ImageBrush((ImageSource)Resources["event-" + OverlaySettings.inactiveEventBackgroundImage]);
				Background.Opacity = OverlaySettings.inactiveEventBackgroundOpacity;
				contentWrapper.Margin = OverlaySettings.inactiveEventMargins;
				break;
			}
		}

		/* Global event hooking */

		private void UserControl_Loaded_1(object sender, System.Windows.RoutedEventArgs e)
		{
			if (!hooked)
			{
				OverlaySettings.settingsUpdated += DisplaySettings_settingsUpdated;
			}
		}

		private void UserControl_Unloaded_1(object sender, System.Windows.RoutedEventArgs e)
		{
			if (hooked)
			{
				OverlaySettings.settingsUpdated -= DisplaySettings_settingsUpdated;
			}
		}

		void DisplaySettings_settingsUpdated(object sender, EventArgs e)
		{
			updateDisplayState();
		}

		private void UserControl_MouseDoubleClick_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (_completed)
			{
				completed = false;
				Properties.Settings.Default.dailyCompletionList.Remove(eventInfo.id);
			}
			else
			{
				completed = true;
				Properties.Settings.Default.dailyCompletionList.Add(eventInfo.id);
			}
		}
	}
}
