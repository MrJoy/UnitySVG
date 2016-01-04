using UnityEngine;

public interface ISVGPathDraw {
  void MoveTo(Vector2 p);

  void CircleTo(Vector2 p, float r);

  void EllipseTo(Vector2 p, float r1, float r2, float angle);

  void ArcTo(float r1, float r2, float angle, bool largeArcFlag, bool sweepFlag, Vector2 p);

  void CubicCurveTo(Vector2 p1, Vector2 p2, Vector2 p);

  void QuadraticCurveTo(Vector2 p1, Vector2 p);

  void LineTo(Vector2 p);

  void Rect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4);

  void RoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 p5, Vector2 p6, Vector2 p7, Vector2 p8,
                   float r1, float r2, float angle);

  void Circle(Vector2 p, float r);

  void Ellipse(Vector2 p, float rx, float ry, float angle);

  void Polygon(Vector2[] points);
}
