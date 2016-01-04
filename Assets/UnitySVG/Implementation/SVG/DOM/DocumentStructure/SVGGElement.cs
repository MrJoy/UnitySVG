using UnityEngine;
using System.Collections.Generic;
using UnitySVG;

public class SVGGElement : SVGTransformable, ISVGDrawable {
  private readonly List<ISVGDrawable> _elementList = new List<ISVGDrawable>();
  private readonly SVGGraphics _render;
  private readonly SVGPaintable _paintable;

  public SVGGElement(SVGParser xmlImp,
                     SVGTransformList inheritTransformList,
                     SVGPaintable inheritPaintable,
                     SVGGraphics render) : base(inheritTransformList) {
    _render = render;
    Dictionary<string, string> attrList = xmlImp.Node.Attributes;
    _paintable = new SVGPaintable(inheritPaintable, attrList);
    currentTransformList = new SVGTransformList(attrList.GetValue("transform"));
    xmlImp.GetElementList(_elementList, _paintable, _render, summaryTransformList);
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
      ISVGDrawable temp = _elementList[i];
      if(temp != null) {
        if(use_color)
          _render.SetColor(_color);
        temp.Render();
      }
    }
  }
}
