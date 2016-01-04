using System.Collections.Generic;
using UnitySVG;

public enum SVGStrokeLineCapMethod {
  Unknown,
  Butt,
  Round,
  Square
}

public enum SVGStrokeLineJoinMethod {
  Unknown,
  Miter,
  Round,
  Bevel
}

public enum SVGPaintMethod {
  SolidGradientFill,
  LinearGradientFill,
  RadialGradientFill,
  PathDraw,
  NoDraw
}

public class SVGPaintable {
  private SVGColor? _fillColor;
  private SVGColor? _strokeColor;
  private SVGLength _strokeWidth;
  private bool isStrokeWidth;
  private SVGStrokeLineCapMethod _strokeLineCap = SVGStrokeLineCapMethod.Unknown;
  private SVGStrokeLineJoinMethod _strokeLineJoin = SVGStrokeLineJoinMethod.Unknown;

  private readonly List<SVGLinearGradientElement> _linearGradList;
  private readonly List<SVGRadialGradientElement> _radialGradList;
  private string _gradientID = "";

  public SVGColor? fillColor { get { return _fillColor; } }

  public SVGColor? strokeColor { get { return IsStroke() ? _strokeColor : null; } } // *NOPAD*

  public float strokeWidth { get { return _strokeWidth.value; } }

  public SVGStrokeLineCapMethod strokeLineCap { get { return _strokeLineCap; } }

  public SVGStrokeLineJoinMethod strokeLineJoin { get { return _strokeLineJoin; } }

  public List<SVGLinearGradientElement> linearGradList { get { return _linearGradList; } }

  public List<SVGRadialGradientElement> radialGradList { get { return _radialGradList; } }

  public string gradientID { get { return _gradientID; } }

  public SVGPaintable() {
    _fillColor = new SVGColor();
    _strokeColor = new SVGColor();
    _strokeWidth = new SVGLength(1);
    _linearGradList = new List<SVGLinearGradientElement>();
    _radialGradList = new List<SVGRadialGradientElement>();
  }

  public SVGPaintable(Dictionary<string, string> attrList) {
    _linearGradList = new List<SVGLinearGradientElement>();
    _radialGradList = new List<SVGRadialGradientElement>();
    Initialize(attrList);
  }

  public SVGPaintable(SVGPaintable inheritPaintable, Dictionary<string, string> attrList) {
    _linearGradList = inheritPaintable.linearGradList;
    _radialGradList = inheritPaintable.radialGradList;
    Initialize(attrList);

    if(IsFillX() == false) {
      if(inheritPaintable.IsLinearGradiantFill())
        _gradientID = inheritPaintable.gradientID;
      else if(inheritPaintable.IsRadialGradiantFill())
        _gradientID = inheritPaintable.gradientID;
      else
        _fillColor = inheritPaintable.fillColor;
    }
    if(!IsStroke() && inheritPaintable.IsStroke())
      _strokeColor = inheritPaintable.strokeColor;

    if(_strokeLineCap == SVGStrokeLineCapMethod.Unknown)
      _strokeLineCap = inheritPaintable.strokeLineCap;

    if(_strokeLineJoin == SVGStrokeLineJoinMethod.Unknown)
      _strokeLineJoin = inheritPaintable.strokeLineJoin;

    if(isStrokeWidth == false)
      _strokeWidth.NewValueSpecifiedUnits(inheritPaintable.strokeWidth);
  }

  private void Initialize(Dictionary<string, string> attrList) {
    isStrokeWidth = false;

    if(attrList.ContainsKey("fill")) {
      string fill = attrList["fill"];
      if(fill.Contains("url"))
        _gradientID = SVGStringExtractor.ExtractUrl4Gradient(fill);
      else
        _fillColor = new SVGColor(fill);
    }

    _strokeColor = new SVGColor(attrList.GetValue("stroke"));

    if(attrList.ContainsKey("stroke-width")) {
      isStrokeWidth = true;
      _strokeWidth = new SVGLength(attrList["stroke-width"]);
    }


    SetStrokeLineCap(attrList.GetValue("stroke-linecap"));
    SetStrokeLineJoin(attrList.GetValue("stroke-linejoin"));

    if(!attrList.ContainsKey("stroke-width"))
      _strokeWidth.NewValueSpecifiedUnits(1f);
    SetStyle(attrList.GetValue("style"));
  }

  private void SetStyle(string styleString) {
    Dictionary<string, string> _dictionary = new Dictionary<string, string>();
    SVGStringExtractor.ExtractStyleValue(styleString, ref _dictionary);
    if(_dictionary.ContainsKey("fill")) {
      string fill = _dictionary["fill"];
      if(fill.Contains("url"))
        _gradientID = SVGStringExtractor.ExtractUrl4Gradient(fill);
      else
        _fillColor = new SVGColor(fill);
    }
    if(_dictionary.ContainsKey("stroke"))
      _strokeColor = new SVGColor(_dictionary["stroke"]);
    if(_dictionary.ContainsKey("stroke-width")) {
      isStrokeWidth = true;
      _strokeWidth = new SVGLength(_dictionary["stroke-width"]);
    }
    if(_dictionary.ContainsKey("stroke-linecap"))
      SetStrokeLineCap(_dictionary["stroke-linecap"]);
    if(_dictionary.ContainsKey("stroke-linejoin"))
      SetStrokeLineJoin(_dictionary["stroke-linejoin"]);
  }

  private void SetStrokeLineCap(string lineCapType) {
    switch(lineCapType) {
    case "butt": _strokeLineCap = SVGStrokeLineCapMethod.Butt; break;
    case "round": _strokeLineCap = SVGStrokeLineCapMethod.Round; break;
    case "square": _strokeLineCap = SVGStrokeLineCapMethod.Square; break;
    }
  }

  private void SetStrokeLineJoin(string lineCapType) {
    switch(lineCapType) {
    case "miter": _strokeLineJoin = SVGStrokeLineJoinMethod.Miter; break;
    case "round": _strokeLineJoin = SVGStrokeLineJoinMethod.Round; break;
    case "bevel": _strokeLineJoin = SVGStrokeLineJoinMethod.Bevel; break;
    }
  }

  public bool IsLinearGradiantFill() {
    if(string.IsNullOrEmpty(_gradientID))
      return false;
    bool flag = false;
    for(int i = 0; i < _linearGradList.Count; i++) {
      if(_linearGradList[i].id == _gradientID) {
        flag = true;
        break;
      }
    }
    return flag;
  }

  public bool IsRadialGradiantFill() {
    if(_gradientID == "")
      return false;
    bool flag = false;
    for(int i = 0; i < _radialGradList.Count; i++) {
      if(_radialGradList[i].id == _gradientID) {
        flag = true;
        break;
      }
    }
    return flag;
  }

  public bool IsSolidFill() {
    if(_fillColor == null)
      return false;
    return (_fillColor.Value.colorType != SVGColorType.None);
  }

  public bool IsFill() {
    if(_fillColor == null)
      return (IsLinearGradiantFill() || IsRadialGradiantFill());
    return (_fillColor.Value.colorType != SVGColorType.None);
  }

  public bool IsFillX() {
    if(_fillColor == null)
      return (IsLinearGradiantFill() || IsRadialGradiantFill());
    return (_fillColor.Value.colorType != SVGColorType.Unknown);
  }

  public bool IsStroke() {
    if(_strokeColor == null)
      return false;
    if((_strokeColor.Value.colorType == SVGColorType.Unknown) ||
        (_strokeColor.Value.colorType == SVGColorType.None))
      return false;
    return true;
  }

  public SVGPaintMethod GetPaintType() {
    if(IsLinearGradiantFill())
      return SVGPaintMethod.LinearGradientFill;
    if(IsRadialGradiantFill())
      return SVGPaintMethod.RadialGradientFill;
    if(IsSolidFill())
      return SVGPaintMethod.SolidGradientFill;
    if(IsStroke())
      return SVGPaintMethod.PathDraw;

    return SVGPaintMethod.NoDraw;
  }

  public void AppendLinearGradient(SVGLinearGradientElement linearGradElement) {
    _linearGradList.Add(linearGradElement);
  }

  public void AppendRadialGradient(SVGRadialGradientElement radialGradElement) {
    _radialGradList.Add(radialGradElement);
  }

  public SVGLinearGradientBrush GetLinearGradientBrush(SVGGraphicsPath graphicsPath) {
    for(int i = 0; i < _linearGradList.Count; i++) {
      if(_linearGradList[i].id == _gradientID)
        return new SVGLinearGradientBrush(_linearGradList[i], graphicsPath);
    }
    return null;
  }

  public SVGRadialGradientBrush GetRadialGradientBrush(SVGGraphicsPath graphicsPath) {
    for(int i = 0; i < _radialGradList.Count; i++) {
      if(_radialGradList[i].id == _gradientID)
        return new SVGRadialGradientBrush(_radialGradList[i], graphicsPath);
    }
    return null;
  }
}
