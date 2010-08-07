public interface ISVGPathDraw {
  void MoveTo(SVGPoint p);

  void CircleTo(SVGPoint p, float r);

  void EllipseTo(SVGPoint p, float r1, float r2, float angle);

  void ArcTo(float r1, float r2, float angle, bool largeArcFlag, bool sweepFlag, SVGPoint p);

  void CubicCurveTo(SVGPoint p1, SVGPoint p2, SVGPoint p);

  void QuadraticCurveTo(SVGPoint p1, SVGPoint p);

  void LineTo(SVGPoint p);

  void Rect(SVGPoint p1, SVGPoint p2, SVGPoint p3, SVGPoint p4);

  void RoundedRect(SVGPoint p1, SVGPoint p2, SVGPoint p3, SVGPoint p4, SVGPoint p5, SVGPoint p6, SVGPoint p7, SVGPoint p8, float r1, float r2, float angle);

  void Circle(SVGPoint p, float r);

  void Ellipse(SVGPoint p, float rx, float ry, float angle);

  void Polyline(SVGPoint[] points);

  void Polygon(SVGPoint[] points);
}
