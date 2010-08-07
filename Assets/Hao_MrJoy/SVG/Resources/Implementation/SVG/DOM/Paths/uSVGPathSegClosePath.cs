public class uSVGPathSegClosePath : uSVGPathSeg, uISVGDrawableSeg {
  private float _x = 0f;
  private float _y = 0f;
  //================================================================================
  public uSVGPathSegClosePath(float x, float y) : base(uSVGPathSegTypes.PATHSEG_CLOSEPATH) {
    if(x == -1f && y == -1f) {
      this._x = previousPoint.x;
      this._y = previousPoint.y;
    } else {
      this._x = x;
      this._y = y;
    }
  }
  //================================================================================
  public override uSVGPoint currentPoint{
    get{
      return new uSVGPoint(this._x, this._y);
    }
  }
  //--------------------------------------------------------------------------------
  //Method: Render
  //--------------------------------------------------------------------------------
  public void Render(SVGGraphicsPath _graphicsPath) {
    uSVGPoint p;
    p = currentPoint;
    _graphicsPath.AddLineTo(p);
  }
}