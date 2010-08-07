public class SVGStopElement {
  private SVGNumber _offset;
  private SVGColor _stopColor;
  /***************************************************************************/
  private AttributeList _attrList;
  /***************************************************************************/
  public SVGNumber offset {
    get{return this._offset;}
  }
  public SVGColor stopColor {
    get{return this._stopColor;}
  }
  /***************************************************************************/
  public SVGStopElement(AttributeList attrList) {
    this._attrList = attrList;
    Initialize();
  }
  /***************************************************************************/
  private void Initialize() {
    _stopColor = new SVGColor(this._attrList.GetValue("stop-color"));
    //-------
    string temp = this._attrList.GetValue("offset");
    temp = temp.Trim();
    if(temp != "") {
      if(temp.EndsWith("%")) {
        temp = temp.TrimEnd(new char[1]{'%'});
      } else {
        float _value = SVGNumber.ParseToFloat(temp)* 100;
        temp = _value.ToString();
      }
    }
    this._offset = new SVGNumber(temp);
  }
}
