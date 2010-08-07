using System.Collections.Generic;

public class uSVGSVGElement : uSVGTransformable, uISVGDrawable {
  private uSVGLength _width;
  private uSVGLength _height;

  private uSVGRect _viewport;

  //-------------------------------
  private AttributeList _attrList;
  private List<object> _elementList;
  private uXMLImp _xmlImp;
  //-------------------------------
  private uSVGPaintable _paintable;
  //-------------------------------
  private uSVGGraphics _render;

  /***********************************************************************************/
  public uSVGSVGElement(  uXMLImp xmlImp,
              uSVGTransformList inheritTransformList,
              uSVGPaintable inheritPaintable,
              uSVGGraphics r) : base(inheritTransformList) {
    _render = r;
    _xmlImp = xmlImp;
    _attrList = _xmlImp.Node.Attributes;
    _paintable = new uSVGPaintable(inheritPaintable, _attrList);
    _width = new uSVGLength(_attrList.GetValue("width"));
    _height = new uSVGLength(_attrList.GetValue("height"));
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
    uSVGTransform temp = CreateSVGTransformFromMatrix(_cachedViewBoxTransform);
    uSVGTransformList t_currentTransformList = new uSVGTransformList();
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
          _elementList.Add(new uSVGRectElement(_xmlImp.Node.Attributes,
                           summaryTransformList,
                           _paintable,
                           _render));
          break;
        case "line":
          _elementList.Add(new uSVGLineElement(_xmlImp.Node.Attributes,
                           summaryTransformList,
                           _paintable,
                           _render));
          break;
        case "circle":
          _elementList.Add(new uSVGCircleElement(_xmlImp.Node.Attributes,
                           summaryTransformList,
                           _paintable,
                           _render));
          break;
        case "ellipse":
          _elementList.Add(new uSVGEllipseElement(_xmlImp.Node.Attributes,
                           summaryTransformList,
                           _paintable,
                           _render));
          break;
        case "polyline":
          _elementList.Add(new uSVGPolylineElement(_xmlImp.Node.Attributes,
                           summaryTransformList,
                           _paintable,
                           _render));
          break;
        case "polygon":
          _elementList.Add(new uSVGPolygonElement(_xmlImp.Node.Attributes,
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
      }
    }
  }
  /***********************************************************************************/
  public void BeforeRender(uSVGTransformList transformList) {

    this.inheritTransformList = transformList;

    for(int i = 0; i < _elementList.Count; i++) {
      uISVGDrawable temp = _elementList[i] as uISVGDrawable;
      if(temp != null) {
        temp.BeforeRender(this.summaryTransformList);
      }
    }
  }

  public void Render() {
    this._render.SetSize(this._width.value, this._height.value);
    for(int i = 0; i < _elementList.Count; i++) {
      uISVGDrawable temp = _elementList[i] as uISVGDrawable;
      if(temp != null) {
        temp.Render();
      }
    }
  }
  /***********************************************************************************/
  private void SetViewBox() {
    string attr = this._attrList.GetValue("viewBox");
    if(attr != "") {
      string[] _temp = uSVGStringExtractor.ExtractTransformValue(attr);
      if(_temp.Length == 4) {
        float x = uSVGNumber.ParseToFloat(_temp[0]);
        float y = uSVGNumber.ParseToFloat(_temp[1]);
        float w = uSVGNumber.ParseToFloat(_temp[2]);
        float h = uSVGNumber.ParseToFloat(_temp[3]);
        this._viewport = new uSVGRect(x, y, w, h);
      }
    }
  }
  /***********************************************************************************/
  public uSVGNumber CreateSVGNumber() {
    return new uSVGNumber(0.0f);
  }

  public uSVGLength CreateSVGLength() {
    return new uSVGLength(0, 0.0f);
  }

  public uSVGPoint CreateSVGPoint() {
    return new uSVGPoint(0.0f, 0.0f);
  }
  public uSVGMatrix CreateSVGMatrix() {
    return new uSVGMatrix();
  }

  public uSVGRect CreateSVGRect() {
    return new uSVGRect(0.0f, 0.0f, 0.0f, 0.0f);
  }

  public uSVGTransform CreateSVGTransform() {
    return new uSVGTransform();
  }

  public uSVGTransform CreateSVGTransformFromMatrix(uSVGMatrix matrix) {
    return new uSVGTransform(matrix);
  }
  /***********************************************************************************/
  private uSVGMatrix _cachedViewBoxTransform = null;
  public uSVGMatrix ViewBoxTransform() {
    if(this._cachedViewBoxTransform == null) {

      uSVGMatrix matrix = CreateSVGMatrix();

      float x = 0.0f;
      float y = 0.0f;
      float w = 0.0f;
      float h = 0.0f;

      float attrWidth = this._width.value;
      float attrHeight = this._height.value;

      if(_attrList.GetValue("viewBox") != "") {
        uSVGRect r = this._viewport;
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