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
  //Method: ExpandBounds
  //--------------------------------------------------------------------------------
  private void ExpandBounds(float x, float y) {
    if(x < boundUL.x) boundUL.x = x;
    if(y < boundUL.y) boundUL.y = y;

    if(x > boundBR.x) boundBR.x = x;
    if(y > boundBR.y) boundBR.y = y;
  }
  //-----
  private void ExpandBounds(float x, float y, float dx, float dy) {
    if((x - dx) < boundUL.x) boundUL.x = x - dx;
    if((y - dy) < boundUL.y) boundUL.y = y - dy;

    if((x + dx) > boundBR.x) boundBR.x = x + dx;
    if((y + dy) > boundBR.y) boundBR.y = y + dy;
  }
  //-----
  private void ExpandBounds(uSVGPoint point) {
    ExpandBounds(point.x, point.y);
  }
  //-----
  private void ExpandBounds(uSVGPoint point, float deltax, float deltay) {
    ExpandBounds(point.x, point.y, deltax, deltay);
  }
  //-----
  private void ExpandBounds(List<uSVGPoint> points) {
    int length = points.Count;
    for(int i = 0; i < length; i++)
      ExpandBounds(points[i]);
  }

  //--------------------------------------------------------------------------------
  //Method: SetFirstPoint
  //--------------------------------------------------------------------------------
  private void SetFirstPoint(uSVGPoint p) {
    if(needSetFirstPoint) {
      beginPoint.SetValue(p);
      needSetFirstPoint = false;
    }
  }
  //--------------------------------------------------------------------------------
  //Method: SetLastPoint
  //--------------------------------------------------------------------------------
  private void SetLastPoint(uSVGPoint p) {
    endPoint.SetValue(p);
  }
  //================================================================================
  //--------------------------------------------------------------------------------
  //Method: Add
  //--------------------------------------------------------------------------------
  public void Add(uSVGPolygonElement polygonElement) {
    SetFirstPoint(polygonElement.listPoints[0]);
    SetLastPoint(polygonElement.listPoints[polygonElement.listPoints.Count - 1]);

    listType.Add(SVGPathElementTypes.Polygon);
    listObject.Add(polygonElement);
  }
  //-----
  public void Add(uSVGPolylineElement polylineElement) {
    SetFirstPoint(polylineElement.listPoints[0]);
    SetLastPoint(polylineElement.listPoints[polylineElement.listPoints.Count - 1]);

    listType.Add(SVGPathElementTypes.Polyline);
    listObject.Add(polylineElement);
  }
  //-----
  public void Add(uSVGRectElement rectElement) {
    SetFirstPoint(new uSVGPoint(rectElement.x.value, rectElement.y.value));
    SetLastPoint(new uSVGPoint(rectElement.x.value, rectElement.y.value));

    listType.Add(SVGPathElementTypes.Rect);
    listObject.Add(rectElement);
  }
  //-----
  public void Add(uSVGCircleElement circleElement) {
    uSVGPoint p = new uSVGPoint(circleElement.cx.value, circleElement.cy.value);
    SetFirstPoint(p);
    SetLastPoint(p);

    listType.Add(SVGPathElementTypes.Circle);
    listObject.Add(circleElement);
  }
  //-----
  public void Add(uSVGEllipseElement ellipseElement) {
    uSVGPoint p = new uSVGPoint(ellipseElement.cx.value, ellipseElement.cy.value);
    SetFirstPoint(p);
    SetLastPoint(p);

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
    SetFirstPoint(p);
    SetLastPoint(p);

    listType.Add(SVGPathElementTypes.MoveTo);
    listObject.Add(p);
  }
  //--------------------------------------------------------------------------------
  //Method: AddArcTo
  //--------------------------------------------------------------------------------
  public void AddArcTo(float r1, float r2, float angle,
                       bool largeArcFlag, bool sweepFlag, uSVGPoint p) {
    SetLastPoint(p);
    uSVGGArcAbs svgGArcAbs = new uSVGGArcAbs(r1, r2, angle, largeArcFlag, sweepFlag, p);
    listType.Add(SVGPathElementTypes.ArcTo);
    listObject.Add(svgGArcAbs);
  }
  //--------------------------------------------------------------------------------
  //Method: AddCubicCurveTo
  //--------------------------------------------------------------------------------
  public void AddCubicCurveTo(uSVGPoint p1, uSVGPoint p2, uSVGPoint p) {
    SetLastPoint(p);
    uSVGGCubicAbs svgGCubicAbs = new uSVGGCubicAbs(p1, p2, p);
    listType.Add(SVGPathElementTypes.CubicCurveTo);
    listObject.Add(svgGCubicAbs);
  }
  //--------------------------------------------------------------------------------
  //Method: AddQuadraticCurveTo
  //--------------------------------------------------------------------------------
  public void AddQuadraticCurveTo(uSVGPoint p1, uSVGPoint p) {
    SetLastPoint(p);
    uSVGGQuadraticAbs svgGQuadraticAbs = new uSVGGQuadraticAbs(p1, p);
    listType.Add(SVGPathElementTypes.QuadraticCurveTo);
    listObject.Add(svgGQuadraticAbs);
  }
  //--------------------------------------------------------------------------------
  //Method: AddLineTo
  //--------------------------------------------------------------------------------
  public void AddLineTo(uSVGPoint p) {
    listType.Add(SVGPathElementTypes.LineTo);
    listObject.Add(p);
  }
  //================================================================================
  //--------------------------------------------------------------------------------
  //Method: GetBound
  //--------------------------------------------------------------------------------
  public uSVGRect GetBound() {
Profiler.BeginSample("uSVGGraphicsPath.GetBound()");
    float cx, cy, r, rx, ry, x, y, width, height;

    for (int i = 0; i < listObject.Count; i++ ) {
      switch((SVGPathElementTypes)listType[i]) {
        case SVGPathElementTypes.Polygon:
          uSVGPolygonElement polygonElement = (uSVGPolygonElement)listObject[i];
          ExpandBounds(polygonElement.listPoints);
          break;
        case SVGPathElementTypes.Polyline:
          uSVGPolylineElement polylineElement = (uSVGPolylineElement)listObject[i];
          ExpandBounds(polylineElement.listPoints);
          break;
        case SVGPathElementTypes.Rect:
          uSVGRectElement rectElement = (uSVGRectElement)listObject[i];

          x = rectElement.x.value;
          y = rectElement.y.value;
          width = rectElement.width.value;
          height = rectElement.height.value;
          ExpandBounds(x, y);
          ExpandBounds(x + width, y);
          ExpandBounds(x + width, y + height);
          ExpandBounds(x, y + height);
          break;
        case SVGPathElementTypes.Circle:
          uSVGCircleElement circleElement = (uSVGCircleElement)listObject[i];

          cx = circleElement.cx.value;
          cy = circleElement.cy.value;
          r = circleElement.r.value;
          ExpandBounds(cx, cy, r, r);
          break;
        case SVGPathElementTypes.Ellipse:
          uSVGEllipseElement ellipseElement = (uSVGEllipseElement)listObject[i];

          cx = ellipseElement.cx.value;
          cy = ellipseElement.cy.value;
          rx = ellipseElement.rx.value;
          ry = ellipseElement.ry.value;
          ExpandBounds(cx, cy, rx, ry);
          break;
        //-----
        case SVGPathElementTypes.CircleTo:
          uSVGGCircle circle = (uSVGGCircle)listObject[i];

          r = circle.r;
          ExpandBounds(circle.point, r, r);
          break;
        //-----
        case SVGPathElementTypes.EllipseTo:
          uSVGGEllipse ellipse = (uSVGGEllipse)listObject[i];

          ExpandBounds(ellipse.point, ellipse.r1, ellipse.r2);
          break;
        //-----
        case SVGPathElementTypes.MoveTo:
          uSVGPoint pointMoveTo = (uSVGPoint)listObject[i];

          ExpandBounds(pointMoveTo);
          break;
        //-----
        case SVGPathElementTypes.ArcTo:
          uSVGGArcAbs gArcAbs = (uSVGGArcAbs)listObject[i];

          r = (int)gArcAbs.r1 + (int)gArcAbs.r2;
          ExpandBounds(gArcAbs.point, r, r);
          break;
        //-----
        case SVGPathElementTypes.CubicCurveTo:
          uSVGGCubicAbs gCubicAbs = (uSVGGCubicAbs)listObject[i];

          ExpandBounds(gCubicAbs.p1);
          ExpandBounds(gCubicAbs.p2);
          ExpandBounds(gCubicAbs.p);
          break;
        //-----
        case SVGPathElementTypes.QuadraticCurveTo:
          uSVGGQuadraticAbs gQuadraticAbs = (uSVGGQuadraticAbs)listObject[i];

          ExpandBounds(gQuadraticAbs.p1);
          ExpandBounds(gQuadraticAbs.p);
          break;
        //-----
        case SVGPathElementTypes.LineTo:
          ExpandBounds((uSVGPoint)listObject[i]);
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

    uSVGPoint p1 = new uSVGPoint(orig.x, orig.y).MatrixTransform(matrixTransform);
    uSVGPoint p2 = new uSVGPoint(orig.x + orig.width, orig.y + orig.height).MatrixTransform(matrixTransform);

    return new uSVGRect(p1.x, p1.y, p2.x - p1.x, p2.y - p1.y);
  }
  //================================================================================
  //                                      RENDER
  //--------------------------------------------------------------------------------
  //Method: RenderPolygonElement
  //--------------------------------------------------------------------------------
  private void RenderPolygonElement(uSVGPolygonElement polygonElement, uISVGPathDraw pathDraw) {
    int length = polygonElement.listPoints.Count;
    uSVGPoint[] points = new uSVGPoint[length];

    for(int i = 0; i < length; i++)
      points[i] = polygonElement.listPoints[i].MatrixTransform(matrixTransform);
    pathDraw.Polygon(points);
  }
  //--------------------------------------------------------------------------------
  //Method: RenderPolylineElement
  //--------------------------------------------------------------------------------
  private void RenderPolylineElement(uSVGPolylineElement polylineElement, uISVGPathDraw pathDraw) {
    int length = polylineElement.listPoints.Count;
    uSVGPoint[] points = new uSVGPoint[length];

    for(int i = 0; i < length; i++)
      points[i] = polylineElement.listPoints[i].MatrixTransform(matrixTransform);
    pathDraw.Polyline(points);
  }
  //--------------------------------------------------------------------------------
  //Method: RenderRectElement
  //--------------------------------------------------------------------------------
  private void RenderRectElement(uSVGRectElement rectElement, uISVGPathDraw pathDraw) {
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

      t_rx = (t_rx > (tw * 0.5f - 2f)) ? (tw * 0.5f - 2f) : t_rx;
      t_ry = (t_ry > (th * 0.5f - 2f)) ? (th * 0.5f - 2f) : t_ry;

      float angle = transformAngle;

      uSVGPoint t_p1 = new uSVGPoint(p1.x + t_rx, p1.y).MatrixTransform(matrixTransform);
      uSVGPoint t_p2 = new uSVGPoint(p2.x - t_rx, p2.y).MatrixTransform(matrixTransform);
      uSVGPoint t_p3 = new uSVGPoint(p2.x, p2.y + t_ry).MatrixTransform(matrixTransform);
      uSVGPoint t_p4 = new uSVGPoint(p3.x, p3.y - t_ry).MatrixTransform(matrixTransform);

      uSVGPoint t_p5 = new uSVGPoint(p3.x - t_rx, p3.y).MatrixTransform(matrixTransform);
      uSVGPoint t_p6 = new uSVGPoint(p4.x + t_rx, p4.y).MatrixTransform(matrixTransform);
      uSVGPoint t_p7 = new uSVGPoint(p4.x, p4.y - t_ry).MatrixTransform(matrixTransform);
      uSVGPoint t_p8 = new uSVGPoint(p1.x, p1.y + t_ry).MatrixTransform(matrixTransform);

      pathDraw.RoundedRect(t_p1, t_p2, t_p3, t_p4, t_p5, t_p6, t_p7, t_p8, t_rx, t_ry, angle);
    }
  }
  //--------------------------------------------------------------------------------
  //Method: RenderCircleElement
  //--------------------------------------------------------------------------------
  private void RenderCircleElement(uSVGCircleElement elem, uISVGPathDraw pathDraw) {
    uSVGPoint p = new uSVGPoint(elem.cx.value, elem.cy.value).MatrixTransform(matrixTransform);
    pathDraw.Circle(p, elem.r.value);
  }
  //--------------------------------------------------------------------------------
  //Method: RenderEllipseElement
  //--------------------------------------------------------------------------------
  private void RenderEllipseElement(uSVGEllipseElement elem, uISVGPathDraw pathDraw) {
    uSVGPoint p = new uSVGPoint(elem.cx.value, elem.cy.value).MatrixTransform(matrixTransform);
    pathDraw.Ellipse(p, elem.rx.value, elem.ry.value, transformAngle);
  }
  //--------------------------------------------------------------------------------
  //Method: RenderGCircle
  //--------------------------------------------------------------------------------
  private void RenderCircleTo(uSVGGCircle circle, uISVGPathDraw pathDraw) {
    pathDraw.CircleTo(circle.point.MatrixTransform(matrixTransform), circle.r);
  }
  //--------------------------------------------------------------------------------
  //Method: RenderGEllipse
  //--------------------------------------------------------------------------------
  private void RenderEllipseTo(uSVGGEllipse ellipse, uISVGPathDraw pathDraw) {
    pathDraw.EllipseTo(ellipse.point.MatrixTransform(matrixTransform), ellipse.r1, ellipse.r2, transformAngle + ellipse.angle);
  }
  //--------------------------------------------------------------------------------
  //Method: RenderMoveTo
  //--------------------------------------------------------------------------------
  private void RenderMoveTo(uSVGPoint p, uISVGPathDraw pathDraw) {
    pathDraw.MoveTo(p.MatrixTransform(matrixTransform));
  }
  //--------------------------------------------------------------------------------
  //Method: RenderArcTo
  //--------------------------------------------------------------------------------
  private void RenderArcTo(uSVGGArcAbs arc, uISVGPathDraw pathDraw) {
    pathDraw.ArcTo(arc.r1, arc.r2, transformAngle + arc.angle, arc.largeArcFlag, arc.sweepFlag, arc.point.MatrixTransform(matrixTransform));
  }
  //--------------------------------------------------------------------------------
  //Method: RenderCubicCurveTo
  //--------------------------------------------------------------------------------
  private void RenderCubicCurveTo(uSVGGCubicAbs curve, uISVGPathDraw pathDraw) {
    pathDraw.CubicCurveTo(curve.p1.MatrixTransform(matrixTransform), curve.p2.MatrixTransform(matrixTransform), curve.p.MatrixTransform(matrixTransform));
  }
  //--------------------------------------------------------------------------------
  //Method: RenderQuadraticCurveTo
  //--------------------------------------------------------------------------------
  private void RenderQuadraticCurveTo(uSVGGQuadraticAbs curve, uISVGPathDraw pathDraw) {
    pathDraw.QuadraticCurveTo(curve.p1.MatrixTransform(matrixTransform), curve.p.MatrixTransform(matrixTransform));
  }
  //--------------------------------------------------------------------------------
  //Method: RenderLineTo
  //--------------------------------------------------------------------------------
  private void RenderLineTo(uSVGPoint p, uISVGPathDraw pathDraw) {
    pathDraw.LineTo(p.MatrixTransform(matrixTransform));
  }
  //================================================================================
  //--------------------------------------------------------------------------------
  //Method: RenderPath
  //--------------------------------------------------------------------------------
  public void RenderPath(uISVGPathDraw pathDraw, bool isClose) {
    for(int i = 0; i < listObject.Count; i++) {
      switch((SVGPathElementTypes)listType[i]) {
        case SVGPathElementTypes.Polygon:
          RenderPolygonElement((uSVGPolygonElement)listObject[i], pathDraw);
          isClose = false;
          break;
        case SVGPathElementTypes.Polyline:
          RenderPolylineElement((uSVGPolylineElement)listObject[i],pathDraw);
          break;
        case SVGPathElementTypes.Rect:
          RenderRectElement((uSVGRectElement)listObject[i], pathDraw);
          isClose = false;
          break;
        case SVGPathElementTypes.Circle:
          RenderCircleElement((uSVGCircleElement)listObject[i], pathDraw);
          isClose = false;
          break;
        case SVGPathElementTypes.Ellipse:
          RenderEllipseElement((uSVGEllipseElement)listObject[i], pathDraw);
          isClose = false;
          break;
        //-----
        case SVGPathElementTypes.CircleTo:
          RenderCircleTo((uSVGGCircle)listObject[i], pathDraw);
          isClose = false;
          break;
        //-----
        case SVGPathElementTypes.EllipseTo:
          RenderEllipseTo((uSVGGEllipse)listObject[i], pathDraw);
          isClose = false;
          break;
        //-----
        case SVGPathElementTypes.MoveTo:
          RenderMoveTo((uSVGPoint)listObject[i], pathDraw);
          break;
        case SVGPathElementTypes.ArcTo:
          RenderArcTo((uSVGGArcAbs)listObject[i], pathDraw);
          break;
        case SVGPathElementTypes.CubicCurveTo:
          RenderCubicCurveTo((uSVGGCubicAbs)listObject[i], pathDraw);
          break;
        case SVGPathElementTypes.QuadraticCurveTo:
          RenderQuadraticCurveTo((uSVGGQuadraticAbs)listObject[i], pathDraw);
          break;
        case SVGPathElementTypes.LineTo:
          RenderLineTo((uSVGPoint)listObject[i], pathDraw);
          break;
      }
    }

    if(isClose)
      pathDraw.LineTo(beginPoint.MatrixTransform(matrixTransform));
  }
}
