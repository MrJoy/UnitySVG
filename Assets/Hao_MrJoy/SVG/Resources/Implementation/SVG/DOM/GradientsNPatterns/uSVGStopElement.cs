public class uSVGStopElement {
	private uSVGAnimatedNumber m_offset;
	private uSVGColor m_stopColor;
	/***************************************************************************/
	private AttributeList m_attrList;
	/***************************************************************************/
	public uSVGAnimatedNumber offset {
		get{return this.m_offset;}
	}
	public uSVGColor stopColor {
		get{return this.m_stopColor;}
	}
	/***************************************************************************/
	public uSVGStopElement (AttributeList attrList) {
		this.m_attrList = attrList;
		f_Initialize();
	}
	/***************************************************************************/
	private void f_Initialize() {
		m_stopColor = new uSVGColor(this.m_attrList.GetValue("stop-color"));
		//-------
		string temp = this.m_attrList.GetValue("offset");
		temp = temp.Trim();
		if (temp != "") {
			if (temp.EndsWith("%")) {
				temp = temp.TrimEnd(new char[1]{'%'});
			} else {
				float m_value = uSVGNumber.ParseToFloat(temp) * 100;
				temp = m_value.ToString();
			}			
		}
		this.m_offset = new uSVGAnimatedNumber(temp);
	}
}
