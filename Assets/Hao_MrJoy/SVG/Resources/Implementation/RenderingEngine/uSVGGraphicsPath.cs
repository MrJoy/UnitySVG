using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Day la Class duoc dung de Fill va Clip
public enum uSVGFillRuleTypes : ushort {
  SVG_NONE_ZERO = 0,
  SVG_EVEN_ODD  = 1
}
public enum SVGPathElementTypes : ushort {
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
};
public class uSVGGraphicsPath {
  private uSVGPoint beginPoint;
  private uSVGPoint endPoint;
  //----
  private uSVGPoint boundUL;
  private uSVGPoint boundBR;
  //-----
  private bool needSetFirstPoint;
  //-----
  private uSVGFillRuleTypes _fillRule = uSVGFillRuleTypes.SVG_NONE_ZERO;
  //-----
  private uSVGTransformList _transformList;
  private uSVGMatrix _matrixTransform;
  //-----
  private ArrayList listObject;
  private ArrayList listType;
  //================================================================================
  public uSVGFillRuleTypes fillRule {
    get { return _fillRule; }
    set { _fillRule = value; }
  }
  //-----
  public uSVGMatrix matrixTransform {
    get {
      if(_matrixTransform == null) {
        _matrixTransform = transformList.Consolidate().matrix;
      }
      return _matrixTransform;
    }
  }
  //-----
  public float transformAngle {
    get {
      float angle = 0.0f;
      for(int i = 0; i < transformList.Count; i++ ) {
        uSVGTransform temp = transformList[i];
        if(temp.type == uSVGTransformType.SVG_TRANSFORM_ROTATE)
          angle += temp.angle;
      }
      return angle;
    }
  }
  //-----
  public uSVGTransformList transformList {
    get { return _transformList; }
    set { _transformList = value; }
  }
  //================================================================================
  public uSVGGraphicsPath() {
    beginPoint        = new uSVGPoint(0f, 0f);
    endPoint          = new uSVGPoint(0f, 0f);
    needSetFirstPoint = true;
    boundUL           = new uSVGPoint(+10000f, +10000f);
    boundBR           = new uSVGPoint(-10000f, -10000f);
    transformList     = new uSVGTransformList();
    listObject        = new ArrayList();
    listType          = new ArrayList();
  }
  //================================================================================

  //--------------------------------------------------------------------------------
  //Method: Reset
  //--------------------------------------------------------------------------------
  public void Reset() {
    beginPoint        = new uSVGPoint(0f, 0f);
    endPoint          = new uSVGPoint(0f, 0f);
    needSetFirstPoint = true;
    boundUL           = new uSVGPoint(+10000f, +10000f);
    boundBR           = new uSVGPoint(-10000f, -10000f);
    _fillRule         = uSVGFillRuleTypes.SVG_NONE_ZERO;
    transformList     = new uSVGTransformList();
    listObject.Clear();
    listType.Clear();
  }
  //--------------------------------------------------------------------------------
  //Method: f_ResetLimitPoints
  //--------------------------------------------------------------------------------
  private void f_ResetLimitPoints(float x, float y) {
    if(x < boundUL.x) boundUL.x = x;
    if(y < boundUL.y) boundUL.y = y;

    if(x > boundBR.x) boundBR.x = x;
    if(y > boundBR.y) boundBR.y = y;
  }
  //-----
  private void f_ResetLimitPoints(float x, float y, float dx, float dy) {
    if((x - dx) < boundUL.x) boundUL.x = x - dx;
    if((y - dy) < boundUL.y) boundUL.y = y - dy;

    if((x + dx) > boundBR.x) boundBR.x = x + dx;
    if((y + dy) > boundBR.y) boundBR.y = y + dy;
  }
  //-----
  private void f_ResetLimitPoints(uSVGPoint point) {
    f_ResetLimitPoints(point.x, point.y);
  }
  //-----
  private void f_ResetLimitPoints(uSVGPoint point, float deltax, float deltay) {
    f_ResetLimitPoints(point.x, point.y, deltax, deltay);
  }
  //-----
  private void f_ResetLimitPoints(List<uSVGPoint> points) {
    int length = points.Count;
    for(int i = 0; i < length; i++)
      f_ResetLimitPoints(points[i]);
  }

  //--------------------------------------------------------------------------------
  //Method: f_SetFirstPoint
  //--------------------------------------------------------------------------------
  private void f_SetFirstPoint(uSVGPoint p) {
    if(needSetFirstPoint) {
      beginPoint.SetValue(p);
      needSetFirstPoint = false;
    }
  }
  //--------------------------------------------------------------------------------
  //Method: f_SetLastPoint
  //--------------------------------------------------------------------------------
  private void f_SetLastPoint(uSVGPoint p) {
    endPoint.SetValue(p);
  }
  //================================================================================
  //--------------------------------------------------------------------------------
  //Method: Add
  //--------------------------------------------------------------------------------
  public void Add(uSVGPolygonElement polygonElement) {
    f_SetFirstPoint(polygonElement.listPoints[0]);
    f_SetLastPoint(polygonElement.listPoints[polygonElement.listPoints.Count - 1]);

    listType.Add(SVGPathElementTypes.Polygon);
    listObject.Add(polygonElement);
  }
  //-----
  public void Add(uSVGPolylineElement polylineElement) {
    f_SetFirstPoint(polylineElement.listPoints[0]);
    f_SetLastPoint(polylineElement.listPoints[polylineElement.listPoints.Count - 1]);

    listType.Add(SVGPathElementTypes.Polyline);
    listObject.Add(polylineElement);
  }
  //-----
  public void Add(uSVGRectElement rectElement) {
    f_SetFirstPoint(new uSVGPoint(rectElement.x.value, rectElement.y.value));
    f_SetLastPoint(new uSVGPoint(rectElement.x.value, rectElement.y.value));

    listType.Add(SVGPathElementTypes.Rect);
    listObject.Add(rectElement);
  }
  //-----
  public void Add(uSVGCircleElement circleElement) {
    f_SetFirstPoint(new uSVGPoint(circleElement.cx.value, circleElement.cy.value));
    f_SetLastPoint(new uSVGPoint(circleElement.cx.value, circleElement.cy.value));

    listType.Add(SVGPathElementTypes.Circle);
    listObject.Add(circleElement);
  }
  //-----
  public void Add(uSVGEllipseElement ellipseElement) {
    f_SetFirstPoint(new uSVGPoint(ellipseElement.cx.value, ellipseElement.cy.value));
    f_SetLastPoint(new uSVGPoint(ellipseElement.cx.value, ellipseElement.cy.value));

    listType.Add(SVGPathElementTypes.Ellipse);
    listObject.Add(ellipseElement);
  }
  //--------------------------------------------------------------------------------
  //Method: AddCircleTo
  //--------------------------------------------------------------------------------
  public void AddCircleTo(uSVGPoint p, float r) {
    uSVGGCircle gCircle = new uSVGGCircle(p, r);
    listType.Add(SVGPathElementTypes.CircleTo);
    listObject.Add(gCircle);
  }
  //--------------------------------------------------------------------------------
  //Method: AddEllipseTo
  //--------------------------------------------------------------------------------
  public void AddEllipseTo(uSVGPoint p, float r1, float r2, float angle) {
    uSVGGEllipse gEllipse = new uSVGGEllipse(p, r1, r2, angle);
    listType.Add(SVGPathElementTypes.EllipseTo);
    listObject.Add(gEllipse);
  }
  //--------------------------------------------------------------------------------
  //Method: AddMoveTo
  //--------------------------------------------------------------------------------
  public void AddMoveTo(uSVGPoint p) {
    uSVGPoint t_p;
    t_p = new uSVGPoint(p.x, p.y);

    f_SetFirstPoint(t_p);
    f_SetLastPoint(t_p);

    listType.Add(SVGPathElementTypes.MoveTo);
    listObject.Add(t_p);
  }
  //--------------------------------------------------------------------------------
  //Method: AddArcTo
  //--------------------------------------------------------------------------------
  public void AddArcTo(float r1, float r2, float angle,
                       bool largeArcFlag, bool sweepFlag, uSVGPoint p) {
    f_SetLastPoint(p);
    uSVGGArcAbs svgGArcAbs = new uSVGGArcAbs(r1, r2, angle, largeArcFlag, sweepFlag, p);
    listType.Add(SVGPathElementTypes.ArcTo);
    listObject.Add(svgGArcAbs);
  }
  //--------------------------------------------------------------------------------
  //Method: AddCubicCurveTo
  //--------------------------------------------------------------------------------
  public void AddCubicCurveTo(uSVGPoint p1, uSVGPoint p2, uSVGPoint p) {
    f_SetLastPoint(p);
    uSVGGCubicAbs svgGCubicAbs = new uSVGGCubicAbs(p1, p2, p);
    listType.Add(SVGPathElementTypes.CubicCurveTo);
    listObject.Add(svgGCubicAbs);
  }
  //--------------------------------------------------------------------------------
  //Method: AddQuadraticCurveTo
  //--------------------------------------------------------------------------------
  public void AddQuadraticCurveTo(uSVGPoint p1, uSVGPoint p) {
    f_SetLastPoint(p);
    uSVGGQuadraticAbs svgGQuadraticAbs = new uSVGGQuadraticAbs(p1, p);
    listType.Add(SVGPathElementTypes.QuadraticCurveTo);
    listObject.Add(svgGQuadraticAbs);
  }
  //--------------------------------------------------------------------------------
  //Method: AddLineTo
  //--------------------------------------------------------------------------------
  public void AddLineTo(uSVGPoint p) {
    uSVGPoint t_p;
    t_p = new uSVGPoint(p.x, p.y);

    f_SetFirstPoint(t_p);
    f_SetLastPoint(t_p);

    listType.Add(SVGPathElementTypes.LineTo);
    listObject.Add(t_p);
  }
  //================================================================================
  //--------------------------------------------------------------------------------
  //Method: GetBound
  //--------------------------------------------------------------------------------
  public uSVGRect GetBound() {
Profiler.BeginSample("uSVGGraphicsPath/GetBound()");
    float cx, cy, r, rx, ry, x, y, width, height;

    for (int i = 0; i < listObject.Count; i++ ) {
      switch((SVGPathElementTypes)listType[i]) {
        case SVGPathElementTypes.Polygon:
          uSVGPolygonElement polygonElement = (uSVGPolygonElement)listObject[i];
          f_ResetLimitPoints(polygonElement.listPoints);
          break;
        case SVGPathElementTypes.Polyline:
          uSVGPolylineElement polylineElement = (uSVGPolylineElement)listObject[i];
          f_ResetLimitPoints(polylineElement.listPoints);
          break;
        case SVGPathElementTypes.Rect:
          uSVGRectElement rectElement = (uSVGRectElement)listObject[i];

          x = rectElement.x.value;
          y = rectElement.y.value;
          width = rectElement.width.value;
          height = rectElement.height.value;
          f_ResetLimitPoints(x, y);
          f_ResetLimitPoints(x + width, y);
          f_ResetLimitPoints(x + width, y + height);
          f_ResetLimitPoints(x, y + height);
          break;
        case SVGPathElementTypes.Circle:
          uSVGCircleElement circleElement = (uSVGCircleElement)listObject[i];

          cx = circleElement.cx.value;
          cy = circleElement.cy.value;
          r = circleElement.r.value;
          f_ResetLimitPoints(cx, cy, r, r);
          break;
        case SVGPathElementTypes.Ellipse:
          uSVGEllipseElement ellipseElement = (uSVGEllipseElement)listObject[i];

          cx = ellipseElement.cx.value;
          cy = ellipseElement.cy.value;
          rx = ellipseElement.rx.value;
          ry = ellipseElement.ry.value;
          f_ResetLimitPoints(cx, cy, rx, ry);
          break;
        //-----
        case SVGPathElementTypes.CircleTo:
          uSVGGCircle circle = (uSVGGCircle)listObject[i];

          r = circle.r;
          f_ResetLimitPoints(circle.point, r, r);
          break;
        //-----
        case SVGPathElementTypes.EllipseTo:
          uSVGGEllipse ellipse = (uSVGGEllipse)listObject[i];

          rx = ellipse.r1;
          ry = ellipse.r2;
          f_ResetLimitPoints(ellipse.point, rx, ry);
          break;
        //-----
        case SVGPathElementTypes.MoveTo:
          uSVGPoint pointMoveTo = (uSVGPoint)listObject[i];

          f_ResetLimitPoints(pointMoveTo);
          break;
        //-----
        case SVGPathElementTypes.ArcTo:
          uSVGGArcAbs gArcAbs = (uSVGGArcAbs)listObject[i];

          r = (int)gArcAbs.r1 + (int)gArcAbs.r2;
          f_ResetLimitPoints(gArcAbs.point, r, r);
          break;
        //-----
        case SVGPathElementTypes.CubicCurveTo:
          uSVGGCubicAbs gCubicAbs = (uSVGGCubicAbs)listObject[i];

          f_ResetLimitPoints(gCubicAbs.p1);
          f_ResetLimitPoints(gCubicAbs.p2);
          f_ResetLimitPoints(gCubicAbs.p);
          break;
        //-----
        case SVGPathElementTypes.QuadraticCurveTo:
          uSVGGQuadraticAbs gQuadraticAbs = (uSVGGQuadraticAbs)listObject[i];

          f_ResetLimitPoints(gQuadraticAbs.p1);
          f_ResetLimitPoints(gQuadraticAbs.p);
          break;
        //-----
        case SVGPathElementTypes.LineTo:
          uSVGPoint pointlineTo = (uSVGPoint)listObject[i];

          f_ResetLimitPoints(pointlineTo);
          break;
      }
    }

    uSVGRect tmp = new uSVGRect(boundUL.x - 1, boundUL.y - 1,
                                boundBR.x - boundUL.x + 2,
                                boundBR.y - boundUL.y + 2);
Profiler.EndSample();
    return tmp;
  }
  //--------------------------------------------------------------------------------
  //Method: GetBoundTransformed
  //--------------------------------------------------------------------------------
  public uSVGRect GetBoundTransformed() {
    uSVGRect orig = GetBound();

    uSVGPoint p1 = new uSVGPoint(orig.x, orig.y);
    p1 = p1.MatrixTransform(matrixTransform);

    uSVGPoint p2 = new uSVGPoint(orig.x + orig.width, orig.y + orig.height);
    p2 = p2.MatrixTransform(matrixTransform);

    return new uSVGRect(p1.x, p1.y, p2.x - p1.x, p2.y - p1.y);
  }
  //================================================================================
  //                                      RENDER
  //--------------------------------------------------------------------------------
  //Method: f_RenderPolygonElement
  //--------------------------------------------------------------------------------
  private void f_RenderPolygonElement(uSVGPolygonElement polygonElement,
                                      uISVGPathDraw pathDraw) {
    int length = polygonElement.listPoints.Count;
    uSVGPoint[] points = new uSVGPoint[length];

    for(int i = 0; i < length; i++)
      points[i] = polygonElement.listPoints[i].MatrixTransform(matrixTransform);
    pathDraw.Polygon(points);
  }
  //--------------------------------------------------------------------------------
  //Method: f_RenderPolylineElement
  //--------------------------------------------------------------------------------
  private void f_RenderPolylineElement(uSVGPolylineElement polylineElement,
                                       uISVGPathDraw pathDraw) {
    int length = polylineElement.listPoints.Count;
    uSVGPoint[] points = new uSVGPoint[length];

    for(int i = 0; i < length; i++)
      points[i] = polylineElement.listPoints[i].MatrixTransform(matrixTransform);
    pathDraw.Polyline(points);
  }
  //--------------------------------------------------------------------------------
  //Method: f_RenderRectElement
  //--------------------------------------------------------------------------------
  private void f_RenderRectElement(uSVGRectElement rectElement,
                                   uISVGPathDraw pathDraw) {
    uSVGPoint p1, p2, p3, p4;
    float tx = rectElement.x.value;
    float ty = rectElement.y.value;
    float tw = rectElement.width.value;
    float th = rectElement.height.value;
    p1 = new uSVGPoint(tx, ty);
    p2 = new uSVGPoint(tx + tw, ty);
    p3 = new uSVGPoint(tx + tw, ty + th);
    p4 = new uSVGPoint(tx, ty + th);

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

      t_rx = (t_rx > (tw / 2f -2f)) ? (tw / 2f - 2f) : t_rx;
      t_ry = (t_ry > (th / 2f - 2f)) ? (th / 2f - 2f) : t_ry;

      uSVGPoint t_p1, t_p2, t_p3, t_p4, t_p5, t_p6, t_p7, t_p8;
      float angle = transformAngle;

      t_p1 = new uSVGPoint(p1.x + t_rx, p1.y);
      t_p2 = new uSVGPoint(p2.x - t_rx, p2.y);
      t_p3 = new uSVGPoint(p2.x, p2.y + t_ry);
      t_p4 = new uSVGPoint(p3.x, p3.y - t_ry);

      t_p5 = new uSVGPoint(p3.x - t_rx, p3.y);
      t_p6 = new uSVGPoint(p4.x + t_rx, p4.y);
      t_p7 = new uSVGPoint(p4.x, p4.y - t_ry);
      t_p8 = new uSVGPoint(p1.x, p1.y + t_ry);

      t_p1 = t_p1.MatrixTransform(matrixTransform);
      t_p2 = t_p2.MatrixTransform(matrixTransform);
      t_p3 = t_p3.MatrixTransform(matrixTransform);
      t_p4 = t_p4.MatrixTransform(matrixTransform);
      t_p5 = t_p5.MatrixTransform(matrixTransform);
      t_p6 = t_p6.MatrixTransform(matrixTransform);
      t_p7 = t_p7.MatrixTransform(matrixTransform);
      t_p8 = t_p8.MatrixTransform(matrixTransform);

      pathDraw.RoundedRect(t_p1, t_p2, t_p3, t_p4, t_p5, t_p6, t_p7, t_p8, t_rx, t_ry, angle);
    }
  }
  //--------------------------------------------------------------------------------
  //Method: f_RenderCircleElement
  //--------------------------------------------------------------------------------
  private void f_RenderCircleElement(uSVGCircleElement circleElement,
                                     uISVGPathDraw pathDraw) {
    uSVGPoint p;
    p = new uSVGPoint(circleElement.cx.value, circleElement.cy.value);
    p = p.MatrixTransform(matrixTransform);
    pathDraw.Circle(p, circleElement.r.value);
  }
  //--------------------------------------------------------------------------------
  //Method: f_RenderEllipseElement
  //--------------------------------------------------------------------------------
  private void f_RenderEllipseElement(uSVGEllipseElement ellipseElement,
                                      uISVGPathDraw pathDraw) {
    uSVGPoint p;
    p = new uSVGPoint(ellipseElement.cx.value, ellipseElement.cy.value);
    p = p.MatrixTransform(matrixTransform);
    float angle = transformAngle;
    pathDraw.Ellipse(p, ellipseElement.rx.value,
                ellipseElement.ry.value, angle);
  }
  //--------------------------------------------------------------------------------
  //Method: f_RenderGCircle
  //--------------------------------------------------------------------------------
  private void f_RenderCircleTo(uSVGGCircle circle, uISVGPathDraw pathDraw) {
    uSVGPoint p;
    p = new uSVGPoint(circle.point.x, circle.point.y);
    p = p.MatrixTransform(matrixTransform);
    pathDraw.CircleTo(p, circle.r);
  }
  //--------------------------------------------------------------------------------
  //Method: f_RenderGEllipse
  //--------------------------------------------------------------------------------
  private void f_RenderEllipseTo(uSVGGEllipse ellipse, uISVGPathDraw pathDraw) {
    uSVGPoint p;
    p = new uSVGPoint(ellipse.point.x, ellipse.point.y);
    p = p.MatrixTransform(matrixTransform);
    float angle = transformAngle + ellipse.angle;
    pathDraw.EllipseTo(p, ellipse.r1, ellipse.r2, angle);
  }
  //--------------------------------------------------------------------------------
  //Method: f_RenderMoveTo
  //--------------------------------------------------------------------------------
  private void f_RenderMoveTo(uSVGPoint p, uISVGPathDraw pathDraw) {
    uSVGPoint tp;
    tp = new uSVGPoint(p.x, p.y);
    tp = tp.MatrixTransform(matrixTransform);
    pathDraw.MoveTo(tp);
  }
  //--------------------------------------------------------------------------------
  //Method: f_RenderArcTo
  //--------------------------------------------------------------------------------
  private void f_RenderArcTo(uSVGGArcAbs gArcAbs, uISVGPathDraw pathDraw) {
    uSVGPoint p;
    p = new uSVGPoint(gArcAbs.point.x, gArcAbs.point.y);
    p = p.MatrixTransform(matrixTransform);

    float angle = transformAngle + gArcAbs.angle;

    pathDraw.ArcTo(gArcAbs.r1, gArcAbs.r2, angle, gArcAbs.largeArcFlag, gArcAbs.sweepFlag, p);
  }
  //--------------------------------------------------------------------------------
  //Method: f_RenderCubicCurveTo
  //--------------------------------------------------------------------------------
  private void f_RenderCubicCurveTo(uSVGGCubicAbs gCubicAbs, uISVGPathDraw pathDraw) {
    uSVGPoint p1, p2, p;
    p1 = new uSVGPoint(0f, 0f); p1.SetValue(gCubicAbs.p1);
    p2 = new uSVGPoint(0f, 0f); p2.SetValue(gCubicAbs.p2);
    p = new uSVGPoint(0f, 0f); p.SetValue(gCubicAbs.p);

    p = p.MatrixTransform(matrixTransform);
    p1 = p1.MatrixTransform(matrixTransform);
    p2 = p2.MatrixTransform(matrixTransform);

    pathDraw.CubicCurveTo(p1, p2, p);
  }
  //--------------------------------------------------------------------------------
  //Method: f_RenderQuadraticCurveTo
  //--------------------------------------------------------------------------------
  private void f_RenderQuadraticCurveTo(uSVGGQuadraticAbs gQuadraticAbs, uISVGPathDraw pathDraw) {
    uSVGPoint p1, p;
    p1 = new uSVGPoint(0f, 0f); p1.SetValue(gQuadraticAbs.p1);
    p = new uSVGPoint(0f, 0f); p.SetValue(gQuadraticAbs.p);

    p = p.MatrixTransform(matrixTransform);
    p1 = p1.MatrixTransform(matrixTransform);

    pathDraw.QuadraticCurveTo(p1, p);
  }
  //--------------------------------------------------------------------------------
  //Method: f_RenderLineTo
  //--------------------------------------------------------------------------------
  private void f_RenderLineTo(uSVGPoint p, uISVGPathDraw pathDraw) {
    uSVGPoint tp;
    tp = new uSVGPoint(0f, 0f); tp.SetValue(p);
    tp = tp.MatrixTransform(matrixTransform);
    pathDraw.LineTo(tp);
  }
  //================================================================================
  //--------------------------------------------------------------------------------
  //Method: RenderPath
  //--------------------------------------------------------------------------------
  public void RenderPath(uISVGPathDraw pathDraw, bool isClose) {
    for(int i = 0; i < listObject.Count; i++) {
      switch((SVGPathElementTypes)listType[i]) {
        case SVGPathElementTypes.Polygon:
          f_RenderPolygonElement((listObject[i] as uSVGPolygonElement), pathDraw);
          isClose = false;
          break;
        case SVGPathElementTypes.Polyline:
          f_RenderPolylineElement((listObject[i] as uSVGPolylineElement),pathDraw);
          break;
        case SVGPathElementTypes.Rect:
          f_RenderRectElement((listObject[i] as uSVGRectElement), pathDraw);
          isClose = false;
          break;
        case SVGPathElementTypes.Circle:
          f_RenderCircleElement((listObject[i] as uSVGCircleElement), pathDraw);
          isClose = false;
          break;
        case SVGPathElementTypes.Ellipse:
          f_RenderEllipseElement((listObject[i] as uSVGEllipseElement), pathDraw);
          isClose = false;
          break;
        //-----
        case SVGPathElementTypes.CircleTo:
          f_RenderCircleTo((listObject[i] as uSVGGCircle), pathDraw);
          isClose = false;
          break;
        //-----
        case SVGPathElementTypes.EllipseTo:
          f_RenderEllipseTo((listObject[i] as uSVGGEllipse), pathDraw);
          isClose = false;
          break;
        //-----
        case SVGPathElementTypes.MoveTo:
          f_RenderMoveTo((uSVGPoint)listObject[i], pathDraw);
          break;
        case SVGPathElementTypes.ArcTo:
          f_RenderArcTo((listObject[i] as uSVGGArcAbs), pathDraw);
          break;
        case SVGPathElementTypes.CubicCurveTo:
          f_RenderCubicCurveTo((listObject[i] as uSVGGCubicAbs), pathDraw);
          break;
        case SVGPathElementTypes.QuadraticCurveTo:
          f_RenderQuadraticCurveTo((listObject[i] as uSVGGQuadraticAbs), pathDraw);
          break;
        case SVGPathElementTypes.LineTo:
          f_RenderLineTo((uSVGPoint)listObject[i], pathDraw);
          break;
      }
    }

    if(isClose) {
      uSVGPoint tp = new uSVGPoint(beginPoint.x, beginPoint.y);
      tp = tp.MatrixTransform(matrixTransform);
      pathDraw.LineTo(tp);
    }
  }
}
