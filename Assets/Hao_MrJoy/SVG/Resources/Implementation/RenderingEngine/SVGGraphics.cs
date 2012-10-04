using UnityEngine;
using System;
using System.Collections.Generic;

public class SVGGraphics {
  //================================================================================
  private SVGDevice _device;

  private SVGGraphicsFill _graphicsFill;
  private SVGGraphicsStroke _graphicsStroke;

  private int _width, _height;

  private SVGStrokeLineCapMethod   _strokeLineCap    = SVGStrokeLineCapMethod.Unknown;
  private SVGStrokeLineJoinMethod  _strokeLineJoin  = SVGStrokeLineJoinMethod.Unknown;
  //================================================================================
  public SVGStrokeLineCapMethod strokeLineCap {
    get { return this._strokeLineCap; }
  }
  //-----
  public SVGStrokeLineJoinMethod strokeLineJoin {
    get { return this._strokeLineJoin; }
  }
  //================================================================================
  public SVGGraphics() {
    this._graphicsFill = new SVGGraphicsFill(this);
    this._graphicsStroke = new SVGGraphicsStroke(this);
  }
  //-----
  public SVGGraphics(SVGDevice device) {
    this._device = device;
    this._graphicsFill = new SVGGraphicsFill(this);
    this._graphicsStroke = new SVGGraphicsStroke(this);
  }
  //================================================================================
  //--------------------------------------------------------------------------------
  //Method: SetSize
  //--------------------------------------------------------------------------------
  public void SetSize(float width, float height) {
    _width = (int)width;
    _height = (int)height;
    this._device.SetDevice(_width, _height);
    //this._device.GetBufferSize(ref _width, ref _height);
    this._graphicsFill.SetSize((int)width, (int)height);
    Clean();
  }
  //--------------------------------------------------------------------------------
  //Method: SetColor
  //--------------------------------------------------------------------------------
  public void SetColor(Color color) {
    this._device.SetColor(color);
  }

  //--------------------------------------------------------------------------------
  //Method: SetPixel
  //--------------------------------------------------------------------------------
  public void SetPixel(int x, int y) {
    this._device.SetPixel(x, y);
  }
  //--------------------------------------------------------------------------------
  //Method: SetStrokeLineCap
  //--------------------------------------------------------------------------------
  public void SetStrokeLineCap(SVGStrokeLineCapMethod strokeLineCap) {
    this._strokeLineCap = strokeLineCap;
  }
  //--------------------------------------------------------------------------------
  //Method: SetStrokeLineJoin
  //--------------------------------------------------------------------------------
  public void SetStrokeLineJoin(SVGStrokeLineJoinMethod strokeLineJoin) {
    this._strokeLineJoin = strokeLineJoin;
  }
  //--------------------------------------------------------------------------------
  //Method: Render
  //--------------------------------------------------------------------------------
  public Texture2D Render() {
    this._device.SetDevice(_width, _height);
    return this._device.Render();
  }
  //--------------------------------------------------------------------------------
  //Method: Clean
  //--------------------------------------------------------------------------------
  public void Clean() {
    int width=0, height=0;
    SetColor(Color.white);
    _device.GetBufferSize(ref width, ref height);
    for(int i=0; i<width; i++) {
      for(int j=0; j<height; j++) {
        SetPixel(i, j);
      }
    }
  }
  //--------------------------------------------------------------------------------
  //GetThickLine
  //--------------------------------------------------------------------------------
  //Tinh 4 diem 1, 2, 3, 4 cua 1 line voi width
  public bool GetThickLine(Vector2 p1, Vector2 p2, float width,
            ref Vector2 rp1, ref Vector2 rp2, ref Vector2 rp3, ref Vector2 rp4) {

    float cx1, cy1, cx2, cy2, cx3, cy3, cx4, cy4;
    float dtx, dty, temp, _half;
    int _ihalf1, _ihalf2;

    _half = width / 2f;
    _ihalf1 = (int)_half;
    _ihalf2 = (int)(width - _ihalf1 + 0.5f);

    dtx = p2.x - p1.x;
    dty = p2.y - p1.y;
    temp = dtx * dtx + dty * dty;
    if(temp == 0f) {
      rp1.x = p1.x - _ihalf2;
      rp1.y = p1.y + _ihalf2;

      rp2.x = p1.x - _ihalf2;
      rp2.y = p1.y - _ihalf2;

      rp3.x = p1.x + _ihalf1;
      rp3.y = p1.y + _ihalf1;

      rp4.x = p1.x + _ihalf1;
      rp4.y = p1.y - _ihalf1;
      return false;
    }

    cy1 = _ihalf1 * dtx /(float)Math.Sqrt(temp)+ p1.y;
    if(dtx == 0) {
      if(dty > 0) {
        cx1 = p1.x - _ihalf1;
      } else {
        cx1 = p1.x + _ihalf1;
      }
    } else {
      cx1 = (-(cy1 - p1.y)* dty)/ dtx + p1.x;
    }

    cy2 = -(_ihalf2 * dtx /(float)Math.Sqrt(temp))+ p1.y;
    if(dtx == 0) {
      if(dty > 0) {
        cx2 = p1.x + _ihalf2;
      } else {
        cx2 = p1.x - _ihalf2;
      }
    } else {
      cx2 = (-(cy2 - p1.y)* dty)/ dtx + p1.x;
    }

    dtx = p1.x - p2.x;
    dty = p1.y - p2.y;
    temp = dtx * dtx + dty * dty;

    cy3 = _ihalf1 * dtx /(float)Math.Sqrt(temp)+ p2.y;
    if(dtx == 0) {
      if(dty > 0) {
        cx3 = p2.x - _ihalf1;
      } else {
        cx3 = p2.x + _ihalf1;
      }
    } else {
      cx3 = (-(cy3 - p2.y)* dty)/ dtx + p2.x;
    }

    cy4 = -(_ihalf2 * dtx /(float)Math.Sqrt(temp))+ p2.y;

    if(dtx == 0) {
      if(dty > 0) {
        cx4 = p2.x + _ihalf2;
      } else {
        cx4 = p2.x - _ihalf2;
      }
    } else {
      cx4 = (-(cy4 - p2.y)* dty)/ dtx + p2.x;
    }

    rp1.x = cx1; rp1.y = cy1;

    rp2.x = cx2; rp2.y = cy2;

    float t1,t2;
    t1 = ((p1.y - cy1)*(p2.x - p1.x))-((p1.x - cx1)*(p2.y - p1.y));
    t2 = ((p1.y - cy4)*(p2.x - p1.x))-((p1.x - cx4)*(p2.y - p1.y));
    if(t1 * t2 > 0) {
      //bi lech
      if(_ihalf1 != _ihalf2) {
        cy3 = _ihalf2 * dtx /(float)Math.Sqrt(temp)+ p2.y;
        if(dtx == 0) {
          if(dty > 0) {
            cx3 = p2.x - _ihalf2;
          } else {
            cx3 = p2.x + _ihalf2;
          }
        } else {
          cx3 = (-(cy3 - p2.y)* dty)/ dtx + p2.x;
        }

        cy4 = -(_ihalf1 * dtx /(float)Math.Sqrt(temp))+ p2.y;

        if(dtx == 0) {
          if(dty > 0) {
            cx4 = p2.x + _ihalf1;
          } else {
            cx4 = p2.x - _ihalf1;
          }
        } else {
          cx4 = (-(cy4 - p2.y)* dty)/ dtx + p2.x;
        }
      }

      rp3.x = cx4; rp3.y = cy4;
      rp4.x = cx3; rp4.y = cy3;
    } else {
      rp3.x = cx3; rp3.y = cy3;
      rp4.x = cx4; rp4.y = cy4;
    }
    return true;
  }
  //--------------------------------------------------------------------------------
  //GetCrossPoint
  //--------------------------------------------------------------------------------
  //Tinh diem giao nhau giua 2 doan thang
  public Vector2 GetCrossPoint(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {

    Vector2 _return = new Vector2(0f, 0f);
    float a1 = 0f, b1 = 0f, a2 = 0f, b2 = 0f;

    float dx1, dy1, dx2, dy2;
    dx1 = p1.x - p2.x;
    dy1 = p1.y - p2.y;
    dx2 = p3.x - p4.x;
    dy2 = p3.y - p4.y;

    if(dx1 != 0f) {
      a1 = dy1 / dx1;
      b1 = p1.y - a1 * p1.x;
    }

    if(dx2 != 0) {
      a2 = dy2 / dx2;
      b2 = p3.y - a2 * p3.x;
    }
    //-----
    float tx = 0f, ty = 0f;

    //truong hop nam tren duong thang
    if((a1 == a2)&&(b1 == b2)) {
      Vector2 t_p1 = p1;
      Vector2 t_p2 = p1;
      if(dx1 == 0f) {
        if(p2.y < t_p1.y)t_p1=p2;
        if(p3.y < t_p1.y)t_p1=p3;
        if(p4.y < t_p1.y)t_p1=p4;

        if(p2.y > t_p2.y)t_p2=p2;
        if(p3.y > t_p2.y)t_p2=p3;
        if(p4.y > t_p2.y)t_p2=p4;
      } else {
        if(p2.x < t_p1.x)t_p1=p2;
        if(p3.x < t_p1.x)t_p1=p3;
        if(p4.x < t_p1.x)t_p1=p4;

        if(p2.x > t_p2.x)t_p2=p2;
        if(p3.x > t_p2.x)t_p2=p3;
        if(p4.x > t_p2.x)t_p2=p4;
      }

      tx = (t_p1.x  - t_p2.x)/2f;
      tx = t_p2.x + tx;

      ty = (t_p1.y  - t_p2.y)/2f;
      ty = t_p2.y + ty;

      _return.x = tx;
      _return.y = ty;
      return _return;
    }



    if((dx1 != 0)&&(dx2 != 0)) {
      tx = -(b1 - b2)/(a1 - a2);
      ty = a1 * tx + b1;
    } else if((dx1 == 0)&&(dx2 != 0)) {
      tx = p1.x;
      ty = a2 * tx + b2;
    } else if((dx1 != 0)&&(dx2 == 0)) {
      tx = p3.x;
      ty = a1 * tx + b1;
    }

    _return.x = tx;
    _return.y = ty;
    return _return;
  }
  //--------------------------------------------------------------------------------
  //AngleBetween2Vector
  //--------------------------------------------------------------------------------
  //Tinh goc giua 2 vector (p1,p2) (p3,p4);
  public float AngleBetween2Vector(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
    Vector2 vt1, vt2;
    vt1 = new Vector2(p2.x - p1.x, p2.y - p1.y);
    vt2 = new Vector2(p4.x - p3.x, p4.y - p3.y);
    float t1 = vt1.x*vt2.x + vt1.y*vt2.y;
    float gtvt1 = (float)Math.Sqrt(vt1.x * vt1.x + vt1.y*vt1.y);
    float gtvt2 = (float)Math.Sqrt(vt2.x * vt2.x + vt2.y*vt2.y);
    float t2 = gtvt1 * gtvt2;
    float cosAngle = t1/t2;

    return((float)Math.Acos(cosAngle));
  }
  //================================================================================
  //--------------------------------------------------------------------------------
  //Method: Line
  //--------------------------------------------------------------------------------
  public void Line(Vector2 p1, Vector2 p2) {
    this._graphicsStroke.Line(p1, p2);
  }
  //-----
  public void Line(Vector2 p1, Vector2 p2, SVGColor? strokeColor) {
    if(strokeColor != null) {
      SetColor(strokeColor.Value.color);
    }
    Line(p1, p2);
  }
  //-----
  public void Line(Vector2 p1, Vector2 p2, float width) {
    this._graphicsStroke.Line(p1, p2, width);
  }
  //-----
  public void Line(Vector2 p1, Vector2 p2, SVGColor? strokeColor, float width) {
    if(strokeColor != null) {
      SetColor(strokeColor.Value.color);
    }
    Line(p1, p2, width);
  }
  //--------------------------------------------------------------------------------
  //Method: Rect
  //--------------------------------------------------------------------------------
  public void Rect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
    this._graphicsStroke.Rect(p1, p2, p3, p4);
  }
  //-----
  public void Rect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, SVGColor? strokeColor) {
    if(strokeColor != null) {
      SetColor(strokeColor.Value.color);
    }
    Rect(p1, p2, p3, p4);
  }
  //-----
  public void Rect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, float width) {
    this._graphicsStroke.Rect(p1, p2, p3, p4, width);
  }
  //-----
  public void Rect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
                             SVGColor? strokeColor, float width) {
    if(strokeColor != null) {
      SetColor(strokeColor.Value.color);
    }
    Rect(p1, p2, p3, p4, width);
  }
  //--------------------------------------------------------------------------------
  //Method: Rounded Rect
  //--------------------------------------------------------------------------------
  public void RoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
      Vector2 p5, Vector2 p6, Vector2 p7, Vector2 p8,
      float r1, float r2, float angle) {

    this._graphicsStroke.RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle);
  }
  //-----
  public void RoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
      Vector2 p5, Vector2 p6, Vector2 p7, Vector2 p8,
      float r1, float r2, float angle, SVGColor? strokeColor) {

    if(strokeColor != null) {
      SetColor(strokeColor.Value.color);
    }
    RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle);
  }
  //-----
  public void RoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
      Vector2 p5, Vector2 p6, Vector2 p7, Vector2 p8,
      float r1, float r2, float angle, float width) {

    if((int)width == 1) {
      RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle);
      return;
    }
    this._graphicsStroke.RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle, width);
  }
  //-----
  public void RoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
      Vector2 p5, Vector2 p6, Vector2 p7, Vector2 p8,
      float r1, float r2, float angle, SVGColor? strokeColor, float width) {

    if(strokeColor != null) {
      SetColor(strokeColor.Value.color);
    }
    RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle, width);
  }
  //--------------------------------------------------------------------------------
  //Method: FillRect
  //--------------------------------------------------------------------------------
  public void FillRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
    this._graphicsFill.Rect(p1, p2, p3, p4);
  }
  //-----
  public void FillRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
                          SVGColor? strokeColor) {
    this._graphicsFill.Rect(p1, p2, p3, p4, strokeColor);
  }
  //-----
  public void FillRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
                          SVGColor fillColor, SVGColor? strokeColor) {
    this._graphicsFill.Rect(p1, p2, p3, p4, fillColor, strokeColor);
  }
  //-----
  public void FillRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
                        SVGColor? strokeColor, float width) {
    if((int)width == 1) {
      FillRect(p1, p2, p3, p4, strokeColor);
      return;
    }
    FillRect(p1, p2, p3, p4);
    if(strokeColor == null)return;
    SetColor(strokeColor.Value.color);
    Rect(p1, p2, p3, p4, strokeColor, width);
  }
  //-----
  public void FillRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
                      SVGColor fillColor, SVGColor? strokeColor, float width) {
    if((int)width == 1) {
      FillRect(p1, p2, p3, p4, fillColor, strokeColor);
      return;
    }
    SetColor(fillColor.color);
    FillRect(p1, p2, p3, p4);
    if(strokeColor == null)return;
    SetColor(strokeColor.Value.color);
    Rect(p1, p2, p3, p4, strokeColor, width);
  }
  //--------------------------------------------------------------------------------
  //Method: FillRoundedRect
  //--------------------------------------------------------------------------------
  public void FillRoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
        Vector2 p5, Vector2 p6, Vector2 p7, Vector2 p8,
        float r1, float r2, float angle) {
    this._graphicsFill.RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle);
  }
  //-----
  public void FillRoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
        Vector2 p5, Vector2 p6, Vector2 p7, Vector2 p8,
        float r1, float r2, float angle,
        SVGColor? strokeColor) {
    this._graphicsFill.RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle, strokeColor);
  }
  //-----
  public void FillRoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
        Vector2 p5, Vector2 p6, Vector2 p7, Vector2 p8,
        float r1, float r2, float angle,
        SVGColor fillColor, SVGColor? strokeColor) {
    this._graphicsFill.RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle,
                                      fillColor, strokeColor);
  }
  //-----
  public void FillRoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
        Vector2 p5, Vector2 p6, Vector2 p7, Vector2 p8,
        float r1, float r2, float angle,
        SVGColor? strokeColor, float width) {
    if((int)width == 1) {
      FillRoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle, strokeColor);
      return;
    }
    this._graphicsFill.RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle);
    if(strokeColor == null)return;
    SetColor(strokeColor.Value.color);
    RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle, strokeColor);
  }
  //-----
  public void FillRoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
        Vector2 p5, Vector2 p6, Vector2 p7, Vector2 p8,
        float r1, float r2, float angle,
        SVGColor fillColor, SVGColor? strokeColor, float width) {

    if((int)width == 1) {
      FillRoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle, strokeColor);
      return;
    }
    SetColor(fillColor.color);
    this._graphicsFill.RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle);
    if(strokeColor == null)return;
    SetColor(strokeColor.Value.color);
    RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle, strokeColor);
  }

  //--------------------------------------------------------------------------------
  //Method: Circle
  //--------------------------------------------------------------------------------
  public void Circle(Vector2 p, float r) {
    this._graphicsStroke.Circle(p, r);
  }
  //-----
  public void Circle(Vector2 p, float r, SVGColor? strokeColor) {
    if(strokeColor != null) {
      SetColor(strokeColor.Value.color);
    }
    Circle(p, r);
  }
  //-----
  public void Circle(Vector2 p, float r, float width) {
    this._graphicsStroke.Circle(p, r, width);
  }
  //-----
  public void Circle(Vector2 p, float r,
                  SVGColor? strokeColor, float width) {
    if(strokeColor != null) {
      SetColor(strokeColor.Value.color);
    }
    Circle(p, r, width);
  }
  //--------------------------------------------------------------------------------
  //Method: FillCircle
  //--------------------------------------------------------------------------------
  public void FillCircle(Vector2 p, float r) {
    this._graphicsFill.Circle(p, r);
  }
  //-----
  public void FillCircle(Vector2 p, float r, SVGColor? strokeColor) {
    this._graphicsFill.Circle(p, r, strokeColor);
  }
  //-----
  public void FillCircle(Vector2 p, float r, SVGColor fillColor, SVGColor? strokeColor) {
    this._graphicsFill.Circle(p, r, fillColor, strokeColor);
  }
  //-----
  public void FillCircle(Vector2 p, float r,
              SVGColor? strokeColor, float width) {
    if((int)width == 1) {
      FillCircle(p, r, strokeColor);
      return;
    }

    FillCircle(p, r);
    if(strokeColor == null)return;
    SetColor(strokeColor.Value.color);
    Circle(p, r, strokeColor, width);
  }
  //-----
  public void FillCircle(Vector2 p, float r,
              SVGColor fillColor, SVGColor? strokeColor, float width) {
    if((int)width == 1) {
      FillCircle(p, r, strokeColor);
      return;
    }
    SetColor(fillColor.color);
    FillCircle(p, r);
    if(strokeColor == null)return;
    SetColor(strokeColor.Value.color);
    Circle(p, r, strokeColor, width);
  }
  //--------------------------------------------------------------------------------
  //Method: Ellipse
  //--------------------------------------------------------------------------------
  public void Ellipse(Vector2 p, float rx, float ry, float angle) {
    this._graphicsStroke.Ellipse(p, rx, ry, angle);
  }
  //-----
  public void Ellipse(Vector2 p, float rx, float ry, float angle, SVGColor? strokeColor) {
    if(strokeColor != null) {
      SetColor(strokeColor.Value.color);
    }
    Ellipse(p, rx, ry, angle);
  }
  //-----
  public void Ellipse(Vector2 p, float rx, float ry, float angle, float width) {
    this._graphicsStroke.Ellipse(p, rx, ry, angle, width);
  }
  //-----
  public void Ellipse(Vector2 p, float rx, float ry, float angle,
                            SVGColor? strokeColor, float width) {
    if(strokeColor != null) {
      SetColor(strokeColor.Value.color);
    }
    Ellipse(p, rx, ry, angle, width);
  }
  //--------------------------------------------------------------------------------
  //Method: FillEllipse
  //--------------------------------------------------------------------------------
  public void FillEllipse(Vector2 p, float rx, float ry, float angle) {
    this._graphicsFill.Ellipse(p, rx, ry, angle);
  }
  //-----
  public void FillEllipse(Vector2 p, float rx, float ry, float angle, SVGColor? strokeColor) {
    this._graphicsFill.Ellipse(p, rx, ry, angle, strokeColor);
  }
  //-----
  public void FillEllipse(Vector2 p, float rx, float ry, float angle,
                SVGColor fillColor, SVGColor? strokeColor) {
    this._graphicsFill.Ellipse(p, rx, ry, angle, fillColor, strokeColor);
  }
  //-----
  public void FillEllipse(Vector2 p, float rx, float ry, float angle,
                          SVGColor? strokeColor, float width) {
    if((int)width == 1) {
      FillEllipse(p, rx, ry, angle, strokeColor);
      return;
    }

    FillEllipse(p, rx, ry, angle);
    if(strokeColor == null)return;
    SetColor(strokeColor.Value.color);
    Ellipse(p, rx, ry, angle, width);

  }
  //-----
  public void FillEllipse(Vector2 p, float rx, float ry, float angle,
                SVGColor fillColor, SVGColor? strokeColor, float width) {
    if((int)width == 1) {
      FillEllipse(p, rx, ry, angle, strokeColor);
      return;
    }
    SetColor(fillColor.color);
    FillEllipse(p, rx, ry, angle);
    if(strokeColor == null)return;
    SetColor(strokeColor.Value.color);
    Ellipse(p, rx, ry, angle, width);

  }
  //--------------------------------------------------------------------------------
  //Method: Polygon
  //--------------------------------------------------------------------------------
  public void Polygon(Vector2[] points) {
    this._graphicsStroke.Polygon(points);
  }
  //-----
  public void Polygon(Vector2[] points, SVGColor? strokeColor) {
    if(strokeColor != null) {
      SetColor(strokeColor.Value.color);
    }
    Polygon(points);
  }
  //-----
  public void Polygon(Vector2[] points, float width) {
    this._graphicsStroke.Polygon(points, width);
  }
  //-----
  public void Polygon(Vector2[] points, SVGColor? strokeColor, float width) {
    if(strokeColor != null) {
      SetColor(strokeColor.Value.color);
    }
    Polygon(points, width);
  }
  //--------------------------------------------------------------------------------
  //Method: FillPolygon
  //--------------------------------------------------------------------------------
  public void FillPolygon(Vector2[] points) {
    this._graphicsFill.Polygon(points);
  }
  //-----
  public void FillPolygon(Vector2[] points, SVGColor? strokeColor) {
    this._graphicsFill.Polygon(points, strokeColor);
  }
  //-----
  public void FillPolygon(Vector2[] points, SVGColor fillColor, SVGColor? strokeColor) {
    this._graphicsFill.Polygon(points, fillColor, strokeColor);
  }
  //-----
  public void FillPolygon(Vector2[] points, SVGColor? strokeColor, float width) {
    if((int)width == 1) {
      FillPolygon(points, strokeColor);
      return;
    }
    FillPolygon(points);
    if(strokeColor == null)return;
    SetColor(strokeColor.Value.color);
    Polygon(points, width);
  }
  //-----
  public void FillPolygon(Vector2[] points,
            SVGColor fillColor, SVGColor? strokeColor, float width) {
    if((int)width == 1) {
      FillPolygon(points, strokeColor);
      return;
    }
    SetColor(fillColor.color);
    FillPolygon(points);
    if(strokeColor == null)return;
    SetColor(strokeColor.Value.color);
    Polygon(points, width);
  }

  //================================================================================
  //--------------------------------------------------------------------------------
  //Path Linear Gradient Fill
  //--------------------------------------------------------------------------------
  //Fill khong to Stroke
  public void FillPath(SVGLinearGradientBrush linearGradientBrush,
                                SVGGraphicsPath graphicsPath) {
    this._graphicsFill.FillPath(linearGradientBrush, graphicsPath);
  }
  //-----
  //Fill co Stroke trong do luon
  public void FillPath(SVGLinearGradientBrush linearGradientBrush,
                              SVGColor? strokePathColor,
                              SVGGraphicsPath graphicsPath) {
    this._graphicsFill.FillPath(linearGradientBrush, strokePathColor, graphicsPath);
  }
  //-----
  //Fill khong co Stroke, va ve stroke sau
  public void FillPath(SVGLinearGradientBrush linearGradientBrush,
                              SVGColor? strokePathColor,
                              float width,
                              SVGGraphicsPath graphicsPath) {

    this._graphicsFill.FillPath(linearGradientBrush, strokePathColor, graphicsPath);

    if((int)width == 1)
      this._graphicsFill.FillPath(linearGradientBrush, strokePathColor, graphicsPath);
    else this._graphicsFill.FillPath(linearGradientBrush, graphicsPath);

    if(strokePathColor == null)return;
    SetColor(strokePathColor.Value.color);
  }
  //--------------------------------------------------------------------------------
  //Path Radial Gradient Fill
  //--------------------------------------------------------------------------------
  //Fill khong to Stroke
  public void FillPath(SVGRadialGradientBrush radialGradientBrush,
                                SVGGraphicsPath graphicsPath) {
    this._graphicsFill.FillPath(radialGradientBrush, graphicsPath);
  }
  //-----
  //Fill co Stroke trong do luon
  public void FillPath(SVGRadialGradientBrush radialGradientBrush,
                              SVGColor? strokePathColor,
                              SVGGraphicsPath graphicsPath) {
    this._graphicsFill.FillPath(radialGradientBrush, strokePathColor, graphicsPath);
  }
  //-----
  //Fill khong co Stroke, va ve stroke sau
  public void FillPath(SVGRadialGradientBrush radialGradientBrush,
                              SVGColor? strokePathColor,
                              float width,
                              SVGGraphicsPath graphicsPath) {
    this._graphicsFill.FillPath(radialGradientBrush, strokePathColor, graphicsPath);
    if((int)width == 1)
      this._graphicsFill.FillPath(radialGradientBrush, strokePathColor, graphicsPath);
    else this._graphicsFill.FillPath(radialGradientBrush, graphicsPath);

    if(strokePathColor == null)return;
    SetColor(strokePathColor.Value.color);
    //graphicsPath.RenderPath(this, width, false);
  }
  //--------------------------------------------------------------------------------
  //Path Solid Fill
  //--------------------------------------------------------------------------------
  //Fill khong to Stroke
  public void FillPath( SVGGraphicsPath graphicsPath) {
    this._graphicsFill.FillPath(graphicsPath);
  }
  //Fill khong to Stroke
  public void FillPath(SVGColor fillColor, SVGGraphicsPath graphicsPath) {
    this._graphicsFill.FillPath(fillColor, graphicsPath);
  }
  //-----
  //Fill co Stroke trong do luon
  public void FillPath(SVGColor fillColor, SVGColor? strokePathColor,
                              SVGGraphicsPath graphicsPath) {
    this._graphicsFill.FillPath(fillColor, strokePathColor, graphicsPath);
  }
  //-----
  //Fill khong co Stroke, va ve stroke sau
  public void FillPath(SVGColor fillColor, SVGColor? strokePathColor,
                              float width,
                              SVGGraphicsPath graphicsPath) {
    this._graphicsFill.FillPath(fillColor, strokePathColor, graphicsPath);
    if((int)width == 1)this._graphicsFill.FillPath(fillColor, strokePathColor, graphicsPath);
    else this._graphicsFill.FillPath(fillColor, graphicsPath);

    if(strokePathColor == null)return;
    SetColor(strokePathColor.Value.color);
  }
  //-----
  public void FillPath( SVGGraphicsPath graphicsPath, Vector2[] points) {
    this._graphicsFill.FillPath(graphicsPath, points);
  }
  //-----
  public void FillPath( SVGGraphicsPath graphicsPath, Vector2 point) {
    this._graphicsFill.FillPath(graphicsPath, point);
  }

  //================================================================================
  //--------------------------------------------------------------------------------
  //Draw Path
  //--------------------------------------------------------------------------------
  //DrawPath
  public void DrawPath(SVGGraphicsPath graphicsPath) {
    this._graphicsStroke.DrawPath(graphicsPath);
  }
  //-----
  //Fill co Stroke trong do luon
  public void DrawPath(SVGGraphicsPath graphicsPath, float width) {
    this._graphicsStroke.DrawPath(graphicsPath, width);
  }
  //-----
  //Fill khong co Stroke, va ve stroke sau
  public void DrawPath(SVGGraphicsPath graphicsPath, float width, SVGColor? strokePathColor) {
    if(strokePathColor == null)return;
    SetColor(strokePathColor.Value.color);
    this._graphicsStroke.DrawPath(graphicsPath, width);
  }
}
