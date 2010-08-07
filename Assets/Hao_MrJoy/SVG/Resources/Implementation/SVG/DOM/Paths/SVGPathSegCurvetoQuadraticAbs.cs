public class SVGPathSegCurvetoQuadraticAbs : SVGPathSegCurvetoQuadratic, ISVGDrawableSeg  {
  private float _x  = 0f;
  private float _y  = 0f;
  private float _x1  = 0f;
  private float _y1  = 0f;
  //================================================================================
  public float x {
    get{ return this._x;}
  }
  //-----
  public float y {
    get{ return this._y;}
  }
  //-----
  public float x1 {
    get{ return this._x1;}
  }
  //-----
  public float y1 {
    get{ return this._y1;}
  }
  //================================================================================
  public SVGPathSegCurvetoQuadraticAbs(float x1, float y1, float x, float y)
                    : base(SVGPathSegTypes.CurveTo_Quadratic_Abs) {
    this._x = x;
    this._y = y;
    this._x1 = x1;
    this._y1 = y1;
  }
  //================================================================================
  public override SVGPoint currentPoint{
    get{
      return new SVGPoint(this._x, this._y);
    }
  }
  //-----
  public override SVGPoint controlPoint1{
    get{
      return new SVGPoint(this._x1, this._y1);
    }
  }
  //--------------------------------------------------------------------------------
  //Method: Render
  //--------------------------------------------------------------------------------
  public void Render(SVGGraphicsPath _graphicsPath) {
    SVGPoint p, p1;
    p = currentPoint;
    p1 = controlPoint1;
    _graphicsPath.AddQuadraticCurveTo(p1, p);
  }
}