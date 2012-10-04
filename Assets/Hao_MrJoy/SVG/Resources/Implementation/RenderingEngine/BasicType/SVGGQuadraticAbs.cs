using UnityEngine;

public class SVGGQuadraticAbs {
  private Vector2 _p1, _p;

  public Vector2 p1 {
    get { return this._p1; }
  }

  public Vector2 p {
    get { return this._p; }
  }

  public SVGGQuadraticAbs(Vector2 q1, Vector2 q) {
    this._p1 = q1;
    this._p = q;
  }
}
