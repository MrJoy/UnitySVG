public class uSVGPathSegMovetoRel : uSVGPathSeg, uISVGDrawableSeg {
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
	public uSVGPathSegMovetoRel(float x, float y) : base(uSVGPathSegTypes.PATHSEG_MOVETO_REL) {
		this.m_x = x;
		this.m_y = y;
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
	//--------------------------------------------------------------------------------
	//Method: f_Render
	//--------------------------------------------------------------------------------
	public void f_Render(uSVGGraphicsPath m_graphicsPath) {
		uSVGPoint p;
		p = currentPoint;
		m_graphicsPath.AddMoveTo(p);
	}
}
