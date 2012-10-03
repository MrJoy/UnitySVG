using System.Collections.Generic;
public enum SVGStrokeLineCapMethod {
  Unknown, Butt, Round, Square
}
public enum SVGStrokeLineJoinMethod {
  Unknown, Miter, Round, Bevel
}

public enum SVGPaintMethod {
  SolidGradientFill,
  LinearGradientFill,
  RadialGradientFill,
  PathDraw,
  NoDraw
}

public class SVGPaintable{

  /***********************************************************************************/
  private SVGColor? _fillColor;
  private SVGColor? _strokeColor;
  private SVGLength _strokeWidth;
  private bool isStrokeWidth = false;
  private SVGStrokeLineCapMethod _strokeLineCap = SVGStrokeLineCapMethod.Unknown;
  private SVGStrokeLineJoinMethod _strokeLineJoin = SVGStrokeLineJoinMethod.Unknown;

  //-----------
  private List<SVGLinearGradientElement> _linearGradList;
  private List<SVGRadialGradientElement> _radialGradList;
  private string _gradientID = "";

  /***********************************************************************************/
  public SVGColor? fillColor {
    get{return this._fillColor;}
  }
  public SVGColor? strokeColor {
    get{
      if(IsStroke())return this._strokeColor;
      else return null;
    }
  }
  public float strokeWidth {
    get{return this._strokeWidth.value;}
  }
  public SVGStrokeLineCapMethod strokeLineCap {
    get{ return this._strokeLineCap;}
  }
  public SVGStrokeLineJoinMethod strokeLineJoin {
    get{ return this._strokeLineJoin;}
  }

  public List<SVGLinearGradientElement> linearGradList {
    get{ return this._linearGradList;}
  }
  public List<SVGRadialGradientElement> radialGradList {
    get{ return this._radialGradList;}
  }

  public string gradientID {
    get{ return this._gradientID;}
  }
  /***********************************************************************************/
  public SVGPaintable() {
    this._fillColor = new SVGColor();
    this._strokeColor = new SVGColor();
    this._strokeWidth = new SVGLength(1);
    this._linearGradList = new List<SVGLinearGradientElement>();
    this._radialGradList = new List<SVGRadialGradientElement>();
  }
  public SVGPaintable(AttributeList attrList) {
    this._linearGradList = new List<SVGLinearGradientElement>();
    this._radialGradList = new List<SVGRadialGradientElement>();
    Initialize(attrList);
  }
  public SVGPaintable(SVGPaintable inheritPaintable, AttributeList attrList) {
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

    if(_strokeLineCap == SVGStrokeLineCapMethod.Unknown) {
      _strokeLineCap = inheritPaintable.strokeLineCap;
    }

    if(_strokeLineJoin == SVGStrokeLineJoinMethod.Unknown) {
      _strokeLineJoin = inheritPaintable.strokeLineJoin;
    }

    if(isStrokeWidth == false)
      this._strokeWidth.NewValueSpecifiedUnits(inheritPaintable.strokeWidth);
  }
  /***********************************************************************************/
  //Khoi tao
  private void Initialize(AttributeList attrList) {
    isStrokeWidth = false;

    if(attrList.GetValue("fill").IndexOf("url") >= 0) {
      _gradientID = SVGStringExtractor.ExtractUrl4Gradient(attrList.GetValue("fill"));
    } else {
      _fillColor = new SVGColor(attrList.GetValue("fill"));
    }
    _strokeColor = new SVGColor(attrList.GetValue("stroke"));

    if(attrList.GetValue("stroke-width") != "") isStrokeWidth = true;
    _strokeWidth = new SVGLength(attrList.GetValue("stroke-width"));


    SetStrokeLineCap(attrList.GetValue("stroke-linecap"));
    SetStrokeLineJoin(attrList.GetValue("stroke-linejoin"));

    if(attrList.GetValue("stroke-width") == "") _strokeWidth.NewValueSpecifiedUnits(1f);
    Style(attrList.GetValue("style"));
    //style="fill: #ffffff; stroke:#000000; stroke-width:0.172"
  }
  /***********************************************************************************/
  //Di Phan Tich Style
  private void Style(string styleString) {
    Dictionary<string, string> _dictionary = new Dictionary<string, string>();
    SVGStringExtractor.ExtractStyleValue(styleString, ref _dictionary);
    if(_dictionary.ContainsKey("fill")) {
      if(_dictionary["fill"].IndexOf("url") >= 0) {
        _gradientID = SVGStringExtractor.ExtractUrl4Gradient(_dictionary["fill"]);
      } else {
        _fillColor = new SVGColor(_dictionary["fill"]);
      }
    }
    if(_dictionary.ContainsKey("stroke")) {
      _strokeColor = new SVGColor(_dictionary["stroke"]);
    }
    if(_dictionary.ContainsKey("stroke-width")) {
      this.isStrokeWidth = true;
      _strokeWidth = new SVGLength(_dictionary["stroke-width"]);
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
      case "butt"  : _strokeLineCap = SVGStrokeLineCapMethod.Butt; break;
      case "round" : _strokeLineCap = SVGStrokeLineCapMethod.Round; break;
      case "square": _strokeLineCap = SVGStrokeLineCapMethod.Square; break;
    }
  }
  private void SetStrokeLineJoin(string lineCapType) {
    switch(lineCapType) {
      case "miter":  _strokeLineJoin = SVGStrokeLineJoinMethod.Miter; break;
      case "round":  _strokeLineJoin = SVGStrokeLineJoinMethod.Round; break;
      case "bevel":  _strokeLineJoin = SVGStrokeLineJoinMethod.Bevel; break;
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
      return(this._fillColor.Value.colorType != SVGColorType.None);
  }
  //-----
  public bool IsFill() {
    if(this._fillColor == null)
      return(IsLinearGradiantFill()|| IsRadialGradiantFill());
    else
      return(this._fillColor.Value.colorType != SVGColorType.None);
  }
  //-----
  //Tuc la Fill hien tai
  public bool IsFillX() {
    if(this._fillColor == null)
      return(IsLinearGradiantFill()|| IsRadialGradiantFill());
    else
      return(this._fillColor.Value.colorType != SVGColorType.Unknown);
  }
  //-----
  public bool IsStroke() {
    if(this._strokeColor == null)return false;
    if((this._strokeColor.Value.colorType == SVGColorType.Unknown)||
     (this._strokeColor.Value.colorType == SVGColorType.None)) {
      return false;
    }
    return true;
  }
  //-----
  public SVGPaintMethod GetPaintType() {
    if(IsLinearGradiantFill()) {
      return SVGPaintMethod.LinearGradientFill;
    }
    if(IsRadialGradiantFill()) {
      return SVGPaintMethod.RadialGradientFill;
    }
    if(IsSolidFill()) {
      return SVGPaintMethod.SolidGradientFill;
    }
    if(IsStroke()) {
      return SVGPaintMethod.PathDraw;
    }

    return SVGPaintMethod.NoDraw;
  }
  //----------------------
  //Cong danh sach cac LinearGradient vao trong Paintable
  public void AppendLinearGradient(SVGLinearGradientElement linearGradElement) {
    this._linearGradList.Add(linearGradElement);
  }
  //----------------------
  //Cong danh sach cac RadialGradient vao trong Paintable
  public void AppendRadialGradient(SVGRadialGradientElement radialGradElement) {
    this._radialGradList.Add(radialGradElement);
  }
  //----------------------
  public SVGLinearGradientBrush GetLinearGradientBrush(SVGGraphicsPath graphicsPath) {
    for(int i=0; i < this._linearGradList.Count; i++) {
      if(this._linearGradList[i].id == this._gradientID) {
        return new SVGLinearGradientBrush(this._linearGradList[i], graphicsPath);
      }
    }
    return null;
  }
  //----------------------
  public SVGRadialGradientBrush GetRadialGradientBrush(SVGGraphicsPath graphicsPath) {
    for(int i=0; i < this._radialGradList.Count; i++) {
      if(this._radialGradList[i].id == this._gradientID) {
        return new SVGRadialGradientBrush(this._radialGradList[i], graphicsPath);
      }
    }
    return null;
  }
}
