public class uSVGGCubicAbs  {
	private uSVGPoint m_p1;
	private uSVGPoint m_p2;
	private uSVGPoint m_p;

	//================================================================================
	public uSVGPoint p1 {
		get{return this.m_p1;}
	}
	//-----
	public uSVGPoint p2 {
		get{return this.m_p2;}
	}
	//-----
	public uSVGPoint p {
		get{return this.m_p;}
	}
	//================================================================================
	public uSVGGCubicAbs(uSVGPoint p1, uSVGPoint p2, uSVGPoint p) {
		this.m_p1 = new uSVGPoint(p1.x, p1.y);
		this.m_p2 = new uSVGPoint(p2.x, p2.y);
		this.m_p = new uSVGPoint(p.x, p.y);
	}
}
