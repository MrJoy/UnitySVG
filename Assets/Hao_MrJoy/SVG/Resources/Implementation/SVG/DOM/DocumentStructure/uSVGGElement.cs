using UnityEngine;
using System.Collections.Generic;

public class uSVGGElement : uSVGTransformable, uISVGDrawable {
  //-------------------------------
  private AttributeList _attrList;
  private List<object> _elementList = null;
  private uXMLImp _xmlImp;
  //-------------------------------
  private SVGGraphics _render;
  //-------------------------------
  private uSVGPaintable _paintable;
  /***********************************************************************************/
  public uSVGGElement(uXMLImp xmlImp,
                      uSVGTransformList inheritTransformList,
                      uSVGPaintable inheritPaintable,
                      SVGGraphics render) : base(inheritTransformList) {
    _render = render;
    _xmlImp = xmlImp;
    _attrList = _xmlImp.Node.Attributes;
    _paintable = new uSVGPaintable(inheritPaintable, _attrList);
    _elementList = new List<object>();
    currentTransformList = new uSVGTransformList(_attrList.GetValue("transform"));
    GetElementList();
  }
  /***********************************************************************************/
  private void GetElementList() {
    bool exitFlag = false;
    while(!exitFlag && _xmlImp.Next()) {
      if(_xmlImp.Node.Kind == NodeKind.BlockClose) {
        exitFlag = true;
        continue;
      }
      switch(_xmlImp.Node.Name) {
        case "rect":
          _elementList.Add(new SVGRectElement(_xmlImp.Node.Attributes,
                                               summaryTransformList,
                                               _paintable,
                                               _render));
          break;
        case "line":
          _elementList.Add(new SVGLineElement(_xmlImp.Node.Attributes,
                                               summaryTransformList,
                                               _paintable,
                                               _render));
          break;
        case "circle":
          _elementList.Add(new SVGCircleElement(_xmlImp.Node.Attributes,
                                                 summaryTransformList,
                                                 _paintable,
                                                 _render));
          break;
        case "ellipse":
          _elementList.Add(new SVGEllipseElement(_xmlImp.Node.Attributes,
                                                  summaryTransformList,
                                                  _paintable,
                                                  _render));
          break;
        case "polyline":
          _elementList.Add(new SVGPolylineElement(_xmlImp.Node.Attributes,
                                                   summaryTransformList,
                                                   _paintable,
                                                   _render));
          break;
        case "polygon":
          _elementList.Add(new SVGPolygonElement(_xmlImp.Node.Attributes,
                                                  summaryTransformList,
                                                  _paintable,
                                                  _render));
          break;
        case "path":
          _elementList.Add(new uSVGPathElement(_xmlImp.Node.Attributes,
                                               summaryTransformList,
                                               _paintable,
                                               _render));
          break;
        case "svg":
          _elementList.Add(new uSVGSVGElement(_xmlImp,
                           summaryTransformList,
                           _paintable,
                           _render));
          break;
        case "g":
          _elementList.Add(new uSVGGElement(_xmlImp,
                           summaryTransformList,
                           _paintable,
                           _render));
          break;
        //--------
        case "linearGradient":
          _paintable.AppendLinearGradient(new uSVGLinearGradientElement(_xmlImp, _xmlImp.Node.Attributes));
          break;
        //--------
        case "radialGradient":
          _paintable.AppendRadialGradient(new uSVGRadialGradientElement(_xmlImp, _xmlImp.Node.Attributes));
          break;
        case "defs":
          GetElementList();
          break;
        case "title":
          GetElementList();
          break;
        case "desc":
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
