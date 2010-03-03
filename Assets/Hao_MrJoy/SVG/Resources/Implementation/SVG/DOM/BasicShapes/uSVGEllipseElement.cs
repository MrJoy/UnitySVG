
public class uSVGEllipseElement : uSVGTransformable, uISVGDrawable {
	private uSVGLength m_cx;
	private uSVGLength m_cy;
	private uSVGLength m_rx;
	private uSVGLength m_ry;
	//================================================================================
	private uSVGGraphics m_render;
	private AttributeList m_attrList;
	private uSVGPaintable m_paintable;
	//================================================================================
	public uSVGLength cx {
		get {
			if (this.m_cx == null) {
				this.m_cx = new uSVGLength(0);
			}
			return this.m_cx;
		}
	}

	public uSVGLength cy {
		get {
			if (this.m_cy == null) {
				this.m_cy = new uSVGLength(0);
			}
			return this.m_cy;
		}
	}

	public uSVGLength rx {
		get {
			if (this.m_rx == null) {
				this.m_rx = new uSVGLength(100);
			}
			return this.m_rx;
		}
	}	

	public uSVGLength ry {
		get {
			if (this.m_ry == null) {
				this.m_ry = new uSVGLength(100);
			}
			return this.m_ry;
		}
	}
	//================================================================================
	public uSVGEllipseElement(AttributeList attrList,
							uSVGTransformList inheritTransformList,
							uSVGPaintable inheritPaintable,
							uSVGGraphics m_render) : base (inheritTransformList) {
		this.m_attrList = attrList;
		this.m_render = m_render;
		this.m_paintable = new uSVGPaintable(inheritPaintable, this.m_attrList);
		this.m_cx = new uSVGLength(attrList.GetValue("CX"));
		this.m_cy = new uSVGLength(attrList.GetValue("CY"));
		this.m_rx = new uSVGLength(attrList.GetValue("RX"));
		this.m_ry = new uSVGLength(attrList.GetValue("RY"));
		this.currentTransformList = new uSVGTransformList(attrList.GetValue("TRANSFORM"));
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
