public class SVGStopElement {
  private uSVGNumber _offset;
  private uSVGColor _stopColor;
  /***************************************************************************/
  private AttributeList _attrList;
  /***************************************************************************/
  public uSVGNumber offset {
    get{return this._offset;}
  }
  public uSVGColor stopColor {
    get{return this._stopColor;}
  }
  /***************************************************************************/
  public SVGStopElement(AttributeList attrList) {
    this._attrList = attrList;
    Initialize();
  }
  /***************************************************************************/
  private void Initialize() {
    _stopColor = new uSVGColor(this._attrList.GetValue("stop-color"));
    //-------
    string temp = this._attrList.GetValue("offset");
    temp = temp.Trim();
    if(temp != "") {
      if(temp.EndsWith("%")) {
        temp = temp.TrimEnd(new char[1]{'%'});
      } else {
        float _value = uSVGNumber.ParseToFloat(temp)* 100;
        temp = _value.ToString();
      }
    }
    this._offset = new uSVGNumber(temp);
  }
}
