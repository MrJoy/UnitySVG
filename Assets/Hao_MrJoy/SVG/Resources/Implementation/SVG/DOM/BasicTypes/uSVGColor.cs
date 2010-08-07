using UnityEngine;
using System.Collections;

public enum SVGColorType : ushort {
  SVG_COLORTYPE_UNKNOWN           = 0,
  SVG_COLORTYPE_RGBCOLOR          = 1,
//  SVG_COLORTYPE_RGBCOLOR_ICCCOLOR = 2,
  SVG_COLORTYPE_CURRENTCOLOR      = 3,
  SVG_COLORTYPE_NONE              = 4
}

public struct uSVGColor {
  public SVGColorType colorType;
  public Color color;
  /***********************************************************************************/
  public uSVGColor(string colorString) {
    if(uSVGColorExtractor.IsHexColor(colorString)) {
      colorType = SVGColorType.SVG_COLORTYPE_RGBCOLOR;
      color = uSVGColorExtractor.HexColor(colorString);
    } else if(uSVGColorExtractor.IsConstName(colorString)) {
      colorType = SVGColorType.SVG_COLORTYPE_RGBCOLOR;
      color = uSVGColorExtractor.ConstColor(colorString);
    } else if(colorString.ToLower() == "current") {
      colorType = SVGColorType.SVG_COLORTYPE_CURRENTCOLOR;
      color = Color.black;
    } else if(colorString.ToLower() == "none") {
      colorType = SVGColorType.SVG_COLORTYPE_NONE;
      color = Color.black;
    } else {
      colorType = SVGColorType.SVG_COLORTYPE_UNKNOWN;
      color = Color.black;
    }
  }
}
