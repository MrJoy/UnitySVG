using UnityEngine;
using System.Collections.Generic;
//Day la Class duoc dung de Fill va Clip
public enum uSVGFillRuleTypes : ushort {
	SVG_NONE_ZERO	= 0,
	SVG_EVEN_ODD	= 1
}
public class uSVGGraphicsPath {
	private uSVGPoint m_beginPoint;
	private uSVGPoint m_endPoint;
	//----
	private uSVGPoint m_boundTopLeft;
	private	uSVGPoint m_boundBottomRight;
	//-----
	private bool needSetFirstPoint;
	//-----
	private uSVGFillRuleTypes m_fillRule = uSVGFillRuleTypes.SVG_NONE_ZERO;
	//-----
	private uSVGTransformList m_transformList;
	private uSVGMatrix m_matrixTransform;
	//-----
	private List<object> m_listObject;
	private List<string> m_listType;
	//================================================================================
	public uSVGFillRuleTypes fillRule {
		get{ return this.m_fillRule;}
		set{ this.m_fillRule = value;}
	}
	//-----
	public uSVGMatrix matrixTransform {
		get{
			if (this.m_matrixTransform == null) {
				this.m_matrixTransform = this.m_transformList.Consolidate().matrix;
			}
			return this.m_matrixTransform;
		}
	}
	//-----
	public float transformAngle {
		get{
			float m_angle = 0.0f;
			for (int i = 0; i < this.m_transformList.numberOfItems; i++ ) {
				uSVGTransform m_temp =this.m_transformList.GetItem(i);
				if (m_temp.type == uSVGTransformType.SVG_TRANSFORM_ROTATE)  {
					m_angle += m_temp.angle;
				}
			}
			return m_angle;
		}
	}
	//-----
	public uSVGTransformList transformList {
		get{ return this.m_transformList;}
		set{ this.m_transformList = value;}
	}
	//================================================================================
	public uSVGGraphicsPath() {
		this.m_beginPoint		= new uSVGPoint(0f, 0f);
		this.m_endPoint			= new uSVGPoint(0f, 0f);
		this.needSetFirstPoint	= true;
		this.m_boundTopLeft 	= new uSVGPoint(+10000f, +10000f);
		this.m_boundBottomRight	= new uSVGPoint(-10000f, -10000f);
		this.m_transformList 	= new uSVGTransformList();
		this.m_listObject		= new List<object>();
		this.m_listType			= new List<string>();
		
	}
	//================================================================================

	//--------------------------------------------------------------------------------
	//Method: Reset
	//--------------------------------------------------------------------------------
	public void Reset() {
		this.m_beginPoint		= new uSVGPoint(0f, 0f);
		this.m_endPoint			= new uSVGPoint(0f, 0f);
		this.needSetFirstPoint	= true;
		this.m_boundTopLeft 	= new uSVGPoint(+10000f, +10000f);
		this.m_boundBottomRight	= new uSVGPoint(-10000f, -10000f);
		this.m_fillRule = uSVGFillRuleTypes.SVG_NONE_ZERO;
		this.m_transformList 	= new uSVGTransformList();
		this.m_listObject.Clear();
		this.m_listType.Clear();
	}
	//--------------------------------------------------------------------------------
	//Method: f_ResetLimitPoints
	//--------------------------------------------------------------------------------
  private void f_ResetLimitPoints(float x, float y) {
    if (x < this.m_boundTopLeft.x) this.m_boundTopLeft.x = x;
    if (y < this.m_boundTopLeft.y) this.m_boundTopLeft.y = y;

    if (x > this.m_boundBottomRight.x) this.m_boundBottomRight.x = x;
    if (y > this.m_boundBottomRight.y) this.m_boundBottomRight.y = y;
  }
  //-----
  private void f_ResetLimitPoints(float x, float y, float dx, float dy) {
    if ((x - dx) < this.m_boundTopLeft.x) this.m_boundTopLeft.x = x - dx;
    if ((y - dy) < this.m_boundTopLeft.y) this.m_boundTopLeft.y = y - dy;

    if ((x + dx) > this.m_boundBottomRight.x) this.m_boundBottomRight.x = x + dx;
    if ((y + dy) > this.m_boundBottomRight.y) this.m_boundBottomRight.y = y + dy;
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
		int m_length = points.Count;
		for (int i = 0; i < m_length; i++) {
		  f_ResetLimitPoints(points[i]);
		}
	}

	//--------------------------------------------------------------------------------
	//Method: f_SetFirstPoint
	//--------------------------------------------------------------------------------
	private void f_SetFirstPoint(uSVGPoint p) {
		if (needSetFirstPoint) {
			this.m_beginPoint.SetValue(p);
			this.needSetFirstPoint = false;
		}
	}
	//--------------------------------------------------------------------------------
	//Method: f_SetLastPoint
	//--------------------------------------------------------------------------------
	private void f_SetLastPoint(uSVGPoint p) {
		this.m_endPoint.SetValue(p);
	}
	//================================================================================
	//--------------------------------------------------------------------------------
	//Method: Add
	//--------------------------------------------------------------------------------
	public void Add(uSVGPolygonElement polygonElement) {
		f_SetFirstPoint(polygonElement.listPoints[0]);
		f_SetLastPoint(polygonElement.listPoints[polygonElement.listPoints.Count - 1]);
		
		m_listType.Add("polygon");
		m_listObject.Add(polygonElement);
	}
	//-----
	public void Add(uSVGPolylineElement polylineElement) {
		f_SetFirstPoint(polylineElement.listPoints[0]);
		f_SetLastPoint(polylineElement.listPoints[polylineElement.listPoints.Count - 1]);
		
		m_listType.Add("polyline");
		m_listObject.Add(polylineElement);
	}
	//-----
	public void Add(uSVGRectElement rectElement) {
		f_SetFirstPoint(new uSVGPoint(rectElement.x.value, rectElement.y.value));
		f_SetLastPoint(new uSVGPoint(rectElement.x.value, rectElement.y.value));
		
		m_listType.Add("rect");
		m_listObject.Add(rectElement);
	}
	//-----
	public void Add(uSVGCircleElement circleElement) {
		f_SetFirstPoint(new uSVGPoint(circleElement.cx.value, circleElement.cy.value));
		f_SetLastPoint(new uSVGPoint(circleElement.cx.value, circleElement.cy.value));

		m_listType.Add("circle");
		m_listObject.Add(circleElement);
	}
	//-----
	public void Add(uSVGEllipseElement ellipseElement) {
		f_SetFirstPoint(new uSVGPoint(ellipseElement.cx.value,
															ellipseElement.cy.value));
		f_SetLastPoint(new uSVGPoint(ellipseElement.cx.value,
															ellipseElement.cy.value));
		
		m_listType.Add("ellipse");
		m_listObject.Add(ellipseElement);
	}
	//--------------------------------------------------------------------------------
	//Method: AddCircleTo
	//--------------------------------------------------------------------------------
	public void AddCircleTo(uSVGPoint p, float r) {
		uSVGGCircle gCircle = new uSVGGCircle(p, r);
		m_listType.Add("circleto");
		m_listObject.Add(gCircle);
	}
	//--------------------------------------------------------------------------------
	//Method: AddEllipseTo
	//--------------------------------------------------------------------------------
	public void AddEllipseTo(uSVGPoint p, float r1, float r2, float angle) {
		uSVGGEllipse gEllipse = new uSVGGEllipse(p, r1, r2, angle);
		m_listType.Add("ellipseto");
		m_listObject.Add(gEllipse);
	}
	//--------------------------------------------------------------------------------
	//Method: AddMoveTo
	//--------------------------------------------------------------------------------
	public void AddMoveTo(uSVGPoint p) {
		uSVGPoint t_p;
		t_p = new uSVGPoint(p.x, p.y);

		f_SetFirstPoint(t_p);
		f_SetLastPoint(t_p);
		
		m_listType.Add("moveto");
		m_listObject.Add(t_p);
	}
	//--------------------------------------------------------------------------------
	//Method: AddArcTo
	//--------------------------------------------------------------------------------
	public void AddArcTo(float r1, float r2, float angle,
								bool largeArcFlag, bool sweepFlag, uSVGPoint p) {
		f_SetLastPoint(p);
		uSVGGArcAbs m_svgGArcAbs = new uSVGGArcAbs(r1, r2, angle, largeArcFlag, sweepFlag, p);
		m_listType.Add("arcto");
		m_listObject.Add(m_svgGArcAbs);
	}
	//--------------------------------------------------------------------------------
	//Method: AddCubicCurveTo
	//--------------------------------------------------------------------------------
	public void AddCubicCurveTo(uSVGPoint p1, uSVGPoint p2, uSVGPoint p) {

		f_SetLastPoint(p);
		uSVGGCubicAbs m_svgGCubicAbs = new uSVGGCubicAbs(p1, p2, p);
		m_listType.Add("cubiccurveto");
		m_listObject.Add(m_svgGCubicAbs);
	}
	//--------------------------------------------------------------------------------
	//Method: AddQuadraticCurveTo
	//--------------------------------------------------------------------------------
	public void AddQuadraticCurveTo(uSVGPoint p1, uSVGPoint p) {

		f_SetLastPoint(p);
		uSVGGQuadraticAbs m_svgGQuadraticAbs = new uSVGGQuadraticAbs(p1, p);
		m_listType.Add("quadraticcurveto");
		m_listObject.Add(m_svgGQuadraticAbs);
	}
	//--------------------------------------------------------------------------------
	//Method: AddLineTo
	//--------------------------------------------------------------------------------
	public void AddLineTo(uSVGPoint p) {
		uSVGPoint t_p;
		t_p = new uSVGPoint(p.x, p.y);

		f_SetFirstPoint(t_p);
		f_SetLastPoint(t_p);
		
		m_listType.Add("lineto");
		m_listObject.Add(t_p);
	}
	//================================================================================
	//--------------------------------------------------------------------------------
	//Method: GetBound
	//--------------------------------------------------------------------------------
	public uSVGRect GetBound() {
Profiler.BeginSample("uSVGGraphicsPath/GetBound()");
		float cx, cy, r, rx, ry, x, y, width, height;

		for (int i = 0; i < this.m_listObject.Count; i++ ) {
			switch(this.m_listType[i]) {
				case "polygon":
					uSVGPolygonElement m_polygonElement = m_listObject[i] as uSVGPolygonElement;
					f_ResetLimitPoints(m_polygonElement.listPoints);
				break;
				case "polyline":
					uSVGPolylineElement m_polylineElement = m_listObject[i] as uSVGPolylineElement;
					f_ResetLimitPoints(m_polylineElement.listPoints);
				break;
				case "rect":
					uSVGRectElement m_rectElement = m_listObject[i] as uSVGRectElement;

					x = m_rectElement.x.value;
					y = m_rectElement.y.value;
					width = m_rectElement.width.value;
					height = m_rectElement.height.value;
					f_ResetLimitPoints(x, y);
					f_ResetLimitPoints(x + width, y);
					f_ResetLimitPoints(x + width, y + height);
					f_ResetLimitPoints(x, y + height);
				break;
				case "circle":
					uSVGCircleElement m_circleElement = m_listObject[i] as uSVGCircleElement;

					cx = m_circleElement.cx.value;
					cy = m_circleElement.cy.value;
					r = m_circleElement.r.value;
					f_ResetLimitPoints(cx, cy, r, r);
				break;
				case "ellipse":
					uSVGEllipseElement m_ellipseElement = m_listObject[i] as uSVGEllipseElement;

					cx = m_ellipseElement.cx.value;
					cy = m_ellipseElement.cy.value;
					rx = m_ellipseElement.rx.value;
					ry = m_ellipseElement.ry.value;
					f_ResetLimitPoints(cx, cy, rx, ry);
				break;
				//-----
				case "circleto":
					uSVGGCircle m_circle = m_listObject[i] as uSVGGCircle;

					r = m_circle.r;
					f_ResetLimitPoints(m_circle.point, r, r);
				break;
				//-----
				case "ellipseto":
					uSVGGEllipse m_ellipse = m_listObject[i] as uSVGGEllipse;

					rx = m_ellipse.r1;
					ry = m_ellipse.r2;
					f_ResetLimitPoints(m_ellipse.point, rx, ry);
				break;
				//-----
				case "moveto":
					uSVGPoint m_pointMoveTo = (uSVGPoint)m_listObject[i];

					f_ResetLimitPoints(m_pointMoveTo);
				break;
				//-----
				case "arcto":
					uSVGGArcAbs m_gArcAbs = m_listObject[i] as uSVGGArcAbs;

          r = (int)m_gArcAbs.r1 + (int)m_gArcAbs.r2;
					f_ResetLimitPoints(m_gArcAbs.point, r, r);
				break;
				//-----
				case "cubiccurveto":
					uSVGGCubicAbs m_gCubicAbs = m_listObject[i] as uSVGGCubicAbs;

					f_ResetLimitPoints(m_gCubicAbs.p1);
					f_ResetLimitPoints(m_gCubicAbs.p2);
					f_ResetLimitPoints(m_gCubicAbs.p);
				break;
				//-----
				case "quadraticcurveto":
					uSVGGQuadraticAbs m_gQuadraticAbs = m_listObject[i] as uSVGGQuadraticAbs;

					f_ResetLimitPoints(m_gQuadraticAbs.p1);
					f_ResetLimitPoints(m_gQuadraticAbs.p);
				break;
				//-----
				case "lineto":
					uSVGPoint m_pointlineTo = (uSVGPoint)m_listObject[i];

					f_ResetLimitPoints(m_pointlineTo);
				break;
			}
		}

		uSVGRect tmp = new uSVGRect(this.m_boundTopLeft.x - 1,
							this.m_boundTopLeft.y - 1,
							this.m_boundBottomRight.x - this.m_boundTopLeft.x + 2,
							this.m_boundBottomRight.y - this.m_boundTopLeft.y + 2);
Profiler.EndSample();
    return tmp;
	}
	//--------------------------------------------------------------------------------
	//Method: GetBoundTransformed
	//--------------------------------------------------------------------------------
	public uSVGRect GetBoundTransformed() {
		uSVGRect m_orginalBound = GetBound();

		uSVGPoint m_point1 = new uSVGPoint(0f, 0f);
		m_point1.x = m_orginalBound.x;
		m_point1.y = m_orginalBound.y;
		m_point1 = m_point1.MatrixTransform(this.matrixTransform);
		
		uSVGPoint m_point2 = new uSVGPoint(0f, 0f);
		m_point2.x = m_orginalBound.x + m_orginalBound.width;
		m_point2.y = m_orginalBound.y + m_orginalBound.height;
		m_point2 = m_point2.MatrixTransform(this.matrixTransform);

		return new uSVGRect(m_point1.x,
							m_point1.y,
							m_point2.x - m_point1.x,
							m_point2.y - m_point1.y);
	}
	//================================================================================
	//                                      RENDER
	//--------------------------------------------------------------------------------
	//Method: f_RenderPolygonElement
	//--------------------------------------------------------------------------------
	private void f_RenderPolygonElement(uSVGPolygonElement polygonElement,
																	uISVGPathDraw m_pathDraw) {
		int m_length = polygonElement.listPoints.Count;
		uSVGPoint[] m_points = new uSVGPoint[m_length];
		
		for (int i = 0; i < m_length; i++ ) {
			m_points[i] = polygonElement.listPoints[i].MatrixTransform(this.matrixTransform);
		}
		m_pathDraw.Polygon(m_points);
	}
	//--------------------------------------------------------------------------------
	//Method: f_RenderPolylineElement
	//--------------------------------------------------------------------------------
	private void f_RenderPolylineElement(uSVGPolylineElement polylineElement, 
																	uISVGPathDraw m_pathDraw) {
		int m_length = polylineElement.listPoints.Count;
		uSVGPoint[] m_points = new uSVGPoint[m_length];
		
		for (int i = 0; i < m_length; i++ ) {
			m_points[i] = polylineElement.listPoints[i].MatrixTransform(this.matrixTransform);
		}
		m_pathDraw.Polyline(m_points);
	}
	//--------------------------------------------------------------------------------
	//Method: f_RenderRectElement
	//--------------------------------------------------------------------------------
	private void f_RenderRectElement(uSVGRectElement m_rectElement,
																	uISVGPathDraw m_pathDraw) {
		uSVGPoint p1, p2, p3, p4;
		float tx = m_rectElement.x.value;
		float ty = m_rectElement.y.value;
		float tw = m_rectElement.width.value;
		float th = m_rectElement.height.value;
		p1 = new uSVGPoint(tx, ty);
		p2 = new uSVGPoint(tx + tw, ty);
		p3 = new uSVGPoint(tx + tw, ty + th);
		p4 = new uSVGPoint(tx, ty + th);

		if (m_rectElement.rx.value == 0.0f && m_rectElement.ry.value == 0.0f ) {
			p1 = p1.MatrixTransform(this.matrixTransform);
			p2 = p2.MatrixTransform(this.matrixTransform);
			p3 = p3.MatrixTransform(this.matrixTransform);
			p4 = p4.MatrixTransform(this.matrixTransform);
			
			m_pathDraw.Rect(p1, p2, p3, p4);
		} else {
			float t_rx = m_rectElement.rx.value;
			float t_ry = m_rectElement.ry.value;
			t_rx = (t_rx == 0.0f) ? t_ry : t_rx;
			t_ry = (t_ry == 0.0f) ? t_rx : t_ry;
			
			t_rx = (t_rx > (tw / 2f -2f))?
									(tw / 2f - 2f):t_rx;
			t_ry = (t_ry > (th / 2f - 2f))?
									(th / 2f - 2f):t_ry;
			
			uSVGPoint t_p1, t_p2, t_p3, t_p4, t_p5, t_p6, t_p7, t_p8;
			float m_angle = this.transformAngle;
			
			t_p1 = new uSVGPoint(p1.x + t_rx, p1.y);
			t_p2 = new uSVGPoint(p2.x - t_rx, p2.y);
			t_p3 = new uSVGPoint(p2.x, p2.y + t_ry);
			t_p4 = new uSVGPoint(p3.x, p3.y - t_ry);

			t_p5 = new uSVGPoint(p3.x - t_rx, p3.y);			
			t_p6 = new uSVGPoint(p4.x + t_rx, p4.y);
			t_p7 = new uSVGPoint(p4.x, p4.y - t_ry);
			t_p8 = new uSVGPoint(p1.x, p1.y + t_ry);
			
			t_p1 = t_p1.MatrixTransform(this.matrixTransform);
			t_p2 = t_p2.MatrixTransform(this.matrixTransform);
			t_p3 = t_p3.MatrixTransform(this.matrixTransform);
			t_p4 = t_p4.MatrixTransform(this.matrixTransform);
			t_p5 = t_p5.MatrixTransform(this.matrixTransform);
			t_p6 = t_p6.MatrixTransform(this.matrixTransform);
			t_p7 = t_p7.MatrixTransform(this.matrixTransform);
			t_p8 = t_p8.MatrixTransform(this.matrixTransform);
			
			m_pathDraw.RoundedRect(t_p1, t_p2, t_p3, t_p4, t_p5, t_p6, t_p7, t_p8, t_rx, t_ry, m_angle);
		}
	}
	//--------------------------------------------------------------------------------
	//Method: f_RenderCircleElement
	//--------------------------------------------------------------------------------
	private void f_RenderCircleElement(uSVGCircleElement m_circleElement,
																uISVGPathDraw m_pathDraw) {
		uSVGPoint p;
		p = new uSVGPoint(m_circleElement.cx.value, m_circleElement.cy.value);
		p = p.MatrixTransform(this.matrixTransform);
		m_pathDraw.Circle(p, m_circleElement.r.value);
	}
	//--------------------------------------------------------------------------------
	//Method: f_RenderEllipseElement
	//--------------------------------------------------------------------------------
	private void f_RenderEllipseElement(uSVGEllipseElement m_ellipseElement,
																	uISVGPathDraw m_pathDraw) {
		uSVGPoint p;
		p = new uSVGPoint(m_ellipseElement.cx.value, m_ellipseElement.cy.value);
		p = p.MatrixTransform(this.matrixTransform);
		float m_angle = this.transformAngle;
		m_pathDraw.Ellipse(p, m_ellipseElement.rx.value,
								m_ellipseElement.ry.value, m_angle);
					
	}
	//--------------------------------------------------------------------------------
	//Method: f_RenderGCircle
	//--------------------------------------------------------------------------------
	private void f_RenderCircleTo(uSVGGCircle m_circle, uISVGPathDraw m_pathDraw) {
		uSVGPoint p;
		p = new uSVGPoint(m_circle.point.x, m_circle.point.y);
		p = p.MatrixTransform(this.matrixTransform);
		m_pathDraw.CircleTo(p, m_circle.r);
	}
	//--------------------------------------------------------------------------------
	//Method: f_RenderGEllipse
	//--------------------------------------------------------------------------------
	private void f_RenderEllipseTo(uSVGGEllipse m_ellipse, uISVGPathDraw m_pathDraw) {
		uSVGPoint p;
		p = new uSVGPoint(m_ellipse.point.x, m_ellipse.point.y);
		p = p.MatrixTransform(this.matrixTransform);
		float m_angle = this.transformAngle + m_ellipse.angle;
		m_pathDraw.EllipseTo(p, m_ellipse.r1, m_ellipse.r2, m_angle);
	}
	//--------------------------------------------------------------------------------
	//Method: f_RenderMoveTo
	//--------------------------------------------------------------------------------
	private void f_RenderMoveTo(uSVGPoint p, uISVGPathDraw m_pathDraw) {
		uSVGPoint tp;
		tp = new uSVGPoint(p.x, p.y);
		tp = tp.MatrixTransform(this.matrixTransform);
		m_pathDraw.MoveTo(tp);				
	}
	//--------------------------------------------------------------------------------
	//Method: f_RenderArcTo
	//--------------------------------------------------------------------------------
	private void f_RenderArcTo(uSVGGArcAbs gArcAbs, uISVGPathDraw m_pathDraw) {
		
		uSVGPoint p;
		p = new uSVGPoint(gArcAbs.point.x, gArcAbs.point.y);
		p = p.MatrixTransform(this.matrixTransform);

		float m_angle = this.transformAngle + gArcAbs.angle;

		m_pathDraw.ArcTo(gArcAbs.r1, gArcAbs.r2, m_angle, 
						gArcAbs.largeArcFlag, gArcAbs.sweepFlag, p);				
	}
	//--------------------------------------------------------------------------------
	//Method: f_RenderCubicCurveTo
	//--------------------------------------------------------------------------------
	private void f_RenderCubicCurveTo(uSVGGCubicAbs gCubicAbs, uISVGPathDraw m_pathDraw) {
		
		uSVGPoint p1, p2, p;
		p1 = new uSVGPoint(0f, 0f); p1.SetValue(gCubicAbs.p1);
		p2 = new uSVGPoint(0f, 0f); p2.SetValue(gCubicAbs.p2);
		p = new uSVGPoint(0f, 0f);  p.SetValue(gCubicAbs.p);
		
		p = p.MatrixTransform(this.matrixTransform);
		p1 = p1.MatrixTransform(this.matrixTransform);
		p2 = p2.MatrixTransform(this.matrixTransform);
		
		m_pathDraw.CubicCurveTo(p1, p2, p);				
	}
	//--------------------------------------------------------------------------------
	//Method: f_RenderQuadraticCurveTo
	//--------------------------------------------------------------------------------
	private void f_RenderQuadraticCurveTo(uSVGGQuadraticAbs gQuadraticAbs, uISVGPathDraw m_pathDraw) {
		
		uSVGPoint p1, p;
		p1 = new uSVGPoint(0f, 0f); p1.SetValue(gQuadraticAbs.p1);
		p = new uSVGPoint(0f, 0f);  p.SetValue(gQuadraticAbs.p);
		
		p = p.MatrixTransform(this.matrixTransform);
		p1 = p1.MatrixTransform(this.matrixTransform);

		m_pathDraw.QuadraticCurveTo(p1, p);				
	}
	//--------------------------------------------------------------------------------
	//Method: f_RenderLineTo
	//--------------------------------------------------------------------------------
	private void f_RenderLineTo(uSVGPoint p, uISVGPathDraw m_pathDraw) {
		uSVGPoint tp;
		tp = new uSVGPoint(0f, 0f);  tp.SetValue(p);
		tp = tp.MatrixTransform(this.matrixTransform);
		m_pathDraw.LineTo(tp);				
	}
	//================================================================================
	//--------------------------------------------------------------------------------
	//Method: RenderPath
	//--------------------------------------------------------------------------------
	public void RenderPath(uISVGPathDraw m_pathDraw, bool isClose) {
		for (int i = 0; i < this.m_listObject.Count; i++ ) {
			switch(this.m_listType[i]) {
				case "polygon":
					f_RenderPolygonElement((m_listObject[i] as uSVGPolygonElement), m_pathDraw);
					isClose = false;
				break;
				case "polyline":
					f_RenderPolylineElement((m_listObject[i] as uSVGPolylineElement),m_pathDraw);
				break;
				case "rect":
					f_RenderRectElement((m_listObject[i] as uSVGRectElement), m_pathDraw);
					isClose = false;
				break;
				case "circle":
					f_RenderCircleElement((m_listObject[i] as uSVGCircleElement), m_pathDraw);
					isClose = false;
				break;
				case "ellipse":
					f_RenderEllipseElement((m_listObject[i] as uSVGEllipseElement), m_pathDraw);
					isClose = false;
				break;
				//-----
				case "circleto":
					f_RenderCircleTo((m_listObject[i] as uSVGGCircle), m_pathDraw);
					isClose = false;
				break;
				//-----
				case "ellipseto":
					f_RenderEllipseTo((m_listObject[i] as uSVGGEllipse), m_pathDraw);
					isClose = false;
				break;
				//-----
				case "moveto":
					f_RenderMoveTo((uSVGPoint)m_listObject[i], m_pathDraw);
				break;
				case "arcto":
					f_RenderArcTo((m_listObject[i] as uSVGGArcAbs), m_pathDraw);
				break;
				case "cubiccurveto":
					f_RenderCubicCurveTo((m_listObject[i] as uSVGGCubicAbs), m_pathDraw);
				break;
				case "quadraticcurveto":
					f_RenderQuadraticCurveTo((m_listObject[i] as uSVGGQuadraticAbs), m_pathDraw);
				break;
				case "lineto":
					f_RenderLineTo((uSVGPoint)m_listObject[i], m_pathDraw);
				break;
			}
		}
		
		if (isClose) {
			uSVGPoint tp = new uSVGPoint(this.m_beginPoint.x, this.m_beginPoint.y);
			tp = tp.MatrixTransform(this.matrixTransform);
			m_pathDraw.LineTo(tp);
		}
	}
}
