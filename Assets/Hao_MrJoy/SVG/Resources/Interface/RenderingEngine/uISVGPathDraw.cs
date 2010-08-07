public interface uISVGPathDraw {
  //================================================================================
  //-----
  //MoveTo
  //-----
  void MoveTo(SVGPoint p);

  //-----
  //CircleTo
  //-----
  void CircleTo(SVGPoint p, float r);

  //-----
  //EllipseTo
  //-----
  void EllipseTo(SVGPoint p, float r1, float r2, float angle);

  //-----
  //ArcTo
  //-----
  void ArcTo(float r1, float r2, float angle,
        bool largeArcFlag, bool sweepFlag, SVGPoint p);

  //-----
  //CubicCurveTo
  //-----
  void CubicCurveTo(SVGPoint p1, SVGPoint p2, SVGPoint p);

  //-----
  //QuadraticCurveTo
  //-----
  void QuadraticCurveTo(SVGPoint p1, SVGPoint p);

  //-----
  //LineTo
  //-----
  void LineTo(SVGPoint p);

  //-----
  //Rect
  //-----
  void Rect(SVGPoint p1, SVGPoint p2, SVGPoint p3, SVGPoint p4);

  //-----
  //RoundedRect
  //-----
  void RoundedRect(SVGPoint p1, SVGPoint p2, SVGPoint p3, SVGPoint p4,
        SVGPoint p5, SVGPoint p6, SVGPoint p7, SVGPoint p8,
        float r1, float r2, float angle);


  //-----
  //Circle
  //-----
  void Circle(SVGPoint p, float r);

  //-----
  //Ellipse
  //-----
  void Ellipse(SVGPoint p, float rx, float ry, float angle);

  //-----
  //Polyline
  //-----
  void Polyline(SVGPoint[] points);

  //-----
  //Polygon
  //-----
  void Polygon(SVGPoint[] points);
}