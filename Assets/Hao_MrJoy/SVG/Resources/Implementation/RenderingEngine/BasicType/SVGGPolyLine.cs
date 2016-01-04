using UnityEngine;
using System.Collections.Generic;

public class SVGGPolyLine : ISVGPathSegment {
  private readonly List<Vector2> points;

  public SVGGPolyLine(List<Vector2> points) {
    this.points = points;
  }

  public void ExpandBounds(SVGGraphicsPath path) {
    path.ExpandBounds(points);
  }

  public bool Render(SVGGraphicsPath path, ISVGPathDraw pathDraw) {
    int length = points.Count;
    pathDraw.MoveTo(path.matrixTransform.Transform(points[0]));
    for(int i = 1; i < length; i++) {
      Vector2 p = path.matrixTransform.Transform(points[i]);
      pathDraw.LineTo(p);
    }

    return false;
  }
}
