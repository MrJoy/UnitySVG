public class uSVGGEllipse {
	private uSVGPoint m_p;
	private float m_r1;
	private float m_r2;
	private float m_angle;
	//================================================================================
	public uSVGPoint point {
		get{ return this.m_p;}
	}
	//------
	public float r1 {
		get{return this.m_r1;}
	}
	//------
	public float r2 {
		get{return this.m_r2;}
	}
		//------
	public float angle {
		get{return this.m_angle;}
	}
	//================================================================================
	public uSVGGEllipse(uSVGPoint p, float r1, float r2, float angle) {
		this.m_p = new uSVGPoint(p.x, p.y);
		this.m_r1 = r1;
		this.m_r2 = r2;
		this.m_angle = angle;
	}
}
