using System.Collections.Generic;

public class uSVGPathElement : uSVGTransformable, uISVGDrawable {
  private uSVGPathSegList _segList;
  /***********************************************************************************/
  private uSVGGraphics _render;
  private AttributeList _attrList;
  private uSVGPaintable _paintable;
  /***********************************************************************************/
  public uSVGPathElement(AttributeList attrList,
              uSVGTransformList inheritTransformList,
              uSVGPaintable inheritPaintable,
              uSVGGraphics _render) : base(inheritTransformList) {
    this._attrList = attrList;
    this._paintable = new uSVGPaintable(inheritPaintable, attrList);
    this._render = _render;
    Initial();
  }
  /***********************************************************************************/
  private void Initial() {
    this.currentTransformList = new uSVGTransformList(
                      this._attrList.GetValue("transform"));
    _segList = new uSVGPathSegList();

    //-----------
    string _d = this._attrList.GetValue("d");

    List<char> _charList = new List<char>();
    List<string> _valueList = new List<string>();

    uSVGStringExtractor.ExtractPathSegList(_d, ref _charList, ref _valueList);
    for(int i = 0; i < _charList.Count; i++) {
      char _char = _charList[i];
      string _value = _valueList[i];
      string[] _valuesOfChar = uSVGStringExtractor.ExtractTransformValue(_value);
      switch(_char) {
        case 'Z' :
        case 'z' : {
          uSVGPathSegClosePath temp = CreateSVGPathSegClosePath();
          _segList.AppendItem(temp);
        break;
        }
        case 'M' : {
          uSVGPathSegMovetoAbs temp = CreateSVGPathSegMovetoAbs(
                          uSVGLength.GetPXLength(_valuesOfChar[0]),
                          uSVGLength.GetPXLength(_valuesOfChar[1]));
          _segList.AppendItem(temp);
        break;
        }
        case 'm' : {
          uSVGPathSegMovetoRel temp = CreateSVGPathSegMovetoRel(
                          uSVGLength.GetPXLength(_valuesOfChar[0]),
                          uSVGLength.GetPXLength(_valuesOfChar[1]));
          _segList.AppendItem(temp);
        break;
        }
        case 'L' : {
          uSVGPathSegLinetoAbs temp = CreateSVGPathSegLinetoAbs(
                          uSVGLength.GetPXLength(_valuesOfChar[0]),
                          uSVGLength.GetPXLength(_valuesOfChar[1]));
          _segList.AppendItem(temp);
          break;
        }
        case 'l' : {
          uSVGPathSegLinetoRel temp = CreateSVGPathSegLinetoRel(
                          uSVGLength.GetPXLength(_valuesOfChar[0]),
                          uSVGLength.GetPXLength(_valuesOfChar[1]));
          _segList.AppendItem(temp);
          break;
        }
        case 'C' : {
          uSVGPathSegCurvetoCubicAbs temp = CreateSVGPathSegCurvetoCubicAbs(
                          uSVGLength.GetPXLength(_valuesOfChar[0]),
                          uSVGLength.GetPXLength(_valuesOfChar[1]),
                          uSVGLength.GetPXLength(_valuesOfChar[2]),
                          uSVGLength.GetPXLength(_valuesOfChar[3]),
                          uSVGLength.GetPXLength(_valuesOfChar[4]),
                          uSVGLength.GetPXLength(_valuesOfChar[5]));
          _segList.AppendItem(temp);
          break;
        }
        case 'c' : {
          uSVGPathSegCurvetoCubicRel temp = CreateSVGPathSegCurvetoCubicRel(
                          uSVGLength.GetPXLength(_valuesOfChar[0]),
                          uSVGLength.GetPXLength(_valuesOfChar[1]),
                          uSVGLength.GetPXLength(_valuesOfChar[2]),
                          uSVGLength.GetPXLength(_valuesOfChar[3]),
                          uSVGLength.GetPXLength(_valuesOfChar[4]),
                          uSVGLength.GetPXLength(_valuesOfChar[5]));
          _segList.AppendItem(temp);
          break;
        }
        case 'S' : {
          uSVGPathSegCurvetoCubicSmoothAbs temp =
                        CreateSVGPathSegCurvetoCubicSmoothAbs(
                          uSVGLength.GetPXLength(_valuesOfChar[0]),
                          uSVGLength.GetPXLength(_valuesOfChar[1]),
                          uSVGLength.GetPXLength(_valuesOfChar[2]),
                          uSVGLength.GetPXLength(_valuesOfChar[3]));
          _segList.AppendItem(temp);
          break;
        }
        case 's' : {
          uSVGPathSegCurvetoCubicSmoothRel temp =
                        CreateSVGPathSegCurvetoCubicSmoothRel(
                          uSVGLength.GetPXLength(_valuesOfChar[0]),
                          uSVGLength.GetPXLength(_valuesOfChar[1]),
                          uSVGLength.GetPXLength(_valuesOfChar[2]),
                          uSVGLength.GetPXLength(_valuesOfChar[3]));
          _segList.AppendItem(temp);
          break;
        }
        case 'Q' : {
          uSVGPathSegCurvetoQuadraticAbs temp = CreateSVGPathSegCurvetoQuadraticAbs(
                          uSVGLength.GetPXLength(_valuesOfChar[0]),
                          uSVGLength.GetPXLength(_valuesOfChar[1]),
                          uSVGLength.GetPXLength(_valuesOfChar[2]),
                          uSVGLength.GetPXLength(_valuesOfChar[3]));
          _segList.AppendItem(temp);
          break;
        }
        case 'q' : {
          uSVGPathSegCurvetoQuadraticRel temp = CreateSVGPathSegCurvetoQuadraticRel(
                          uSVGLength.GetPXLength(_valuesOfChar[0]),
                          uSVGLength.GetPXLength(_valuesOfChar[1]),
                          uSVGLength.GetPXLength(_valuesOfChar[2]),
                          uSVGLength.GetPXLength(_valuesOfChar[3]));
          _segList.AppendItem(temp);
          break;
        }
        case 'T' : {
          uSVGPathSegCurvetoQuadraticSmoothAbs temp =
                          CreateSVGPathSegCurvetoQuadraticSmoothAbs(
                          uSVGLength.GetPXLength(_valuesOfChar[0]),
                          uSVGLength.GetPXLength(_valuesOfChar[1]));
          _segList.AppendItem(temp);
          break;
        }
        case 't' : {
          uSVGPathSegCurvetoQuadraticSmoothRel temp =
                        CreateSVGPathSegCurvetoQuadraticSmoothRel(
                          uSVGLength.GetPXLength(_valuesOfChar[0]),
                          uSVGLength.GetPXLength(_valuesOfChar[1]));
          _segList.AppendItem(temp);
          break;
        }
        case 'A' : {
          uSVGPathSegArcAbs temp = CreateSVGPathSegArcAbs(
                          uSVGLength.GetPXLength(_valuesOfChar[0]),
                          uSVGLength.GetPXLength(_valuesOfChar[1]),
                          uSVGLength.GetPXLength(_valuesOfChar[2]),
                          uSVGLength.GetPXLength(_valuesOfChar[3]),
                          uSVGLength.GetPXLength(_valuesOfChar[4]),
                          uSVGLength.GetPXLength(_valuesOfChar[5]),
                          uSVGLength.GetPXLength(_valuesOfChar[6]));
          _segList.AppendItem(temp);
          break;
        }
        case 'a' : {
          uSVGPathSegArcRel temp = CreateSVGPathSegArcRel(
                          uSVGLength.GetPXLength(_valuesOfChar[0]),
                          uSVGLength.GetPXLength(_valuesOfChar[1]),
                          uSVGLength.GetPXLength(_valuesOfChar[2]),
                          uSVGLength.GetPXLength(_valuesOfChar[3]),
                          uSVGLength.GetPXLength(_valuesOfChar[4]),
                          uSVGLength.GetPXLength(_valuesOfChar[5]),
                          uSVGLength.GetPXLength(_valuesOfChar[6]));
          _segList.AppendItem(temp);
          break;
        }
        case 'H' : {
          uSVGPathSegLinetoHorizontalAbs temp = CreateSVGPathSegLinetoHorizontalAbs(
                          uSVGLength.GetPXLength(_valuesOfChar[0]));
          _segList.AppendItem(temp);
          break;
        }
        case 'h' : {
          uSVGPathSegLinetoHorizontalRel temp = CreateSVGPathSegLinetoHorizontalRel(
                          uSVGLength.GetPXLength(_valuesOfChar[0]));
          _segList.AppendItem(temp);
          break;
        }
        case 'V' : {
          uSVGPathSegLinetoVerticalAbs temp = CreateSVGPathSegLinetoVerticalAbs(
                          uSVGLength.GetPXLength(_valuesOfChar[0]));
          _segList.AppendItem(temp);
          break;
        }
        case 'v' : {
          uSVGPathSegLinetoVerticalRel temp = CreateSVGPathSegLinetoVerticalRel(
                          uSVGLength.GetPXLength(_valuesOfChar[0]));
          _segList.AppendItem(temp);
          break;
        }
      }
    }
  }
  /***********************************************************************************/
  //Create Methods
  public uSVGPathSegClosePath CreateSVGPathSegClosePath() {
    uSVGPathSegMovetoAbs _firstPoint = _segList.GetItem(0)as uSVGPathSegMovetoAbs;
    if(_firstPoint == null) {
      uSVGPathSegMovetoRel _firstPoint1 = _segList.GetItem(0)as uSVGPathSegMovetoRel;
      if(_firstPoint1 != null) {
        return new uSVGPathSegClosePath(_firstPoint1.x, _firstPoint1.y);
      }
    } else {
      return new uSVGPathSegClosePath(_firstPoint.x, _firstPoint.y);
    }

    return new uSVGPathSegClosePath(-1f, -1f);
  }
  //--------------
  //MoveToAbs
  public uSVGPathSegMovetoAbs CreateSVGPathSegMovetoAbs(float x, float y) {
    return new uSVGPathSegMovetoAbs(x, y);
  }
  //--------------
  //MoveToRel
  public uSVGPathSegMovetoRel CreateSVGPathSegMovetoRel(float x, float y) {
    return new uSVGPathSegMovetoRel(x, y);
  }

  //--------------
  //LineToAbs
  public uSVGPathSegLinetoAbs CreateSVGPathSegLinetoAbs(float x, float y) {
    return new uSVGPathSegLinetoAbs(x, y);
  }
  //--------------
  //LineToRel
  public uSVGPathSegLinetoRel CreateSVGPathSegLinetoRel(float x, float y) {
    return new uSVGPathSegLinetoRel(x, y);
  }
  //--------------
  //CubicCurveAbs
  public uSVGPathSegCurvetoCubicAbs CreateSVGPathSegCurvetoCubicAbs(float x1,
                  float y1, float x2, float y2, float x, float y) {
    return new uSVGPathSegCurvetoCubicAbs(x1, y1, x2, y2, x, y);
  }
  //--------------
  //CubicCurveRel
  public uSVGPathSegCurvetoCubicRel CreateSVGPathSegCurvetoCubicRel(float x1,
                  float y1, float x2, float y2, float x, float y) {
    return new uSVGPathSegCurvetoCubicRel(x1, y1, x2, y2, x, y);
  }
  //--------------
  //SmoothCubicCurveAbs(S)
  public uSVGPathSegCurvetoCubicSmoothAbs CreateSVGPathSegCurvetoCubicSmoothAbs(float x2,
                                float y2, float x, float y) {
    return new uSVGPathSegCurvetoCubicSmoothAbs(x2, y2, x, y);
  }
  //--------------
  //SmoothCubicCurveRel(s)
  public uSVGPathSegCurvetoCubicSmoothRel CreateSVGPathSegCurvetoCubicSmoothRel(float x2,
                                float y2, float x, float y) {
    return new uSVGPathSegCurvetoCubicSmoothRel(x2, y2, x, y);
  }
  //--------------
  //QuadraticCurveAbs(Q)
  public uSVGPathSegCurvetoQuadraticAbs CreateSVGPathSegCurvetoQuadraticAbs(float x1,
                  float y1, float x, float y) {
    return new uSVGPathSegCurvetoQuadraticAbs(x1, y1, x, y);
  }
  //--------------
  //QuadraticCurveAbs(q)
  public uSVGPathSegCurvetoQuadraticRel CreateSVGPathSegCurvetoQuadraticRel(float x1,
                  float y1, float x, float y) {
    return new uSVGPathSegCurvetoQuadraticRel(x1, y1, x, y);
  }
  //--------------
  //SmoothQuadraticCurveAbs(T)
  public uSVGPathSegCurvetoQuadraticSmoothAbs CreateSVGPathSegCurvetoQuadraticSmoothAbs(float x,
                                float y) {
    return new uSVGPathSegCurvetoQuadraticSmoothAbs(x, y);
  }
  //--------------
  //SmoothQuadraticCurveRel(t)
  public uSVGPathSegCurvetoQuadraticSmoothRel CreateSVGPathSegCurvetoQuadraticSmoothRel(float x,
                                float y) {
    return new uSVGPathSegCurvetoQuadraticSmoothRel(x, y);
  }
  //--------------
  //ArcAbs(A)
  public uSVGPathSegArcAbs CreateSVGPathSegArcAbs(float r1, float r2, float angle,
                          float largeArcFlag, float sweepFlag,
                          float x, float y) {
    return new uSVGPathSegArcAbs(r1, r2, angle, largeArcFlag == 1.0f, sweepFlag == 1.0f, x, y);
  }
  //--------------
  //ArcRel(a)
  public uSVGPathSegArcRel CreateSVGPathSegArcRel(float r1, float r2, float angle,
                          float largeArcFlag, float sweepFlag,
                          float x, float y) {
    return new uSVGPathSegArcRel(r1, r2, angle, largeArcFlag == 1.0f, sweepFlag == 1.0f, x, y);
  }
  //--------------
  //LinetoHorizontalAbs(H)
  public uSVGPathSegLinetoHorizontalAbs CreateSVGPathSegLinetoHorizontalAbs(float x) {
    return new uSVGPathSegLinetoHorizontalAbs(x);
  }
  //--------------
  //LinetoHorizontalRel(h)
  public uSVGPathSegLinetoHorizontalRel CreateSVGPathSegLinetoHorizontalRel(float x) {
    return new uSVGPathSegLinetoHorizontalRel(x);
  }
  //--------------
  //LinetVerticalAbs(V)
  public uSVGPathSegLinetoVerticalAbs CreateSVGPathSegLinetoVerticalAbs(float y) {
    return new uSVGPathSegLinetoVerticalAbs(y);
  }
  //--------------
  //LinetoVerticalRel(v)
  public uSVGPathSegLinetoVerticalRel CreateSVGPathSegLinetoVerticalRel(float y) {
    return new uSVGPathSegLinetoVerticalRel(y);
  }
  //================================================================================
  private uSVGGraphicsPath _graphicsPath;
  private void CreateGraphicsPath() {
    this._graphicsPath = new uSVGGraphicsPath();
    for(int i = 0; i < this._segList.numberOfItems; i++) {
      uISVGDrawableSeg temp = this._segList.GetItem(i)as uISVGDrawableSeg;
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
  public void BeforeRender(uSVGTransformList transformList) {
    this.inheritTransformList = transformList;
    for(int i = 0; i < this._segList.numberOfItems; i++) {
      uISVGDrawable temp = this._segList.GetItem(i)as uISVGDrawable;
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
      case uSVGPaintTypes.SVG_PAINT_SOLID_GRADIENT_FILL : {
        this._render.FillPath(this._paintable.fillColor.Value, this._graphicsPath);
        Draw();
        break;
      }
      case uSVGPaintTypes.SVG_PAINT_LINEAR_GRADIENT_FILL : {

        uSVGLinearGradientBrush _linearGradBrush =
                  this._paintable.GetLinearGradientBrush(this._graphicsPath);

        if(_linearGradBrush != null) {
          this._render.FillPath(_linearGradBrush, _graphicsPath);
        }
        Draw();
        break;
      }
      case uSVGPaintTypes.SVG_PAINT_RADIAL_GRADIENT_FILL : {
        uSVGRadialGradientBrush _radialGradBrush =
                  this._paintable.GetRadialGradientBrush(this._graphicsPath);

        if(_radialGradBrush != null) {
          this._render.FillPath(_radialGradBrush, _graphicsPath);
        }
        Draw();
        break;
      }
      case uSVGPaintTypes.SVG_PAINT_PATH_DRAW : {
        Draw();
        break;
      }
    }
  }
}