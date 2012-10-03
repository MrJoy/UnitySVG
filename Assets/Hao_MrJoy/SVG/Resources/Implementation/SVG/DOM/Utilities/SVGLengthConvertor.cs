public static class SVGLengthConvertor  {
  /***********************************************************************************/
  public static bool ExtractType(string text, ref float value, ref SVGLengthType lengthType) {
    string _value = "";
    int i;
    for(i = 0; i < text.Length; i++) {
      if((('0' <= text[i]) && (text[i] <= '9')) || (text[i] == '+') || (text[i] == '-') || (text[i] == '.')) {
        _value = _value + text[i];
      } else if(text[i] == ' ') {
        // Skip.
      } else {
        break;
      }
    }
    string unit = text.Substring(i);

    if(_value == "") return false;

    value = float.Parse(_value, System.Globalization.CultureInfo.InvariantCulture);
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
      default : lengthType = SVGLengthType.Unknown; break;
    }
    return true;
  }
  /***********************************************************************************/
  public static float ConvertToPX(float value, SVGLengthType lengthType) {
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
