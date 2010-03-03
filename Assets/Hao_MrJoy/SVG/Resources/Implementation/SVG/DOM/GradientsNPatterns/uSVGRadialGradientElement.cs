using System.Collections.Generic;

public class uSVGRadialGradientElement : uSVGGradientElement {
	
	private uSVGLength m_cx;
	private uSVGLength m_cy;
	private uSVGLength m_r;
	private uSVGLength m_fx;
	private uSVGLength m_fy;
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
	public uSVGLength cx {
		get{return this.m_cx;}
	}
	public uSVGLength cy {
		get{return this.m_cy;}
	}
		public uSVGLength r {
		get{return this.m_r;}
	}
		public uSVGLength fx {
		get{return this.m_fx;}
	}
		public uSVGLength fy {
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
			m_cx = new uSVGLength("50%");
		} else m_cx = new uSVGLength(temp);

		temp = this.m_attrList.GetValue("CY");
		if (temp == "") {
			m_cy = new uSVGLength("50%");
		} else m_cy = new uSVGLength(temp);

		temp = this.m_attrList.GetValue("R");
		if (temp == "") {
			m_r = new uSVGLength("50%");
		} else m_r = new uSVGLength(temp);

		temp = this.m_attrList.GetValue("FX");
		if (temp == "") {
			m_fx = new uSVGLength("50%");
		} else m_fx = new uSVGLength(temp);

		temp = this.m_attrList.GetValue("FY");
		if (temp == "") {
			m_fy = new uSVGLength("50%");
		} else m_fy = new uSVGLength(temp);
		
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
