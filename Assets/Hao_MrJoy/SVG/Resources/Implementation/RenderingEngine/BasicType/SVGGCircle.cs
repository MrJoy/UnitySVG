using UnityEngine;

public struct SVGGCircle : ISVGPathSegment {
  private Vector2 point;
  private float r;

  public SVGGCircle(Vector2 p, float r) {
    this.point = p;
    this.r = r;
  }

  public void ExpandBounds(SVGGraphicsPath path) {
    path.ExpandBounds(point, r, r);
  }

  public bool Render(SVGGraphicsPath path, ISVGPathDraw pathDraw) {
    pathDraw.CircleTo(path.matrixTransform.Transform(point), r);

    return true;
  }
}
