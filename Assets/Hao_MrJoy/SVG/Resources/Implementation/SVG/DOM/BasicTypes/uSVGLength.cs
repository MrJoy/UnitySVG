public enum uSVGLengthType : ushort {
	SVG_LENGTHTYPE_UNKNOWN = 0,
	SVG_LENGTHTYPE_NUMBER = 1,
	SVG_LENGTHTYPE_PERCENTAGE = 2,
	SVG_LENGTHTYPE_EMS = 3,
	SVG_LENGTHTYPE_EXS = 4,
	SVG_LENGTHTYPE_PX = 5,
	SVG_LENGTHTYPE_CM = 6,
	SVG_LENGTHTYPE_MM = 7,
	SVG_LENGTHTYPE_IN = 8,
	SVG_LENGTHTYPE_PT = 9,
	SVG_LENGTHTYPE_PC = 10,
}
/**************************************************************************************************/
public struct uSVGLength  {
		private uSVGLengthType m_unitType;
		private float m_valueInSpecifiedUnits;

		/***********************************************************************************/
		public float value {
			get{return uSVGLengthConvertor.f_ConvertToPX(this.m_valueInSpecifiedUnits, this.m_unitType);}
		}
		public uSVGLengthType unitType {
			get{ return this.m_unitType;}
		}
		/***********************************************************************************/
		public uSVGLength(ushort unitType, float valueInSpecifiedUnits) {
			m_unitType = (uSVGLengthType)unitType;
			m_valueInSpecifiedUnits = valueInSpecifiedUnits;
		}
		public uSVGLength(float valueInSpecifiedUnits) {
			m_unitType = (uSVGLengthType)0;
			m_valueInSpecifiedUnits = valueInSpecifiedUnits;
		}
		public uSVGLength(string valueText) {
			float t_value = 0.0f;
			uSVGLengthType t_type = uSVGLengthType.SVG_LENGTHTYPE_UNKNOWN;
			uSVGLengthConvertor.f_ExtractType(valueText, ref t_value, ref t_type);
			m_unitType = t_type;			
			m_valueInSpecifiedUnits = t_value;
		}
		/***********************************************************************************/
		public void NewValueSpecifiedUnits(ushort unitType, float valueInSpecifiedUnits) {
			this.m_unitType = (uSVGLengthType)unitType;
			this.m_valueInSpecifiedUnits = valueInSpecifiedUnits;
		}
		public void NewValueSpecifiedUnits(float valueInSpecifiedUnits) {
			NewValueSpecifiedUnits(0, valueInSpecifiedUnits);
		}
		public static float GetPXLength(string valueText) {
			float t_value = 0.0f;
			uSVGLengthType t_type = uSVGLengthType.SVG_LENGTHTYPE_UNKNOWN;
			uSVGLengthConvertor.f_ExtractType(valueText, ref t_value, ref t_type);
			return uSVGLengthConvertor.f_ConvertToPX(t_value, t_type);
		}

		/***********************************************************************************/
		private float f_GetValue(uSVGLengthType unitType) {

			float temp = uSVGLengthConvertor.f_ConvertToPX(this.value, this.m_unitType);
			switch (unitType) {
				case uSVGLengthType.SVG_LENGTHTYPE_IN :
					return temp / 90.0f;
				case uSVGLengthType.SVG_LENGTHTYPE_CM :
					return temp / 35.43307f;
				case uSVGLengthType.SVG_LENGTHTYPE_MM :
					return temp / 3.543307f;
				case uSVGLengthType.SVG_LENGTHTYPE_PT :
					return temp / 1.25f;
				case uSVGLengthType.SVG_LENGTHTYPE_PC :
					return temp / 15.0f;
				default:
					return temp;
			}
		}
}