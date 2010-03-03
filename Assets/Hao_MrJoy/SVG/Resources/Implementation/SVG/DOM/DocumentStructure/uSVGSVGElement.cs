using System.Collections.Generic;

public class uSVGSVGElement : uSVGTransformable, uISVGDrawable {
	private uSVGAnimatedLength m_width;
	private uSVGAnimatedLength m_height;
	private string m_contentScriptType;
	private string m_contentStyleType;
	
	private uSVGRect m_viewport;

	private float currentScale;
	private uSVGPoint currentTranslate;
	//-------------------------------
	private AttributeList m_attrList;
	private List<object> m_elementList;
	private uXMLImp m_xmlImp;
	//-------------------------------
	private uSVGPaintable m_paintable;
	//-------------------------------
	private uSVGGraphics m_render;

	/***********************************************************************************/
	public uSVGSVGElement(	uXMLImp xmlImp,
							uSVGAnimatedTransformList inheritTransformList,
							uSVGPaintable inheritPaintable,
							uSVGGraphics m_render) : base (inheritTransformList) {
		this.m_render = m_render;
		this.m_xmlImp = xmlImp;
		this.m_attrList = this.m_xmlImp.f_GetCurrentAttributesList();
		this.m_paintable = new uSVGPaintable(inheritPaintable, this.m_attrList);
		this.m_width = new uSVGAnimatedLength(m_attrList.GetValue("WIDTH"));
		this.m_height = new uSVGAnimatedLength(m_attrList.GetValue("HEIGHT"));
		f_Initial();
	}
	/***********************************************************************************/
	private void f_Initial() {
		//trich cac gia tri cua thuoc tinh VIEWBOX va chua vao trong m_viewport
		f_SetViewBox();
		m_elementList = new List<object>();
		
		//Viewbox transform se lay thuoc tinh VIEWBOX de tao ra 1 transform
		//va transform nay se chua trong m_cachedViewBoxTransform
		ViewBoxTransform();
		
		//Tao currentTransformList va add cai transform dau tien vao, do la cai VIEWBOX.
		uSVGTransform temp = CreateSVGTransformFromMatrix(m_cachedViewBoxTransform);
		uSVGAnimatedTransformList t_currentTransformList = new uSVGAnimatedTransformList();		
		t_currentTransformList.animVal.AppendItem(temp);

		this.currentTransformList = t_currentTransformList;

		//Get all element between <SVG>...</SVG>
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
		this.m_render.SetSize(this.m_width.animVal.value, this.m_height.animVal.value);
		for (int i = 0; i < m_elementList.Count; i++) {
			uISVGDrawable temp = m_elementList[i] as uISVGDrawable;
			if (temp != null) {
				temp.f_Render();
			}
		}
	}
	/***********************************************************************************/
	private void f_SetViewBox() {
		string attr = this.m_attrList.GetValue("VIEWBOX");
		if (attr != "") {
			string[] m_temp = uSVGStringExtractor.f_ExtractTransformValue(attr);
			if (m_temp.Length == 4) {
				float x = uSVGNumber.ParseToFloat(m_temp[0]);
				float y = uSVGNumber.ParseToFloat(m_temp[1]);
				float w = uSVGNumber.ParseToFloat(m_temp[2]);
				float h = uSVGNumber.ParseToFloat(m_temp[3]);
				this.m_viewport = new uSVGRect(x, y, w, h);
			}
		}
	}
	/***********************************************************************************/
	public uSVGNumber CreateSVGNumber() {
		return new uSVGNumber(0.0f);
	}
	
	public uSVGLength CreateSVGLength() {
		return new uSVGLength(0, 0.0f);
	}
	
	public uSVGPoint CreateSVGPoint() {
		return new uSVGPoint(0.0f, 0.0f);
	}
	public uSVGMatrix CreateSVGMatrix() {
		return new uSVGMatrix();
	}
	
	public uSVGRect CreateSVGRect() {
		return new uSVGRect(0.0f, 0.0f, 0.0f, 0.0f);
	}
	
	public uSVGTransform CreateSVGTransform() {
		return new uSVGTransform();
	}
	
	public uSVGTransform CreateSVGTransformFromMatrix(uSVGMatrix matrix) {
		return new uSVGTransform(matrix);
	}
	/***********************************************************************************/
	private uSVGMatrix m_cachedViewBoxTransform = null;
	public uSVGMatrix ViewBoxTransform() {
		if (this.m_cachedViewBoxTransform == null) {
			
			uSVGMatrix matrix = CreateSVGMatrix();
				
			float x = 0.0f;
			float y = 0.0f;
			float w = 0.0f;
			float h = 0.0f;
			
			float attrWidth = this.m_width.animVal.value;
			float attrHeight = this.m_height.animVal.value;

			if (m_attrList.GetValue("VIEWBOX") != "") {
				uSVGRect r = this.m_viewport;
				x += -r.x;
				y += -r.y;
				w = r.width;
				h = r.height;
			} else {
				w = attrWidth;
				h = attrHeight;
			}
			
			float x_ratio = attrWidth / w;
			float y_ratio = attrHeight / h;

			matrix = matrix.ScaleNonUniform(x_ratio, y_ratio);
			matrix = matrix.Translate(x, y);
			m_cachedViewBoxTransform = matrix;
		}
		return this.m_cachedViewBoxTransform;
	}
}