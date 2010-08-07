using UnityEngine;
using System;
using System.Collections;

public class uSVGGraphicsFill : uISVGPathDraw {
  //================================================================================
  private const int _fillFlag = -1;
  private float[,] _neighbor = new float[4, 2] {  {-1.0f, 0.0f},
                        {0.0f, -1.0f},
                        {1.0f, 0.0f},
                        {0.0f, 1.0f}};

  private uSVGGraphics _graphics;
  private uSVGBasicDraw _basicDraw;

  private int _flagStep;
  private int[,] _flag;

  //Chieu rong va chieu dai cua picture;
  private int _width, _height;

  private int _translateX;
  private int _translateY;

  private int _subW, _subH;

  //================================================================================
  public uSVGGraphicsFill(uSVGGraphics graphics) {
    this._graphics = graphics;
    this._flagStep = 0;
    this._width = 0;
    this._height = 0;

    this._translateX = 0;
    this._translateY = 0;
    this._subW = this._subH = 0;
    //Basic Draw
    this._basicDraw = new uSVGBasicDraw();
    this._basicDraw.SetPixelMethod = new SetPixelDelegate(SetPixelForFlag);
  }
  //================================================================================
  //--------------------------------------------------------------------------------
  //SetSize
  //--------------------------------------------------------------------------------
  public void SetSize(float width, float height) {
    this._width = (int)width;
    this._height = (int)height;
    this._subW = this._width;
    this._subH = this._height;
    this._flag = new int[(int)width + 1, (int)height + 1];
  }
  //--------------------------------------------------------------------------------
  //SetColor
  //--------------------------------------------------------------------------------
  public void SetColor(Color color) {
    this._graphics.SetColor(color);
  }
  //--------------------------------------------------------------------------------
  //SetPixelForFlag
  //--------------------------------------------------------------------------------
  private void SetPixelForFlag(int x, int y) {

    int tx = x + this._translateX;
    int ty = y + this._translateY;
    if(isInZone(tx, ty)) {
      this._flag[tx, ty] = this._flagStep;
    }
  }

  //--------------------------------------------------------------------------------
  //isInZone
  //--------------------------------------------------------------------------------
  private int _inZoneL = 0, _inZoneT = 0;
  private bool isInZone(int x, int y) {
    if((x>=_inZoneL && x< this._subW+_inZoneL)&&(y>=_inZoneT && y<this._subH+_inZoneT)) {
      return true;
    }
    return false;
  }
  //================================================================================
  private uSVGPoint _boundTopLeft;
  private  uSVGPoint _boundBottomRight;
  //-----
  //Tinh Bound cho Fill
  private void ResetLimitPoints(uSVGPoint[] points) {
    int _length = points.GetLength(0);
    for(int i = 0; i < _length; i++) {
      if(points[i].x < this._boundTopLeft.x)this._boundTopLeft.x = points[i].x;
      if(points[i].y < this._boundTopLeft.y)this._boundTopLeft.y = points[i].y;

      if(points[i].x > this._boundBottomRight.x)this._boundBottomRight.x = points[i].x;
      if(points[i].y > this._boundBottomRight.y)this._boundBottomRight.y = points[i].y;
    }

  }
  //-----
  private void ResetLimitPoints(uSVGPoint[] points, int deltax, int deltay) {
    int _length = points.GetLength(0);
    for(int i = 0; i < _length; i++) {
      if((points[i].x - deltax) < this._boundTopLeft.x)
                      this._boundTopLeft.x = points[i].x - deltax;
      if((points[i].y - deltay) < this._boundTopLeft.y)
                      this._boundTopLeft.y = points[i].y - deltay;

      if((points[i].x + deltax) > this._boundBottomRight.x)
                      this._boundBottomRight.x = points[i].x + deltax;
      if((points[i].y + deltay) > this._boundBottomRight.y)
                      this._boundBottomRight.y = points[i].y + deltay;
    }
  }
  //--------------------------------------------------------------------------------
  //Method: Fill
  //Fill se to lan tu vi tri(x,y)theo gia tri this._flagStep
  //--------------------------------------------------------------------------------
  private void Fill(int x, int y) {
    if(!isInZone(x, y))return;
    LiteStack<uSVGPoint> _stack = new LiteStack<uSVGPoint>();

    uSVGPoint temp = new uSVGPoint(x, y);
    this._flag[(int)temp.x, (int)temp.y] = _fillFlag;
    _stack.Push(temp);

    while(_stack.Count > 0) {
        temp = _stack.Pop();
        for(int t = 0; t < 4; t++) {
          float tx, ty;
          tx = temp.x + this._neighbor[t,0];
          ty = temp.y + this._neighbor[t,1];
          if(isInZone((int)tx, (int)ty)) {
            if(this._flag[(int)tx, (int)ty] == 0) {
              this._flag[(int)tx, (int)ty] = _fillFlag;
              _stack.Push(new uSVGPoint(tx, ty));
            }
          }
        }
    }
  }
  //-----
  public void Fill(float x, float y) {
    Fill((int)x, (int)y);
  }
  //-----
  public void Fill(uSVGPoint point) {
    Fill((int)point.x, (int)point.y);
  }
  //-----
  public void Fill(float x, float y, int flagStep) {
    this._flagStep = flagStep;
    Fill((int)x, (int)y);
  }
  //-----
  public void Fill(uSVGPoint point, int flagStep) {
    this._flagStep = flagStep;
    Fill((int)point.x, (int)point.y);
  }
  //================================================================================
  //--------------------------------------------------------------------------------
  //Sub Buffer For Path Process
  //--------------------------------------------------------------------------------
  public void BeginSubBuffer() {

    this._boundTopLeft   = new uSVGPoint(+10000f, +10000f);
    this._boundBottomRight  = new uSVGPoint(-10000f, -10000f);

    this._subW = this._width;
    this._subH = this._height;
    this._inZoneL = 0;
    this._inZoneT = 0;
    this._translateX = 0;
    this._translateY = 0;

    this._flagStep = 0;
    for(int i = 0; i < this._subW; i++) {
      for(int j = 0; j < this._subH; j++) {
        this._flag[i,j] = 0;
      }
    }
    this._flagStep = 1;
  }
  //-----
  private void PreEndSubBuffer() {
    this._translateX = 0;
    this._translateY = 0;

    if(_boundTopLeft.x < 0f)_boundTopLeft.x = 0f;
    if(_boundTopLeft.y < 0f)_boundTopLeft.y = 0f;
    if(_boundBottomRight.x >= this._width)_boundBottomRight.x = this._width - 1f;
    if(_boundBottomRight.y >= this._height)_boundBottomRight.y = this._height - 1f;

    this._subW = (int)Math.Abs((int)_boundTopLeft.x -(int)_boundBottomRight.x)+ 1 +(2*1);
    this._subH = (int)Math.Abs((int)_boundTopLeft.y -(int)_boundBottomRight.y)+ 1 +(2*1);

    this._inZoneL = (int)_boundTopLeft.x -1;
    this._inZoneT = (int)_boundTopLeft.y -1;

    this._inZoneL = (_inZoneL < 0) ? 0 : _inZoneL;
    this._inZoneT = (_inZoneT < 0) ? 0 : _inZoneT;

    this._inZoneL = (_inZoneL >= this._width) ? (this._width -1) : _inZoneL;
    this._inZoneT = (_inZoneT >= this._height) ? (this._height -1) : _inZoneT;

    this._subW = (this._subW + this._inZoneL >= this._width) ?
           (this._width - this._inZoneL -1 ) : _subW;
    this._subH = (this._subH + this._inZoneT >= this._height) ?
           (this._height - this._inZoneT -1 ) : _subH;

    //Fill
    Fill(this._inZoneL, this._inZoneT);
    if((this._inZoneL == 0)&&(this._inZoneT == 0)) {
      Fill(this._inZoneL + this._subW - 1, this._inZoneT + this._subH - 1);
    }
  }
  //-----
  //Fill Solid color, No fill Stroke
  public void EndSubBuffer() {

    PreEndSubBuffer();

    Fill(this._inZoneL, this._inZoneT);
    for(int i = this._inZoneL; i < this._subW + this._inZoneL; i++) {
      for(int j = this._inZoneT; j < this._subH + this._inZoneT; j++) {
        if(this._flag[i, j] != -1 ) {
          this._graphics.SetPixel(i, j);
        }
      }
    }
  }
  //-----
  //Fill Solid color, No fill Stroke
  public void EndSubBuffer(uSVGPoint[] points) {

    PreEndSubBuffer();

    for(int i = 0; i < points.GetLength(0); i++) {
      Fill(points[i].x, points[i].y);
    }
    for(int i = this._inZoneL; i < this._subW + this._inZoneL; i++) {
      for(int j = this._inZoneT; j < this._subH + this._inZoneT; j++) {
        if(this._flag[i, j] != -1 ) {
          this._graphics.SetPixel(i, j);
        }
      }
    }
  }
  //-----
  //Fill Solid color, with fill Stroke
  public void EndSubBuffer(uSVGColor? strokePathColor) {

    PreEndSubBuffer();

    for(int i = this._inZoneL; i < this._subW + this._inZoneL; i++) {
      for(int j = this._inZoneT; j < this._subH + this._inZoneT; j++) {
        if(this._flag[i, j] == 0 ) {
          this._graphics.SetPixel(i, j);
        }
      }
    }

    this._graphics.SetColor(strokePathColor.Value.color);

    for(int i = this._inZoneL; i < this._subW + this._inZoneL; i++) {
      for(int j = this._inZoneT; j < this._subH + this._inZoneT; j++) {
        if(this._flag[i, j] > 0 ) {
          this._graphics.SetPixel(i, j);
        }
      }
    }
  }
  //-----
  //Fill Linear Gradient, no fill Stroke
  public void EndSubBuffer(uSVGLinearGradientBrush linearGradientBrush) {

    PreEndSubBuffer();
    for(int i = this._inZoneL; i < this._subW + this._inZoneL; i++) {
      for(int j = this._inZoneT; j < this._subH + this._inZoneT; j++) {
        if(this._flag[i, j] == 0 ) {
          Color _color = linearGradientBrush.GetColor(i, j);
          this._graphics.SetColor(_color);
          this._graphics.SetPixel(i, j);
        }
      }
    }
  }
  //-----
  //Fill Linear Gradient, with fill Stroke
  public void EndSubBuffer(uSVGLinearGradientBrush linearGradientBrush,
                      uSVGColor? strokePathColor) {
    PreEndSubBuffer();

    for(int i = this._inZoneL; i < this._subW + this._inZoneL; i++) {
      for(int j = this._inZoneT; j < this._subH + this._inZoneT; j++) {
        if(this._flag[i, j] != -1 ) {
          Color _color = linearGradientBrush.GetColor(i, j);
          this._graphics.SetColor(_color);
          this._graphics.SetPixel(i, j);
        }
      }
    }

    this._graphics.SetColor(strokePathColor.Value.color);

    for(int i = this._inZoneL; i < this._subW + this._inZoneL; i++) {
      for(int j = this._inZoneT; j < this._subH + this._inZoneT; j++) {
        if(this._flag[i, j] > 0 ) {
          this._graphics.SetPixel(i, j);
        }
      }
    }
  }
  //-----
  //Fill Radial Gradient, no fill Stroke
  public void EndSubBuffer(uSVGRadialGradientBrush radialGradientBrush) {

    PreEndSubBuffer();

    for(int i = this._inZoneL; i < this._subW + this._inZoneL; i++) {
      for(int j = this._inZoneT; j < this._subH + this._inZoneT; j++) {
        if(this._flag[i, j] == 0 ) {
          Color _color = radialGradientBrush.GetColor(i, j);
          this._graphics.SetColor(_color);
          this._graphics.SetPixel(i, j);
        }
      }
    }
  }
  //-----
  //Fill Radial Gradient, with fill Stroke
  public void EndSubBuffer(uSVGRadialGradientBrush radialGradientBrush,
                      uSVGColor? strokePathColor) {
    PreEndSubBuffer();

    for(int i = this._inZoneL; i < this._subW + this._inZoneL; i++) {
      for(int j = this._inZoneT; j < this._subH + this._inZoneT; j++) {
        if(this._flag[i, j] != -1 ) {
          Color _color = radialGradientBrush.GetColor(i, j);
          this._graphics.SetColor(_color);
          this._graphics.SetPixel(i, j);
        }
      }
    }

    this._graphics.SetColor(strokePathColor.Value.color);

    for(int i = this._inZoneL; i < this._subW + this._inZoneL; i++) {
      for(int j = this._inZoneT; j < this._subH + this._inZoneT; j++) {
        if(this._flag[i, j] > 0 ) {
          this._graphics.SetPixel(i, j);
        }
      }
    }
  }
  //--------------------------------------------------------------------------------
  //Method: Rect
  //--------------------------------------------------------------------------------
  private void PreRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4 ) {
    uSVGPoint[] points = new uSVGPoint[4];
    points[0] = p1;  points[1] = p2; points[2] = p3;  points[3] = p4;

    BeginSubBuffer();
    ResetLimitPoints(points);

    this._basicDraw.MoveTo(p1);
    this._basicDraw.LineTo(p2);
    this._basicDraw.LineTo(p3);
    this._basicDraw.LineTo(p4);
    this._basicDraw.LineTo(p1);
  }
  //-----
  public void Rect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4) {
    PreRect(p1, p2, p3, p4);
    EndSubBuffer();
  }
  //-----
  public void Rect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4,
                          uSVGColor? strokeColor) {
    PreRect(p1, p2, p3, p4);
    EndSubBuffer(strokeColor);
  }
  //-----
  public void Rect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4,
                          uSVGColor fillColor, uSVGColor? strokeColor) {
    SetColor(fillColor.color);
    PreRect(p1, p2, p3, p4);
    EndSubBuffer(strokeColor);
  }
  //--------------------------------------------------------------------------------
  //Method: RoundedRect
  //--------------------------------------------------------------------------------
  private void PreRoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4,
        uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
        float r1, float r2, float angle) {
    uSVGPoint[] points = new uSVGPoint[8];
    points[0] = p1;  points[1] = p2; points[2] = p3;  points[3] = p4;
    points[4] = p5;  points[5] = p6; points[6] = p7;  points[7] = p8;

    BeginSubBuffer();
    ResetLimitPoints(points, ((r1 > r2) ? (int)r1: (int)r2), ((r1 > r2) ? (int)r1: (int)r2));

    this._basicDraw.MoveTo(p1);this._basicDraw.LineTo(p2);
    this._basicDraw.ArcTo(r1, r2, angle, false, true, p3);

    this._basicDraw.MoveTo(p3);this._basicDraw.LineTo(p4);
    this._basicDraw.ArcTo(r1, r2, angle, false, true, p5);

    this._basicDraw.MoveTo(p5);this._basicDraw.LineTo(p6);
    this._basicDraw.ArcTo(r1, r2, angle, false, true, p7);

    this._basicDraw.MoveTo(p7);this._basicDraw.LineTo(p8);
    this._basicDraw.ArcTo(r1, r2, angle, false, true, p1);


  }
  //-----
  public void RoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4,
        uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
        float r1, float r2, float angle) {
    PreRoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle);
    EndSubBuffer();
  }
  //-----
  public void RoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4,
        uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
        float r1, float r2, float angle,
        uSVGColor? strokeColor) {
    PreRoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle);
    EndSubBuffer(strokeColor);
  }
  //-----
  public void RoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4,
        uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
        float r1, float r2, float angle,
        uSVGColor fillColor, uSVGColor? strokeColor) {
    SetColor(fillColor.color);
    PreRoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle);
    EndSubBuffer(strokeColor);
  }
  //--------------------------------------------------------------------------------
  //Method: CircleFill
  //--------------------------------------------------------------------------------
  private void PreCircle(uSVGPoint p, float r) {
    uSVGPoint[] points = new uSVGPoint[1];
    points[0] = p;
    BeginSubBuffer();
    ResetLimitPoints(points, (int)r+2, (int)r+2);

    this._basicDraw.Circle(p, r);
  }
  //-----
  public void Circle(uSVGPoint p, float r) {
    PreCircle(p, r);
    EndSubBuffer();
  }
  //-----
  public void Circle(uSVGPoint p, float r, uSVGColor? strokeColor) {
    PreCircle(p, r);
    EndSubBuffer(strokeColor);
  }
  //-----
  public void Circle(uSVGPoint p, float r, uSVGColor fillColor, uSVGColor? strokeColor) {
    SetColor(fillColor.color);
    PreCircle(p, r);
    EndSubBuffer();
  }
  //--------------------------------------------------------------------------------
  //Method: Ellipse
  //--------------------------------------------------------------------------------
  private void PreEllipse(uSVGPoint p, float rx, float ry, float angle) {
    uSVGPoint[] points = new uSVGPoint[1];
    points[0] = p;
    BeginSubBuffer();
    ResetLimitPoints(points, ((rx > ry) ? (int)rx: (int)ry), ((rx > ry) ? (int)rx: (int)ry));

    this._basicDraw.Ellipse(p, (int)rx, (int)ry, angle);
  }
  //-----
  public void Ellipse(uSVGPoint p, float rx, float ry, float angle) {
    PreEllipse(p, rx, ry, angle);
    EndSubBuffer();
  }
  //-----
  public void Ellipse(uSVGPoint p, float rx, float ry, float angle, uSVGColor? strokeColor) {
    PreEllipse(p, rx, ry, angle);
    EndSubBuffer(strokeColor);
  }
  //-----
  public void Ellipse(uSVGPoint p, float rx, float ry, float angle,
                          uSVGColor fillColor, uSVGColor? strokeColor) {
    SetColor(fillColor.color);
    PreEllipse(p, rx, ry, angle);
    EndSubBuffer(strokeColor);
  }
  //--------------------------------------------------------------------------------
  //Method: Polygon.
  //--------------------------------------------------------------------------------
  private void PrePolygon(uSVGPoint[] points) {
    if((points != null)&&(points.GetLength(0) > 0)) {
      BeginSubBuffer();
      ResetLimitPoints(points);

      this._basicDraw.MoveTo(points[0]);
      int _length = points.GetLength(0);
      for(int i = 1; i < _length; i++) {
        this._basicDraw.LineTo(points[i]);
      }
      this._basicDraw.LineTo(points[0]);
    }
  }
  //-----
  public void Polygon(uSVGPoint[] points) {
    PrePolygon(points);
    EndSubBuffer();
  }
  //-----
  public void Polygon(uSVGPoint[] points, uSVGColor? strokeColor) {
    PrePolygon(points);
    EndSubBuffer(strokeColor);
  }
  //-----
  public void Polygon(uSVGPoint[] points, uSVGColor fillColor, uSVGColor? strokeColor) {
    SetColor(fillColor.color);
    PrePolygon(points);
    EndSubBuffer(strokeColor);
  }
  //--------------------------------------------------------------------------------
  //Method: Polyline.
  //--------------------------------------------------------------------------------
  public void Polyline(uSVGPoint[] points) {
    Polygon(points);
  }
  //-----
  public void Polyline(uSVGPoint[] points, uSVGColor? strokeColor) {
    Polygon(points, strokeColor);
  }
  //-----
  public void Polyline(uSVGPoint[] points, uSVGColor fillColor, uSVGColor? strokeColor) {
    Polygon(points, fillColor, strokeColor);
  }

  //--------------------------------------------------------------------------------
  //Method: Fill Path
  //--------------------------------------------------------------------------------
  public void FillPath(uSVGGraphicsPath graphicsPath) {
    BeginSubBuffer();
    graphicsPath.RenderPath(this, true);
    EndSubBuffer();
  }
  //-----
  public void FillPath(uSVGGraphicsPath graphicsPath, uSVGPoint[] points) {
    BeginSubBuffer();
    graphicsPath.RenderPath(this, true);
    EndSubBuffer(points);
  }
  //-----
  //Path Solid Fill
  public void FillPath(uSVGColor fillColor, uSVGGraphicsPath graphicsPath) {
    SetColor(fillColor.color);
    FillPath(graphicsPath);
  }
  //-----
  public void FillPath(uSVGColor fillColor, uSVGColor? strokePathColor,
                                uSVGGraphicsPath graphicsPath) {
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
  public void FillPath(uSVGLinearGradientBrush linearGradientBrush, uSVGGraphicsPath graphicsPath) {
    BeginSubBuffer();
    graphicsPath.RenderPath(this, true);
    EndSubBuffer(linearGradientBrush);
  }
  //-----
  public void FillPath(uSVGLinearGradientBrush linearGradientBrush,
                                uSVGColor? strokePathColor,
                                uSVGGraphicsPath graphicsPath) {
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
  public void FillPath(uSVGRadialGradientBrush radialGradientBrush, uSVGGraphicsPath graphicsPath) {
    BeginSubBuffer();
    graphicsPath.RenderPath(this, true);
    EndSubBuffer(radialGradientBrush);
  }
  //-----
  public void FillPath(uSVGRadialGradientBrush radialGradientBrush,
                                uSVGColor? strokePathColor,
                                uSVGGraphicsPath graphicsPath) {
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
  public void CircleTo(uSVGPoint p, float r) {
    uSVGPoint[] points = new uSVGPoint[1];
    points[0] = p;
    ResetLimitPoints(points, (int)r+1, (int)r+1);

    //---------------
    this._basicDraw.Circle(p, r);
  }
  //--------------------------------------------------------------------------------
  //Method: EllipseTo
  //--------------------------------------------------------------------------------
  public void EllipseTo(uSVGPoint p, float rx, float ry, float angle) {
    uSVGPoint[] points = new uSVGPoint[1];
    points[0] = p;
    ResetLimitPoints(points, ((rx > ry) ? (int)rx+2: (int)ry+2), ((rx > ry) ? (int)rx+2: (int)ry+2));

    //---------------
    this._basicDraw.Ellipse(p, (int)rx, (int)ry, angle);
  }
  //--------------------------------------------------------------------------------
  //Method: LineTo4Path
  //--------------------------------------------------------------------------------
  public void LineTo(uSVGPoint p) {
    uSVGPoint[] points = new uSVGPoint[1];
    points[0] = p;
    ResetLimitPoints(points);
    //---------------
    this._basicDraw.LineTo(p);
  }
  //--------------------------------------------------------------------------------
  //Method: MoveTo
  //--------------------------------------------------------------------------------
  public void MoveTo(uSVGPoint p) {
    uSVGPoint[] points = new uSVGPoint[1];
    points[0] = p;
    ResetLimitPoints(points);
    //---------------
    this._basicDraw.MoveTo(p);
  }

  /*-------------------------------------------------------------------------------
  //Method: Arc4Path
  /-------------------------------------------------------------------------------*/
  public void ArcTo(float r1, float r2, float angle,
        bool largeArcFlag, bool sweepFlag,
        uSVGPoint p) {
    uSVGPoint[] points = new uSVGPoint[1];
    points[0] = p;
    ResetLimitPoints(points,
         (r1>r2) ?2*(int)r1+2:2*(int)r2+2,
         (r1>r2) ?2*(int)r1+2:2*(int)r2+2);
    //---------------
    this._basicDraw.ArcTo(r1, r2, angle, largeArcFlag, sweepFlag, p);
  }
  /*-------------------------------------------------------------------------------
  //Method: CubicCurveTo4Path
  /-------------------------------------------------------------------------------*/
  public void CubicCurveTo(uSVGPoint p1, uSVGPoint p2, uSVGPoint p) {
    uSVGPoint[] points = new uSVGPoint[3];
    points[0] = p1;
    points[1] = p2;
    points[2] = p;
    ResetLimitPoints(points);
    //---------------
    this._basicDraw.CubicCurveTo(p1, p2, p);
  }

  /*-------------------------------------------------------------------------------
  //Method: QuadraticCurveTo4Path
  /-------------------------------------------------------------------------------*/
  public void QuadraticCurveTo(uSVGPoint p1, uSVGPoint p) {
    uSVGPoint[] points = new uSVGPoint[2];
    points[0] = p1;
    points[1] = p;
    ResetLimitPoints(points);
    //---------------
    this._basicDraw.QuadraticCurveTo(p1, p);
  }
}
