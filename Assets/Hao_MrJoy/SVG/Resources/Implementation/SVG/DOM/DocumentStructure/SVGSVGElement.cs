using System.Collections.Generic;

public class SVGSVGElement : SVGTransformable, ISVGDrawable {
  private SVGLength _width;
  private SVGLength _height;

  private SVGRect _viewport;

  //-------------------------------
  private AttributeList _attrList;
  private List<object> _elementList;
  private uXMLImp _xmlImp;
  //-------------------------------
  private SVGPaintable _paintable;
  //-------------------------------
  private SVGGraphics _render;

  /***********************************************************************************/
  public SVGSVGElement(  uXMLImp xmlImp,
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
          _elementList.Add(new SVGPathElement(_xmlImp.Node.Attributes,
                                               summaryTransformList,
                                               _paintable,
                                               _render));
          break;
        case "svg":
          _elementList.Add(new SVGSVGElement(_xmlImp,
                           summaryTransformList,
                           _paintable,
                           _render));
          break;
        case "g":
          _elementList.Add(new SVGGElement(_xmlImp,
                           summaryTransformList,
                           _paintable,
                           _render));
          break;
        //--------
        case "linearGradient":
          _paintable.AppendLinearGradient(new SVGLinearGradientElement(_xmlImp, _xmlImp.Node.Attributes));
          break;
        //--------
        case "radialGradient":
          _paintable.AppendRadialGradient(new SVGRadialGradientElement(_xmlImp, _xmlImp.Node.Attributes));
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
      }
    }
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
    this._render.SetSize(this._width.value, this._height.value);
    for(int i = 0; i < _elementList.Count; i++) {
      ISVGDrawable temp = _elementList[i] as ISVGDrawable;
      if(temp != null) {
        temp.Render();
      }
    }
  }
  /***********************************************************************************/
  private void SetViewBox() {
    string attr = this._attrList.GetValue("viewBox");
    if(attr != "") {
      string[] _temp = SVGStringExtractor.ExtractTransformValue(attr);
      if(_temp.Length == 4) {
        float x = SVGNumber.ParseToFloat(_temp[0]);
        float y = SVGNumber.ParseToFloat(_temp[1]);
        float w = SVGNumber.ParseToFloat(_temp[2]);
        float h = SVGNumber.ParseToFloat(_temp[3]);
        this._viewport = new SVGRect(x, y, w, h);
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
        SVGRect r = this._viewport;
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