using System.Collections.Generic;

public class uSVGGElement : uSVGTransformable, uISVGDrawable {
	//-------------------------------
	private AttributeList m_attrList;
	private List<object> m_elementList;
	private uXMLImp m_xmlImp;
	//-------------------------------
	private uISVGGraphics m_render;
	//-------------------------------
	private uSVGPaintable m_paintable;
	/***********************************************************************************/
	public uSVGGElement(string gElement,
						uSVGAnimatedTransformList inheritTransformList,
						uSVGPaintable inheritPaintable,
						uISVGGraphics m_render) : base (inheritTransformList) {
		this.m_render = m_render;
		this.m_xmlImp = new uXMLImp(gElement);		
		this.m_attrList = this.m_xmlImp.f_GetNextAttrList();
		this.m_paintable = new uSVGPaintable(inheritPaintable, this.m_attrList);
		f_Initial();
	}
	/***********************************************************************************/
	private void f_Initial() {
		m_elementList = new List<object>();
		this.currentTransformList = new uSVGAnimatedTransformList(
											this.m_attrList.GetValue("TRANSFORM", true));
		f_GetElementList();
	}
	/***********************************************************************************/
	private void f_GetElementList() {
		while(this.m_xmlImp.f_ReadNextTag()) {
			string t_name = this.m_xmlImp.f_GetCurrentTagName();
			AttributeList t_attrList;
			if (this.m_xmlImp.f_GetCurrentTagState() != uXMLImp.XMLTagState.CLOSE) {
				switch(t_name.ToUpper()) {
					case "RECT": {
						t_attrList = this.m_xmlImp.f_GetCurrentAttributesList();
						uSVGRectElement temp = new uSVGRectElement(	t_attrList,
																	this.summaryTransformList,
																	this.m_paintable,
								 									this.m_render);	
						m_elementList.Add(temp);
					break;
					}
					case "LINE": {
						t_attrList = this.m_xmlImp.f_GetCurrentAttributesList();
						uSVGLineElement temp = new uSVGLineElement(	t_attrList,
																	this.summaryTransformList,
																	this.m_paintable,
								 									this.m_render);	
						m_elementList.Add(temp);
					break;
					}
					case "CIRCLE": {
						t_attrList = this.m_xmlImp.f_GetCurrentAttributesList();
						uSVGCircleElement temp = new uSVGCircleElement(	t_attrList,
																	this.summaryTransformList,
																	this.m_paintable,
								 									this.m_render);	
						m_elementList.Add(temp);
					break;
					}
					case "ELLIPSE": {
						t_attrList = this.m_xmlImp.f_GetCurrentAttributesList();
						uSVGEllipseElement temp = new uSVGEllipseElement(	t_attrList,
																	this.summaryTransformList,
																	this.m_paintable,
								 									this.m_render);	
						m_elementList.Add(temp);
					break;
					}
					case "POLYLINE": {
						t_attrList = this.m_xmlImp.f_GetCurrentAttributesList();
						uSVGPolylineElement temp = new uSVGPolylineElement(	t_attrList,
																	this.summaryTransformList,
																	this.m_paintable,
								 									this.m_render);	
						m_elementList.Add(temp);
					break;
					}
					case "POLYGON": {
						t_attrList = this.m_xmlImp.f_GetCurrentAttributesList();
						uSVGPolygonElement temp = new uSVGPolygonElement(t_attrList,
																	this.summaryTransformList,
																	this.m_paintable,
								 									this.m_render);	
						m_elementList.Add(temp);
					break;
					}
					case "PATH": {
						t_attrList = this.m_xmlImp.f_GetCurrentAttributesList();
						uSVGPathElement temp = new uSVGPathElement(	t_attrList,
																	this.summaryTransformList,
																	this.m_paintable,
								 									this.m_render);	
						m_elementList.Add(temp);
					break;
					}
					case "SVG": {
						string t_string = this.m_xmlImp.f_GetCurrentLineText();
						string t_restString = "";
						t_restString = this.m_xmlImp.f_GetUntilCloseTag("svg");
						t_string += t_restString;
						m_elementList.Add(new uSVGSVGElement(	t_string,
																this.summaryTransformList,
																this.m_paintable,
																this.m_render));
						break;
					}
					case "G": {
						string t_string = this.m_xmlImp.f_GetCurrentLineText();
						string t_restString = "";
						t_restString = this.m_xmlImp.f_GetUntilCloseTag("g");
						t_string += t_restString;
						m_elementList.Add(new uSVGGElement(	t_string, 
															this.summaryTransformList,
															this.m_paintable,
															this.m_render));
						break;
					}
					//--------
					case "LINEARGRADIENT": {
						t_attrList = this.m_xmlImp.f_GetCurrentAttributesList();
						string t_string = this.m_xmlImp.f_GetCurrentLineText();
						string t_restString = "";
						t_restString = this.m_xmlImp.f_GetUntilCloseTag("linearGradient");
						t_string += t_restString;
						uSVGLinearGradientElement temp = new uSVGLinearGradientElement(t_string,
																					t_attrList);
						this.m_paintable.AppendLinearGradient(temp);
						break;
					}
					//--------
					case "RADIALGRADIENT": {
						t_attrList = this.m_xmlImp.f_GetCurrentAttributesList();
						string t_string = this.m_xmlImp.f_GetCurrentLineText();
						string t_restString = "";
						t_restString = this.m_xmlImp.f_GetUntilCloseTag("radialGradient");
						t_string += t_restString;
						uSVGRadialGradientElement temp = new uSVGRadialGradientElement(t_string,
																					t_attrList);
						this.m_paintable.AppendRadialGradient(temp);
						break;
					}
				}
			}
		}
	}	
	/***********************************************************************************/
	public void f_BeforeRender(uSVGAnimatedTransformList transformList) {		
		this.inheritTransformList = transformList;		
		for (int i = 0; i < m_elementList.Count; i++) {
			uISVGDrawable temp = m_elementList[i] as uISVGDrawable;
			if (temp != null) {
				temp.f_BeforeRender(this.summaryTransformList);
			}
		}
	}
	public void f_Render() {

		UnityEngine.Color m_color = new UnityEngine.Color(0f, 0f, 0f);
		if (this.m_paintable.strokeColor != null) {
			m_color = this.m_paintable.strokeColor.color;
		}
		
		if ((this.m_paintable.IsFill() == true) && (this.m_paintable.IsLinearGradiantFill() == false)) {
			m_color = this.m_paintable.fillColor.color;
		}
		
		for (int i = 0; i < m_elementList.Count; i++) {
			uISVGDrawable temp = m_elementList[i] as uISVGDrawable;
			if (temp != null) {
				if (this.m_paintable.strokeColor != null) {
					this.m_render.SetColor(m_color);
				}
				if ((this.m_paintable.IsFill() == true) && (this.m_paintable.IsLinearGradiantFill() == false)) {
					this.m_render.SetColor(m_color);
				}
				temp.f_Render();
			}
		}
	}
}