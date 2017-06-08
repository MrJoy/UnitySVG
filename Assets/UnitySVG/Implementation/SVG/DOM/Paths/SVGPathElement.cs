using System;
using System.Collections.Generic;
using UnityEngine;
using UnitySVG;
using UnityEngine.Profiling;

public class SVGPathElement : SVGTransformable, ISVGDrawable {
  private readonly SVGPathSegList _segList;
  private readonly SVGGraphics _render;
  private readonly SVGPaintable _paintable;
  private SVGGraphicsPath _graphicsPath;

  public SVGPathElement(Dictionary<string, string> attrList, SVGTransformList inheritTransformList,
                        SVGPaintable inheritPaintable, SVGGraphics r) : base(inheritTransformList) {
    Profiler.BeginSample("SVGPathElement constructor");
    _paintable = new SVGPaintable(inheritPaintable, attrList);
    _render = r;
    currentTransformList = new SVGTransformList(attrList.GetValue("transform"));

    string dstr = attrList.GetValue("d");
    int nbSegments = 0;
    for(int i = 0; i < dstr.Length; ++i) {
      // TODO: Make safety-checking optional, and on-by-default in development.
      switch(dstr[i]) {
      case 'Z':
      case 'z':
      case 'M':
      case 'm':
      case 'L':
      case 'l':
      case 'C':
      case 'c':
      case 'S':
      case 's':
      case 'Q':
      case 'q':
      case 'T':
      case 't':
      case 'A':
      case 'a':
      case 'H':
      case 'h':
      case 'V':
      case 'v':
        ++nbSegments;
        break;
      }
    }

    _segList = new SVGPathSegList(nbSegments); // optimization: count number of segments before starting
    for(int i = 0; i < dstr.Length;) {
      while(i < dstr.Length - 1 && dstr[i] == ' ') // skip whitespace before type character
        ++i;
      char _char = dstr[i];
      switch(_char) {
      case 'Z':
      case 'z':
        _segList.AppendItem(CreateSVGPathSegClosePath());
        ++i;
        break;
      case 'M': {
        try {
          float a = ReadFloat(dstr, ref i);
          float b = ReadFloat(dstr, ref i);
          _segList.AppendItem(new SVGPathSegMovetoAbs(a, b));
        } catch(Exception) {
          Debug.Log("exception when parsing " + dstr);
          throw;
        }
        break;
      }
      case 'm': {
        float a = ReadFloat(dstr, ref i);
        float b = ReadFloat(dstr, ref i);
        _segList.AppendItem(new SVGPathSegMovetoRel(a, b));
        break;
      }
      case 'L': {
        float a = ReadFloat(dstr, ref i);
        float b = ReadFloat(dstr, ref i);
        _segList.AppendItem(new SVGPathSegLinetoAbs(a, b));
        break;
      }
      case 'l': {
        float a = ReadFloat(dstr, ref i);
        float b = ReadFloat(dstr, ref i);
        _segList.AppendItem(new SVGPathSegLinetoRel(a, b));
        break;
      }
      case 'C': {
        float a = ReadFloat(dstr, ref i);
        float b = ReadFloat(dstr, ref i);
        float c = ReadFloat(dstr, ref i);
        float d = ReadFloat(dstr, ref i);
        float e = ReadFloat(dstr, ref i);
        float f = ReadFloat(dstr, ref i);
        _segList.AppendItem(new SVGPathSegCurvetoCubicAbs(a, b, c, d, e, f));
        break;
      }
      case 'c': {
        float a = ReadFloat(dstr, ref i);
        float b = ReadFloat(dstr, ref i);
        float c = ReadFloat(dstr, ref i);
        float d = ReadFloat(dstr, ref i);
        float e = ReadFloat(dstr, ref i);
        float f = ReadFloat(dstr, ref i);
        _segList.AppendItem(new SVGPathSegCurvetoCubicRel(a, b, c, d, e, f));
        break;
      }
      case 'S': {
        float a = ReadFloat(dstr, ref i);
        float b = ReadFloat(dstr, ref i);
        float c = ReadFloat(dstr, ref i);
        float d = ReadFloat(dstr, ref i);
        _segList.AppendItem(new SVGPathSegCurvetoCubicSmoothAbs(a, b, c, d));
        break;
      }
      case 's': {
        float a = ReadFloat(dstr, ref i);
        float b = ReadFloat(dstr, ref i);
        float c = ReadFloat(dstr, ref i);
        float d = ReadFloat(dstr, ref i);
        _segList.AppendItem(new SVGPathSegCurvetoCubicSmoothRel(a, b, c, d));
        break;
      }
      case 'Q': {
        float a = ReadFloat(dstr, ref i);
        float b = ReadFloat(dstr, ref i);
        float c = ReadFloat(dstr, ref i);
        float d = ReadFloat(dstr, ref i);
        _segList.AppendItem(new SVGPathSegCurvetoQuadraticAbs(a, b, c, d));
        break;
      }
      case 'q': {
        float a = ReadFloat(dstr, ref i);
        float b = ReadFloat(dstr, ref i);
        float c = ReadFloat(dstr, ref i);
        float d = ReadFloat(dstr, ref i);
        _segList.AppendItem(new SVGPathSegCurvetoQuadraticRel(a, b, c, d));
        break;
      }
      case 'T': {
        float a = ReadFloat(dstr, ref i);
        float b = ReadFloat(dstr, ref i);
        _segList.AppendItem(new SVGPathSegCurvetoQuadraticSmoothAbs(a, b));
        break;
      }
      case 't': {
        float a = ReadFloat(dstr, ref i);
        float b = ReadFloat(dstr, ref i);
        _segList.AppendItem(new SVGPathSegCurvetoQuadraticSmoothRel(a, b));
        break;
      }
      case 'A': {
        float a = ReadFloat(dstr, ref i);
        float b = ReadFloat(dstr, ref i);
        float c = ReadFloat(dstr, ref i);
        bool d = ReadFloat(dstr, ref i) > 0;
        bool e = ReadFloat(dstr, ref i) > 0;
        float f = ReadFloat(dstr, ref i);
        float g = ReadFloat(dstr, ref i);
        _segList.AppendItem(new SVGPathSegArcAbs(a, b, c, d, e, f, g));
        break;
      }
      case 'a': {
        float a = ReadFloat(dstr, ref i);
        float b = ReadFloat(dstr, ref i);
        float c = ReadFloat(dstr, ref i);
        bool d = ReadFloat(dstr, ref i) > 0;
        bool e = ReadFloat(dstr, ref i) > 0;
        float f = ReadFloat(dstr, ref i);
        float g = ReadFloat(dstr, ref i);
        _segList.AppendItem(new SVGPathSegArcRel(a, b, c, d, e, f, g));
        break;
      }
      case 'H': {
        float a = ReadFloat(dstr, ref i);
        _segList.AppendItem(new SVGPathSegLinetoHorizontalAbs(a));
        break;
      }
      case 'h': {
        float a = ReadFloat(dstr, ref i);
        _segList.AppendItem(new SVGPathSegLinetoHorizontalRel(a));
        break;
      }
      case 'V': {
        float a = ReadFloat(dstr, ref i);
        _segList.AppendItem(new SVGPathSegLinetoVerticalAbs(a));
        break;
      }
      case 'v': {
        float a = ReadFloat(dstr, ref i);
        _segList.AppendItem(new SVGPathSegLinetoVerticalRel(a));
        break;
      }
      default:
        ++i;
        break;
      }
    }

    //List<char> _charList = new List<char>();
    //List<string> _valueList = new List<string>();

    //SVGStringExtractor.ExtractPathSegList(dstr, ref _charList, ref _valueList);
    //_segList = new SVGPathSegList(_charList.Count);
    //for (int i = 0; i < _charList.Count; i++)
    //{
    //  char _char = _charList[i];
    //  string _value = _valueList[i];
    //  float[] parms = SVGStringExtractor.ExtractTransformValueAsPX(_value);
    //  switch (_char)
    //  {
    //    case 'Z':
    //    case 'z':
    //      _segList.AppendItem(CreateSVGPathSegClosePath());
    //      break;
    //    case 'M':
    //      _segList.AppendItem(new SVGPathSegMovetoAbs(parms[0], parms[1]));
    //      break;
    //    case 'm':
    //      _segList.AppendItem(new SVGPathSegMovetoRel(parms[0], parms[1]));
    //      break;
    //    case 'L':
    //      _segList.AppendItem(new SVGPathSegLinetoAbs(parms[0], parms[1]));
    //      break;
    //    case 'l':
    //      _segList.AppendItem(new SVGPathSegLinetoRel(parms[0], parms[1]));
    //      break;
    //    case 'C':
    //      _segList.AppendItem(new SVGPathSegCurvetoCubicAbs(parms[0], parms[1], parms[2], parms[3], parms[4], parms[5]));
    //      break;
    //    case 'c':
    //      _segList.AppendItem(new SVGPathSegCurvetoCubicRel(parms[0], parms[1], parms[2], parms[3], parms[4], parms[5]));
    //      break;
    //    case 'S':
    //      _segList.AppendItem(new SVGPathSegCurvetoCubicSmoothAbs(parms[0], parms[1], parms[2], parms[3]));
    //      break;
    //    case 's':
    //      _segList.AppendItem(new SVGPathSegCurvetoCubicSmoothRel(parms[0], parms[1], parms[2], parms[3]));
    //      break;
    //    case 'Q':
    //      _segList.AppendItem(new SVGPathSegCurvetoQuadraticAbs(parms[0], parms[1], parms[2], parms[3]));
    //      break;
    //    case 'q':
    //      _segList.AppendItem(new SVGPathSegCurvetoQuadraticRel(parms[0], parms[1], parms[2], parms[3]));
    //      break;
    //    case 'T':
    //      _segList.AppendItem(new SVGPathSegCurvetoQuadraticSmoothAbs(parms[0], parms[1]));
    //      break;
    //    case 't':
    //      _segList.AppendItem(new SVGPathSegCurvetoQuadraticSmoothRel(parms[0], parms[1]));
    //      break;
    //    case 'A':
    //      _segList.AppendItem(new SVGPathSegArcAbs(parms[0], parms[1], parms[2], parms[3] == 1f, parms[4] == 1f, parms[5],
    //        parms[6]));
    //      break;
    //    case 'a':
    //      _segList.AppendItem(new SVGPathSegArcRel(parms[0], parms[1], parms[2], parms[3] == 1f, parms[4] == 1f, parms[5],
    //        parms[6]));
    //      break;
    //    case 'H':
    //      _segList.AppendItem(new SVGPathSegLinetoHorizontalAbs(parms[0]));
    //      break;
    //    case 'h':
    //      _segList.AppendItem(new SVGPathSegLinetoHorizontalRel(parms[0]));
    //      break;
    //    case 'V':
    //      _segList.AppendItem(new SVGPathSegLinetoVerticalAbs(parms[0]));
    //      break;
    //    case 'v':
    //      _segList.AppendItem(new SVGPathSegLinetoVerticalRel(parms[0]));
    //      break;
    //  }
    //}
    Profiler.EndSample();
  }

  private static float ReadFloat(string s, ref int i) {
    ++i;
    while((s[i] == ' '))
      ++i;
    int start = i;
    int l = 0;
    for(; i < s.Length; ++i, ++l) {
      if(((s[i] >= 'a') && (s[i] <= 'z')) || ((s[i] >= 'A') && (s[i] <= 'Z')) || (s[i] == ' ') || (s[i] == ',')
          || (s[i] == '\n') || (s[i] == '\t') || (s[i] == '\r'))
        break;
    }
    return float.Parse(s.Substring(start, l));
  }

  private SVGPathSegClosePath CreateSVGPathSegClosePath() {
    SVGPathSegMovetoAbs _firstPoint = _segList.GetItem(0) as SVGPathSegMovetoAbs;
    if(_firstPoint == null) {
      SVGPathSegMovetoRel _firstPoint1 = _segList.GetItem(0) as SVGPathSegMovetoRel;
      if(_firstPoint1 != null)
        return new SVGPathSegClosePath(_firstPoint1.x, _firstPoint1.y);
    } else
      return new SVGPathSegClosePath(_firstPoint.x, _firstPoint.y);

    return new SVGPathSegClosePath(-1f, -1f);
  }


  private void CreateGraphicsPath() {
    _graphicsPath = new SVGGraphicsPath();
    for(int i = 0; i < _segList.Count; i++) {
      ISVGDrawableSeg temp = _segList.GetItem(i) as ISVGDrawableSeg;
      if(temp != null)
        temp.Render(_graphicsPath);
    }
    _graphicsPath.transformList = summaryTransformList;
  }

  private void Draw() {
    if(_paintable.strokeColor == null)
      return;

    _render.DrawPath(_graphicsPath, _paintable.strokeWidth, _paintable.strokeColor);
  }

  public void BeforeRender(SVGTransformList transformList) {
    inheritTransformList = transformList;
    for(int i = 0; i < _segList.Count; i++) {
      ISVGDrawable temp = _segList.GetItem(i) as ISVGDrawable;
      if(temp != null)
        temp.BeforeRender(summaryTransformList);
    }
  }

  public void Render() {
    CreateGraphicsPath();
    _render.StrokeLineCap = _paintable.strokeLineCap;
    _render.StrokeLineJoin = _paintable.strokeLineJoin;
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
