public class uSVGPathSegCurvetoCubicRel : uSVGPathSegCurvetoCubic, uISVGDrawableSeg  {
	private float m_x	= 0f;
	private float m_y	= 0f;
	private float m_x1	= 0f;
	private float m_y1	= 0f;
	private float m_x2	= 0f;
	private float m_y2	= 0f;
	//================================================================================
	public float x {
		get{ return this.m_x;}
	}
	public float y {
		get{ return this.m_y;}
	}
	public float x1 {
		get{ return this.m_x1;}
	}
	public float y1 {
		get{ return this.m_y1;}
	}
	public float x2 {
		get{ return this.m_x2;}
	}
	public float y2 {
		get{ return this.m_y2;}
	}
	//================================================================================
	public uSVGPathSegCurvetoCubicRel(float x1, float y1, float x2, float y2, float x, float y)
												: base(uSVGPathSegTypes.PATHSEG_CURVETO_CUBIC_REL) {
		this.m_x = x;
		this.m_y = y;
		this.m_x1 = x1;
		this.m_y1 = y1;
		this.m_x2 = x2;
		this.m_y2 = y2;
	}
	//================================================================================
	public override uSVGPoint currentPoint{
		get{
			uSVGPoint m_return = new uSVGPoint(0f,0f);
			uSVGPathSeg m_prevSeg = previousSeg;
			if (m_prevSeg != null) {
				m_return.x = m_prevSeg.currentPoint.x + this.m_x;
				m_return.y = m_prevSeg.currentPoint.y + this.m_y;
			}
			return m_return;
		}
	}
	//-----
	public override uSVGPoint controlPoint1{
		get{
			uSVGPoint m_return = new uSVGPoint(0f,0f);
			uSVGPathSeg m_prevSeg = previousSeg;
			if (m_prevSeg != null) {
				m_return.x = m_prevSeg.currentPoint.x + this.m_x1;
				m_return.y = m_prevSeg.currentPoint.y + this.m_y1;
			}
			return m_return;
		}
	}
	//-----
	public override uSVGPoint controlPoint2{
		get{
			uSVGPoint m_return = new uSVGPoint(0f,0f);
			uSVGPathSeg m_prevSeg = previousSeg;
			if (m_prevSeg != null) {
				m_return.x = m_prevSeg.currentPoint.x + this.m_x2;
				m_return.y = m_prevSeg.currentPoint.y + this.m_y2;
			}
			return m_return;
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