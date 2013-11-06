using System;

namespace GW2Stuff
{
	[Serializable]
	public class FontSettings
	{
		public String fontFace = "Segoe UI";
		public Int32 fontSize = 14;
		public bool fontBold = false;
		public bool fontItalic = false;
		public bool fontUnderline = false;
		public UInt32 fontColour = 0xffffffff;
		public UInt32 shadowColour = 0x000000ff;

		public FontSettings clone()
		{
			return (FontSettings)this.MemberwiseClone();
		}
	}
}
