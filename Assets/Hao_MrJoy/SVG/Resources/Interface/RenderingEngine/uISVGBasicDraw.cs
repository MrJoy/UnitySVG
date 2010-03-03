//Inteface danh cho uSVGBasicDraw
public interface uISVGBasicDraw  {

	uSVGPoint currentPoint { get;}
	SetPixelDelegate SetPixelMethod { set;}
	//========================================================================================
	//----------
	//MOVE TO
	void MoveTo(float x, float y);
	void MoveTo(uSVGPoint p);

	//----------
	//LINE
	void Line(float x0, float y0, float x1, float y1);
	void Line(uSVGPoint p1, uSVGPoint p2);
	
	//----------
	//LINE TO
	void LineTo(float x, float y);
	void LineTo(uSVGPoint p);

	//----------
	//RECT
	void Rect(float x0, float y0, float x1, float y1);
	void Rect(uSVGPoint p1, uSVGPoint p2);
	void Rect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4);
	
	//----------
	//CIRCLE
	void Circle(float x0, float y0, float r);
	void Circle(uSVGPoint p, float r);
	
	//----------
	//ELLIPSE
	void Ellipse(float x0, float y0, float rx, float ry, float angle);
	void Ellipse(uSVGPoint p, float rx, float ry, float angle);
	
	//----------
	//ARC
	void Arc(uSVGPoint p1, float rx, float ry, float angle,
				bool largeArcFlag, bool sweepFlag, uSVGPoint p2);
	
	//----------
	//ARC TO
	void ArcTo(float r1, float r2, float angle,
				bool largeArcFlag, bool sweepFlag, uSVGPoint p);
				
	//----------
	//CUBIC CURVE
	void CubicCurve(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4);
	
	//----------
	//CUBIC CURVE TO
	void CubicCurveTo(uSVGPoint p1, uSVGPoint p2, uSVGPoint p);

	//----------
	//QUADRATIC CURVE
	void QuadraticCurve(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3);

	//----------
	//QUADRATIC CURVE TO
	void QuadraticCurveTo(uSVGPoint p1, uSVGPoint p);
}