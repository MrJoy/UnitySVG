using UnityEngine;

public struct SVGGMoveTo : ISVGPathSegment {
  private Vector2 point;

  public SVGGMoveTo(Vector2 p) {
    point = p;
  }

  public void ExpandBounds(SVGGraphicsPath path) {
    path.ExpandBounds(point);
  }

  public bool Render(SVGGraphicsPath path, ISVGPathDraw pathDraw) {
    pathDraw.MoveTo(path.matrixTransform.Transform(point));
    return false;
  }
}
