public class SVGGQuadraticAbs  {
  private SVGPoint _p1;
  private SVGPoint _p;

  //================================================================================
  public SVGPoint p1 {
    get{return this._p1;}
  }
  //-----
  public SVGPoint p {
    get{return this._p;}
  }
  //================================================================================
  public SVGGQuadraticAbs(SVGPoint p1, SVGPoint p) {
    this._p1 = new SVGPoint(p1.x, p1.y);
    this._p = new SVGPoint(p.x, p.y);
  }
}
