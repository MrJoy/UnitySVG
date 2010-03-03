public class uSVGPathSegCurvetoQuadraticSmoothAbs : uSVGPathSegCurvetoQuadratic, uISVGDrawableSeg  {
	private float m_x = 0f;
	private float m_y = 0f;
	//================================================================================
	public float x {
		get{ return this.m_x;}
	}
	//-----
	public float y {
		get{ return this.m_y;}
	}
	//================================================================================
	public uSVGPathSegCurvetoQuadraticSmoothAbs(float x, float y)
									: base(uSVGPathSegTypes.PATHSEG_CURVETO_QUADRATIC_SMOOTH_ABS) {
		this.m_x = x;
		this.m_y = y;
	}
	//================================================================================
	public override uSVGPoint currentPoint{
		get{
			return new uSVGPoint(this.m_x, this.m_y);
		}
	}
	//-----
	public override uSVGPoint controlPoint1{
		get{
			uSVGPoint m_return = new uSVGPoint(0f,0f);
			uSVGPathSeg m_prevSeg = previousSeg;
			if (m_prevSeg != null) {
				uSVGPoint t_currP = previousPoint;
				uSVGPoint t_prevCP2 = ((uSVGPathSegCurvetoQuadratic)m_prevSeg).controlPoint1;
				uSVGPoint t_P = t_currP - t_prevCP2;		
				m_return = t_currP + t_P;
			}
			return m_return;
		}
	}
	//--------------------------------------------------------------------------------
	//Method: f_Render
	//--------------------------------------------------------------------------------
	public void f_Render (uSVGGraphicsPath m_graphicsPath) {
		uSVGPoint p, p1;
		p = currentPoint;
		p1 = controlPoint1;
		m_graphicsPath.AddQuadraticCurveTo(p1, p);
	}
}