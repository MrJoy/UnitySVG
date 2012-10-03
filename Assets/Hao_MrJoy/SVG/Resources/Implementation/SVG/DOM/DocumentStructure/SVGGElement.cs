using UnityEngine;
using System.Collections.Generic;

public class SVGGElement : SVGTransformable, ISVGDrawable {
  //-------------------------------
  private AttributeList _attrList;
  private List<object> _elementList = null;
  private SVGParser _xmlImp;
  //-------------------------------
  private SVGGraphics _render;
  //-------------------------------
  private SVGPaintable _paintable;
  /***********************************************************************************/
  public SVGGElement(SVGParser xmlImp,
                      SVGTransformList inheritTransformList,
                      SVGPaintable inheritPaintable,
                      SVGGraphics render) : base(inheritTransformList) {
    _render = render;
    _xmlImp = xmlImp;
    _attrList = _xmlImp.Node.Attributes;
    _paintable = new SVGPaintable(inheritPaintable, _attrList);
    _elementList = new List<object>();
    currentTransformList = new SVGTransformList(_attrList.GetValue("transform"));
    GetElementList();
  }
  /***********************************************************************************/
  private void GetElementList() {
    _xmlImp.GetElementList(_elementList, _paintable, _render, summaryTransformList);
  }
  /***********************************************************************************/
  public void BeforeRender(SVGTransformList transformList) {
    inheritTransformList = transformList;
    for(int i = 0; i < _elementList.Count; i++) {
      ISVGDrawable temp = _elementList[i] as ISVGDrawable;
      if(temp != null) temp.BeforeRender(summaryTransformList);
    }
  }
  public void Render() {
    Color _color = Color.black;
    bool use_color = false;
    if(_paintable.IsFill() && !_paintable.IsLinearGradiantFill()) {
      _color = _paintable.fillColor.Value.color;
      use_color = true;
    } else if(_paintable.strokeColor != null) {
      _color = _paintable.strokeColor.Value.color;
      use_color = true;
    }

    for(int i = 0; i < _elementList.Count; i++) {
      ISVGDrawable temp = _elementList[i] as ISVGDrawable;
      if(temp != null) {
        if(use_color)
          _render.SetColor(_color);
        temp.Render();
      }
    }
  }
}
