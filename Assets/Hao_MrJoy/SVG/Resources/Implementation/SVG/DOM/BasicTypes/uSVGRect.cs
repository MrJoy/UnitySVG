public class uSVGRect {
	private float m_x;
	private float m_y;
	private float m_width;
	private float m_height;
	/***********************************************************************************/
	public float x {
		get {return this.m_x;}
	}
	public float y {
		get {return this.m_y;}
	}
	public float width {
		get {return this.m_width;}
	}
		public float height {
		get {return this.m_height;}
	}
	/***********************************************************************************/
	public uSVGRect(float x, float y, float width, float height) {
		this.m_x = x;
		this.m_y = y;
		this.m_width = width;
		this.m_height = height;
	}
}