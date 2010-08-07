public class SVGGQuadraticAbs {
  private SVGPoint _p1, _p;

  public SVGPoint p1 {
    get { return this._p1; }
  }

  public SVGPoint p {
    get { return this._p; }
  }

  public SVGGQuadraticAbs(SVGPoint q1, SVGPoint q) {
    this._p1 = q1;
    this._p = q;
  }
}
