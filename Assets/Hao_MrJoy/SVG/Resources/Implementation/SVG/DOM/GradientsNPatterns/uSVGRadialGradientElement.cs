using System.Collections.Generic;

public class uSVGRadialGradientElement : uSVGGradientElement {
	
	private uSVGAnimatedLength m_cx;
	private uSVGAnimatedLength m_cy;
	private uSVGAnimatedLength m_r;
	private uSVGAnimatedLength m_fx;
	private uSVGAnimatedLength m_fy;
	//---------
	private string m_id;
	private uXMLImp m_xmlImp;
	private List<uSVGStopElement> m_stopList;
	//---------
	private AttributeList m_attrList;
	/***************************************************************************/
	public string id {
		get{ return this.m_id;}
	}
	public uSVGAnimatedLength cx {
		get{return this.m_cx;}
	}
	public uSVGAnimatedLength cy {
		get{return this.m_cy;}
	}
		public uSVGAnimatedLength r {
		get{return this.m_r;}
	}
		public uSVGAnimatedLength fx {
		get{return this.m_fx;}
	}
		public uSVGAnimatedLength fy {
		get{return this.m_fy;}
	}
	
	public int stopCount {
		get{return this.m_stopList.Count;}
	}

	public List<uSVGStopElement> stopList {
		get{return this.m_stopList;}
	}
	/***************************************************************************/
	public uSVGRadialGradientElement(uXMLImp xmlImp,
										AttributeList attrList) : base(attrList) {
		this.m_attrList = attrList;
		this.m_xmlImp = xmlImp;
		f_Initialize();
	}
	/***************************************************************************/
	private void f_Initialize() {
		this.m_stopList = new List<uSVGStopElement>();		
		string temp = this.m_attrList.GetValue("ID");
		this.m_id = temp;

		temp = this.m_attrList.GetValue("CX");
		if (temp == "") {
			m_cx = new uSVGAnimatedLength("50%");
		} else m_cx = new uSVGAnimatedLength(temp);

		temp = this.m_attrList.GetValue("CY");
		if (temp == "") {
			m_cy = new uSVGAnimatedLength("50%");
		} else m_cy = new uSVGAnimatedLength(temp);

		temp = this.m_attrList.GetValue("R");
		if (temp == "") {
			m_r = new uSVGAnimatedLength("50%");
		} else m_r = new uSVGAnimatedLength(temp);

		temp = this.m_attrList.GetValue("FX");
		if (temp == "") {
			m_fx = new uSVGAnimatedLength("50%");
		} else m_fx = new uSVGAnimatedLength(temp);

		temp = this.m_attrList.GetValue("FY");
		if (temp == "") {
			m_fy = new uSVGAnimatedLength("50%");
		} else m_fy = new uSVGAnimatedLength(temp);
		
		f_GetElementList();
	}
	//---------------
	private void f_GetElementList() {
    bool exitFlag = false;
    while(!exitFlag && this.m_xmlImp.f_ReadNextTag()) {
      if(this.m_xmlImp.f_GetCurrentTagState() == uXMLImp.XMLTagState.CLOSE) {
        exitFlag = true;
        continue;
      }
			string t_name = this.m_xmlImp.f_GetCurrentTagName();
			AttributeList t_attrList;
				if (t_name == "stop") {
					t_attrList = this.m_xmlImp.f_GetCurrentAttributesList();
					uSVGStopElement temp = new uSVGStopElement(	t_attrList);	
					m_stopList.Add(temp);
				}
		}
	}
	/***************************************************************************/
	public uSVGStopElement GetStopElement(int i) {
		if ((i >=0 ) && (i < stopCount)) {
			return this.m_stopList[i];
		}
		return null;
	}
}
