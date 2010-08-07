public class SVGGCubicAbs  {
  private SVGPoint _p1;
  private SVGPoint _p2;
  private SVGPoint _p;

  //================================================================================
  public SVGPoint p1 {
    get{return this._p1;}
  }
  //-----
  public SVGPoint p2 {
    get{return this._p2;}
  }
  //-----
  public SVGPoint p {
    get{return this._p;}
  }
  //================================================================================
  public SVGGCubicAbs(SVGPoint p1, SVGPoint p2, SVGPoint p) {
    this._p1 = new SVGPoint(p1.x, p1.y);
    this._p2 = new SVGPoint(p2.x, p2.y);
    this._p = new SVGPoint(p.x, p.y);
  }
}
