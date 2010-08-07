using UnityEngine;
using System;
using System.Collections.Generic;
public class SVGLinearGradientBrush {
  private SVGLinearGradientElement _linearGradElement;
  //-----
  //Gradient Vector
  private float _x1, _y1, _x2, _y2;
  //-----
  private List<Color> _stopColorList;
  private List<float> _stopOffsetList;
  //-----
  private SVGSpreadMethod _spreadMethod;
  /*********************************************************************************/
  public SVGLinearGradientBrush(SVGLinearGradientElement linearGradElement) {
    this._linearGradElement = linearGradElement;
    Initialize();
  }
  public SVGLinearGradientBrush(SVGLinearGradientElement linearGradElement,
                            SVGGraphicsPath graphicsPath) {
    this._linearGradElement = linearGradElement;
    Initialize();

    SetGradientVector(graphicsPath);
    PreLocationProcess();
  }


  /*********************************************************************************/
  private void Initialize() {
    this._x1 = this._linearGradElement.x1.value;
    this._y1 = this._linearGradElement.y1.value;
    this._x2 = this._linearGradElement.x2.value;
    this._y2 = this._linearGradElement.y2.value;

    this._stopColorList = new List<Color>();
    this._stopOffsetList = new List<float>();
    this._spreadMethod = this._linearGradElement.spreadMethod;

    GetStopList();
    this._vitriOffset = 0;
    PreColorProcess(this._vitriOffset);
  }
  //-----
  private void GetStopList() {
    List<SVGStopElement> _stopList = this._linearGradElement.stopList;
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
  private float _deltaR, _deltaG, _deltaB;
  private int _vitriOffset = 0;
  private void PreColorProcess(int index) {
    float dp = _stopOffsetList[index + 1] - _stopOffsetList[index];

    _deltaR = (_stopColorList[index + 1].r - _stopColorList[index].r)/ dp;
    _deltaG = (_stopColorList[index + 1].g - _stopColorList[index].g)/ dp;
    _deltaB = (_stopColorList[index + 1].b - _stopColorList[index].b)/ dp;
  }
  //------
  private float _a, _b, _aP, _bP, _cP;
  private void PreLocationProcess() {
    if((this._x1 - this._x2 == 0f)||(this._y1 - this._y2 == 0f)) {
      return;
    }
    float dx, dy;
    dx = _x2 - _x1;
    dy = _y2 - _y1;

    this._a = dy / dx;
    this._b = this._y1 - this._a * this._x1;

    this._aP = (dx)/( dx + this._a*dy);
    this._bP = (dy)/(dx + this._a*dy);
    this._cP = -(this._b*dy)/(dx + this._a*dy);
  }
  //-----
  private float Percent(float x, float y) {
    float cx, cy;
    if( this._x1 - this. _x2 == 0) {
      cx = this._x1;
      cy = y;
    } else if(this._y1 - this. _y2 == 0) {
      cx = x;
      cy = this._y1;
    } else {
      cx = this._aP * x + this._bP * y + this._cP;
      cy = this._a * cx + this._b;
    }


    float d1 = (float)Math.Sqrt((this._x1 - cx)*(this._x1 - cx)+
               (this._y1 - cy)*(this._y1 - cy));
    float d2 = (float)Math.Sqrt((this._x2 - cx)*(this._x2 - cx)+
               (this._y2 - cy)*(this._y2 - cy));
    float dd = (float)Math.Sqrt((this._x2 - this._x1)*(this._x2 - this._x1)+
         (this._y2 - this._y1)*(this._y2 - this._y1));
    //-1 trai, 0 giua, 1 phai
    int vt = 0;
    if((d1 >= dd)||(d2 >= dd)) {
      if(d1 < d2)vt = -1;
      else vt = 1;
    }

    int _reflectTimes;
    float _remainder;

    switch(this._spreadMethod) {
      case SVGSpreadMethod.Pad :
        if(vt == -1)return 0f;
        if(vt == 1)return 100f;
        return(d1/dd * 100f);
      case SVGSpreadMethod.Reflect :
        _reflectTimes = (int)(d1 / dd);
        _remainder = d1 -(dd *(float)_reflectTimes);
        int _od = (int)(_reflectTimes)% 2;

        return((100f * _od)+(1 - 2 * _od)*(_remainder/dd * 100f));
      case SVGSpreadMethod.Repeat :
        _reflectTimes = (int)(d1 / dd);
        _remainder = d1 -(dd *(float)_reflectTimes);
        return(_remainder/dd * 100f);
    }

    return 100f;
  }
  //-----
  private void SetGradientVector(SVGGraphicsPath graphicsPath) {
    SVGRect bound = graphicsPath.GetBound();
    if(this._linearGradElement.x1.unitType == SVGLengthType.Percentage) {
      this._x1 = bound.x +(bound.width * this._x1 / 100f);
    }

    if(this._linearGradElement.y1.unitType == SVGLengthType.Percentage) {
      this._y1 = bound.y +(bound.height * this._y1 / 100f);
    }

    if(this._linearGradElement.x2.unitType == SVGLengthType.Percentage) {
      this._x2 = bound.x +(bound.width * this._x2 / 100f);
    }

    if(this._linearGradElement.y2.unitType == SVGLengthType.Percentage) {
      this._y2 = bound.y +(bound.height * this._y2 / 100f);
    }

    if(this._linearGradElement.gradientUnits == SVGGradientUnit.ObjectBoundingBox) {
      SVGPoint _point = new SVGPoint(this._x1, this._y1);
      _point = _point.MatrixTransform(graphicsPath.matrixTransform);
      this._x1 = _point.x;
      this._y1 = _point.y;

      _point = new SVGPoint(this._x2, this._y2);
      _point = _point.MatrixTransform(graphicsPath.matrixTransform);
      this._x2 = _point.x;
      this._y2 = _point.y;
    }
  }
  /*********************************************************************************/
  /*private float _ox = 0;
  private int _dem = 0;
  private bool _show = false;*/
  public Color GetColor(float x, float y) {
    Color _color = Color.black;


    /*if(_ox != x) {
      _ox = x;
      _dem ++ ;

      if(_dem < 300) {
        _show = true;
      }
    }*/

    float _percent = Percent(x, y);

    /*if(_show == true) {
      UnityEngine.Debug.Log("x " + x + " y " + y + " percent " + _percent);
    }*/

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