using UnityEngine;

public struct SVGGLineTo {
  private Vector2 _p;

  public Vector2 point {
    get { return _p; }
  }

  public SVGGLineTo(Vector2 p) {
    _p = p;
  }
}
