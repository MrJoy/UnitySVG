public class uSVGLengthConvertor  {
  /***********************************************************************************/
  public static bool ExtractType(string text, ref float value, ref uSVGLengthType lengthType) {
    string _value = "";
    string unit = "";
    int i;
    text = text.Replace(" ", "");
    for(i = 0; i < text.Length; i++) {
      if((('0' <= text[i])&&(text[i] <= '9'))||
       (text[i] == '+')||(text[i] == '-')||(text[i] == '.')) {
        _value = _value + text[i];
      } else {
        break;
      }
    }
    unit = unit + text.Substring(i);

    if(_value == "") {
      return false;
    }

    value = float.Parse(_value);
    switch(unit.ToUpper()) {
      case "EM": lengthType = uSVGLengthType.SVG_LENGTHTYPE_EMS; break;
      case "EX": lengthType = uSVGLengthType.SVG_LENGTHTYPE_EXS; break;
      case "PX": lengthType = uSVGLengthType.SVG_LENGTHTYPE_PX; break;
      case "CM": lengthType = uSVGLengthType.SVG_LENGTHTYPE_CM; break;
      case "MM": lengthType = uSVGLengthType.SVG_LENGTHTYPE_MM; break;
      case "IN": lengthType = uSVGLengthType.SVG_LENGTHTYPE_IN; break;
      case "PT": lengthType = uSVGLengthType.SVG_LENGTHTYPE_PT; break;
      case "PC": lengthType = uSVGLengthType.SVG_LENGTHTYPE_PC; break;
      case "%": lengthType = uSVGLengthType.SVG_LENGTHTYPE_PERCENTAGE; break;
      default : lengthType = uSVGLengthType.SVG_LENGTHTYPE_UNKNOWN; break;
    }
    return true;
  }
  /***********************************************************************************/
  public static float ConvertToPX(float value, uSVGLengthType lengthType) {
    switch(lengthType) {
    case uSVGLengthType.SVG_LENGTHTYPE_IN :
      return value * 90.0f;
    case uSVGLengthType.SVG_LENGTHTYPE_CM :
      return value * 35.43307f;
    case uSVGLengthType.SVG_LENGTHTYPE_MM :
      return value * 3.543307f;
    case uSVGLengthType.SVG_LENGTHTYPE_PT :
      return value * 1.25f;
    case uSVGLengthType.SVG_LENGTHTYPE_PC :
      return value * 15.0f;
    default:
      return value;
    }
  }
}