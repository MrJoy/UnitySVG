using UnityEngine;
using System.Collections.Generic;

public struct SVGColorExtractor {
  public static Dictionary<string, Color> ConstantColors = new Dictionary<string, Color> {
    { "aliceblue", change(240, 248, 255) },
    { "antiquewhite", change(250, 235, 215) },
    { "aqua", change(0, 255, 255) },
    { "aquamarine", change(127, 255, 212) },
    { "azure", change(240, 255, 255) },
    { "beige", change(245, 245, 220) },
    { "bisque", change(255, 228, 196) },
    { "black", change(0, 0, 0) },
    { "blanchedalmond", change(255, 235, 205) },
    { "blue", change(0, 0, 255) },
    { "blueviolet", change(138, 43, 226) },
    { "brown", change(165, 42, 42) },
    { "burlywood", change(222, 184, 135) },
    { "cadetblue", change(95, 158, 160) },
    { "chartreuse", change(127, 255, 0) },
    { "chocolate", change(210, 105, 30) },
    { "coral", change(255, 127, 80) },
    { "cornflowerblue", change(100, 149, 237) },
    { "cornsilk", change(255, 248, 220) },
    { "crimson", change(220, 20, 60) },
    { "cyan", change(0, 255, 255) },
    { "darkblue", change(0, 0, 139) },
    { "darkcyan", change(0, 139, 139) },
    { "darkgoldenrod", change(184, 134, 11) },
    { "darkgray", change(169, 169, 169) },
    { "darkgreen", change(0, 100, 0) },
    { "darkgrey", change(169, 169, 169) },
    { "darkkhaki", change(189, 183, 107) },
    { "darkmagenta", change(139, 0, 139) },
    { "darkolivegreen", change(85, 107, 47) },
    { "darkorange", change(255, 140, 0) },
    { "darkorchid", change(153, 50, 204) },
    { "darkred", change(139, 0, 0) },
    { "darksalmon", change(233, 150, 122) },
    { "darkseagreen", change(143, 188, 143) },
    { "darkslateblue", change(72, 61, 139) },
    { "darkslategray", change(47, 79, 79) },
    { "darkslategrey", change(47, 79, 79) },
    { "darkturquoise", change(0, 206, 209) },
    { "darkviolet", change(148, 0, 211) },
    { "deeppink", change(255, 20, 147) },
    { "deepskyblue", change(0, 191, 255) },
    { "dimgray", change(105, 105, 105) },
    { "dimgrey", change(105, 105, 105) },
    { "dodgerblue", change(30, 144, 255) },
    { "firebrick", change(178, 34, 34) },
    { "floralwhite", change(255, 250, 240) },
    { "forestgreen", change(34, 139, 34) },
    { "fuchsia", change(255, 0, 255) },
    { "gainsboro", change(220, 220, 220) },
    { "ghostwhite", change(248, 248, 255) },
    { "gold", change(255, 215, 0) },
    { "goldenrod", change(218, 165, 32) },
    { "gray", change(128, 128, 128) },
    { "grey", change(128, 128, 128) },
    { "green", change(0, 128, 0) },
    { "greenyellow", change(173, 255, 47) },
    { "honeydew", change(240, 255, 240) },
    { "hotpink", change(255, 105, 180) },
    { "indianred", change(205, 92, 92) },
    { "indigo", change(75, 0, 130) },
    { "ivory", change(255, 255, 240) },
    { "khaki", change(240, 230, 140) },
    { "lavender", change(230, 230, 250) },
    { "lavenderblush", change(255, 240, 245) },
    { "lawngreen", change(124, 252, 0) },
    { "lemonchiffon", change(255, 250, 205) },
    { "lightblue", change(173, 216, 230) },
    { "lightcoral", change(240, 128, 128) },
    { "lightcyan", change(224, 255, 255) },
    { "lightgoldenrodyellow", change(250, 250, 210) },
    { "lightgray", change(211, 211, 211) },
    { "lightgreen", change(144, 238, 144) },
    { "lightgrey", change(211, 211, 211) },
    { "lightpink", change(255, 182, 193) },
    { "lightsalmon", change(255, 160, 122) },
    { "lightseagreen", change(32, 178, 170) },
    { "lightskyblue", change(135, 206, 250) },
    { "lightslategray", change(119, 136, 153) },
    { "lightslategrey", change(119, 136, 153) },
    { "lightsteelblue", change(176, 196, 222) },
    { "lightyellow", change(255, 255, 224) },
    { "lime", change(0, 255, 0) },
    { "limegreen", change(50, 205, 50) },
    { "linen", change(250, 240, 230) },
    { "magenta", change(255, 0, 255) },
    { "maroon", change(128, 0, 0) },
    { "mediumaquamarine", change(102, 205, 170) },
    { "mediumblue", change(0, 0, 205) },
    { "mediumorchid", change(186, 85, 211) },
    { "mediumpurple", change(147, 112, 219) },
    { "mediumseagreen", change(60, 179, 113) },
    { "mediumslateblue", change(123, 104, 238) },
    { "mediumspringgreen", change(0, 250, 154) },
    { "mediumturquoise", change(72, 209, 204) },
    { "mediumvioletred", change(199, 21, 133) },
    { "midnightblue", change(25, 25, 112) },
    { "mintcream", change(245, 255, 250) },
    { "mistyrose", change(255, 228, 225) },
    { "moccasin", change(255, 228, 181) },
    { "navajowhite", change(255, 222, 173) },
    { "navy", change(0, 0, 128) },
    { "oldlace", change(253, 245, 230) },
    { "olive", change(128, 128, 0) },
    { "olivedrab", change(107, 142, 35) },
    { "orange", change(255, 165, 0) },
    { "orangered", change(255, 69, 0) },
    { "orchid", change(218, 112, 214) },
    { "palegoldenrod", change(238, 232, 170) },
    { "palegreen", change(152, 251, 152) },
    { "paleturquoise", change(175, 238, 238) },
    { "palevioletred", change(219, 112, 147) },
    { "papayawhip", change(255, 239, 213) },
    { "peachpuff", change(255, 218, 185) },
    { "peru", change(205, 133, 63) },
    { "pink", change(255, 192, 203) },
    { "plum", change(221, 160, 221) },
    { "powderblue", change(176, 224, 230) },
    { "purple", change(128, 0, 128) },
    { "red", change(255, 0, 0) },
    { "rosybrown", change(188, 143, 143) },
    { "royalblue", change(65, 105, 225) },
    { "saddlebrown", change(139, 69, 19) },
    { "salmon", change(250, 128, 114) },
    { "sandybrown", change(244, 164, 96) },
    { "seagreen", change(46, 139, 87) },
    { "seashell", change(255, 245, 238) },
    { "sienna", change(160, 82, 45) },
    { "silver", change(192, 192, 192) },
    { "skyblue", change(135, 206, 235) },
    { "slateblue", change(106, 90, 205) },
    { "slategray", change(112, 128, 144) },
    { "slategrey", change(112, 128, 144) },
    { "snow", change(255, 250, 250) },
    { "springgreen", change(0, 255, 127) },
    { "steelblue", change(70, 130, 180) },
    { "tan", change(210, 180, 140) },
    { "teal", change(0, 128, 128) },
    { "thistle", change(216, 191, 216) },
    { "tomato", change(255, 99, 71) },
    { "turquoise", change(64, 224, 208) },
    { "violet", change(238, 130, 238) },
    { "wheat", change(245, 222, 179) },
    { "white", change(255, 255, 255) },
    { "whitesmoke", change(245, 245, 245) },
    { "yellow", change(255, 255, 0) },
    { "yellowgreen", change(154, 205, 50) }
  };

  private static Color change(int r, int g, int b) {
    return new Color((float)r / 255.0f, (float)g / 255.0f, (float)b / 255.0f);
  }

  public static Color ConstColor(string name) {
    if(name.Length == 0)
      return Color.black;
    return ConstantColors[name.ToLower()];
  }
  //------------
  public static bool IsConstName(string textColor) {
    if(textColor.Length == 0)
      return false;
    if(textColor[0] == '#')
      return false;
    return ConstantColors.ContainsKey(textColor.ToLower());
  }

  public static bool IsHexColor(string colorStr) {
    if(colorStr.Length > 0) {
      if(colorStr[0] == '#') {
        if((colorStr.Length == 4) || (colorStr.Length == 7))
          return true;
      }
    }
    return false;
  }

  private static int ParseHexDigit(char c) {
    int cc = (int)c - (int)'0';
    if(cc >= 0 && cc < 10)
      return cc;
    cc = c - (int)'a';
    if(cc >= 0 && cc < 6)
      return 10 + cc;
    cc = c - (int)'A';
    if(cc >= 0 && cc < 6)
      return 10 + cc;
    return 0;
  }

  public static Color HexColor(string colorStr) {
    int r = 0, g = 0, b = 0;
    if(colorStr.Length > 0) {
      if(colorStr[0] == '#') {
        if(colorStr.Length == 4) {
          int i = ParseHexDigit(colorStr[1]);
          r = i * 16 + i;
          i = ParseHexDigit(colorStr[2]);
          g = i * 16 + i;
          i = ParseHexDigit(colorStr[3]);
          b = i * 16 + i;
        } else if(colorStr.Length == 7) {
          r = ParseHexDigit(colorStr[1]) * 16 + ParseHexDigit(colorStr[2]);
          g = ParseHexDigit(colorStr[3]) * 16 + ParseHexDigit(colorStr[4]);
          b = ParseHexDigit(colorStr[5]) * 16 + ParseHexDigit(colorStr[6]);
        }
      }
    }
    return change(r, g, b);
  }
}
