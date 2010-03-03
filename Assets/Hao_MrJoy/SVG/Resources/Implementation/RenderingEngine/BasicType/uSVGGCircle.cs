public class uSVGGCircle{
	private uSVGPoint m_p;
	private float m_r;
	//================================================================================
	public uSVGPoint point {
		get{ return this.m_p;}
	}
	//------
	public float r {
		get{return this.m_r;}
	}
	//================================================================================
	public uSVGGCircle(uSVGPoint p, float r) {
		this.m_p = new uSVGPoint(p.x, p.y);
		this.m_r = r;
	}
}
