using UnityEngine;

public interface uISVGGraphics {
	uSVGStrokeLineCapTypes strokeLineCap{get;}
	uSVGStrokeLineJoinTypes strokeLineJoin{get;}
	//================================================================================
	void SetSize(float width, float height);
	void SetColor(Color color);
	void SetPixel(int x, int y);
	//-----
	void SetStrokeLineCap(uSVGStrokeLineCapTypes strokeLineCap);
	void SetStrokeLineJoin(uSVGStrokeLineJoinTypes strokeLineJoin);
	//-----
	Texture2D Render();
	//-----
	void Clean();
	//-----
	bool GetThickLine(uSVGPoint p1, uSVGPoint p2, float width,
						ref uSVGPoint rp1, ref uSVGPoint rp2, ref uSVGPoint rp3, ref uSVGPoint rp4);
	//-----
	uSVGPoint GetCrossPoint(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4);
	
	//-----
	float AngleBetween2Vector(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4);

	//-----
	//Line
	//-----
	void Line(uSVGPoint p1, uSVGPoint p2);
	void Line(uSVGPoint p1, uSVGPoint p2, uSVGColor strokeColor);
	void Line(uSVGPoint p1, uSVGPoint p2, float width);
	void Line(uSVGPoint p1, uSVGPoint p2, uSVGColor strokeColor, float width);
	
	//-----
	//Rect
	//-----
	void Rect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4);
	void Rect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, uSVGColor strokeColor);
	void Rect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, float width);
	void Rect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4,
														uSVGColor strokeColor, float width);
	//-----
	//RoundedRect
	//-----
	void RoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
				uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
				float r1, float r2, float angle);
	void RoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
				uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
				float r1, float r2, float angle, uSVGColor strokeColor);
	void RoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
				uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
				float r1, float r2, float angle, float width);
	void RoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
				uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
				float r1, float r2, float angle, uSVGColor strokeColor, float width);
	
	//-----
	//FillRect
	//-----
	void FillRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4);
	void FillRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
													uSVGColor strokeColor);
	void FillRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
													uSVGColor fillColor, uSVGColor strokeColor);

	void FillRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4,
												uSVGColor strokeColor, float width);
	void FillRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4,
												uSVGColor fillColor, uSVGColor strokeColor, float width);
	//-----
	//FillRoundedRect
	//-----
	void FillRoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
				uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
				float r1, float r2, float angle);

	void FillRoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
				uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
				float r1, float r2, float angle,
				uSVGColor strokeColor);
	
	void FillRoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
				uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
				float r1, float r2, float angle,
				uSVGColor fillColor, uSVGColor strokeColor);
	
	void FillRoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
				uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
				float r1, float r2, float angle,
				uSVGColor strokeColor, float width);
								
	void FillRoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
				uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
				float r1, float r2, float angle,
				uSVGColor fillColor, uSVGColor strokeColor, float width);
	
	//-----
	//Circle
	//-----
	void Circle(uSVGPoint p, float r);
	void Circle(uSVGPoint p, float r, uSVGColor strokeColor);
	void Circle(uSVGPoint p, float r, float width);
	void Circle(uSVGPoint p, float r, uSVGColor strokeColor, float width);
	//-----
	//Fill Circle
	//-----	
	void FillCircle(uSVGPoint p, float r);
	void FillCircle(uSVGPoint p, float r, uSVGColor strokeColor);
	void FillCircle(uSVGPoint p, float r, uSVGColor fillColor, uSVGColor strokeColor);
	void FillCircle(uSVGPoint p, float r, 
								uSVGColor strokeColor, float width);
	void FillCircle(uSVGPoint p, float r, 
								uSVGColor fillColor, uSVGColor strokeColor, float width);
	//-----
	//Ellipse
	//-----
	void Ellipse(uSVGPoint p, float rx, float ry, float angle);
	void Ellipse(uSVGPoint p, float rx, float ry, float angle, uSVGColor strokeColor);
	void Ellipse(uSVGPoint p, float rx, float ry, float angle, float width);
	void Ellipse(uSVGPoint p, float rx, float ry, float angle, uSVGColor strokeColor, float width);
	
	//-----
	//Fill Ellipse
	//-----	
	void FillEllipse(uSVGPoint p, float rx, float ry, float angle);
	
	void FillEllipse(uSVGPoint p, float rx, float ry, float angle, uSVGColor strokeColor);
	
	void FillEllipse(uSVGPoint p, float rx, float ry, float angle,
								uSVGColor fillColor, uSVGColor strokeColor);

	void FillEllipse(uSVGPoint p, float rx, float ry, float angle,
													uSVGColor strokeColor, float width);

	void FillEllipse(uSVGPoint p, float rx, float ry, float angle,
								uSVGColor fillColor, uSVGColor strokeColor, float width);
	//-----
	//Polygon
	//-----
	void Polygon(uSVGPoint[] points);
	void Polygon(uSVGPoint[] points, uSVGColor strokeColor);
	void Polygon(uSVGPoint[] points, float width);
	void Polygon(uSVGPoint[] points, uSVGColor strokeColor, float width);

	//-----
	//FillPolygon
	//-----
	void FillPolygon(uSVGPoint[] points);
	void FillPolygon(uSVGPoint[] points, uSVGColor strokeColor);
	void FillPolygon(uSVGPoint[] points, uSVGColor fillColor, uSVGColor strokeColor);
	void FillPolygon(uSVGPoint[] points, uSVGColor strokeColor, float width);
	void FillPolygon(uSVGPoint[] points, uSVGColor fillColor, uSVGColor strokeColor, float width);
	
	//-----
	//Polyline
	//-----
	void Polyline(uSVGPoint[] points);
	void Polyline(uSVGPoint[] points, uSVGColor strokeColor);
	void Polyline(uSVGPoint[] points, float width);
	void Polyline(uSVGPoint[] points, uSVGColor strokeColor, float width);
	
	//-----
	//FillPolyline
	//-----
	void FillPolyline(uSVGPoint[] points);
	void FillPolyline(uSVGPoint[] points, uSVGColor strokeColor);
	void FillPolyline(uSVGPoint[] points, uSVGColor fillColor, uSVGColor strokeColor);
	void FillPolyline(uSVGPoint[] points, uSVGColor strokeColor, float width);
	void FillPolyline(uSVGPoint[] points, uSVGColor fillColor, uSVGColor strokeColor, float width);

	//-----
	//Draw Path
	//-----
	void DrawPath(uSVGGraphicsPath graphicsPath);
	void DrawPath(uSVGGraphicsPath graphicsPath, float width);
	void DrawPath(uSVGGraphicsPath graphicsPath, float width, uSVGColor strokePathColor);
	//-----
	//Path Linear Gradient Fill
	//-----
	void FillPath(uSVGLinearGradientBrush linearGradientBrush, 
																uSVGGraphicsPath graphicsPath);
	void FillPath(uSVGLinearGradientBrush linearGradientBrush,
																uSVGColor strokePathColor,
																uSVGGraphicsPath graphicsPath);
	void FillPath(uSVGLinearGradientBrush linearGradientBrush,
																uSVGColor strokePathColor,
																float width,
																uSVGGraphicsPath graphicsPath);
	//-----
	//Path Radial Gradient Fill
	//-----
	void FillPath(uSVGRadialGradientBrush radialGradientBrush, 
																uSVGGraphicsPath graphicsPath);
	void FillPath(uSVGRadialGradientBrush radialGradientBrush,
																uSVGColor strokePathColor,
																uSVGGraphicsPath graphicsPath);
	void FillPath(uSVGRadialGradientBrush radialGradientBrush,
																uSVGColor strokePathColor,
																float width,
																uSVGGraphicsPath graphicsPath);
	//-----
	//Path Solid Fill
	//-----
	void FillPath(uSVGGraphicsPath graphicsPath);
	void FillPath(uSVGColor fillColor, uSVGGraphicsPath graphicsPath);
	void FillPath(uSVGColor fillColor, uSVGColor strokePathColor,
																uSVGGraphicsPath graphicsPath);
	void FillPath(uSVGColor fillColor, uSVGColor strokePathColor,
																float width,
																uSVGGraphicsPath graphicsPath);
	
	//-----
	void FillPath(uSVGGraphicsPath graphicsPath, uSVGPoint[] points);
}