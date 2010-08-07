public class SVGPathSegLinetoVerticalAbs : SVGPathSeg, ISVGDrawableSeg  {
  private float _y = 0f;
  //================================================================================
  public float y {
    get{ return this._y;}
  }
  //================================================================================
  public SVGPathSegLinetoVerticalAbs(float y) :
                  base(SVGPathSegTypes.LineTo_Vertical_Abs) {
    this._y = y;
  }
  //================================================================================
  public override SVGPoint currentPoint{
    get{
      SVGPoint _return = new SVGPoint(0f,0f);
      SVGPathSeg _prevSeg = previousSeg;
      if(_prevSeg != null) {
        _return.x = _prevSeg.currentPoint.x;
        _return.y = this._y;
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
