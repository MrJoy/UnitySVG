public class SVGPathSegMovetoAbs : SVGPathSeg, ISVGDrawableSeg {
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
  public SVGPathSegMovetoAbs(float x, float y) : base() {
    this._x = x;
    this._y = y;
  }
  //================================================================================
  public override SVGPoint currentPoint{
    get{
      SVGPoint _return = new SVGPoint(this._x, this._y);
      return _return;
    }
  }
  //--------------------------------------------------------------------------------
  //Method: Render
  //--------------------------------------------------------------------------------
  public void Render(SVGGraphicsPath _graphicsPath) {
    SVGPoint p;
    p = new SVGPoint(this._x, this._y);
    _graphicsPath.AddMoveTo(p);
  }
}