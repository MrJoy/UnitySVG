public class SVGStopElement {
  private float _offset;
  private SVGColor _stopColor;
  /***************************************************************************/
  public float offset {
    get { return _offset; }
  }
  public SVGColor stopColor {
    get { return _stopColor; }
  }
  /***************************************************************************/
  public SVGStopElement(AttributeList attrList) {
    _stopColor = new SVGColor(attrList.GetValue("stop-color"));
    string temp = attrList.GetValue("offset").Trim();
    if(temp != "") {
      if(temp.EndsWith("%")) {
        _offset = SVGNumber.ParseToFloat(temp.TrimEnd(new char[1]{'%'}));
      } else {
        _offset = SVGNumber.ParseToFloat(temp)* 100;
      }
    }
  }
}
