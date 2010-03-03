// TODO: Convert to use UnityEngine.Vector2
public struct uSVGPoint {
	private float m_x;
	private float m_y;

	/***********************************************************************************/
	public float x {
		get{ return this.m_x;}
		set{ this.m_x = value;}
	}
	public float y {
		get{ return this.m_y;}
		set{ this.m_y = value;}
	}
	public static uSVGPoint operator + (uSVGPoint a, uSVGPoint b) {
		return new uSVGPoint(a.x + b.x, a.y + b.y);
	}
	public static uSVGPoint operator - (uSVGPoint a, uSVGPoint b) {
		return new uSVGPoint(a.x - b.x, a.y - b.y);
	}
	public static uSVGPoint operator * (uSVGPoint a, uSVGPoint b) {
		return new uSVGPoint(a.x * b.x, a.y * b.y);
	}	
	public static uSVGPoint operator / (uSVGPoint a, uSVGPoint b) {
		return new uSVGPoint(a.x / b.x, a.y / b.y);
	}
	public static uSVGPoint operator * (float t, uSVGPoint a) {
		return new uSVGPoint(t * a.x, t * a.y);
	}
	public static uSVGPoint operator + (float t, uSVGPoint a) {
		return new uSVGPoint(t + a.x, t + a.y);
	}

	/***********************************************************************************/
	public uSVGPoint(float x, float y) {
		this.m_x = x;
		this.m_y = y;
	}
	public void SetValue(float x, float y) {
		this.m_x = x;
		this.m_y = y;
	}
	public void SetValue(uSVGPoint point) {
		this.m_x = point.x;
		this.m_y = point.y;
	}

	public uSVGPoint MatrixTransform (uSVGMatrix matrix) {
		float a,b,c,d,e,f;
		a = matrix.a;
		b = matrix.b;
		c = matrix.c;
		d = matrix.d;
		e = matrix.e;
		f = matrix.f;
		return new uSVGPoint(a*x + c*y + e, b*x + d*y +f);
	}
}