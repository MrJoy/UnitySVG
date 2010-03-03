public class uSVGPathSegArcAbs : uSVGPathSeg, uISVGDrawableSeg  {
	private float m_x			= 0f;
	private float m_y			= 0f;
	private float m_r1			= 0f;
	private float m_r2			= 0f;
	private float m_angle		= 0f;
	private bool m_largeArcFlag	= false;
	private bool m_sweepFlag	= false;
	//================================================================================
	public float x {
		get{ return this.m_x;}
	}
	//-----
	public float y {
		get{ return this.m_y;}
	}
	//================================================================================
	public uSVGPathSegArcAbs(float r1, float r2, float angle,
							bool largeArcFlag, bool sweepFlag,
							float x, float y) : base(uSVGPathSegTypes.PATHSEG_ARC_ABS) {
		this.m_r1 = r1;
		this.m_r2 = r2;
		this.m_angle = angle;
		this.m_largeArcFlag = largeArcFlag;
		this.m_sweepFlag = sweepFlag;
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
	public void f_Render(uSVGGraphicsPath m_graphicsPath) {
		uSVGPoint p;
		p = currentPoint;
		m_graphicsPath.AddArcTo(this.m_r1, this.m_r2, this.m_angle,
						this.m_largeArcFlag, this.m_sweepFlag, p);
	}
}