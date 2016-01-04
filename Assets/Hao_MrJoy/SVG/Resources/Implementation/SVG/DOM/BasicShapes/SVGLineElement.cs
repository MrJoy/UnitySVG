using System.Collections.Generic;
using UnityEngine;
using UnitySVG;

public class SVGLineElement : SVGTransformable, ISVGDrawable {
  private SVGLength _x1, _y1, _x2, _y2;
  private readonly SVGGraphics _render;
  private readonly SVGPaintable _paintable;

  public SVGLength x1 { get { return _x1; } }

  public SVGLength y1 { get { return _y1; } }

  public SVGLength x2 { get { return _x2; } }

  public SVGLength y2 { get { return _y2; } }

  public SVGLineElement(Dictionary<string, string> attrList,
                        SVGTransformList inheritTransformList,
                        SVGPaintable inheritPaintable,
                        SVGGraphics render) : base(inheritTransformList) {
    _paintable = new SVGPaintable(inheritPaintable, attrList);
    _render = render;
    _x1 = new SVGLength(attrList.GetValue("x1"));
    _y1 = new SVGLength(attrList.GetValue("y1"));
    _x2 = new SVGLength(attrList.GetValue("x2"));
    _y2 = new SVGLength(attrList.GetValue("y2"));
  }

  public void BeforeRender(SVGTransformList transformList) {
    inheritTransformList = transformList;
  }

  public void Render() {
    if(_paintable.strokeColor == null)
      return;
    Matrix2x3 _matrix = transformMatrix;
    float _width = _paintable.strokeWidth;
    _render.StrokeLineCap = _paintable.strokeLineCap;

    Vector2 p1 = _matrix.Transform(new Vector2(_x1.value, _y1.value));
    Vector2 p2 = _matrix.Transform(new Vector2(_x2.value, _y2.value));

    _render.Line(p1, p2, _paintable.strokeColor, _width);
  }
}
