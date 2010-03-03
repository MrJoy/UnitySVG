public class uSVGPathSegCurvetoQuadraticAbs : uSVGPathSegCurvetoQuadratic, uISVGDrawableSeg  {
	private float m_x	= 0f;
	private float m_y	= 0f;
	private float m_x1	= 0f;
	private float m_y1	= 0f;
	//================================================================================
	public float x {
		get{ return this.m_x;}
	}
	//-----
	public float y {
		get{ return this.m_y;}
	}
	//-----
	public float x1 {
		get{ return this.m_x1;}
	}
	//-----
	public float y1 {
		get{ return this.m_y1;}
	}
	//================================================================================
	public uSVGPathSegCurvetoQuadraticAbs(float x1, float y1, float x, float y)
										: base(uSVGPathSegTypes.PATHSEG_CURVETO_QUADRATIC_ABS) {
		this.m_x = x;
		this.m_y = y;
		this.m_x1 = x1;
		this.m_y1 = y1;
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
			return new uSVGPoint(this.m_x1, this.m_y1);
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