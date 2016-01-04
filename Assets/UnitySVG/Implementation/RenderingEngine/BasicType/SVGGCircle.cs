using UnityEngine;

public class SVGGCircle : ISVGPathSegment {
  private readonly Vector2 point;
  private readonly float r;

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
