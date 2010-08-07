using System.Collections.Generic;

public class uSVGLinearGradientElement : uSVGGradientElement {
  private uSVGLength _x1;
  private uSVGLength _y1;
  private uSVGLength _x2;
  private uSVGLength _y2;
  /***************************************************************************/
  public uSVGLength x1 {
    get { return _x1; }
  }

  public uSVGLength y1 {
    get { return _y1; }
  }

  public uSVGLength x2 {
    get { return _x2; }
  }

  public uSVGLength y2 {
    get { return _y2; }
  }
  /***************************************************************************/
  public uSVGLinearGradientElement(uXMLImp xmlImp, AttributeList attrList) : base(xmlImp, attrList) {
    string temp;
    temp = _attrList.GetValue("x1");
    _x1 = new uSVGLength((temp == "") ? "0%" : temp);

    temp = this._attrList.GetValue("y1");
    _y1 = new uSVGLength((temp == "") ? "0%" : temp);

    temp = this._attrList.GetValue("x2");
    _x2 = new uSVGLength((temp == "") ? "100%" : temp);

    temp = this._attrList.GetValue("y2");
    _y2 = new uSVGLength((temp == "") ? "0%" : temp);
  }
}
