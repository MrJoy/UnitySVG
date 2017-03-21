using UnityEngine;
using UnityEngine.Profiling;

public class SVGGArcAbs : ISVGPathSegment {
  private readonly Vector2 point;
  private readonly float r1;
  private readonly float r2;
  private readonly float angle;
  private readonly bool largeArcFlag;
  private readonly bool sweepFlag;

  public SVGGArcAbs(float r1, float r2, float angle,
                    bool largeArcFlag, bool sweepFlag, Vector2 p) {
    this.r1 = r1;
    this.r2 = r2;
    this.angle = angle;
    this.largeArcFlag = largeArcFlag;
    this.sweepFlag = sweepFlag;
    point = p;
  }

  public void ExpandBounds(SVGGraphicsPath path) {
    int r = (int)r1 + (int)r2;
    path.ExpandBounds(point, r, r);
  }

  public bool Render(SVGGraphicsPath path, ISVGPathDraw pathDraw) {
    Profiler.BeginSample("SVGGArcAbs.Render");
    pathDraw.ArcTo(r1, r2, path.transformAngle + angle, largeArcFlag, sweepFlag, path.matrixTransform.Transform(point));
    Profiler.EndSample();
    return false;
  }
}
