public class uSVGAnimatedNumber{

	private float m_baseVal;
	private float m_animVal;
	/***************************************************************************/
	public float baseVal {
		get{return this.m_baseVal;}
		set{this.m_baseVal = value;}
	}

	public float animVal {
		get{return this.m_animVal;}
		set{this.m_animVal = value;}
	}
	/***************************************************************************/
	public uSVGAnimatedNumber (string str) {
		this.m_baseVal = uSVGNumber.ParseToFloat(str);
		this.m_animVal = baseVal;
	}
}
