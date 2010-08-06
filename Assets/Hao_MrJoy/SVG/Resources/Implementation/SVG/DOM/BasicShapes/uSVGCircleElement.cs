public class uSVGCircleElement : uSVGTransformable, uISVGDrawable {
		
	private uSVGLength m_cx = new uSVGLength(0);
	private uSVGLength m_cy = new uSVGLength(0);
	private uSVGLength m_r = new uSVGLength(100);
	//================================================================================
	private uSVGGraphics m_render;
	private AttributeList m_attrList;
	private uSVGPaintable m_paintable;
	//================================================================================
	public uSVGLength cx {
		get {
			return this.m_cx;
		}
	}

	public uSVGLength cy {
		get {
			return this.m_cy;
		}
	}

	public uSVGLength r {
		get {
			return this.m_r;
		}
	}
	//================================================================================
	public uSVGCircleElement(AttributeList attrList,
							uSVGTransformList inheritTransformList,
							uSVGPaintable inheritPaintable,
							uSVGGraphics m_render) : base (inheritTransformList)  {
		this.m_attrList = attrList;
		this.m_render = m_render;
		this.m_paintable = new uSVGPaintable(inheritPaintable, this.m_attrList);
		this.m_cx = new uSVGLength(attrList.GetValue("CX"));
		this.m_cy = new uSVGLength(attrList.GetValue("CY"));
		this.m_r = new uSVGLength(attrList.GetValue("R"));
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
