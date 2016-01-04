using UnityEngine;
using System;
using System.Collections.Generic;

// TODO: Find a way to break this the hell up.

// TODO: Normalize conventions away from Java-style SetX to properties.
public class SVGGraphics {
  private ISVGDevice device;

  private SVGGraphicsFill graphicsFill;
  private SVGGraphicsStroke graphicsStroke;

  public Vector2 Size {
    set {
      int width   = (int)value.x,
          height  = (int)value.y;
      device.SetDevice(width, height);
      graphicsFill.Size = new Vector2(width, height);
      Clean();
    }
  }

  public SVGStrokeLineCapMethod StrokeLineCap { get; set; }
  public SVGStrokeLineJoinMethod StrokeLineJoin { get; set; }

  public SVGGraphics() {
    graphicsFill = new SVGGraphicsFill(this);
    graphicsStroke = new SVGGraphicsStroke(this);
  }

  public SVGGraphics(ISVGDevice dev) {
    device = dev;
    graphicsFill = new SVGGraphicsFill(this);
    graphicsStroke = new SVGGraphicsStroke(this);
  }

  public void SetColor(Color color) {
    device.SetColor(color);
  }

  public void SetPixel(int x, int y) {
    device.SetPixel(x, y);
  }

  public Texture2D Render() {
    return device.Render();
  }

  public void Clean() {
    int width = device.Width;
    int height = device.Height;
    SetColor(Color.white);
    for(int i = 0; i < width; i++) {
      for(int j = 0; j < height; j++)
        SetPixel(i, j);
    }
  }

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

    cy1 = _ihalf1 * dtx / (float)Math.Sqrt(temp) + p1.y;
    if(dtx == 0) {
      if(dty > 0)
        cx1 = p1.x - _ihalf1;
      else
        cx1 = p1.x + _ihalf1;
    } else
      cx1 = (-(cy1 - p1.y) * dty) / dtx + p1.x;

    cy2 = -(_ihalf2 * dtx / (float)Math.Sqrt(temp)) + p1.y;
    if(dtx == 0) {
      if(dty > 0)
        cx2 = p1.x + _ihalf2;
      else
        cx2 = p1.x - _ihalf2;
    } else
      cx2 = (-(cy2 - p1.y) * dty) / dtx + p1.x;

    dtx = p1.x - p2.x;
    dty = p1.y - p2.y;
    temp = dtx * dtx + dty * dty;

    cy3 = _ihalf1 * dtx / (float)Math.Sqrt(temp) + p2.y;
    if(dtx == 0) {
      if(dty > 0)
        cx3 = p2.x - _ihalf1;
      else
        cx3 = p2.x + _ihalf1;
    } else
      cx3 = (-(cy3 - p2.y) * dty) / dtx + p2.x;

    cy4 = -(_ihalf2 * dtx / (float)Math.Sqrt(temp)) + p2.y;

    if(dtx == 0) {
      if(dty > 0)
        cx4 = p2.x + _ihalf2;
      else
        cx4 = p2.x - _ihalf2;
    } else
      cx4 = (-(cy4 - p2.y) * dty) / dtx + p2.x;

    rp1.x = cx1;
    rp1.y = cy1;

    rp2.x = cx2;
    rp2.y = cy2;

    float t1, t2;
    t1 = ((p1.y - cy1) * (p2.x - p1.x)) - ((p1.x - cx1) * (p2.y - p1.y));
    t2 = ((p1.y - cy4) * (p2.x - p1.x)) - ((p1.x - cx4) * (p2.y - p1.y));
    if(t1 * t2 > 0) {
      //bi lech
      if(_ihalf1 != _ihalf2) {
        cy3 = _ihalf2 * dtx / (float)Math.Sqrt(temp) + p2.y;
        if(dtx == 0) {
          if(dty > 0)
            cx3 = p2.x - _ihalf2;
          else
            cx3 = p2.x + _ihalf2;
        } else
          cx3 = (-(cy3 - p2.y) * dty) / dtx + p2.x;

        cy4 = -(_ihalf1 * dtx / (float)Math.Sqrt(temp)) + p2.y;

        if(dtx == 0) {
          if(dty > 0)
            cx4 = p2.x + _ihalf1;
          else
            cx4 = p2.x - _ihalf1;
        } else
          cx4 = (-(cy4 - p2.y) * dty) / dtx + p2.x;
      }

      rp3.x = cx4;
      rp3.y = cy4;
      rp4.x = cx3;
      rp4.y = cy3;
    } else {
      rp3.x = cx3;
      rp3.y = cy3;
      rp4.x = cx4;
      rp4.y = cy4;
    }
    return true;
  }

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

    float tx = 0f, ty = 0f;

    //truong hop nam tren duong thang
    if((a1 == a2) && (b1 == b2)) {
      Vector2 t_p1 = p1;
      Vector2 t_p2 = p1;
      if(dx1 == 0f) {
        if(p2.y < t_p1.y)
          t_p1 = p2;
        if(p3.y < t_p1.y)
          t_p1 = p3;
        if(p4.y < t_p1.y)
          t_p1 = p4;

        if(p2.y > t_p2.y)
          t_p2 = p2;
        if(p3.y > t_p2.y)
          t_p2 = p3;
        if(p4.y > t_p2.y)
          t_p2 = p4;
      } else {
        if(p2.x < t_p1.x)
          t_p1 = p2;
        if(p3.x < t_p1.x)
          t_p1 = p3;
        if(p4.x < t_p1.x)
          t_p1 = p4;

        if(p2.x > t_p2.x)
          t_p2 = p2;
        if(p3.x > t_p2.x)
          t_p2 = p3;
        if(p4.x > t_p2.x)
          t_p2 = p4;
      }

      tx = (t_p1.x - t_p2.x) / 2f;
      tx = t_p2.x + tx;

      ty = (t_p1.y - t_p2.y) / 2f;
      ty = t_p2.y + ty;

      _return.x = tx;
      _return.y = ty;
      return _return;
    }


    if((dx1 != 0) && (dx2 != 0)) {
      tx = -(b1 - b2) / (a1 - a2);
      ty = a1 * tx + b1;
    } else if((dx1 == 0) && (dx2 != 0)) {
      tx = p1.x;
      ty = a2 * tx + b2;
    } else if((dx1 != 0) && (dx2 == 0)) {
      tx = p3.x;
      ty = a1 * tx + b1;
    }

    _return.x = tx;
    _return.y = ty;
    return _return;
  }

  public float AngleBetween2Vector(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
    Vector2 vt1, vt2;
    vt1 = new Vector2(p2.x - p1.x, p2.y - p1.y);
    vt2 = new Vector2(p4.x - p3.x, p4.y - p3.y);
    float t1 = vt1.x * vt2.x + vt1.y * vt2.y;
    float gtvt1 = (float)Math.Sqrt(vt1.x * vt1.x + vt1.y * vt1.y);
    float gtvt2 = (float)Math.Sqrt(vt2.x * vt2.x + vt2.y * vt2.y);
    float t2 = gtvt1 * gtvt2;
    float cosAngle = t1 / t2;

    return ((float)Math.Acos(cosAngle));
  }

  public void Line(Vector2 p1, Vector2 p2) {
    graphicsStroke.Line(p1, p2);
  }

  public void Line(Vector2 p1, Vector2 p2, SVGColor? strokeColor) {
    if(strokeColor != null)
      SetColor(strokeColor.Value.color);
    Line(p1, p2);
  }

  public void Line(Vector2 p1, Vector2 p2, float width) {
    graphicsStroke.Line(p1, p2, width);
  }

  public void Line(Vector2 p1, Vector2 p2, SVGColor? strokeColor, float width) {
    if(strokeColor != null)
      SetColor(strokeColor.Value.color);
    Line(p1, p2, width);
  }

  public void Rect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
    graphicsStroke.Rect(p1, p2, p3, p4);
  }

  public void Rect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, SVGColor? strokeColor) {
    if(strokeColor != null)
      SetColor(strokeColor.Value.color);
    Rect(p1, p2, p3, p4);
  }

  public void Rect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, float width) {
    graphicsStroke.Rect(p1, p2, p3, p4, width);
  }

  public void Rect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
                   SVGColor? strokeColor, float width) {
    if(strokeColor != null)
      SetColor(strokeColor.Value.color);
    Rect(p1, p2, p3, p4, width);
  }

  public void RoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
                          Vector2 p5, Vector2 p6, Vector2 p7, Vector2 p8,
                          float r1, float r2, float angle) {
    graphicsStroke.RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle);
  }

  public void RoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
                          Vector2 p5, Vector2 p6, Vector2 p7, Vector2 p8,
                          float r1, float r2, float angle, SVGColor? strokeColor) {
    if(strokeColor != null)
      SetColor(strokeColor.Value.color);
    RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle);
  }

  public void RoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
                          Vector2 p5, Vector2 p6, Vector2 p7, Vector2 p8,
                          float r1, float r2, float angle, float width) {
    if((int)width == 1) {
      RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle);
      return;
    }
    graphicsStroke.RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle, width);
  }

  public void RoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
                          Vector2 p5, Vector2 p6, Vector2 p7, Vector2 p8,
                          float r1, float r2, float angle, SVGColor? strokeColor, float width) {
    if(strokeColor != null)
      SetColor(strokeColor.Value.color);
    RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle, width);
  }

  public void FillRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
    graphicsFill.Rect(p1, p2, p3, p4);
  }

  public void FillRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
                       SVGColor? strokeColor) {
    graphicsFill.Rect(p1, p2, p3, p4, strokeColor);
  }

  public void FillRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
                       SVGColor fillColor, SVGColor? strokeColor) {
    graphicsFill.Rect(p1, p2, p3, p4, fillColor, strokeColor);
  }

  public void FillRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
                       SVGColor? strokeColor, float width) {
    if((int)width == 1) {
      FillRect(p1, p2, p3, p4, strokeColor);
      return;
    }
    FillRect(p1, p2, p3, p4);
    if(strokeColor == null)
      return;
    SetColor(strokeColor.Value.color);
    Rect(p1, p2, p3, p4, strokeColor, width);
  }

  public void FillRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
                       SVGColor fillColor, SVGColor? strokeColor, float width) {
    if((int)width == 1) {
      FillRect(p1, p2, p3, p4, fillColor, strokeColor);
      return;
    }
    SetColor(fillColor.color);
    FillRect(p1, p2, p3, p4);
    if(strokeColor == null)
      return;
    SetColor(strokeColor.Value.color);
    Rect(p1, p2, p3, p4, strokeColor, width);
  }

  public void FillRoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
                              Vector2 p5, Vector2 p6, Vector2 p7, Vector2 p8,
                              float r1, float r2, float angle) {
    graphicsFill.RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle);
  }

  public void FillRoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
                              Vector2 p5, Vector2 p6, Vector2 p7, Vector2 p8,
                              float r1, float r2, float angle,
                              SVGColor? strokeColor) {
    graphicsFill.RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle, strokeColor);
  }

  public void FillRoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
                              Vector2 p5, Vector2 p6, Vector2 p7, Vector2 p8,
                              float r1, float r2, float angle,
                              SVGColor fillColor, SVGColor? strokeColor) {
    graphicsFill.RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle,
                             fillColor, strokeColor);
  }

  public void FillRoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
                              Vector2 p5, Vector2 p6, Vector2 p7, Vector2 p8,
                              float r1, float r2, float angle,
                              SVGColor? strokeColor, float width) {
    if((int)width == 1) {
      FillRoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle, strokeColor);
      return;
    }
    graphicsFill.RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle);
    if(strokeColor == null)
      return;
    SetColor(strokeColor.Value.color);
    RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle, strokeColor);
  }

  public void FillRoundedRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
                              Vector2 p5, Vector2 p6, Vector2 p7, Vector2 p8,
                              float r1, float r2, float angle,
                              SVGColor fillColor, SVGColor? strokeColor, float width) {
    if((int)width == 1) {
      FillRoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle, strokeColor);
      return;
    }
    SetColor(fillColor.color);
    graphicsFill.RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle);
    if(strokeColor == null)
      return;
    SetColor(strokeColor.Value.color);
    RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle, strokeColor);
  }

  public void Circle(Vector2 p, float r) {
    graphicsStroke.Circle(p, r);
  }

  public void Circle(Vector2 p, float r, SVGColor? strokeColor) {
    if(strokeColor != null)
      SetColor(strokeColor.Value.color);
    Circle(p, r);
  }

  public void Circle(Vector2 p, float r, float width) {
    graphicsStroke.Circle(p, r, width);
  }

  public void Circle(Vector2 p, float r,
                     SVGColor? strokeColor, float width) {
    if(strokeColor != null)
      SetColor(strokeColor.Value.color);
    Circle(p, r, width);
  }

  public void FillCircle(Vector2 p, float r) {
    graphicsFill.Circle(p, r);
  }

  public void FillCircle(Vector2 p, float r, SVGColor? strokeColor) {
    graphicsFill.Circle(p, r, strokeColor);
  }

  public void FillCircle(Vector2 p, float r, SVGColor fillColor, SVGColor? strokeColor) {
    graphicsFill.Circle(p, r, fillColor, strokeColor);
  }

  public void FillCircle(Vector2 p, float r,
                         SVGColor? strokeColor, float width) {
    if((int)width == 1) {
      FillCircle(p, r, strokeColor);
      return;
    }

    FillCircle(p, r);
    if(strokeColor == null)
      return;
    SetColor(strokeColor.Value.color);
    Circle(p, r, strokeColor, width);
  }

  public void FillCircle(Vector2 p, float r,
                         SVGColor fillColor, SVGColor? strokeColor, float width) {
    if((int)width == 1) {
      FillCircle(p, r, strokeColor);
      return;
    }
    SetColor(fillColor.color);
    FillCircle(p, r);
    if(strokeColor == null)
      return;
    SetColor(strokeColor.Value.color);
    Circle(p, r, strokeColor, width);
  }

  public void Ellipse(Vector2 p, float rx, float ry, float angle) {
    graphicsStroke.Ellipse(p, rx, ry, angle);
  }

  public void Ellipse(Vector2 p, float rx, float ry, float angle, SVGColor? strokeColor) {
    if(strokeColor != null)
      SetColor(strokeColor.Value.color);
    Ellipse(p, rx, ry, angle);
  }

  public void Ellipse(Vector2 p, float rx, float ry, float angle, float width) {
    graphicsStroke.Ellipse(p, rx, ry, angle, width);
  }

  public void Ellipse(Vector2 p, float rx, float ry, float angle,
                      SVGColor? strokeColor, float width) {
    if(strokeColor != null)
      SetColor(strokeColor.Value.color);
    Ellipse(p, rx, ry, angle, width);
  }

  public void FillEllipse(Vector2 p, float rx, float ry, float angle) {
    graphicsFill.Ellipse(p, rx, ry, angle);
  }

  public void FillEllipse(Vector2 p, float rx, float ry, float angle, SVGColor? strokeColor) {
    graphicsFill.Ellipse(p, rx, ry, angle, strokeColor);
  }

  public void FillEllipse(Vector2 p, float rx, float ry, float angle,
                          SVGColor fillColor, SVGColor? strokeColor) {
    graphicsFill.Ellipse(p, rx, ry, angle, fillColor, strokeColor);
  }

  public void FillEllipse(Vector2 p, float rx, float ry, float angle,
                          SVGColor? strokeColor, float width) {
    if((int)width == 1) {
      FillEllipse(p, rx, ry, angle, strokeColor);
      return;
    }

    FillEllipse(p, rx, ry, angle);
    if(strokeColor == null)
      return;
    SetColor(strokeColor.Value.color);
    Ellipse(p, rx, ry, angle, width);
  }

  public void FillEllipse(Vector2 p, float rx, float ry, float angle,
                          SVGColor fillColor, SVGColor? strokeColor, float width) {
    if((int)width == 1) {
      FillEllipse(p, rx, ry, angle, strokeColor);
      return;
    }
    SetColor(fillColor.color);
    FillEllipse(p, rx, ry, angle);
    if(strokeColor == null)
      return;
    SetColor(strokeColor.Value.color);
    Ellipse(p, rx, ry, angle, width);
  }

  public void Polygon(Vector2[] points) {
    graphicsStroke.Polygon(points);
  }

  public void Polygon(Vector2[] points, SVGColor? strokeColor) {
    if(strokeColor != null)
      SetColor(strokeColor.Value.color);
    Polygon(points);
  }

  public void Polygon(Vector2[] points, float width) {
    graphicsStroke.Polygon(points, width);
  }

  public void Polygon(Vector2[] points, SVGColor? strokeColor, float width) {
    if(strokeColor != null)
      SetColor(strokeColor.Value.color);
    Polygon(points, width);
  }

  public void FillPolygon(Vector2[] points) {
    graphicsFill.Polygon(points);
  }

  public void FillPolygon(Vector2[] points, SVGColor? strokeColor) {
    graphicsFill.Polygon(points, strokeColor);
  }

  public void FillPolygon(Vector2[] points, SVGColor fillColor, SVGColor? strokeColor) {
    graphicsFill.Polygon(points, fillColor, strokeColor);
  }

  public void FillPolygon(Vector2[] points, SVGColor? strokeColor, float width) {
    if((int)width == 1) {
      FillPolygon(points, strokeColor);
      return;
    }
    FillPolygon(points);
    if(strokeColor == null)
      return;
    SetColor(strokeColor.Value.color);
    Polygon(points, width);
  }

  public void FillPolygon(Vector2[] points,
                          SVGColor fillColor, SVGColor? strokeColor, float width) {
    if((int)width == 1) {
      FillPolygon(points, strokeColor);
      return;
    }
    SetColor(fillColor.color);
    FillPolygon(points);
    if(strokeColor == null)
      return;
    SetColor(strokeColor.Value.color);
    Polygon(points, width);
  }

  //Fill khong to Stroke
  public void FillPath(SVGLinearGradientBrush linearGradientBrush,
                       SVGGraphicsPath graphicsPath) {
    graphicsFill.FillPath(linearGradientBrush, graphicsPath);
  }

  //Fill co Stroke trong do luon
  public void FillPath(SVGLinearGradientBrush linearGradientBrush,
                       SVGColor? strokePathColor,
                       SVGGraphicsPath graphicsPath) {
    graphicsFill.FillPath(linearGradientBrush, strokePathColor, graphicsPath);
  }

  //Fill khong co Stroke, va ve stroke sau
  public void FillPath(SVGLinearGradientBrush linearGradientBrush,
                       SVGColor? strokePathColor,
                       float width,
                       SVGGraphicsPath graphicsPath) {
    graphicsFill.FillPath(linearGradientBrush, strokePathColor, graphicsPath);

    if((int)width == 1)
      graphicsFill.FillPath(linearGradientBrush, strokePathColor, graphicsPath);
    else
      graphicsFill.FillPath(linearGradientBrush, graphicsPath);

    if(strokePathColor == null)
      return;
    SetColor(strokePathColor.Value.color);
  }

  //Fill khong to Stroke
  public void FillPath(SVGRadialGradientBrush radialGradientBrush,
                       SVGGraphicsPath graphicsPath) {
    graphicsFill.FillPath(radialGradientBrush, graphicsPath);
  }

  //Fill co Stroke trong do luon
  public void FillPath(SVGRadialGradientBrush radialGradientBrush,
                       SVGColor? strokePathColor,
                       SVGGraphicsPath graphicsPath) {
    graphicsFill.FillPath(radialGradientBrush, strokePathColor, graphicsPath);
  }

  //Fill khong co Stroke, va ve stroke sau
  public void FillPath(SVGRadialGradientBrush radialGradientBrush,
                       SVGColor? strokePathColor,
                       float width,
                       SVGGraphicsPath graphicsPath) {
    graphicsFill.FillPath(radialGradientBrush, strokePathColor, graphicsPath);
    if((int)width == 1)
      graphicsFill.FillPath(radialGradientBrush, strokePathColor, graphicsPath);
    else
      graphicsFill.FillPath(radialGradientBrush, graphicsPath);

    if(strokePathColor == null)
      return;
    SetColor(strokePathColor.Value.color);
    //graphicsPath.RenderPath(this, width, false);
  }

  //Fill khong to Stroke
  public void FillPath(SVGGraphicsPath graphicsPath) {
    graphicsFill.FillPath(graphicsPath);
  }

  //Fill khong to Stroke
  public void FillPath(SVGColor fillColor, SVGGraphicsPath graphicsPath) {
    graphicsFill.FillPath(fillColor, graphicsPath);
  }

  //Fill co Stroke trong do luon
  public void FillPath(SVGColor fillColor, SVGColor? strokePathColor,
                       SVGGraphicsPath graphicsPath) {
    graphicsFill.FillPath(fillColor, strokePathColor, graphicsPath);
  }

  //Fill khong co Stroke, va ve stroke sau
  public void FillPath(SVGColor fillColor, SVGColor? strokePathColor,
                       float width,
                       SVGGraphicsPath graphicsPath) {
    graphicsFill.FillPath(fillColor, strokePathColor, graphicsPath);
    if((int)width == 1)
      graphicsFill.FillPath(fillColor, strokePathColor, graphicsPath);
    else
      graphicsFill.FillPath(fillColor, graphicsPath);

    if(strokePathColor == null)
      return;
    SetColor(strokePathColor.Value.color);
  }

  public void FillPath(SVGGraphicsPath graphicsPath, Vector2[] points) {
    graphicsFill.FillPath(graphicsPath, points);
  }

  public void FillPath(SVGGraphicsPath graphicsPath, Vector2 point) {
    graphicsFill.FillPath(graphicsPath, point);
  }

  public void DrawPath(SVGGraphicsPath graphicsPath) {
    graphicsStroke.DrawPath(graphicsPath);
  }

  //Fill co Stroke trong do luon
  public void DrawPath(SVGGraphicsPath graphicsPath, float width) {
    graphicsStroke.DrawPath(graphicsPath, width);
  }

  //Fill khong co Stroke, va ve stroke sau
  public void DrawPath(SVGGraphicsPath graphicsPath, float width, SVGColor? strokePathColor) {
    if(strokePathColor == null)
      return;
    SetColor(strokePathColor.Value.color);
    graphicsStroke.DrawPath(graphicsPath, width);
  }
}
