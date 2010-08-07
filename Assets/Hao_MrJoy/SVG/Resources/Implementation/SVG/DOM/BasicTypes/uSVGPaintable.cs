using System.Collections.Generic;
public enum uSVGStrokeLineCapTypes {
  UNKNOWN, BUTT, ROUND, SQUARE
}
public enum uSVGStrokeLineJoinTypes {
  UNKNOWN, MITER, ROUND, BEVEL
}

public enum uSVGPaintTypes {
  SVG_PAINT_SOLID_GRADIENT_FILL,
  SVG_PAINT_LINEAR_GRADIENT_FILL,
  SVG_PAINT_RADIAL_GRADIENT_FILL,
  SVG_PAINT_PATH_DRAW,
  SVG_NODRAW
}

public class uSVGPaintable{

  /***********************************************************************************/
  private uSVGColor? _fillColor;
  private uSVGColor? _strokeColor;
  private uSVGLength _strokeWidth;
  private bool isStrokeWidth = false;
  private uSVGStrokeLineCapTypes _strokeLineCap = uSVGStrokeLineCapTypes.UNKNOWN;
  private uSVGStrokeLineJoinTypes _strokeLineJoin = uSVGStrokeLineJoinTypes.UNKNOWN;

  //-----------
  private List<uSVGLinearGradientElement> _linearGradList;
  private List<uSVGRadialGradientElement> _radialGradList;
  private string _gradientID = "";

  /***********************************************************************************/
  public uSVGColor? fillColor {
    get{return this._fillColor;}
  }
  public uSVGColor? strokeColor {
    get{
      if(IsStroke())return this._strokeColor;
      else return null;
    }
  }
  public float strokeWidth {
    get{return this._strokeWidth.value;}
  }
  public uSVGStrokeLineCapTypes strokeLineCap {
    get{ return this._strokeLineCap;}
  }
  public uSVGStrokeLineJoinTypes strokeLineJoin {
    get{ return this._strokeLineJoin;}
  }

  public List<uSVGLinearGradientElement> linearGradList {
    get{ return this._linearGradList;}
  }
  public List<uSVGRadialGradientElement> radialGradList {
    get{ return this._radialGradList;}
  }

  public string gradientID {
    get{ return this._gradientID;}
  }
  /***********************************************************************************/
  public uSVGPaintable() {
    this._fillColor = new uSVGColor();
    this._strokeColor = new uSVGColor();
    this._strokeWidth = new uSVGLength("1");
    this._linearGradList = new List<uSVGLinearGradientElement>();
    this._radialGradList = new List<uSVGRadialGradientElement>();
  }
  public uSVGPaintable(AttributeList attrList) {
    this._linearGradList = new List<uSVGLinearGradientElement>();
    this._radialGradList = new List<uSVGRadialGradientElement>();
    Initialize(attrList);
  }
  public uSVGPaintable(uSVGPaintable inheritPaintable, AttributeList attrList) {
    this._linearGradList = inheritPaintable.linearGradList;
    this._radialGradList = inheritPaintable.radialGradList;;
    Initialize(attrList);

    if(IsFillX() == false) {
      if(inheritPaintable.IsLinearGradiantFill()) {
        this._gradientID = inheritPaintable.gradientID;
      } else if(inheritPaintable.IsRadialGradiantFill()) {
        this._gradientID = inheritPaintable.gradientID;
      } else this._fillColor = inheritPaintable.fillColor;
    }
    if(!IsStroke()&& inheritPaintable.IsStroke()) {
      this._strokeColor = inheritPaintable.strokeColor;
    }

    if(_strokeLineCap == uSVGStrokeLineCapTypes.UNKNOWN) {
      _strokeLineCap = inheritPaintable.strokeLineCap;
    }

    if(_strokeLineJoin == uSVGStrokeLineJoinTypes.UNKNOWN) {
      _strokeLineJoin = inheritPaintable.strokeLineJoin;
    }

    if(isStrokeWidth == false)
      this._strokeWidth.NewValueSpecifiedUnits(inheritPaintable.strokeWidth);
  }
  /***********************************************************************************/
  //Khoi tao
  private void Initialize(AttributeList attrList) {
    isStrokeWidth = false;

    if(attrList.GetValue("FILL").IndexOf("url") >= 0) {
      _gradientID = uSVGStringExtractor.ExtractUrl4Gradient(attrList.GetValue("FILL"));
    } else {
      _fillColor = new uSVGColor(attrList.GetValue("FILL"));
    }
    _strokeColor = new uSVGColor(attrList.GetValue("STROKE"));

    if(attrList.GetValue("STROKE-WIDTH") != "") {
      this.isStrokeWidth = true;
    }
    _strokeWidth = new uSVGLength(attrList.GetValue("STROKE-WIDTH"));


    SetStrokeLineCap(attrList.GetValue("STROKE-LINECAP"));
    SetStrokeLineJoin(attrList.GetValue("STROKE-LINEJOIN"));

    if(attrList.GetValue("STROKE-WIDTH") == "")this._strokeWidth.NewValueSpecifiedUnits(1f);
    Style(attrList.GetValue("STYLE"));
    //style="fill: #ffffff; stroke:#000000; stroke-width:0.172"
  }
  /***********************************************************************************/
  //Di Phan Tich Style
  private void Style(string styleString) {
    Dictionary<string, string> _dictionary = new Dictionary<string, string>();
    uSVGStringExtractor.ExtractStyleValue(styleString, ref _dictionary);
    if(_dictionary.ContainsKey("fill")) {
      if(_dictionary["fill"].IndexOf("url") >= 0) {
        _gradientID = uSVGStringExtractor.ExtractUrl4Gradient(_dictionary["fill"]);
      } else {
        _fillColor = new uSVGColor(_dictionary["fill"]);
      }
    }
    if(_dictionary.ContainsKey("stroke")) {
      _strokeColor = new uSVGColor(_dictionary["stroke"]);
    }
    if(_dictionary.ContainsKey("stroke-width")) {
      this.isStrokeWidth = true;
      _strokeWidth = new uSVGLength(_dictionary["stroke-width"]);
    }

    if(_dictionary.ContainsKey("stroke-linecap")) {
      SetStrokeLineCap(_dictionary["stroke-linecap"]);
    }
    if(_dictionary.ContainsKey("stroke-linejoin")) {
      SetStrokeLineJoin(_dictionary["stroke-linejoin"]);
    }
  }
  /***********************************************************************************/
  private void SetStrokeLineCap(string lineCapType) {
    switch(lineCapType) {
      case "butt" :   _strokeLineCap = uSVGStrokeLineCapTypes.BUTT;  break;
      case "round" :  _strokeLineCap = uSVGStrokeLineCapTypes.ROUND;  break;
      case "square" : _strokeLineCap = uSVGStrokeLineCapTypes.SQUARE;break;
    }
  }
  private void SetStrokeLineJoin(string lineCapType) {
    switch(lineCapType) {
      case "miter" :   _strokeLineJoin = uSVGStrokeLineJoinTypes.MITER;  break;
      case "round" :  _strokeLineJoin = uSVGStrokeLineJoinTypes.ROUND;  break;
      case "bevel" :   _strokeLineJoin = uSVGStrokeLineJoinTypes.BEVEL;  break;
    }
  }
  /***********************************************************************************/
  public bool IsLinearGradiantFill() {
    if(this._gradientID == "") {
      return false;
    }
    bool flag = false;
    for(int i=0; i < this._linearGradList.Count; i++) {
      if(this._linearGradList[i].id == this._gradientID) {
        flag = true;
        break;
      }
    }
    return flag;
  }
  //-----
  public bool IsRadialGradiantFill() {
    if(this._gradientID == "") {
      return false;
    }
    bool flag = false;
    for(int i=0; i < this._radialGradList.Count; i++) {
      if(this._radialGradList[i].id == this._gradientID) {
        flag = true;
        break;
      }
    }
    return flag;
  }
  //-----
  public bool IsSolidFill() {
    if(this._fillColor == null)
      return false;
    else
      return(this._fillColor.Value.colorType != SVGColorType.SVG_COLORTYPE_NONE);
  }
  //-----
  public bool IsFill() {
    if(this._fillColor == null)
      return(IsLinearGradiantFill()|| IsRadialGradiantFill());
    else
      return(this._fillColor.Value.colorType != SVGColorType.SVG_COLORTYPE_NONE);
  }
  //-----
  //Tuc la Fill hien tai
  public bool IsFillX() {
    if(this._fillColor == null)
      return(IsLinearGradiantFill()|| IsRadialGradiantFill());
    else
      return(this._fillColor.Value.colorType != SVGColorType.SVG_COLORTYPE_UNKNOWN);
  }
  //-----
  public bool IsStroke() {
    if(this._strokeColor == null)return false;
    if((this._strokeColor.Value.colorType == SVGColorType.SVG_COLORTYPE_UNKNOWN)||
     (this._strokeColor.Value.colorType == SVGColorType.SVG_COLORTYPE_NONE)) {
      return false;
    }
    return true;
  }
  //-----
  public uSVGPaintTypes GetPaintType() {
    if(IsLinearGradiantFill()) {
      return uSVGPaintTypes.SVG_PAINT_LINEAR_GRADIENT_FILL;
    }
    if(IsRadialGradiantFill()) {
      return uSVGPaintTypes.SVG_PAINT_RADIAL_GRADIENT_FILL;
    }
    if(IsSolidFill()) {
      return uSVGPaintTypes.SVG_PAINT_SOLID_GRADIENT_FILL;
    }
    if(IsStroke()) {
      return uSVGPaintTypes.SVG_PAINT_PATH_DRAW;
    }

    return uSVGPaintTypes.SVG_NODRAW;
  }
  //----------------------
  //Cong danh sach cac LinearGradient vao trong Paintable
  public void AppendLinearGradient(uSVGLinearGradientElement linearGradElement) {
    this._linearGradList.Add(linearGradElement);
  }
  //----------------------
  //Cong danh sach cac RadialGradient vao trong Paintable
  public void AppendRadialGradient(uSVGRadialGradientElement radialGradElement) {
    this._radialGradList.Add(radialGradElement);
  }
  //----------------------
  public uSVGLinearGradientBrush GetLinearGradientBrush(uSVGGraphicsPath graphicsPath) {
    for(int i=0; i < this._linearGradList.Count; i++) {
      if(this._linearGradList[i].id == this._gradientID) {
        return new uSVGLinearGradientBrush(this._linearGradList[i], graphicsPath);
      }
    }
    return null;
  }
  //----------------------
  public uSVGRadialGradientBrush GetRadialGradientBrush(uSVGGraphicsPath graphicsPath) {
    for(int i=0; i < this._radialGradList.Count; i++) {
      if(this._radialGradList[i].id == this._gradientID) {
        return new uSVGRadialGradientBrush(this._radialGradList[i], graphicsPath);
      }
    }
    return null;
  }
}