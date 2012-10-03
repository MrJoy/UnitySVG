using System.Collections.Generic;

public class SVGPathElement : SVGTransformable, ISVGDrawable {
  private SVGPathSegList _segList;
  /***********************************************************************************/
  private SVGGraphics _render;
  private AttributeList _attrList;
  private SVGPaintable _paintable;
  /***********************************************************************************/
  public SVGPathElement(AttributeList attrList, SVGTransformList inheritTransformList, SVGPaintable inheritPaintable, SVGGraphics r) : base(inheritTransformList) {
    _attrList = attrList;
    _paintable = new SVGPaintable(inheritPaintable, attrList);
    _render = r;
    Initial();
  }
  /***********************************************************************************/
  private void Initial() {
    currentTransformList = new SVGTransformList(_attrList.GetValue("transform"));
    _segList = new SVGPathSegList();

    //-----------
    string _d = _attrList.GetValue("d");

    List<char> _charList = new List<char>();
    List<string> _valueList = new List<string>();

    SVGStringExtractor.ExtractPathSegList(_d, ref _charList, ref _valueList);
    for(int i = 0; i < _charList.Count; i++) {
      char _char = _charList[i];
      string _value = _valueList[i];
      float[] parms = SVGStringExtractor.ExtractTransformValueAsPX(_value);
      switch(_char) {
      case 'Z':
      case 'z':
        _segList.AppendItem(CreateSVGPathSegClosePath());
        break;
      case 'M':
        _segList.AppendItem(new SVGPathSegMovetoAbs(parms[0], parms[1]));
        break;
      case 'm':
        _segList.AppendItem(new SVGPathSegMovetoRel(parms[0], parms[1]));
        break;
      case 'L':
        _segList.AppendItem(new SVGPathSegLinetoAbs(parms[0], parms[1]));
        break;
      case 'l':
        _segList.AppendItem(new SVGPathSegLinetoRel(parms[0], parms[1]));
        break;
      case 'C':
        _segList.AppendItem(new SVGPathSegCurvetoCubicAbs(parms[0], parms[1], parms[2], parms[3], parms[4], parms[5]));
        break;
      case 'c':
        _segList.AppendItem(new SVGPathSegCurvetoCubicRel(parms[0], parms[1], parms[2], parms[3], parms[4], parms[5]));
        break;
      case 'S':
        _segList.AppendItem(new SVGPathSegCurvetoCubicSmoothAbs(parms[0], parms[1], parms[2], parms[3]));
        break;
      case 's':
        _segList.AppendItem(new SVGPathSegCurvetoCubicSmoothRel(parms[0], parms[1], parms[2], parms[3]));
        break;
      case 'Q':
        _segList.AppendItem(new SVGPathSegCurvetoQuadraticAbs(parms[0], parms[1], parms[2], parms[3]));
        break;
      case 'q':
        _segList.AppendItem(new SVGPathSegCurvetoQuadraticRel(parms[0], parms[1], parms[2], parms[3]));
        break;
      case 'T':
        _segList.AppendItem(new SVGPathSegCurvetoQuadraticSmoothAbs(parms[0], parms[1]));
        break;
      case 't':
        _segList.AppendItem(new SVGPathSegCurvetoQuadraticSmoothRel(parms[0], parms[1]));
        break;
      case 'A':
        _segList.AppendItem(new SVGPathSegArcAbs(parms[0], parms[1], parms[2], parms[3] == 1f, parms[4] == 1f, parms[5], parms[6]));
        break;
      case 'a':
        _segList.AppendItem(new SVGPathSegArcRel(parms[0], parms[1], parms[2], parms[3] == 1f, parms[4] == 1f, parms[5], parms[6]));
        break;
      case 'H':
        _segList.AppendItem(new SVGPathSegLinetoHorizontalAbs(parms[0]));
        break;
      case 'h':
        _segList.AppendItem(new SVGPathSegLinetoHorizontalRel(parms[0]));
        break;
      case 'V':
        _segList.AppendItem(new SVGPathSegLinetoVerticalAbs(parms[0]));
        break;
      case 'v':
        _segList.AppendItem(new SVGPathSegLinetoVerticalRel(parms[0]));
        break;
      }
    }
  }
  /***********************************************************************************/
  //Create Methods
  private SVGPathSegClosePath CreateSVGPathSegClosePath() {
    SVGPathSegMovetoAbs _firstPoint = _segList.GetItem(0) as SVGPathSegMovetoAbs;
    if(_firstPoint == null) {
      SVGPathSegMovetoRel _firstPoint1 = _segList.GetItem(0) as SVGPathSegMovetoRel;
      if(_firstPoint1 != null)
        return new SVGPathSegClosePath(_firstPoint1.x, _firstPoint1.y);
    } else {
      return new SVGPathSegClosePath(_firstPoint.x, _firstPoint.y);
    }

    return new SVGPathSegClosePath(-1f, -1f);
  }
  //================================================================================
  private SVGGraphicsPath _graphicsPath;
  private void CreateGraphicsPath() {
    _graphicsPath = new SVGGraphicsPath();
    for(int i = 0; i < _segList.Count; i++) {
      ISVGDrawableSeg temp = _segList.GetItem(i) as ISVGDrawableSeg;
      if(temp != null)
        temp.Render(_graphicsPath);
    }
    _graphicsPath.transformList = summaryTransformList;
  }
  //-----
  private void Draw() {
    if(_paintable.strokeColor == null)
      return;

    _render.DrawPath(_graphicsPath, _paintable.strokeWidth, _paintable.strokeColor);
  }
  //================================================================================
  //Thuc thi Interface Drawable
  public void BeforeRender(SVGTransformList transformList) {
    inheritTransformList = transformList;
    for(int i = 0; i < _segList.Count; i++) {
      ISVGDrawable temp = _segList.GetItem(i) as ISVGDrawable;
      if(temp != null)
        temp.BeforeRender(summaryTransformList);
    }
  }

  //------
  public void Render() {
    CreateGraphicsPath();
    _render.SetStrokeLineCap(_paintable.strokeLineCap);
    _render.SetStrokeLineJoin(_paintable.strokeLineJoin);
    switch(_paintable.GetPaintType()) {
    case SVGPaintMethod.SolidGradientFill:
      _render.FillPath(_paintable.fillColor.Value, _graphicsPath);
      Draw();
      break;

    case SVGPaintMethod.LinearGradientFill:
      SVGLinearGradientBrush _linearGradBrush = _paintable.GetLinearGradientBrush(_graphicsPath);

      if(_linearGradBrush != null)
        _render.FillPath(_linearGradBrush, _graphicsPath);
      Draw();
      break;

    case SVGPaintMethod.RadialGradientFill:
      SVGRadialGradientBrush _radialGradBrush = _paintable.GetRadialGradientBrush(_graphicsPath);

      if(_radialGradBrush != null)
        _render.FillPath(_radialGradBrush, _graphicsPath);
      Draw();
      break;

    case SVGPaintMethod.PathDraw:
      Draw();
      break;
    }
  }
}
