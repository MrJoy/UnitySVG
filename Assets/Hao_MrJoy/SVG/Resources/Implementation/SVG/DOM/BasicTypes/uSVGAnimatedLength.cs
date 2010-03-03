public class uSVGAnimatedLength {
	private uSVGLength m_baseVal;
	private uSVGLength m_animVal;
	
	/***********************************************************************************/
	public uSVGLength baseVal {
		get{return this.m_baseVal;}
	}
	public uSVGLength animVal {
		get{return this.m_animVal;}
	}
	/***********************************************************************************/
	public uSVGAnimatedLength (string baseValText) {
		this.m_baseVal = new uSVGLength(baseValText);
		this.m_animVal = new uSVGLength(baseValText);
	}
	public uSVGAnimatedLength (uSVGLength baseVal) {
		this.m_baseVal = this.m_animVal = baseVal;
	}
	public uSVGAnimatedLength (ushort unitType, float valueInSpecifiedUnits) {
		uSVGLength t_baseval = new uSVGLength(unitType, valueInSpecifiedUnits);
		this.m_baseVal = this.m_animVal = t_baseval;
	}
	public uSVGAnimatedLength (float valueInSpecifiedUnits) {
		uSVGLength t_baseval = new uSVGLength(0, valueInSpecifiedUnits);
		this.m_baseVal = this.m_animVal = t_baseval;
	}
}