public interface uISVGGraphicsStroke : uISVGPathDraw {
	//================================================================================
	//-----
	//Line
	//-----
	void Line(uSVGPoint p1, uSVGPoint p2);
	void Line(uSVGPoint p1, uSVGPoint p2, float width);
	
	//-----
	//CircleTo
	//-----
	void CircleTo(uSVGPoint p, float r, float width);
	
	//-----
	//EllipseTo
	//-----
	void EllipseTo(uSVGPoint p, float r1, float r2, float angle, float width);
	
	//-----
	//ArcTo
	//-----
	void ArcTo(float r1, float r2, float angle,
				bool largeArcFlag, bool sweepFlag, uSVGPoint p, float width);
				
	//-----
	//CubicCurveTo
	//-----
	void CubicCurveTo(uSVGPoint p1, uSVGPoint p2, uSVGPoint p, float width);
	
	//-----
	//QuadraticCurveTo
	//-----
	void QuadraticCurveTo(uSVGPoint p1, uSVGPoint p, float width);

	//-----
	//LineTo
	//-----
	void LineTo(uSVGPoint p, float width);


	//-----
	//Rect
	//-----	
	//void Rect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4);
	void Rect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, float width);

	//-----
	//RoundedRect
	//-----
	//void RoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
	//			uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
	//			float r1, float r2, float angle);
	void RoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
				uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
				float r1, float r2, float angle, float width);
	//-----
	//Circle
	//-----
	//void Circle(uSVGPoint p, float r);
	void Circle(uSVGPoint p, float r, float width);

	//-----
	//Ellipse
	//-----
	//void Ellipse(uSVGPoint p, float rx, float ry, float angle);
	void Ellipse(uSVGPoint p, float rx, float ry, float angle, float width);
	
	//-----
	//Polygon
	//-----
	//void Polygon(uSVGPoint[] points);
	void Polygon(uSVGPoint[] points, float width);
	
	//-----
	//Polyline
	//-----
	//void Polyline(uSVGPoint[] points);
	void Polyline(uSVGPoint[] points, float width);
	
	//-----
	//DrawPath
	//-----
	void DrawPath(uSVGGraphicsPath graphicsPath);
	void DrawPath(uSVGGraphicsPath graphicsPath, float width);
}
