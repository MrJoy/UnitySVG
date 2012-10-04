using UnityEngine;

public struct SVGGMoveTo {
  private Vector2 _p;

  public Vector2 point {
    get { return _p; }
  }

  public SVGGMoveTo(Vector2 p) {
    _p = p;
  }
}
