using UnityEngine;
using System;
using System.Collections.Generic;

public class uSVGRadialGradientBrush {
  private uSVGRadialGradientElement _radialGradElement;
  //-----
  //Gradient Circle
  private float _cx, _cy, _r, _fx, _fy;
  //-----
  private List<Color> _stopColorList;
  private List<float> _stopOffsetList;
  //-----
  private uSVGSpreadMethodTypes _spreadMethod;
  /*********************************************************************************/
  public uSVGRadialGradientBrush(uSVGRadialGradientElement radialGradElement) {
    this._radialGradElement = radialGradElement;
    Initialize();
  }
  public uSVGRadialGradientBrush(uSVGRadialGradientElement radialGradElement,
                            uSVGGraphicsPath graphicsPath) {
    this._radialGradElement = radialGradElement;
    Initialize();

    SetGradientVector(graphicsPath);
  }
  /*********************************************************************************/
  private void Initialize() {
    this._cx = this._radialGradElement.cx.value;
    this._cy = this._radialGradElement.cy.value;
    this._r = this._radialGradElement.r.value;
    this._fx = this._radialGradElement.fx.value;
    this._fy = this._radialGradElement.fy.value;

    this._stopColorList = new List<Color>();
    this._stopOffsetList = new List<float>();
    this._spreadMethod = this._radialGradElement.spreadMethod;

    GetStopList();
    FixF();
    this._vitriOffset = 0;
    PreColorProcess(this._vitriOffset);

  }
  //-----
  //Sap xep lai Offset va Stop-color List
  private void GetStopList() {
    List<uSVGStopElement> _stopList = this._radialGradElement.stopList;
    int _length = _stopList.Count;
    if(_length == 0)return;

    _stopColorList.Add(_stopList[0].stopColor.color);
    _stopOffsetList.Add(0f);
    int i = 0;
    for(i = 0; i < _length; i++) {
      float t_offset = _stopList[i].offset.value;
      if((t_offset > _stopOffsetList[_stopOffsetList.Count - 1]) &&(t_offset <= 100f)) {
        _stopColorList.Add(_stopList[i].stopColor.color);
        _stopOffsetList.Add(t_offset);
      } else if(t_offset == _stopOffsetList[_stopOffsetList.Count - 1]) {
        _stopColorList[_stopOffsetList.Count - 1] = _stopList[i].stopColor.color;
      }
    }

    if(_stopOffsetList[_stopOffsetList.Count - 1] != 100f) {
      _stopColorList.Add(_stopColorList[_stopOffsetList.Count - 1]);
      _stopOffsetList.Add(100f);
    }
  }
  //-----
  //Sua lai vi tri cua diem x,y
  private void FixF() {
    if((float)Math.Sqrt((this._fx-this._cx)*(this._fx-this._cx))+((this._fy-this._cy)*(this._fy-this._cy)) > this._r) {

      float dx = this._fx - this._cx;
      float dy = this._fy - this._cy;

      if(dx == 0) {
        this._fy = (this._fy > this._cy) ? (this._cy + this._r) : (this._cy - this._r);
      } else {
        float a, b;
        a = dy / dx;
        b = this._fy - a * this._fx;

        double ta, tb, tc;

        ta = 1 + a * a;
        tb = 2 *(a *(b - this._cy)- this._cx);
        tc = (this._cx * this._cx)+(b - this._cy)*(b - this._cy)-(this._r * this._r);

        float delta = (float)((tb * tb)- 4 * ta * tc);

        delta = (float)Math.Sqrt(delta);
        float x1 = (float)((-tb + delta)/(2 * ta));
        float y1 = (float)(a * x1 + b);
        float x2 = (float)((-tb - delta)/(2 * ta));
        float y2 = (float)(a * x2 + b);

        if((this._cx < x1) &&(x1 < this._fx)) {
          this._fx = x1 - 1;
          this._fy = y1;
        } else {
          this._fx = x2 + 1;
          this._fy = y2;
        }
      }
    }
  }
  //-----
  private float _deltaR, _deltaG, _deltaB;
  private int _vitriOffset = 0;
  private void PreColorProcess(int index) {
    float dp = _stopOffsetList[index + 1] - _stopOffsetList[index];

    _deltaR = (_stopColorList[index + 1].r - _stopColorList[index].r)/ dp;
    _deltaG = (_stopColorList[index + 1].g - _stopColorList[index].g)/ dp;
    _deltaB = (_stopColorList[index + 1].b - _stopColorList[index].b)/ dp;
  }
  //----
  //Tim giao diem giua duong thang(x,y)->(fx, fy)voi duong tron
  private uSVGPoint CrossPoint(float x, float y) {
    uSVGPoint _point = new uSVGPoint(0f, 0f);

    float dx = this._fx - x;
    float dy = this._fy - y;

    if(dx == 0) {
      _point.x = this._fx;
      _point.y = (this._fy > y) ? (this._fy - this._r) : (this._fy + this._r);
    } else {
      float a, b;
      a = dy / dx;
      b = this._fy - a * this._fx;

      double ta, tb, tc;

      ta = 1 + a * a;
      tb = 2 *(a *(b - this._cy)- this._cx);
      tc = (this._cx * this._cx)+(b - this._cy)*(b - this._cy)-(this._r * this._r);

      float delta = (float)((tb * tb)- 4 * ta * tc);

      delta = (float)Math.Sqrt(delta);
      float x1 = (float)((-tb + delta)/(2 * ta));
      float y1 = (float)(a * x1 + b);
      float x2 = (float)((-tb - delta)/(2 * ta));
      float y2 = (float)(a * x2 + b);

      uSVGPoint vt1 = new uSVGPoint(x1 - this._fx, y1 - this._fy);
      uSVGPoint vt2 = new uSVGPoint(x - this._fx, y - this._fy);

      if(((vt1.x * vt2.x) >= 0)&&((vt1.y * vt2.y) >=0)) {
        _point.x = x1;
        _point.y = y1;
      } else {
        _point.x = x2;
        _point.y = y2;
      }
    }
    return _point;
  }
  //-----
  //Tinh % tai vi tri x,y
  private float Percent(float x, float y) {
    uSVGPoint _cP = CrossPoint(x, y);

    //float d1 = (float)Math.Sqrt((_cP.x - x)*(_cP.x - x)+(_cP.y - y)*(_cP.y - y));
    float d2 = (float)Math.Sqrt((this._fx - x)*(this._fx - x)+
               (this._fy - y)*(this._fy - y));
    float dd = (float)Math.Sqrt((_cP.x - this._fx)*(_cP.x - this._fx)+
         (_cP.y - this._fy)*(_cP.y - this._fy ));
    //0 giua, 1 ngoai
    int vt = 0;
    if(d2 > dd) {
      vt = 1;
    }

    int _reflectTimes;
    float _remainder;

    switch(this._spreadMethod) {
      case uSVGSpreadMethodTypes.SVG_SPREADMETHOD_PAD :
        if(vt == 1)return 100f;
        return(d2/dd * 100f);
      case uSVGSpreadMethodTypes.SVG_SPREADMETHOD_REFLECT :
        _reflectTimes = (int)(d2 / dd);
        _remainder = d2 -(dd *(float)_reflectTimes);
        int _od = (int)(_reflectTimes)% 2;
        return((100f * _od)+(1 - 2 * _od)*(_remainder/dd * 100f));
      case uSVGSpreadMethodTypes.SVG_SPREADMETHOD_REPEAT :
        _reflectTimes = (int)(d2 / dd);
        _remainder = d2 -(dd *(float)_reflectTimes);
        return(_remainder/dd * 100f);
    }

    return 100f;
  }
  //-----
  private void SetGradientVector(uSVGGraphicsPath graphicsPath) {
    uSVGRect bound = graphicsPath.GetBound();

    if(this._radialGradElement.cx.unitType == uSVGLengthType.SVG_LENGTHTYPE_PERCENTAGE) {
      this._cx = bound.x +(bound.width * this._cx / 100f);
    }

    if(this._radialGradElement.cy.unitType == uSVGLengthType.SVG_LENGTHTYPE_PERCENTAGE) {
      this._cy = bound.y +(bound.height * this._cy / 100f);
    }

    if(this._radialGradElement.r.unitType == uSVGLengthType.SVG_LENGTHTYPE_PERCENTAGE) {
      uSVGPoint _p1 = new uSVGPoint(bound.x, bound.y);
      uSVGPoint _p2 = new uSVGPoint(bound.x + bound.width, bound.y + bound.height);
      _p1 = _p1.MatrixTransform(graphicsPath.matrixTransform);
      _p2 = _p2.MatrixTransform(graphicsPath.matrixTransform);

      float dd = (float)Math.Sqrt((_p2.x - _p1.x)*(_p2.x - _p1.x)+
                   (_p2.y - _p1.y)*(_p2.y - _p1.y));
      this._r = (dd * this._r / 100f);
    }

    if(this._radialGradElement.fx.unitType == uSVGLengthType.SVG_LENGTHTYPE_PERCENTAGE) {
      this._fx = bound.x +(bound.width * this._fx / 100f);
    }
    if(this._radialGradElement.fy.unitType == uSVGLengthType.SVG_LENGTHTYPE_PERCENTAGE) {
      this._fy = bound.y +(bound.height * this._fy / 100f);
    }


    if((float)Math.Sqrt((this._cx - this._fx)*(this._cx - this._fx)+
           (this._cy - this._fy)*(this._cy - this._fy)) > this._r) {
      uSVGPoint _cP = CrossPoint(this._cx, this._cy);
      this._fx = _cP.x;
      this._fy = _cP.y;
    }



    if(this._radialGradElement.gradientUnits == uSVGGradientUnitType.SVG_OBJECT_BOUNDING_BOX) {
      uSVGPoint _point = new uSVGPoint(this._cx, this._cy);
      _point = _point.MatrixTransform(graphicsPath.matrixTransform);
      this._cx = _point.x;
      this._cy = _point.y;

      _point = new uSVGPoint(this._fx, this._fy);
      _point = _point.MatrixTransform(graphicsPath.matrixTransform);
      this._fx = _point.x;
      this._fy = _point.y;
    }
  }
  /*********************************************************************************/
  //private float _ox = 0;
  //private int _dem = 0;
  //private bool _show = false;
  public Color GetColor(float x, float y) {
    Color _color = Color.black;


    //if((_ox != x)&&(y == 200)) {
    //  _ox = x;
    //  _dem ++ ;

    //  if(_dem < 300) {
    //    _show = true;
    //  }
    //}

    float _percent = Percent(x, y);

    //if(_show == true) {
    //  UnityEngine.Debug.Log("x " + x + " y " + y + " percent " + _percent);
    //}

    if((_stopOffsetList[_vitriOffset] <= _percent)&&
             (_percent <= _stopOffsetList[_vitriOffset+1])) {
      _color.r = ((_percent - _stopOffsetList[_vitriOffset])* _deltaR)+
                            _stopColorList[_vitriOffset].r;
      _color.g = ((_percent - _stopOffsetList[_vitriOffset])* _deltaG)+
                            _stopColorList[_vitriOffset].g;
      _color.b = ((_percent - _stopOffsetList[_vitriOffset])* _deltaB)+
                            _stopColorList[_vitriOffset].b;

    } else {
      for(int i = 0;  i < _stopOffsetList.Count - 1; i++) {
        if((_stopOffsetList[i] <= _percent)&&(_percent <= _stopOffsetList[i+1])) {
          _vitriOffset = i;
          PreColorProcess(_vitriOffset);

          _color.r = ((_percent - _stopOffsetList[i])* _deltaR)+
                                _stopColorList[i].r;
          _color.g = ((_percent - _stopOffsetList[i])* _deltaG)+
                                _stopColorList[i].g;
          _color.b = ((_percent - _stopOffsetList[i])* _deltaB)+
                                _stopColorList[i].b;
          break;
        }
      }
    }
    //_show = false;
    return _color;
  }
}
