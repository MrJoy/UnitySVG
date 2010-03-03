using UnityEngine;
public class uSVGGraphicsStroke : uISVGPathDraw {
	private uSVGGraphics		m_graphics;
	private uSVGBasicDraw		m_basicDraw;
	private float m_width;
	private bool isUseWidth = false;
	
	//================================================================================
	public uSVGGraphicsStroke(uSVGGraphics graphics) {
		this.m_graphics = graphics;

		//Basic Draw
		this.m_basicDraw = new uSVGBasicDraw();
		this.m_basicDraw.SetPixelMethod = new SetPixelDelegate(SetPixel);
	}
	//================================================================================
	//--------------------------------------------------------------------------------
	//SetPixel
	//--------------------------------------------------------------------------------
	private void SetPixel(int x, int y) {
		this.m_graphics.SetPixel(x, y);

	}
	//--------------------------------------------------------------------------------
	//Method: f_StrokeLineCapLeft
	//Ve Line Cap, dau cuoi Left
	//--------------------------------------------------------------------------------
	private void f_StrokeLineCapLeft(uSVGPoint p1, uSVGPoint p2, float width) {
		if ((int)width == 1) return;
		if ((this.m_graphics.strokeLineCap == uSVGStrokeLineCapTypes.UNKNOWN) ||
			(this.m_graphics.strokeLineCap == uSVGStrokeLineCapTypes.BUTT)) return;
		if (((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y)) <= 4f) return;
		if (this.m_graphics.strokeLineCap == uSVGStrokeLineCapTypes.ROUND) {
			this.m_graphics.FillCircle(p1, width/2f);
			return;
		}

		uSVGPoint m_p1 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p2 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p3 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p4 = new uSVGPoint(0f, 0f);

		this.m_graphics.GetThickLine(p1, p2, width,
					ref m_p1, ref m_p2, ref m_p3, ref m_p4);
					
		uSVGPoint t_p1 = new uSVGPoint(0f, 0f);
		uSVGPoint t_p2 = new uSVGPoint(0f, 0f);
		uSVGPoint t_p3 = new uSVGPoint(0f, 0f);
		uSVGPoint t_p4 = new uSVGPoint(0f, 0f);
		
		this.m_graphics.GetThickLine(m_p2, m_p1, width,
					ref t_p1, ref t_p2, ref t_p3, ref t_p4);
		
		uSVGPoint[] points = new uSVGPoint[4];
		points[0] = t_p1;
		points[1] = m_p2;
		points[2] = m_p1;
		points[3] = t_p3;
		this.m_graphics.FillPolygon(points);
	}

	//--------------------------------------------------------------------------------
	//Method: f_StrokeLineCapRight
	//Ve Line Cap, dau cuoi Right
	//--------------------------------------------------------------------------------
	private void f_StrokeLineCapRight(uSVGPoint p1, uSVGPoint p2, float width) {
		if ((int)width == 1) return;
		if ((this.m_graphics.strokeLineCap == uSVGStrokeLineCapTypes.UNKNOWN) ||
			(this.m_graphics.strokeLineCap == uSVGStrokeLineCapTypes.BUTT)) return;

		if (((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y)) <= 4f) return;
		if (this.m_graphics.strokeLineCap == uSVGStrokeLineCapTypes.ROUND) {
			this.m_graphics.FillCircle(p2, width/2f);
			return;
		}

		uSVGPoint m_p1 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p2 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p3 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p4 = new uSVGPoint(0f, 0f);

		this.m_graphics.GetThickLine(p1, p2, width,
					ref m_p1, ref m_p2, ref m_p3, ref m_p4);
					
		uSVGPoint t_p1 = new uSVGPoint(0f, 0f);
		uSVGPoint t_p2 = new uSVGPoint(0f, 0f);
		uSVGPoint t_p3 = new uSVGPoint(0f, 0f);
		uSVGPoint t_p4 = new uSVGPoint(0f, 0f);
		
		this.m_graphics.GetThickLine(m_p4, m_p3, width,
					ref t_p1, ref t_p2, ref t_p3, ref t_p4);
		
		uSVGPoint[] points = new uSVGPoint[4];
		points[0] = m_p4;
		points[1] = t_p2;
		points[2] = t_p4;
		points[3] = m_p3;
		this.m_graphics.FillPolygon(points);
	}
	//--------------------------------------------------------------------------------
	//Method: f_StrokeLineJoin
	//Ve LineJoin
	//--------------------------------------------------------------------------------
	private void f_StrokeLineJoin(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, float width) {
		if ((int)width == 1) return;
		if (((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y)) <= 4f) return;
		if (this.m_graphics.strokeLineJoin == uSVGStrokeLineJoinTypes.ROUND) {
			this.m_graphics.FillCircle(p2, width/2f);
			return;
		}

		if ((this.m_graphics.strokeLineJoin == uSVGStrokeLineJoinTypes.MITER) ||
		(this.m_graphics.strokeLineJoin == uSVGStrokeLineJoinTypes.UNKNOWN)) {
			uSVGPoint m_p1 = new uSVGPoint(0f, 0f);
			uSVGPoint m_p2 = new uSVGPoint(0f, 0f);
			uSVGPoint m_p3 = new uSVGPoint(0f, 0f);
			uSVGPoint m_p4 = new uSVGPoint(0f, 0f);

			this.m_graphics.GetThickLine(p1, p2, width,
					ref m_p1, ref m_p2, ref m_p3, ref m_p4);


			uSVGPoint m_p5 = new uSVGPoint(0f, 0f);
			uSVGPoint m_p6 = new uSVGPoint(0f, 0f);
			uSVGPoint m_p7 = new uSVGPoint(0f, 0f);
			uSVGPoint m_p8 = new uSVGPoint(0f, 0f);

			this.m_graphics.GetThickLine(p2, p3, width,
					ref m_p5, ref m_p6, ref m_p7, ref m_p8);
			
			uSVGPoint m_cp1, m_cp2;		
			m_cp1 = this.m_graphics.GetCrossPoint(m_p1, m_p3, m_p5, m_p7);
			m_cp2 = this.m_graphics.GetCrossPoint(m_p2, m_p4, m_p6, m_p8);

		
			uSVGPoint[] points = new uSVGPoint[8];
			points[0] = p2;
			points[1] = m_p3;
			points[2] = m_cp1;
			points[3] = m_p5;
		
			points[4] = p2;
			points[5] = m_p6;
			points[6] = m_cp2;
			points[7] = m_p4;
			this.m_graphics.FillPolygon(points);
			return;
		}
		if (this.m_graphics.strokeLineJoin == uSVGStrokeLineJoinTypes.BEVEL) {
			uSVGPoint m_p1 = new uSVGPoint(0f, 0f);
			uSVGPoint m_p2 = new uSVGPoint(0f, 0f);
			uSVGPoint m_p3 = new uSVGPoint(0f, 0f);
			uSVGPoint m_p4 = new uSVGPoint(0f, 0f);

			this.m_graphics.GetThickLine(p1, p2, width,
					ref m_p1, ref m_p2, ref m_p3, ref m_p4);


			uSVGPoint m_p5 = new uSVGPoint(0f, 0f);
			uSVGPoint m_p6 = new uSVGPoint(0f, 0f);
			uSVGPoint m_p7 = new uSVGPoint(0f, 0f);
			uSVGPoint m_p8 = new uSVGPoint(0f, 0f);

			this.m_graphics.GetThickLine(p2, p3, width,
					ref m_p5, ref m_p6, ref m_p7, ref m_p8);
			
			uSVGPoint[] points = new uSVGPoint[6];
			points[0] = p2;
			points[1] = m_p3;
			points[2] = m_p5;
		
			points[3] = p2;
			points[4] = m_p6;
			points[5] = m_p4;
			this.m_graphics.FillPolygon(points);
			return;
		}
	}
	//================================================================================
	//--------------------------------------------------------------------------------
	//Methods: MoveTo
	//--------------------------------------------------------------------------------
	public void MoveTo(uSVGPoint p) {
		this.m_basicDraw.MoveTo(p);
	}
	//--------------------------------------------------------------------------------
	//Methods: CircleTo
	//--------------------------------------------------------------------------------
	public void CircleTo(uSVGPoint p, float r) {
		if ((this.isUseWidth) && ((int)this.m_width > 1)) {
			CircleTo(p, r, this.m_width);
			return;
		}
		Circle(p, r);
	}
	//-----
	public void CircleTo(uSVGPoint p, float r, float width) {
		Circle(p, r, width);
	}
	//--------------------------------------------------------------------------------
	//Methods: EllipseTo
	//--------------------------------------------------------------------------------
	public void EllipseTo(uSVGPoint p, float r1, float r2, float angle) {
		if ((this.isUseWidth) && ((int)this.m_width > 1)) {
			EllipseTo(p, r1, r2, this.m_width);
			return;
		}
		Ellipse(p, r1, r2, angle);
	}
	//-----
	public void EllipseTo(uSVGPoint p, float r1, float r2, float angle, float width) {
		Ellipse(p, r1, r2, angle, width);
	}
	//--------------------------------------------------------------------------------
	//Methods: ArcTo
	//--------------------------------------------------------------------------------
	public void ArcTo(float r1, float r2, float angle,
				bool largeArcFlag, bool sweepFlag, uSVGPoint p) {
		if ((this.isUseWidth) && ((int)this.m_width > 1))
			ArcTo(r1, r2, angle, largeArcFlag, sweepFlag, p, this.m_width);
		else
		  this.m_basicDraw.ArcTo(r1, r2, angle, largeArcFlag, sweepFlag, p);
	}
	//-----
	public void ArcTo(float r1, float r2, float angle,
				bool largeArcFlag, bool sweepFlag, uSVGPoint p, float width) {
UnityEngine.Profiler.BeginSample("SVG.SVGGraphicsStroke.ArcTo[0]");
		float tx, ty, rx, ry;
		rx = r1;
		ry = r2;
		uSVGPoint p1 = new uSVGPoint(0f, 0f);
		uSVGPoint p2 = new uSVGPoint(0f, 0f);
		p1.SetValue(this.m_basicDraw.currentPoint);		
		p2.SetValue(p);
		
		double trx2, try2, tx2, ty2;
		float temp1, temp2;
		float m_radian = (angle * Mathf.PI / 180.0f);
		float m_CosRadian = Mathf.Cos(m_radian);
		float m_SinRadian = Mathf.Sin(m_radian);
		temp1 = (p1.x - p2.x)/2.0f;
		temp2 = (p1.y - p2.y)/2.0f;
		tx = ( m_CosRadian * temp1) + (m_SinRadian * temp2);
		ty = (-m_SinRadian * temp1) + (m_CosRadian * temp2);
		
		trx2 = rx * rx;
		try2 = ry * ry;
		tx2 = tx * tx;
		ty2 = ty * ty;
		
		
		double radiiCheck = tx2/trx2 + ty2/try2;
		if (radiiCheck > 1) {
		rx = Mathf.Sqrt((float)radiiCheck) * rx;
		ry = Mathf.Sqrt((float)radiiCheck) * ry;
		trx2 = rx * rx;
		try2 = ry * ry;
		}

		double tm1;
		tm1 = (trx2*try2 - trx2*ty2 - try2*tx2) / (trx2*ty2 + try2*tx2);		
		tm1 = (tm1 < 0) ? 0 : tm1;

		float tm2;
		tm2 = (largeArcFlag == sweepFlag) ? -Mathf.Sqrt((float)tm1) : Mathf.Sqrt((float)tm1);

		float tcx, tcy;
		tcx = tm2 * ((rx * ty) / ry);
		tcy = tm2 * (-(ry * tx) / rx);

		float cx, cy;
		cx = m_CosRadian * tcx - m_SinRadian * tcy + ((p1.x + p2.x) / 2.0f);
		cy = m_SinRadian * tcx + m_CosRadian * tcy + ((p1.y + p2.y) / 2.0f);
		
		float ux = (tx - tcx)/rx;
		float uy = (ty - tcy)/ry;
		float vx = (-tx - tcx)/rx;
		float vy = (-ty - tcy)/ry;		 
		float m_angle, m_delta;
		
		float tp, n;
		n = Mathf.Sqrt((ux * ux) + (uy * uy));
		tp = ux;
		m_angle = (uy < 0) ? -Mathf.Acos(tp/n):Mathf.Acos(tp/n);	
		m_angle = m_angle * 180.0f / Mathf.PI;
		m_angle %= 360f;
			
		n =  Mathf.Sqrt((ux * ux + uy * uy) * (vx * vx + vy * vy));
		tp = ux * vx + uy * vy;
		m_delta = (ux * vy - uy * vx < 0) ? -Mathf.Acos(tp/n):Mathf.Acos(tp/n);
		m_delta = m_delta * 180.0f / Mathf.PI;
		
		if (!sweepFlag && m_delta > 0) {
			m_delta -= 360f;
		} else if (sweepFlag && m_delta < 0) {
			m_delta += 360f;
		}
		
		m_delta %= 360f;

		int number = 50;
		float deltaT = m_delta  / number;
		//---Get Control Point
		uSVGPoint m_controlPoint1 = new uSVGPoint(0f, 0f);
		uSVGPoint m_controlPoint2 = new uSVGPoint(0f, 0f);

		for(int i = 0; i <= number; i++) {
			float t_angle = (deltaT * i + m_angle) * Mathf.PI / 180.0f;
			m_controlPoint1.x = m_CosRadian * rx * Mathf.Cos(t_angle) - 
						m_SinRadian * ry * Mathf.Sin(t_angle)
						+ cx; 
			m_controlPoint1.y = m_SinRadian * rx * Mathf.Cos(t_angle) + 
						m_CosRadian * ry * Mathf.Sin(t_angle)
					+ cy;
			if ((m_controlPoint1.x != p1.x) && (m_controlPoint1.y != p1.y)) {
				i = number + 1;
			}
		}
		
		
		for(int i = number; i >= 0; i--) {
			float t_angle = (deltaT * i + m_angle) * Mathf.PI / 180.0f;
			m_controlPoint2.x = m_CosRadian * rx * Mathf.Cos(t_angle) - 
						m_SinRadian * ry * Mathf.Sin(t_angle)
						+ cx; 
			m_controlPoint2.y = m_SinRadian * rx * Mathf.Cos(t_angle) + 
						m_CosRadian * ry * Mathf.Sin(t_angle)
					+ cy;
			if ((m_controlPoint2.x != p2.x) && (m_controlPoint2.y != p2.y)) {
				i = -1;
			}
		}
		//-----
		uSVGPoint m_p1 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p2 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p3 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p4 = new uSVGPoint(0f, 0f);

		this.m_graphics.GetThickLine(p1, m_controlPoint1, width,
				ref m_p1, ref m_p2, ref m_p3, ref m_p4);
		
		uSVGPoint m_p5 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p6 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p7 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p8 = new uSVGPoint(0f, 0f);

		this.m_graphics.GetThickLine(m_controlPoint2, p2, width,
					ref m_p5, ref m_p6, ref m_p7, ref m_p8);
		
		float m_half, m_ihalf1, m_ihalf2;
		m_half = width / 2f;
		m_ihalf1 = m_half;
		m_ihalf2 = width - m_ihalf1 + 0.5f;
		//-----
		
		float t_len1, t_len2;
		t_len1 = (m_p1.x - cx) * (m_p1.x - cx) + (m_p1.y - cy) * (m_p1.y - cy);
		t_len2 = (m_p2.x - cx) * (m_p2.x - cx) + (m_p2.y - cy) * (m_p2.y - cy);
		
		uSVGPoint tempPoint = new uSVGPoint(0f, 0f);
		if (t_len1 > t_len2) {
			tempPoint.SetValue(m_p1);
			m_p1.SetValue(m_p2);
			m_p2.SetValue(tempPoint);
		}
		
		t_len1 = (m_p7.x - cx) * (m_p7.x - cx) + (m_p7.y - cy) * (m_p7.y - cy);
		t_len2 = (m_p8.x - cx) * (m_p8.x - cx) + (m_p8.y - cy) * (m_p8.y - cy);
		
		if (t_len1 > t_len2) {
			tempPoint.SetValue(m_p7);
			m_p7.SetValue(m_p8);
			m_p8.SetValue(tempPoint);
		}
	
		uSVGGraphicsPath m_graphicsPath = new uSVGGraphicsPath();			
		m_graphicsPath.AddMoveTo(m_p2);
		m_graphicsPath.AddArcTo(r1 + m_ihalf1, r2 + m_ihalf1, angle, largeArcFlag, sweepFlag, m_p8);
		m_graphicsPath.AddLineTo(m_p7);
		m_graphicsPath.AddArcTo(r1 - m_ihalf2, r2 - m_ihalf2, angle, largeArcFlag, !sweepFlag, m_p1);
		m_graphicsPath.AddLineTo(m_p2);
		this.m_graphics.FillPath(m_graphicsPath);

		MoveTo(p);
	}
	//--------------------------------------------------------------------------------
	//Methods: CubicCurveTo
	//--------------------------------------------------------------------------------
	public void CubicCurveTo(uSVGPoint p1, uSVGPoint p2, uSVGPoint p) {
		if ((this.isUseWidth) && ((int)this.m_width > 1)) {
			CubicCurveTo(p1, p2, p, this.m_width);
			return;
		}
		this.m_basicDraw.CubicCurveTo(p1, p2, p);
	}
	//-----
	public void CubicCurveTo(uSVGPoint p1, uSVGPoint p2, uSVGPoint p, float width) {
		uSVGPoint m_point = new uSVGPoint(0f, 0f);
		m_point.SetValue(this.m_basicDraw.currentPoint);
		
		uSVGPoint m_p1 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p2 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p3 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p4 = new uSVGPoint(0f, 0f);

		bool temp;
		temp = this.m_graphics.GetThickLine(m_point, p1, width,
				ref m_p1, ref m_p2, ref m_p3, ref m_p4);
		if (temp == false) {
			QuadraticCurveTo(p2, p, width);
			return;
		}
		
		uSVGPoint m_p5 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p6 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p7 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p8 = new uSVGPoint(0f, 0f);

		this.m_graphics.GetThickLine(p1, p2, width,
					ref m_p5, ref m_p6, ref m_p7, ref m_p8);
		
		uSVGPoint m_p9 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p10 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p11 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p12 = new uSVGPoint(0f, 0f);

		this.m_graphics.GetThickLine(p2, p, width,
					ref m_p9, ref m_p10, ref m_p11, ref m_p12);

		uSVGPoint m_cp1, m_cp2, m_cp3, m_cp4;		
		m_cp1 = this.m_graphics.GetCrossPoint(m_p1, m_p3, m_p5, m_p7);
		m_cp2 = this.m_graphics.GetCrossPoint(m_p2, m_p4, m_p6, m_p8);
		m_cp3 = this.m_graphics.GetCrossPoint(m_p5, m_p7, m_p9, m_p11);
		m_cp4 = this.m_graphics.GetCrossPoint(m_p6, m_p8, m_p10, m_p12);

		
		this.m_basicDraw.MoveTo(m_point);
		this.m_basicDraw.CubicCurveTo(p1, p2, p);
		
		uSVGGraphicsPath m_graphicsPath = new uSVGGraphicsPath();
		m_graphicsPath.AddMoveTo(m_p2);		
		m_graphicsPath.AddCubicCurveTo(m_cp2, m_cp4, m_p12);
		m_graphicsPath.AddLineTo(m_p11);
		m_graphicsPath.AddCubicCurveTo(m_cp3, m_cp1, m_p1);
		m_graphicsPath.AddLineTo(m_p2);
		this.m_graphics.FillPath(m_graphicsPath);

		MoveTo(p);
		
	}
	//--------------------------------------------------------------------------------
	//Methods: QuadraticCurveTo
	//--------------------------------------------------------------------------------
	public void QuadraticCurveTo(uSVGPoint p1, uSVGPoint p) {
		if ((this.isUseWidth) && ((int)this.m_width > 1)) {
			QuadraticCurveTo(p1, p, this.m_width);
			return;
		}
		this.m_basicDraw.QuadraticCurveTo(p1, p);
	}
	//-----
	public void QuadraticCurveTo(uSVGPoint p1, uSVGPoint p, float width) {
		uSVGPoint m_point = new uSVGPoint(0f, 0f);
		m_point.SetValue(this.m_basicDraw.currentPoint);
		
		uSVGPoint m_p1 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p2 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p3 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p4 = new uSVGPoint(0f, 0f);

		this.m_graphics.GetThickLine(m_point, p1, width,
				ref m_p1, ref m_p2, ref m_p3, ref m_p4);

		uSVGPoint m_p5 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p6 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p7 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p8 = new uSVGPoint(0f, 0f);

		this.m_graphics.GetThickLine(p1, p, width,
					ref m_p5, ref m_p6, ref m_p7, ref m_p8);
						
		uSVGPoint m_cp1, m_cp2;		
		m_cp1 = this.m_graphics.GetCrossPoint(m_p1, m_p3, m_p5, m_p7);
		m_cp2 = this.m_graphics.GetCrossPoint(m_p2, m_p4, m_p6, m_p8);

		uSVGGraphicsPath m_graphicsPath = new uSVGGraphicsPath();
		m_graphicsPath.AddMoveTo(m_p2);
		m_graphicsPath.AddQuadraticCurveTo(m_cp2, m_p8);
		m_graphicsPath.AddLineTo(m_p7);
		m_graphicsPath.AddQuadraticCurveTo(m_cp1, m_p1);
		m_graphicsPath.AddLineTo(m_p2);
		this.m_graphics.FillPath(m_graphicsPath);
		
		MoveTo(p);
	}
	//--------------------------------------------------------------------------------
	//Methods: LineTo
	//--------------------------------------------------------------------------------
	public void LineTo(uSVGPoint p) {
		if ((this.isUseWidth) && ((int)this.m_width > 1)) {
			LineTo(p, this.m_width);
			return;
		}
		this.m_basicDraw.LineTo(p);
	}
	//-----
	public void LineTo(uSVGPoint p, float width) {
		uSVGPoint m_point = new uSVGPoint(0f, 0f);
		m_point.SetValue(this.m_basicDraw.currentPoint);
		Line(m_point, p, width);
		MoveTo(p);
	}
	//--------------------------------------------------------------------------------
	//Methods: Line
	//--------------------------------------------------------------------------------
	public void Line(uSVGPoint p1, uSVGPoint p2) {
		if ((this.isUseWidth) && ((int)this.m_width > 1)){
			Line(p1, p2, this.m_width);
			return;
		}
		this.m_basicDraw.Line(p1, p2);
	}
	//-----
	public void Line(uSVGPoint p1, uSVGPoint p2, float width) {
		if ((int)width == 1) {
			Line(p1, p2);
		} else {
			if (((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y)) <= 4f) 
				return;
			f_StrokeLineCapLeft(p1, p2, width);
			f_StrokeLineCapRight(p1, p2, width);
			uSVGPoint m_p1 = new uSVGPoint(0f, 0f);
			uSVGPoint m_p2 = new uSVGPoint(0f, 0f);
			uSVGPoint m_p3 = new uSVGPoint(0f, 0f);
			uSVGPoint m_p4 = new uSVGPoint(0f, 0f);

			this.m_graphics.GetThickLine(p1, p2, width,
					ref m_p1, ref m_p2, ref m_p3, ref m_p4);

			uSVGPoint[] points = new uSVGPoint[4];
			points[0] = m_p1;
			points[1] = m_p3;
			points[2] = m_p4;
			points[3] = m_p2;
			this.m_graphics.FillPolygon(points);
		}
	}
	//--------------------------------------------------------------------------------
	//Method: Rect
	//--------------------------------------------------------------------------------
	public void Rect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4) {
		if ((this.isUseWidth) && ((int)this.m_width > 1)){
			Rect(p1, p2, p3, p4, this.m_width);
			return;
		}
		this.m_basicDraw.Rect(p1, p2, p3, p4);
	}
	//-----
	public void Rect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, float width) {
		if ((int)width == 1) {
			Rect(p1, p2, p3, p4);
		}
		uSVGPoint[] points = new uSVGPoint[4];
		points[0] = p1;
		points[1] = p2;
		points[2] = p3;
		points[3] = p4;
		Polygon(points, width);
	}
	//--------------------------------------------------------------------------------
	//Methods: Rounded Rect
	//--------------------------------------------------------------------------------
	public void RoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
			uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
			float r1, float r2, float angle) {

		if ((this.isUseWidth) && ((int)this.m_width > 1)){
			RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle, this.m_width);
			return;
		}
		this.m_basicDraw.MoveTo(p1);
		this.m_basicDraw.LineTo(p2);
		this.m_basicDraw.ArcTo(r1, r2, angle, false, true, p3);

		this.m_basicDraw.MoveTo(p3);
		this.m_basicDraw.LineTo(p4);
		this.m_basicDraw.ArcTo(r1, r2, angle, false, true, p5);

		this.m_basicDraw.MoveTo(p5);
		this.m_basicDraw.LineTo(p6);
		this.m_basicDraw.ArcTo(r1, r2, angle, false, true, p7);
		
		this.m_basicDraw.MoveTo(p7);
		this.m_basicDraw.LineTo(p8);
		this.m_basicDraw.ArcTo(r1, r2, angle, false, true, p1);
	}
	//-----
	public void RoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
			uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
			float r1, float r2, float angle, float width) {
		
		if ((int)width == 1) {
			RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle);
			return;
		}

		Line(p1, p2, width); Line(p3, p4, width); Line(p5, p6, width); Line(p7, p8, width);
		uSVGPoint m_p1 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p2 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p3 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p4 = new uSVGPoint(0f, 0f);

		this.m_graphics.GetThickLine(p1, p2, width,
					ref m_p1, ref m_p2, ref m_p3, ref m_p4);
					
		uSVGPoint m_p5 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p6 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p7 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p8 = new uSVGPoint(0f, 0f);

		//-------
		this.m_graphics.GetThickLine(p3, p4, width,
					ref m_p5, ref m_p6, ref m_p7, ref m_p8);
		
		uSVGGraphicsPath m_graphicsPath = new uSVGGraphicsPath();

		m_graphicsPath.AddMoveTo(m_p4);
		m_graphicsPath.AddArcTo(r1 + (width/2f), r2 + (width/2f),
									angle, false, true, m_p6);
		m_graphicsPath.AddLineTo(m_p5);
		m_graphicsPath.AddArcTo(r1 - (width/2f), r2 - (width/2f),
									angle, false, false, m_p3);
		m_graphicsPath.AddLineTo(m_p4);
		

		this.m_graphics.FillPath(m_graphicsPath);

		//-------
		this.m_graphics.GetThickLine(p5, p6, width,
					ref m_p1, ref m_p2, ref m_p3, ref m_p4);
		
		m_graphicsPath.Reset();
		m_graphicsPath.AddMoveTo(m_p8);
		m_graphicsPath.AddArcTo(r1 + (width/2f), r2 + (width/2f),
									angle, false, true, m_p2);
		m_graphicsPath.AddLineTo(m_p1);
		m_graphicsPath.AddArcTo(r1 - (width/2f), r2 - (width/2f),
									angle, false, false, m_p7);
		m_graphicsPath.AddLineTo(m_p8);
		
		this.m_graphics.FillPath(m_graphicsPath);

		//----------
		this.m_graphics.GetThickLine(p7, p8, width,
					ref m_p5, ref m_p6, ref m_p7, ref m_p8);
		
		m_graphicsPath.Reset();
		m_graphicsPath.AddMoveTo(m_p4);
		m_graphicsPath.AddArcTo(r1 + (width/2f), r2 + (width/2f),
									angle, false, true, m_p6);
		m_graphicsPath.AddLineTo(m_p5);
		m_graphicsPath.AddArcTo(r1 - (width/2f), r2 - (width/2f),
									angle, false, false, m_p3);
		m_graphicsPath.AddLineTo(m_p4);
		
		this.m_graphics.FillPath(m_graphicsPath);

		//-------
		this.m_graphics.GetThickLine(p1, p2, width,
					ref m_p1, ref m_p2, ref m_p3, ref m_p4);

		m_graphicsPath.Reset();
		m_graphicsPath.AddMoveTo(m_p8);
		m_graphicsPath.AddArcTo(r1 + (width/2f), r2 + (width/2f),
									angle, false, true, m_p2);
		m_graphicsPath.AddLineTo(m_p1);
		m_graphicsPath.AddArcTo(r1 - (width/2f), r2 - (width/2f),
									angle, false, false, m_p7);
		m_graphicsPath.AddLineTo(m_p8);
		
		this.m_graphics.FillPath(m_graphicsPath);
	}
	//--------------------------------------------------------------------------------
	//Methods: Circle
	//--------------------------------------------------------------------------------
	public void Circle(uSVGPoint p, float r) {
		if ((this.isUseWidth) && ((int)this.m_width > 1)){
			Circle(p, r, this.m_width);
			return;
		}
		this.m_basicDraw.Circle(p, r);
	}
	//-----
	public void Circle(uSVGPoint p, float r, float width) {
		if ((int)width == 1) {
			Circle(p, r);
		} else {
			int r1 = (int)(width / 2f);
			int r2 = (int)width - r1;

			uSVGPoint[] m_points = new uSVGPoint[1];
			m_points[0] = new uSVGPoint(p.x, p.y);

			uSVGGraphicsPath m_graphicsPath = new uSVGGraphicsPath();
			m_graphicsPath.AddCircleTo(p, r + r1);
			m_graphicsPath.AddCircleTo(p, r - r2);

			this.m_graphics.FillPath(m_graphicsPath, m_points);
		}
	}
	//--------------------------------------------------------------------------------
	//Methods: Ellipse
	//--------------------------------------------------------------------------------
	public void Ellipse(uSVGPoint p, float rx, float ry, float angle) {
		if ((this.isUseWidth) && ((int)this.m_width > 1)){
			Ellipse(p, rx, ry, angle, this.m_width);
			return;
		}
		this.m_basicDraw.Ellipse(p, rx, ry, angle);
		
	}
	//-----
	public void Ellipse(uSVGPoint p, float rx, float ry, float angle, float width) {
		if ((int)width == 1) {
			Ellipse(p, rx, ry, angle);
		} else {
			int r1 = (int)(width / 2f);
			int r2 = (int)width - r1;
			
			uSVGPoint[] m_points = new uSVGPoint[1];
			m_points[0] = new uSVGPoint(p.x, p.y);

			uSVGGraphicsPath m_graphicsPath = new uSVGGraphicsPath();
			m_graphicsPath.AddEllipseTo(p, rx + r1, ry + r1, angle);
			m_graphicsPath.AddEllipseTo(p, rx - r2, ry - r2, angle);
			
			this.m_graphics.FillPath(m_graphicsPath, m_points);
		}
	}
	//--------------------------------------------------------------------------------
	//Method: Polygon
	//--------------------------------------------------------------------------------
	public void Polygon(uSVGPoint[] points) {

		if ((this.isUseWidth) && ((int)this.m_width > 1)){
			Polygon(points, this.m_width);
			return;
		}

		int m_length = points.GetLength(0);
		if (m_length > 1) {
			this.m_basicDraw.MoveTo(points[0]);
			for(int i = 1; i < m_length; i++ ) {
				this.m_basicDraw.LineTo(points[1]);
			}
			this.m_basicDraw.LineTo(points[0]);
		}
	}
	//-----
	public void Polygon(uSVGPoint[] points, float width) {
		if ((int)width == 1) {
			Polygon(points);
			return;
		}
		int m_length = points.GetLength(0);
		if (m_length > 1) {
			if (m_length == 2) {
				Line(points[0], points[1], width);
				f_StrokeLineCapLeft(points[0], points[1], width);
				f_StrokeLineCapRight(points[0], points[1], width);
			} else if (m_length > 2) {
				f_StrokeLineJoin(points[m_length - 1], points[0], points[1], width);
				Line(points[0], points[1], width);

				f_StrokeLineJoin(points[m_length - 2], points[m_length - 1], points[0], width);
				Line(points[m_length - 1], points[0], width);
				for (int i = 1; i < m_length - 1; i++) {
					f_StrokeLineJoin(points[i-1], points[i], points[i+1], width);
					Line(points[i], points[i+1], width);
				}			
			}
		}
	}
	//--------------------------------------------------------------------------------
	//Methods: Polyline
	//--------------------------------------------------------------------------------
	public void Polyline(uSVGPoint[] points) {
		
		if ((this.isUseWidth) && ((int)this.m_width > 1)){
			Polyline(points, this.m_width);
			return;
		}

		int m_length = points.GetLength(0);
		if (m_length > 1) {
			this.m_basicDraw.MoveTo(points[0]);
			for(int i = 1; i < m_length; i++ ) {
				this.m_basicDraw.LineTo(points[1]);
			}
		}
	}
	//-----
	public void Polyline(uSVGPoint[] points, float width) {		
		if ((int)width == 1) {
			Polygon(points);
			return;
		}
		int m_length = points.GetLength(0);
		if (m_length > 1) {
			if (m_length >= 2) {
				Line(points[0], points[1], width);
				f_StrokeLineCapLeft(points[0], points[1], width);					
				f_StrokeLineCapRight(points[m_length - 2], points[m_length - 1], width);

				for (int i = 1; i < m_length - 1; i++) {
					f_StrokeLineJoin(points[i-1], points[i], points[i+1], width);
					Line(points[i], points[i+1], width);
				}
			}
		}
	}
	//================================================================================
	//--------------------------------------------------------------------------------
	//Methods: DrawPath
	//--------------------------------------------------------------------------------
	public void DrawPath(uSVGGraphicsPath graphicsPath) {
		graphicsPath.RenderPath(this, false);
	}
	//-----
	//Fill co Stroke trong do luon
	public void DrawPath(uSVGGraphicsPath graphicsPath, float width) {
		this.m_width = width;
		this.isUseWidth = true;
		graphicsPath.RenderPath(this, false);
		this.isUseWidth = false;
	}
}
