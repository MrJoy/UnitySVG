public class uSVGPathSegLinetoHorizontalRel : uSVGPathSeg, uISVGDrawableSeg  {
  private float _x = 0f;
  //================================================================================
  public float x {
    get{ return this._x;}
  }
  //================================================================================
  public uSVGPathSegLinetoHorizontalRel(float x) :
                    base(uSVGPathSegTypes.PATHSEG_LINETO_HORIZONTAL_REL) {
    this._x = x;
  }
  //================================================================================
  public override uSVGPoint currentPoint{
    get{
      uSVGPoint _return = new uSVGPoint(0f,0f);
      uSVGPathSeg _prevSeg = previousSeg;
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
    uSVGPoint p;
    p = currentPoint;
    _graphicsPath.AddLineTo(p);
  }
}
