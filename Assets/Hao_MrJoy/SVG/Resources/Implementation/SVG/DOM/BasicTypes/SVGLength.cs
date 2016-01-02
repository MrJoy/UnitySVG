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

public struct SVGLength {
  private SVGLengthType _unitType;
  private float _valueInSpecifiedUnits, _value;

  public float value { get { return _value; } }

  public SVGLengthType unitType { get { return _unitType; } }

  public SVGLength(SVGLengthType unitType, float valueInSpecifiedUnits) {
    _unitType = unitType;
    _valueInSpecifiedUnits = valueInSpecifiedUnits;
    _value = ConvertToPX(_valueInSpecifiedUnits, _unitType);
  }

  public SVGLength(float valueInSpecifiedUnits) {
    _unitType = SVGLengthType.Number;
    _valueInSpecifiedUnits = valueInSpecifiedUnits;
    _value = ConvertToPX(_valueInSpecifiedUnits, _unitType);
  }

  public SVGLength(string valueText) {
    float t_value = 0.0f;
    SVGLengthType t_type = SVGLengthType.Unknown;
    ExtractType(valueText, ref t_value, ref t_type);
    _unitType = t_type;
    _valueInSpecifiedUnits = t_value;
    _value = ConvertToPX(_valueInSpecifiedUnits, _unitType);
  }

  public void NewValueSpecifiedUnits(float valueInSpecifiedUnits) {
    _unitType = SVGLengthType.Unknown;
    _valueInSpecifiedUnits = valueInSpecifiedUnits;
    _value = ConvertToPX(_valueInSpecifiedUnits, _unitType);
  }

  public static float GetPXLength(string valueText) {
    float t_value = 0.0f;
    SVGLengthType t_type = SVGLengthType.Unknown;
    ExtractType(valueText, ref t_value, ref t_type);
    return ConvertToPX(t_value, t_type);
  }

  private static void ExtractType(string text, ref float value, ref SVGLengthType lengthType) {
    if(string.IsNullOrEmpty(text))
      return;
    text = text.Trim();
    int i;
    for(i = 0; i < text.Length; ++i) {
      char c = text[i];
      if((('0' <= c) && (c <= '9')) || (c == '+') || (c == '-') || (c == '.') || (c == ' '))
        continue;
      break;
    }

    var strValue = text.Substring(0, i);
    if(!string.IsNullOrEmpty(strValue)) {
      string unit = text.Substring(i);
      value = float.Parse(strValue, System.Globalization.CultureInfo.InvariantCulture);
      switch(unit.ToUpper()) {
      case "EM": lengthType = SVGLengthType.EMs; break;
      case "EX": lengthType = SVGLengthType.EXs; break;
      case "PX": lengthType = SVGLengthType.PX; break;
      case "CM": lengthType = SVGLengthType.CM; break;
      case "MM": lengthType = SVGLengthType.MM; break;
      case "IN": lengthType = SVGLengthType.IN; break;
      case "PT": lengthType = SVGLengthType.PT; break;
      case "PC": lengthType = SVGLengthType.PC; break;
      case "%": lengthType = SVGLengthType.Percentage; break;
      default: lengthType = SVGLengthType.Unknown; break;
      }
    }
  }

  private static float ConvertToPX(float value, SVGLengthType lengthType) {
    // TODO: Shouldn't PX-per-<real-world-unit> be a function of the render target size?
    switch(lengthType) {
    case SVGLengthType.IN: return value * 90.0f;
    case SVGLengthType.CM: return value * 35.43307f;
    case SVGLengthType.MM: return value * 3.543307f;
    case SVGLengthType.PT: return value * 1.25f;
    case SVGLengthType.PC: return value * 15.0f;
    default: return value;
    }
  }
}
