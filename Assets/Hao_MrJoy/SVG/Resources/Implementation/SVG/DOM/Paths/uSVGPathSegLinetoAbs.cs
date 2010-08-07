public class uSVGPathSegLinetoAbs : uSVGPathSeg, uISVGDrawableSeg {
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
  public uSVGPathSegLinetoAbs(float x, float y) : base(uSVGPathSegTypes.PATHSEG_LINETO_ABS) {
    this._x = x;
    this._y = y;
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
  public void Render(uSVGGraphicsPath _graphicsPath) {
    uSVGPoint p;
    p = currentPoint;
    _graphicsPath.AddLineTo(p);
  }
}
