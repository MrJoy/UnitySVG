public enum SVGLengthType : ushort {
  Unknown = 0,
  Number = 1,
  Percentage = 2,
  EMs = 3,
  EXs = 4,
  PX = 5,
  CM = 6,
  MM = 7,
  IN = 8,
  PT = 9,
  PC = 10,
}
/**************************************************************************************************/
public struct SVGLength  {
  private SVGLengthType _unitType;
  private float _valueInSpecifiedUnits, _value;

  /***********************************************************************************/
  public float value {
    get { return _value; }
  }
  public SVGLengthType unitType {
    get{ return _unitType; }
  }
  /***********************************************************************************/
  public SVGLength(ushort unitType, float valueInSpecifiedUnits) {
    _unitType = (SVGLengthType)unitType;
    _valueInSpecifiedUnits = valueInSpecifiedUnits;
    _value = uSVGLengthConvertor.ConvertToPX(_valueInSpecifiedUnits, _unitType);
  }
  public SVGLength(float valueInSpecifiedUnits) {
    _unitType = (SVGLengthType)0;
    _valueInSpecifiedUnits = valueInSpecifiedUnits;
    _value = uSVGLengthConvertor.ConvertToPX(_valueInSpecifiedUnits, _unitType);
  }
  public SVGLength(string valueText) {
    float t_value = 0.0f;
    SVGLengthType t_type = SVGLengthType.Unknown;
    uSVGLengthConvertor.ExtractType(valueText, ref t_value, ref t_type);
    _unitType = t_type;
    _valueInSpecifiedUnits = t_value;
    _value = uSVGLengthConvertor.ConvertToPX(_valueInSpecifiedUnits, _unitType);
  }
  /***********************************************************************************/
  public void NewValueSpecifiedUnits(float valueInSpecifiedUnits) {
    _unitType = (SVGLengthType)0;
    _valueInSpecifiedUnits = valueInSpecifiedUnits;
    _value = uSVGLengthConvertor.ConvertToPX(_valueInSpecifiedUnits, _unitType);
  }
  public static float GetPXLength(string valueText) {
    float t_value = 0.0f;
    SVGLengthType t_type = SVGLengthType.Unknown;
    uSVGLengthConvertor.ExtractType(valueText, ref t_value, ref t_type);
    return uSVGLengthConvertor.ConvertToPX(t_value, t_type);
  }
}
