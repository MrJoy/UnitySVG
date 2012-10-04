using UnityEngine;

public class SVGGCircle {
  private Vector2 _p;
  private float _r;

  public Vector2 point {
    get { return _p; }
  }

  public float r {
    get { return _r; }
  }

  public SVGGCircle(Vector2 p, float r) {
    _p = p;
    _r = r;
  }
}
