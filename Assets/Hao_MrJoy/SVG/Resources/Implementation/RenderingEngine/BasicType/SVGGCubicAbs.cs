using UnityEngine;

public class SVGGCubicAbs {
  private Vector2 _p1, _p2, _p;

  public Vector2 p1 {
    get { return _p1; }
  }

  public Vector2 p2 {
    get { return _p2; }
  }

  public Vector2 p {
    get { return _p; }
  }

  public SVGGCubicAbs(Vector2 q1, Vector2 q2, Vector2 q) {
    _p1 = q1;
    _p2 = q2;
    _p = q;
  }
}
