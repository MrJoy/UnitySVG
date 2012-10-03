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
    bool exitFlag = false;
    while(!exitFlag && _xmlImp.Next()) {
      if(_xmlImp.Node is BlockCloseNode) {
        exitFlag = true;
        continue;
      }
      switch(_xmlImp.Node.Name) {
        case SVGNodeName.Rect:
          _elementList.Add(new SVGRectElement(_xmlImp.Node.Attributes,
                                               summaryTransformList,
                                               _paintable,
                                               _render));
          break;
        case SVGNodeName.Line:
          _elementList.Add(new SVGLineElement(_xmlImp.Node.Attributes,
                                               summaryTransformList,
                                               _paintable,
                                               _render));
          break;
        case SVGNodeName.Circle:
          _elementList.Add(new SVGCircleElement(_xmlImp.Node.Attributes,
                                                 summaryTransformList,
                                                 _paintable,
                                                 _render));
          break;
        case SVGNodeName.Ellipse:
          _elementList.Add(new SVGEllipseElement(_xmlImp.Node.Attributes,
                                                  summaryTransformList,
                                                  _paintable,
                                                  _render));
          break;
        case SVGNodeName.PolyLine:
          _elementList.Add(new SVGPolylineElement(_xmlImp.Node.Attributes,
                                                   summaryTransformList,
                                                   _paintable,
                                                   _render));
          break;
        case SVGNodeName.Polygon:
          _elementList.Add(new SVGPolygonElement(_xmlImp.Node.Attributes,
                                                  summaryTransformList,
                                                  _paintable,
                                                  _render));
          break;
        case SVGNodeName.Path:
          _elementList.Add(new SVGPathElement(_xmlImp.Node.Attributes,
                                               summaryTransformList,
                                               _paintable,
                                               _render));
          break;
        case SVGNodeName.SVG:
          _elementList.Add(new SVGSVGElement(_xmlImp,
                                             summaryTransformList,
                                             _paintable,
                                             _render));
          break;
        case SVGNodeName.G:
          _elementList.Add(new SVGGElement(_xmlImp,
                                           summaryTransformList,
                                           _paintable,
                                           _render));
          break;
        //--------
        case SVGNodeName.LinearGradient:
          _paintable.AppendLinearGradient(new SVGLinearGradientElement(_xmlImp, _xmlImp.Node.Attributes));
          break;
        //--------
        case SVGNodeName.RadialGradient:
          _paintable.AppendRadialGradient(new SVGRadialGradientElement(_xmlImp, _xmlImp.Node.Attributes));
          break;
        case SVGNodeName.Defs:
          GetElementList();
          break;
        case SVGNodeName.Title:
          GetElementList();
          break;
        case SVGNodeName.Desc:
          GetElementList();
          break;
//          default:
//            UnityEngine.Debug.LogError("Unexpected tag: " + t_name);
//            break;
      }
    }
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
