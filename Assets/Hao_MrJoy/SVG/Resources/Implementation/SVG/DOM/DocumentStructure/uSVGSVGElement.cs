using System.Collections.Generic;

public class uSVGSVGElement : uSVGTransformable, uISVGDrawable {
  private uSVGLength _width;
  private uSVGLength _height;
  private string _contentScriptType;
  private string _contentStyleType;

  private uSVGRect _viewport;

  private float currentScale;
  private uSVGPoint currentTranslate;
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
              uSVGGraphics _render) : base(inheritTransformList) {
    this._render = _render;
    this._xmlImp = xmlImp;
    this._attrList = this._xmlImp.GetCurrentAttributesList();
    this._paintable = new uSVGPaintable(inheritPaintable, this._attrList);
    this._width = new uSVGLength(_attrList.GetValue("width"));
    this._height = new uSVGLength(_attrList.GetValue("height"));
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
    while(!exitFlag && this._xmlImp.ReadNextTag()) {
      if(this._xmlImp.GetCurrentTagState() == uXMLImp.XMLTagState.CLOSE) {
        exitFlag = true;
        continue;
      }
      string t_name = this._xmlImp.GetCurrentTagName();
      AttributeList t_attrList;
        switch(t_name) {
          case "rect": {
            t_attrList = this._xmlImp.GetCurrentAttributesList();
            uSVGRectElement temp = new uSVGRectElement(  t_attrList,
                                  this.summaryTransformList,
                                  this._paintable,
                                   this._render);
            _elementList.Add(temp);
          break;
          }
          case "line": {
            t_attrList = this._xmlImp.GetCurrentAttributesList();
            uSVGLineElement temp = new uSVGLineElement(  t_attrList,
                                  this.summaryTransformList,
                                  this._paintable,
                                   this._render);
            _elementList.Add(temp);
          break;
          }
          case "circle": {
            t_attrList = this._xmlImp.GetCurrentAttributesList();
            uSVGCircleElement temp = new uSVGCircleElement(  t_attrList,
                                  this.summaryTransformList,
                                  this._paintable,
                                   this._render);
            _elementList.Add(temp);
          break;
          }
          case "ellipse": {
            t_attrList = this._xmlImp.GetCurrentAttributesList();
            uSVGEllipseElement temp = new uSVGEllipseElement(  t_attrList,
                                  this.summaryTransformList,
                                  this._paintable,
                                   this._render);
            _elementList.Add(temp);
          break;
          }
          case "polyline": {
            t_attrList = this._xmlImp.GetCurrentAttributesList();
            uSVGPolylineElement temp = new uSVGPolylineElement(  t_attrList,
                                  this.summaryTransformList,
                                  this._paintable,
                                   this._render);
            _elementList.Add(temp);
          break;
          }
          case "polygon": {
            t_attrList = this._xmlImp.GetCurrentAttributesList();
            uSVGPolygonElement temp = new uSVGPolygonElement(t_attrList,
                                  this.summaryTransformList,
                                  this._paintable,
                                   this._render);
            _elementList.Add(temp);
          break;
          }
          case "path": {
            t_attrList = this._xmlImp.GetCurrentAttributesList();
            uSVGPathElement temp = new uSVGPathElement(  t_attrList,
                                  this.summaryTransformList,
                                  this._paintable,
                                   this._render);
            _elementList.Add(temp);
          break;
          }
          case "svg": {
            _elementList.Add(new uSVGSVGElement(  this._xmlImp,
                                this.summaryTransformList,
                                this._paintable,
                                this._render));
            break;
          }
          case "g": {
            _elementList.Add(new uSVGGElement(  this._xmlImp,
                              this.summaryTransformList,
                              this._paintable,
                              this._render));
            break;
          }
          //--------
          case "linearGradient": {
            t_attrList = this._xmlImp.GetCurrentAttributesList();
            uSVGLinearGradientElement temp = new uSVGLinearGradientElement(this._xmlImp,
                                          t_attrList);
            this._paintable.AppendLinearGradient(temp);
            break;
          }
          //--------
          case "radialGradient": {
            t_attrList = this._xmlImp.GetCurrentAttributesList();
            uSVGRadialGradientElement temp = new uSVGRadialGradientElement(this._xmlImp,
                                          t_attrList);
            this._paintable.AppendRadialGradient(temp);
            break;
          }
          case "defs": {
            GetElementList();
            break;
          }
          case "title": {
            GetElementList();
            break;
          }
          case "desc": {
            GetElementList();
            break;
          }
//          default:
//            UnityEngine.Debug.LogError("Unexpected tag: " + t_name);
//            break;
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