using UnityEngine;
using UnityEngine.Profiling;
using System;
using System.Collections.Generic;

public delegate void SetPixelDelegate(int x, int y);

public class SVGBasicDraw {
  private struct Vector2Ext {
    private readonly float _delta;
    private readonly Vector2 _point;

    public float t { get { return _delta; } }

    public Vector2 point { get { return _point; } }

    public Vector2Ext(Vector2 point, float t) {
      _point = point;
      _delta = t;
    }
  }

  private Vector2 _currentPoint;
  public SetPixelDelegate SetPixel;

  public Vector2 currentPoint { get { return _currentPoint; } }

  public SetPixelDelegate SetPixelMethod { set { SetPixel = value; } }

  public SVGBasicDraw() {
    _currentPoint = new Vector2(0f, 0f);
  }

  private static void Swap<T>(ref T x1, ref T x2) {
    T temp = x1;
    x1 = x2;
    x2 = temp;
  }

  public void MoveTo(float x, float y) {
    _currentPoint.x = x;
    _currentPoint.y = y;
  }

  public void MoveTo(Vector2 p) {
    _currentPoint = p;
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

    int deltax = x1 - x0,
        deltay = Math.Abs(y1 - y0),
        error = -(deltax + 1) / 2,
        y = y0,
        ystep = (y0 < y1) ? 1 : -1;

    for(int x = x0; x <= x1; x++) {
      if(steep)
        SetPixel(y, x);
      else
        SetPixel(x, y);
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

  public void Line(Vector2 p1, Vector2 p2) {
    Line(p1.x, p1.y, p2.x, p2.y);
  }

  public void LineTo(float x, float y) {
    Vector2 temp = new Vector2(x, y);
    Line(_currentPoint, temp);
    _currentPoint = temp;
  }

  public void LineTo(Vector2 p) {
    Line(_currentPoint, p);
    _currentPoint = p;
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

  public void Rect(Vector2 p1, Vector2 p2) {
    Rect(p1.x, p1.y, p2.x, p2.y);
  }

  public void Rect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
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
    float _angle = (2 * Mathf.PI) / _delta;

    float tx, ty, temp;
    tx = x0;
    ty = radius + y0;

    Vector2 fPoint = new Vector2(tx, ty);
    MoveTo(fPoint);
    for(int i = 1; i <= _delta; i++) {
      temp = i * _angle;
      tx = radius * (float)Math.Sin(temp) + x0;
      ty = radius * (float)Math.Cos(temp) + y0;
      Vector2 tPoint = new Vector2(tx, ty);
      LineTo(tPoint);
    }
    LineTo(fPoint);
  }

  public void Circle(float x0, float y0, float r) {
    Circle((int)x0, (int)y0, r);
  }

  public void Circle(Vector2 p, float r) {
    Circle((int)p.x, (int)p.y, r);
  }

  private void Ellipse(int cx, int cy, int rx, int ry, float angle) {
    float chuvi = 2f * Mathf.PI * (float)Math.Sqrt(rx * rx + ry * ry);
    int steps = (int)(chuvi / 3);
    if(steps > 50)
      steps = 50;
    float beta = angle * Mathf.Deg2Rad;
    float sinbeta = Mathf.Sin(beta);
    float cosbeta = Mathf.Cos(beta);

    steps = 360 / steps;

    int i = 0;
    float alpha = i * Mathf.Deg2Rad;
    float sinalpha = Mathf.Sin(alpha);
    float cosalpha = Mathf.Cos(alpha);

    float _x = cx + (rx * cosalpha * cosbeta - ry * sinalpha * sinbeta);
    float _y = cy + (rx * cosalpha * sinbeta + ry * sinalpha * cosbeta);

    float _fPointx = _x;
    float _fPointy = _y;
    MoveTo(_x, _y);

    for(i = 1; i < 360; i += steps) {
      alpha = i * Mathf.Deg2Rad;
      sinalpha = Mathf.Sin(alpha);
      cosalpha = Mathf.Cos(alpha);

      _x = cx + (rx * cosalpha * cosbeta - ry * sinalpha * sinbeta);
      _y = cy + (rx * cosalpha * sinbeta + ry * sinalpha * cosbeta);
      LineTo(_x, _y);
    }
    LineTo(_fPointx, _fPointy);
  }

  public void Ellipse(float x0, float y0, float rx, float ry, float angle) {
    Ellipse((int)x0, (int)y0, (int)rx, (int)ry, angle);
  }

  public void Ellipse(Vector2 p, float rx, float ry, float angle) {
    Ellipse((int)p.x, (int)p.y, (int)rx, (int)ry, angle);
  }

  public void Arc(Vector2 p1, float rx, float ry, float angle, bool largeArcFlag, bool sweepFlag, Vector2 p2) {
    Profiler.BeginSample("SVGBasicDraw.Arc(...)");
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
      rx = (float)Math.Sqrt((float)radiiCheck) * rx;
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

    if(!sweepFlag && _delta > 0)
      _delta -= 360f;
    else if(sweepFlag && _delta < 0)
      _delta += 360f;

    _delta %= 360f;

    int number = 100;
    float deltaT = _delta / number;

    Vector2 _point = new Vector2(0, 0);
    float t_angle;
    for(int i = 0; i <= number; i++) {
      t_angle = (deltaT * i + _angle) * Mathf.PI / 180.0f;
      _point.x = _CosRadian * rx * (float)Math.Cos(t_angle) - _SinRadian * ry * (float)Math.Sin(t_angle) + cx;
      _point.y = _SinRadian * rx * (float)Math.Cos(t_angle) + _CosRadian * ry * (float)Math.Sin(t_angle) + cy;
      LineTo(_point);
    }
    Profiler.EndSample();
  }

  public void ArcTo(float r1, float r2, float angle, bool largeArcFlag, bool sweepFlag, Vector2 p) {
    Vector2 _tempPoint = new Vector2(_currentPoint.x, _currentPoint.y);
    Arc(_tempPoint, r1, r2, angle, largeArcFlag, sweepFlag, p);
    _currentPoint = p;
  }

  private static float BelongPosition(Vector2 a, Vector2 b, Vector2 c) {
    float _up = ((a.y - c.y) * (b.x - a.x)) - ((a.x - c.x) * (b.y - a.y));
    float _under = ((b.x - a.x) * (b.x - a.x)) + ((b.y - a.y) * (b.y - a.y));
    float _r = _up / _under;
    return _r;
  }

  //Caculate Distance from c point to line segment [a,b]
  //return d point is the point on that line segment.
  private static int NumberOfLimitForCubic(Vector2 a, Vector2 b, Vector2 c, Vector2 d) {
    float _r1 = BelongPosition(a, d, b);
    float _r2 = BelongPosition(a, d, c);
    if((_r1 * _r2) > 0)
      return 0;
    return 1;
  }

  private static float Distance(Vector2 a, Vector2 b, Vector2 c) {
    float _up = ((a.y - c.y) * (b.x - a.x)) - ((a.x - c.x) * (b.y - a.y));
    float _under = ((b.x - a.x) * (b.x - a.x)) + ((b.y - a.y) * (b.y - a.y));
    return Math.Abs(_up / _under) * (float)Math.Sqrt(_under);
  }

  private static Vector2 EvaluateForCubic(float t, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
    Vector2 result = new Vector2(0, 0);
    float b0 = (1.0f - t);
    float b1 = b0 * b0 * b0;
    float b2 = 3 * t * b0 * b0;
    float b3 = 3 * t * t * b0;
    float b4 = t * t * t;
    result.x = b1 * p1.x + b2 * p2.x + b3 * p3.x + b4 * p4.x;
    result.y = b1 * p1.y + b2 * p2.y + b3 * p3.y + b4 * p4.y;
    return result;
  }

  private static Vector2 EvaluateForQuadratic(float t, Vector2 p1, Vector2 p2, Vector2 p3) {
    Vector2 result = Vector2.zero;
    float b0 = (1.0f - t);
    float b1 = b0 * b0;
    float b2 = 2 * t * b0;
    float b3 = t * t;
    result.x = b1 * p1.x + b2 * p2.x + b3 * p3.x;
    result.y = b1 * p1.y + b2 * p2.y + b3 * p3.y;
    return result;
  }

  private static readonly LiteStack<Vector2Ext> _stack = new LiteStack<Vector2Ext>();
  private static readonly List<Vector2Ext> _limitList = new List<Vector2Ext>();

  private void CubicCurve(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, int numberOfLimit, bool cubic) {
    Profiler.BeginSample("SVGBasicDraw.CubicCurve(...)");
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

    Vector2Ext _pStart, _pEnd, _pMid;
    _pStart = new Vector2Ext(cubic ? EvaluateForCubic(t1, p1, p2, p3, p4) : EvaluateForQuadratic(t1, p1, p2, p3), t1);

    _pEnd = new Vector2Ext(cubic ? EvaluateForCubic(t2, p1, p2, p3, p4) : EvaluateForQuadratic(t2, p1, p2, p3), t2);

    // The point on Line Segment[_pStart, _pEnd] correlate with _t

    _stack.Clear();
    _stack.Push(_pEnd);
    //Push End Point into Stack
    //Array of Change Point
    _limitList.Clear();
    if(_limitList.Capacity < _limit + 1)
      _limitList.Capacity = _limit + 1;

    int _count = 0;
    while(true) {
      _count++;
      float _tm = (t1 + t2) / 2;
      //tm is a middle of t1 .. t2. [t1 .. tm .. t2]
      //The point on the Curve correlate with tm
      _pMid = new Vector2Ext(cubic ? EvaluateForCubic(_tm, p1, p2, p3, p4) : EvaluateForQuadratic(_tm, p1, p2, p3), _tm);

      //Calculate Distance from Middle Point to the Flatnet
      float dist = Distance(_pStart.point, _stack.Peek().point, _pMid.point);

      //flag = true, Curve Segment must be drawn, else continue calculate other middle point.
      bool flag = false;
      if(dist < _flatness) {
        int i = 0;
        float mm = 0.0f;

        for(i = 0; i < _limit; i++) {
          mm = (t1 + _tm) / 2;

          Vector2Ext _q =
            new Vector2Ext(cubic ? EvaluateForCubic(mm, p1, p2, p3, p4) : EvaluateForQuadratic(mm, p1, p2, p3), mm);
          if(_limitList.Count - 1 < i)
            _limitList.Add(_q);
          else
            _limitList[i] = _q;
          dist = Distance(_pStart.point, _pMid.point, _q.point);
          if(dist >= _flatness)
            break;
          else
            _tm = mm;
        }

        if(i == _limit)
          flag = true;
        else {
          //Continue calculate the first point has Distance > Flatness
          _stack.Push(_pMid);

          for(int j = 0; j <= i; ++j)
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
    Profiler.EndSample();
  }

  public void CubicCurve(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
    int _temp = NumberOfLimitForCubic(p1, p2, p3, p4);
    CubicCurve(p1, p2, p3, p4, _temp, true);
  }

  public void CubicCurveTo(Vector2 p1, Vector2 p2, Vector2 p) {
    Vector2 _tempPoint = new Vector2(_currentPoint.x, _currentPoint.y);
    CubicCurve(_tempPoint, p1, p2, p);
    _currentPoint = p;
  }

  public void QuadraticCurve(Vector2 p1, Vector2 p2, Vector2 p3) {
    Vector2 p4 = new Vector2(p2.x, p2.y);
    CubicCurve(p1, p2, p3, p4, 0, false);
    _currentPoint = p3;
  }

  public void QuadraticCurveTo(Vector2 p1, Vector2 p) {
    Vector2 _tempPoint = new Vector2(_currentPoint.x, _currentPoint.y);
    QuadraticCurve(_tempPoint, p1, p);
    _currentPoint = p;
  }
}
