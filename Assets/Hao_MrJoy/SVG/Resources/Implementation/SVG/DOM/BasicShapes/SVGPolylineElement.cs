using System.Collections.Generic;

public class SVGPolylineElement : uSVGTransformable, uISVGDrawable {
  private List<uSVGPoint> _listPoints;
  //================================================================================
  private SVGGraphics _render;
  private AttributeList _attrList;
  private uSVGPaintable _paintable;
  //================================================================================
  public List<uSVGPoint> listPoints {
    get{ return this._listPoints;}
  }
  //================================================================================
  public SVGPolylineElement(  AttributeList attrList,
                uSVGTransformList inheritTransformList,
                uSVGPaintable inheritPaintable,
                SVGGraphics _render) : base(inheritTransformList) {
    this._attrList = attrList;
    this._render = _render;
    this._paintable = new uSVGPaintable(inheritPaintable, attrList);
    this._listPoints = ExtractPoints(this._attrList.GetValue("points"));
  }
  //================================================================================
  private List<uSVGPoint> ExtractPoints(string inputText) {
    List<uSVGPoint> _return = new List<uSVGPoint>();
    string[] _lstStr = uSVGStringExtractor.ExtractTransformValue(inputText);

    int len = _lstStr.Length;
    for(int i = 0; i < len -1; i++) {
      string value1, value2;
      value1 = _lstStr[i];
      value2 = _lstStr[i+1];
      SVGLength _length1 = new SVGLength(value1);
      SVGLength _length2 = new SVGLength(value2);
      uSVGPoint _point = new uSVGPoint(_length1.value, _length2.value);
      _return.Add(_point);
      i++;
    }
    return _return;
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
  //************************************************************************************
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

        SVGLinearGradientBrush _linearGradBrush =
                  this._paintable.GetLinearGradientBrush(this._graphicsPath);

        if(_linearGradBrush != null) {
          this._render.FillPath(_linearGradBrush, _graphicsPath);
        }
        Draw();
        break;
      }
      case uSVGPaintTypes.SVG_PAINT_RADIAL_GRADIENT_FILL : {
        SVGRadialGradientBrush _radialGradBrush =
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