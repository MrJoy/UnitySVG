public class uSVGPathSegCurvetoQuadraticSmoothRel : uSVGPathSegCurvetoQuadratic, uISVGDrawableSeg  {
  private float _x = 0f;
  private float _y = 0f;
  //================================================================================
  public float x {
    get{ return this._x;}
  }
  //-----
  public float y {
    get{ return this._y;}
  }
  //================================================================================
  public uSVGPathSegCurvetoQuadraticSmoothRel(float x, float y) : base(uSVGPathSegTypes.PATHSEG_CURVETO_QUADRATIC_SMOOTH_REL) {
    this._x = x;
    this._y = y;
  }
  //================================================================================
  public override uSVGPoint currentPoint{
    get{
      uSVGPoint _return = new uSVGPoint(0f,0f);
      uSVGPathSeg _prevSeg = previousSeg;
      if(_prevSeg != null) {
        _return.x = _prevSeg.currentPoint.x + this._x;
        _return.y = _prevSeg.currentPoint.y + this._y;
      }
      return _return;
    }
  }
  //-----
  public override uSVGPoint controlPoint1{
    get{
      uSVGPoint _return = new uSVGPoint(0f,0f);
      uSVGPathSeg _prevSeg = previousSeg;
      if(_prevSeg != null) {
        uSVGPoint t_currP = previousPoint;
        uSVGPoint t_prevCP2 = ((uSVGPathSegCurvetoQuadratic)_prevSeg).controlPoint1;
        uSVGPoint t_P = t_currP - t_prevCP2;
        _return = t_currP + t_P;
      }
      return _return;
    }
  }
  //--------------------------------------------------------------------------------
  //Method: Render
  //--------------------------------------------------------------------------------
  public void Render(uSVGGraphicsPath _graphicsPath) {
    uSVGPoint p, p1;
    p = currentPoint;
    p1 = controlPoint1;
    _graphicsPath.AddQuadraticCurveTo(p1, p);
  }
}