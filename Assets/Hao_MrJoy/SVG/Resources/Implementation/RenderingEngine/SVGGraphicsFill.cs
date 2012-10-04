using UnityEngine;
using System;
using System.Collections;

public class SVGGraphicsFill : ISVGPathDraw {
  private const int FILL_FLAG = -1;
  private float[,] _neighbor = new float[4, 2] { { -1.0f, 0.0f }, { 0.0f, -1.0f }, { 1.0f, 0.0f }, { 0.0f, 1.0f } };

  private SVGGraphics _graphics;
  private SVGBasicDraw _basicDraw;

  private int _flagStep;
  private int[,] _flag;

  //Chieu rong va chieu dai cua picture;
  private int _width, _height;

  private int _translateX;
  private int _translateY;

  private int _subW, _subH;

  public SVGGraphicsFill(SVGGraphics graphics) {
    this._graphics = graphics;
    this._flagStep = 0;
    this._width = 0;
    this._height = 0;

    this._translateX = 0;
    this._translateY = 0;
    this._subW = this._subH = 0;
    //Basic Draw
    this._basicDraw = new SVGBasicDraw();
    this._basicDraw.SetPixelMethod = new SetPixelDelegate(SetPixelForFlag);
  }

  public void SetSize(float width, float height) {
    this._width = (int)width;
    this._height = (int)height;
    this._subW = this._width;
    this._subH = this._height;
    this._flag = new int[(int)width + 1, (int)height + 1];
  }

  public void SetColor(Color color) {
    this._graphics.SetColor(color);
  }

  private void SetPixelForFlag(int x, int y) {
    int tx = x + this._translateX;
    int ty = y + this._translateY;
    if(isInZone(tx, ty)) {
      this._flag[tx, ty] = this._flagStep;
    }
  }

  private int _inZoneL = 0, _inZoneT = 0;
  private bool isInZone(int x, int y) {
    if((x >= _inZoneL && x < this._subW + _inZoneL) && (y >= _inZoneT && y < this._subH + _inZoneT)) {
      return true;
    }
    return false;
  }

  private Vector2 _boundTopLeft, _boundBottomRight;

  private void ExpandBounds(Vector2 point) {
      if(point.x < _boundTopLeft.x) _boundTopLeft.x = point.x;
      if(point.y < _boundTopLeft.y) _boundTopLeft.y = point.y;

      if(point.x > _boundBottomRight.x) _boundBottomRight.x = point.x;
      if(point.y > _boundBottomRight.y) _boundBottomRight.y = point.y;
  }

  private void ExpandBounds(Vector2 point, float dx, float dy) {
      if(point.x - dy < _boundTopLeft.x) _boundTopLeft.x = point.x - dx;
      if(point.y - dx < _boundTopLeft.y) _boundTopLeft.y = point.y - dy;

      if(point.x + dx > _boundBottomRight.x) _boundBottomRight.x = point.x + dx;
      if(point.y + dy > _boundBottomRight.y) _boundBottomRight.y = point.y + dy;
  }

  //Tinh Bound cho Fill
  private void ExpandBounds(Vector2[] points) {
    int _length = points.Length;
    for(int i = 0; i < _length; i++)
      ExpandBounds(points[i]);
  }

  private void ExpandBounds(Vector2[] points, int deltax, int deltay) {
    int _length = points.Length;
    for(int i = 0; i < _length; i++)
      ExpandBounds(points[i], deltax, deltay);
  }

  //Fill se to lan tu vi tri(x,y)theo gia tri this._flagStep
  private static LiteStack<Vector2> _stack = new LiteStack<Vector2>();
  private void Fill(int x, int y) {
    if(!isInZone(x, y))
      return;
    _stack.Clear();

    Vector2 temp = new Vector2(x, y);
    this._flag[(int)temp.x, (int)temp.y] = FILL_FLAG;
    _stack.Push(temp);

    while(_stack.Count > 0) {
      temp = _stack.Pop();
      for(int t = 0; t < 4; t++) {
        float tx, ty;
        tx = temp.x + this._neighbor[t, 0];
        ty = temp.y + this._neighbor[t, 1];
        if(isInZone((int)tx, (int)ty)) {
          if(this._flag[(int)tx, (int)ty] == 0) {
            this._flag[(int)tx, (int)ty] = FILL_FLAG;
            _stack.Push(new Vector2(tx, ty));
          }
        }
      }
    }
  }

  public void Fill(float x, float y) {
    Fill((int)x, (int)y);
  }

  public void Fill(Vector2 point) {
    Fill((int)point.x, (int)point.y);
  }

  public void Fill(float x, float y, int flagStep) {
    this._flagStep = flagStep;
    Fill((int)x, (int)y);
  }

  public void Fill(Vector2 point, int flagStep) {
    this._flagStep = flagStep;
    Fill((int)point.x, (int)point.y);
  }

  public void BeginSubBuffer() {
    this._boundTopLeft = new Vector2(+10000f, +10000f);
    this._boundBottomRight = new Vector2(-10000f, -10000f);

    this._subW = this._width;
    this._subH = this._height;
    this._inZoneL = 0;
    this._inZoneT = 0;
    this._translateX = 0;
    this._translateY = 0;

    this._flagStep = 0;
    for(int i = 0; i < this._subW; i++)
      for(int j = 0; j < this._subH; j++)
        this._flag[i, j] = 0;
    this._flagStep = 1;
  }

  private void PreEndSubBuffer() {
    this._translateX = 0;
    this._translateY = 0;

    if(_boundTopLeft.x < 0f)
      _boundTopLeft.x = 0f;
    if(_boundTopLeft.y < 0f)
      _boundTopLeft.y = 0f;
    if(_boundBottomRight.x >= this._width)
      _boundBottomRight.x = this._width - 1f;
    if(_boundBottomRight.y >= this._height)
      _boundBottomRight.y = this._height - 1f;

    this._subW = (int)Math.Abs((int)_boundTopLeft.x - (int)_boundBottomRight.x) + 1 + (2 * 1);
    this._subH = (int)Math.Abs((int)_boundTopLeft.y - (int)_boundBottomRight.y) + 1 + (2 * 1);

    this._inZoneL = (int)_boundTopLeft.x - 1;
    this._inZoneT = (int)_boundTopLeft.y - 1;

    this._inZoneL = (_inZoneL < 0) ? 0 : _inZoneL;
    this._inZoneT = (_inZoneT < 0) ? 0 : _inZoneT;

    this._inZoneL = (_inZoneL >= this._width) ? (this._width - 1) : _inZoneL;
    this._inZoneT = (_inZoneT >= this._height) ? (this._height - 1) : _inZoneT;

    this._subW = (this._subW + this._inZoneL >= this._width) ? (this._width - this._inZoneL - 1) : _subW;
    this._subH = (this._subH + this._inZoneT >= this._height) ? (this._height - this._inZoneT - 1) : _subH;

    //Fill
    Fill(this._inZoneL, this._inZoneT);
    if((this._inZoneL == 0) && (this._inZoneT == 0)) {
      Fill(this._inZoneL + this._subW - 1, this._inZoneT + this._subH - 1);
    }
  }

  //Fill Solid color, No fill Stroke
  public void EndSubBuffer() {
    PreEndSubBuffer();

    Fill(this._inZoneL, this._inZoneT);
    for(int i = this._inZoneL; i < this._subW + this._inZoneL; i++)
      for(int j = this._inZoneT; j < this._subH + this._inZoneT; j++)
        if(this._flag[i, j] != -1)
          this._graphics.SetPixel(i, j);
  }

  //Fill Solid color, No fill Stroke
  public void EndSubBuffer(Vector2[] points) {
    PreEndSubBuffer();

    for(int i = 0; i < points.GetLength(0); i++)
      Fill(points[i].x, points[i].y);
    for(int i = this._inZoneL; i < this._subW + this._inZoneL; i++)
      for(int j = this._inZoneT; j < this._subH + this._inZoneT; j++)
        if(this._flag[i, j] != -1)
          this._graphics.SetPixel(i, j);
  }

  //Fill Solid color, with fill Stroke
  public void EndSubBuffer(SVGColor? strokePathColor) {
    PreEndSubBuffer();

    for(int i = this._inZoneL; i < this._subW + this._inZoneL; i++)
      for(int j = this._inZoneT; j < this._subH + this._inZoneT; j++)
        if(this._flag[i, j] == 0)
          this._graphics.SetPixel(i, j);

    this._graphics.SetColor(strokePathColor.Value.color);

    for(int i = this._inZoneL; i < this._subW + this._inZoneL; i++)
      for(int j = this._inZoneT; j < this._subH + this._inZoneT; j++)
        if(this._flag[i, j] > 0)
          this._graphics.SetPixel(i, j);
  }

  //Fill Linear Gradient, no fill Stroke
  public void EndSubBuffer(SVGLinearGradientBrush linearGradientBrush) {
    PreEndSubBuffer();
    for(int i = this._inZoneL; i < this._subW + this._inZoneL; i++) {
      for(int j = this._inZoneT; j < this._subH + this._inZoneT; j++) {
        if(this._flag[i, j] == 0) {
          Color _color = linearGradientBrush.GetColor(i, j);
          this._graphics.SetColor(_color);
          this._graphics.SetPixel(i, j);
        }
      }
    }
  }

  //Fill Linear Gradient, with fill Stroke
  public void EndSubBuffer(SVGLinearGradientBrush linearGradientBrush, SVGColor? strokePathColor) {
    PreEndSubBuffer();

    for(int i = this._inZoneL; i < this._subW + this._inZoneL; i++) {
      for(int j = this._inZoneT; j < this._subH + this._inZoneT; j++) {
        if(this._flag[i, j] != -1) {
          Color _color = linearGradientBrush.GetColor(i, j);
          this._graphics.SetColor(_color);
          this._graphics.SetPixel(i, j);
        }
      }
    }

    this._graphics.SetColor(strokePathColor.Value.color);

    for(int i = this._inZoneL; i < this._subW + this._inZoneL; i++)
      for(int j = this._inZoneT; j < this._subH + this._inZoneT; j++)
        if(this._flag[i, j] > 0)
          this._graphics.SetPixel(i, j);
  }

  //Fill Radial Gradient, no fill Stroke
  public void EndSubBuffer(SVGRadialGradientBrush radialGradientBrush) {
    PreEndSubBuffer();

    for(int i = this._inZoneL; i < this._subW + this._inZoneL; i++) {
      for(int j = this._inZoneT; j < this._subH + this._inZoneT; j++) {
        if(this._flag[i, j] == 0) {
          Color _color = radialGradientBrush.GetColor(i, j);
          this._graphics.SetColor(_color);
          this._graphics.SetPixel(i, j);
        }
      }
    }
  }

  //Fill Radial Gradient, with fill Stroke
  public void EndSubBuffer(SVGRadialGradientBrush radialGradientBrush, SVGColor? strokePathColor) {
    PreEndSubBuffer();

    for(int i = this._inZoneL; i < this._subW + this._inZoneL; i++) {
      for(int j = this._inZoneT; j < this._subH + this._inZoneT; j++) {
        if(this._flag[i, j] != -1) {
          Color _color = radialGradientBrush.GetColor(i, j);
          this._graphics.SetColor(_color);
          this._graphics.SetPixel(i, j);
        }
      }
    }

    this._graphics.SetColor(strokePathColor.Value.color);

    for(int i = this._inZoneL; i < this._subW + this._inZoneL; i++)
      for(int j = this._inZoneT; j < this._subH + this._inZoneT; j++)
        if(this._flag[i, j] > 0)
          this._graphics.SetPixel(i, j);
  }

  private void PreRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
    BeginSubBuffer();

    ExpandBounds(p1);
    ExpandBounds(p2);
    ExpandBounds(p3);
    ExpandBounds(p4);

    this._basicDraw.MoveTo(p1);
    this._basicDraw.LineTo(p2);
    this._basicDraw.LineTo(p3);
    this._basicDraw.LineTo(p4);
    this._basicDraw.LineTo(p1);
  }
  //-----
  public void Rect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
    PreRect(p1, p2, p3, p4);
    EndSubBuffer();
  }
  //-----
  public void Rect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, SVGColor? strokeColor) {
    PreRect(p1, p2, p3, p4);
    EndSubBuffer(strokeColor);
  }
  //-----
  public void Rect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, SVGColor fillColor, SVGColor? strokeColor) {
    SetColor(fillColor.color);
    PreRect(p1, p2, p3, p4);
    EndSubBuffer(strokeColor);
  }
  //--------------------------------------------------------------------------------
  //Method: RoundedRect
  //--------------------------------------------------------------------------------
  private void PreRoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 p5, Vector2 p6, Vector2 p7, Vector2 p8, float r1, float r2, float angle) {
    float dxy = ((r1 > r2) ? (int)r1 : (int)r2);

    BeginSubBuffer();

    ExpandBounds(p1, dxy, dxy);
    ExpandBounds(p2, dxy, dxy);
    ExpandBounds(p3, dxy, dxy);
    ExpandBounds(p4, dxy, dxy);
    ExpandBounds(p5, dxy, dxy);
    ExpandBounds(p6, dxy, dxy);
    ExpandBounds(p7, dxy, dxy);
    ExpandBounds(p8, dxy, dxy);

    this._basicDraw.MoveTo(p1);
    this._basicDraw.LineTo(p2);
    this._basicDraw.ArcTo(r1, r2, angle, false, true, p3);

    this._basicDraw.MoveTo(p3);
    this._basicDraw.LineTo(p4);
    this._basicDraw.ArcTo(r1, r2, angle, false, true, p5);

    this._basicDraw.MoveTo(p5);
    this._basicDraw.LineTo(p6);
    this._basicDraw.ArcTo(r1, r2, angle, false, true, p7);

    this._basicDraw.MoveTo(p7);
    this._basicDraw.LineTo(p8);
    this._basicDraw.ArcTo(r1, r2, angle, false, true, p1);


  }
  //-----
  public void RoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 p5, Vector2 p6, Vector2 p7, Vector2 p8, float r1, float r2,
  float angle) {
    PreRoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2,
    angle);
    EndSubBuffer();
  }
  //-----
  public void RoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 p5, Vector2 p6, Vector2 p7, Vector2 p8, float r1, float r2,
  float angle, SVGColor? strokeColor) {
    PreRoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2,
    angle);
    EndSubBuffer(strokeColor);
  }
  //-----
  public void RoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 p5, Vector2 p6, Vector2 p7, Vector2 p8, float r1, float r2,
  float angle, SVGColor fillColor, SVGColor? strokeColor) {
    SetColor(fillColor.color);
    PreRoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2,
    angle);
    EndSubBuffer(strokeColor);
  }
  //--------------------------------------------------------------------------------
  //Method: CircleFill
  //--------------------------------------------------------------------------------
  private void PreCircle(Vector2 p, float r) {
    Vector2[] points = new Vector2[1];
    points[0] = p;
    BeginSubBuffer();
    ExpandBounds(points, (int)r + 2, (int)r + 2);

    this._basicDraw.Circle(p, r);
  }
  //-----
  public void Circle(Vector2 p, float r) {
    PreCircle(p, r);
    EndSubBuffer();
  }
  //-----
  public void Circle(Vector2 p, float r, SVGColor? strokeColor) {
    PreCircle(p, r);
    EndSubBuffer(strokeColor);
  }
  //-----
  public void Circle(Vector2 p, float r, SVGColor fillColor, SVGColor? strokeColor) {
    SetColor(fillColor.color);
    PreCircle(p, r);
    EndSubBuffer();
  }
  //--------------------------------------------------------------------------------
  //Method: Ellipse
  //--------------------------------------------------------------------------------
  private void PreEllipse(Vector2 p, float rx, float ry, float angle) {
    Vector2[] points = new Vector2[1];
    points[0] = p;
    BeginSubBuffer();
    ExpandBounds(points, ((rx > ry) ? (int)rx : (int)ry), ((rx > ry) ? (int)rx : (int)ry));

    this._basicDraw.Ellipse(p, (int)rx, (int)ry, angle);
  }
  //-----
  public void Ellipse(Vector2 p, float rx, float ry, float angle) {
    PreEllipse(p, rx, ry, angle);
    EndSubBuffer();
  }
  //-----
  public void Ellipse(Vector2 p, float rx, float ry, float angle, SVGColor? strokeColor) {
    PreEllipse(p, rx, ry, angle);
    EndSubBuffer(strokeColor);
  }
  //-----
  public void Ellipse(Vector2 p, float rx, float ry, float angle, SVGColor fillColor, SVGColor? strokeColor) {
    SetColor(fillColor.color);
    PreEllipse(p, rx, ry, angle);
    EndSubBuffer(strokeColor);
  }
  //--------------------------------------------------------------------------------
  //Method: Polygon.
  //--------------------------------------------------------------------------------
  private void PrePolygon(Vector2[] points) {
    if((points != null) && (points.GetLength(0) > 0)) {
      BeginSubBuffer();
      ExpandBounds(points);

      this._basicDraw.MoveTo(points[0]);
      int _length = points.GetLength(0);
      for(int i = 1; i < _length; i++)
        this._basicDraw.LineTo(points[i]);
      this._basicDraw.LineTo(points[0]);
    }
  }
  //-----
  public void Polygon(Vector2[] points) {
    PrePolygon(points);
    EndSubBuffer();
  }
  //-----
  public void Polygon(Vector2[] points, SVGColor? strokeColor) {
    PrePolygon(points);
    EndSubBuffer(strokeColor);
  }
  //-----
  public void Polygon(Vector2[] points, SVGColor fillColor, SVGColor? strokeColor) {
    SetColor(fillColor.color);
    PrePolygon(points);
    EndSubBuffer(strokeColor);
  }
  //--------------------------------------------------------------------------------
  //Method: Polyline.
  //--------------------------------------------------------------------------------
  public void Polyline(Vector2[] points) {
    Polygon(points);
  }
  //-----
  public void Polyline(Vector2[] points, SVGColor? strokeColor) {
    Polygon(points, strokeColor);
  }
  //-----
  public void Polyline(Vector2[] points, SVGColor fillColor, SVGColor? strokeColor) {
    Polygon(points, fillColor, strokeColor);
  }

  //--------------------------------------------------------------------------------
  //Method: Fill Path
  //--------------------------------------------------------------------------------
  public void FillPath(SVGGraphicsPath graphicsPath) {
    BeginSubBuffer();
    graphicsPath.RenderPath(this, true);
    EndSubBuffer();
  }
  //-----
  public void FillPath(SVGGraphicsPath graphicsPath, Vector2[] points) {
    BeginSubBuffer();
    graphicsPath.RenderPath(this, true);
    EndSubBuffer(points);
  }
  //-----
  //Path Solid Fill
  public void FillPath(SVGColor fillColor, SVGGraphicsPath graphicsPath) {
    SetColor(fillColor.color);
    FillPath(graphicsPath);
  }
  //-----
  public void FillPath(SVGColor fillColor, SVGColor? strokePathColor, SVGGraphicsPath graphicsPath) {
    BeginSubBuffer();
    graphicsPath.RenderPath(this, true);
    if(strokePathColor != null) {
      EndSubBuffer(strokePathColor);
    } else {
      EndSubBuffer();
    }
  }
  //-----
  //Path Linear Fill
  public void FillPath(SVGLinearGradientBrush linearGradientBrush, SVGGraphicsPath graphicsPath) {
    BeginSubBuffer();
    graphicsPath.RenderPath(this, true);
    EndSubBuffer(linearGradientBrush);
  }
  //-----
  public void FillPath(SVGLinearGradientBrush linearGradientBrush, SVGColor? strokePathColor, SVGGraphicsPath graphicsPath) {
    BeginSubBuffer();
    graphicsPath.RenderPath(this, true);
    if(strokePathColor != null) {
      EndSubBuffer(linearGradientBrush, strokePathColor);
    } else {
      EndSubBuffer(linearGradientBrush);
    }
  }
  //-----
  //Path Radial Fill
  public void FillPath(SVGRadialGradientBrush radialGradientBrush, SVGGraphicsPath graphicsPath) {
    BeginSubBuffer();
    graphicsPath.RenderPath(this, true);
    EndSubBuffer(radialGradientBrush);
  }
  //-----
  public void FillPath(SVGRadialGradientBrush radialGradientBrush, SVGColor? strokePathColor, SVGGraphicsPath graphicsPath) {
    BeginSubBuffer();
    graphicsPath.RenderPath(this, true);
    if(strokePathColor != null) {
      EndSubBuffer(radialGradientBrush, strokePathColor);
    } else {
      EndSubBuffer(radialGradientBrush);
    }
  }

  //================================================================================
  //--------------------------------------------------------------------------------
  //Method: CircleTo
  //--------------------------------------------------------------------------------
  public void CircleTo(Vector2 p, float r) {
    Vector2[] points = new Vector2[1];
    points[0] = p;
    ExpandBounds(points, (int)r + 1, (int)r + 1);

    //---------------
    this._basicDraw.Circle(p, r);
  }
  //--------------------------------------------------------------------------------
  //Method: EllipseTo
  //--------------------------------------------------------------------------------
  public void EllipseTo(Vector2 p, float rx, float ry, float angle) {
    Vector2[] points = new Vector2[1];
    points[0] = p;
    ExpandBounds(points, ((rx > ry) ? (int)rx + 2 : (int)ry + 2), ((rx > ry) ? (int)rx + 2 : (int)ry + 2));

    //---------------
    this._basicDraw.Ellipse(p, (int)rx, (int)ry, angle);
  }
  //--------------------------------------------------------------------------------
  //Method: LineTo4Path
  //--------------------------------------------------------------------------------
  public void LineTo(Vector2 p) {
    Vector2[] points = new Vector2[1];
    points[0] = p;
    ExpandBounds(points);
    //---------------
    this._basicDraw.LineTo(p);
  }
  //--------------------------------------------------------------------------------
  //Method: MoveTo
  //--------------------------------------------------------------------------------
  public void MoveTo(Vector2 p) {
    Vector2[] points = new Vector2[1];
    points[0] = p;
    ExpandBounds(points);
    //---------------
    this._basicDraw.MoveTo(p);
  }

  /*-------------------------------------------------------------------------------
  //Method: Arc4Path
  /-------------------------------------------------------------------------------*/
  public void ArcTo(float r1, float r2, float angle, bool largeArcFlag, bool sweepFlag, Vector2 p) {
    Vector2[] points = new Vector2[1];
    points[0] = p;
    ExpandBounds(points, (r1 > r2) ? 2 * (int)r1 + 2 : 2 * (int)r2 + 2, (r1 > r2) ? 2 * (int)r1 + 2 : 2 * (int)r2 + 2);
    //---------------
    this._basicDraw.ArcTo(r1, r2, angle, largeArcFlag, sweepFlag, p);
  }
  /*-------------------------------------------------------------------------------
  //Method: CubicCurveTo4Path
  /-------------------------------------------------------------------------------*/
  public void CubicCurveTo(Vector2 p1, Vector2 p2, Vector2 p) {
    Vector2[] points = new Vector2[3];
    points[0] = p1;
    points[1] = p2;
    points[2] = p;
    ExpandBounds(points);
    //---------------
    this._basicDraw.CubicCurveTo(p1, p2, p);
  }

  /*-------------------------------------------------------------------------------
  //Method: QuadraticCurveTo4Path
  /-------------------------------------------------------------------------------*/
  public void QuadraticCurveTo(Vector2 p1, Vector2 p) {
    Vector2[] points = new Vector2[2];
    points[0] = p1;
    points[1] = p;
    ExpandBounds(points);
    //---------------
    this._basicDraw.QuadraticCurveTo(p1, p);
  }
}
