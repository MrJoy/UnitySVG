using System.Collections.Generic;

public class SVGPathElement : SVGTransformable, ISVGDrawable {
  private SVGPathSegList _segList;
  /***********************************************************************************/
  private SVGGraphics _render;
  private AttributeList _attrList;
  private SVGPaintable _paintable;
  /***********************************************************************************/
  public SVGPathElement(AttributeList attrList,
              SVGTransformList inheritTransformList,
              SVGPaintable inheritPaintable,
              SVGGraphics _render) : base(inheritTransformList) {
    this._attrList = attrList;
    this._paintable = new SVGPaintable(inheritPaintable, attrList);
    this._render = _render;
    Initial();
  }
  /***********************************************************************************/
  private void Initial() {
    this.currentTransformList = new SVGTransformList(
                      this._attrList.GetValue("transform"));
    _segList = new SVGPathSegList();

    //-----------
    string _d = this._attrList.GetValue("d");

    List<char> _charList = new List<char>();
    List<string> _valueList = new List<string>();

    SVGStringExtractor.ExtractPathSegList(_d, ref _charList, ref _valueList);
    for(int i = 0; i < _charList.Count; i++) {
      char _char = _charList[i];
      string _value = _valueList[i];
      string[] _valuesOfChar = SVGStringExtractor.ExtractTransformValue(_value);
      switch(_char) {
        case 'Z' :
        case 'z' : {
          SVGPathSegClosePath temp = CreateSVGPathSegClosePath();
          _segList.AppendItem(temp);
        break;
        }
        case 'M' : {
          SVGPathSegMovetoAbs temp = CreateSVGPathSegMovetoAbs(
                          SVGLength.GetPXLength(_valuesOfChar[0]),
                          SVGLength.GetPXLength(_valuesOfChar[1]));
          _segList.AppendItem(temp);
        break;
        }
        case 'm' : {
          SVGPathSegMovetoRel temp = CreateSVGPathSegMovetoRel(
                          SVGLength.GetPXLength(_valuesOfChar[0]),
                          SVGLength.GetPXLength(_valuesOfChar[1]));
          _segList.AppendItem(temp);
        break;
        }
        case 'L' : {
          SVGPathSegLinetoAbs temp = CreateSVGPathSegLinetoAbs(
                          SVGLength.GetPXLength(_valuesOfChar[0]),
                          SVGLength.GetPXLength(_valuesOfChar[1]));
          _segList.AppendItem(temp);
          break;
        }
        case 'l' : {
          SVGPathSegLinetoRel temp = CreateSVGPathSegLinetoRel(
                          SVGLength.GetPXLength(_valuesOfChar[0]),
                          SVGLength.GetPXLength(_valuesOfChar[1]));
          _segList.AppendItem(temp);
          break;
        }
        case 'C' : {
          SVGPathSegCurvetoCubicAbs temp = CreateSVGPathSegCurvetoCubicAbs(
                          SVGLength.GetPXLength(_valuesOfChar[0]),
                          SVGLength.GetPXLength(_valuesOfChar[1]),
                          SVGLength.GetPXLength(_valuesOfChar[2]),
                          SVGLength.GetPXLength(_valuesOfChar[3]),
                          SVGLength.GetPXLength(_valuesOfChar[4]),
                          SVGLength.GetPXLength(_valuesOfChar[5]));
          _segList.AppendItem(temp);
          break;
        }
        case 'c' : {
          SVGPathSegCurvetoCubicRel temp = CreateSVGPathSegCurvetoCubicRel(
                          SVGLength.GetPXLength(_valuesOfChar[0]),
                          SVGLength.GetPXLength(_valuesOfChar[1]),
                          SVGLength.GetPXLength(_valuesOfChar[2]),
                          SVGLength.GetPXLength(_valuesOfChar[3]),
                          SVGLength.GetPXLength(_valuesOfChar[4]),
                          SVGLength.GetPXLength(_valuesOfChar[5]));
          _segList.AppendItem(temp);
          break;
        }
        case 'S' : {
          SVGPathSegCurvetoCubicSmoothAbs temp =
                        CreateSVGPathSegCurvetoCubicSmoothAbs(
                          SVGLength.GetPXLength(_valuesOfChar[0]),
                          SVGLength.GetPXLength(_valuesOfChar[1]),
                          SVGLength.GetPXLength(_valuesOfChar[2]),
                          SVGLength.GetPXLength(_valuesOfChar[3]));
          _segList.AppendItem(temp);
          break;
        }
        case 's' : {
          SVGPathSegCurvetoCubicSmoothRel temp =
                        CreateSVGPathSegCurvetoCubicSmoothRel(
                          SVGLength.GetPXLength(_valuesOfChar[0]),
                          SVGLength.GetPXLength(_valuesOfChar[1]),
                          SVGLength.GetPXLength(_valuesOfChar[2]),
                          SVGLength.GetPXLength(_valuesOfChar[3]));
          _segList.AppendItem(temp);
          break;
        }
        case 'Q' : {
          SVGPathSegCurvetoQuadraticAbs temp = CreateSVGPathSegCurvetoQuadraticAbs(
                          SVGLength.GetPXLength(_valuesOfChar[0]),
                          SVGLength.GetPXLength(_valuesOfChar[1]),
                          SVGLength.GetPXLength(_valuesOfChar[2]),
                          SVGLength.GetPXLength(_valuesOfChar[3]));
          _segList.AppendItem(temp);
          break;
        }
        case 'q' : {
          SVGPathSegCurvetoQuadraticRel temp = CreateSVGPathSegCurvetoQuadraticRel(
                          SVGLength.GetPXLength(_valuesOfChar[0]),
                          SVGLength.GetPXLength(_valuesOfChar[1]),
                          SVGLength.GetPXLength(_valuesOfChar[2]),
                          SVGLength.GetPXLength(_valuesOfChar[3]));
          _segList.AppendItem(temp);
          break;
        }
        case 'T' : {
          SVGPathSegCurvetoQuadraticSmoothAbs temp =
                          CreateSVGPathSegCurvetoQuadraticSmoothAbs(
                          SVGLength.GetPXLength(_valuesOfChar[0]),
                          SVGLength.GetPXLength(_valuesOfChar[1]));
          _segList.AppendItem(temp);
          break;
        }
        case 't' : {
          SVGPathSegCurvetoQuadraticSmoothRel temp =
                        CreateSVGPathSegCurvetoQuadraticSmoothRel(
                          SVGLength.GetPXLength(_valuesOfChar[0]),
                          SVGLength.GetPXLength(_valuesOfChar[1]));
          _segList.AppendItem(temp);
          break;
        }
        case 'A' : {
          SVGPathSegArcAbs temp = CreateSVGPathSegArcAbs(
                          SVGLength.GetPXLength(_valuesOfChar[0]),
                          SVGLength.GetPXLength(_valuesOfChar[1]),
                          SVGLength.GetPXLength(_valuesOfChar[2]),
                          SVGLength.GetPXLength(_valuesOfChar[3]),
                          SVGLength.GetPXLength(_valuesOfChar[4]),
                          SVGLength.GetPXLength(_valuesOfChar[5]),
                          SVGLength.GetPXLength(_valuesOfChar[6]));
          _segList.AppendItem(temp);
          break;
        }
        case 'a' : {
          SVGPathSegArcRel temp = CreateSVGPathSegArcRel(
                          SVGLength.GetPXLength(_valuesOfChar[0]),
                          SVGLength.GetPXLength(_valuesOfChar[1]),
                          SVGLength.GetPXLength(_valuesOfChar[2]),
                          SVGLength.GetPXLength(_valuesOfChar[3]),
                          SVGLength.GetPXLength(_valuesOfChar[4]),
                          SVGLength.GetPXLength(_valuesOfChar[5]),
                          SVGLength.GetPXLength(_valuesOfChar[6]));
          _segList.AppendItem(temp);
          break;
        }
        case 'H' : {
          SVGPathSegLinetoHorizontalAbs temp = CreateSVGPathSegLinetoHorizontalAbs(
                          SVGLength.GetPXLength(_valuesOfChar[0]));
          _segList.AppendItem(temp);
          break;
        }
        case 'h' : {
          SVGPathSegLinetoHorizontalRel temp = CreateSVGPathSegLinetoHorizontalRel(
                          SVGLength.GetPXLength(_valuesOfChar[0]));
          _segList.AppendItem(temp);
          break;
        }
        case 'V' : {
          SVGPathSegLinetoVerticalAbs temp = CreateSVGPathSegLinetoVerticalAbs(
                          SVGLength.GetPXLength(_valuesOfChar[0]));
          _segList.AppendItem(temp);
          break;
        }
        case 'v' : {
          SVGPathSegLinetoVerticalRel temp = CreateSVGPathSegLinetoVerticalRel(
                          SVGLength.GetPXLength(_valuesOfChar[0]));
          _segList.AppendItem(temp);
          break;
        }
      }
    }
  }
  /***********************************************************************************/
  //Create Methods
  public SVGPathSegClosePath CreateSVGPathSegClosePath() {
    SVGPathSegMovetoAbs _firstPoint = _segList.GetItem(0)as SVGPathSegMovetoAbs;
    if(_firstPoint == null) {
      SVGPathSegMovetoRel _firstPoint1 = _segList.GetItem(0)as SVGPathSegMovetoRel;
      if(_firstPoint1 != null) {
        return new SVGPathSegClosePath(_firstPoint1.x, _firstPoint1.y);
      }
    } else {
      return new SVGPathSegClosePath(_firstPoint.x, _firstPoint.y);
    }

    return new SVGPathSegClosePath(-1f, -1f);
  }
  //--------------
  //MoveToAbs
  public SVGPathSegMovetoAbs CreateSVGPathSegMovetoAbs(float x, float y) {
    return new SVGPathSegMovetoAbs(x, y);
  }
  //--------------
  //MoveToRel
  public SVGPathSegMovetoRel CreateSVGPathSegMovetoRel(float x, float y) {
    return new SVGPathSegMovetoRel(x, y);
  }

  //--------------
  //LineToAbs
  public SVGPathSegLinetoAbs CreateSVGPathSegLinetoAbs(float x, float y) {
    return new SVGPathSegLinetoAbs(x, y);
  }
  //--------------
  //LineToRel
  public SVGPathSegLinetoRel CreateSVGPathSegLinetoRel(float x, float y) {
    return new SVGPathSegLinetoRel(x, y);
  }
  //--------------
  //CubicCurveAbs
  public SVGPathSegCurvetoCubicAbs CreateSVGPathSegCurvetoCubicAbs(float x1,
                  float y1, float x2, float y2, float x, float y) {
    return new SVGPathSegCurvetoCubicAbs(x1, y1, x2, y2, x, y);
  }
  //--------------
  //CubicCurveRel
  public SVGPathSegCurvetoCubicRel CreateSVGPathSegCurvetoCubicRel(float x1,
                  float y1, float x2, float y2, float x, float y) {
    return new SVGPathSegCurvetoCubicRel(x1, y1, x2, y2, x, y);
  }
  //--------------
  //SmoothCubicCurveAbs(S)
  public SVGPathSegCurvetoCubicSmoothAbs CreateSVGPathSegCurvetoCubicSmoothAbs(float x2,
                                float y2, float x, float y) {
    return new SVGPathSegCurvetoCubicSmoothAbs(x2, y2, x, y);
  }
  //--------------
  //SmoothCubicCurveRel(s)
  public SVGPathSegCurvetoCubicSmoothRel CreateSVGPathSegCurvetoCubicSmoothRel(float x2,
                                float y2, float x, float y) {
    return new SVGPathSegCurvetoCubicSmoothRel(x2, y2, x, y);
  }
  //--------------
  //QuadraticCurveAbs(Q)
  public SVGPathSegCurvetoQuadraticAbs CreateSVGPathSegCurvetoQuadraticAbs(float x1,
                  float y1, float x, float y) {
    return new SVGPathSegCurvetoQuadraticAbs(x1, y1, x, y);
  }
  //--------------
  //QuadraticCurveAbs(q)
  public SVGPathSegCurvetoQuadraticRel CreateSVGPathSegCurvetoQuadraticRel(float x1,
                  float y1, float x, float y) {
    return new SVGPathSegCurvetoQuadraticRel(x1, y1, x, y);
  }
  //--------------
  //SmoothQuadraticCurveAbs(T)
  public SVGPathSegCurvetoQuadraticSmoothAbs CreateSVGPathSegCurvetoQuadraticSmoothAbs(float x,
                                float y) {
    return new SVGPathSegCurvetoQuadraticSmoothAbs(x, y);
  }
  //--------------
  //SmoothQuadraticCurveRel(t)
  public SVGPathSegCurvetoQuadraticSmoothRel CreateSVGPathSegCurvetoQuadraticSmoothRel(float x,
                                float y) {
    return new SVGPathSegCurvetoQuadraticSmoothRel(x, y);
  }
  //--------------
  //ArcAbs(A)
  public SVGPathSegArcAbs CreateSVGPathSegArcAbs(float r1, float r2, float angle,
                          float largeArcFlag, float sweepFlag,
                          float x, float y) {
    return new SVGPathSegArcAbs(r1, r2, angle, largeArcFlag == 1.0f, sweepFlag == 1.0f, x, y);
  }
  //--------------
  //ArcRel(a)
  public SVGPathSegArcRel CreateSVGPathSegArcRel(float r1, float r2, float angle,
                          float largeArcFlag, float sweepFlag,
                          float x, float y) {
    return new SVGPathSegArcRel(r1, r2, angle, largeArcFlag == 1.0f, sweepFlag == 1.0f, x, y);
  }
  //--------------
  //LinetoHorizontalAbs(H)
  public SVGPathSegLinetoHorizontalAbs CreateSVGPathSegLinetoHorizontalAbs(float x) {
    return new SVGPathSegLinetoHorizontalAbs(x);
  }
  //--------------
  //LinetoHorizontalRel(h)
  public SVGPathSegLinetoHorizontalRel CreateSVGPathSegLinetoHorizontalRel(float x) {
    return new SVGPathSegLinetoHorizontalRel(x);
  }
  //--------------
  //LinetVerticalAbs(V)
  public SVGPathSegLinetoVerticalAbs CreateSVGPathSegLinetoVerticalAbs(float y) {
    return new SVGPathSegLinetoVerticalAbs(y);
  }
  //--------------
  //LinetoVerticalRel(v)
  public SVGPathSegLinetoVerticalRel CreateSVGPathSegLinetoVerticalRel(float y) {
    return new SVGPathSegLinetoVerticalRel(y);
  }
  //================================================================================
  private SVGGraphicsPath _graphicsPath;
  private void CreateGraphicsPath() {
    this._graphicsPath = new SVGGraphicsPath();
    for(int i = 0; i < this._segList.Count; i++) {
      ISVGDrawableSeg temp = this._segList.GetItem(i)as ISVGDrawableSeg;
      if(temp != null) {
        temp.Render(this._graphicsPath);
      }
    }
    this._graphicsPath.transformList = this.summaryTransformList;
  }
  //-----
  private void Draw() {
    if(this._paintable.strokeColor == null)return;

    this._render.DrawPath(this._graphicsPath, this._paintable.strokeWidth,
                            this._paintable.strokeColor);
  }
  //================================================================================
  //Thuc thi Interface Drawable
  public void BeforeRender(SVGTransformList transformList) {
    this.inheritTransformList = transformList;
    for(int i = 0; i < this._segList.Count; i++) {
      ISVGDrawable temp = this._segList.GetItem(i)as ISVGDrawable;
      if(temp != null) {
        temp.BeforeRender(this.summaryTransformList);
      }
    }
  }

  //------
  public void Render() {
    CreateGraphicsPath();
    this._render.SetStrokeLineCap(this._paintable.strokeLineCap);
    this._render.SetStrokeLineJoin(this._paintable.strokeLineJoin);
    switch(this._paintable.GetPaintType()) {
      case SVGPaintMethod.SolidGradientFill : {
        this._render.FillPath(this._paintable.fillColor.Value, this._graphicsPath);
        Draw();
        break;
      }
      case SVGPaintMethod.LinearGradientFill : {

        SVGLinearGradientBrush _linearGradBrush =
                  this._paintable.GetLinearGradientBrush(this._graphicsPath);

        if(_linearGradBrush != null) {
          this._render.FillPath(_linearGradBrush, _graphicsPath);
        }
        Draw();
        break;
      }
      case SVGPaintMethod.RadialGradientFill : {
        SVGRadialGradientBrush _radialGradBrush =
                  this._paintable.GetRadialGradientBrush(this._graphicsPath);

        if(_radialGradBrush != null) {
          this._render.FillPath(_radialGradBrush, _graphicsPath);
        }
        Draw();
        break;
      }
      case SVGPaintMethod.PathDraw : {
        Draw();
        break;
      }
    }
  }
}