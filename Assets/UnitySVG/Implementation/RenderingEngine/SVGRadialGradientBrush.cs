using System;
using System.Collections.Generic;
using UnityEngine;

public class SVGRadialGradientBrush {
  private SVGRadialGradientElement _radialGradElement;

  private float _cx, _cy, _r, _fx, _fy;

  private List<Color> _stopColorList;
  private List<float> _stopOffsetList;

  private SVGSpreadMethod _spreadMethod;

  public SVGRadialGradientBrush(SVGRadialGradientElement radialGradElement) {
    _radialGradElement = radialGradElement;
    Initialize();
  }

  public SVGRadialGradientBrush(SVGRadialGradientElement radialGradElement, SVGGraphicsPath graphicsPath) {
    _radialGradElement = radialGradElement;
    Initialize();

    SetGradientVector(graphicsPath);
  }

  private void Initialize() {
    _cx = _radialGradElement.cx.value;
    _cy = _radialGradElement.cy.value;
    _r = _radialGradElement.r.value;
    _fx = _radialGradElement.fx.value;
    _fy = _radialGradElement.fy.value;

    _stopColorList = new List<Color>();
    _stopOffsetList = new List<float>();
    _spreadMethod = _radialGradElement.spreadMethod;

    GetStopList();
    FixF();
    _vitriOffset = 0;
    PreColorProcess(_vitriOffset);

  }

  //Sap xep lai Offset va Stop-color List
  private void GetStopList() {
    List<SVGStopElement> _stopList = _radialGradElement.stopList;
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

  //Sua lai vi tri cua diem x,y
  private void FixF() {
    // TODO: Measure performance of comparing squared values instead of taking square root.
    if((float)Math.Sqrt((_fx - _cx) * (_fx - _cx)) + ((_fy - _cy) * (_fy - _cy)) > _r) {

      float dx = _fx - _cx;
      float dy = _fy - _cy;

      if(dx == 0)
        _fy = (_fy > _cy) ? (_cy + _r) : (_cy - _r);
      else {
        float a, b;
        a = dy / dx;
        b = _fy - a * _fx;

        double ta, tb, tc;

        ta = 1 + a * a;
        tb = 2 * (a * (b - _cy) - _cx);
        tc = (_cx * _cx) + (b - _cy) * (b - _cy) - (_r * _r);

        float delta = (float)((tb * tb) - 4 * ta * tc);

        delta = (float)Math.Sqrt(delta);
        float x1 = (float)((-tb + delta) / (2 * ta));
        float y1 = (float)(a * x1 + b);
        float x2 = (float)((-tb - delta) / (2 * ta));
        float y2 = (float)(a * x2 + b);

        if((_cx < x1) && (x1 < _fx)) {
          _fx = x1 - 1;
          _fy = y1;
        } else {
          _fx = x2 + 1;
          _fy = y2;
        }
      }
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

  //Tim giao diem giua duong thang(x,y)->(fx, fy)voi duong tron
  private Vector2 CrossPoint(float x, float y) {
    Vector2 _point = new Vector2(0f, 0f);

    float dx = _fx - x;
    float dy = _fy - y;

    if(dx == 0) {
      _point.x = _fx;
      _point.y = (_fy > y) ? (_fy - _r) : (_fy + _r);
    } else {
      float a, b;
      a = dy / dx;
      b = _fy - a * _fx;

      double ta, tb, tc;

      ta = 1 + a * a;
      tb = 2 * (a * (b - _cy) - _cx);
      tc = (_cx * _cx) + (b - _cy) * (b - _cy) - (_r * _r);

      float delta = (float)((tb * tb) - 4 * ta * tc);

      delta = (float)Math.Sqrt(delta);
      float x1 = (float)((-tb + delta) / (2 * ta));
      float y1 = (float)(a * x1 + b);
      float x2 = (float)((-tb - delta) / (2 * ta));
      float y2 = (float)(a * x2 + b);

      Vector2 vt1 = new Vector2(x1 - _fx, y1 - _fy);
      Vector2 vt2 = new Vector2(x - _fx, y - _fy);

      if(((vt1.x * vt2.x) >= 0) && ((vt1.y * vt2.y) >= 0)) {
        _point.x = x1;
        _point.y = y1;
      } else {
        _point.x = x2;
        _point.y = y2;
      }
    }
    return _point;
  }

  //Tinh % tai vi tri x,y
  private float Percent(float x, float y) {
    Vector2 _cP = CrossPoint(x, y);

    //float d1 = (float)Math.Sqrt((_cP.x - x)*(_cP.x - x)+(_cP.y - y)*(_cP.y - y));
    float d2 = (float)Math.Sqrt((_fx - x) * (_fx - x) + (_fy - y) * (_fy - y));
    float dd = (float)Math.Sqrt((_cP.x - _fx) * (_cP.x - _fx) + (_cP.y - _fy) * (_cP.y - _fy));
    //0 giua, 1 ngoai
    int vt = 0;
    if(d2 > dd)
      vt = 1;

    int _reflectTimes;
    float _remainder;

    switch(_spreadMethod) {
    case SVGSpreadMethod.Pad:
      if(vt == 1)
        return 100f;
      return (d2 / dd * 100f);
    case SVGSpreadMethod.Reflect:
      _reflectTimes = (int)(d2 / dd);
      _remainder = d2 - (dd * (float)_reflectTimes);
      int _od = (int)(_reflectTimes) % 2;
      return ((100f * _od) + (1 - 2 * _od) * (_remainder / dd * 100f));
    case SVGSpreadMethod.Repeat:
      _reflectTimes = (int)(d2 / dd);
      _remainder = d2 - (dd * (float)_reflectTimes);
      return (_remainder / dd * 100f);
    }

    return 100f;
  }

  private void SetGradientVector(SVGGraphicsPath graphicsPath) {
    Rect bound = graphicsPath.GetBound();

    if(_radialGradElement.cx.unitType == SVGLengthType.Percentage)
      _cx = bound.x + (bound.width * _cx / 100f);

    if(_radialGradElement.cy.unitType == SVGLengthType.Percentage)
      _cy = bound.y + (bound.height * _cy / 100f);

    if(_radialGradElement.r.unitType == SVGLengthType.Percentage) {
      Vector2 _p1 = new Vector2(bound.x, bound.y);
      Vector2 _p2 = new Vector2(bound.x + bound.width, bound.y + bound.height);
      _p1 = graphicsPath.matrixTransform.Transform(_p1);
      _p2 = graphicsPath.matrixTransform.Transform(_p2);

      float dd = (float)Math.Sqrt((_p2.x - _p1.x) * (_p2.x - _p1.x) + (_p2.y - _p1.y) * (_p2.y - _p1.y));
      _r = (dd * _r / 100f);
    }

    if(_radialGradElement.fx.unitType == SVGLengthType.Percentage)
      _fx = bound.x + (bound.width * _fx / 100f);
    if(_radialGradElement.fy.unitType == SVGLengthType.Percentage)
      _fy = bound.y + (bound.height * _fy / 100f);

    if((float)Math.Sqrt((_cx - _fx) * (_cx - _fx) + (_cy - _fy) * (_cy - _fy)) > _r) {
      Vector2 _cP = CrossPoint(_cx, _cy);
      _fx = _cP.x;
      _fy = _cP.y;
    }

    if(_radialGradElement.gradientUnits == SVGGradientUnit.ObjectBoundingBox) {
      Vector2 _point = new Vector2(_cx, _cy);
      _point = graphicsPath.matrixTransform.Transform(_point);
      _cx = _point.x;
      _cy = _point.y;

      _point = new Vector2(_fx, _fy);
      _point = graphicsPath.matrixTransform.Transform(_point);
      _fx = _point.x;
      _fy = _point.y;
    }
  }

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
