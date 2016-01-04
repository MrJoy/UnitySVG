using UnityEngine;

public class SVGGEllipse : ISVGPathSegment {
  private readonly Vector2 p;
  private readonly float r1;
  private readonly float r2;
  private readonly float angle;

  public SVGGEllipse(Vector2 p, float r1, float r2, float angle) {
    this.p = p;
    this.r1 = r1;
    this.r2 = r2;
    this.angle = angle;
  }

  public void ExpandBounds(SVGGraphicsPath path) {
    path.ExpandBounds(p, r1, r2);
  }

  public bool Render(SVGGraphicsPath path, ISVGPathDraw pathDraw) {
    pathDraw.EllipseTo(path.matrixTransform.Transform(p), r1, r2, path.transformAngle + angle);
    return true;
  }
}
