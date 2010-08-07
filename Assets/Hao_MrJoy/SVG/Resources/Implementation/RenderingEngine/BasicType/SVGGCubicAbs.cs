public class SVGGCubicAbs {
  private SVGPoint _p1, _p2, _p;

  public SVGPoint p1 {
    get { return _p1; }
  }

  public SVGPoint p2 {
    get { return _p2; }
  }

  public SVGPoint p {
    get { return _p; }
  }

  public SVGGCubicAbs(SVGPoint q1, SVGPoint q2, SVGPoint q) {
    _p1 = q1;
    _p2 = q2;
    _p = q;
  }
}
