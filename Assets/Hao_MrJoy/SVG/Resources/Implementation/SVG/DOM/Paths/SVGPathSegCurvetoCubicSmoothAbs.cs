public class SVGPathSegCurvetoCubicSmoothAbs : SVGPathSegCurvetoCubic, ISVGDrawableSeg  {
  private float _x  = 0f;
  private float _y  = 0f;
  private float _x2  = 0f;
  private float _y2  = 0f;
  //================================================================================
  public float x {
    get{ return this._x;}
  }
  //-----
  public float y {
    get{ return this._y;}
  }
  //-----
  public float x2 {
    get{ return this._x2;}
  }
  //-----
  public float y2 {
    get{ return this._y2;}
  }
  //================================================================================
  public SVGPathSegCurvetoCubicSmoothAbs(float x2, float y2, float x, float y) : base() {
    this._x = x;
    this._y = y;
    this._x2 = x2;
    this._y2 = y2;
  }
  //================================================================================
  public override SVGPoint currentPoint{
    get{
      SVGPoint _return = new SVGPoint(this._x, this._y);
      return _return;
    }
  }
  //-----
  public override SVGPoint controlPoint1{
    get{
      SVGPoint _return = new SVGPoint(0f,0f);
      SVGPathSeg _prevSeg = previousSeg;
      if(_prevSeg != null) {
        SVGPoint t_currP = previousPoint;
        SVGPoint t_prevCP2 = ((SVGPathSegCurvetoCubic)_prevSeg).controlPoint2;
        SVGPoint t_P = t_currP - t_prevCP2;
        _return = t_currP + t_P;
      }
      return _return;
    }
  }
  //-----
  public override SVGPoint controlPoint2{
    get{
      return new SVGPoint(this._x2, this._y2);
    }
  }
  //--------------------------------------------------------------------------------
  //Method: Render
  //--------------------------------------------------------------------------------
  public void Render(SVGGraphicsPath _graphicsPath) {
    SVGPoint p, p1, p2;
    p1 = controlPoint1;
    p2 = controlPoint2;
    p = currentPoint;
    _graphicsPath.AddCubicCurveTo(p1, p2, p);
  }
}