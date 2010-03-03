public class uSVGAnimatedEnumeration{
	private ushort m_baseVal;
	private ushort m_animVal;
	/***************************************************************************/
	public ushort baseVal {
		get{return this.m_baseVal;}
		set{this.m_baseVal = value;}
	}

	public ushort animVal {
		get{return this.m_animVal;}
		set{this.m_animVal = value;}
	}
	/***************************************************************************/
	public uSVGAnimatedEnumeration(ushort val) {
		this.m_baseVal = this.m_animVal = val;
	}
}
