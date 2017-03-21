using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public enum SVGFillRule : ushort {
  NoneZero = 0,
  EvenOdd = 1
}

// TODO: Find a way to break this the hell up.

// TODO: Normalize conventions away from Java-style SetX to properties.
public class SVGGraphicsPath {
  private Vector2 beginPoint;

  private Vector2 boundUL, boundBR;

  private bool needSetFirstPoint;

  private Matrix2x3 _matrixTransform;

  private readonly List<ISVGPathSegment> listObject = new List<ISVGPathSegment>();

  public SVGFillRule fillRule = SVGFillRule.NoneZero;

  public Matrix2x3 matrixTransform {
    get {
      if(_matrixTransform == null)
        _matrixTransform = transformList.Consolidate().matrix;
      return _matrixTransform;
    }
  }

  public float transformAngle {
    get {
      float angle = 0.0f;
      for(int i = 0; i < transformList.Count; ++i) {
        SVGTransform temp = transformList[i];
        if(temp.type == SVGTransformMode.Rotate)
          angle += temp.angle;
      }
      return angle;
    }
  }

  public SVGTransformList transformList { get; set; }

  public SVGGraphicsPath() {
    beginPoint = new Vector2(0f, 0f);
    needSetFirstPoint = true;
    boundUL = new Vector2(+10000f, +10000f);
    boundBR = new Vector2(-10000f, -10000f);
    transformList = new SVGTransformList();
  }

  public void Reset() {
    beginPoint = new Vector2(0f, 0f);
    needSetFirstPoint = true;
    boundUL = new Vector2(+10000f, +10000f);
    boundBR = new Vector2(-10000f, -10000f);
    fillRule = SVGFillRule.NoneZero;
    transformList.Clear();
    listObject.Clear();
  }

  public void ExpandBounds(float x, float y) {
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

  public void ExpandBounds(Vector2 point) {
    ExpandBounds(point.x, point.y);
  }

  public void ExpandBounds(Vector2 point, float deltax, float deltay) {
    ExpandBounds(point.x, point.y, deltax, deltay);
  }

  public void ExpandBounds(List<Vector2> points) {
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
    listObject.Add(new SVGGPolygon(polygonElement.listPoints));
  }

  public void Add(SVGPolylineElement polylineElement) {
    SetFirstPoint(polylineElement.listPoints[0]);
    listObject.Add(new SVGGPolyLine(polylineElement.listPoints));
  }

  public void Add(SVGRectElement rectElement) {
    SVGGRect rect = new SVGGRect(rectElement.x.value, rectElement.y.value, rectElement.width.value,
                                 rectElement.height.value, rectElement.rx.value, rectElement.ry.value);
    SetFirstPoint(new Vector2(rect.x, rect.y));
    listObject.Add(rect);
  }

  public void Add(SVGCircleElement circleElement) {
    Vector2 p = new Vector2(circleElement.cx.value, circleElement.cy.value);
    SetFirstPoint(p);
    listObject.Add(new SVGGCircle(p, circleElement.r.value));
  }

  public void Add(SVGEllipseElement ellipseElement) {
    Vector2 p = new Vector2(ellipseElement.cx.value, ellipseElement.cy.value);
    SetFirstPoint(p);
    listObject.Add(new SVGGEllipse(p, ellipseElement.rx.value, ellipseElement.ry.value, 0));
  }

  public void AddCircleTo(Vector2 p, float r) {
    listObject.Add(new SVGGCircle(p, r));
  }

  public void AddEllipseTo(Vector2 p, float r1, float r2, float angle) {
    listObject.Add(new SVGGEllipse(p, r1, r2, angle));
  }

  public void AddMoveTo(Vector2 p) {
    SetFirstPoint(p);
    listObject.Add(new SVGGMoveTo(p));
  }

  public void AddArcTo(float r1, float r2, float angle, bool largeArcFlag, bool sweepFlag, Vector2 p) {
    SVGGArcAbs svgGArcAbs = new SVGGArcAbs(r1, r2, angle, largeArcFlag, sweepFlag, p);
    listObject.Add(svgGArcAbs);
  }

  public void AddCubicCurveTo(Vector2 p1, Vector2 p2, Vector2 p) {
    SVGGCubicAbs svgGCubicAbs = new SVGGCubicAbs(p1, p2, p);
    listObject.Add(svgGCubicAbs);
  }

  public void AddQuadraticCurveTo(Vector2 p1, Vector2 p) {
    SVGGQuadraticAbs svgGQuadraticAbs = new SVGGQuadraticAbs(p1, p);
    listObject.Add(svgGQuadraticAbs);
  }

  public void AddLineTo(Vector2 p) {
    listObject.Add(new SVGGLineTo(p));
  }

  public Rect GetBound() {
    Profiler.BeginSample("SVGGraphicsPath.GetBound");
    for(int i = 0; i < listObject.Count; ++i) {
      ISVGPathSegment seg = listObject[i];
      seg.ExpandBounds(this);
    }

    Rect tmp = new Rect(boundUL.x - 1, boundUL.y - 1, boundBR.x - boundUL.x + 2, boundBR.y - boundUL.y + 2);
    Profiler.EndSample();
    return tmp;
  }

  public void RenderPath(ISVGPathDraw pathDraw, bool isClose) {
    Profiler.BeginSample("SVGGraphicsPath.RenderPath");
    isClose = !isClose;
    Profiler.BeginSample("SVGGraphicsPath.RenderPath[for...]");
    for(int i = 0; i < listObject.Count; i++) {
      Profiler.BeginSample("SVGGraphicsPath.RenderPath[...each]");
      ISVGPathSegment seg = listObject[i];
      isClose = seg.Render(this, pathDraw) || isClose;
      Profiler.EndSample();
    }
    Profiler.EndSample();

    if(!isClose) {
      Profiler.BeginSample("SVGPathSegment.Render[LineTo]");
      pathDraw.LineTo(matrixTransform.Transform(beginPoint));
      Profiler.EndSample();
    }
    Profiler.EndSample();
  }
}
