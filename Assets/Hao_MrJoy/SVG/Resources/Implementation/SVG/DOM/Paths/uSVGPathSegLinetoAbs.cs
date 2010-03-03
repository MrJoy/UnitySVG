public class uSVGPathSegLinetoAbs : uSVGPathSeg, uISVGDrawableSeg {
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
	public uSVGPathSegLinetoAbs(float x, float y) : base(uSVGPathSegTypes.PATHSEG_LINETO_ABS) {
		this.m_x = x;
		this.m_y = y;
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
