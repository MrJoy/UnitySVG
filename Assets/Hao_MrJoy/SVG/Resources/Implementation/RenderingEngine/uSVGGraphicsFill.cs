using UnityEngine;
using System.Collections;

public class uSVGGraphicsFill : uISVGPathDraw {
	//================================================================================
	private const int m_fillFlag = -1;
	private float[,] m_neighbor = new float[4, 2] {	{-1.0f, 0.0f},
												{0.0f, -1.0f},
												{1.0f, 0.0f},
												{0.0f, 1.0f}}; 
	
	private uSVGGraphics m_graphics;	
	private uSVGBasicDraw m_basicDraw;
	
	private int m_flagStep;
	private int[,] m_flag;

	//Chieu rong va chieu dai cua picture;
	private int m_width, m_height;
	
	private int m_translateX;
	private int m_translateY;
	
	private int m_subW, m_subH;
	
	//================================================================================
	public uSVGGraphicsFill(uSVGGraphics graphics) {
		this.m_graphics = graphics;
		this.m_flagStep = 0;
		this.m_width = 0;
		this.m_height = 0;

		this.m_translateX = 0;
		this.m_translateY = 0;
		this.m_subW = this.m_subH = 0;
		//Basic Draw
		this.m_basicDraw = new uSVGBasicDraw();
		this.m_basicDraw.SetPixelMethod = new SetPixelDelegate(SetPixelForFlag);
	}
	//================================================================================
	//--------------------------------------------------------------------------------
	//SetSize
	//--------------------------------------------------------------------------------
	public void SetSize(float width, float height) {
		this.m_width = (int)width;
		this.m_height = (int)height;
		this.m_subW = this.m_width;
		this.m_subH = this.m_height;
		this.m_flag = new int[(int)width + 1, (int)height + 1];
	}	
	//--------------------------------------------------------------------------------
	//SetColor
	//--------------------------------------------------------------------------------
	public void SetColor(Color color) {
		this.m_graphics.SetColor(color);
	}
	//--------------------------------------------------------------------------------
	//SetPixelForFlag
	//--------------------------------------------------------------------------------
	private void SetPixelForFlag(int x, int y) {

		int tx = x + this.m_translateX;
		int ty = y + this.m_translateY;
		if (isInZone(tx, ty)) {			
			this.m_flag[tx, ty] = this.m_flagStep;
		}
	}
	
	//--------------------------------------------------------------------------------
	//isInZone
	//--------------------------------------------------------------------------------
	private int m_inZoneL = 0, m_inZoneT = 0;
	private bool isInZone(int x, int y) {
		if ((x>=m_inZoneL && x< this.m_subW+m_inZoneL) && (y>=m_inZoneT && y<this.m_subH+m_inZoneT)) {
			return true;
		}
		return false;
	}
	//================================================================================
	private uSVGPoint m_boundTopLeft;
	private	uSVGPoint m_boundBottomRight;
	//-----
	//Tinh Bound cho Fill
	private void f_ResetLimitPoints(uSVGPoint[] points) {
		int m_length = points.GetLength(0);
		for (int i = 0; i < m_length; i++) {
			if (points[i].x < this.m_boundTopLeft.x) this.m_boundTopLeft.x = points[i].x;
			if (points[i].y < this.m_boundTopLeft.y) this.m_boundTopLeft.y = points[i].y;

			if (points[i].x > this.m_boundBottomRight.x) this.m_boundBottomRight.x = points[i].x;
			if (points[i].y > this.m_boundBottomRight.y) this.m_boundBottomRight.y = points[i].y;
		}
		
	}
	//-----
	private void f_ResetLimitPoints(uSVGPoint[] points, int deltax, int deltay) {
		int m_length = points.GetLength(0);
		for (int i = 0; i < m_length; i++) {
			if ((points[i].x - deltax) < this.m_boundTopLeft.x)
											this.m_boundTopLeft.x = points[i].x - deltax;
			if ((points[i].y - deltay) < this.m_boundTopLeft.y)
											this.m_boundTopLeft.y = points[i].y - deltay;

			if ((points[i].x + deltax) > this.m_boundBottomRight.x)
											this.m_boundBottomRight.x = points[i].x + deltax;
			if ((points[i].y + deltay) > this.m_boundBottomRight.y) 
											this.m_boundBottomRight.y = points[i].y + deltay;
		}
	}
	//--------------------------------------------------------------------------------
	//Method: Fill
	//Fill se to lan tu vi tri (x,y) theo gia tri this.m_flagStep
	//--------------------------------------------------------------------------------
	private void Fill(int x, int y) {
		if (!isInZone(x, y)) return;
		XStack<uSVGPoint> m_stack = new XStack<uSVGPoint>();

		uSVGPoint temp = new uSVGPoint(x, y);
		this.m_flag[(int)temp.x, (int)temp.y] = m_fillFlag;
		m_stack.Push(temp);

    while(m_stack.Count > 0) {
				temp = m_stack.Pop();
				for(int t = 0; t < 4; t++) {
					float tx, ty;
					tx = temp.x + this.m_neighbor[t,0];
					ty = temp.y + this.m_neighbor[t,1];
					if (isInZone((int)tx, (int)ty)) {
						if (this.m_flag[(int)tx, (int)ty] == 0) {
							this.m_flag[(int)tx, (int)ty] = m_fillFlag;
							m_stack.Push(new uSVGPoint(tx, ty));
						}
					}
				}			
		}
	}
	//-----
	public void Fill(float x, float y) {
		Fill((int)x, (int)y);
	}
	//-----
	public void Fill(uSVGPoint point) {
		Fill((int)point.x, (int)point.y);
	}
	//-----
	public void Fill(float x, float y, int flagStep) {
		this.m_flagStep = flagStep;
		Fill((int)x, (int)y);
	}
	//-----
	public void Fill(uSVGPoint point, int flagStep) {
		this.m_flagStep = flagStep;
		Fill((int)point.x, (int)point.y);
	}
	//================================================================================
	//--------------------------------------------------------------------------------
	//Sub Buffer For Path Process
	//--------------------------------------------------------------------------------
	public void BeginSubBuffer() {

		this.m_boundTopLeft 	= new uSVGPoint(+10000f, +10000f);
		this.m_boundBottomRight	= new uSVGPoint(-10000f, -10000f);

		this.m_subW = this.m_width;		
		this.m_subH = this.m_height;
		this.m_inZoneL = 0;
		this.m_inZoneT = 0;
		this.m_translateX = 0;
		this.m_translateY = 0;

		this.m_flagStep = 0;
		for(int i = 0; i < this.m_subW; i++) {
			for (int j = 0; j < this.m_subH; j++) {
				this.m_flag[i,j] = 0;
			}
		}
		this.m_flagStep = 1;
	}
	//-----
	private void f_PreEndSubBuffer() {
		this.m_translateX = 0;
		this.m_translateY = 0;
		
		if (m_boundTopLeft.x < 0f) m_boundTopLeft.x = 0f;
		if (m_boundTopLeft.y < 0f) m_boundTopLeft.y = 0f;
		if (m_boundBottomRight.x >= this.m_width) m_boundBottomRight.x = this.m_width - 1f;
		if (m_boundBottomRight.y >= this.m_height) m_boundBottomRight.y = this.m_height - 1f;
		
		this.m_subW = (int)Mathf.Abs((int)m_boundTopLeft.x - (int)m_boundBottomRight.x) + 1 + (2*1);
		this.m_subH = (int)Mathf.Abs((int)m_boundTopLeft.y - (int)m_boundBottomRight.y) + 1 + (2*1);

		this.m_inZoneL = (int)m_boundTopLeft.x -1;
		this.m_inZoneT = (int)m_boundTopLeft.y -1;
		
		this.m_inZoneL = (m_inZoneL < 0) ? 0 : m_inZoneL;
		this.m_inZoneT = (m_inZoneT < 0) ? 0 : m_inZoneT;
		
		this.m_inZoneL = (m_inZoneL >= this.m_width) ? (this.m_width -1) : m_inZoneL;
		this.m_inZoneT = (m_inZoneT >= this.m_height) ? (this.m_height -1) : m_inZoneT;
		
		this.m_subW = (this.m_subW + this.m_inZoneL >= this.m_width) ?
						(this.m_width - this.m_inZoneL -1 ) : m_subW;
		this.m_subH = (this.m_subH + this.m_inZoneT >= this.m_height) ?
						(this.m_height - this.m_inZoneT -1 ) : m_subH;
						
		//Fill
		Fill(this.m_inZoneL, this.m_inZoneT);
		if ((this.m_inZoneL == 0) && (this.m_inZoneT == 0)) {
			Fill(this.m_inZoneL + this.m_subW - 1, this.m_inZoneT + this.m_subH - 1);
		}
	}
	//-----
	//Fill Solid color, No fill Stroke
	public void EndSubBuffer() {

		f_PreEndSubBuffer();

		Fill(this.m_inZoneL, this.m_inZoneT);
		for(int i = this.m_inZoneL; i < this.m_subW + this.m_inZoneL; i++) {
			for (int j = this.m_inZoneT; j < this.m_subH + this.m_inZoneT; j++) {
				if (this.m_flag[i, j] != -1 ) {					
					this.m_graphics.SetPixel(i, j);
				}				
			}
		}
	}
	//-----
	//Fill Solid color, No fill Stroke
	public void EndSubBuffer(uSVGPoint[] points) {

		f_PreEndSubBuffer();

		for(int i = 0; i < points.GetLength(0); i++) {
			Fill(points[i].x, points[i].y);
		}
		for(int i = this.m_inZoneL; i < this.m_subW + this.m_inZoneL; i++) {
			for (int j = this.m_inZoneT; j < this.m_subH + this.m_inZoneT; j++) {
				if (this.m_flag[i, j] != -1 ) {					
					this.m_graphics.SetPixel(i, j);
				}				
			}
		}
	}
	//-----
	//Fill Solid color, with fill Stroke
	public void EndSubBuffer(uSVGColor strokePathColor) {

		f_PreEndSubBuffer();

		for(int i = this.m_inZoneL; i < this.m_subW + this.m_inZoneL; i++) {
			for (int j = this.m_inZoneT; j < this.m_subH + this.m_inZoneT; j++) {
				if (this.m_flag[i, j] == 0 ) {
					this.m_graphics.SetPixel(i, j);
				}				
			}
		}
			
		this.m_graphics.SetColor(strokePathColor.color);
			
		for(int i = this.m_inZoneL; i < this.m_subW + this.m_inZoneL; i++) {
			for (int j = this.m_inZoneT; j < this.m_subH + this.m_inZoneT; j++) {
				if (this.m_flag[i, j] > 0 ) {
					this.m_graphics.SetPixel(i, j);
				}				
			}
		}
	}
	//-----
	//Fill Linear Gradient, no fill Stroke
	public void EndSubBuffer(uSVGLinearGradientBrush linearGradientBrush) {

		f_PreEndSubBuffer();
		for(int i = this.m_inZoneL; i < this.m_subW + this.m_inZoneL; i++) {
			for (int j = this.m_inZoneT; j < this.m_subH + this.m_inZoneT; j++) {
				if (this.m_flag[i, j] == 0 ) {
					Color m_color = linearGradientBrush.GetColor(i, j);
					this.m_graphics.SetColor(m_color);
					this.m_graphics.SetPixel(i, j);
				}				
			}
		}
	}
	//-----
	//Fill Linear Gradient, with fill Stroke
	public void EndSubBuffer(uSVGLinearGradientBrush linearGradientBrush,
											uSVGColor strokePathColor) {
		f_PreEndSubBuffer();

		for(int i = this.m_inZoneL; i < this.m_subW + this.m_inZoneL; i++) {
			for (int j = this.m_inZoneT; j < this.m_subH + this.m_inZoneT; j++) {
				if (this.m_flag[i, j] != -1 ) {
					Color m_color = linearGradientBrush.GetColor(i, j);
					this.m_graphics.SetColor(m_color);
					this.m_graphics.SetPixel(i, j);
				}				
			}
		}

		this.m_graphics.SetColor(strokePathColor.color);
			
		for(int i = this.m_inZoneL; i < this.m_subW + this.m_inZoneL; i++) {
			for (int j = this.m_inZoneT; j < this.m_subH + this.m_inZoneT; j++) {
				if (this.m_flag[i, j] > 0 ) {
					this.m_graphics.SetPixel(i, j);
				}				
			}
		}
	}
	//-----
	//Fill Radial Gradient, no fill Stroke
	public void EndSubBuffer(uSVGRadialGradientBrush radialGradientBrush) {

		f_PreEndSubBuffer();
		
		for(int i = this.m_inZoneL; i < this.m_subW + this.m_inZoneL; i++) {
			for (int j = this.m_inZoneT; j < this.m_subH + this.m_inZoneT; j++) {
				if (this.m_flag[i, j] == 0 ) {
					Color m_color = radialGradientBrush.GetColor(i, j);
					this.m_graphics.SetColor(m_color);
					this.m_graphics.SetPixel(i, j);
				}				
			}
		}
	}
	//-----
	//Fill Radial Gradient, with fill Stroke
	public void EndSubBuffer(uSVGRadialGradientBrush radialGradientBrush,
											uSVGColor strokePathColor) {
		f_PreEndSubBuffer();

		for(int i = this.m_inZoneL; i < this.m_subW + this.m_inZoneL; i++) {
			for (int j = this.m_inZoneT; j < this.m_subH + this.m_inZoneT; j++) {
				if (this.m_flag[i, j] != -1 ) {
					Color m_color = radialGradientBrush.GetColor(i, j);
					this.m_graphics.SetColor(m_color);
					this.m_graphics.SetPixel(i, j);
				}				
			}
		}

		this.m_graphics.SetColor(strokePathColor.color);
			
		for(int i = this.m_inZoneL; i < this.m_subW + this.m_inZoneL; i++) {
			for (int j = this.m_inZoneT; j < this.m_subH + this.m_inZoneT; j++) {
				if (this.m_flag[i, j] > 0 ) {
					this.m_graphics.SetPixel(i, j);
				}				
			}
		}
	}
	//--------------------------------------------------------------------------------
	//Method: Rect
	//--------------------------------------------------------------------------------
	private void f_PreRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4 ) {
		uSVGPoint[] points = new uSVGPoint[4];
		points[0] = p1;	points[1] = p2; points[2] = p3;	points[3] = p4;
		
		BeginSubBuffer();
		f_ResetLimitPoints(points);
		
		this.m_basicDraw.MoveTo(p1);
		this.m_basicDraw.LineTo(p2);
		this.m_basicDraw.LineTo(p3);
		this.m_basicDraw.LineTo(p4);
		this.m_basicDraw.LineTo(p1);
	}
	//-----
	public void Rect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4) {
		f_PreRect(p1, p2, p3, p4);
		EndSubBuffer();
	}
	//-----
	public void Rect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
													uSVGColor strokeColor) {
		f_PreRect(p1, p2, p3, p4);
		EndSubBuffer(strokeColor);													
	}
	//-----
	public void Rect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
													uSVGColor fillColor, uSVGColor strokeColor) {
		SetColor(fillColor.color);
		f_PreRect(p1, p2, p3, p4);
		EndSubBuffer(strokeColor);
	}
	//--------------------------------------------------------------------------------
	//Method: RoundedRect
	//--------------------------------------------------------------------------------
	private void f_PreRoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
				uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
				float r1, float r2, float angle) {
		uSVGPoint[] points = new uSVGPoint[8];
		points[0] = p1;	points[1] = p2; points[2] = p3;	points[3] = p4;
		points[4] = p5;	points[5] = p6; points[6] = p7;	points[7] = p8;
		
		BeginSubBuffer();
		f_ResetLimitPoints(points, ((r1 > r2)?(int)r1:(int)r2), ((r1 > r2)?(int)r1:(int)r2));
		
		this.m_basicDraw.MoveTo(p1);this.m_basicDraw.LineTo(p2);
		this.m_basicDraw.ArcTo(r1, r2, angle, false, true, p3);

		this.m_basicDraw.MoveTo(p3);this.m_basicDraw.LineTo(p4);
		this.m_basicDraw.ArcTo(r1, r2, angle, false, true, p5);

		this.m_basicDraw.MoveTo(p5);this.m_basicDraw.LineTo(p6);
		this.m_basicDraw.ArcTo(r1, r2, angle, false, true, p7);

		this.m_basicDraw.MoveTo(p7);this.m_basicDraw.LineTo(p8);
		this.m_basicDraw.ArcTo(r1, r2, angle, false, true, p1);
		

	}
	//-----
	public void RoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
				uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
				float r1, float r2, float angle) {
		f_PreRoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle);
		EndSubBuffer();
	}
	//-----
	public void RoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
				uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
				float r1, float r2, float angle,
				uSVGColor strokeColor) {
		f_PreRoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle);
		EndSubBuffer(strokeColor);
	}
	//-----
	public void RoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
				uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
				float r1, float r2, float angle,
				uSVGColor fillColor, uSVGColor strokeColor) {
		SetColor(fillColor.color);
		f_PreRoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle);
		EndSubBuffer(strokeColor);
	}
	//--------------------------------------------------------------------------------
	//Method: CircleFill
	//--------------------------------------------------------------------------------
	private void f_PreCircle(uSVGPoint p, float r) {
		uSVGPoint[] points = new uSVGPoint[1];
		points[0] = p;
		BeginSubBuffer();
		f_ResetLimitPoints(points, (int)r+2, (int)r+2);

		this.m_basicDraw.Circle(p, r);
	}
	//-----
	public void Circle(uSVGPoint p, float r) {
		f_PreCircle(p, r);	
		EndSubBuffer();
	}
	//-----
	public void Circle(uSVGPoint p, float r, uSVGColor strokeColor) {
		f_PreCircle(p, r);	
		EndSubBuffer(strokeColor);
	}
	//-----
	public void Circle(uSVGPoint p, float r, uSVGColor fillColor, uSVGColor strokeColor) {
		SetColor(fillColor.color);
		f_PreCircle(p, r);
		EndSubBuffer();
	}
	//--------------------------------------------------------------------------------
	//Method: Ellipse
	//--------------------------------------------------------------------------------
	private void f_PreEllipse(uSVGPoint p, float rx, float ry, float angle) {
		uSVGPoint[] points = new uSVGPoint[1];
		points[0] = p;
		BeginSubBuffer();
		f_ResetLimitPoints(points, ((rx > ry)?(int)rx:(int)ry), ((rx > ry)?(int)rx:(int)ry));

		this.m_basicDraw.Ellipse(p, (int)rx, (int)ry, angle);
	}
	//-----
	public void Ellipse(uSVGPoint p, float rx, float ry, float angle) {
		f_PreEllipse(p, rx, ry, angle);
		EndSubBuffer();
	}
	//-----
	public void Ellipse(uSVGPoint p, float rx, float ry, float angle, uSVGColor strokeColor) {
		f_PreEllipse(p, rx, ry, angle);
		EndSubBuffer(strokeColor);
	}
	//-----
	public void Ellipse(uSVGPoint p, float rx, float ry, float angle, 
													uSVGColor fillColor, uSVGColor strokeColor) {
		SetColor(fillColor.color);
		f_PreEllipse(p, rx, ry, angle);
		EndSubBuffer(strokeColor);
	}
	//--------------------------------------------------------------------------------
	//Method: Polygon.
	//--------------------------------------------------------------------------------
	private void f_PrePolygon(uSVGPoint[] points) {
		if ((points != null) && (points.GetLength(0) > 0)) {
			BeginSubBuffer();
			f_ResetLimitPoints(points);
			
			this.m_basicDraw.MoveTo(points[0]);
			int m_length = points.GetLength(0);
			for (int i = 1; i < m_length; i++) {
				this.m_basicDraw.LineTo(points[i]);
			}
			this.m_basicDraw.LineTo(points[0]);
		}
	}
	//-----
	public void Polygon(uSVGPoint[] points) {
		f_PrePolygon(points);
		EndSubBuffer();
	}
	//-----
	public void Polygon(uSVGPoint[] points, uSVGColor strokeColor) {
		f_PrePolygon(points);
		EndSubBuffer(strokeColor);
	}
	//-----
	public void Polygon(uSVGPoint[] points, uSVGColor fillColor, uSVGColor strokeColor) {
		SetColor(fillColor.color);
		f_PrePolygon(points);
		EndSubBuffer(strokeColor);
	}
	//--------------------------------------------------------------------------------
	//Method: Polyline.
	//--------------------------------------------------------------------------------
	public void Polyline(uSVGPoint[] points) {
		Polygon(points);
	}
	//-----
	public void Polyline(uSVGPoint[] points, uSVGColor strokeColor) {
		Polygon(points, strokeColor);
	}
	//-----
	public void Polyline(uSVGPoint[] points, uSVGColor fillColor, uSVGColor strokeColor) {
		Polygon(points, fillColor, strokeColor);
	}

	//--------------------------------------------------------------------------------
	//Method: Fill Path
	//--------------------------------------------------------------------------------
	public void FillPath(uSVGGraphicsPath graphicsPath) {
		BeginSubBuffer();
		graphicsPath.RenderPath(this, true);
		EndSubBuffer();
	}
	//-----
	public void FillPath(uSVGGraphicsPath graphicsPath, uSVGPoint[] points) {
		BeginSubBuffer();
		graphicsPath.RenderPath(this, true);
		EndSubBuffer(points);
	}
	//-----
	//Path Solid Fill
	public void FillPath(uSVGColor fillColor, uSVGGraphicsPath graphicsPath) {
		SetColor(fillColor.color);
		FillPath(graphicsPath);
	}
	//-----
	public void FillPath(uSVGColor fillColor, uSVGColor strokePathColor,
																uSVGGraphicsPath graphicsPath) {
		BeginSubBuffer();
		graphicsPath.RenderPath(this, true);
		if (strokePathColor != null) {
			EndSubBuffer(strokePathColor);
		} else {
			EndSubBuffer();
		}
	}
	//-----
	//Path Linear Fill
	public void FillPath(uSVGLinearGradientBrush linearGradientBrush, uSVGGraphicsPath graphicsPath) {
		BeginSubBuffer();
		graphicsPath.RenderPath(this, true);
		EndSubBuffer(linearGradientBrush);
	}
	//-----
	public void FillPath(uSVGLinearGradientBrush linearGradientBrush,
																uSVGColor strokePathColor,
																uSVGGraphicsPath graphicsPath) {
		BeginSubBuffer();
		graphicsPath.RenderPath(this, true);
		if (strokePathColor != null) {
			EndSubBuffer(linearGradientBrush, strokePathColor);
		} else {
			EndSubBuffer(linearGradientBrush);
		}
	}
	//-----
	//Path Radial Fill
	public void FillPath(uSVGRadialGradientBrush radialGradientBrush, uSVGGraphicsPath graphicsPath) {
		BeginSubBuffer();
		graphicsPath.RenderPath(this, true);
		EndSubBuffer(radialGradientBrush);
	}
	//-----
	public void FillPath(uSVGRadialGradientBrush radialGradientBrush,
																uSVGColor strokePathColor,
																uSVGGraphicsPath graphicsPath) {
		BeginSubBuffer();
		graphicsPath.RenderPath(this, true);
		if (strokePathColor != null) {
			EndSubBuffer(radialGradientBrush, strokePathColor);
		} else {
			EndSubBuffer(radialGradientBrush);
		}
	}
	
	//================================================================================
	//--------------------------------------------------------------------------------
	//Method: CircleTo
	//--------------------------------------------------------------------------------
	public void CircleTo(uSVGPoint p, float r) {
		uSVGPoint[] points = new uSVGPoint[1];
		points[0] = p;
		f_ResetLimitPoints(points, (int)r+1, (int)r+1);

		//---------------
		this.m_basicDraw.Circle(p, r);
	}
	//--------------------------------------------------------------------------------
	//Method: EllipseTo
	//--------------------------------------------------------------------------------
	public void EllipseTo(uSVGPoint p, float rx, float ry, float angle) {
		uSVGPoint[] points = new uSVGPoint[1];
		points[0] = p;
		f_ResetLimitPoints(points, ((rx > ry)?(int)rx+2:(int)ry+2), ((rx > ry)?(int)rx+2:(int)ry+2));

		//---------------
		this.m_basicDraw.Ellipse(p, (int)rx, (int)ry, angle);
	}
	//--------------------------------------------------------------------------------
	//Method: LineTo4Path
	//--------------------------------------------------------------------------------
	public void LineTo(uSVGPoint p) {
		uSVGPoint[] points = new uSVGPoint[1];
		points[0] = p;
		f_ResetLimitPoints(points);
		//---------------
		this.m_basicDraw.LineTo(p);
	}
	//--------------------------------------------------------------------------------
	//Method: MoveTo
	//--------------------------------------------------------------------------------
	public void MoveTo(uSVGPoint p) {
		uSVGPoint[] points = new uSVGPoint[1];
		points[0] = p;
		f_ResetLimitPoints(points);
		//---------------
		this.m_basicDraw.MoveTo(p);
	}
	
	/*-------------------------------------------------------------------------------
	//Method: Arc4Path
	/-------------------------------------------------------------------------------*/	
	public void ArcTo(float r1, float r2, float angle,
				bool largeArcFlag, bool sweepFlag,
				uSVGPoint p) {
		uSVGPoint[] points = new uSVGPoint[1];
		points[0] = p;
		f_ResetLimitPoints(points,
					(r1>r2)?2*(int)r1+2:2*(int)r2+2,
					(r1>r2)?2*(int)r1+2:2*(int)r2+2);
		//---------------
		this.m_basicDraw.ArcTo(r1, r2, angle, largeArcFlag, sweepFlag, p);
	}
	/*-------------------------------------------------------------------------------
	//Method: CubicCurveTo4Path
	/-------------------------------------------------------------------------------*/	
	public void CubicCurveTo(uSVGPoint p1, uSVGPoint p2, uSVGPoint p) {
		uSVGPoint[] points = new uSVGPoint[3];
		points[0] = p1;
		points[1] = p2;
		points[2] = p;
		f_ResetLimitPoints(points);
		//---------------
		this.m_basicDraw.CubicCurveTo(p1, p2, p);
	}
	
	/*-------------------------------------------------------------------------------
	//Method: QuadraticCurveTo4Path
	/-------------------------------------------------------------------------------*/
	public void QuadraticCurveTo(uSVGPoint p1, uSVGPoint p) {
		uSVGPoint[] points = new uSVGPoint[2];
		points[0] = p1;
		points[1] = p;
		f_ResetLimitPoints(points);
		//---------------
		this.m_basicDraw.QuadraticCurveTo(p1, p);
	}
}
