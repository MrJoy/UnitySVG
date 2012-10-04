using UnityEngine;

public struct SVGGArcAbs : ISVGPathSegment{
  private Vector2 point;
  private float r1, r2, angle;
  private bool largeArcFlag, sweepFlag;

  //================================================================================
  public SVGGArcAbs(float r1, float r2, float angle,
                    bool largeArcFlag, bool sweepFlag, Vector2 p) {
    this.r1 = r1;
    this.r2 = r2;
    this.angle = angle;
    this.largeArcFlag = largeArcFlag;
    this.sweepFlag = sweepFlag;
    this.point = p;
  }

  public void ExpandBounds(SVGGraphicsPath path) {
    int r = (int)r1 + (int)r2;
    path.ExpandBounds(point, r, r);
  }

  public bool Render(SVGGraphicsPath path, ISVGPathDraw pathDraw) {
    pathDraw.ArcTo(r1, r2, path.transformAngle + angle, largeArcFlag, sweepFlag, path.matrixTransform.Transform(point));
    return false;
  }
}
