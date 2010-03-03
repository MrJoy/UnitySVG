public class uSVGStopElement {
	private uSVGNumber m_offset;
	private uSVGColor m_stopColor;
	/***************************************************************************/
	private AttributeList m_attrList;
	/***************************************************************************/
	public uSVGNumber offset {
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
		m_stopColor = new uSVGColor(this.m_attrList.GetValue("STOP-COLOR"));
		//-------
		string temp = this.m_attrList.GetValue("OFFSET");
		temp = temp.Trim();
		if (temp != "") {
			if (temp.EndsWith("%")) {
				temp = temp.TrimEnd(new char[1]{'%'});
			} else {
				float m_value = uSVGNumber.ParseToFloat(temp) * 100;
				temp = m_value.ToString();
			}			
		}
		this.m_offset = new uSVGNumber(temp);
	}
}
