using UnityEngine;
using System.Collections;

public enum uSVGColorTypes : ushort {
	SVG_COLORTYPE_UNKNOWN			= 0,
	SVG_COLORTYPE_RGBCOLOR			= 1,
	SVG_COLORTYPE_RGBCOLOR_ICCCOLOR	= 2,
	SVG_COLORTYPE_CONSTNAME			= 3,
	SVG_COLORTYPE_HEXSTRING			= 4,
	SVG_COLORTYPE_CURRENTCOLOR		= 5,
	SVG_COLORTYPE_NONE				= 6
}

public class uSVGColor {
	private uSVGColorTypes m_colorType = uSVGColorTypes.SVG_COLORTYPE_UNKNOWN;
	private Color m_color;
	/***********************************************************************************/
	public uSVGColorTypes colorType {
		get{ return this.m_colorType;}
	}
	public Color color {
		get{ return this.m_color;}
	}
	/***********************************************************************************/
	public uSVGColor() {
		m_colorType = uSVGColorTypes.SVG_COLORTYPE_UNKNOWN;
		m_color = new Color(0f, 0f, 0f);
	}
	public uSVGColor(string colorString) {		
		SetColor(colorString);
	}
	/***********************************************************************************/
	public void SetColor(string colorString) {
		if (uSVGColorExtractor.IsConstName(colorString) == true) {
			this.m_colorType = uSVGColorTypes.SVG_COLORTYPE_CONSTNAME;
		} else if (uSVGColorExtractor.IsHexColor(colorString) == true) {
			this.m_colorType = uSVGColorTypes.SVG_COLORTYPE_HEXSTRING;
		} else if (colorString.ToLower() == "current") {
			this.m_colorType = uSVGColorTypes.SVG_COLORTYPE_CURRENTCOLOR;
		} else if (colorString.ToLower() == "current") {
			this.m_colorType = uSVGColorTypes.SVG_COLORTYPE_CURRENTCOLOR;
		} else if (colorString.ToLower() == "none") {
			this.m_colorType = uSVGColorTypes.SVG_COLORTYPE_NONE;
		}
		
		if (this.m_colorType != uSVGColorTypes.SVG_COLORTYPE_UNKNOWN) {
			this.m_color = uSVGColorExtractor.f_GetColor(colorString);
		}
	}
}
