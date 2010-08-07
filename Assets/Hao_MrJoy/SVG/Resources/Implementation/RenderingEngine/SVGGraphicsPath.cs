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
  private SVGPoint beginPoint, endPoint;

  private SVGPoint boundUL, boundBR;

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
    beginPoint = new SVGPoint(0f, 0f);
    endPoint = new SVGPoint(0f, 0f);
    needSetFirstPoint = true;
    boundUL = new SVGPoint(+10000f, +10000f);
    boundBR = new SVGPoint(-10000f, -10000f);
    transformList = new SVGTransformList();
    listObject = new ArrayList();
    listType = new ArrayList();
  }

  public void Reset() {
    beginPoint = new SVGPoint(0f, 0f);
    endPoint = new SVGPoint(0f, 0f);
    needSetFirstPoint = true;
    boundUL = new SVGPoint(+10000f, +10000f);
    boundBR = new SVGPoint(-10000f, -10000f);
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

  private void ExpandBounds(SVGPoint point) {
    ExpandBounds(point.x, point.y);
  }

  private void ExpandBounds(SVGPoint point, float deltax, float deltay) {
    ExpandBounds(point.x, point.y, deltax, deltay);
  }

  private void ExpandBounds(List<SVGPoint> points) {
    int length = points.Count;
    for(int i = 0; i < length; i++)
      ExpandBounds(points[i]);
  }

  private void SetFirstPoint(SVGPoint p) {
    if(needSetFirstPoint) {
      beginPoint.SetValue(p);
      needSetFirstPoint = false;
    }
  }

  private void SetLastPoint(SVGPoint p) {
    endPoint.SetValue(p);
  }

  public void Add(SVGPolygonElement polygonElement) {
    SetFirstPoint(polygonElement.listPoints[0]);
    SetLastPoint(polygonElement.listPoints[polygonElement.listPoints.Count - 1]);
    
    listType.Add(SVGPathElementType.Polygon);
    listObject.Add(polygonElement);
  }

  public void Add(SVGPolylineElement polylineElement) {
    SetFirstPoint(polylineElement.listPoints[0]);
    SetLastPoint(polylineElement.listPoints[polylineElement.listPoints.Count - 1]);
    
    listType.Add(SVGPathElementType.Polyline);
    listObject.Add(polylineElement);
  }

  public void Add(SVGRectElement rectElement) {
    SetFirstPoint(new SVGPoint(rectElement.x.value, rectElement.y.value));
    SetLastPoint(new SVGPoint(rectElement.x.value, rectElement.y.value));
    
    listType.Add(SVGPathElementType.Rect);
    listObject.Add(rectElement);
  }

  public void Add(SVGCircleElement circleElement) {
    SVGPoint p = new SVGPoint(circleElement.cx.value, circleElement.cy.value);
    SetFirstPoint(p);
    SetLastPoint(p);
    
    listType.Add(SVGPathElementType.Circle);
    listObject.Add(circleElement);
  }

  public void Add(SVGEllipseElement ellipseElement) {
    SVGPoint p = new SVGPoint(ellipseElement.cx.value, ellipseElement.cy.value);
    SetFirstPoint(p);
    SetLastPoint(p);
    
    listType.Add(SVGPathElementType.Ellipse);
    listObject.Add(ellipseElement);
  }

  public void AddCircleTo(SVGPoint p, float r) {
    SVGGCircle gCircle = new SVGGCircle(p, r);
    listType.Add(SVGPathElementType.CircleTo);
    listObject.Add(gCircle);
  }

  public void AddEllipseTo(SVGPoint p, float r1, float r2, float angle) {
    SVGGEllipse gEllipse = new SVGGEllipse(p, r1, r2, angle);
    listType.Add(SVGPathElementType.EllipseTo);
    listObject.Add(gEllipse);
  }

  public void AddMoveTo(SVGPoint p) {
    SetFirstPoint(p);
    SetLastPoint(p);
    
    listType.Add(SVGPathElementType.MoveTo);
    listObject.Add(p);
  }

  public void AddArcTo(float r1, float r2, float angle, bool largeArcFlag, bool sweepFlag, SVGPoint p) {
    SetLastPoint(p);
    SVGGArcAbs svgGArcAbs = new SVGGArcAbs(r1, r2, angle, largeArcFlag, sweepFlag, p);
    listType.Add(SVGPathElementType.ArcTo);
    listObject.Add(svgGArcAbs);
  }

  public void AddCubicCurveTo(SVGPoint p1, SVGPoint p2, SVGPoint p) {
    SetLastPoint(p);
    SVGGCubicAbs svgGCubicAbs = new SVGGCubicAbs(p1, p2, p);
    listType.Add(SVGPathElementType.CubicCurveTo);
    listObject.Add(svgGCubicAbs);
  }

  public void AddQuadraticCurveTo(SVGPoint p1, SVGPoint p) {
    SetLastPoint(p);
    SVGGQuadraticAbs svgGQuadraticAbs = new SVGGQuadraticAbs(p1, p);
    listType.Add(SVGPathElementType.QuadraticCurveTo);
    listObject.Add(svgGQuadraticAbs);
  }

  public void AddLineTo(SVGPoint p) {
    listType.Add(SVGPathElementType.LineTo);
    listObject.Add(p);
  }

  public SVGRect GetBound() {
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
        SVGPoint pointMoveTo = (SVGPoint)listObject[i];
        
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
        ExpandBounds((SVGPoint)listObject[i]);
        break;
      }
    
    SVGRect tmp = new SVGRect(boundUL.x - 1, boundUL.y - 1, boundBR.x - boundUL.x + 2, boundBR.y - boundUL.y + 2);
    return tmp;
  }

  private void RenderPolygonElement(SVGPolygonElement polygonElement, ISVGPathDraw pathDraw) {
    int length = polygonElement.listPoints.Count;
    SVGPoint[] points = new SVGPoint[length];

    for(int i = 0; i < length; i++)
      points[i] = polygonElement.listPoints[i].MatrixTransform(matrixTransform);
    pathDraw.Polygon(points);
  }

  private void RenderPolylineElement(SVGPolylineElement polylineElement, ISVGPathDraw pathDraw) {
    int length = polylineElement.listPoints.Count;
    SVGPoint[] points = new SVGPoint[length];

    for(int i = 0; i < length; i++)
      points[i] = polylineElement.listPoints[i].MatrixTransform(matrixTransform);
    pathDraw.Polyline(points);
  }

  private void RenderRectElement(SVGRectElement rectElement, ISVGPathDraw pathDraw) {
    SVGPoint p1, p2, p3, p4;
    float tx = rectElement.x.value;
    float ty = rectElement.y.value;
    float tw = rectElement.width.value;
    float th = rectElement.height.value;
    p1 = new SVGPoint(tx, ty);
    p2 = new SVGPoint(tx + tw, ty);
    p3 = new SVGPoint(tx + tw, ty + th);
    p4 = new SVGPoint(tx, ty + th);
    
    if(rectElement.rx.value == 0.0f && rectElement.ry.value == 0.0f) {
      p1 = p1.MatrixTransform(matrixTransform);
      p2 = p2.MatrixTransform(matrixTransform);
      p3 = p3.MatrixTransform(matrixTransform);
      p4 = p4.MatrixTransform(matrixTransform);
      
      pathDraw.Rect(p1, p2, p3, p4);
    } else {
      float t_rx = rectElement.rx.value;
      float t_ry = rectElement.ry.value;
      t_rx = (t_rx == 0.0f) ? t_ry : t_rx;
      t_ry = (t_ry == 0.0f) ? t_rx : t_ry;
      
      t_rx = (t_rx > (tw * 0.5f - 2f)) ? (tw * 0.5f - 2f) : t_rx;
      t_ry = (t_ry > (th * 0.5f - 2f)) ? (th * 0.5f - 2f) : t_ry;
      
      float angle = transformAngle;
      
      SVGPoint t_p1 = new SVGPoint(p1.x + t_rx, p1.y).MatrixTransform(matrixTransform);
      SVGPoint t_p2 = new SVGPoint(p2.x - t_rx, p2.y).MatrixTransform(matrixTransform);
      SVGPoint t_p3 = new SVGPoint(p2.x, p2.y + t_ry).MatrixTransform(matrixTransform);
      SVGPoint t_p4 = new SVGPoint(p3.x, p3.y - t_ry).MatrixTransform(matrixTransform);
      
      SVGPoint t_p5 = new SVGPoint(p3.x - t_rx, p3.y).MatrixTransform(matrixTransform);
      SVGPoint t_p6 = new SVGPoint(p4.x + t_rx, p4.y).MatrixTransform(matrixTransform);
      SVGPoint t_p7 = new SVGPoint(p4.x, p4.y - t_ry).MatrixTransform(matrixTransform);
      SVGPoint t_p8 = new SVGPoint(p1.x, p1.y + t_ry).MatrixTransform(matrixTransform);
      
      pathDraw.RoundedRect(t_p1, t_p2, t_p3, t_p4, t_p5, t_p6, t_p7, t_p8, t_rx, t_ry,
      angle);
    }
  }

  private void RenderCircleElement(SVGCircleElement elem, ISVGPathDraw pathDraw) {
    SVGPoint p = new SVGPoint(elem.cx.value, elem.cy.value).MatrixTransform(matrixTransform);
    pathDraw.Circle(p, elem.r.value);
  }

  private void RenderEllipseElement(SVGEllipseElement elem, ISVGPathDraw pathDraw) {
    SVGPoint p = new SVGPoint(elem.cx.value, elem.cy.value).MatrixTransform(matrixTransform);
    pathDraw.Ellipse(p, elem.rx.value, elem.ry.value, transformAngle);
  }

  private void RenderCircleTo(SVGGCircle circle, ISVGPathDraw pathDraw) {
    pathDraw.CircleTo(circle.point.MatrixTransform(matrixTransform), circle.r);
  }

  private void RenderEllipseTo(SVGGEllipse ellipse, ISVGPathDraw pathDraw) {
    pathDraw.EllipseTo(ellipse.point.MatrixTransform(matrixTransform), ellipse.r1, ellipse.r2, transformAngle + ellipse.angle);
  }

  private void RenderMoveTo(SVGPoint p, ISVGPathDraw pathDraw) {
    pathDraw.MoveTo(p.MatrixTransform(matrixTransform));
  }

  private void RenderArcTo(SVGGArcAbs arc, ISVGPathDraw pathDraw) {
    pathDraw.ArcTo(arc.r1, arc.r2, transformAngle + arc.angle, arc.largeArcFlag, arc.sweepFlag, arc.point.MatrixTransform(matrixTransform));
  }

  private void RenderCubicCurveTo(SVGGCubicAbs curve, ISVGPathDraw pathDraw) {
    pathDraw.CubicCurveTo(curve.p1.MatrixTransform(matrixTransform), curve.p2.MatrixTransform(matrixTransform), curve.p.MatrixTransform(matrixTransform));
  }

  private void RenderQuadraticCurveTo(SVGGQuadraticAbs curve, ISVGPathDraw pathDraw) {
    pathDraw.QuadraticCurveTo(curve.p1.MatrixTransform(matrixTransform), curve.p.MatrixTransform(matrixTransform));
  }

  private void RenderLineTo(SVGPoint p, ISVGPathDraw pathDraw) {
    pathDraw.LineTo(p.MatrixTransform(matrixTransform));
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
        RenderMoveTo((SVGPoint)listObject[i], pathDraw);
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
        RenderLineTo((SVGPoint)listObject[i], pathDraw);
        break;
      }
    
    if(isClose)
      pathDraw.LineTo(beginPoint.MatrixTransform(matrixTransform));
  }
}
