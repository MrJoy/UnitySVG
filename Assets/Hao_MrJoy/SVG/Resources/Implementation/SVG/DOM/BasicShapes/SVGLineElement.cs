public class SVGLineElement : SVGTransformable, ISVGDrawable {
  private SVGLength _x1;
  private SVGLength _y1;
  private SVGLength _x2;
  private SVGLength _y2;
  /***********************************************************************************/
  private SVGGraphics _render;
  private AttributeList _attrList;
  private SVGPaintable _paintable;
  /***********************************************************************************/
  public SVGLength x1 {
    get {
      return this._x1;
    }
  }

  public SVGLength y1 {
    get {
      return this._y1;
    }
  }

  public SVGLength x2 {
    get {
      return this._x2;
    }
  }

  public SVGLength y2 {
    get {
      return this._y2;
    }
  }
  /***********************************************************************************/
  public SVGLineElement(  AttributeList attrList,
              SVGTransformList inheritTransformList,
              SVGPaintable inheritPaintable,
              SVGGraphics _render) : base(inheritTransformList) {
    this._attrList = attrList;
    this._paintable = new SVGPaintable(inheritPaintable, this._attrList);
    this._render = _render;
    this._x1 = new SVGLength(attrList.GetValue("x1"));
    this._y1 = new SVGLength(attrList.GetValue("y1"));
    this._x2 = new SVGLength(attrList.GetValue("x2"));
    this._y2 = new SVGLength(attrList.GetValue("y2"));
  }
  /***********************************************************************************/
  //Thuc thi Interface Drawable
  public void BeforeRender(SVGTransformList transformList) {
    this.inheritTransformList = transformList;
  }
  public void Render() {
    SVGPoint p1, p2;
    SVGMatrix _matrix = this.transformMatrix;
    if(this._paintable.strokeColor == null)return;

    float _width = this._paintable.strokeWidth;
    this._render.SetStrokeLineCap(this._paintable.strokeLineCap);

    float tx1 = this._x1.value;
    float ty1 = this._y1.value;
    float tx2 = this._x2.value;
    float ty2 = this._y2.value;
    p1 = new SVGPoint(tx1, ty1);
    p2 = new SVGPoint(tx2, ty2);

    p1 = p1.MatrixTransform(_matrix);
    p2 = p2.MatrixTransform(_matrix);

    this._render.Line(p1, p2, this._paintable.strokeColor, _width);
  }
}
