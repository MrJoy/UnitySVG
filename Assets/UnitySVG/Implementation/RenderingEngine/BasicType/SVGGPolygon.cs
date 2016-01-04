using UnityEngine;
using System.Collections.Generic;

public class SVGGPolygon : ISVGPathSegment {
  private readonly List<Vector2> points;

  public SVGGPolygon(List<Vector2> points) {
    this.points = points;
  }

  public void ExpandBounds(SVGGraphicsPath path) {
    path.ExpandBounds(points);
  }

  public bool Render(SVGGraphicsPath path, ISVGPathDraw pathDraw) {
    int length = points.Count;
    Vector2[] tPoints = new Vector2[length];

    for(int i = 0; i < length; i++)
      tPoints[i] = path.matrixTransform.Transform(points[i]);
    pathDraw.Polygon(tPoints);

    return true;
  }
}
