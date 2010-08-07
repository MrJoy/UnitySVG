public class uSVGPathSegCurvetoQuadraticRel : uSVGPathSegCurvetoQuadratic, uISVGDrawableSeg  {
  private float _x  = 0f;
  private float _y  = 0f;
  private float _x1  = 0f;
  private float _y1  = 0f;
  /***********************************************************************************/
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
  /***********************************************************************************/
  public uSVGPathSegCurvetoQuadraticRel(float x1, float y1, float x, float y)
                  : base(uSVGPathSegTypes.PATHSEG_CURVETO_QUADRATIC_REL) {
    this._x = x;
    this._y = y;
    this._x1 = x1;
    this._y1 = y1;
  }
  /***********************************************************************************/
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
        _return.x = _prevSeg.currentPoint.x + this._x1;
        _return.y = _prevSeg.currentPoint.y + this._y1;
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