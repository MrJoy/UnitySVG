using UnityEngine;

public struct SVGGQuadraticAbs : ISVGPathSegment {
  private Vector2 p1, p;

  public SVGGQuadraticAbs(Vector2 q1, Vector2 q) {
    this.p1 = q1;
    this.p = q;
  }

  public void ExpandBounds(SVGGraphicsPath path) {
    path.ExpandBounds(p1);
    path.ExpandBounds(p);
  }

  public bool Render(SVGGraphicsPath path, ISVGPathDraw pathDraw) {
    pathDraw.QuadraticCurveTo(path.matrixTransform.Transform(p1), path.matrixTransform.Transform(p));
    return false;
  }
}
