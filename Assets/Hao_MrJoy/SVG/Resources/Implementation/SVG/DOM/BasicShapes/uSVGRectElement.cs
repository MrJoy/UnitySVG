public class uSVGRectElement : uSVGTransformable, uISVGDrawable {
	private uSVGAnimatedLength m_x;
	private uSVGAnimatedLength m_y;
	private uSVGAnimatedLength m_width;
	private uSVGAnimatedLength m_height;
	private uSVGAnimatedLength m_rx;
	private uSVGAnimatedLength m_ry;	
	//================================================================================
	private uSVGGraphics m_render;
	private AttributeList m_attrList;
	private uSVGPaintable m_paintable;
	//================================================================================
	public uSVGAnimatedLength x {
		get {
			if (this.m_x == null) {
				this.m_x = new uSVGAnimatedLength(0);
			}
			return this.m_x;
			}
		}

	public uSVGAnimatedLength y {
		get {
			if (this.m_y == null) {
				this.m_y = new uSVGAnimatedLength(0);
			}
			return this.m_y;
		}
	}

	public uSVGAnimatedLength width {
		get {
			if (this.m_width == null) {
				this.m_width =  new uSVGAnimatedLength(100);
			}
			return this.m_width;
		}
	}
		
	public uSVGAnimatedLength height {
		get {
			if (this.m_height == null) {
				this.m_height =  new uSVGAnimatedLength(100);
			}
			return this.m_height;
		}
	}


	public uSVGAnimatedLength rx {
		get {
			if (this.m_rx == null) {
				this.m_rx = new uSVGAnimatedLength(0);
			}
			return this.m_rx;
		}
	}	

	public uSVGAnimatedLength ry {
		get {
			if (this.m_ry == null) {
				this.m_ry = new uSVGAnimatedLength(0);
			}
			return this.m_ry;
		}
	}
	//================================================================================
	public uSVGRectElement(AttributeList attrList,
							uSVGAnimatedTransformList inheritTransformList,
							uSVGPaintable inheritPaintable,
							uSVGGraphics m_render) : base (inheritTransformList) {
		this.m_attrList = attrList;
		this.m_render = m_render;
		this.m_paintable = new uSVGPaintable(inheritPaintable, this.m_attrList);
		this.m_x = new uSVGAnimatedLength(attrList.GetValue("x"));
		this.m_y = new uSVGAnimatedLength(attrList.GetValue("y"));
		this.m_width = new uSVGAnimatedLength(attrList.GetValue("width"));
		this.m_height = new uSVGAnimatedLength(attrList.GetValue("height"));
		this.m_rx = new uSVGAnimatedLength(attrList.GetValue("rx"));
		this.m_ry = new uSVGAnimatedLength(attrList.GetValue("ry"));
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