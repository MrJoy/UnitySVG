public class uSVGPathSegMovetoAbs : uSVGPathSeg, uISVGDrawableSeg {
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
  public uSVGPathSegMovetoAbs(float x, float y) : base(uSVGPathSegTypes.PATHSEG_MOVETO_ABS) {
    this._x = x;
    this._y = y;
  }
  //================================================================================
  public override uSVGPoint currentPoint{
    get{
      uSVGPoint _return = new uSVGPoint(this._x, this._y);
      return _return;
    }
  }
  //--------------------------------------------------------------------------------
  //Method: Render
  //--------------------------------------------------------------------------------
  public void Render(uSVGGraphicsPath _graphicsPath) {
    uSVGPoint p;
    p = new uSVGPoint(this._x, this._y);
    _graphicsPath.AddMoveTo(p);
  }
}