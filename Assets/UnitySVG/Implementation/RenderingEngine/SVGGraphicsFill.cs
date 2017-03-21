using UnityEngine;
using UnityEngine.Profiling;
using System;

// TODO: Find a way to break this the hell up.

// TODO: Normalize conventions away from Java-style SetX to properties.
public class SVGGraphicsFill : ISVGPathDraw {
  private const sbyte FILL_FLAG = -1;
  private readonly int[,] _neighbor = { { -1, 0 }, { 0, -1 }, { 1, 0 }, { 0, 1 } };

  private readonly SVGGraphics graphics;
  private readonly SVGBasicDraw basicDraw;

  private sbyte flagStep;
  private sbyte[,] flag;

  private int width, height;

  private int subW, subH;
  private int inZoneL, inZoneT;

  private Vector2 boundTopLeft, boundBottomRight;

  public SVGGraphicsFill(SVGGraphics _graphics) {
    graphics = _graphics;
    flagStep = 0;
    width = 0;
    height = 0;

    subW = subH = 0;
    //Basic Draw
    basicDraw = new SVGBasicDraw { SetPixelMethod = SetPixelForFlag };
  }

  public Vector2 Size {
    set {
      width   = (int)value.x;
      height  = (int)value.y;
      subW    = width;
      subH    = height;
      flag    = new sbyte[(int)width + 1, (int)height + 1];
    }
  }

  public void SetColor(Color color) {
    graphics.SetColor(color);
  }

  private void SetPixelForFlag(int x, int y) {
    if(isInZone(x, y))
      flag[x, y] = flagStep;
  }

  private bool isInZone(int x, int y) {
    return ((x >= inZoneL && x < subW + inZoneL) && (y >= inZoneT && y < subH + inZoneT));
  }

  private void ExpandBounds(Vector2 point) {
    if(point.x < boundTopLeft.x)
      boundTopLeft.x = point.x;
    if(point.y < boundTopLeft.y)
      boundTopLeft.y = point.y;

    if(point.x > boundBottomRight.x)
      boundBottomRight.x = point.x;
    if(point.y > boundBottomRight.y)
      boundBottomRight.y = point.y;
  }

  private void ExpandBounds(Vector2 point, float dx, float dy) {
    if(point.x - dy < boundTopLeft.x)
      boundTopLeft.x = point.x - dx;
    if(point.y - dx < boundTopLeft.y)
      boundTopLeft.y = point.y - dy;

    if(point.x + dx > boundBottomRight.x)
      boundBottomRight.x = point.x + dx;
    if(point.y + dy > boundBottomRight.y)
      boundBottomRight.y = point.y + dy;
  }

  private void ExpandBounds(Vector2[] points) {
    int _length = points.Length;
    for(int i = 0; i < _length; i++)
      ExpandBounds(points[i]);
  }

  private static readonly LiteStack<IntVector2> _stack = new LiteStack<IntVector2>();

  private struct IntVector2 {
    public int x;
    public int y;
  }

  private void Fill(int x, int y) {
    // Debug.LogFormat("Fill called: w:{0}, h:{1}, subW:{2}, subH:{3}, inZoneL:{4}, inZoneT:{5}, x:{6}, y:{7}", width, height, subW, subH, inZoneL, inZoneT, x, y);
    Profiler.BeginSample("SVGGraphicsFill.Fill");
    if(!isInZone(x, y) || flag[x, y] != 0) {
      Profiler.EndSample();
      return;
    }

    flag[x, y] = FILL_FLAG;
    _stack.Clear();

    int anticipatedCapacityNeed = ((width + height) / 2) * 5;
    if(_stack.Capacity < anticipatedCapacityNeed)
      _stack.Capacity = anticipatedCapacityNeed;

    IntVector2 temp = new IntVector2 { x = x, y = y };
    _stack.Push(temp);

    int nbIterations = 0;
    while(_stack.Count > 0) {
      ++nbIterations;
      temp = _stack.Pop();
      for(int t = 0; t < 4; ++t) {
        int tx = temp.x + _neighbor[t, 0];
        int ty = temp.y + _neighbor[t, 1];
        if(isInZone(tx, ty) && flag[tx, ty] == 0) {
          flag[tx, ty] = FILL_FLAG;
          _stack.Push(new IntVector2 { x = tx, y = ty });
        }
      }
    }
    // Debug.LogFormat("NbIter:{0}", nbIterations);
    Profiler.EndSample();
  }

  public void ResetSubBuffer() {
    boundTopLeft = new Vector2(+10000f, +10000f);
    boundBottomRight = new Vector2(-10000f, -10000f);

    subW = width;
    subH = height;
    inZoneL = 0;
    inZoneT = 0;

    for(int i = 0; i < width; i++)
      for(int j = 0; j < height; j++)
        flag[i, j] = 0;
    flagStep = 1;
  }

  private void PreEndSubBuffer() {
    if(boundTopLeft.x < 0f)
      boundTopLeft.x = 0f;
    if(boundTopLeft.y < 0f)
      boundTopLeft.y = 0f;
    if(boundBottomRight.x >= width)
      boundBottomRight.x = width - 1f;
    if(boundBottomRight.y >= height)
      boundBottomRight.y = height - 1f;

    subW = Math.Abs((int)boundTopLeft.x - (int)boundBottomRight.x) + 3;
    subH = Math.Abs((int)boundTopLeft.y - (int)boundBottomRight.y) + 3;

    inZoneL = (int)boundTopLeft.x - 1;
    inZoneT = (int)boundTopLeft.y - 1;

    inZoneL = (inZoneL < 0) ? 0 : inZoneL;
    inZoneT = (inZoneT < 0) ? 0 : inZoneT;

    inZoneL = (inZoneL >= width) ? (width - 1) : inZoneL;
    inZoneT = (inZoneT >= height) ? (height - 1) : inZoneT;

    subW = (subW + inZoneL > width) ? (width - inZoneL) : subW;
    subH = (subH + inZoneT > height) ? (height - inZoneT) : subH;

    Fill(inZoneL, inZoneT);
    // TODO: This seems buggy:
    if((inZoneL == 0) && (inZoneT == 0))
      Fill(subW - 1, subH - 1);
  }

  private void FillInZone() {
    for(int i = inZoneL; i < subW + inZoneL; i++)
      for(int j = inZoneT; j < subH + inZoneT; j++)
        if(flag[i, j] != FILL_FLAG)
          graphics.SetPixel(i, j);
  }

  public void EndSubBuffer() {
    PreEndSubBuffer();
    Fill(inZoneL, inZoneT);
    FillInZone();
  }

  public void EndSubBuffer(Vector2[] points) {
    PreEndSubBuffer();
    for(int i = 0; i < points.GetLength(0); i++)
      Fill((int)points[i].x, (int)points[i].y);
    FillInZone();
  }

  public void EndSubBuffer(Vector2 point) {
    PreEndSubBuffer();

    Fill((int)point.x, (int)point.y);
    FillInZone();
  }

  public void EndSubBuffer(SVGColor? strokePathColor) {
    PreEndSubBuffer();

    FillInZone();

    graphics.SetColor(strokePathColor.Value.color);

    FillInZone();
  }

  public void EndSubBuffer(SVGLinearGradientBrush linearGradientBrush) {
    PreEndSubBuffer();
    for(int i = inZoneL; i < subW + inZoneL; i++) {
      for(int j = inZoneT; j < subH + inZoneT; j++) {
        if(flag[i, j] == 0) {
          Color _color = linearGradientBrush.GetColor(i, j);
          graphics.SetColor(_color);
          graphics.SetPixel(i, j);
        }
      }
    }
  }

  public void EndSubBuffer(SVGLinearGradientBrush linearGradientBrush, SVGColor? strokePathColor) {
    PreEndSubBuffer();

    for(int i = inZoneL; i < subW + inZoneL; i++) {
      for(int j = inZoneT; j < subH + inZoneT; j++) {
        if(flag[i, j] != FILL_FLAG) {
          Color _color = linearGradientBrush.GetColor(i, j);
          graphics.SetColor(_color);
          graphics.SetPixel(i, j);
        }
      }
    }

    graphics.SetColor(strokePathColor.Value.color);

    FillInZone();
  }

  public void EndSubBuffer(SVGRadialGradientBrush radialGradientBrush) {
    PreEndSubBuffer();

    for(int i = inZoneL; i < subW + inZoneL; i++) {
      for(int j = inZoneT; j < subH + inZoneT; j++) {
        if(flag[i, j] == 0) {
          Color _color = radialGradientBrush.GetColor(i, j);
          graphics.SetColor(_color);
          graphics.SetPixel(i, j);
        }
      }
    }
  }

  public void EndSubBuffer(SVGRadialGradientBrush radialGradientBrush, SVGColor? strokePathColor) {
    PreEndSubBuffer();

    for(int i = inZoneL; i < subW + inZoneL; ++i) {
      for(int j = inZoneT; j < subH + inZoneT; ++j) {
        if(flag[i, j] != FILL_FLAG) {
          Color _color = radialGradientBrush.GetColor(i, j);
          graphics.SetColor(_color);
          graphics.SetPixel(i, j);
        }
      }
    }

    graphics.SetColor(strokePathColor.Value.color);

    FillInZone();
  }

  private void PreRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
    ResetSubBuffer();

    ExpandBounds(p1);
    ExpandBounds(p2);
    ExpandBounds(p3);
    ExpandBounds(p4);

    basicDraw.MoveTo(p1);
    basicDraw.LineTo(p2);
    basicDraw.LineTo(p3);
    basicDraw.LineTo(p4);
    basicDraw.LineTo(p1);
  }

  public void Rect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
    PreRect(p1, p2, p3, p4);
    EndSubBuffer();
  }

  public void Rect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, SVGColor? strokeColor) {
    PreRect(p1, p2, p3, p4);
    EndSubBuffer(strokeColor);
  }

  public void Rect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, SVGColor fillColor, SVGColor? strokeColor) {
    SetColor(fillColor.color);
    PreRect(p1, p2, p3, p4);
    EndSubBuffer(strokeColor);
  }

  private void PreRoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 p5, Vector2 p6, Vector2 p7,
                              Vector2 p8, float r1, float r2, float angle) {
    float dxy = ((r1 > r2) ? (int)r1 : (int)r2);

    ResetSubBuffer();

    ExpandBounds(p1, dxy, dxy);
    ExpandBounds(p2, dxy, dxy);
    ExpandBounds(p3, dxy, dxy);
    ExpandBounds(p4, dxy, dxy);
    ExpandBounds(p5, dxy, dxy);
    ExpandBounds(p6, dxy, dxy);
    ExpandBounds(p7, dxy, dxy);
    ExpandBounds(p8, dxy, dxy);

    basicDraw.MoveTo(p1);
    basicDraw.LineTo(p2);
    basicDraw.ArcTo(r1, r2, angle, false, true, p3);

    basicDraw.MoveTo(p3);
    basicDraw.LineTo(p4);
    basicDraw.ArcTo(r1, r2, angle, false, true, p5);

    basicDraw.MoveTo(p5);
    basicDraw.LineTo(p6);
    basicDraw.ArcTo(r1, r2, angle, false, true, p7);

    basicDraw.MoveTo(p7);
    basicDraw.LineTo(p8);
    basicDraw.ArcTo(r1, r2, angle, false, true, p1);
  }

  public void RoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 p5, Vector2 p6, Vector2 p7, Vector2 p8,
                          float r1, float r2,
                          float angle) {
    PreRoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2,
                   angle);
    EndSubBuffer();
  }

  public void RoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 p5, Vector2 p6, Vector2 p7, Vector2 p8,
                          float r1, float r2,
                          float angle, SVGColor? strokeColor) {
    PreRoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2,
                   angle);
    EndSubBuffer(strokeColor);
  }


  public void RoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 p5, Vector2 p6, Vector2 p7, Vector2 p8,
                          float r1, float r2,
                          float angle, SVGColor fillColor, SVGColor? strokeColor) {
    SetColor(fillColor.color);
    PreRoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2,
                   angle);
    EndSubBuffer(strokeColor);
  }

  private void PreCircle(Vector2 p, float r) {
    ResetSubBuffer();
    ExpandBounds(p, (int)r + 2, (int)r + 2);

    basicDraw.Circle(p, r);
  }

  public void Circle(Vector2 p, float r) {
    PreCircle(p, r);
    EndSubBuffer();
  }

  public void Circle(Vector2 p, float r, SVGColor? strokeColor) {
    PreCircle(p, r);
    EndSubBuffer(strokeColor);
  }

  public void Circle(Vector2 p, float r, SVGColor fillColor, SVGColor? strokeColor) {
    SetColor(fillColor.color);
    PreCircle(p, r);
    EndSubBuffer();
  }

  private void PreEllipse(Vector2 p, float rx, float ry, float angle) {
    int d = (rx > ry) ? (int)rx : (int)ry;
    ExpandBounds(p, d, d);

    basicDraw.Ellipse(p, (int)rx, (int)ry, angle);
  }

  public void Ellipse(Vector2 p, float rx, float ry, float angle) {
    PreEllipse(p, rx, ry, angle);
    EndSubBuffer();
  }

  public void Ellipse(Vector2 p, float rx, float ry, float angle, SVGColor? strokeColor) {
    PreEllipse(p, rx, ry, angle);
    EndSubBuffer(strokeColor);
  }

  public void Ellipse(Vector2 p, float rx, float ry, float angle, SVGColor fillColor, SVGColor? strokeColor) {
    SetColor(fillColor.color);
    PreEllipse(p, rx, ry, angle);
    EndSubBuffer(strokeColor);
  }

  private void PrePolygon(Vector2[] points) {
    if((points != null) && (points.GetLength(0) > 0)) {
      ResetSubBuffer();
      ExpandBounds(points);

      basicDraw.MoveTo(points[0]);
      int _length = points.GetLength(0);
      for(int i = 1; i < _length; i++)
        basicDraw.LineTo(points[i]);
      basicDraw.LineTo(points[0]);
    }
  }

  public void Polygon(Vector2[] points) {
    PrePolygon(points);
    EndSubBuffer();
  }

  public void Polygon(Vector2[] points, SVGColor? strokeColor) {
    PrePolygon(points);
    EndSubBuffer(strokeColor);
  }

  public void Polygon(Vector2[] points, SVGColor fillColor, SVGColor? strokeColor) {
    SetColor(fillColor.color);
    PrePolygon(points);
    EndSubBuffer(strokeColor);
  }

  public void FillPath(SVGGraphicsPath graphicsPath) {
    ResetSubBuffer();
    graphicsPath.RenderPath(this, true);
    EndSubBuffer();
  }

  public void FillPath(SVGGraphicsPath graphicsPath, Vector2[] points) {
    ResetSubBuffer();
    graphicsPath.RenderPath(this, true);
    EndSubBuffer(points);
  }

  public void FillPath(SVGGraphicsPath graphicsPath, Vector2 point) {
    ResetSubBuffer();
    graphicsPath.RenderPath(this, true);
    EndSubBuffer(point);
  }

  public void FillPath(SVGColor fillColor, SVGGraphicsPath graphicsPath) {
    SetColor(fillColor.color);
    FillPath(graphicsPath);
  }

  public void FillPath(SVGColor fillColor, SVGColor? strokePathColor, SVGGraphicsPath graphicsPath) {
    ResetSubBuffer();
    graphicsPath.RenderPath(this, true);
    if(strokePathColor != null)
      EndSubBuffer(strokePathColor);
    else
      EndSubBuffer();
  }

  public void FillPath(SVGLinearGradientBrush linearGradientBrush, SVGGraphicsPath graphicsPath) {
    ResetSubBuffer();
    graphicsPath.RenderPath(this, true);
    EndSubBuffer(linearGradientBrush);
  }

  public void FillPath(SVGLinearGradientBrush linearGradientBrush, SVGColor? strokePathColor,
                       SVGGraphicsPath graphicsPath) {
    ResetSubBuffer();
    graphicsPath.RenderPath(this, true);
    if(strokePathColor != null)
      EndSubBuffer(linearGradientBrush, strokePathColor);
    else
      EndSubBuffer(linearGradientBrush);
  }

  public void FillPath(SVGRadialGradientBrush radialGradientBrush, SVGGraphicsPath graphicsPath) {
    ResetSubBuffer();
    graphicsPath.RenderPath(this, true);
    EndSubBuffer(radialGradientBrush);
  }


  public void FillPath(SVGRadialGradientBrush radialGradientBrush, SVGColor? strokePathColor,
                       SVGGraphicsPath graphicsPath) {
    ResetSubBuffer();
    graphicsPath.RenderPath(this, true);
    if(strokePathColor != null)
      EndSubBuffer(radialGradientBrush, strokePathColor);
    else
      EndSubBuffer(radialGradientBrush);
  }

  public void CircleTo(Vector2 p, float r) {
    ExpandBounds(p, (int)r + 1, (int)r + 1);
    basicDraw.Circle(p, r);
  }

  public void EllipseTo(Vector2 p, float rx, float ry, float angle) {
    int d = (rx > ry) ? (int)rx + 2 : (int)ry + 2;
    ExpandBounds(p, d, d);
    basicDraw.Ellipse(p, (int)rx, (int)ry, angle);
  }

  public void LineTo(Vector2 p) {
    ExpandBounds(p);
    basicDraw.LineTo(p);
  }

  public void MoveTo(Vector2 p) {
    ExpandBounds(p);
    basicDraw.MoveTo(p);
  }

  public void ArcTo(float r1, float r2, float angle, bool largeArcFlag, bool sweepFlag, Vector2 p) {
    ExpandBounds(p, (r1 > r2) ? 2 * (int)r1 + 2 : 2 * (int)r2 + 2, (r1 > r2) ? 2 * (int)r1 + 2 : 2 * (int)r2 + 2);
    basicDraw.ArcTo(r1, r2, angle, largeArcFlag, sweepFlag, p);
  }

  public void CubicCurveTo(Vector2 p1, Vector2 p2, Vector2 p) {
    ExpandBounds(p1);
    ExpandBounds(p2);
    ExpandBounds(p);
    basicDraw.CubicCurveTo(p1, p2, p);
  }

  public void QuadraticCurveTo(Vector2 p1, Vector2 p) {
    ExpandBounds(p1);
    ExpandBounds(p);
    basicDraw.QuadraticCurveTo(p1, p);
  }
}
