public interface uISVGPathDraw {
	//================================================================================
	//-----
	//MoveTo
	//-----
	void MoveTo(uSVGPoint p);
	
	//-----
	//CircleTo
	//-----
	void CircleTo(uSVGPoint p, float r);
	
	//-----
	//EllipseTo
	//-----
	void EllipseTo(uSVGPoint p, float r1, float r2, float angle);
				
	//-----
	//ArcTo
	//-----
	void ArcTo(float r1, float r2, float angle,
				bool largeArcFlag, bool sweepFlag, uSVGPoint p);
	
	//-----
	//CubicCurveTo
	//-----
	void CubicCurveTo(uSVGPoint p1, uSVGPoint p2, uSVGPoint p);
	
	//-----
	//QuadraticCurveTo
	//-----
	void QuadraticCurveTo(uSVGPoint p1, uSVGPoint p);

	//-----
	//LineTo
	//-----
	void LineTo(uSVGPoint p);
	
	//-----
	//Rect
	//-----
	void Rect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4);
	
	//-----
	//RoundedRect
	//-----
	void RoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
				uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
				float r1, float r2, float angle);


	//-----
	//Circle
	//-----
	void Circle(uSVGPoint p, float r);
	
	//-----
	//Ellipse
	//-----
	void Ellipse(uSVGPoint p, float rx, float ry, float angle);
	
	//-----
	//Polyline
	//-----
	void Polyline(uSVGPoint[] points);
	
	//-----
	//Polygon
	//-----
	void Polygon(uSVGPoint[] points);
}