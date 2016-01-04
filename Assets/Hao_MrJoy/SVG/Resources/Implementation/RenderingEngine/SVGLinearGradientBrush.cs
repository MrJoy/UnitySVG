using System;
using System.Collections.Generic;
using UnityEngine;

public class SVGLinearGradientBrush {
  private SVGLinearGradientElement _linearGradElement;
  private float _x1, _y1, _x2, _y2;

  private List<Color> _stopColorList;
  private List<float> _stopOffsetList;

  private SVGSpreadMethod _spreadMethod;

  public SVGLinearGradientBrush(SVGLinearGradientElement linearGradElement) {
    _linearGradElement = linearGradElement;
    Initialize();
  }

  public SVGLinearGradientBrush(SVGLinearGradientElement linearGradElement, SVGGraphicsPath graphicsPath) {
    _linearGradElement = linearGradElement;
    Initialize();

    SetGradientVector(graphicsPath);
    PreLocationProcess();
  }

  private void Initialize() {
    _x1 = _linearGradElement.x1.value;
    _y1 = _linearGradElement.y1.value;
    _x2 = _linearGradElement.x2.value;
    _y2 = _linearGradElement.y2.value;

    _stopColorList = new List<Color>();
    _stopOffsetList = new List<float>();
    _spreadMethod = _linearGradElement.spreadMethod;

    GetStopList();
    _vitriOffset = 0;
    PreColorProcess(_vitriOffset);
  }

  private void GetStopList() {
    List<SVGStopElement> _stopList = _linearGradElement.stopList;
    int _length = _stopList.Count;
    if(_length == 0)
      return;

    _stopColorList.Add(_stopList[0].stopColor.color);
    _stopOffsetList.Add(0f);
    int i = 0;
    for(i = 0; i < _length; i++) {
      float t_offset = _stopList[i].offset;
      if((t_offset > _stopOffsetList[_stopOffsetList.Count - 1]) && (t_offset <= 100f)) {
        _stopColorList.Add(_stopList[i].stopColor.color);
        _stopOffsetList.Add(t_offset);
      } else if(t_offset == _stopOffsetList[_stopOffsetList.Count - 1])
        _stopColorList[_stopOffsetList.Count - 1] = _stopList[i].stopColor.color;
    }

    if(_stopOffsetList[_stopOffsetList.Count - 1] != 100f) {
      _stopColorList.Add(_stopColorList[_stopOffsetList.Count - 1]);
      _stopOffsetList.Add(100f);
    }
  }

  private float _deltaR, _deltaG, _deltaB;
  private int _vitriOffset = 0;

  private void PreColorProcess(int index) {
    float dp = _stopOffsetList[index + 1] - _stopOffsetList[index];

    _deltaR = (_stopColorList[index + 1].r - _stopColorList[index].r) / dp;
    _deltaG = (_stopColorList[index + 1].g - _stopColorList[index].g) / dp;
    _deltaB = (_stopColorList[index + 1].b - _stopColorList[index].b) / dp;
  }

  private float _a, _b, _aP, _bP, _cP;

  private void PreLocationProcess() {
    if((_x1 - _x2 == 0f) || (_y1 - _y2 == 0f))
      return;
    float dx, dy;
    dx = _x2 - _x1;
    dy = _y2 - _y1;

    _a = dy / dx;
    _b = _y1 - _a * _x1;

    _aP = (dx) / (dx + _a * dy);
    _bP = (dy) / (dx + _a * dy);
    _cP = -(_b * dy) / (dx + _a * dy);
  }
  //-----
  private float Percent(float x, float y) {
    float cx, cy;
    if(_x1 - _x2 == 0) {
      cx = _x1;
      cy = y;
    } else if(_y1 - _y2 == 0) {
      cx = x;
      cy = _y1;
    } else {
      cx = _aP * x + _bP * y + _cP;
      cy = _a * cx + _b;
    }


    float d1 = (float)Math.Sqrt((_x1 - cx) * (_x1 - cx) + (_y1 - cy) * (_y1 - cy));
    float d2 = (float)Math.Sqrt((_x2 - cx) * (_x2 - cx) + (_y2 - cy) * (_y2 - cy));
    float dd = (float)Math.Sqrt((_x2 - _x1) * (_x2 - _x1) + (_y2 - _y1) * (_y2 - _y1));
    //-1 trai, 0 giua, 1 phai
    int vt = 0;
    if((d1 >= dd) || (d2 >= dd))
      vt = (d1 < d2) ? -1 : 1;

    int _reflectTimes;
    float _remainder;

    switch(_spreadMethod) {
    case SVGSpreadMethod.Pad:
      if(vt == -1)
        return 0f;
      if(vt == 1)
        return 100f;
      return (d1 / dd * 100f);
    case SVGSpreadMethod.Reflect:
      _reflectTimes = (int)(d1 / dd);
      _remainder = d1 - (dd * (float)_reflectTimes);
      int _od = (int)(_reflectTimes) % 2;

      return ((100f * _od) + (1 - 2 * _od) * (_remainder / dd * 100f));
    case SVGSpreadMethod.Repeat:
      _reflectTimes = (int)(d1 / dd);
      _remainder = d1 - (dd * (float)_reflectTimes);
      return (_remainder / dd * 100f);
    }

    return 100f;
  }

  private void SetGradientVector(SVGGraphicsPath graphicsPath) {
    Rect bound = graphicsPath.GetBound();
    if(_linearGradElement.x1.unitType == SVGLengthType.Percentage)
      _x1 = bound.x + (bound.width * _x1 / 100f);
    if(_linearGradElement.y1.unitType == SVGLengthType.Percentage)
      _y1 = bound.y + (bound.height * _y1 / 100f);
    if(_linearGradElement.x2.unitType == SVGLengthType.Percentage)
      _x2 = bound.x + (bound.width * _x2 / 100f);
    if(_linearGradElement.y2.unitType == SVGLengthType.Percentage)
      _y2 = bound.y + (bound.height * _y2 / 100f);

    if(_linearGradElement.gradientUnits == SVGGradientUnit.ObjectBoundingBox) {
      Vector2 _point = graphicsPath.matrixTransform.Transform(new Vector2(_x1, _y1));
      _x1 = _point.x;
      _y1 = _point.y;

      _point = graphicsPath.matrixTransform.Transform(new Vector2(_x2, _y2));
      _x2 = _point.x;
      _y2 = _point.y;
    }
  }

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

    if((_stopOffsetList[_vitriOffset] <= _percent) && (_percent <= _stopOffsetList[_vitriOffset + 1])) {
      _color.r = ((_percent - _stopOffsetList[_vitriOffset]) * _deltaR) + _stopColorList[_vitriOffset].r;
      _color.g = ((_percent - _stopOffsetList[_vitriOffset]) * _deltaG) + _stopColorList[_vitriOffset].g;
      _color.b = ((_percent - _stopOffsetList[_vitriOffset]) * _deltaB) + _stopColorList[_vitriOffset].b;

    } else {
      for(int i = 0; i < _stopOffsetList.Count - 1; i++)
        if((_stopOffsetList[i] <= _percent) && (_percent <= _stopOffsetList[i + 1])) {
          _vitriOffset = i;
          PreColorProcess(_vitriOffset);

          _color.r = ((_percent - _stopOffsetList[i]) * _deltaR) + _stopColorList[i].r;
          _color.g = ((_percent - _stopOffsetList[i]) * _deltaG) + _stopColorList[i].g;
          _color.b = ((_percent - _stopOffsetList[i]) * _deltaB) + _stopColorList[i].b;
          break;
        }
    }
    //_show = false;
    return _color;
  }
}
