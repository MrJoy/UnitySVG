using UnityEngine;
using System;
using System.Collections.Generic;

public delegate void SetPixelDelegate(int x, int y);

public class SVGBasicDraw {
  private struct SVGPointExt {
    private float _delta;
    private SVGPoint _point;
    public float t {
      get { return this._delta; }
    }
    public SVGPoint point {
      get { return this._point; }
    }
    public SVGPointExt(SVGPoint point, float t) {
      this._point = point;
      this._delta = t;
    }
  }

  private SVGPoint _currentPoint;
  public SetPixelDelegate SetPixel;

  public SVGPoint currentPoint {
    get { return this._currentPoint; }
  }
  public SetPixelDelegate SetPixelMethod {
    set{ SetPixel = value;}
  }

  public SVGBasicDraw() {
    this._currentPoint = new SVGPoint(0f, 0f);
  }

  private static void Swap<T>(ref T x1, ref T x2) {
    T temp;
    temp = x1;
    x1 = x2;
    x2 = temp;
  }

  public void MoveTo(float x, float y) {
    this._currentPoint.x = x;
    this._currentPoint.y = y;
  }

  public void MoveTo(SVGPoint p) {
    this._currentPoint.SetValue(p);
  }

  public void Line(int x0, int y0, int x1, int y1) {
    bool steep = (Math.Abs(y1 - y0) > Math.Abs(x1 - x0));
    if(steep) {
      Swap(ref x0, ref y0);
      Swap(ref x1, ref y1);
    }

    if(x0 > x1) {
      Swap(ref x0, ref x1);
      Swap(ref y0, ref y1);
    }

    int deltax = x1 - x0;
    int deltay = Math.Abs(y1 - y0);
    int error = -(deltax + 1) / 2;
    int ystep;
    int y = y0;
    if(y0 < y1) {
      ystep = 1;
    } else {
      ystep = -1;
    }

    for(int x = x0; x <= x1; x++) {
      if(steep) {
        SetPixel(y, x);
      } else {
        SetPixel(x, y);
      }
      error += deltay;
      if(error >= 0) {
        y += ystep;
        error -= deltax;
      }
    }
  }

  public void Line(float x0, float y0, float x1, float y1) {
    Line((int)x0, (int)y0, (int)x1, (int)y1);
  }

  public void Line(SVGPoint p1, SVGPoint p2) {
    Line(p1.x, p1.y, p2.x, p2.y);
  }

  public void LineTo(float x, float y) {
    SVGPoint temp = new SVGPoint(x, y);
    Line(this._currentPoint, temp);
    this._currentPoint.SetValue(temp);
  }

  public void LineTo(SVGPoint p) {
    Line(this._currentPoint, p);
    this._currentPoint.SetValue(p);
  }

  public void Rect(float x0, float y0, float x1, float y1) {
    MoveTo(x0, y0);
    LineTo(x1, y0);
    MoveTo(x1, y0);
    LineTo(x1, y1);
    MoveTo(x1, y1);
    LineTo(x0, y1);
    MoveTo(x0, y1);
    LineTo(x0, y0);

  }

  public void Rect(SVGPoint p1, SVGPoint p2) {
    Rect(p1.x, p1.y, p2.x, p2.y);
  }

  public void Rect(SVGPoint p1, SVGPoint p2, SVGPoint p3, SVGPoint p4) {
    MoveTo(p1);
    LineTo(p2);
    LineTo(p3);
    LineTo(p4);
    LineTo(p1);
  }

  private void Circle(int x0, int y0, float radius) {
    float chuvi = 2f * Mathf.PI * radius;
    int _delta = (int)(chuvi / 2f);
    if(_delta > 50)
      _delta = 50;
    float _angle = (2 * Mathf.PI) / (float)_delta;

    float tx, ty, temp;
    tx = x0;
    ty = radius + y0;
    
    SVGPoint fPoint = new SVGPoint(tx, ty);
    MoveTo(fPoint);
    for(int i = 1; i <= _delta; i++) {
      temp = i * _angle;
      tx = radius * (float)Math.Sin(temp) + x0;
      ty = radius * (float)Math.Cos(temp) + y0;
      SVGPoint tPoint = new SVGPoint(tx, ty);
      LineTo(tPoint);
    }
    LineTo(fPoint);
  }

  public void Circle(float x0, float y0, float r) {
    Circle((int)x0, (int)y0, r);
  }

  public void Circle(SVGPoint p, float r) {
    Circle((int)p.x, (int)p.y, r);
  }

  private void Ellipse(int cx, int cy, int rx, int ry, float angle) {
    float chuvi = 2f * Mathf.PI * (float)Math.Sqrt(rx * rx + ry * ry);
    int steps = (int)(chuvi / 3f);
    if(steps > 50)
      steps = 50;
    float beta = (float)angle / 180.0f * Mathf.PI;
    float sinbeta = (float)Math.Sin(beta);
    float cosbeta = (float)Math.Cos(beta);

    steps = 360 / steps;

    int i = 0;
    float alpha = (float)i / 180.0f * Mathf.PI;
    float sinalpha = (float)Math.Sin(alpha);
    float cosalpha = (float)Math.Cos(alpha);

    float _x = cx + (rx * cosalpha * cosbeta - ry * sinalpha * sinbeta);
    float _y = cy + (rx * cosalpha * sinbeta + ry * sinalpha * cosbeta);

    float _fPointx = _x;
    float _fPointy = _y;
    MoveTo(_x, _y);

    for(i = 1; i < 360; i += steps) {
      alpha = (float)i / 180.0f * Mathf.PI;
      sinalpha = (float)Math.Sin(alpha);
      cosalpha = (float)Math.Cos(alpha);

      _x = cx + (rx * cosalpha * cosbeta - ry * sinalpha * sinbeta);
      _y = cy + (rx * cosalpha * sinbeta + ry * sinalpha * cosbeta);
      LineTo(_x, _y);
    }
    LineTo(_fPointx, _fPointy);
  }

  public void Ellipse(float x0, float y0, float rx, float ry, float angle) {
    Ellipse((int)x0, (int)y0, (int)rx, (int)ry, angle);
  }

  public void Ellipse(SVGPoint p, float rx, float ry, float angle) {
    Ellipse((int)p.x, (int)p.y, (int)rx, (int)ry, angle);
  }

  public void Arc(SVGPoint p1, float rx, float ry, float angle, bool largeArcFlag, bool sweepFlag, SVGPoint p2) {
    float tx, ty;
    double trx2, try2, tx2, ty2;
    float temp1, temp2;
    float _radian = (angle * Mathf.PI / 180.0f);
    float _CosRadian = (float)Math.Cos(_radian);
    float _SinRadian = (float)Math.Sin(_radian);
    temp1 = (p1.x - p2.x) / 2.0f;
    temp2 = (p1.y - p2.y) / 2.0f;
    tx = (_CosRadian * temp1) + (_SinRadian * temp2);
    ty = (-_SinRadian * temp1) + (_CosRadian * temp2);

    trx2 = rx * rx;
    try2 = ry * ry;
    tx2 = tx * tx;
    ty2 = ty * ty;


    double radiiCheck = tx2 / trx2 + ty2 / try2;
    if(radiiCheck > 1) {
      rx = (float)(float)Math.Sqrt((float)radiiCheck) * rx;
      ry = (float)Math.Sqrt((float)radiiCheck) * ry;
      trx2 = rx * rx;
      try2 = ry * ry;
    }

    double tm1;
    tm1 = (trx2 * try2 - trx2 * ty2 - try2 * tx2) / (trx2 * ty2 + try2 * tx2);
    tm1 = (tm1 < 0) ? 0 : tm1;

    float tm2;
    tm2 = (largeArcFlag == sweepFlag) ? -(float)Math.Sqrt((float)tm1) : (float)Math.Sqrt((float)tm1);


    float tcx, tcy;
    tcx = tm2 * ((rx * ty) / ry);
    tcy = tm2 * (-(ry * tx) / rx);

    float cx, cy;
    cx = _CosRadian * tcx - _SinRadian * tcy + ((p1.x + p2.x) / 2.0f);
    cy = _SinRadian * tcx + _CosRadian * tcy + ((p1.y + p2.y) / 2.0f);

    float ux = (tx - tcx) / rx;
    float uy = (ty - tcy) / ry;
    float vx = (-tx - tcx) / rx;
    float vy = (-ty - tcy) / ry;
    float _angle, _delta;

    float p, n, t;
    n = (float)Math.Sqrt((ux * ux) + (uy * uy));
    p = ux;
    _angle = (uy < 0) ? -(float)Math.Acos(p / n) : (float)Math.Acos(p / n);
    _angle = _angle * 180.0f / Mathf.PI;
    _angle %= 360f;

    n = (float)Math.Sqrt((ux * ux + uy * uy) * (vx * vx + vy * vy));
    p = ux * vx + uy * vy;
    t = p / n;
    if((Math.Abs(t) >= 0.99999f) && (Math.Abs(t) < 1.000009f)) {
      if(t > 0)
        t = 1f;
      else
        t = -1f;
    }
    _delta = (ux * vy - uy * vx < 0) ? -(float)Math.Acos(t) : (float)Math.Acos(t);

    _delta = _delta * 180.0f / Mathf.PI;

    if(!sweepFlag && _delta > 0) {
      _delta -= 360f;
    } else if(sweepFlag && _delta < 0)
      _delta += 360f;

    _delta %= 360f;

    int number = 100;
    float deltaT = _delta / number;
    
    SVGPoint _point = new SVGPoint(0, 0);
    float t_angle;
    for(int i = 0; i <= number; i++) {
      t_angle = (deltaT * i + _angle) * Mathf.PI / 180.0f;
      _point.x = _CosRadian * rx * (float)Math.Cos(t_angle) - _SinRadian * ry * (float)Math.Sin(t_angle) + cx;
      _point.y = _SinRadian * rx * (float)Math.Cos(t_angle) + _CosRadian * ry * (float)Math.Sin(t_angle) + cy;
      LineTo(_point);
    }
  }

  public void ArcTo(float r1, float r2, float angle, bool largeArcFlag, bool sweepFlag, SVGPoint p) {
    SVGPoint _tempPoint = new SVGPoint(this._currentPoint.x, this._currentPoint.y);
    Arc(_tempPoint, r1, r2, angle, largeArcFlag, sweepFlag, p);
    this._currentPoint.SetValue(p);
  }

  private float BelongPosition(SVGPoint a, SVGPoint b, SVGPoint c) {
    float _up, _under, _r;
    _up = ((a.y - c.y) * (b.x - a.x)) - ((a.x - c.x) * (b.y - a.y));
    _under = ((b.x - a.x) * (b.x - a.x)) + ((b.y - a.y) * (b.y - a.y));
    _r = _up / _under;
    return _r;
  }
  //Caculate Distance from c point to line segment [a,b]
  //return d point is the point on that line segment.
  private int NumberOfLimitForCubic(SVGPoint a, SVGPoint b, SVGPoint c, SVGPoint d) {
    float _r1 = BelongPosition(a, d, b);
    float _r2 = BelongPosition(a, d, c);
    if((_r1 * _r2) > 0)
      return 0;
    else
      return 1;
  }
  private float Distance(SVGPoint a, SVGPoint b, SVGPoint c) {
    float _up, _under, _distance;
    _up = ((a.y - c.y) * (b.x - a.x)) - ((a.x - c.x) * (b.y - a.y));
    _under = ((b.x - a.x) * (b.x - a.x)) + ((b.y - a.y) * (b.y - a.y));
    _distance = Math.Abs(_up / _under) * (float)Math.Sqrt(_under);
    return _distance;
  }

  private static SVGPoint EvaluateForCubic(float t, SVGPoint p1, SVGPoint p2, SVGPoint p3, SVGPoint p4) {
    SVGPoint _return = new SVGPoint(0, 0);
    float b0, b1, b2, b3, b4;
    b0 = (1.0f - t);
    b1 = b0 * b0 * b0;
    b2 = 3 * t * b0 * b0;
    b3 = 3 * t * t * b0;
    b4 = t * t * t;
    _return.x = b1 * p1.x + b2 * p2.x + b3 * p3.x + b4 * p4.x;
    _return.y = b1 * p1.y + b2 * p2.y + b3 * p3.y + b4 * p4.y;
    return _return;
  }

  private static SVGPoint EvaluateForQuadratic(float t, SVGPoint p1, SVGPoint p2, SVGPoint p3, SVGPoint p4) {
    SVGPoint _return = new SVGPoint(0, 0);
    float b0, b1, b2, b3;
    b0 = (1.0f - t);
    b1 = b0 * b0;
    b2 = 2 * t * b0;
    b3 = t * t;
    _return.x = b1 * p1.x + b2 * p2.x + b3 * p3.x;
    _return.y = b1 * p1.y + b2 * p2.y + b3 * p3.y;
    return _return;
  }

  private void CubicCurve(SVGPoint p1, SVGPoint p2, SVGPoint p3, SVGPoint p4, int numberOfLimit, bool cubic) {
    
    MoveTo(p1);
    //MoveTo the first Point;
    //How many times the curve change form innegative -> negative or vice versa
    int _limit = numberOfLimit;
    float t1, t2, _flatness;
    t1 = 0.0f;
    //t1 is the start point of [0..1].
    t2 = 1.0f;
    //t2 is the end point of [0..1]
    _flatness = 1.0f;
    
    SVGPointExt _pStart, _pEnd, _pMid;
    _pStart = new SVGPointExt(cubic ? EvaluateForCubic(t1, p1, p2, p3, p4) : EvaluateForQuadratic(t1, p1, p2, p3, p4), t1);
    
    _pEnd = new SVGPointExt(cubic ? EvaluateForCubic(t2, p1, p2, p3, p4) : EvaluateForQuadratic(t2, p1, p2, p3, p4), t2);
    
    // The point on Line Segment[_pStart, _pEnd] correlate with _t
    
    LiteStack<SVGPointExt> _stack = new LiteStack<SVGPointExt>();
    _stack.Push(_pEnd);
    //Push End Point into Stack
    //Array of Change Point
    SVGPointExt[] _limitList = new SVGPointExt[_limit + 1];
    
    int _count = 0;
    while(true) {
      _count++;
      float _tm = (t1 + t2) / 2;
      //tm is a middle of t1 .. t2. [t1 .. tm .. t2]
      //The point on the Curve correlate with tm
      _pMid = new SVGPointExt(cubic ? EvaluateForCubic(_tm, p1, p2, p3, p4) : EvaluateForQuadratic(_tm, p1, p2, p3, p4), _tm);
      
      //Calculate Distance from Middle Point to the Flatnet
      float dist = Distance(_pStart.point, ((SVGPointExt)_stack.Peek()).point, _pMid.point);
      
      //flag = true, Curve Segment must be drawn, else continue calculate other middle point.
      bool flag = false;
      if(dist < _flatness) {
        int i = 0;
        float mm = 0.0f;

        for(i = 0; i < _limit; i++) {
          mm = (t1 + _tm) / 2;
          
          SVGPointExt _q = new SVGPointExt(cubic ? EvaluateForCubic(mm, p1, p2, p3, p4) : EvaluateForQuadratic(mm, p1, p2, p3, p4), mm);
          _limitList[i] = _q;
          dist = Distance(_pStart.point, _pMid.point, _q.point);
          if(dist >= _flatness) {
            break;
          } else {
            _tm = mm;
          }
        }

        if(i == _limit) {
          flag = true;
        } else {
          //Continue calculate the first point has Distance > Flatness
          _stack.Push(_pMid);

          for(int j = 0; j <= i; j++)
            _stack.Push(_limitList[j]);
          t2 = mm;
        }
      }

      if(flag) {
        LineTo(_pStart.point);
        LineTo(_pMid.point);
        _pStart = _stack.Pop();

        if(_stack.Count == 0)
          break;

        _pMid = _stack.Peek();
        t1 = t2;
        t2 = _pMid.t;
      } else if(t2 > _tm) {
        //If Distance > Flatness and t1 < tm < t2 then new t2 is tm.
        _stack.Push(_pMid);
        t2 = _tm;
      }
    }
    LineTo(_pStart.point);
  }

  public void CubicCurve(SVGPoint p1, SVGPoint p2, SVGPoint p3, SVGPoint p4) {
    int _temp = NumberOfLimitForCubic(p1, p2, p3, p4);
    CubicCurve(p1, p2, p3, p4, _temp, true);
  }

  public void CubicCurveTo(SVGPoint p1, SVGPoint p2, SVGPoint p) {
    SVGPoint _tempPoint = new SVGPoint(this._currentPoint.x, this._currentPoint.y);
    CubicCurve(_tempPoint, p1, p2, p);
    this._currentPoint.SetValue(p);
  }

  public void QuadraticCurve(SVGPoint p1, SVGPoint p2, SVGPoint p3) {
    SVGPoint p4 = new SVGPoint(p2.x, p2.y);
    CubicCurve(p1, p2, p3, p4, 0, false);
    this._currentPoint.SetValue(p3);
  }

  public void QuadraticCurveTo(SVGPoint p1, SVGPoint p) {
    SVGPoint _tempPoint = new SVGPoint(this._currentPoint.x, this._currentPoint.y);
    QuadraticCurve(_tempPoint, p1, p);
    this._currentPoint.SetValue(p);
  }
}
