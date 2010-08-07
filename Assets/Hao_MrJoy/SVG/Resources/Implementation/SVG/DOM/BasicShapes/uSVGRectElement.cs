public class uSVGRectElement : uSVGTransformable, uISVGDrawable {
  private uSVGLength _x;
  private uSVGLength _y;
  private uSVGLength _width;
  private uSVGLength _height;
  private uSVGLength _rx;
  private uSVGLength _ry;
  //================================================================================
  private uSVGGraphics _render;
  private AttributeList _attrList;
  private uSVGPaintable _paintable;
  //================================================================================
  public uSVGLength x {
    get {
      return this._x;
    }
  }

  public uSVGLength y {
    get {
      return this._y;
    }
  }

  public uSVGLength width {
    get {
      return this._width;
    }
  }

  public uSVGLength height {
    get {
      return this._height;
    }
  }


  public uSVGLength rx {
    get {
      return this._rx;
    }
  }

  public uSVGLength ry {
    get {
      return this._ry;
    }
  }
  //================================================================================
  public uSVGRectElement(AttributeList attrList,
              uSVGTransformList inheritTransformList,
              uSVGPaintable inheritPaintable,
              uSVGGraphics _render) : base(inheritTransformList) {
    this._attrList = attrList;
    this._render = _render;
    this._paintable = new uSVGPaintable(inheritPaintable, this._attrList);
    this._x = new uSVGLength(attrList.GetValue("x"));
    this._y = new uSVGLength(attrList.GetValue("y"));
    this._width = new uSVGLength(attrList.GetValue("width"));
    this._height = new uSVGLength(attrList.GetValue("height"));
    this._rx = new uSVGLength(attrList.GetValue("rx"));
    this._ry = new uSVGLength(attrList.GetValue("ry"));
  }
  //================================================================================
  private uSVGGraphicsPath _graphicsPath;
  private void CreateGraphicsPath() {
    this._graphicsPath = new uSVGGraphicsPath();

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
  public void BeforeRender(uSVGTransformList transformList) {
    this.inheritTransformList = transformList;
  }
  //------
  public void Render() {
    CreateGraphicsPath();
    this._render.SetStrokeLineCap(this._paintable.strokeLineCap);
    this._render.SetStrokeLineJoin(this._paintable.strokeLineJoin);
    switch(this._paintable.GetPaintType()) {
      case uSVGPaintTypes.SVG_PAINT_SOLID_GRADIENT_FILL : {
        this._render.FillPath(this._paintable.fillColor.Value, this._graphicsPath);
        Draw();
        break;
      }
      case uSVGPaintTypes.SVG_PAINT_LINEAR_GRADIENT_FILL : {

        uSVGLinearGradientBrush _linearGradBrush =
                  this._paintable.GetLinearGradientBrush(this._graphicsPath);

        if(_linearGradBrush != null) {
          this._render.FillPath(_linearGradBrush, _graphicsPath);
        }
        Draw();
        break;
      }
      case uSVGPaintTypes.SVG_PAINT_RADIAL_GRADIENT_FILL : {
        uSVGRadialGradientBrush _radialGradBrush =
                  this._paintable.GetRadialGradientBrush(this._graphicsPath);

        if(_radialGradBrush != null) {
          this._render.FillPath(_radialGradBrush, _graphicsPath);
        }
        Draw();
        break;
      }
      case uSVGPaintTypes.SVG_PAINT_PATH_DRAW : {
        Draw();
      break;
      }
    }
  }
}