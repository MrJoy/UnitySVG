public class SVGPathSegLinetoHorizontalRel : SVGPathSeg, ISVGDrawableSeg  {
  private float _x = 0f;
  //================================================================================
  public float x {
    get{ return this._x;}
  }
  //================================================================================
  public SVGPathSegLinetoHorizontalRel(float x) : base() {
    this._x = x;
  }
  //================================================================================
  public override SVGPoint currentPoint{
    get{
      SVGPoint _return = new SVGPoint(0f,0f);
      SVGPathSeg _prevSeg = previousSeg;
      if(_prevSeg != null) {
        _return.x = _prevSeg.currentPoint.x + this._x;
        _return.y = _prevSeg.currentPoint.y;
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
    _graphicsPath.AddLineTo(p);
  }
}
