using System.Collections.Generic;

public class SVGLinearGradientElement : SVGGradientElement {
  private SVGLength _x1;
  private SVGLength _y1;
  private SVGLength _x2;
  private SVGLength _y2;
  /***************************************************************************/
  public SVGLength x1 {
    get { return _x1; }
  }

  public SVGLength y1 {
    get { return _y1; }
  }

  public SVGLength x2 {
    get { return _x2; }
  }

  public SVGLength y2 {
    get { return _y2; }
  }
  /***************************************************************************/
  public SVGLinearGradientElement(uXMLImp xmlImp, AttributeList attrList) : base(xmlImp, attrList) {
    string temp;
    temp = _attrList.GetValue("x1");
    _x1 = new SVGLength((temp == "") ? "0%" : temp);

    temp = this._attrList.GetValue("y1");
    _y1 = new SVGLength((temp == "") ? "0%" : temp);

    temp = this._attrList.GetValue("x2");
    _x2 = new SVGLength((temp == "") ? "100%" : temp);

    temp = this._attrList.GetValue("y2");
    _y2 = new SVGLength((temp == "") ? "0%" : temp);
  }
}
