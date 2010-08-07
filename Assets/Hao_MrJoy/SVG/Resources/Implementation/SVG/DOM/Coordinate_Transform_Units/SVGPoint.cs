// TODO: Convert to use UnityEngine.Vector2
public struct SVGPoint {
  private float _x;
  private float _y;

  /***********************************************************************************/
  public float x {
    get{ return this._x;}
    set{ this._x = value;}
  }
  public float y {
    get{ return this._y;}
    set{ this._y = value;}
  }
  public static SVGPoint operator +(SVGPoint a, SVGPoint b) {
    return new SVGPoint(a.x + b.x, a.y + b.y);
  }
  public static SVGPoint operator -(SVGPoint a, SVGPoint b) {
    return new SVGPoint(a.x - b.x, a.y - b.y);
  }
  public static SVGPoint operator *(SVGPoint a, SVGPoint b) {
    return new SVGPoint(a.x * b.x, a.y * b.y);
  }
  public static SVGPoint operator /(SVGPoint a, SVGPoint b) {
    return new SVGPoint(a.x / b.x, a.y / b.y);
  }
  public static SVGPoint operator *(float t, SVGPoint a) {
    return new SVGPoint(t * a.x, t * a.y);
  }
  public static SVGPoint operator +(float t, SVGPoint a) {
    return new SVGPoint(t + a.x, t + a.y);
  }

  /***********************************************************************************/
  public SVGPoint(float x, float y) {
    this._x = x;
    this._y = y;
  }
  public void SetValue(float x, float y) {
    this._x = x;
    this._y = y;
  }
  public void SetValue(SVGPoint point) {
    this._x = point.x;
    this._y = point.y;
  }

  public SVGPoint MatrixTransform(SVGMatrix matrix) {
    float a,b,c,d,e,f;
    a = matrix.a;
    b = matrix.b;
    c = matrix.c;
    d = matrix.d;
    e = matrix.e;
    f = matrix.f;
    return new SVGPoint(a*x + c*y + e, b*x + d*y +f);
  }
}