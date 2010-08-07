public class SVGPathSegMovetoRel : SVGPathSeg, ISVGDrawableSeg {
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
  public SVGPathSegMovetoRel(float x, float y) : base() {
    this._x = x;
    this._y = y;
  }
  //================================================================================
  public override SVGPoint currentPoint{
    get{
      SVGPoint _return = new SVGPoint(0f,0f);
      SVGPathSeg _prevSeg = previousSeg;
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
    SVGPoint p;
    p = currentPoint;
    _graphicsPath.AddMoveTo(p);
  }
}
