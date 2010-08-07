public class SVGGCircle{
  private uSVGPoint _p;
  private float _r;
  //================================================================================
  public uSVGPoint point {
    get{ return this._p;}
  }
  //------
  public float r {
    get{return this._r;}
  }
  //================================================================================
  public SVGGCircle(uSVGPoint p, float r) {
    this._p = new uSVGPoint(p.x, p.y);
    this._r = r;
  }
}
