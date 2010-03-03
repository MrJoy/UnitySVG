public class uSVGGArcAbs  {
	private uSVGPoint m_p;
	private float m_r1;
	private float m_r2;
	private float m_angle;
	private bool m_largeArcFlag;
	private bool m_sweepFlag;
	//================================================================================
	public float r1 {
		get{return this.m_r1;}
	}
	//-----
	public float r2 {
		get{return this.m_r2;}
	}
	//-----
	public float angle {
		get{return this.m_angle;}
	}
	//-----
	public bool largeArcFlag {
		get{return this.m_largeArcFlag;}
	}
		//-----
	public bool sweepFlag {
		get{return this.m_sweepFlag;}
	}
	//-----
	public uSVGPoint point {
		get{return this.m_p;}
	}
	//================================================================================
	public uSVGGArcAbs(float r1, float r2, float angle,
							bool largeArcFlag, bool sweepFlag, uSVGPoint p) {
		this.m_r1 = r1;
		this.m_r2 = r2;
		this.m_angle = angle;
		this.m_largeArcFlag = largeArcFlag;
		this.m_sweepFlag = sweepFlag;
		this.m_p = p;
	}
}
