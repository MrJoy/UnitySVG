using System.Collections.Generic;
using UnitySVG;

public class SVGRadialGradientElement : SVGGradientElement {
  private readonly SVGLength _cx, _cy, _r, _fx, _fy;

  // TODO: Does C# have auto properties?
  public SVGLength cx { get { return _cx; } }

  public SVGLength cy { get { return _cy; } }

  public SVGLength r { get { return _r; } }

  public SVGLength fx { get { return _fx; } }

  public SVGLength fy { get { return _fy; } }

  public SVGRadialGradientElement(SVGParser xmlImp, Dictionary<string, string> attrList) : base(xmlImp, attrList) {
    // TODO: Override GetValue to return `null` and use `||`.
    string temp = attrList.GetValue("cx");
    _cx = new SVGLength((temp == "") ? "50%" : temp);

    temp = attrList.GetValue("cy");
    _cy = new SVGLength((temp == "") ? "50%" : temp);

    temp = attrList.GetValue("r");
    _r = new SVGLength((temp == "") ? "50%" : temp);

    temp = attrList.GetValue("fx");
    _fx = new SVGLength((temp == "") ? "50%" : temp);

    temp = attrList.GetValue("fy");
    _fy = new SVGLength((temp == "") ? "50%" : temp);
  }
}
