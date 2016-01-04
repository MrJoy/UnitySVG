using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnitySVG;

public class SVGSVGElement : SVGTransformable, ISVGDrawable {
  private SVGLength _width, _height;
  private Rect _viewport;
  private readonly Dictionary<string, string> _attrList;
  private readonly List<ISVGDrawable> _elementList = new List<ISVGDrawable>();
  private readonly SVGGraphics _render;

  public SVGSVGElement(SVGParser xmlImp,
                       SVGTransformList inheritTransformList,
                       SVGPaintable inheritPaintable,
                       SVGGraphics r) : base(inheritTransformList) {
    _render = r;
    _attrList = xmlImp.Node.Attributes;
    var paintable = new SVGPaintable(inheritPaintable, _attrList);
    _width = new SVGLength(_attrList.GetValue("width"));
    _height = new SVGLength(_attrList.GetValue("height"));

    SetViewBox();

    ViewBoxTransform();

    SVGTransform temp = new SVGTransform(_cachedViewBoxTransform);
    SVGTransformList t_currentTransformList = new SVGTransformList();
    t_currentTransformList.AppendItem(temp);
    currentTransformList = t_currentTransformList; // use setter only once, since it also updates other lists

    xmlImp.GetElementList(_elementList, paintable, _render, summaryTransformList);
  }

  public void BeforeRender(SVGTransformList transformList) {
    inheritTransformList = transformList;

    for(int i = 0; i < _elementList.Count; ++i) {
      ISVGDrawable temp = _elementList[i];
      if(temp != null)
        temp.BeforeRender(summaryTransformList);
    }
  }

  public void Render() {
    _render.Size = new Vector2(_width.value, _height.value);
    for(int i = 0; i < _elementList.Count; ++i) {
      ISVGDrawable temp = _elementList[i];
      if(temp != null)
        temp.Render();
    }
  }

  private void SetViewBox() {
    string attr = _attrList.GetValue("viewBox");
    if(!string.IsNullOrEmpty(attr)) {
      string[] _temp = SVGStringExtractor.ExtractTransformValue(attr);
      if(_temp.Length == 4) {
        float x = float.Parse(_temp[0], CultureInfo.InvariantCulture);
        float y = float.Parse(_temp[1], CultureInfo.InvariantCulture);
        float w = float.Parse(_temp[2], CultureInfo.InvariantCulture);
        float h = float.Parse(_temp[3], CultureInfo.InvariantCulture);
        _viewport = new Rect(x, y, w, h);
      }
    }
  }

  private Matrix2x3 _cachedViewBoxTransform;

  public Matrix2x3 ViewBoxTransform() {
    if(_cachedViewBoxTransform == null) {
      Matrix2x3 matrix = new Matrix2x3();

      float x = 0.0f, y = 0.0f, w, h, attrWidth = _width.value, attrHeight = _height.value;

      if(!string.IsNullOrEmpty(_attrList.GetValue("viewBox"))) {
        Rect r = _viewport;
        x += -r.x;
        y += -r.y;
        w = r.width;
        h = r.height;
      } else {
        w = attrWidth;
        h = attrHeight;
      }

      float x_ratio = attrWidth / w, y_ratio = attrHeight / h;

      matrix = matrix.ScaleNonUniform(x_ratio, y_ratio);
      matrix = matrix.Translate(x, y);
      _cachedViewBoxTransform = matrix;
    }
    return _cachedViewBoxTransform;
  }
}
