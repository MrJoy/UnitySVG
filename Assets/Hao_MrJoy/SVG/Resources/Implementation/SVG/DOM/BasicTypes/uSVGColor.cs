using UnityEngine;
using System.Collections;

public enum SVGColorType : ushort {
  SVG_COLORTYPE_UNKNOWN           = 0,
//  SVG_COLORTYPE_RGBCOLOR          = 1,
//  SVG_COLORTYPE_RGBCOLOR_ICCCOLOR = 2,
  SVG_COLORTYPE_CONSTNAME         = 3,
  SVG_COLORTYPE_HEXSTRING         = 4,
  SVG_COLORTYPE_CURRENTCOLOR      = 5,
  SVG_COLORTYPE_NONE              = 6
}

public struct uSVGColor {
  public SVGColorType colorType;
  public Color color;
  /***********************************************************************************/
  public uSVGColor(string colorString) {
    if(uSVGColorExtractor.IsConstName(colorString) == true) {
      colorType = SVGColorType.SVG_COLORTYPE_CONSTNAME;
    } else if(uSVGColorExtractor.IsHexColor(colorString) == true) {
      colorType = SVGColorType.SVG_COLORTYPE_HEXSTRING;
    } else if(colorString.ToLower() == "current") {
      colorType = SVGColorType.SVG_COLORTYPE_CURRENTCOLOR;
    } else if(colorString.ToLower() == "none") {
      colorType = SVGColorType.SVG_COLORTYPE_NONE;
    } else {
      colorType = SVGColorType.SVG_COLORTYPE_UNKNOWN;
    }

    if(colorType != SVGColorType.SVG_COLORTYPE_UNKNOWN)
      color = uSVGColorExtractor.GetColor(colorString);
    else
      color = Color.black;
  }
}
