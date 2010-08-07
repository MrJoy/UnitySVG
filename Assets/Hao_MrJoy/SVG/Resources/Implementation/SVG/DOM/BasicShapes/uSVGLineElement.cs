public class uSVGLineElement : uSVGTransformable, uISVGDrawable {
  private uSVGLength _x1;
  private uSVGLength _y1;
  private uSVGLength _x2;
  private uSVGLength _y2;
  /***********************************************************************************/
  private uSVGGraphics _render;
  private AttributeList _attrList;
  private uSVGPaintable _paintable;
  /***********************************************************************************/
  public uSVGLength x1 {
    get {
      return this._x1;
    }
  }

  public uSVGLength y1 {
    get {
      return this._y1;
    }
  }

  public uSVGLength x2 {
    get {
      return this._x2;
    }
  }

  public uSVGLength y2 {
    get {
      return this._y2;
    }
  }
  /***********************************************************************************/
  public uSVGLineElement(  AttributeList attrList,
              uSVGTransformList inheritTransformList,
              uSVGPaintable inheritPaintable,
              uSVGGraphics _render) : base(inheritTransformList) {
    this._attrList = attrList;
    this._paintable = new uSVGPaintable(inheritPaintable, this._attrList);
    this._render = _render;
    this._x1 = new uSVGLength(attrList.GetValue("X1"));
    this._y1 = new uSVGLength(attrList.GetValue("Y1"));
    this._x2 = new uSVGLength(attrList.GetValue("X2"));
    this._y2 = new uSVGLength(attrList.GetValue("Y2"));
  }
  /***********************************************************************************/
  //Thuc thi Interface Drawable
  public void BeforeRender(uSVGTransformList transformList) {
    this.inheritTransformList = transformList;
  }
  public void Render() {
    uSVGPoint p1, p2;
    uSVGMatrix _matrix = this.transformMatrix;
    if(this._paintable.strokeColor == null)return;

    float _width = this._paintable.strokeWidth;
    this._render.SetStrokeLineCap(this._paintable.strokeLineCap);

    float tx1 = this._x1.value;
    float ty1 = this._y1.value;
    float tx2 = this._x2.value;
    float ty2 = this._y2.value;
    p1 = new uSVGPoint(tx1, ty1);
    p2 = new uSVGPoint(tx2, ty2);

    p1 = p1.MatrixTransform(_matrix);
    p2 = p2.MatrixTransform(_matrix);

    this._render.Line(p1, p2, this._paintable.strokeColor, _width);
  }
}
