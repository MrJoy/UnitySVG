using System.Collections.Generic;

public class uSVGGElement : uSVGTransformable, uISVGDrawable {
	//-------------------------------
	private AttributeList m_attrList;
	private List<object> m_elementList = null;
	private uXMLImp m_xmlImp;
	//-------------------------------
	private uSVGGraphics m_render;
	//-------------------------------
	private uSVGPaintable m_paintable;
	/***********************************************************************************/
	public uSVGGElement(uXMLImp xmlImp,
						uSVGTransformList inheritTransformList,
						uSVGPaintable inheritPaintable,
						uSVGGraphics m_render) : base (inheritTransformList) {
		this.m_render = m_render;
		this.m_xmlImp = xmlImp;		
		this.m_attrList = this.m_xmlImp.f_GetCurrentAttributesList();
		this.m_paintable = new uSVGPaintable(inheritPaintable, this.m_attrList);
		f_Initial();
	}
	/***********************************************************************************/
	private void f_Initial() {
    m_elementList = new List<object>();
		this.currentTransformList = new uSVGTransformList(
											this.m_attrList.GetValue("TRANSFORM"));
		f_GetElementList();
	}
	/***********************************************************************************/
	private void f_GetElementList() {
    bool exitFlag = false;
    while(!exitFlag && this.m_xmlImp.f_ReadNextTag()) {
      if(this.m_xmlImp.f_GetCurrentTagState() == uXMLImp.XMLTagState.CLOSE) {
        exitFlag = true;
        continue;
      }
			string t_name = this.m_xmlImp.f_GetCurrentTagName();
			AttributeList t_attrList;
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
						m_elementList.Add(new uSVGSVGElement(	this.m_xmlImp,
																this.summaryTransformList,
																this.m_paintable,
																this.m_render));
						break;
					}
					case "G": {
						m_elementList.Add(new uSVGGElement(	this.m_xmlImp, 
															this.summaryTransformList,
															this.m_paintable,
															this.m_render));
						break;
					}
					//--------
					case "LINEARGRADIENT": {
						t_attrList = this.m_xmlImp.f_GetCurrentAttributesList();
						uSVGLinearGradientElement temp = new uSVGLinearGradientElement(this.m_xmlImp,
																					t_attrList);
						this.m_paintable.AppendLinearGradient(temp);
						break;
					}
					//--------
					case "RADIALGRADIENT": {
						t_attrList = this.m_xmlImp.f_GetCurrentAttributesList();
						uSVGRadialGradientElement temp = new uSVGRadialGradientElement(this.m_xmlImp,
																					t_attrList);
						this.m_paintable.AppendRadialGradient(temp);
						break;
					}
					case "DEFS": {
					  f_GetElementList();
					  break;
					}
					case "TITLE": {
					  f_GetElementList();
					  break;
					}
					case "DESC": {
					  f_GetElementList();
					  break;
					}
//					default:
//					  UnityEngine.Debug.LogError("Unexpected tag: " + t_name);
//					  break;
				}
		}
	}	
	/***********************************************************************************/
	public void f_BeforeRender(uSVGTransformList transformList) {		
		this.inheritTransformList = transformList;		
		for (int i = 0; i < m_elementList.Count; i++) {
			uISVGDrawable temp = m_elementList[i] as uISVGDrawable;
			if (temp != null) {
				temp.f_BeforeRender(this.summaryTransformList);
			}
		}
	}
	public void f_Render() {
		UnityEngine.Color m_color = UnityEngine.Color.black;
		bool use_m_color = false;
		if ((this.m_paintable.IsFill() == true) && (this.m_paintable.IsLinearGradiantFill() == false)) {
			m_color = this.m_paintable.fillColor.color;
			use_m_color = true;
		} else if (this.m_paintable.strokeColor != null) {
			m_color = this.m_paintable.strokeColor.color;
			use_m_color = true;
		}
		
		
		for (int i = 0; i < m_elementList.Count; i++) {
			uISVGDrawable temp = m_elementList[i] as uISVGDrawable;
			if (temp != null) {
			  if(use_m_color)
					this.m_render.SetColor(m_color);
				temp.f_Render();
			}
		}
	}
}