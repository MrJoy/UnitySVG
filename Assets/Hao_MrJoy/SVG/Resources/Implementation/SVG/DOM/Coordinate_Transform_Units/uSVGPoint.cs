// TODO: Convert to use UnityEngine.Vector2
public struct uSVGPoint {
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
  public static uSVGPoint operator +(uSVGPoint a, uSVGPoint b) {
    return new uSVGPoint(a.x + b.x, a.y + b.y);
  }
  public static uSVGPoint operator -(uSVGPoint a, uSVGPoint b) {
    return new uSVGPoint(a.x - b.x, a.y - b.y);
  }
  public static uSVGPoint operator *(uSVGPoint a, uSVGPoint b) {
    return new uSVGPoint(a.x * b.x, a.y * b.y);
  }
  public static uSVGPoint operator /(uSVGPoint a, uSVGPoint b) {
    return new uSVGPoint(a.x / b.x, a.y / b.y);
  }
  public static uSVGPoint operator *(float t, uSVGPoint a) {
    return new uSVGPoint(t * a.x, t * a.y);
  }
  public static uSVGPoint operator +(float t, uSVGPoint a) {
    return new uSVGPoint(t + a.x, t + a.y);
  }

  /***********************************************************************************/
  public uSVGPoint(float x, float y) {
    this._x = x;
    this._y = y;
  }
  public void SetValue(float x, float y) {
    this._x = x;
    this._y = y;
  }
  public void SetValue(uSVGPoint point) {
    this._x = point.x;
    this._y = point.y;
  }

  public uSVGPoint MatrixTransform(uSVGMatrix matrix) {
    float a,b,c,d,e,f;
    a = matrix.a;
    b = matrix.b;
    c = matrix.c;
    d = matrix.d;
    e = matrix.e;
    f = matrix.f;
    return new uSVGPoint(a*x + c*y + e, b*x + d*y +f);
  }
}