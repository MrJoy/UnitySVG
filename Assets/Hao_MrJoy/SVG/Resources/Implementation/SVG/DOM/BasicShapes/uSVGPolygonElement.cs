using System.Collections.Generic;

public class uSVGPolygonElement : uSVGTransformable, uISVGDrawable {
	private List<uSVGPoint> m_listPoints;
	//================================================================================
	private uSVGGraphics m_render;
	private AttributeList m_attrList;
	private uSVGPaintable m_paintable;
	//================================================================================
	public List<uSVGPoint> listPoints {
		get{ return this.m_listPoints;}
	}
	//================================================================================
	public uSVGPolygonElement(	AttributeList attrList,
								uSVGTransformList inheritTransformList,
								uSVGPaintable inheritPaintable,
								uSVGGraphics m_render) : base (inheritTransformList) {
		this.m_attrList = attrList;
		this.m_render = m_render;
		this.m_paintable = new uSVGPaintable(inheritPaintable, attrList);
		this.m_listPoints = f_ExtractPoints(this.m_attrList.GetValue("POINTS"));
	}
	//================================================================================
	private List<uSVGPoint> f_ExtractPoints(string inputText) {
		List<uSVGPoint> m_return = new List<uSVGPoint>();
		string[] m_lstStr = uSVGStringExtractor.f_ExtractTransformValue(inputText);

		int len = m_lstStr.Length;

		for (int i = 0; i < len -1; i++) {
			string value1, value2;
			value1 = m_lstStr[i];
			value2 = m_lstStr[i+1];
			uSVGLength m_length1 = new uSVGLength(value1);
			uSVGLength m_length2 = new uSVGLength(value2);
			uSVGPoint m_point = new uSVGPoint(m_length1.value, m_length2.value);
			m_return.Add(m_point);
			i++;
		}
		return m_return;		
	}
	
	//================================================================================
	private uSVGGraphicsPath m_graphicsPath;
	private void f_CreateGraphicsPath() {
		this.m_graphicsPath = new uSVGGraphicsPath();
					
		this.m_graphicsPath.Add(this);
		this.m_graphicsPath.transformList = this.summaryTransformList;
	}
	//-----
	private void f_Draw() {
		if (this.m_paintable.strokeColor == null) return;
		
		this.m_render.DrawPath(this.m_graphicsPath, this.m_paintable.strokeWidth,
														this.m_paintable.strokeColor);
	}
	//================================================================================	
	//Thuc thi Interface Drawable
	public void f_BeforeRender (uSVGTransformList transformList) {
		this.inheritTransformList = transformList;
	}
	//------
	public void f_Render () {
		f_CreateGraphicsPath();
		this.m_render.SetStrokeLineCap(this.m_paintable.strokeLineCap);
		this.m_render.SetStrokeLineJoin(this.m_paintable.strokeLineJoin);
		switch(this.m_paintable.GetPaintType()) {
			case uSVGPaintTypes.SVG_PAINT_SOLID_GRADIENT_FILL : {
				this.m_render.FillPath(this.m_paintable.fillColor, this.m_graphicsPath);
				f_Draw();
				break;
			}
			case uSVGPaintTypes.SVG_PAINT_LINEAR_GRADIENT_FILL : {

				uSVGLinearGradientBrush m_linearGradBrush = 
									this.m_paintable.GetLinearGradientBrush(this.m_graphicsPath);

				if (m_linearGradBrush != null) {
					this.m_render.FillPath(m_linearGradBrush, m_graphicsPath);
				}
				f_Draw();
				break;
			}
			case uSVGPaintTypes.SVG_PAINT_RADIAL_GRADIENT_FILL : {
				uSVGRadialGradientBrush m_radialGradBrush = 
									this.m_paintable.GetRadialGradientBrush(this.m_graphicsPath);

				if (m_radialGradBrush != null) {
					this.m_render.FillPath(m_radialGradBrush, m_graphicsPath);
				}
				f_Draw();
				break;
			}
			case uSVGPaintTypes.SVG_PAINT_PATH_DRAW : {
				f_Draw();
				break;
			}
		}
	}
}
