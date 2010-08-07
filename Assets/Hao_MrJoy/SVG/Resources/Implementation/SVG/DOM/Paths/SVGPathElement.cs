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
          SVGPathSegMovetoAbs temp = new SVGPathSegMovetoAbs(
                          SVGLength.GetPXLength(_valuesOfChar[0]),
                          SVGLength.GetPXLength(_valuesOfChar[1]));
          _segList.AppendItem(temp);
        break;
        }
        case 'm' : {
          SVGPathSegMovetoRel temp = new SVGPathSegMovetoRel(
                          SVGLength.GetPXLength(_valuesOfChar[0]),
                          SVGLength.GetPXLength(_valuesOfChar[1]));
          _segList.AppendItem(temp);
        break;
        }
        case 'L' : {
          SVGPathSegLinetoAbs temp = new SVGPathSegLinetoAbs(
                          SVGLength.GetPXLength(_valuesOfChar[0]),
                          SVGLength.GetPXLength(_valuesOfChar[1]));
          _segList.AppendItem(temp);
          break;
        }
        case 'l' : {
          SVGPathSegLinetoRel temp = new SVGPathSegLinetoRel(
                          SVGLength.GetPXLength(_valuesOfChar[0]),
                          SVGLength.GetPXLength(_valuesOfChar[1]));
          _segList.AppendItem(temp);
          break;
        }
        case 'C' : {
          SVGPathSegCurvetoCubicAbs temp = new SVGPathSegCurvetoCubicAbs(
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
          SVGPathSegCurvetoCubicRel temp = new SVGPathSegCurvetoCubicRel(
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
                        new SVGPathSegCurvetoCubicSmoothAbs(
                          SVGLength.GetPXLength(_valuesOfChar[0]),
                          SVGLength.GetPXLength(_valuesOfChar[1]),
                          SVGLength.GetPXLength(_valuesOfChar[2]),
                          SVGLength.GetPXLength(_valuesOfChar[3]));
          _segList.AppendItem(temp);
          break;
        }
        case 's' : {
          SVGPathSegCurvetoCubicSmoothRel temp =
                        new SVGPathSegCurvetoCubicSmoothRel(
                          SVGLength.GetPXLength(_valuesOfChar[0]),
                          SVGLength.GetPXLength(_valuesOfChar[1]),
                          SVGLength.GetPXLength(_valuesOfChar[2]),
                          SVGLength.GetPXLength(_valuesOfChar[3]));
          _segList.AppendItem(temp);
          break;
        }
        case 'Q' : {
          SVGPathSegCurvetoQuadraticAbs temp = new SVGPathSegCurvetoQuadraticAbs(
                          SVGLength.GetPXLength(_valuesOfChar[0]),
                          SVGLength.GetPXLength(_valuesOfChar[1]),
                          SVGLength.GetPXLength(_valuesOfChar[2]),
                          SVGLength.GetPXLength(_valuesOfChar[3]));
          _segList.AppendItem(temp);
          break;
        }
        case 'q' : {
          SVGPathSegCurvetoQuadraticRel temp = new SVGPathSegCurvetoQuadraticRel(
                          SVGLength.GetPXLength(_valuesOfChar[0]),
                          SVGLength.GetPXLength(_valuesOfChar[1]),
                          SVGLength.GetPXLength(_valuesOfChar[2]),
                          SVGLength.GetPXLength(_valuesOfChar[3]));
          _segList.AppendItem(temp);
          break;
        }
        case 'T' : {
          SVGPathSegCurvetoQuadraticSmoothAbs temp =
                          new SVGPathSegCurvetoQuadraticSmoothAbs(
                          SVGLength.GetPXLength(_valuesOfChar[0]),
                          SVGLength.GetPXLength(_valuesOfChar[1]));
          _segList.AppendItem(temp);
          break;
        }
        case 't' : {
          SVGPathSegCurvetoQuadraticSmoothRel temp =
                        new SVGPathSegCurvetoQuadraticSmoothRel(
                          SVGLength.GetPXLength(_valuesOfChar[0]),
                          SVGLength.GetPXLength(_valuesOfChar[1]));
          _segList.AppendItem(temp);
          break;
        }
        case 'A' : {
          SVGPathSegArcAbs temp = new SVGPathSegArcAbs(
                          SVGLength.GetPXLength(_valuesOfChar[0]),
                          SVGLength.GetPXLength(_valuesOfChar[1]),
                          SVGLength.GetPXLength(_valuesOfChar[2]),
                          SVGLength.GetPXLength(_valuesOfChar[3]) == 1f,
                          SVGLength.GetPXLength(_valuesOfChar[4]) == 1f,
                          SVGLength.GetPXLength(_valuesOfChar[5]),
                          SVGLength.GetPXLength(_valuesOfChar[6]));
          _segList.AppendItem(temp);
          break;
        }
        case 'a' : {
          SVGPathSegArcRel temp = new SVGPathSegArcRel(
                          SVGLength.GetPXLength(_valuesOfChar[0]),
                          SVGLength.GetPXLength(_valuesOfChar[1]),
                          SVGLength.GetPXLength(_valuesOfChar[2]),
                          SVGLength.GetPXLength(_valuesOfChar[3]) == 1f,
                          SVGLength.GetPXLength(_valuesOfChar[4]) == 1f,
                          SVGLength.GetPXLength(_valuesOfChar[5]),
                          SVGLength.GetPXLength(_valuesOfChar[6]));
          _segList.AppendItem(temp);
          break;
        }
        case 'H' : {
          SVGPathSegLinetoHorizontalAbs temp = new SVGPathSegLinetoHorizontalAbs(
                          SVGLength.GetPXLength(_valuesOfChar[0]));
          _segList.AppendItem(temp);
          break;
        }
        case 'h' : {
          SVGPathSegLinetoHorizontalRel temp = new SVGPathSegLinetoHorizontalRel(
                          SVGLength.GetPXLength(_valuesOfChar[0]));
          _segList.AppendItem(temp);
          break;
        }
        case 'V' : {
          SVGPathSegLinetoVerticalAbs temp = new SVGPathSegLinetoVerticalAbs(
                          SVGLength.GetPXLength(_valuesOfChar[0]));
          _segList.AppendItem(temp);
          break;
        }
        case 'v' : {
          SVGPathSegLinetoVerticalRel temp = new SVGPathSegLinetoVerticalRel(
                          SVGLength.GetPXLength(_valuesOfChar[0]));
          _segList.AppendItem(temp);
          break;
        }
      }
    }
  }
  /***********************************************************************************/
  //Create Methods
  private SVGPathSegClosePath CreateSVGPathSegClosePath() {
    SVGPathSegMovetoAbs _firstPoint = _segList.GetItem(0) as SVGPathSegMovetoAbs;
    if(_firstPoint == null) {
      SVGPathSegMovetoRel _firstPoint1 = _segList.GetItem(0) as SVGPathSegMovetoRel;
      if(_firstPoint1 != null) {
        return new SVGPathSegClosePath(_firstPoint1.x, _firstPoint1.y);
      }
    } else {
      return new SVGPathSegClosePath(_firstPoint.x, _firstPoint.y);
    }

    return new SVGPathSegClosePath(-1f, -1f);
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