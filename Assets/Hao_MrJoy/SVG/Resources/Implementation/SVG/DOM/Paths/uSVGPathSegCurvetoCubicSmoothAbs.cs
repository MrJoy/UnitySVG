public class uSVGPathSegCurvetoCubicSmoothAbs : uSVGPathSegCurvetoCubic, uISVGDrawableSeg  {
	private float m_x	= 0f;
	private float m_y	= 0f;
	private float m_x2	= 0f;
	private float m_y2	= 0f;
	//================================================================================
	public float x {
		get{ return this.m_x;}
	}
	//-----
	public float y {
		get{ return this.m_y;}
	}
	//-----
	public float x2 {
		get{ return this.m_x2;}
	}
	//-----
	public float y2 {
		get{ return this.m_y2;}
	}
	//================================================================================
	public uSVGPathSegCurvetoCubicSmoothAbs(float x2, float y2, float x, float y)
										: base(uSVGPathSegTypes.PATHSEG_CURVETO_CUBIC_SMOOTH_ABS) {
		this.m_x = x;
		this.m_y = y;
		this.m_x2 = x2;
		this.m_y2 = y2;
	}
	//================================================================================
	public override uSVGPoint currentPoint{
		get{
			uSVGPoint m_return = new uSVGPoint(this.m_x, this.m_y);
			return m_return;
		}
	}
	//-----
	public override uSVGPoint controlPoint1{
		get{
			uSVGPoint m_return = new uSVGPoint(0f,0f);
			uSVGPathSeg m_prevSeg = previousSeg;
			if (m_prevSeg != null) {
				uSVGPoint t_currP = previousPoint;
				uSVGPoint t_prevCP2 = ((uSVGPathSegCurvetoCubic)m_prevSeg).controlPoint2;			
				uSVGPoint t_P = t_currP - t_prevCP2;		
				m_return = t_currP + t_P;
			}
			return m_return;
		}
	}
	//-----
	public override uSVGPoint controlPoint2{
		get{
			return new uSVGPoint(this.m_x2, this.m_y2);
		}
	}
	//--------------------------------------------------------------------------------
	//Method: f_Render
	//--------------------------------------------------------------------------------
	public void f_Render (uSVGGraphicsPath m_graphicsPath) {
		uSVGPoint p, p1, p2;
		p1 = controlPoint1;
		p2 = controlPoint2;
		p = currentPoint;
		m_graphicsPath.AddCubicCurveTo(p1, p2, p);
	}
}