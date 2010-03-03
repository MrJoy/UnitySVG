using UnityEngine;
using System;
using System.Collections.Generic;

public class uSVGRadialGradientBrush {
	private uSVGRadialGradientElement m_radialGradElement;
	//-----
	//Gradient Circle
	private float m_cx, m_cy, m_r, m_fx, m_fy;
	//-----
	private List<Color> m_stopColorList;
	private List<float> m_stopOffsetList;
	//-----
	private uSVGSpreadMethodTypes m_spreadMethod;
	/*********************************************************************************/
	public uSVGRadialGradientBrush (uSVGRadialGradientElement radialGradElement) {
		this.m_radialGradElement = radialGradElement;
		f_Initialize();
	}
	public uSVGRadialGradientBrush (uSVGRadialGradientElement radialGradElement,
														uSVGGraphicsPath graphicsPath) {
		this.m_radialGradElement = radialGradElement;
		f_Initialize();
		
		f_SetGradientVector(graphicsPath);
	}
	/*********************************************************************************/
	private void f_Initialize() {
		this.m_cx = this.m_radialGradElement.cx.value;
		this.m_cy = this.m_radialGradElement.cy.value;
		this.m_r = this.m_radialGradElement.r.value;
		this.m_fx = this.m_radialGradElement.fx.value;
		this.m_fy = this.m_radialGradElement.fy.value;

		this.m_stopColorList = new List<Color>();
		this.m_stopOffsetList = new List<float>();
		this.m_spreadMethod = this.m_radialGradElement.spreadMethod;
		
		f_GetStopList();
		f_FixF();
		this.m_vitriOffset = 0;
		f_PreColorProcess(this.m_vitriOffset);
		
	}
	//-----
	//Sap xep lai Offset va Stop-color List
	private void f_GetStopList() {
		List<uSVGStopElement> m_stopList = this.m_radialGradElement.stopList;
		int m_length = m_stopList.Count;
		if (m_length == 0) return;
		
		m_stopColorList.Add(m_stopList[0].stopColor.color);
		m_stopOffsetList.Add(0f);
		int i = 0;
		for (i = 0; i < m_length; i++) {
			float t_offset = m_stopList[i].offset.value;
			if ((t_offset > m_stopOffsetList[m_stopOffsetList.Count - 1])  && (t_offset <= 100f)) {
				m_stopColorList.Add(m_stopList[i].stopColor.color);
				m_stopOffsetList.Add(t_offset);
			} else if (t_offset == m_stopOffsetList[m_stopOffsetList.Count - 1]){
				m_stopColorList[m_stopOffsetList.Count - 1] = m_stopList[i].stopColor.color;
			}
		}

		if (m_stopOffsetList[m_stopOffsetList.Count - 1] != 100f) {
			m_stopColorList.Add(m_stopColorList[m_stopOffsetList.Count - 1]);
			m_stopOffsetList.Add(100f);
		}
	}
	//-----
	//Sua lai vi tri cua diem x,y
	private void f_FixF() {
		if ((float)Math.Sqrt((this.m_fx-this.m_cx) * (this.m_fx-this.m_cx)) + ((this.m_fy-this.m_cy) * (this.m_fy-this.m_cy)) > this.m_r) {

			float dx = this.m_fx - this.m_cx;
			float dy = this.m_fy - this.m_cy;

			if (dx == 0) {
				this.m_fy = (this.m_fy > this.m_cy) ? (this.m_cy + this.m_r) : (this.m_cy - this.m_r);
			} else {
				float a, b;
				a = dy / dx;
				b = this.m_fy - a * this.m_fx;
			
				double ta, tb, tc;
				
				ta = 1 + a * a;
				tb = 2 * (a * (b - this.m_cy) - this.m_cx);
				tc = (this.m_cx * this.m_cx) + (b - this.m_cy) * (b - this.m_cy) - (this.m_r * this.m_r);

				float delta = (float)((tb * tb) - 4 * ta * tc);
				
				delta = (float)Math.Sqrt(delta);
				float x1 = (float)((-tb + delta) / (2 * ta));
				float y1 = (float)(a * x1 + b);
				float x2 = (float)((-tb - delta) / (2 * ta));
				float y2 = (float)(a * x2 + b);
				
				if ((this.m_cx < x1)  && (x1 < this.m_fx)){
					this.m_fx = x1 - 1;
					this.m_fy = y1;
				} else {
					this.m_fx = x2 + 1;
					this.m_fy = y2;
				}
			}			
		}
	}
	//-----
	private float m_deltaR, m_deltaG, m_deltaB;
	private int m_vitriOffset = 0;
	private void f_PreColorProcess(int index) {
		float dp = m_stopOffsetList[index + 1] - m_stopOffsetList[index];
		
		m_deltaR = (m_stopColorList[index + 1].r - m_stopColorList[index].r) / dp;
		m_deltaG = (m_stopColorList[index + 1].g - m_stopColorList[index].g) / dp;
		m_deltaB = (m_stopColorList[index + 1].b - m_stopColorList[index].b) / dp;
	}
	//----
	//Tim giao diem giua duong thang (x,y) -> (fx, fy) voi duong tron
	private uSVGPoint f_CrossPoint(float x, float y) {
		uSVGPoint m_point = new uSVGPoint(0f, 0f);

		float dx = this.m_fx - x;
		float dy = this.m_fy - y;

		if (dx == 0) {
			m_point.x = this.m_fx;
			m_point.y = (this.m_fy > y) ? (this.m_fy - this.m_r) : (this.m_fy + this.m_r);
		} else {
			float a, b;
			a = dy / dx;
			b = this.m_fy - a * this.m_fx;
			
			double ta, tb, tc;
				
			ta = 1 + a * a;
			tb = 2 * (a * (b - this.m_cy) - this.m_cx);
			tc = (this.m_cx * this.m_cx) + (b - this.m_cy) * (b - this.m_cy) - (this.m_r * this.m_r);

			float delta = (float)((tb * tb) - 4 * ta * tc);
				
			delta = (float)Math.Sqrt(delta);
			float x1 = (float)((-tb + delta) / (2 * ta));
			float y1 = (float)(a * x1 + b);
			float x2 = (float)((-tb - delta) / (2 * ta));
			float y2 = (float)(a * x2 + b);
			
			uSVGPoint vt1 = new uSVGPoint (x1 - this.m_fx, y1 - this.m_fy);
			uSVGPoint vt2 = new uSVGPoint (x - this.m_fx, y - this.m_fy);

			if (((vt1.x * vt2.x) >= 0) && ((vt1.y * vt2.y) >=0)) {
				m_point.x = x1;
				m_point.y = y1;
			} else {
				m_point.x = x2;
				m_point.y = y2;
			}
		}
		return m_point;
	}
	//-----
	//Tinh % tai vi tri x,y
	private float f_Percent(float x, float y) {
		uSVGPoint m_cP = f_CrossPoint(x, y);
		
		//float d1 = (float)Math.Sqrt((m_cP.x - x) * (m_cP.x - x) + (m_cP.y - y) * (m_cP.y - y));
		float d2 = (float)Math.Sqrt((this.m_fx - x) * (this.m_fx - x) + 
								(this.m_fy - y) * (this.m_fy - y));
		float dd = (float)Math.Sqrt((m_cP.x - this.m_fx) * (m_cP.x - this.m_fx) + 
					(m_cP.y - this.m_fy) * (m_cP.y - this.m_fy ));
		//0 giua, 1 ngoai
		int vt = 0;
		if (d2 > dd) {
			vt = 1;
		}
				
		int m_reflectTimes;
		float m_remainder;

		switch (this.m_spreadMethod) {
			case uSVGSpreadMethodTypes.SVG_SPREADMETHOD_PAD : 
				if (vt == 1) return 100f;
				return (d2/dd * 100f);
			case uSVGSpreadMethodTypes.SVG_SPREADMETHOD_REFLECT :				
				m_reflectTimes = (int)(d2 / dd);
				m_remainder = d2 - (dd * (float)m_reflectTimes);
				int m_od = (int)(m_reflectTimes) % 2;
				return ((100f * m_od) + (1 - 2 * m_od) * (m_remainder/dd * 100f));
			case uSVGSpreadMethodTypes.SVG_SPREADMETHOD_REPEAT :
				m_reflectTimes = (int)(d2 / dd);
				m_remainder = d2 - (dd * (float)m_reflectTimes);					
				return (m_remainder/dd * 100f);
		}
		
		return 100f;
	}
	//-----
	private void f_SetGradientVector(uSVGGraphicsPath graphicsPath) {
		uSVGRect bound = graphicsPath.GetBound();
		
		if (this.m_radialGradElement.cx.unitType == uSVGLengthType.SVG_LENGTHTYPE_PERCENTAGE) {
			this.m_cx = bound.x + (bound.width * this.m_cx / 100f);
		}

		if (this.m_radialGradElement.cy.unitType == uSVGLengthType.SVG_LENGTHTYPE_PERCENTAGE) {
			this.m_cy = bound.y + (bound.height * this.m_cy / 100f);
		}

		if (this.m_radialGradElement.r.unitType == uSVGLengthType.SVG_LENGTHTYPE_PERCENTAGE) {
			uSVGPoint m_p1 = new uSVGPoint(bound.x, bound.y);
			uSVGPoint m_p2 = new uSVGPoint(bound.x + bound.width, bound.y + bound.height);
			m_p1 = m_p1.MatrixTransform(graphicsPath.matrixTransform);
			m_p2 = m_p2.MatrixTransform(graphicsPath.matrixTransform);

			float dd = (float)Math.Sqrt((m_p2.x - m_p1.x) * (m_p2.x - m_p1.x) + 
										(m_p2.y - m_p1.y) * (m_p2.y - m_p1.y));
			this.m_r = (dd * this.m_r / 100f);
		}
		
		if (this.m_radialGradElement.fx.unitType == uSVGLengthType.SVG_LENGTHTYPE_PERCENTAGE) {
			this.m_fx = bound.x + (bound.width * this.m_fx / 100f);
		}
		if (this.m_radialGradElement.fy.unitType == uSVGLengthType.SVG_LENGTHTYPE_PERCENTAGE) {
			this.m_fy = bound.y + (bound.height * this.m_fy / 100f);
		}


		if ((float)Math.Sqrt((this.m_cx - this.m_fx) * (this.m_cx - this.m_fx) +
						(this.m_cy - this.m_fy) * (this.m_cy - this.m_fy)) > this.m_r) {
			uSVGPoint m_cP = f_CrossPoint (this.m_cx, this.m_cy);
			this.m_fx = m_cP.x;
			this.m_fy = m_cP.y;
		}



		if (this.m_radialGradElement.gradientUnits == uSVGGradientUnitType.SVG_OBJECT_BOUNDING_BOX) {
			uSVGPoint m_point = new uSVGPoint(this.m_cx, this.m_cy);
			m_point = m_point.MatrixTransform(graphicsPath.matrixTransform);
			this.m_cx = m_point.x;
			this.m_cy = m_point.y;

			m_point = new uSVGPoint(this.m_fx, this.m_fy);
			m_point = m_point.MatrixTransform(graphicsPath.matrixTransform);
			this.m_fx = m_point.x;
			this.m_fy = m_point.y;
		}
	}
	/*********************************************************************************/
	//private float m_ox = 0;
	//private int m_dem = 0;
	//private bool m_show = false;
	public Color GetColor(float x, float y) {
		Color m_color = Color.black;
		

		//if ((m_ox != x) && (y == 200)) {
		//	m_ox = x;
		//	m_dem ++ ;
					
		//	if (m_dem < 300) {
		//		m_show = true;
		//	}
		//}

		float m_percent = f_Percent(x, y);

		//if (m_show == true) {
		//	UnityEngine.Debug.Log("x " + x + " y " + y + " percent " + m_percent);
		//}

		if ((m_stopOffsetList[m_vitriOffset] <= m_percent) &&
							(m_percent <= m_stopOffsetList[m_vitriOffset+1])) {
			m_color.r = ((m_percent - m_stopOffsetList[m_vitriOffset]) * m_deltaR) +
														m_stopColorList[m_vitriOffset].r;
			m_color.g = ((m_percent - m_stopOffsetList[m_vitriOffset]) * m_deltaG) + 
														m_stopColorList[m_vitriOffset].g;
			m_color.b = ((m_percent - m_stopOffsetList[m_vitriOffset]) * m_deltaB) + 
														m_stopColorList[m_vitriOffset].b;
								
		} else {	
			for (int i = 0;  i < m_stopOffsetList.Count - 1; i++) {
				if ((m_stopOffsetList[i] <= m_percent) && (m_percent <= m_stopOffsetList[i+1])) {
					m_vitriOffset = i;
					f_PreColorProcess(m_vitriOffset);
					
					m_color.r = ((m_percent - m_stopOffsetList[i]) * m_deltaR) + 
																m_stopColorList[i].r;
					m_color.g = ((m_percent - m_stopOffsetList[i]) * m_deltaG) + 
																m_stopColorList[i].g;
					m_color.b = ((m_percent - m_stopOffsetList[i]) * m_deltaB) + 
																m_stopColorList[i].b;
					break;
				}
			}
		}
		//m_show = false;
		return m_color;
	}
}
