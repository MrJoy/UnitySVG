using UnityEngine;
using UnityEngine.Profiling;

public class SVGGMoveTo : ISVGPathSegment {
  private readonly Vector2 point;

  public SVGGMoveTo(Vector2 p) {
    point = p;
  }

  public void ExpandBounds(SVGGraphicsPath path) {
    path.ExpandBounds(point);
  }

  public bool Render(SVGGraphicsPath path, ISVGPathDraw pathDraw) {
    Profiler.BeginSample("SVGGMoveTo.Render");
    pathDraw.MoveTo(path.matrixTransform.Transform(point));
    Profiler.EndSample();
    return false;
  }
}
