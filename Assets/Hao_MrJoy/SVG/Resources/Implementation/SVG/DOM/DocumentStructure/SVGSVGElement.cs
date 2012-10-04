using System.Collections.Generic;
using UnityEngine;

public class SVGSVGElement : SVGTransformable, ISVGDrawable {
  private SVGLength _width;
  private SVGLength _height;

  private Rect _viewport;

  //-------------------------------
  private AttributeList _attrList;
  private List<object> _elementList;
  private SVGParser _xmlImp;
  //-------------------------------
  private SVGPaintable _paintable;
  //-------------------------------
  private SVGGraphics _render;

  /***********************************************************************************/
  public SVGSVGElement(  SVGParser xmlImp,
              SVGTransformList inheritTransformList,
              SVGPaintable inheritPaintable,
              SVGGraphics r) : base(inheritTransformList) {
    _render = r;
    _xmlImp = xmlImp;
    _attrList = _xmlImp.Node.Attributes;
    _paintable = new SVGPaintable(inheritPaintable, _attrList);
    _width = new SVGLength(_attrList.GetValue("width"));
    _height = new SVGLength(_attrList.GetValue("height"));
    Initial();
  }
  /***********************************************************************************/
  private void Initial() {
    //trich cac gia tri cua thuoc tinh VIEWBOX va chua vao trong _viewport
    SetViewBox();
    _elementList = new List<object>();

    //Viewbox transform se lay thuoc tinh VIEWBOX de tao ra 1 transform
    //va transform nay se chua trong _cachedViewBoxTransform
    ViewBoxTransform();

    //Tao currentTransformList va add cai transform dau tien vao, do la cai VIEWBOX.
    SVGTransform temp = new SVGTransform(_cachedViewBoxTransform);
    SVGTransformList t_currentTransformList = new SVGTransformList();
    t_currentTransformList.AppendItem(temp);

    this.currentTransformList = t_currentTransformList;

    //Get all element between <SVG>...</SVG>
    GetElementList();
  }
  /***********************************************************************************/
  private void GetElementList() {
    _xmlImp.GetElementList(_elementList, _paintable, _render, summaryTransformList);
  }
  /***********************************************************************************/
  public void BeforeRender(SVGTransformList transformList) {

    this.inheritTransformList = transformList;

    for(int i = 0; i < _elementList.Count; i++) {
      ISVGDrawable temp = _elementList[i] as ISVGDrawable;
      if(temp != null) {
        temp.BeforeRender(this.summaryTransformList);
      }
    }
  }

  public void Render() {
//Profiler.BeginSample("SVGSVGElement.Render() => SetSize");
    this._render.SetSize(this._width.value, this._height.value);
//Profiler.EndSample();
    for(int i = 0; i < _elementList.Count; i++) {
      ISVGDrawable temp = _elementList[i] as ISVGDrawable;
      if(temp != null) {
//Profiler.BeginSample("SVGSVGElement.Render() => " + temp.GetType().ToString());
        temp.Render();
//Profiler.EndSample();
      }
    }
  }
  /***********************************************************************************/
  private void SetViewBox() {
    string attr = this._attrList.GetValue("viewBox");
    if(attr != "") {
      string[] _temp = SVGStringExtractor.ExtractTransformValue(attr);
      if(_temp.Length == 4) {
        float x = float.Parse(_temp[0], System.Globalization.CultureInfo.InvariantCulture);
        float y = float.Parse(_temp[1], System.Globalization.CultureInfo.InvariantCulture);
        float w = float.Parse(_temp[2], System.Globalization.CultureInfo.InvariantCulture);
        float h = float.Parse(_temp[3], System.Globalization.CultureInfo.InvariantCulture);
        this._viewport = new Rect(x, y, w, h);
      }
    }
  }
  /***********************************************************************************/
  private SVGMatrix _cachedViewBoxTransform = null;
  public SVGMatrix ViewBoxTransform() {
    if(this._cachedViewBoxTransform == null) {

      SVGMatrix matrix = new SVGMatrix();

      float x = 0.0f;
      float y = 0.0f;
      float w = 0.0f;
      float h = 0.0f;

      float attrWidth = this._width.value;
      float attrHeight = this._height.value;

      if(_attrList.GetValue("viewBox") != "") {
        Rect r = this._viewport;
        x += -r.x;
        y += -r.y;
        w = r.width;
        h = r.height;
      } else {
        w = attrWidth;
        h = attrHeight;
      }

      float x_ratio = attrWidth / w;
      float y_ratio = attrHeight / h;

      matrix = matrix.ScaleNonUniform(x_ratio, y_ratio);
      matrix = matrix.Translate(x, y);
      _cachedViewBoxTransform = matrix;
    }
    return this._cachedViewBoxTransform;
  }
}
