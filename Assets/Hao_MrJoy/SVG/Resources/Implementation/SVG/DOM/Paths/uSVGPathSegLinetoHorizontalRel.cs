public class uSVGPathSegLinetoHorizontalRel : uSVGPathSeg, uISVGDrawableSeg  {
	private float m_x = 0f;
	//================================================================================
	public float x {
		get{ return this.m_x;}
	}
	//================================================================================
	public uSVGPathSegLinetoHorizontalRel(float x) :
										base(uSVGPathSegTypes.PATHSEG_LINETO_HORIZONTAL_REL) {
		this.m_x = x;
	}
	//================================================================================
	public override uSVGPoint currentPoint{
		get{
			uSVGPoint m_return = new uSVGPoint(0f,0f);
			uSVGPathSeg m_prevSeg = previousSeg;
			if (m_prevSeg != null) {
				m_return.x = m_prevSeg.currentPoint.x + this.m_x;
				m_return.y = m_prevSeg.currentPoint.y;
			}
			return m_return;
		}
	}
	//--------------------------------------------------------------------------------
	//Method: f_Render
	//--------------------------------------------------------------------------------
	public void f_Render (uSVGGraphicsPath m_graphicsPath) {
		uSVGPoint p;
		p = currentPoint;
		m_graphicsPath.AddLineTo(p);
	}
}
