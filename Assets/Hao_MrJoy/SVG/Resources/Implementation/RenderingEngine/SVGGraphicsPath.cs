using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Day la Class duoc dung de Fill va Clip
public enum SVGFillRule : ushort {
  NoneZero = 0,
  EvenOdd = 1
}

public enum SVGPathElementType : ushort {
  Polygon = 0,
  Polyline = 1,
  Rect = 2,
  Circle = 3,
  Ellipse = 4,
  CircleTo = 5,
  EllipseTo = 6,
  MoveTo = 7,
  ArcTo = 8,
  CubicCurveTo = 9,
  QuadraticCurveTo = 10,
  LineTo = 11
}

public class SVGGraphicsPath {
  private Vector2 beginPoint;

  private Vector2 boundUL, boundBR;

  private bool needSetFirstPoint;

  private SVGFillRule _fillRule = SVGFillRule.NoneZero;

  private SVGTransformList _transformList;
  private SVGMatrix _matrixTransform;

  private ArrayList listObject, listType;

  public SVGFillRule fillRule {
    get { return _fillRule; }
    set { _fillRule = value; }
  }

  public SVGMatrix matrixTransform {
    get {
      if(_matrixTransform == null) {
        _matrixTransform = transformList.Consolidate().matrix;
      }
      return _matrixTransform;
    }
  }

  public float transformAngle {
    get {
      float angle = 0.0f;
      for(int i = 0; i < transformList.Count; i++) {
        SVGTransform temp = transformList[i];
        if(temp.type == SVGTransformMode.Rotate)
          angle += temp.angle;
      }
      return angle;
    }
  }

  public SVGTransformList transformList {
    get { return _transformList; }
    set { _transformList = value; }
  }

  public SVGGraphicsPath() {
    beginPoint = new Vector2(0f, 0f);
    needSetFirstPoint = true;
    boundUL = new Vector2(+10000f, +10000f);
    boundBR = new Vector2(-10000f, -10000f);
    transformList = new SVGTransformList();
    listObject = new ArrayList();
    listType = new ArrayList();
  }

  public void Reset() {
    beginPoint = new Vector2(0f, 0f);
    needSetFirstPoint = true;
    boundUL = new Vector2(+10000f, +10000f);
    boundBR = new Vector2(-10000f, -10000f);
    _fillRule = SVGFillRule.NoneZero;
    transformList.Clear();
    listObject.Clear();
    listType.Clear();
  }

  private void ExpandBounds(float x, float y) {
    if(x < boundUL.x)
      boundUL.x = x;
    if(y < boundUL.y)
      boundUL.y = y;

    if(x > boundBR.x)
      boundBR.x = x;
    if(y > boundBR.y)
      boundBR.y = y;
  }

  private void ExpandBounds(float x, float y, float dx, float dy) {
    if((x - dx) < boundUL.x)
      boundUL.x = x - dx;
    if((y - dy) < boundUL.y)
      boundUL.y = y - dy;

    if((x + dx) > boundBR.x)
      boundBR.x = x + dx;
    if((y + dy) > boundBR.y)
      boundBR.y = y + dy;
  }

  private void ExpandBounds(Vector2 point) {
    ExpandBounds(point.x, point.y);
  }

  private void ExpandBounds(Vector2 point, float deltax, float deltay) {
    ExpandBounds(point.x, point.y, deltax, deltay);
  }

  private void ExpandBounds(List<Vector2> points) {
    int length = points.Count;
    for(int i = 0; i < length; i++)
      ExpandBounds(points[i]);
  }

  private void SetFirstPoint(Vector2 p) {
    if(needSetFirstPoint) {
      beginPoint = p;
      needSetFirstPoint = false;
    }
  }

  public void Add(SVGPolygonElement polygonElement) {
    SetFirstPoint(polygonElement.listPoints[0]);

    listType.Add(SVGPathElementType.Polygon);
    listObject.Add(polygonElement);
  }

  public void Add(SVGPolylineElement polylineElement) {
    SetFirstPoint(polylineElement.listPoints[0]);

    listType.Add(SVGPathElementType.Polyline);
    listObject.Add(polylineElement);
  }

  public void Add(SVGRectElement rectElement) {
    SetFirstPoint(new Vector2(rectElement.x.value, rectElement.y.value));

    listType.Add(SVGPathElementType.Rect);
    listObject.Add(rectElement);
  }

  public void Add(SVGCircleElement circleElement) {
    Vector2 p = new Vector2(circleElement.cx.value, circleElement.cy.value);
    SetFirstPoint(p);

    listType.Add(SVGPathElementType.Circle);
    listObject.Add(circleElement);
  }

  public void Add(SVGEllipseElement ellipseElement) {
    Vector2 p = new Vector2(ellipseElement.cx.value, ellipseElement.cy.value);
    SetFirstPoint(p);

    listType.Add(SVGPathElementType.Ellipse);
    listObject.Add(ellipseElement);
  }

  public void AddCircleTo(Vector2 p, float r) {
    SVGGCircle gCircle = new SVGGCircle(p, r);
    listType.Add(SVGPathElementType.CircleTo);
    listObject.Add(gCircle);
  }

  public void AddEllipseTo(Vector2 p, float r1, float r2, float angle) {
    SVGGEllipse gEllipse = new SVGGEllipse(p, r1, r2, angle);
    listType.Add(SVGPathElementType.EllipseTo);
    listObject.Add(gEllipse);
  }

  public void AddMoveTo(Vector2 p) {
    SetFirstPoint(p);

    listType.Add(SVGPathElementType.MoveTo);
    listObject.Add(p);
  }

  public void AddArcTo(float r1, float r2, float angle, bool largeArcFlag, bool sweepFlag, Vector2 p) {
    SVGGArcAbs svgGArcAbs = new SVGGArcAbs(r1, r2, angle, largeArcFlag, sweepFlag, p);
    listType.Add(SVGPathElementType.ArcTo);
    listObject.Add(svgGArcAbs);
  }

  public void AddCubicCurveTo(Vector2 p1, Vector2 p2, Vector2 p) {
    SVGGCubicAbs svgGCubicAbs = new SVGGCubicAbs(p1, p2, p);
    listType.Add(SVGPathElementType.CubicCurveTo);
    listObject.Add(svgGCubicAbs);
  }

  public void AddQuadraticCurveTo(Vector2 p1, Vector2 p) {
    SVGGQuadraticAbs svgGQuadraticAbs = new SVGGQuadraticAbs(p1, p);
    listType.Add(SVGPathElementType.QuadraticCurveTo);
    listObject.Add(svgGQuadraticAbs);
  }

  public void AddLineTo(Vector2 p) {
    listType.Add(SVGPathElementType.LineTo);
    listObject.Add(p);
  }

  public Rect GetBound() {
    float cx, cy, r, rx, ry, x, y, width, height;

    for(int i = 0; i < listObject.Count; i++)
      switch((SVGPathElementType)listType[i]) {
      case SVGPathElementType.Polygon:
        SVGPolygonElement polygonElement = (SVGPolygonElement)listObject[i];
        ExpandBounds(polygonElement.listPoints);
        break;
      case SVGPathElementType.Polyline:
        SVGPolylineElement polylineElement = (SVGPolylineElement)listObject[i];
        ExpandBounds(polylineElement.listPoints);
        break;
      case SVGPathElementType.Rect:
        SVGRectElement rectElement = (SVGRectElement)listObject[i];

        x = rectElement.x.value;
        y = rectElement.y.value;
        width = rectElement.width.value;
        height = rectElement.height.value;
        ExpandBounds(x, y);
        ExpandBounds(x + width, y);
        ExpandBounds(x + width, y + height);
        ExpandBounds(x, y + height);
        break;
      case SVGPathElementType.Circle:
        SVGCircleElement circleElement = (SVGCircleElement)listObject[i];

        cx = circleElement.cx.value;
        cy = circleElement.cy.value;
        r = circleElement.r.value;
        ExpandBounds(cx, cy, r, r);
        break;
      case SVGPathElementType.Ellipse:
        SVGEllipseElement ellipseElement = (SVGEllipseElement)listObject[i];

        cx = ellipseElement.cx.value;
        cy = ellipseElement.cy.value;
        rx = ellipseElement.rx.value;
        ry = ellipseElement.ry.value;
        ExpandBounds(cx, cy, rx, ry);
        break;
      //-----
      case SVGPathElementType.CircleTo:
        SVGGCircle circle = (SVGGCircle)listObject[i];

        r = circle.r;
        ExpandBounds(circle.point, r, r);
        break;
      //-----
      case SVGPathElementType.EllipseTo:
        SVGGEllipse ellipse = (SVGGEllipse)listObject[i];

        ExpandBounds(ellipse.point, ellipse.r1, ellipse.r2);
        break;
      //-----
      case SVGPathElementType.MoveTo:
        Vector2 pointMoveTo = (Vector2)listObject[i];

        ExpandBounds(pointMoveTo);
        break;
      //-----
      case SVGPathElementType.ArcTo:
        SVGGArcAbs gArcAbs = (SVGGArcAbs)listObject[i];

        r = (int)gArcAbs.r1 + (int)gArcAbs.r2;
        ExpandBounds(gArcAbs.point, r, r);
        break;
      //-----
      case SVGPathElementType.CubicCurveTo:
        SVGGCubicAbs gCubicAbs = (SVGGCubicAbs)listObject[i];

        ExpandBounds(gCubicAbs.p1);
        ExpandBounds(gCubicAbs.p2);
        ExpandBounds(gCubicAbs.p);
        break;
      //-----
      case SVGPathElementType.QuadraticCurveTo:
        SVGGQuadraticAbs gQuadraticAbs = (SVGGQuadraticAbs)listObject[i];

        ExpandBounds(gQuadraticAbs.p1);
        ExpandBounds(gQuadraticAbs.p);
        break;
      //-----
      case SVGPathElementType.LineTo:
        ExpandBounds((Vector2)listObject[i]);
        break;
      }

    Rect tmp = new Rect(boundUL.x - 1, boundUL.y - 1, boundBR.x - boundUL.x + 2, boundBR.y - boundUL.y + 2);
    return tmp;
  }

  private void RenderPolygonElement(SVGPolygonElement polygonElement, ISVGPathDraw pathDraw) {
    int length = polygonElement.listPoints.Count;
    Vector2[] points = new Vector2[length];

    for(int i = 0; i < length; i++)
      points[i] = matrixTransform.Transform(polygonElement.listPoints[i]);
    pathDraw.Polygon(points);
  }

  private void RenderPolylineElement(SVGPolylineElement polylineElement, ISVGPathDraw pathDraw) {
    int length = polylineElement.listPoints.Count;
    Vector2[] points = new Vector2[length];

    for(int i = 0; i < length; i++)
      points[i] = matrixTransform.Transform(polylineElement.listPoints[i]);
    pathDraw.Polyline(points);
  }

  private void RenderRectElement(SVGRectElement rectElement, ISVGPathDraw pathDraw) {
    Vector2 p1, p2, p3, p4;
    float tx = rectElement.x.value;
    float ty = rectElement.y.value;
    float tw = rectElement.width.value;
    float th = rectElement.height.value;
    p1 = new Vector2(tx, ty);
    p2 = new Vector2(tx + tw, ty);
    p3 = new Vector2(tx + tw, ty + th);
    p4 = new Vector2(tx, ty + th);

    if(rectElement.rx.value == 0.0f && rectElement.ry.value == 0.0f) {
      p1 = matrixTransform.Transform(p1);
      p2 = matrixTransform.Transform(p2);
      p3 = matrixTransform.Transform(p3);
      p4 = matrixTransform.Transform(p4);

      pathDraw.Rect(p1, p2, p3, p4);
    } else {
      float t_rx = rectElement.rx.value;
      float t_ry = rectElement.ry.value;
      t_rx = (t_rx == 0.0f) ? t_ry : t_rx;
      t_ry = (t_ry == 0.0f) ? t_rx : t_ry;

      t_rx = (t_rx > (tw * 0.5f - 2f)) ? (tw * 0.5f - 2f) : t_rx;
      t_ry = (t_ry > (th * 0.5f - 2f)) ? (th * 0.5f - 2f) : t_ry;

      float angle = transformAngle;

      Vector2 t_p1 = matrixTransform.Transform(new Vector2(p1.x + t_rx, p1.y));
      Vector2 t_p2 = matrixTransform.Transform(new Vector2(p2.x - t_rx, p2.y));
      Vector2 t_p3 = matrixTransform.Transform(new Vector2(p2.x, p2.y + t_ry));
      Vector2 t_p4 = matrixTransform.Transform(new Vector2(p3.x, p3.y - t_ry));

      Vector2 t_p5 = matrixTransform.Transform(new Vector2(p3.x - t_rx, p3.y));
      Vector2 t_p6 = matrixTransform.Transform(new Vector2(p4.x + t_rx, p4.y));
      Vector2 t_p7 = matrixTransform.Transform(new Vector2(p4.x, p4.y - t_ry));
      Vector2 t_p8 = matrixTransform.Transform(new Vector2(p1.x, p1.y + t_ry));

      pathDraw.RoundedRect(t_p1, t_p2, t_p3, t_p4, t_p5, t_p6, t_p7, t_p8, t_rx, t_ry,
      angle);
    }
  }

  private void RenderCircleElement(SVGCircleElement elem, ISVGPathDraw pathDraw) {
    Vector2 p = matrixTransform.Transform(new Vector2(elem.cx.value, elem.cy.value));
    pathDraw.Circle(p, elem.r.value);
  }

  private void RenderEllipseElement(SVGEllipseElement elem, ISVGPathDraw pathDraw) {
    Vector2 p = matrixTransform.Transform(new Vector2(elem.cx.value, elem.cy.value));
    pathDraw.Ellipse(p, elem.rx.value, elem.ry.value, transformAngle);
  }

  private void RenderCircleTo(SVGGCircle circle, ISVGPathDraw pathDraw) {
    pathDraw.CircleTo(matrixTransform.Transform(circle.point), circle.r);
  }

  private void RenderEllipseTo(SVGGEllipse ellipse, ISVGPathDraw pathDraw) {
    pathDraw.EllipseTo(matrixTransform.Transform(ellipse.point), ellipse.r1, ellipse.r2, transformAngle + ellipse.angle);
  }

  private void RenderMoveTo(Vector2 p, ISVGPathDraw pathDraw) {
    pathDraw.MoveTo(matrixTransform.Transform(p));
  }

  private void RenderArcTo(SVGGArcAbs arc, ISVGPathDraw pathDraw) {
    pathDraw.ArcTo(arc.r1, arc.r2, transformAngle + arc.angle, arc.largeArcFlag, arc.sweepFlag, matrixTransform.Transform(arc.point));
  }

  private void RenderCubicCurveTo(SVGGCubicAbs curve, ISVGPathDraw pathDraw) {
    pathDraw.CubicCurveTo(matrixTransform.Transform(curve.p1), matrixTransform.Transform(curve.p2), matrixTransform.Transform(curve.p));
  }

  private void RenderQuadraticCurveTo(SVGGQuadraticAbs curve, ISVGPathDraw pathDraw) {
    pathDraw.QuadraticCurveTo(matrixTransform.Transform(curve.p1), matrixTransform.Transform(curve.p));
  }

  private void RenderLineTo(Vector2 p, ISVGPathDraw pathDraw) {
    pathDraw.LineTo(matrixTransform.Transform(p));
  }

  public void RenderPath(ISVGPathDraw pathDraw, bool isClose) {
    for(int i = 0; i < listObject.Count; i++)
      switch((SVGPathElementType)listType[i]) {
      case SVGPathElementType.Polygon:
        RenderPolygonElement((SVGPolygonElement)listObject[i], pathDraw);
        isClose = false;
        break;
      case SVGPathElementType.Polyline:
        RenderPolylineElement((SVGPolylineElement)listObject[i], pathDraw);
        break;
      case SVGPathElementType.Rect:
        RenderRectElement((SVGRectElement)listObject[i], pathDraw);
        isClose = false;
        break;
      case SVGPathElementType.Circle:
        RenderCircleElement((SVGCircleElement)listObject[i], pathDraw);
        isClose = false;
        break;
      case SVGPathElementType.Ellipse:
        RenderEllipseElement((SVGEllipseElement)listObject[i], pathDraw);
        isClose = false;
        break;
      //-----
      case SVGPathElementType.CircleTo:
        RenderCircleTo((SVGGCircle)listObject[i], pathDraw);
        isClose = false;
        break;
      //-----
      case SVGPathElementType.EllipseTo:
        RenderEllipseTo((SVGGEllipse)listObject[i], pathDraw);
        isClose = false;
        break;
      //-----
      case SVGPathElementType.MoveTo:
        RenderMoveTo((Vector2)listObject[i], pathDraw);
        break;
      case SVGPathElementType.ArcTo:
        RenderArcTo((SVGGArcAbs)listObject[i], pathDraw);
        break;
      case SVGPathElementType.CubicCurveTo:
        RenderCubicCurveTo((SVGGCubicAbs)listObject[i], pathDraw);
        break;
      case SVGPathElementType.QuadraticCurveTo:
        RenderQuadraticCurveTo((SVGGQuadraticAbs)listObject[i], pathDraw);
        break;
      case SVGPathElementType.LineTo:
        RenderLineTo((Vector2)listObject[i], pathDraw);
        break;
      }

    if(isClose)
      pathDraw.LineTo(matrixTransform.Transform(beginPoint));
  }
}
