using System.Collections.Generic;
public enum uSVGStrokeLineCapTypes {
	UNKNOWN, BUTT, ROUND, SQUARE
}
public enum uSVGStrokeLineJoinTypes {
	UNKNOWN, MITER, ROUND, BEVEL
}

public enum uSVGPaintTypes {
	SVG_PAINT_SOLID_GRADIENT_FILL,
	SVG_PAINT_LINEAR_GRADIENT_FILL,
	SVG_PAINT_RADIAL_GRADIENT_FILL,
	SVG_PAINT_PATH_DRAW,
	SVG_NODRAW
}

public class uSVGPaintable{

	/***********************************************************************************/
	private uSVGColor m_fillColor;
	private uSVGColor m_strokeColor;
	private uSVGLength m_strokeWidth;
	private bool isStrokeWidth = false;
	private uSVGStrokeLineCapTypes m_strokeLineCap = uSVGStrokeLineCapTypes.UNKNOWN;
	private uSVGStrokeLineJoinTypes m_strokeLineJoin = uSVGStrokeLineJoinTypes.UNKNOWN;
	
	//-----------
	private List<uSVGLinearGradientElement> m_linearGradList;
	private List<uSVGRadialGradientElement> m_radialGradList;
	private string m_gradientID = "";
	
	/***********************************************************************************/
	public uSVGColor fillColor {
		get{return this.m_fillColor;}
	}
	public uSVGColor strokeColor {
		get{
			if (IsStroke()) return this.m_strokeColor;
			else return null;
		}
	}
	public float strokeWidth {
		get{return this.m_strokeWidth.value;}
	}
	public uSVGStrokeLineCapTypes strokeLineCap {
		get{ return this.m_strokeLineCap;}
	}
	public uSVGStrokeLineJoinTypes strokeLineJoin {
		get{ return this.m_strokeLineJoin;}
	}
	
	public List<uSVGLinearGradientElement> linearGradList {
		get{ return this.m_linearGradList;}
	}
	public List<uSVGRadialGradientElement> radialGradList {
		get{ return this.m_radialGradList;}
	}
	
	public string gradientID {
		get{ return this.m_gradientID;}
	}
	/***********************************************************************************/
	public uSVGPaintable() {
		this.m_fillColor = new uSVGColor();
		this.m_strokeColor = new uSVGColor();
		this.m_strokeWidth = new uSVGLength("1");
		this.m_linearGradList = new List<uSVGLinearGradientElement>();
		this.m_radialGradList = new List<uSVGRadialGradientElement>();
	}
	public uSVGPaintable(AttributeList attrList) {
		this.m_linearGradList = new List<uSVGLinearGradientElement>();
		this.m_radialGradList = new List<uSVGRadialGradientElement>(); 
		f_Initialize(attrList);
	}
	public uSVGPaintable(uSVGPaintable inheritPaintable, AttributeList attrList) {
		this.m_linearGradList = inheritPaintable.linearGradList;
		this.m_radialGradList = inheritPaintable.radialGradList;;
		f_Initialize(attrList);

		if (IsFillX() == false) {
			if (inheritPaintable.IsLinearGradiantFill()) {
				this.m_gradientID = inheritPaintable.gradientID;
			} else if (inheritPaintable.IsRadialGradiantFill()) {
				this.m_gradientID = inheritPaintable.gradientID;
			} else this.m_fillColor = inheritPaintable.fillColor;
		}
		if (!IsStroke() && inheritPaintable.IsStroke()) {
			this.m_strokeColor = inheritPaintable.strokeColor;
		}
		
		if (m_strokeLineCap == uSVGStrokeLineCapTypes.UNKNOWN) {
			m_strokeLineCap = inheritPaintable.strokeLineCap;
		}

		if (m_strokeLineJoin == uSVGStrokeLineJoinTypes.UNKNOWN) {
			m_strokeLineJoin = inheritPaintable.strokeLineJoin;
		}
		
		if (isStrokeWidth == false) 
			this.m_strokeWidth.NewValueSpecifiedUnits(inheritPaintable.strokeWidth);
	}
	/***********************************************************************************/
	//Khoi tao
	private void f_Initialize(AttributeList attrList) {
		isStrokeWidth = false;

		if (attrList.GetValue("fill").IndexOf("url") >= 0) {
			m_gradientID = uSVGStringExtractor.f_ExtractUrl4Gradient(attrList.GetValue("fill"));
		} else {
			m_fillColor = new uSVGColor(attrList.GetValue("fill"));
		}
		m_strokeColor = new uSVGColor(attrList.GetValue("stroke"));
		
		if (attrList.GetValue("stroke-width") != "") {
			this.isStrokeWidth = true;
		}
		m_strokeWidth = new uSVGLength(attrList.GetValue("stroke-width"));
		

		SetStrokeLineCap(attrList.GetValue("stroke-linecap"));
		SetStrokeLineJoin(attrList.GetValue("stroke-linejoin"));

		if (attrList.GetValue("stroke-width") == "") this.m_strokeWidth.NewValueSpecifiedUnits(1f);
		f_Style(attrList.GetValue("style"));
		//style="fill: #ffffff; stroke:#000000; stroke-width:0.172"
	}
	/***********************************************************************************/
	//Di Phan Tich Style
	private void f_Style(string styleString) {
		Dictionary<string, string> m_dictionary = new Dictionary<string, string>();
		uSVGStringExtractor.f_ExtractStyleValue(styleString, ref m_dictionary);
		if (m_dictionary.ContainsKey("fill")) {
			if (m_dictionary["fill"].IndexOf("url") >= 0) {
				m_gradientID = uSVGStringExtractor.f_ExtractUrl4Gradient(m_dictionary["fill"]);
			} else {
				m_fillColor = new uSVGColor(m_dictionary["fill"]);
			}
		}
		if (m_dictionary.ContainsKey("stroke")) {
			m_strokeColor = new uSVGColor(m_dictionary["stroke"]);
		}
		if (m_dictionary.ContainsKey("stroke-width")) {
			this.isStrokeWidth = true;
			m_strokeWidth = new uSVGLength(m_dictionary["stroke-width"]);
		}
		
		if (m_dictionary.ContainsKey("stroke-linecap")) {
			SetStrokeLineCap(m_dictionary["stroke-linecap"]);
		}
		if (m_dictionary.ContainsKey("stroke-linejoin")) {
			SetStrokeLineJoin(m_dictionary["stroke-linejoin"]);
		}
	}
	/***********************************************************************************/
	private void SetStrokeLineCap(string lineCapType) {
		switch (lineCapType) {
			case "butt" : 	m_strokeLineCap = uSVGStrokeLineCapTypes.BUTT;	break;
			case "round" :	m_strokeLineCap = uSVGStrokeLineCapTypes.ROUND;	break;
			case "square" : m_strokeLineCap = uSVGStrokeLineCapTypes.SQUARE;break;
		}
	}
	private void SetStrokeLineJoin(string lineCapType) {
		switch (lineCapType) {
			case "miter" : 	m_strokeLineJoin = uSVGStrokeLineJoinTypes.MITER;	break;
			case "round" :	m_strokeLineJoin = uSVGStrokeLineJoinTypes.ROUND;	break;
			case "bevel" : 	m_strokeLineJoin = uSVGStrokeLineJoinTypes.BEVEL;	break;
		}
	}
	/***********************************************************************************/
	public bool IsLinearGradiantFill() {
		if (this.m_gradientID == "") {
			return false;
		}
		bool flag = false;
		for (int i=0; i < this.m_linearGradList.Count; i++) {
			if (this.m_linearGradList[i].id == this.m_gradientID) {
				flag = true;
				break;
			}
		}
		return flag;
	}
	//-----
	public bool IsRadialGradiantFill() {
		if (this.m_gradientID == "") {
			return false;
		}
		bool flag = false;
		for (int i=0; i < this.m_radialGradList.Count; i++) {
			if (this.m_radialGradList[i].id == this.m_gradientID) {
				flag = true;
				break;
			}
		}
		return flag;
	}
	//-----
	public bool IsSolidFill() {
		if (this.m_fillColor == null) return false;
		if (this.m_fillColor.colorType == uSVGColorTypes.SVG_COLORTYPE_NONE) {
			return false;
		}
		return true;
	}
	//-----
	public bool IsFill() {
		if (this.m_fillColor == null) {
			if (IsLinearGradiantFill()) return true;
			if (IsRadialGradiantFill()) return true;
			return false;
		}
		if (this.m_fillColor.colorType == uSVGColorTypes.SVG_COLORTYPE_NONE) {
			return false;
		}
		return true;
	}
	//-----
	//Tuc la Fill hien tai
	public bool IsFillX() {
		if (this.m_fillColor == null) {
			if (IsLinearGradiantFill()) return true;
			if (IsRadialGradiantFill()) return true;
			return false;
		}
		if (this.m_fillColor.colorType == uSVGColorTypes.SVG_COLORTYPE_UNKNOWN) {
			return false;
		}
		return true;
	}
	//-----
	public bool IsStroke() {
		if (this.m_strokeColor == null) return false;
		if ((this.m_strokeColor.colorType == uSVGColorTypes.SVG_COLORTYPE_UNKNOWN) ||	
			(this.m_strokeColor.colorType == uSVGColorTypes.SVG_COLORTYPE_NONE)) {
			return false;
		}
		return true;
	}
	//-----
	public uSVGPaintTypes GetPaintType() {
		if (IsLinearGradiantFill()) {
			return uSVGPaintTypes.SVG_PAINT_LINEAR_GRADIENT_FILL;
		}
		if (IsRadialGradiantFill()) {
			return uSVGPaintTypes.SVG_PAINT_RADIAL_GRADIENT_FILL;
		} 
		if (IsSolidFill()) {
			return uSVGPaintTypes.SVG_PAINT_SOLID_GRADIENT_FILL;
		}
		if (IsStroke()) {
			return uSVGPaintTypes.SVG_PAINT_PATH_DRAW;
		}
		
		return uSVGPaintTypes.SVG_NODRAW;
	}
	//----------------------
	//Cong danh sach cac LinearGradient vao trong Paintable
	public void AppendLinearGradient(uSVGLinearGradientElement linearGradElement) {
		this.m_linearGradList.Add(linearGradElement);
	}
	//----------------------
	//Cong danh sach cac RadialGradient vao trong Paintable
	public void AppendRadialGradient(uSVGRadialGradientElement radialGradElement) {
		this.m_radialGradList.Add(radialGradElement);
	}
	//----------------------
	public uSVGLinearGradientBrush GetLinearGradientBrush(uSVGGraphicsPath graphicsPath) {
		for (int i=0; i < this.m_linearGradList.Count; i++) {
			if (this.m_linearGradList[i].id == this.m_gradientID) {
				return new uSVGLinearGradientBrush(this.m_linearGradList[i], graphicsPath);
			}
		}
		return null;
	}
	//----------------------
	public uSVGRadialGradientBrush GetRadialGradientBrush(uSVGGraphicsPath graphicsPath) {
		for (int i=0; i < this.m_radialGradList.Count; i++) {
			if (this.m_radialGradList[i].id == this.m_gradientID) {
				return new uSVGRadialGradientBrush(this.m_radialGradList[i], graphicsPath);
			}
		}
		return null;
	}
}