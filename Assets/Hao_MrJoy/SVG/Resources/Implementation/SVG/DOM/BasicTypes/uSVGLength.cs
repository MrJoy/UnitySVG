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
  private uSVGLengthType _unitType;
  private float _valueInSpecifiedUnits, _value;

  /***********************************************************************************/
  public float value {
    get { return _value; }
  }
  public uSVGLengthType unitType {
    get{ return _unitType; }
  }
  /***********************************************************************************/
  public uSVGLength(ushort unitType, float valueInSpecifiedUnits) {
    _unitType = (uSVGLengthType)unitType;
    _valueInSpecifiedUnits = valueInSpecifiedUnits;
    _value = uSVGLengthConvertor.ConvertToPX(_valueInSpecifiedUnits, _unitType);
  }
  public uSVGLength(float valueInSpecifiedUnits) {
    _unitType = (uSVGLengthType)0;
    _valueInSpecifiedUnits = valueInSpecifiedUnits;
    _value = uSVGLengthConvertor.ConvertToPX(_valueInSpecifiedUnits, _unitType);
  }
  public uSVGLength(string valueText) {
    float t_value = 0.0f;
    uSVGLengthType t_type = uSVGLengthType.SVG_LENGTHTYPE_UNKNOWN;
    uSVGLengthConvertor.ExtractType(valueText, ref t_value, ref t_type);
    _unitType = t_type;
    _valueInSpecifiedUnits = t_value;
    _value = uSVGLengthConvertor.ConvertToPX(_valueInSpecifiedUnits, _unitType);
  }
  /***********************************************************************************/
  public void NewValueSpecifiedUnits(float valueInSpecifiedUnits) {
    _unitType = (uSVGLengthType)0;
    _valueInSpecifiedUnits = valueInSpecifiedUnits;
    _value = uSVGLengthConvertor.ConvertToPX(_valueInSpecifiedUnits, _unitType);
  }
  public static float GetPXLength(string valueText) {
    float t_value = 0.0f;
    uSVGLengthType t_type = uSVGLengthType.SVG_LENGTHTYPE_UNKNOWN;
    uSVGLengthConvertor.ExtractType(valueText, ref t_value, ref t_type);
    return uSVGLengthConvertor.ConvertToPX(t_value, t_type);
  }
}
