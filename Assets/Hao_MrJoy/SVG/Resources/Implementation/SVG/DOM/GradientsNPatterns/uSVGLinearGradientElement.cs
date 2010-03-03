using System.Collections.Generic;

public class uSVGLinearGradientElement : uSVGGradientElement {
	private uSVGAnimatedLength m_x1;
	private uSVGAnimatedLength m_y1;
	private uSVGAnimatedLength m_x2;
	private uSVGAnimatedLength m_y2;
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
	public uSVGAnimatedLength x1 {
		get{return this.m_x1;}
	}
	
	public uSVGAnimatedLength y1 {
		get{return this.m_y1;}
	}
	
	public uSVGAnimatedLength x2 {
		get{return this.m_x2;}
	}
	
	public uSVGAnimatedLength y2 {
		get{return this.m_y2;}
	}

	public int stopCount {
		get{return this.m_stopList.Count;}
	}

	public List<uSVGStopElement> stopList {
		get{return this.m_stopList;}
	}
	/***************************************************************************/
	public uSVGLinearGradientElement(uXMLImp xmlImp,
										AttributeList attrList): base(attrList) {
		this.m_attrList = attrList;
		this.m_xmlImp = xmlImp;
		f_Initialize();
	}
	/***************************************************************************/
	private void f_Initialize() {
		this.m_stopList = new List<uSVGStopElement>();
		
		string temp = this.m_attrList.GetValue("ID");
		this.m_id = temp;
		
		temp = this.m_attrList.GetValue("X1");
		if (temp == "") {
			m_x1 = new uSVGAnimatedLength("0%");
		} else m_x1 = new uSVGAnimatedLength(temp);

		temp = this.m_attrList.GetValue("Y1");
		if (temp == "") {
			m_y1 = new uSVGAnimatedLength("0%");
		} else m_y1 = new uSVGAnimatedLength(temp);
		
		temp = this.m_attrList.GetValue("X2");
		if (temp == "") {
			m_x2 = new uSVGAnimatedLength("100%");
		} else m_x2 = new uSVGAnimatedLength(temp);

		temp = this.m_attrList.GetValue("Y2");
		if (temp == "") {
			m_y2 = new uSVGAnimatedLength("0%");
		} else m_y2 = new uSVGAnimatedLength(temp);
		
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
				if (t_name == "STOP") {
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
