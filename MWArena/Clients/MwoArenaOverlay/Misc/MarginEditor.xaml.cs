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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GW2Stuff
{
	/// <summary>
	/// Interaction logic for MarginEditor.xaml
	/// </summary>
	public partial class MarginEditor : UserControl
	{
		public MarginEditor()
		{
			InitializeComponent();
		}

		public Thickness margins
		{
			get
			{
				Thickness value = new Thickness(0);
				Double temp;

				if (Double.TryParse(topMargin.Text, out temp)) value.Top = temp;
				if (Double.TryParse(rightMargin.Text, out temp)) value.Right = temp;
				if (Double.TryParse(bottomMargin.Text, out temp)) value.Bottom = temp;
				if (Double.TryParse(leftMargin.Text, out temp)) value.Left = temp;

				return value;
			}
			set
			{
				topMargin.Text = value.Top.ToString();
				rightMargin.Text = value.Right.ToString();
				bottomMargin.Text = value.Bottom.ToString();
				leftMargin.Text = value.Left.ToString();
			}
		}

		private void trim(TextBox textBox)
		{
			UInt32 value;
			if (UInt32.TryParse(textBox.Text, out value))
			{
				textBox.Text = (value < 0) ? "0" : value.ToString();
			}
			else
			{
				textBox.Text = "0";
			}
		}

		private void topMargin_LostFocus(object sender, RoutedEventArgs e)
		{
			trim(topMargin);
		}

		private void rightMargin_LostFocus(object sender, RoutedEventArgs e)
		{
			trim(rightMargin);
		}

		private void bottomMargin_LostFocus(object sender, RoutedEventArgs e)
		{
			trim(bottomMargin);
		}

		private void leftMargin_LostFocus(object sender, RoutedEventArgs e)
		{
			trim(leftMargin);
		}
	}
}
