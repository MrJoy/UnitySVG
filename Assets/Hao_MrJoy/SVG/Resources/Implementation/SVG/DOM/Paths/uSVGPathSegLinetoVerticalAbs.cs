public class uSVGPathSegLinetoVerticalAbs : uSVGPathSeg, uISVGDrawableSeg  {
	private float m_y = 0f;
	//================================================================================
	public float y {
		get{ return this.m_y;}
	}
	//================================================================================
	public uSVGPathSegLinetoVerticalAbs(float y) :
									base(uSVGPathSegTypes.PATHSEG_LINETO_HORIZONTAL_ABS) {
		this.m_y = y;
	}
	//================================================================================
	public override uSVGPoint currentPoint{
		get{
			uSVGPoint m_return = new uSVGPoint(0f,0f);
			uSVGPathSeg m_prevSeg = previousSeg;
			if (m_prevSeg != null) {
				m_return.x = m_prevSeg.currentPoint.x;
				m_return.y = this.m_y;
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
