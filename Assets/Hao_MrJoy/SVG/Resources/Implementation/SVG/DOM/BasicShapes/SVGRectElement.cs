public class SVGRectElement : SVGTransformable, ISVGDrawable {
  private SVGLength _x;
  private SVGLength _y;
  private SVGLength _width;
  private SVGLength _height;
  private SVGLength _rx;
  private SVGLength _ry;
  //================================================================================
  private SVGGraphics _render;
  private AttributeList _attrList;
  private SVGPaintable _paintable;
  //================================================================================
  public SVGLength x {
    get {
      return this._x;
    }
  }

  public SVGLength y {
    get {
      return this._y;
    }
  }

  public SVGLength width {
    get {
      return this._width;
    }
  }

  public SVGLength height {
    get {
      return this._height;
    }
  }


  public SVGLength rx {
    get {
      return this._rx;
    }
  }

  public SVGLength ry {
    get {
      return this._ry;
    }
  }
  //================================================================================
  public SVGRectElement(AttributeList attrList,
              SVGTransformList inheritTransformList,
              SVGPaintable inheritPaintable,
              SVGGraphics _render) : base(inheritTransformList) {
    this._attrList = attrList;
    this._render = _render;
    this._paintable = new SVGPaintable(inheritPaintable, this._attrList);
    this._x = new SVGLength(attrList.GetValue("x"));
    this._y = new SVGLength(attrList.GetValue("y"));
    this._width = new SVGLength(attrList.GetValue("width"));
    this._height = new SVGLength(attrList.GetValue("height"));
    this._rx = new SVGLength(attrList.GetValue("rx"));
    this._ry = new SVGLength(attrList.GetValue("ry"));
  }
  //================================================================================
  private SVGGraphicsPath _graphicsPath;
  private void CreateGraphicsPath() {
    this._graphicsPath = new SVGGraphicsPath();

    this._graphicsPath.Add(this);
    this._graphicsPath.transformList = this.summaryTransformList;
  }
  //-----
  private void Draw() {
    if(this._paintable.strokeColor == null)return;

    this._render.DrawPath(this._graphicsPath, this._paintable.strokeWidth,
                            this._paintable.strokeColor);
  }
  //================================================================================
  //Thuc thi Interface Drawable
  public void BeforeRender(SVGTransformList transformList) {
    this.inheritTransformList = transformList;
  }
  //------
  public void Render() {
    CreateGraphicsPath();
    this._render.SetStrokeLineCap(this._paintable.strokeLineCap);
    this._render.SetStrokeLineJoin(this._paintable.strokeLineJoin);
    switch(this._paintable.GetPaintType()) {
      case SVGPaintMethod.SolidGradientFill : {
        this._render.FillPath(this._paintable.fillColor.Value, this._graphicsPath);
        Draw();
        break;
      }
      case SVGPaintMethod.LinearGradientFill : {

        SVGLinearGradientBrush _linearGradBrush =
                  this._paintable.GetLinearGradientBrush(this._graphicsPath);

        if(_linearGradBrush != null) {
          this._render.FillPath(_linearGradBrush, _graphicsPath);
        }
        Draw();
        break;
      }
      case SVGPaintMethod.RadialGradientFill : {
        SVGRadialGradientBrush _radialGradBrush =
                  this._paintable.GetRadialGradientBrush(this._graphicsPath);

        if(_radialGradBrush != null) {
          this._render.FillPath(_radialGradBrush, _graphicsPath);
        }
        Draw();
        break;
      }
      case SVGPaintMethod.PathDraw : {
        Draw();
      break;
      }
    }
  }
}