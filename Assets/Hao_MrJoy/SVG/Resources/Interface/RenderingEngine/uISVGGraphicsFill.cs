//Day la interface danh cho GraphicFill
using UnityEngine;
public interface uISVGGraphicsFill : uISVGPathDraw {
	//================================================================================
	void SetSize(float width, float height);
	//void SetColor(Color color);
	//-----
	void Fill(float x, float y);
	void Fill(uSVGPoint point);
	//-----
	void Fill(float x, float y, int flagStep);
	void Fill(uSVGPoint point, int flagStep);
	//-----
	void BeginSubBuffer();
	//-----
	void EndSubBuffer();
	void EndSubBuffer(uSVGColor strokePathColor);
	
	void EndSubBuffer(uSVGLinearGradientBrush linearGradientBrush);
	void EndSubBuffer(uSVGLinearGradientBrush linearGradientBrush, uSVGColor strokePathColor);
	
	void EndSubBuffer(uSVGRadialGradientBrush radialGradientBrush);
	void EndSubBuffer(uSVGRadialGradientBrush radialGradientBrush, uSVGColor strokePathColor);
	//-----
	//Rect
	//-----
	void Rect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
													uSVGColor strokeColor);
	void Rect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
													uSVGColor fillColor, uSVGColor strokeColor);

	//-----
	//RoundedRect
	//-----
	void RoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
				uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
				float r1, float r2, float angle,
				uSVGColor strokeColor);
	
	void RoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
				uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
				float r1, float r2, float angle,
				uSVGColor fillColor, uSVGColor strokeColor);

	//-----
	//Circle
	//-----
	void Circle(uSVGPoint p, float r, uSVGColor strokeColor);
	void Circle(uSVGPoint p, float r, uSVGColor fillColor, uSVGColor strokeColor);

	//-----
	//Ellipse
	//-----
	void Ellipse(uSVGPoint p, float rx, float ry, float angle, uSVGColor strokeColor);
	void Ellipse(uSVGPoint p, float rx, float ry, float angle, 
													uSVGColor fillColor, uSVGColor strokeColor);

	//-----
	//Polygon
	//-----
	void Polygon(uSVGPoint[] points, uSVGColor strokeColor);
	void Polygon(uSVGPoint[] points, uSVGColor fillColor, uSVGColor strokeColor);

	//-----
	//Polyline
	//-----
	void Polyline(uSVGPoint[] points, uSVGColor strokeColor);
	void Polyline(uSVGPoint[] points, uSVGColor fillColor, uSVGColor strokeColor);





	//-----
	void FillPath(uSVGGraphicsPath graphicsPath);
	void FillPath(uSVGGraphicsPath graphicsPath, uSVGPoint[] points);
	//-----
	//Path Solid Fill
	//-----															
	void FillPath(uSVGColor fillColor, uSVGGraphicsPath graphicsPath);
	void FillPath(uSVGColor fillColor, uSVGColor strokePathColor,
																uSVGGraphicsPath graphicsPath);
	//-----
	//Path Linear Gradient Fill
	//-----
	void FillPath(uSVGLinearGradientBrush linearGradientBrush, uSVGGraphicsPath graphicsPath);
	void FillPath(uSVGLinearGradientBrush linearGradientBrush,
																uSVGColor strokePathColor,
																uSVGGraphicsPath graphicsPath);
	//-----
	//Path Radial Gradient Fill
	//-----
	void FillPath(uSVGRadialGradientBrush radialGradientBrush, uSVGGraphicsPath graphicsPath);
	void FillPath(uSVGRadialGradientBrush radialGradientBrush,
																uSVGColor strokePathColor,
																uSVGGraphicsPath graphicsPath);
	
}
