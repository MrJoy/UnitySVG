using System.Collections.Generic;
using UnitySVG;

public class SVGEllipseElement : SVGBasicElement {
  private readonly SVGLength _cx, _cy, _rx, _ry;

  public SVGLength cx { get { return _cx; } }

  public SVGLength cy { get { return _cy; } }

  public SVGLength rx { get { return _rx; } }

  public SVGLength ry { get { return _ry; } }

  public SVGEllipseElement(Dictionary<string, string> attrList,
                           SVGTransformList inheritTransformList,
                           SVGPaintable inheritPaintable,
                           SVGGraphics render) : base(attrList, inheritTransformList, inheritPaintable, render) {
    _cx = new SVGLength(attrList.GetValue("cx"));
    _cy = new SVGLength(attrList.GetValue("cy"));
    _rx = new SVGLength(attrList.GetValue("rx"));
    _ry = new SVGLength(attrList.GetValue("ry"));
    currentTransformList = new SVGTransformList(attrList.GetValue("transform"));
  }

  protected override void CreateGraphicsPath() {
    _graphicsPath = new SVGGraphicsPath();
    _graphicsPath.Add(this);
    _graphicsPath.transformList = summaryTransformList;
  }
}
