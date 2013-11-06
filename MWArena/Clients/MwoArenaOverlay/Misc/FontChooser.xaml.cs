using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows;
using System.Windows.Controls;

namespace GW2Stuff
{
	/// <summary>
	/// Interaction logic for FontChooser.xaml
	/// </summary>
	public partial class FontChooser: UserControl
	{
		public FontChooser()
		{
			InitializeComponent();

			InstalledFontCollection installedFonts = new InstalledFontCollection();
			foreach (FontFamily font in installedFonts.Families)
			{
				fontFace.Items.Add(font.Name);
			}
		}

		public FontSettings fontSettings
		{
			get
			{
				FontSettings settings = new FontSettings();

				if (fontFace.SelectedItem != null)
				{
					settings.fontFace = (String)fontFace.SelectedItem;
				}
				else
				{
					return null;
				}

				if (fontSize.SelectedItem != null)
				{
					settings.fontSize = Int32.Parse((String)fontSize.SelectedItem);
				}
				else
				{
					return null;
				}

				settings.fontBold = (bool)fontBold.IsChecked;
				settings.fontItalic = (bool)fontItalic.IsChecked;
				settings.fontUnderline = (bool)fontUnderline.IsChecked;

				if (!UInt32.TryParse(fontColour.Text, System.Globalization.NumberStyles.HexNumber, null, out settings.fontColour))
				{
					return null;
				}

				if (!UInt32.TryParse(shadowColour.Text, System.Globalization.NumberStyles.HexNumber, null, out settings.shadowColour))
				{
					return null;
				}

				return settings;
			}
			set
			{
				if (fontFace.Items.Contains(value.fontFace))
				{
					fontFace.SelectedItem = value.fontFace;
				}

				if (fontSize.Items.Contains(value.fontSize.ToString()))
				{
					fontSize.SelectedItem = value.fontSize.ToString();
				}

				fontBold.IsChecked = value.fontBold;
				fontItalic.IsChecked = value.fontItalic;
				fontUnderline.IsChecked = value.fontUnderline;
				fontColour.Text = value.fontColour.ToString("X8");
				shadowColour.Text = value.shadowColour.ToString("X8");
			}
		}

		private void fontColour_LostFocus(object sender, RoutedEventArgs e)
		{
			UInt32 colour;
			if (!UInt32.TryParse(fontColour.Text, System.Globalization.NumberStyles.HexNumber, null, out colour))
			{
				colour = 0xffffffff;
			}
			fontColour.Text = colour.ToString("X8");
		}

		private void shadowColour_LostFocus(object sender, RoutedEventArgs e)
		{
			UInt32 colour;
			if (!UInt32.TryParse(shadowColour.Text, System.Globalization.NumberStyles.HexNumber, null, out colour))
			{
				colour = 0x000000ff;
			}
			shadowColour.Text = colour.ToString("X8");
		}
	}
}
