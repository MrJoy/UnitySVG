public enum uSVGSpreadMethodTypes : ushort {
	SVG_SPREADMETHOD_UNKNOWN	= 0,
	SVG_SPREADMETHOD_PAD		= 1,
	SVG_SPREADMETHOD_REFLECT 	= 2,
	SVG_SPREADMETHOD_REPEAT		= 3
}

public enum uSVGGradientUnitType : ushort {
	SVG_USER_SPACE_ON_USE		= 0,
	SVG_OBJECT_BOUNDING_BOX		= 1
}
public class uSVGGradientElement {
	private uSVGGradientUnitType 		m_gradientUnits;
	private uSVGAnimatedTransformList 	m_gradientTransfrom;
	private uSVGSpreadMethodTypes		m_spreadMethod;
	/***************************************************************************/
	private AttributeList m_attrList;
	/***************************************************************************/
	public uSVGGradientUnitType gradientUnits {
		get{ return this.m_gradientUnits;}
	}
	
	public uSVGSpreadMethodTypes spreadMethod {
		get{ return this.m_spreadMethod;}
	}
	/***************************************************************************/
	public uSVGGradientElement(AttributeList attrList) {
		this.m_attrList = attrList;
		f_Initialize();
	}
	/***************************************************************************/
	private void f_Initialize() {
		m_gradientUnits = uSVGGradientUnitType.SVG_OBJECT_BOUNDING_BOX;
		if (this.m_attrList.GetValue("GRADIENTUNITS").ToLower() == "userspaceonuse") {
			m_gradientUnits = uSVGGradientUnitType.SVG_USER_SPACE_ON_USE;
		}
		
		//------
		// TODO: It's probably a bug that the value is not innoculated for CaSe 
		// VaRiAtIoN in GetValue, below:
		m_spreadMethod = uSVGSpreadMethodTypes.SVG_SPREADMETHOD_PAD;
		if (this.m_attrList.GetValue("SPREADMETHOD") == "reflect") {
			m_spreadMethod = uSVGSpreadMethodTypes.SVG_SPREADMETHOD_REFLECT;
		} else if (this.m_attrList.GetValue("SPREADMETHOD") == "repeat") {
			m_spreadMethod = uSVGSpreadMethodTypes.SVG_SPREADMETHOD_REPEAT;
		}
	}
}
