using UnityEngine;
using System.Collections.Generic;

public delegate void SetPixelDelegate(int x, int y);

public class uSVGBasicDraw : uISVGBasicDraw {
	//================================================================================
	private struct uSVGPointExt {
		private float m_delta;
		private uSVGPoint m_point;
		public float t {
			get {
				return this.m_delta;
			}
		}
		public uSVGPoint point {
		  get {
		    return this.m_point;
		  }
		}
		public uSVGPointExt(uSVGPoint point, float t) {
		  this.m_point = point;
			this.m_delta = t;
		}
	}
	//================================================================================
	private uSVGPoint m_currentPoint;	
	public SetPixelDelegate SetPixel;
	//================================================================================
	public uSVGPoint currentPoint {
		get{ return this.m_currentPoint;}
	}
	public SetPixelDelegate SetPixelMethod {
		set{ SetPixel = value;}
	}
	//================================================================================
	public uSVGBasicDraw () {
		this.m_currentPoint = new uSVGPoint(0f, 0f);
	}
	//================================================================================
	//--------------------------------------------------------------------------------
	//Methods: Swap
	//--------------------------------------------------------------------------------
	private static void Swap<T>(ref T x1, ref T x2) {
		T temp;
		temp = x1;
		x1 = x2;
		x2 = temp;	
	}
	//--------------------------------------------------------------------------------
	//Methods: MoveTo
	//--------------------------------------------------------------------------------
	public void MoveTo(float x, float y) {
		this.m_currentPoint.x = x;
		this.m_currentPoint.y = y;
	}
	//-----
	public void MoveTo(uSVGPoint p) {
		this.m_currentPoint.SetValue(p);
	}
	//--------------------------------------------------------------------------------
	//Methods: Line
	//--------------------------------------------------------------------------------
	public void Line(int x0, int y0, int x1, int y1) {
		bool steep = (Mathf.Abs(y1 - y0) > Mathf.Abs(x1 - x0));
		if (steep) {
			Swap(ref x0, ref y0);
			Swap(ref x1, ref  y1);
		}

		if (x0 > x1) {
			Swap(ref x0, ref x1);
			Swap(ref y0, ref y1);
		}

		int deltax = x1 - x0;
		int deltay = Mathf.Abs(y1 - y0);
		int error = -(deltax + 1) / 2;
		int ystep;
		int y = y0;
		if (y0 < y1) {
			ystep = 1;
		} else {
			ystep = -1;
		}

		for (int x = x0; x <= x1; x++) {
			if (steep) {
				SetPixel(y, x);
			} else {
				SetPixel(x, y);
			}
			error += deltay;
			if (error >= 0) {
				y += ystep;
				error -= deltax;
			}
		}
	}
	//-----
	public void Line(float x0, float y0, float x1, float y1) {
		Line((int)x0, (int)y0, (int)x1, (int)y1);		
	}
	//-----
	public void Line(uSVGPoint p1, uSVGPoint p2) {
		Line(p1.x, p1.y, p2.x, p2.y);
	}
	//--------------------------------------------------------------------------------
	//Methods: LineTo
	//--------------------------------------------------------------------------------
	public void LineTo(float x, float y) {
		uSVGPoint temp = new uSVGPoint(x, y);		
		Line(this.m_currentPoint, temp);
		this.m_currentPoint.SetValue(temp);
	}
	public void LineTo(uSVGPoint p) {
		Line(this.m_currentPoint, p);
		this.m_currentPoint.SetValue(p);
	}
	//--------------------------------------------------------------------------------
	//Methods: Rect
	//--------------------------------------------------------------------------------
	public void Rect(float x0, float y0, float x1, float y1) {
		MoveTo(x0, y0);
		LineTo(x1, y0); MoveTo(x1, y0);
		LineTo(x1, y1); MoveTo(x1, y1);
		LineTo(x0, y1); MoveTo(x0, y1);
		LineTo(x0, y0);
		
	}
	//-----
	public void Rect(uSVGPoint p1, uSVGPoint p2) {
		Rect(p1.x, p1.y, p2.x, p2.y);
	}
	//-----
	public void Rect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4) {
		MoveTo(p1);
		LineTo(p2);LineTo(p3);LineTo(p4);LineTo(p1);
	}
	//--------------------------------------------------------------------------------
	//Methods: Circle
	//--------------------------------------------------------------------------------
	private void Circle(int x0, int y0, float radius) {

		float chuvi = 2f * Mathf.PI * radius;
		int m_delta = (int) (chuvi / 2f);
		if (m_delta > 50) m_delta = 50; 
		float m_angle = (2 * Mathf.PI) / (float)m_delta;

		float tx, ty, temp;
		tx = x0;
		ty = radius + y0;
			
		uSVGPoint fPoint = new uSVGPoint(tx, ty);
		MoveTo(fPoint);
		for (int i = 1; i <= m_delta; i++) {
			temp = i * m_angle;
			tx = radius * Mathf.Sin(temp) + x0;
			ty = radius * Mathf.Cos(temp) + y0;
			uSVGPoint tPoint = new uSVGPoint(tx, ty);
			LineTo(tPoint);
		}
		LineTo(fPoint);
	}
	//-----
	public void Circle(float x0, float y0, float r) {
		Circle((int)x0, (int)y0, r);
	}
	//-----
	public void Circle(uSVGPoint p, float r) {
		Circle((int)p.x, (int)p.y, r);
	}
	//--------------------------------------------------------------------------------
	//Methods: Ellipse
	//--------------------------------------------------------------------------------
	private void Ellipse(int cx, int cy, int rx, int ry, float angle) {
		float chuvi = 2f * Mathf.PI * Mathf.Sqrt(rx * rx + ry*ry);
		int steps = (int) (chuvi / 3f);
		if (steps > 50) steps = 50;
		float beta = (float)angle / 180.0f * Mathf.PI;
		float sinbeta = Mathf.Sin(beta);
		float cosbeta = Mathf.Cos(beta);
		
		steps = 360 / steps;
		
		int i = 0;
		float alpha = (float)i / 180.0f * Mathf.PI;
		float sinalpha = Mathf.Sin(alpha);
		float cosalpha = Mathf.Cos(alpha);

		float m_x = cx + (rx * cosalpha * cosbeta - ry * sinalpha * sinbeta);
		float m_y = cy + (rx * cosalpha * sinbeta + ry * sinalpha * cosbeta);
		
		float m_fPointx = m_x;
		float m_fPointy = m_y;
		MoveTo(m_x, m_y);

		for (i = 1; i < 360; i += steps) {
			alpha = (float)i / 180.0f * Mathf.PI;
			sinalpha = Mathf.Sin(alpha);
			cosalpha = Mathf.Cos(alpha);

			m_x = cx + (rx * cosalpha * cosbeta - ry * sinalpha * sinbeta);
			m_y = cy + (rx * cosalpha * sinbeta + ry * sinalpha * cosbeta);
			LineTo(m_x, m_y);
		}
		LineTo(m_fPointx, m_fPointy);
	}
	//-----
	public void Ellipse(float x0, float y0, float rx, float ry, float angle) {
		Ellipse((int)x0, (int)y0, (int)rx, (int)ry, angle);
	}
	//-----
	public void Ellipse(uSVGPoint p, float rx, float ry, float angle) {
		Ellipse((int)p.x, (int)p.y, (int)rx, (int)ry, angle);
	}
	//--------------------------------------------------------------------------------
	//Methods: Arc
	//--------------------------------------------------------------------------------
	public void Arc(uSVGPoint p1, float rx, float ry, float angle,
				bool largeArcFlag, bool sweepFlag,
				uSVGPoint p2) {
		float tx, ty;
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
		
		float p, n, t;
		n = Mathf.Sqrt((ux * ux) + (uy * uy));
		p = ux;
		m_angle = (uy < 0) ? -Mathf.Acos(p/n):Mathf.Acos(p/n);	
		m_angle = m_angle * 180.0f / Mathf.PI;
		m_angle %= 360f;
			
		n =  Mathf.Sqrt((ux * ux + uy * uy) * (vx * vx + vy * vy));
		p = ux * vx + uy * vy;
		t = p/n;
		if ((Mathf.Abs(t) >= 0.99999f)  && (Mathf.Abs(t) < 1.000009f)){
			if (t>0) t = 1f; else t= -1f;
		}
		m_delta = (ux * vy - uy * vx < 0) ? -Mathf.Acos(t) : Mathf.Acos(t);

		m_delta = m_delta * 180.0f / Mathf.PI;

		if (!sweepFlag && m_delta > 0) {
			m_delta -= 360f;
		} else if (sweepFlag && m_delta < 0) {
			m_delta += 360f;
		}
		
		m_delta %= 360f;

		int number = 100;
		float deltaT = m_delta  / number;

		uSVGPoint m_point = new uSVGPoint(0, 0);
		float t_angle;
		for(int i = 0; i <= number; i++) {
			t_angle = (deltaT * i + m_angle) * Mathf.PI / 180.0f;
			m_point.x = m_CosRadian * rx * Mathf.Cos(t_angle) - 
						m_SinRadian * ry * Mathf.Sin(t_angle)
						+ cx; 
			m_point.y = m_SinRadian * rx * Mathf.Cos(t_angle) + 
						m_CosRadian * ry * Mathf.Sin(t_angle)
					+ cy;
			LineTo(m_point);
		}
	}
	//--------------------------------------------------------------------------------
	//Methods: ArcTo
	//--------------------------------------------------------------------------------
	public void ArcTo(float r1, float r2, float angle,
				bool largeArcFlag, bool sweepFlag,
				uSVGPoint p) {
		uSVGPoint m_tempPoint = new uSVGPoint(this.m_currentPoint.x, this.m_currentPoint.y);
		Arc(m_tempPoint, r1, r2, angle, largeArcFlag, sweepFlag, p);
		this.m_currentPoint.SetValue(p);
	}
	//--------------------------------------------------------------------------------
	//Methods: CubicCurve Process
	//--------------------------------------------------------------------------------
	private float BelongPosition(uSVGPoint a, uSVGPoint b, uSVGPoint c) {
		float m_up, m_under, m_r;
		m_up = ((a.y - c.y) * (b.x - a.x)) - ((a.x - c.x) * (b.y - a.y));
		m_under = ((b.x - a.x) * (b.x - a.x)) + ((b.y - a.y) * (b.y - a.y));
		m_r = m_up/m_under;
		return m_r;
	}
	//Caculate Distance from c point to line segment [a,b]
	//return d point is the point on that line segment.
	private int NumberOfLimitForCubic(uSVGPoint a, uSVGPoint b, uSVGPoint c, uSVGPoint d) {
		float m_r1 = BelongPosition(a, d, b);
		float m_r2 = BelongPosition(a, d, c);
		if ((m_r1 * m_r2) > 0) return 0; else return 1;
	}
	private float Distance(uSVGPoint a, uSVGPoint b, uSVGPoint c) {
		float m_up, m_under, m_distance;
		m_up = ((a.y - c.y) * (b.x - a.x)) - ((a.x - c.x) * (b.y - a.y));
		m_under = ((b.x - a.x) * (b.x - a.x)) + ((b.y - a.y) * (b.y - a.y));
		m_distance = Mathf.Abs(m_up / m_under) * Mathf.Sqrt(m_under);
		return m_distance;
	}

	private delegate uSVGPoint EvaluateDelegate(float t, uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4);
	private uSVGPoint EvaluateForCubic(float t, uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4) {
		uSVGPoint m_return = new uSVGPoint(0, 0);
		float b0, b1, b2, b3, b4;
		b0 = (1.0f - t);
		b1 = b0 * b0 * b0;
		b2 = 3 * t * b0 * b0;
		b3 = 3 * t * t * b0;
		b4 = t * t * t;
		m_return.x = b1*p1.x + b2*p2.x + b3*p3.x + b4*p4.x;
		m_return.y = b1*p1.y + b2*p2.y + b3*p3.y + b4*p4.y;
		return m_return;
	}

	private uSVGPoint EvaluateForQuadratic(float t, uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4) {
		uSVGPoint m_return = new uSVGPoint(0, 0);
		float b0, b1, b2, b3;
		b0 = (1.0f - t);
		b1 = b0 * b0;
		b2 = 2 * t * b0;
		b3 = t * t;
		m_return.x = b1*p1.x + b2*p2.x + b3*p3.x;
		m_return.y = b1*p1.y + b2*p2.y + b3*p3.y;
		return m_return;
	}
	
	private void CubicCurve(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, int numberOfLimit, EvaluateDelegate evalFunction) {

		MoveTo(p1); //MoveTo the first Point;
		
		//How many times the curve change form innegative -> negative or vice versa
		int m_limit = numberOfLimit;
		float t1, t2, m_flatness;
		t1 = 0.0f; //t1 is the start point of [0..1].
		t2 = 1.0f; //t2 is the end point of [0..1]
		m_flatness = 1.0f;

		uSVGPointExt m_pStart, m_pEnd, m_pMid;
		m_pStart = new uSVGPointExt (evalFunction(t1, p1, p2, p3, p4), t1);

		m_pEnd = new uSVGPointExt (evalFunction(t2, p1, p2, p3, p4), t2);
		
		// The point on Line Segment[m_pStart, m_pEnd] correlate with m_t
		
		XStack<uSVGPointExt> m_stack = new XStack<uSVGPointExt>();
		m_stack.Push(m_pEnd); //Push End Point into Stack

		//Array of Change Point
		uSVGPointExt[] m_limitList = new uSVGPointExt[m_limit + 1];

		int m_count = 0;
		while (true) {
			m_count ++;
			float m_tm = (t1 + t2)/2; //tm is a middle of t1 .. t2. [t1 .. tm .. t2]
		
			//The point on the Curve correlate with tm
			m_pMid = new uSVGPointExt(evalFunction(m_tm, p1, p2, p3, p4), m_tm);
	
			//Calculate Distance from Middle Point to the Flatnet
			float dist = Distance(	m_pStart.point,
									((uSVGPointExt)m_stack.Peek()).point,
									m_pMid.point);

			//flag = true, Curve Segment must be drawn, else continue calculate other middle point.
			bool flag = false;
			if (dist < m_flatness) {
				int i = 0;
				float mm = 0.0f;
			
				for (i = 0; i < m_limit; i++) {
					mm = (t1 + m_tm) / 2;
	
					uSVGPointExt m_q = new uSVGPointExt(evalFunction(mm, p1, p2, p3, p4), mm);
					m_limitList[i] = m_q;
					dist = Distance(m_pStart.point,
									m_pMid.point,
									m_q.point);
					if (dist >= m_flatness) {
						break;
					} else {
						m_tm = mm;
					}
				}
			
				if (i == m_limit) {
					flag = true;
				} else {
					//Continue calculate the first point has Distance > Flatness
					m_stack.Push(m_pMid);
			
					for (int j = 0; j <= i; j++ ) {
						m_stack.Push(m_limitList[j]);
					}
					t2 = mm;
				}			
			}
		
			if (flag) {
				LineTo(m_pStart.point);
				LineTo(m_pMid.point);
				m_pStart = m_stack.Pop();
		
				if (m_stack.Count == 0) break;
			
				m_pMid = m_stack.Peek();
				t1 = t2;
				t2 = m_pMid.t;
			} else if (t2 > m_tm) {
				//If Distance > Flatness and t1 < tm < t2 then new t2 is tm.
				m_stack.Push(m_pMid);
				t2 = m_tm;
			}
		}
		LineTo(m_pStart.point);
	}
	//--------------------------------------------------------------------------------
	//Methods: CubicCurve
	//--------------------------------------------------------------------------------
	public void CubicCurve(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4) {
		EvaluateDelegate evalFunction = new EvaluateDelegate(EvaluateForCubic);
		int m_temp = NumberOfLimitForCubic(p1, p2, p3, p4);
		CubicCurve(p1, p2, p3, p4, m_temp, evalFunction);
	}
	//--------------------------------------------------------------------------------
	//Methods: CubicCurveTo
	//--------------------------------------------------------------------------------
	public void CubicCurveTo(uSVGPoint p1, uSVGPoint p2, uSVGPoint p) {
		uSVGPoint m_tempPoint = new uSVGPoint(this.m_currentPoint.x, this.m_currentPoint.y);
		CubicCurve(m_tempPoint, p1, p2, p);
		this.m_currentPoint.SetValue(p);
	}
	//--------------------------------------------------------------------------------
	//Methods: QuadraticCurve
	//--------------------------------------------------------------------------------
	public void QuadraticCurve(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3) {
		uSVGPoint p4 = new uSVGPoint(p2.x, p2.y);
		EvaluateDelegate evalFunction = new EvaluateDelegate(EvaluateForQuadratic);
		CubicCurve(p1, p2, p3, p4, 0, evalFunction);
		this.m_currentPoint.SetValue(p3);
	}
	//--------------------------------------------------------------------------------
	//Methods: QuadraticCurveTo
	//--------------------------------------------------------------------------------
	public void QuadraticCurveTo(uSVGPoint p1, uSVGPoint p) {
		uSVGPoint m_tempPoint = new uSVGPoint(this.m_currentPoint.x, this.m_currentPoint.y);
		QuadraticCurve(m_tempPoint, p1, p);
		this.m_currentPoint.SetValue(p);
	}
}