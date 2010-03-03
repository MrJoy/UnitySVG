public class uSVGCircleElement : uSVGTransformable, uISVGDrawable {
		
	private uSVGAnimatedLength m_cx;
	private uSVGAnimatedLength m_cy;
	private uSVGAnimatedLength m_r;
	//================================================================================
	private uISVGGraphics m_render;
	private AttributeList m_attrList;
	private uSVGPaintable m_paintable;
	//================================================================================
	public uSVGAnimatedLength cx {
		get {
			if (this.m_cx == null) {
				this.m_cx = new uSVGAnimatedLength(0);
			}
			return this.m_cx;
		}
	}

	public uSVGAnimatedLength cy {
		get {
			if (this.m_cy == null) {
				this.m_cy = new uSVGAnimatedLength(0);
			}
			return this.m_cy;
		}
	}

	public uSVGAnimatedLength r {
		get {
			if (this.m_r == null) {
				this.m_r = new uSVGAnimatedLength(100);
			}
			return this.m_r;
		}
	}
	//================================================================================
	public uSVGCircleElement(AttributeList attrList,
							uSVGAnimatedTransformList inheritTransformList,
							uSVGPaintable inheritPaintable,
							uISVGGraphics m_render) : base (inheritTransformList)  {
		this.m_attrList = attrList;
		this.m_render = m_render;
		this.m_paintable = new uSVGPaintable(inheritPaintable, this.m_attrList);
		this.m_cx = new uSVGAnimatedLength(attrList.GetValue("cx"));
		this.m_cy = new uSVGAnimatedLength(attrList.GetValue("cy"));
		this.m_r = new uSVGAnimatedLength(attrList.GetValue("r"));
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
	public void f_BeforeRender (uSVGAnimatedTransformList transformList) {
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
