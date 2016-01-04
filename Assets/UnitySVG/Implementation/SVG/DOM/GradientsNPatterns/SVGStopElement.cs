using System.Collections.Generic;
using UnitySVG;

public class SVGStopElement {
  private readonly float _offset;
  private readonly SVGColor _stopColor;

  public float offset { get { return _offset; } }

  public SVGColor stopColor { get { return _stopColor; } }

  public SVGStopElement(Dictionary<string, string> attrList) {
    _stopColor = new SVGColor(attrList.GetValue("stop-color"));
    string temp = attrList.GetValue("offset").Trim();
    if(temp != "") {
      if(temp.EndsWith("%"))
        _offset = float.Parse(temp.TrimEnd(new[] { '%' }), System.Globalization.CultureInfo.InvariantCulture);
      else
        _offset = float.Parse(temp, System.Globalization.CultureInfo.InvariantCulture) * 100;
    }
  }
}
