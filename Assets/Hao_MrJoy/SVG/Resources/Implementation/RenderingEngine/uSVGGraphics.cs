using UnityEngine;
using System.Collections.Generic;

public class uSVGGraphics {
	//================================================================================
	private uSVGDevice m_device;

	private uSVGGraphicsFill m_graphicsFill;
	private uSVGGraphicsStroke m_graphicsStroke;
	
	private int m_width, m_height;
	
	private uSVGStrokeLineCapTypes 	m_strokeLineCap		= uSVGStrokeLineCapTypes.UNKNOWN;
	private uSVGStrokeLineJoinTypes	m_strokeLineJoin	= uSVGStrokeLineJoinTypes.UNKNOWN;
	//================================================================================
	public uSVGStrokeLineCapTypes strokeLineCap {
		get{ return this.m_strokeLineCap;}
	}
	//-----
	public uSVGStrokeLineJoinTypes strokeLineJoin {
		get{ return this.m_strokeLineJoin;}
	}
	//================================================================================
	public uSVGGraphics() {
		this.m_graphicsFill = new uSVGGraphicsFill(this);
		this.m_graphicsStroke = new uSVGGraphicsStroke(this);
	}
	//-----
	public uSVGGraphics(uSVGDevice device) {
		this.m_device = device;
		this.m_graphicsFill = new uSVGGraphicsFill(this);
		this.m_graphicsStroke = new uSVGGraphicsStroke(this);
	}
	//================================================================================
	//--------------------------------------------------------------------------------
	//Method: SetSize
	//--------------------------------------------------------------------------------
	public void SetSize(float width, float height) {
		this.m_device.f_SetDevice(width, height);
		this.m_device.GetBufferSize(ref m_width, ref m_height);
		this.m_graphicsFill.SetSize((int)width, (int)height);
		Clean();
	}
	//--------------------------------------------------------------------------------
	//Method: SetColor
	//--------------------------------------------------------------------------------
	public void SetColor(Color color) {	
		this.m_device.SetColor(color);
	}

	//--------------------------------------------------------------------------------
	//Method: SetPixel
	//--------------------------------------------------------------------------------
	public void SetPixel(int x, int y) {
		this.m_device.SetPixel(x, y);
	}
	//--------------------------------------------------------------------------------
	//Method: SetStrokeLineCap
	//--------------------------------------------------------------------------------
	public void SetStrokeLineCap(uSVGStrokeLineCapTypes strokeLineCap) {
		this.m_strokeLineCap = strokeLineCap;
	}
	//--------------------------------------------------------------------------------
	//Method: SetStrokeLineJoin
	//--------------------------------------------------------------------------------
	public void SetStrokeLineJoin(uSVGStrokeLineJoinTypes strokeLineJoin) {
		this.m_strokeLineJoin = strokeLineJoin;
	}
	//--------------------------------------------------------------------------------
	//Method: Render
	//--------------------------------------------------------------------------------
	public Texture2D Render() {
		return this.m_device.Render();
	}
	//--------------------------------------------------------------------------------
	//Method: Clean
	//--------------------------------------------------------------------------------
	public void Clean() {
		int width=0, height=0;
		SetColor(Color.white);
		m_device.GetBufferSize(ref width, ref height);
		for(int i=0; i<width; i++) {
			for (int j=0; j<height; j++) {
				SetPixel(i, j);
			}
		}
	}
	//--------------------------------------------------------------------------------
	//GetThickLine
	//--------------------------------------------------------------------------------
	//Tinh 4 diem 1, 2, 3, 4 cua 1 line voi width
	public bool GetThickLine(uSVGPoint p1, uSVGPoint p2, float width,
						ref uSVGPoint rp1, ref uSVGPoint rp2, ref uSVGPoint rp3, ref uSVGPoint rp4) {
							
		float cx1, cy1, cx2, cy2, cx3, cy3, cx4, cy4;
		float dtx, dty, temp, m_half;
		int m_ihalf1, m_ihalf2;
		
		m_half = width / 2f;
		m_ihalf1 = (int)m_half;
		m_ihalf2 = (int)(width - m_ihalf1 + 0.5f);
		
		dtx = p2.x - p1.x;
		dty = p2.y - p1.y;
		temp = dtx * dtx + dty * dty;
		if (temp == 0f) {
			rp1.x = p1.x - m_ihalf2;
			rp1.y = p1.y + m_ihalf2;

			rp2.x = p1.x - m_ihalf2;
			rp2.y = p1.y - m_ihalf2;
			
			rp3.x = p1.x + m_ihalf1;
			rp3.y = p1.y + m_ihalf1;

			rp4.x = p1.x + m_ihalf1;
			rp4.y = p1.y - m_ihalf1;
			return false;
		}

		cy1 = m_ihalf1 * dtx / Mathf.Sqrt(temp) + p1.y;
		if (dtx == 0) {
			if (dty > 0) {
				cx1 = p1.x - m_ihalf1;
			} else {
				cx1 = p1.x + m_ihalf1;
			}
		} else {
			cx1 = (-(cy1 - p1.y) * dty) / dtx + p1.x;
		}

		cy2 = -(m_ihalf2 * dtx / Mathf.Sqrt(temp)) + p1.y;
		if (dtx == 0) {
			if (dty > 0) {
				cx2 = p1.x + m_ihalf2;
			} else {
				cx2 = p1.x - m_ihalf2;
			}
		} else {
			cx2 = (-(cy2 - p1.y) * dty) / dtx + p1.x;
		}
	
		dtx = p1.x - p2.x;
		dty = p1.y - p2.y;
		temp = dtx * dtx + dty * dty;		

		cy3 = m_ihalf1 * dtx / Mathf.Sqrt(temp) + p2.y;
		if (dtx == 0) {
			if (dty > 0) {
				cx3 = p2.x - m_ihalf1;
			} else {
				cx3 = p2.x + m_ihalf1;
			}
		} else {
			cx3 = (-(cy3 - p2.y) * dty) / dtx + p2.x;
		}
		
		cy4 = -(m_ihalf2 * dtx / Mathf.Sqrt(temp)) + p2.y;
		
		if (dtx == 0) {
			if (dty > 0) {
				cx4 = p2.x + m_ihalf2;
			} else {
				cx4 = p2.x - m_ihalf2;
			}
		} else {
			cx4 = (-(cy4 - p2.y) * dty) / dtx + p2.x;
		}

		rp1.x = cx1; rp1.y = cy1;

		rp2.x = cx2; rp2.y = cy2;

		float t1,t2;
		t1 = ((p1.y - cy1) * (p2.x - p1.x)) - ((p1.x - cx1) * (p2.y - p1.y));
		t2 = ((p1.y - cy4) * (p2.x - p1.x)) - ((p1.x - cx4) * (p2.y - p1.y));
		if (t1 * t2 > 0) {
			//bi lech
			if (m_ihalf1 != m_ihalf2) {
				cy3 = m_ihalf2 * dtx / Mathf.Sqrt(temp) + p2.y;
				if (dtx == 0) {
					if (dty > 0) {
						cx3 = p2.x - m_ihalf2;
					} else {
						cx3 = p2.x + m_ihalf2;
					}
				} else {
					cx3 = (-(cy3 - p2.y) * dty) / dtx + p2.x;
				}
		
				cy4 = -(m_ihalf1 * dtx / Mathf.Sqrt(temp)) + p2.y;
		
				if (dtx == 0) {
					if (dty > 0) {
						cx4 = p2.x + m_ihalf1;
					} else {
						cx4 = p2.x - m_ihalf1;
					}
				} else {
					cx4 = (-(cy4 - p2.y) * dty) / dtx + p2.x;
				}
			}
		
			rp3.x = cx4; rp3.y = cy4;
			rp4.x = cx3; rp4.y = cy3;
		} else {
			rp3.x = cx3; rp3.y = cy3;
			rp4.x = cx4; rp4.y = cy4;
		}
		return true;
	}
	//--------------------------------------------------------------------------------
	//GetCrossPoint
	//--------------------------------------------------------------------------------
	//Tinh diem giao nhau giua 2 doan thang
	public uSVGPoint GetCrossPoint(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4) {

		uSVGPoint m_return = new uSVGPoint(0f, 0f);
		float a1 = 0f, b1 = 0f, a2 = 0f, b2 = 0f;
		
		float dx1, dy1, dx2, dy2;
		dx1 = p1.x - p2.x;
		dy1 = p1.y - p2.y;
		dx2 = p3.x - p4.x;
		dy2 = p3.y - p4.y;
		
		if (dx1 != 0f) {
			a1 = dy1 / dx1;
			b1 = p1.y - a1 * p1.x;
		}
		
		if (dx2 != 0) {		
			a2 = dy2 / dx2;
			b2 = p3.y - a2 * p3.x;
		}
		//-----
		float tx = 0f, ty = 0f;

		//truong hop nam tren duong thang
		if ((a1 == a2) && (b1 == b2)) {
			uSVGPoint t_p1 = p1;
			uSVGPoint t_p2 = p1;
			if (dx1 == 0f) {
				if (p2.y < t_p1.y) t_p1=p2;
				if (p3.y < t_p1.y) t_p1=p3;
				if (p4.y < t_p1.y) t_p1=p4;
				
				if (p2.y > t_p2.y) t_p2=p2;
				if (p3.y > t_p2.y) t_p2=p3;
				if (p4.y > t_p2.y) t_p2=p4;
			} else {
				if (p2.x < t_p1.x) t_p1=p2;
				if (p3.x < t_p1.x) t_p1=p3;
				if (p4.x < t_p1.x) t_p1=p4;
				
				if (p2.x > t_p2.x) t_p2=p2;
				if (p3.x > t_p2.x) t_p2=p3;
				if (p4.x > t_p2.x) t_p2=p4;
			}
			
			tx = (t_p1.x  - t_p2.x)/2f;
			tx = t_p2.x + tx;

			ty = (t_p1.y  - t_p2.y)/2f;
			ty = t_p2.y + ty;

			m_return.x = tx;
			m_return.y = ty;
			return m_return;
		}
		

		
		if ((dx1 != 0) && (dx2 != 0)) {
			tx = - (b1 - b2) / (a1 - a2);
			ty = a1 * tx + b1;
		} else if ((dx1 == 0) && (dx2 != 0)) {
			tx = p1.x;
			ty = a2 * tx + b2;
		} else if ((dx1 != 0) && (dx2 == 0)) {
			tx = p3.x;
			ty = a1 * tx + b1;
		}

		m_return.x = tx;
		m_return.y = ty;
		return m_return;
	}
	//--------------------------------------------------------------------------------
	//AngleBetween2Vector
	//--------------------------------------------------------------------------------
	//Tinh goc giua 2 vector  (p1,p2)   (p3,p4);
	public float AngleBetween2Vector(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4) {
		uSVGPoint vt1, vt2;
		vt1 = new uSVGPoint(p2.x - p1.x, p2.y - p1.y);
		vt2 = new uSVGPoint(p4.x - p3.x, p4.y - p3.y);
		float t1 = vt1.x*vt2.x + vt1.y*vt2.y;
		float gtvt1 = Mathf.Sqrt(vt1.x * vt1.x + vt1.y*vt1.y);
		float gtvt2 = Mathf.Sqrt(vt2.x * vt2.x + vt2.y*vt2.y);
		float t2 = gtvt1 * gtvt2;
		float cosAngle = t1/t2;
		
		return (Mathf.Acos(cosAngle));
	}
	//================================================================================
	//--------------------------------------------------------------------------------
	//Method: Line
	//--------------------------------------------------------------------------------
	public void Line(uSVGPoint p1, uSVGPoint p2) {
		this.m_graphicsStroke.Line(p1, p2);
	}
	//-----
	public void Line(uSVGPoint p1, uSVGPoint p2, uSVGColor strokeColor) {
		if (strokeColor != null) {
			SetColor(strokeColor.color);
		}
		Line(p1, p2);
	}
	//-----
	public void Line(uSVGPoint p1, uSVGPoint p2, float width) {
		this.m_graphicsStroke.Line(p1, p2, width);
	}
	//-----
	public void Line(uSVGPoint p1, uSVGPoint p2, uSVGColor strokeColor, float width) {
		if (strokeColor != null) {
			SetColor(strokeColor.color);
		}
		Line(p1, p2, width);
	}
	//--------------------------------------------------------------------------------
	//Method: Rect
	//--------------------------------------------------------------------------------
	public void Rect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4) {
		this.m_graphicsStroke.Rect(p1, p2, p3, p4);
	}
	//-----
	public void Rect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, uSVGColor strokeColor) {
		if (strokeColor !=null ) {
			SetColor(strokeColor.color);
		}
		Rect(p1, p2, p3, p4);
	}
	//-----
	public void Rect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, float width) {
		this.m_graphicsStroke.Rect(p1, p2, p3, p4, width);
		/*
		if ((int)width == 1) {
			Rect(p1, p2, p3, p4);
		}
		uSVGPoint[] points = new uSVGPoint[4];
		points[0] = p1;
		points[1] = p2;
		points[2] = p3;
		points[3] = p4;
		Polygon(points, width);
		*/
	}
	//-----
	public void Rect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4,
	 													uSVGColor strokeColor, float width) {
		if (strokeColor !=null ) {
			SetColor(strokeColor.color);
		}
		Rect(p1, p2, p3, p4, width);
	}
	//--------------------------------------------------------------------------------
	//Method: Rounded Rect
	//--------------------------------------------------------------------------------
	public void RoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
			uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
			float r1, float r2, float angle) {
		
		this.m_graphicsStroke.RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle);
	}
	//-----
	public void RoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
			uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
			float r1, float r2, float angle, uSVGColor strokeColor) {

		if (strokeColor != null) {
			SetColor(strokeColor.color);
		}
		RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle);
	}
	//-----
	public void RoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
			uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
			float r1, float r2, float angle, float width) {
		
		if ((int)width == 1) {
			RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle);
			return;
		}
		this.m_graphicsStroke.RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle, width);
		/*
		Line(p1, p2, width); Line(p3, p4, width); Line(p5, p6, width); Line(p7, p8, width);
		uSVGPoint m_p1 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p2 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p3 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p4 = new uSVGPoint(0f, 0f);

		GetThickLine(p1, p2, width,
					ref m_p1, ref m_p2, ref m_p3, ref m_p4);
					
		uSVGPoint m_p5 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p6 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p7 = new uSVGPoint(0f, 0f);
		uSVGPoint m_p8 = new uSVGPoint(0f, 0f);

		//-------
		GetThickLine(p3, p4, width,
					ref m_p5, ref m_p6, ref m_p7, ref m_p8);
		
		this.m_graphicsFill.BeginSubBuffer();
		this.m_graphicsFill.MoveTo4Path(m_p4);
		this.m_graphicsFill.ArcTo4Path(r1 + (width/2f), r2 + (width/2f),
									angle, false, true, m_p6);
		this.m_graphicsFill.LineTo4Path(m_p5);
		this.m_graphicsFill.ArcTo4Path(r1 - (width/2f), r2 - (width/2f),
									angle, false, false, m_p3);
		this.m_graphicsFill.LineTo4Path(m_p4);
		this.m_graphicsFill.EndSubBuffer();
		
		//-------
		GetThickLine(p5, p6, width,
					ref m_p1, ref m_p2, ref m_p3, ref m_p4);
					
		this.m_graphicsFill.BeginSubBuffer();
		this.m_graphicsFill.MoveTo4Path(m_p8);
		this.m_graphicsFill.ArcTo4Path(r1 + (width/2f), r2 + (width/2f),
									angle, false, true, m_p2);
		this.m_graphicsFill.LineTo4Path(m_p1);
		this.m_graphicsFill.ArcTo4Path(r1 - (width/2f), r2 - (width/2f),
									angle, false, false, m_p7);
		this.m_graphicsFill.LineTo4Path(m_p8);
		this.m_graphicsFill.EndSubBuffer();
		
		//----------
		GetThickLine(p7, p8, width,
					ref m_p5, ref m_p6, ref m_p7, ref m_p8);
		
		this.m_graphicsFill.BeginSubBuffer();
		this.m_graphicsFill.MoveTo4Path(m_p4);
		this.m_graphicsFill.ArcTo4Path(r1 + (width/2f), r2 + (width/2f),
									angle, false, true, m_p6);
		this.m_graphicsFill.LineTo4Path(m_p5);
		this.m_graphicsFill.ArcTo4Path(r1 - (width/2f), r2 - (width/2f),
									angle, false, false, m_p3);
		this.m_graphicsFill.LineTo4Path(m_p4);
		this.m_graphicsFill.EndSubBuffer();
		
		//-------
		GetThickLine(p1, p2, width,
					ref m_p1, ref m_p2, ref m_p3, ref m_p4);
					
		this.m_graphicsFill.BeginSubBuffer();
		this.m_graphicsFill.MoveTo4Path(m_p8);
		this.m_graphicsFill.ArcTo4Path(r1 + (width/2f), r2 + (width/2f),
									angle, false, true, m_p2);
		this.m_graphicsFill.LineTo4Path(m_p1);
		this.m_graphicsFill.ArcTo4Path(r1 - (width/2f), r2 - (width/2f),
									angle, false, false, m_p7);
		this.m_graphicsFill.LineTo4Path(m_p8);
		this.m_graphicsFill.EndSubBuffer();
		*/
		
	}
	//-----
	public void RoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
			uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
			float r1, float r2, float angle, uSVGColor strokeColor, float width) {

		if (strokeColor != null) {
			SetColor(strokeColor.color);
		}
		RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle, width);
	}
	//--------------------------------------------------------------------------------
	//Method: FillRect
	//--------------------------------------------------------------------------------
	public void FillRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4) {
		this.m_graphicsFill.Rect(p1, p2, p3, p4);
	}
	//-----
	public void FillRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
													uSVGColor strokeColor) {
		this.m_graphicsFill.Rect(p1, p2, p3, p4, strokeColor);
	}
	//-----
	public void FillRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
													uSVGColor fillColor, uSVGColor strokeColor) {
		this.m_graphicsFill.Rect(p1, p2, p3, p4, fillColor, strokeColor);
	}
	//-----
	public void FillRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4,
												uSVGColor strokeColor, float width) {
		if ((int)width == 1) {
			FillRect(p1, p2, p3, p4, strokeColor);
			return;
		}
		FillRect(p1, p2, p3, p4);
		if (strokeColor == null) return;
		SetColor(strokeColor.color);
		Rect(p1, p2, p3, p4, strokeColor, width);
	}
	//-----
	public void FillRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4,
											uSVGColor fillColor, uSVGColor strokeColor, float width) {
		if ((int)width == 1) {
			FillRect(p1, p2, p3, p4, fillColor, strokeColor);
			return;
		}
		SetColor(fillColor.color);
		FillRect(p1, p2, p3, p4);
		if (strokeColor == null) return;
		SetColor(strokeColor.color);
		Rect(p1, p2, p3, p4, strokeColor, width);										
	}
	//--------------------------------------------------------------------------------
	//Method: FillRoundedRect
	//--------------------------------------------------------------------------------
	public void FillRoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
				uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
				float r1, float r2, float angle) {
		this.m_graphicsFill.RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle);
	}
	//-----
	public void FillRoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
				uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
				float r1, float r2, float angle,
				uSVGColor strokeColor) {
		this.m_graphicsFill.RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle, strokeColor);
	}
	//-----
	public void FillRoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
				uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
				float r1, float r2, float angle,
				uSVGColor fillColor, uSVGColor strokeColor) {
		this.m_graphicsFill.RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle,
																			fillColor, strokeColor);
	}
	//-----
	public void FillRoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
				uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
				float r1, float r2, float angle,
				uSVGColor strokeColor, float width) {
		if ((int)width == 1) {
			FillRoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle, strokeColor);
			return;
		}
		this.m_graphicsFill.RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle);
		if (strokeColor == null) return;
		SetColor(strokeColor.color);
		RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle, strokeColor);
	}
	//-----
	public void FillRoundedRect(uSVGPoint p1, uSVGPoint p2, uSVGPoint p3, uSVGPoint p4, 
				uSVGPoint p5, uSVGPoint p6, uSVGPoint p7, uSVGPoint p8,
				float r1, float r2, float angle,
				uSVGColor fillColor, uSVGColor strokeColor, float width) {

		if ((int)width == 1) {
			FillRoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle, strokeColor);
			return;
		}
		SetColor(fillColor.color);
		this.m_graphicsFill.RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle);
		if (strokeColor == null) return;
		SetColor(strokeColor.color);
		RoundedRect(p1, p2, p3, p4, p5, p6, p7, p8, r1, r2, angle, strokeColor);
	}

	//--------------------------------------------------------------------------------
	//Method: Circle
	//--------------------------------------------------------------------------------
	public void Circle(uSVGPoint p, float r) {
		this.m_graphicsStroke.Circle(p, r);
	}
	//-----
	public void Circle(uSVGPoint p, float r, uSVGColor strokeColor) {
		if (strokeColor !=null ) {
			SetColor(strokeColor.color);
		}
		Circle(p, r);
	}
	//-----
	public void Circle(uSVGPoint p, float r, float width) {
		this.m_graphicsStroke.Circle(p, r, width);
	}
	//-----
	public void Circle(uSVGPoint p, float r,
									uSVGColor strokeColor, float width) {
		if (strokeColor != null) {
			SetColor(strokeColor.color);
		}
		Circle(p, r, width);
	}
	//--------------------------------------------------------------------------------
	//Method: FillCircle
	//--------------------------------------------------------------------------------
	public void FillCircle(uSVGPoint p, float r)	{
		this.m_graphicsFill.Circle(p, r);
	}
	//-----
	public void FillCircle(uSVGPoint p, float r, uSVGColor strokeColor)	{
		this.m_graphicsFill.Circle(p, r, strokeColor);
	}
	//-----
	public void FillCircle(uSVGPoint p, float r, uSVGColor fillColor, uSVGColor strokeColor)	{
		this.m_graphicsFill.Circle(p, r, fillColor, strokeColor);
	}
	//-----
	public void FillCircle(uSVGPoint p, float r,
							uSVGColor strokeColor, float width) {
		if ((int)width == 1) {
			FillCircle(p, r, strokeColor);
			return;
		}
		
		FillCircle(p, r);
		if (strokeColor == null) return;
		SetColor(strokeColor.color);
		Circle(p, r, strokeColor, width);
	}
	//-----
	public void FillCircle(uSVGPoint p, float r,
							uSVGColor fillColor, uSVGColor strokeColor, float width) {
		if ((int)width == 1) {
			FillCircle(p, r, strokeColor);
			return;
		}
		SetColor(fillColor.color);
		FillCircle(p, r);
		if (strokeColor == null) return;
		SetColor(strokeColor.color);
		Circle(p, r, strokeColor, width);
	}
	//--------------------------------------------------------------------------------
	//Method: Ellipse
	//--------------------------------------------------------------------------------
	public void Ellipse(uSVGPoint p, float rx, float ry, float angle) {
		this.m_graphicsStroke.Ellipse(p, rx, ry, angle);
	}
	//-----
	public void Ellipse(uSVGPoint p, float rx, float ry, float angle, uSVGColor strokeColor) {
		if (strokeColor !=null ) {
			SetColor(strokeColor.color);
		}
		Ellipse(p, rx, ry, angle);
	}
	//-----
	public void Ellipse(uSVGPoint p, float rx, float ry, float angle, float width) {
		this.m_graphicsStroke.Ellipse(p, rx, ry, angle, width);
	}
	//-----
	public void Ellipse(uSVGPoint p, float rx, float ry, float angle,
														uSVGColor strokeColor, float width) {
		if (strokeColor != null) {
			SetColor(strokeColor.color);
		}
		Ellipse(p, rx, ry, angle, width);
	}
	//--------------------------------------------------------------------------------
	//Method: FillEllipse
	//--------------------------------------------------------------------------------
	public void FillEllipse(uSVGPoint p, float rx, float ry, float angle) {
		this.m_graphicsFill.Ellipse(p, rx, ry, angle);
	}
	//-----
	public void FillEllipse(uSVGPoint p, float rx, float ry, float angle, uSVGColor strokeColor) {
		this.m_graphicsFill.Ellipse(p, rx, ry, angle, strokeColor);
	}
	//-----
	public void FillEllipse(uSVGPoint p, float rx, float ry, float angle,
								uSVGColor fillColor, uSVGColor strokeColor) {
		this.m_graphicsFill.Ellipse(p, rx, ry, angle, fillColor, strokeColor);
	}
	//-----
	public void FillEllipse(uSVGPoint p, float rx, float ry, float angle,
													uSVGColor strokeColor, float width) {
		if ((int)width == 1) {
			FillEllipse(p, rx, ry, angle, strokeColor);
			return;
		}
		
		FillEllipse(p, rx, ry, angle);
		if (strokeColor == null) return;
		SetColor(strokeColor.color);
		Ellipse(p, rx, ry, angle, width);

	}
	//-----
	public void FillEllipse(uSVGPoint p, float rx, float ry, float angle,
								uSVGColor fillColor, uSVGColor strokeColor, float width) {
		if ((int)width == 1) {
			FillEllipse(p, rx, ry, angle, strokeColor);
			return;
		}
		SetColor(fillColor.color);
		FillEllipse(p, rx, ry, angle);
		if (strokeColor == null) return;
		SetColor(strokeColor.color);
		Ellipse(p, rx, ry, angle, width);

	}
	//--------------------------------------------------------------------------------
	//Method: Polygon
	//--------------------------------------------------------------------------------
	public void Polygon(uSVGPoint[] points) {
		this.m_graphicsStroke.Polygon(points);
	}
	//-----
	public void Polygon(uSVGPoint[] points, uSVGColor strokeColor) {
		if (strokeColor !=null ) {
			SetColor(strokeColor.color);
		}
		Polygon(points);
	}
	//-----
	public void Polygon(uSVGPoint[] points, float width) {
		this.m_graphicsStroke.Polygon(points, width);
	}
	//-----
	public void Polygon(uSVGPoint[] points, uSVGColor strokeColor, float width) {
		if (strokeColor !=null ) {
			SetColor(strokeColor.color);
		}
		Polygon(points, width);
	}
	//--------------------------------------------------------------------------------
	//Method: FillPolygon
	//--------------------------------------------------------------------------------
	public void FillPolygon(uSVGPoint[] points) {
		this.m_graphicsFill.Polygon(points);
	}
	//-----
	public void FillPolygon(uSVGPoint[] points, uSVGColor strokeColor) {
		this.m_graphicsFill.Polygon(points, strokeColor);
	}
	//-----
	public void FillPolygon(uSVGPoint[] points, uSVGColor fillColor, uSVGColor strokeColor) {
		this.m_graphicsFill.Polygon(points, fillColor, strokeColor);
	}
	//-----
	public void FillPolygon(uSVGPoint[] points, uSVGColor strokeColor, float width) {
		if ((int)width == 1) {
			FillPolygon(points, strokeColor);
			return;
		}
		FillPolygon(points);
		if (strokeColor == null) return;
		SetColor(strokeColor.color);
		Polygon(points, width);
	}
	//-----
	public void FillPolygon(uSVGPoint[] points,
						uSVGColor fillColor, uSVGColor strokeColor, float width) {
		if ((int)width == 1) {
			FillPolygon(points, strokeColor);
			return;
		}
		SetColor(fillColor.color);
		FillPolygon(points);
		if (strokeColor == null) return;
		SetColor(strokeColor.color);
		Polygon(points, width);
	}
	//--------------------------------------------------------------------------------
	//Method: Polyline
	//--------------------------------------------------------------------------------
	public void Polyline(uSVGPoint[] points) {
		this.m_graphicsStroke.Polyline(points);
	}
	//-----
	public void Polyline(uSVGPoint[] points, uSVGColor strokeColor) {
		if (strokeColor !=null ) {
			SetColor(strokeColor.color);
		}
		Polyline(points);
	}
	//-----
	public void Polyline(uSVGPoint[] points, float width) {
		this.m_graphicsStroke.Polyline(points, width);
	}
	//-----
	public void Polyline(uSVGPoint[] points, uSVGColor strokeColor, float width) {
		if (strokeColor !=null ) {
			SetColor(strokeColor.color);
		}
		Polyline(points, width);
	}
	//--------------------------------------------------------------------------------
	//Method: FillPolyline
	//--------------------------------------------------------------------------------
	public void FillPolyline(uSVGPoint[] points) {
		this.m_graphicsFill.Polyline(points);
	}
	//-----
	public void FillPolyline(uSVGPoint[] points, uSVGColor strokeColor) {
		this.m_graphicsFill.Polyline(points, strokeColor);
	}
	//-----
	public void FillPolyline(uSVGPoint[] points, uSVGColor fillColor, uSVGColor strokeColor) {
		this.m_graphicsFill.Polyline(points, fillColor, strokeColor);
	}
	//-----
	public void FillPolyline(uSVGPoint[] points, uSVGColor strokeColor, float width) {
		if ((int)width == 1) {
			FillPolyline(points, strokeColor);
			return;
		}
		FillPolyline(points);
		if (strokeColor == null) return;
		SetColor(strokeColor.color);
		Polyline(points, width);
	}
	//-----
	public void FillPolyline(uSVGPoint[] points,
						uSVGColor fillColor, uSVGColor strokeColor, float width) {
		if ((int)width == 1) {
			FillPolyline(points, strokeColor);
			return;
		}
		SetColor(fillColor.color);
		FillPolyline(points);
		if (strokeColor == null) return;
		SetColor(strokeColor.color);
		Polyline(points, width);
	}

	//================================================================================
	//--------------------------------------------------------------------------------
	//Path Linear Gradient Fill
	//--------------------------------------------------------------------------------
	//Fill khong to Stroke
	public void FillPath(uSVGLinearGradientBrush linearGradientBrush,
																uSVGGraphicsPath graphicsPath) {
		this.m_graphicsFill.FillPath(linearGradientBrush, graphicsPath);
	}
	//-----
	//Fill co Stroke trong do luon
	public void FillPath(uSVGLinearGradientBrush linearGradientBrush,
															uSVGColor strokePathColor,
															uSVGGraphicsPath graphicsPath) {
		this.m_graphicsFill.FillPath(linearGradientBrush, strokePathColor, graphicsPath);
	}
	//-----
	//Fill khong co Stroke, va ve stroke sau
	public void FillPath(uSVGLinearGradientBrush linearGradientBrush,
															uSVGColor strokePathColor,
															float width,
															uSVGGraphicsPath graphicsPath) {

		this.m_graphicsFill.FillPath(linearGradientBrush, strokePathColor, graphicsPath);

		if ((int)width == 1)
			this.m_graphicsFill.FillPath(linearGradientBrush, strokePathColor, graphicsPath);
		else this.m_graphicsFill.FillPath(linearGradientBrush, graphicsPath);
		
		if (strokePathColor == null) return;
		SetColor(strokePathColor.color);
	}
	//--------------------------------------------------------------------------------
	//Path Radial Gradient Fill
	//--------------------------------------------------------------------------------
	//Fill khong to Stroke
	public void FillPath(uSVGRadialGradientBrush radialGradientBrush,
																uSVGGraphicsPath graphicsPath) {
		this.m_graphicsFill.FillPath(radialGradientBrush, graphicsPath);
	}
	//-----
	//Fill co Stroke trong do luon
	public void FillPath(uSVGRadialGradientBrush radialGradientBrush,
															uSVGColor strokePathColor,
															uSVGGraphicsPath graphicsPath) {
		this.m_graphicsFill.FillPath(radialGradientBrush, strokePathColor, graphicsPath);
	}
	//-----
	//Fill khong co Stroke, va ve stroke sau
	public void FillPath(uSVGRadialGradientBrush radialGradientBrush,
															uSVGColor strokePathColor,
															float width,
															uSVGGraphicsPath graphicsPath) {
		this.m_graphicsFill.FillPath(radialGradientBrush, strokePathColor, graphicsPath);
		if ((int)width == 1) 
			this.m_graphicsFill.FillPath(radialGradientBrush, strokePathColor, graphicsPath);
		else this.m_graphicsFill.FillPath(radialGradientBrush, graphicsPath);
		
		if (strokePathColor == null) return;
		SetColor(strokePathColor.color);
		//graphicsPath.RenderPath(this, width, false);
	}
	//--------------------------------------------------------------------------------
	//Path Solid Fill
	//--------------------------------------------------------------------------------
	//Fill khong to Stroke
	public void FillPath( uSVGGraphicsPath graphicsPath) {
		this.m_graphicsFill.FillPath(graphicsPath);
	}
	//Fill khong to Stroke
	public void FillPath(uSVGColor fillColor, uSVGGraphicsPath graphicsPath) {
		this.m_graphicsFill.FillPath(fillColor, graphicsPath);
	}
	//-----
	//Fill co Stroke trong do luon
	public void FillPath(uSVGColor fillColor, uSVGColor strokePathColor,
															uSVGGraphicsPath graphicsPath) {
		this.m_graphicsFill.FillPath(fillColor, strokePathColor, graphicsPath);
	}
	//-----
	//Fill khong co Stroke, va ve stroke sau
	public void FillPath(uSVGColor fillColor, uSVGColor strokePathColor,
															float width,
															uSVGGraphicsPath graphicsPath) {
		this.m_graphicsFill.FillPath(fillColor, strokePathColor, graphicsPath);
		if ((int)width == 1) this.m_graphicsFill.FillPath(fillColor, strokePathColor, graphicsPath);
		else this.m_graphicsFill.FillPath(fillColor, graphicsPath);
		
		if (strokePathColor == null) return;
		SetColor(strokePathColor.color);
	}
	//-----
	public void FillPath( uSVGGraphicsPath graphicsPath, uSVGPoint[] points) {
		this.m_graphicsFill.FillPath(graphicsPath, points);
	}
	//================================================================================
	//--------------------------------------------------------------------------------
	//Draw Path
	//--------------------------------------------------------------------------------
	//DrawPath
	public void DrawPath(uSVGGraphicsPath graphicsPath) {
		this.m_graphicsStroke.DrawPath(graphicsPath);
	}
	//-----
	//Fill co Stroke trong do luon
	public void DrawPath(uSVGGraphicsPath graphicsPath, float width) {
		this.m_graphicsStroke.DrawPath(graphicsPath, width);
	}
	//-----
	//Fill khong co Stroke, va ve stroke sau
	public void DrawPath(uSVGGraphicsPath graphicsPath, float width, uSVGColor strokePathColor) {
		if (strokePathColor == null) return;
		SetColor(strokePathColor.color);
		this.m_graphicsStroke.DrawPath(graphicsPath, width);
	}
}