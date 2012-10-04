using UnityEngine;
using System.Collections.Generic;

public class SVGPolylineElement : SVGTransformable, ISVGDrawable {
  private List<Vector2> _listPoints;
  //================================================================================
  private SVGGraphics _render;
  private AttributeList _attrList;
  private SVGPaintable _paintable;
  //================================================================================
  public List<Vector2> listPoints {
    get{ return this._listPoints;}
  }
  //================================================================================
  public SVGPolylineElement(  AttributeList attrList,
                SVGTransformList inheritTransformList,
                SVGPaintable inheritPaintable,
                SVGGraphics _render) : base(inheritTransformList) {
    this._attrList = attrList;
    this._render = _render;
    this._paintable = new SVGPaintable(inheritPaintable, attrList);
    this._listPoints = ExtractPoints(this._attrList.GetValue("points"));
  }
  //================================================================================
  private List<Vector2> ExtractPoints(string inputText) {
    List<Vector2> _return = new List<Vector2>();
    string[] _lstStr = SVGStringExtractor.ExtractTransformValue(inputText);

    int len = _lstStr.Length;
    for(int i = 0; i < len -1; i++) {
      string value1, value2;
      value1 = _lstStr[i];
      value2 = _lstStr[i+1];
      SVGLength _length1 = new SVGLength(value1);
      SVGLength _length2 = new SVGLength(value2);
      Vector2 _point = new Vector2(_length1.value, _length2.value);
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
