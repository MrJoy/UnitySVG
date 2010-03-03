using UnityEngine;
using System.Collections.Generic;
public class uSVGLinearGradientBrush {
	private uSVGLinearGradientElement m_linearGradElement;
	//-----
	//Gradient Vector
	private float m_x1, m_y1, m_x2, m_y2;
	//-----
	private List<Color> m_stopColorList;
	private List<float> m_stopOffsetList;
	//-----
	private uSVGSpreadMethodTypes m_spreadMethod;
	/*********************************************************************************/
	public uSVGLinearGradientBrush (uSVGLinearGradientElement linearGradElement) {
		this.m_linearGradElement = linearGradElement;
		f_Initialize();
	}
	public uSVGLinearGradientBrush (uSVGLinearGradientElement linearGradElement,
														uSVGGraphicsPath graphicsPath) {
		this.m_linearGradElement = linearGradElement;
		f_Initialize();
		
		f_SetGradientVector(graphicsPath);
		f_PreLocationProcess();
	}
	
	
	/*********************************************************************************/
	private void f_Initialize() {
		this.m_x1 = this.m_linearGradElement.x1.animVal.value;
		this.m_y1 = this.m_linearGradElement.y1.animVal.value;
		this.m_x2 = this.m_linearGradElement.x2.animVal.value;
		this.m_y2 = this.m_linearGradElement.y2.animVal.value;

		this.m_stopColorList = new List<Color>();
		this.m_stopOffsetList = new List<float>();
		this.m_spreadMethod = this.m_linearGradElement.spreadMethod;
		
		f_GetStopList();
		this.m_vitriOffset = 0;
		f_PreColorProcess(this.m_vitriOffset);
	}
	//-----
	private void f_GetStopList() {
		List<uSVGStopElement> m_stopList = this.m_linearGradElement.stopList;
		int m_length = m_stopList.Count;
		if (m_length == 0) return;
		
		m_stopColorList.Add(m_stopList[0].stopColor.color);
		m_stopOffsetList.Add(0f);
		int i = 0;
		for (i = 0; i < m_length; i++) {
			float t_offset = m_stopList[i].offset.animVal;
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
	private float m_deltaR, m_deltaG, m_deltaB;
	private int m_vitriOffset = 0;
	private void f_PreColorProcess(int index) {
		float dp = m_stopOffsetList[index + 1] - m_stopOffsetList[index];
		
		m_deltaR = (m_stopColorList[index + 1].r - m_stopColorList[index].r) / dp;
		m_deltaG = (m_stopColorList[index + 1].g - m_stopColorList[index].g) / dp;
		m_deltaB = (m_stopColorList[index + 1].b - m_stopColorList[index].b) / dp;
	}
	//------
	private float m_a, m_b, m_aP, m_bP, m_cP;
	private void f_PreLocationProcess() {		
		if ((this.m_x1 - this.m_x2 == 0f) || (this.m_y1 - this.m_y2 == 0f)) {
			return;
		}
		float dx, dy;
		dx = m_x2 - m_x1;
		dy = m_y2 - m_y1;
			
		this.m_a = dy / dx;
		this.m_b = this.m_y1 - this.m_a * this.m_x1;
			
		this.m_aP = (dx) / ( dx + this.m_a*dy);
		this.m_bP = (dy) / (dx + this.m_a*dy);
		this.m_cP = -(this.m_b*dy) / (dx + this.m_a*dy);
	}
	//-----
	private float f_Percent(float x, float y) {
		float cx, cy;
		if ( this.m_x1 - this. m_x2 == 0) {
			cx = this.m_x1;
			cy = y;
		} else if (this.m_y1 - this. m_y2 == 0) {
			cx = x;
			cy = this.m_y1;
		} else {
			cx = this.m_aP * x + this.m_bP * y + this.m_cP;
			cy = this.m_a * cx + this.m_b;
		}


		float d1 = Mathf.Sqrt((this.m_x1 - cx) * (this.m_x1 - cx) + 
								(this.m_y1 - cy) * (this.m_y1 - cy));
		float d2 = Mathf.Sqrt((this.m_x2 - cx) * (this.m_x2 - cx) + 
								(this.m_y2 - cy) * (this.m_y2 - cy));
		float dd = Mathf.Sqrt((this.m_x2 - this.m_x1) * (this.m_x2 - this.m_x1) + 
					(this.m_y2 - this.m_y1) * (this.m_y2 - this.m_y1));
		//-1 trai, 0 giua, 1 phai
		int vt = 0;
		if ((d1 >= dd) || (d2 >= dd)) {
			if (d1 < d2) vt = -1;
			else vt = 1;
		}
				
		int m_reflectTimes;
		float m_remainder;

		switch (this.m_spreadMethod) {
			case uSVGSpreadMethodTypes.SVG_SPREADMETHOD_PAD : 
				if (vt == -1) return 0f;
				if (vt == 1) return 100f;
				return (d1/dd * 100f);
			case uSVGSpreadMethodTypes.SVG_SPREADMETHOD_REFLECT :				
				m_reflectTimes = (int)(d1 / dd);
				m_remainder = d1 - (dd * (float)m_reflectTimes);
				int m_od = (int)(m_reflectTimes) % 2;
					
				return ((100f * m_od) + (1 - 2 * m_od) * (m_remainder/dd * 100f));
			case uSVGSpreadMethodTypes.SVG_SPREADMETHOD_REPEAT :
				m_reflectTimes = (int)(d1 / dd);
				m_remainder = d1 - (dd * (float)m_reflectTimes);					
				return (m_remainder/dd * 100f);
		}
		
		return 100f;
	}
	//-----
	private void f_SetGradientVector(uSVGGraphicsPath graphicsPath) {
		uSVGRect bound = graphicsPath.GetBound();
		if (this.m_linearGradElement.x1.animVal.unitType == uSVGLengthType.SVG_LENGTHTYPE_PERCENTAGE) {
			this.m_x1 = bound.x + (bound.width * this.m_x1 / 100f);
		}
		
		if (this.m_linearGradElement.y1.animVal.unitType == uSVGLengthType.SVG_LENGTHTYPE_PERCENTAGE) {
			this.m_y1 = bound.y + (bound.height * this.m_y1 / 100f);
		}
		
		if (this.m_linearGradElement.x2.animVal.unitType == uSVGLengthType.SVG_LENGTHTYPE_PERCENTAGE) {
			this.m_x2 = bound.x + (bound.width * this.m_x2 / 100f);
		}
		
		if (this.m_linearGradElement.y2.animVal.unitType == uSVGLengthType.SVG_LENGTHTYPE_PERCENTAGE) {
			this.m_y2 = bound.y + (bound.height * this.m_y2 / 100f);
		}

		if (this.m_linearGradElement.gradientUnits == uSVGGradientUnitType.SVG_OBJECT_BOUNDING_BOX) {
			uSVGPoint m_point = new uSVGPoint(this.m_x1, this.m_y1);
			m_point = m_point.MatrixTransform(graphicsPath.matrixTransform);
			this.m_x1 = m_point.x;
			this.m_y1 = m_point.y;

			m_point = new uSVGPoint(this.m_x2, this.m_y2);
			m_point = m_point.MatrixTransform(graphicsPath.matrixTransform);
			this.m_x2 = m_point.x;
			this.m_y2 = m_point.y;
		}
	}
	/*********************************************************************************/
	/*private float m_ox = 0;
	private int m_dem = 0;
	private bool m_show = false;*/
	public Color GetColor(float x, float y) {
		Color m_color = Color.black;
		

		/*if (m_ox != x) {
			m_ox = x;
			m_dem ++ ;
					
			if (m_dem < 300) {
				m_show = true;
			}
		}*/

		float m_percent = f_Percent(x, y);

		/*if (m_show == true) {
			UnityEngine.Debug.Log("x " + x + " y " + y + " percent " + m_percent);
		}*/

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