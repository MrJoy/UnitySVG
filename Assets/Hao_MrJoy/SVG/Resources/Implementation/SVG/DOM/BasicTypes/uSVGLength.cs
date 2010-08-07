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
  private float m_valueInSpecifiedUnits, m_value;

  /***********************************************************************************/
  public float value {
    get { return m_value; }
  }
  public uSVGLengthType unitType {
    get{ return m_unitType; }
  }
  /***********************************************************************************/
  public uSVGLength(ushort unitType, float valueInSpecifiedUnits) {
    m_unitType = (uSVGLengthType)unitType;
    m_valueInSpecifiedUnits = valueInSpecifiedUnits;
    m_value = uSVGLengthConvertor.f_ConvertToPX(m_valueInSpecifiedUnits, m_unitType);
  }
  public uSVGLength(float valueInSpecifiedUnits) {
    m_unitType = (uSVGLengthType)0;
    m_valueInSpecifiedUnits = valueInSpecifiedUnits;
    m_value = uSVGLengthConvertor.f_ConvertToPX(m_valueInSpecifiedUnits, m_unitType);
  }
  public uSVGLength(string valueText) {
    float t_value = 0.0f;
    uSVGLengthType t_type = uSVGLengthType.SVG_LENGTHTYPE_UNKNOWN;
    uSVGLengthConvertor.f_ExtractType(valueText, ref t_value, ref t_type);
    m_unitType = t_type;      
    m_valueInSpecifiedUnits = t_value;
    m_value = uSVGLengthConvertor.f_ConvertToPX(m_valueInSpecifiedUnits, m_unitType);
  }
  /***********************************************************************************/
  public void NewValueSpecifiedUnits(ushort unitType, float valueInSpecifiedUnits) {
    m_unitType = (uSVGLengthType)unitType;
    m_valueInSpecifiedUnits = valueInSpecifiedUnits;
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
}
