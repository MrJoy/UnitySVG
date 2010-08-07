public class uSVGPathSegLinetoVerticalRel : uSVGPathSeg, uISVGDrawableSeg  {
  private float _y = 0f;
  //================================================================================
  public float y {
    get{ return this._y;}
  }
  //================================================================================
  public uSVGPathSegLinetoVerticalRel(float y) :
                    base(uSVGPathSegTypes.PATHSEG_LINETO_HORIZONTAL_REL) {
    this._y = y;
  }
  //================================================================================
  public override uSVGPoint currentPoint{
    get{
      uSVGPoint _return = new uSVGPoint(0f,0f);
      uSVGPathSeg _prevSeg = previousSeg;
      if(_prevSeg != null) {
        _return.x = _prevSeg.currentPoint.x;
        _return.y = _prevSeg.currentPoint.y + this._y;
      }
      return _return;
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
