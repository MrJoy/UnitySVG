using UnityEngine;

public enum SVGColorType : ushort {
  Unknown = 0,
  RGB = 1,
  Current = 2,
  None = 3
}

public struct SVGColor {
  public SVGColorType colorType;
  public Color color;

  public SVGColor(string colorString) {
    if(SVGColorExtractor.IsHexColor(colorString)) {
      colorType = SVGColorType.RGB;
      color = SVGColorExtractor.HexColor(colorString);
    } else if(SVGColorExtractor.IsConstName(colorString)) {
      colorType = SVGColorType.RGB;
      color = SVGColorExtractor.ConstColor(colorString);
    } else if(colorString.ToLower() == "current") {
      colorType = SVGColorType.Current;
      color = Color.black;
    } else if(colorString.ToLower() == "none") {
      colorType = SVGColorType.None;
      color = Color.black;
    } else {
      colorType = SVGColorType.Unknown;
      color = Color.black;
    }
  }
}
