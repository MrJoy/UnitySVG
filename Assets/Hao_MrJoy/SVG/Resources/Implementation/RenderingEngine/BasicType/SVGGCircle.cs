public class SVGGCircle {
  private SVGPoint _p;
  private float _r;

  public SVGPoint point {
    get { return _p; }
  }

  public float r {
    get { return _r; }
  }

  public SVGGCircle(SVGPoint p, float r) {
    _p = p;
    _r = r;
  }
}
