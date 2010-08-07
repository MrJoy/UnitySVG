public class uSVGPathSegMovetoRel : uSVGPathSeg, uISVGDrawableSeg {
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
  public uSVGPathSegMovetoRel(float x, float y) : base(uSVGPathSegTypes.PATHSEG_MOVETO_REL) {
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
  //--------------------------------------------------------------------------------
  //Method: Render
  //--------------------------------------------------------------------------------
  public void Render(SVGGraphicsPath _graphicsPath) {
    uSVGPoint p;
    p = currentPoint;
    _graphicsPath.AddMoveTo(p);
  }
}
