public class uSVGPathSegClosePath : uSVGPathSeg, uISVGDrawableSeg {
	private float m_x = 0f;
	private float m_y = 0f;
	//================================================================================
	public uSVGPathSegClosePath(float x, float y) : base(uSVGPathSegTypes.PATHSEG_CLOSEPATH) {
		if (x == -1f && y == -1f) {
			this.m_x = previousPoint.x;
			this.m_y = previousPoint.y;
		} else {
			this.m_x = x;
			this.m_y = y;
		}		
	}
	//================================================================================
	public override uSVGPoint currentPoint{
		get{
			return new uSVGPoint(this.m_x, this.m_y);
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