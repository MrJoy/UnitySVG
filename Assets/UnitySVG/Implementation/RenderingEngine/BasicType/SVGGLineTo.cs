using UnityEngine;

public class SVGGLineTo : ISVGPathSegment {
  private readonly Vector2 point;

  public SVGGLineTo(Vector2 p) {
    point = p;
  }

  public void ExpandBounds(SVGGraphicsPath path) {
    path.ExpandBounds(point);
  }

  public bool Render(SVGGraphicsPath path, ISVGPathDraw pathDraw) {
    pathDraw.LineTo(path.matrixTransform.Transform(point));
    return false;
  }
}
