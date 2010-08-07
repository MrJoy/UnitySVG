using UnityEngine;
using System.Collections.Generic;

public class uSVGGElement : uSVGTransformable, uISVGDrawable {
  //-------------------------------
  private AttributeList _attrList;
  private List<object> _elementList = null;
  private uXMLImp _xmlImp;
  //-------------------------------
  private uSVGGraphics _render;
  //-------------------------------
  private uSVGPaintable _paintable;
  /***********************************************************************************/
  public uSVGGElement(uXMLImp xmlImp,
                      uSVGTransformList inheritTransformList,
                      uSVGPaintable inheritPaintable,
                      uSVGGraphics render) : base(inheritTransformList) {
    _render = render;
    _xmlImp = xmlImp;
    _attrList = _xmlImp.GetCurrentAttributesList();
    _paintable = new uSVGPaintable(inheritPaintable, _attrList);
    _elementList = new List<object>();
    currentTransformList = new uSVGTransformList(_attrList.GetValue("TRANSFORM"));
    GetElementList();
  }
  /***********************************************************************************/
  private void GetElementList() {
    bool exitFlag = false;
    while(!exitFlag && _xmlImp.ReadNextTag()) {
      if(_xmlImp.GetCurrentTagState() == uXMLImp.XMLTagState.CLOSE) {
        exitFlag = true;
        continue;
      }
      switch(_xmlImp.GetCurrentTagName().ToUpper()) {
        case "RECT":
          _elementList.Add(new uSVGRectElement(_xmlImp.GetCurrentAttributesList(),
                                               summaryTransformList,
                                               _paintable,
                                               _render));
          break;
        case "LINE":
          _elementList.Add(new uSVGLineElement(_xmlImp.GetCurrentAttributesList(),
                                               summaryTransformList,
                                               _paintable,
                                               _render));
          break;
        case "CIRCLE":
          _elementList.Add(new uSVGCircleElement(_xmlImp.GetCurrentAttributesList(),
                                                 summaryTransformList,
                                                 _paintable,
                                                 _render));
          break;
        case "ELLIPSE":
          _elementList.Add(new uSVGEllipseElement(_xmlImp.GetCurrentAttributesList(),
                                                  summaryTransformList,
                                                  _paintable,
                                                  _render));
          break;
        case "POLYLINE":
          _elementList.Add(new uSVGPolylineElement(_xmlImp.GetCurrentAttributesList(),
                                                   summaryTransformList,
                                                   _paintable,
                                                   _render));
          break;
        case "POLYGON":
          _elementList.Add(new uSVGPolygonElement(_xmlImp.GetCurrentAttributesList(),
                                                  summaryTransformList,
                                                  _paintable,
                                                  _render));
          break;
        case "PATH":
          _elementList.Add(new uSVGPathElement(_xmlImp.GetCurrentAttributesList(),
                                               summaryTransformList,
                                               _paintable,
                                               _render));
          break;
        case "SVG":
          _elementList.Add(new uSVGSVGElement(_xmlImp,
                           summaryTransformList,
                           _paintable,
                           _render));
          break;
        case "G":
          _elementList.Add(new uSVGGElement(_xmlImp,
                           summaryTransformList,
                           _paintable,
                           _render));
          break;
        //--------
        case "LINEARGRADIENT":
          _paintable.AppendLinearGradient(new uSVGLinearGradientElement(_xmlImp, _xmlImp.GetCurrentAttributesList()));
          break;
        //--------
        case "RADIALGRADIENT":
          _paintable.AppendRadialGradient(new uSVGRadialGradientElement(_xmlImp, _xmlImp.GetCurrentAttributesList()));
          break;
        case "DEFS":
          GetElementList();
          break;
        case "TITLE":
          GetElementList();
          break;
        case "DESC":
          GetElementList();
          break;
//          default:
//            UnityEngine.Debug.LogError("Unexpected tag: " + t_name);
//            break;
      }
    }
  }
  /***********************************************************************************/
  public void BeforeRender(uSVGTransformList transformList) {
    inheritTransformList = transformList;
    for(int i = 0; i < _elementList.Count; i++) {
      uISVGDrawable temp = _elementList[i] as uISVGDrawable;
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
      uISVGDrawable temp = _elementList[i] as uISVGDrawable;
      if(temp != null) {
        if(use_color)
          _render.SetColor(_color);
        temp.Render();
      }
    }
  }
}
