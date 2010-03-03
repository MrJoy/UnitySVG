public class uSVGLineElement : uSVGTransformable, uISVGDrawable {
	private uSVGAnimatedLength m_x1;
	private uSVGAnimatedLength m_y1;
	private uSVGAnimatedLength m_x2;
	private uSVGAnimatedLength m_y2;
	/***********************************************************************************/
	private uISVGGraphics m_render;
	private AttributeList m_attrList;
	private uSVGPaintable m_paintable;
	/***********************************************************************************/
	public uSVGAnimatedLength x1 {
		get {
			if (this.m_x1 == null) {
				this.m_x1 = new uSVGAnimatedLength(0);
			}
			return this.m_x1;
		}
	}

	public uSVGAnimatedLength y1 {
		get {
			if (this.m_y1 == null) {
				this.m_y1 = new uSVGAnimatedLength(0);
			}
			return this.m_y1;
		}
	}

	public uSVGAnimatedLength x2 {
		get {
			if (this.m_x2 == null) {
				this.m_x2 = new uSVGAnimatedLength(100);
			}
			return this.m_x2;
		}
	}

	public uSVGAnimatedLength y2 {
		get {
			if (this.m_y2 == null) {
				this.m_y2 = new uSVGAnimatedLength(100);
			}
			return this.m_y2;
		}  			
	}
	/***********************************************************************************/
	public uSVGLineElement(	AttributeList attrList,
							uSVGAnimatedTransformList inheritTransformList,
							uSVGPaintable inheritPaintable,
							uISVGGraphics m_render) : base (inheritTransformList) {
		this.m_attrList = attrList;
		this.m_paintable = new uSVGPaintable(inheritPaintable, this.m_attrList);
		this.m_render = m_render;
		this.m_x1 = new uSVGAnimatedLength(attrList.GetValue("x1"));
		this.m_y1 = new uSVGAnimatedLength(attrList.GetValue("y1"));
		this.m_x2 = new uSVGAnimatedLength(attrList.GetValue("x2"));
		this.m_y2 = new uSVGAnimatedLength(attrList.GetValue("y2"));
	}
	/***********************************************************************************/
	//Thuc thi Interface Drawable
	public void f_BeforeRender (uSVGAnimatedTransformList transformList) {
		this.inheritTransformList = transformList;
	}
	public void f_Render () {
		uSVGPoint p1, p2;
		uSVGMatrix m_matrix = this.transformMatrix;
		if (this.m_paintable.strokeColor == null) return;

		float m_width = this.m_paintable.strokeWidth;
		this.m_render.SetStrokeLineCap(this.m_paintable.strokeLineCap);

		float tx1 = this.m_x1.animVal.value;
		float ty1 = this.m_y1.animVal.value;
		float tx2 = this.m_x2.animVal.value;
		float ty2 = this.m_y2.animVal.value;
		p1 = new uSVGPoint(tx1, ty1);
		p2 = new uSVGPoint(tx2, ty2);

		p1 = p1.MatrixTransform(m_matrix);
		p2 = p2.MatrixTransform(m_matrix);
		
		this.m_render.Line(p1, p2, this.m_paintable.strokeColor, m_width);
	}
}
