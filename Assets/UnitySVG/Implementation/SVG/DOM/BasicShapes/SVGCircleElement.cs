using System.Collections.Generic;
using UnitySVG;

public class SVGCircleElement : SVGBasicElement {
  private readonly SVGLength _cx, _cy, _r;

  public SVGLength cx { get { return _cx; } }

  public SVGLength cy { get { return _cy; } }

  public SVGLength r { get { return _r; } }

  public SVGCircleElement(Dictionary<string, string> attrList,
                          SVGTransformList inheritTransformList,
                          SVGPaintable inheritPaintable,
                          SVGGraphics render) : base(attrList, inheritTransformList, inheritPaintable, render) {
    _cx = new SVGLength(attrList.GetValue("cx"));
    _cy = new SVGLength(attrList.GetValue("cy"));
    _r = new SVGLength(attrList.GetValue("r"));
  }

  protected override void CreateGraphicsPath() {
    _graphicsPath = new SVGGraphicsPath();
    _graphicsPath.Add(this);
    _graphicsPath.transformList = summaryTransformList;
  }
}
