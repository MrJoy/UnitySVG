using UnityEngine;

public struct SVGGCubicAbs : ISVGPathSegment {
  private Vector2 p1, p2, point;

  public SVGGCubicAbs(Vector2 q1, Vector2 q2, Vector2 p) {
    this.p1 = q1;
    this.p2 = q2;
    this.point = p;
  }

  public void ExpandBounds(SVGGraphicsPath path) {
    path.ExpandBounds(p1);
    path.ExpandBounds(p2);
    path.ExpandBounds(point);
  }

  public bool Render(SVGGraphicsPath path, ISVGPathDraw pathDraw) {
    pathDraw.CubicCurveTo(path.matrixTransform.Transform(p1), path.matrixTransform.Transform(p2), path.matrixTransform.Transform(point));
    return false;
  }
}
