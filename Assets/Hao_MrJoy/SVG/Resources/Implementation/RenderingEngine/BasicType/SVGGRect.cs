using UnityEngine;

public class SVGGRect : ISVGPathSegment {
  public readonly float x;
  public readonly float y;
  private readonly float width;
  private readonly float height;
  private readonly float rx;
  private readonly float ry;

  public SVGGRect(float x, float y, float width, float height, float rx, float ry) {
    this.x = x;
    this.y = y;
    this.width = width;
    this.height = height;
    this.rx = rx;
    this.ry = ry;
  }

  public void ExpandBounds(SVGGraphicsPath path) {
    path.ExpandBounds(x, y);
    path.ExpandBounds(x + width, y);
    path.ExpandBounds(x + width, y + height);
    path.ExpandBounds(x, y + height);
  }

  public bool Render(SVGGraphicsPath path, ISVGPathDraw pathDraw) {
    Vector2 p1 = new Vector2(x, y),
    p2 = new Vector2(x + width, y),
    p3 = new Vector2(x + width, y + height),
    p4 = new Vector2(x, y + height);

    if(rx == 0.0f && ry == 0.0f) {
      p1 = path.matrixTransform.Transform(p1);
      p2 = path.matrixTransform.Transform(p2);
      p3 = path.matrixTransform.Transform(p3);
      p4 = path.matrixTransform.Transform(p4);

      pathDraw.Rect(p1, p2, p3, p4);
    } else {
      float t_rx = (rx == 0.0f) ? ry : rx;
      float t_ry = (ry == 0.0f) ? rx : ry;

      t_rx = (t_rx > (width * 0.5f - 2f)) ? (width * 0.5f - 2f) : t_rx;
      t_ry = (t_ry > (height * 0.5f - 2f)) ? (height * 0.5f - 2f) : t_ry;

      float angle = path.transformAngle;

      Vector2 t_p1 = path.matrixTransform.Transform(new Vector2(p1.x + t_rx, p1.y));
      Vector2 t_p2 = path.matrixTransform.Transform(new Vector2(p2.x - t_rx, p2.y));
      Vector2 t_p3 = path.matrixTransform.Transform(new Vector2(p2.x, p2.y + t_ry));
      Vector2 t_p4 = path.matrixTransform.Transform(new Vector2(p3.x, p3.y - t_ry));

      Vector2 t_p5 = path.matrixTransform.Transform(new Vector2(p3.x - t_rx, p3.y));
      Vector2 t_p6 = path.matrixTransform.Transform(new Vector2(p4.x + t_rx, p4.y));
      Vector2 t_p7 = path.matrixTransform.Transform(new Vector2(p4.x, p4.y - t_ry));
      Vector2 t_p8 = path.matrixTransform.Transform(new Vector2(p1.x, p1.y + t_ry));

      pathDraw.RoundedRect(t_p1, t_p2, t_p3, t_p4, t_p5, t_p6, t_p7, t_p8, t_rx, t_ry, angle);
    }
    return true;
  }
}
