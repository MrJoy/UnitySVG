public class uSVGLineElement : uSVGTransformable, uISVGDrawable {
	private uSVGLength m_x1;
	private uSVGLength m_y1;
	private uSVGLength m_x2;
	private uSVGLength m_y2;
	/***********************************************************************************/
	private uSVGGraphics m_render;
	private AttributeList m_attrList;
	private uSVGPaintable m_paintable;
	/***********************************************************************************/
	public uSVGLength x1 {
		get {
			return this.m_x1;
		}
	}

	public uSVGLength y1 {
		get {
			return this.m_y1;
		}
	}

	public uSVGLength x2 {
		get {
			return this.m_x2;
		}
	}

	public uSVGLength y2 {
		get {
			return this.m_y2;
		}  			
	}
	/***********************************************************************************/
	public uSVGLineElement(	AttributeList attrList,
							uSVGTransformList inheritTransformList,
							uSVGPaintable inheritPaintable,
							uSVGGraphics m_render) : base (inheritTransformList) {
		this.m_attrList = attrList;
		this.m_paintable = new uSVGPaintable(inheritPaintable, this.m_attrList);
		this.m_render = m_render;
		this.m_x1 = new uSVGLength(attrList.GetValue("X1"));
		this.m_y1 = new uSVGLength(attrList.GetValue("Y1"));
		this.m_x2 = new uSVGLength(attrList.GetValue("X2"));
		this.m_y2 = new uSVGLength(attrList.GetValue("Y2"));
	}
	/***********************************************************************************/
	//Thuc thi Interface Drawable
	public void f_BeforeRender (uSVGTransformList transformList) {
		this.inheritTransformList = transformList;
	}
	public void f_Render () {
		uSVGPoint p1, p2;
		uSVGMatrix m_matrix = this.transformMatrix;
		if (this.m_paintable.strokeColor == null) return;

		float m_width = this.m_paintable.strokeWidth;
		this.m_render.SetStrokeLineCap(this.m_paintable.strokeLineCap);

		float tx1 = this.m_x1.value;
		float ty1 = this.m_y1.value;
		float tx2 = this.m_x2.value;
		float ty2 = this.m_y2.value;
		p1 = new uSVGPoint(tx1, ty1);
		p2 = new uSVGPoint(tx2, ty2);

		p1 = p1.MatrixTransform(m_matrix);
		p2 = p2.MatrixTransform(m_matrix);
		
		this.m_render.Line(p1, p2, this.m_paintable.strokeColor, m_width);
	}
}
