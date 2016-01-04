using System.Collections.Generic;
using UnitySVG;

public class SVGRectElement : SVGBasicElement {
  private readonly SVGLength _x, _y, _width, _height, _rx, _ry;

  public SVGLength x { get { return _x; } }

  public SVGLength y { get { return _y; } }

  public SVGLength width { get { return _width; } }

  public SVGLength height { get { return _height; } }

  public SVGLength rx { get { return _rx; } }

  public SVGLength ry { get { return _ry; } }

  public SVGRectElement(Dictionary<string, string> attrList,
                        SVGTransformList inheritTransformList,
                        SVGPaintable inheritPaintable,
                        SVGGraphics render) : base(attrList, inheritTransformList, inheritPaintable, render) {
    _x = new SVGLength(attrList.GetValue("x"));
    _y = new SVGLength(attrList.GetValue("y"));
    _width = new SVGLength(attrList.GetValue("width"));
    _height = new SVGLength(attrList.GetValue("height"));
    _rx = new SVGLength(attrList.GetValue("rx"));
    _ry = new SVGLength(attrList.GetValue("ry"));
  }

  protected override void CreateGraphicsPath() {
    _graphicsPath = new SVGGraphicsPath();

    _graphicsPath.Add(this);
    _graphicsPath.transformList = summaryTransformList;
  }
}
