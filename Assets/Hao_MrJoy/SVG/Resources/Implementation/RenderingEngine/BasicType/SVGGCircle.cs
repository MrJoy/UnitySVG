public class SVGGCircle{
  private SVGPoint _p;
  private float _r;
  //================================================================================
  public SVGPoint point {
    get{ return this._p;}
  }
  //------
  public float r {
    get{return this._r;}
  }
  //================================================================================
  public SVGGCircle(SVGPoint p, float r) {
    this._p = new SVGPoint(p.x, p.y);
    this._r = r;
  }
}
