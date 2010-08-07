using System.Collections.Generic;

public class SVGRadialGradientElement : SVGGradientElement {
  private SVGLength _cx;
  private SVGLength _cy;
  private SVGLength _r;
  private SVGLength _fx;
  private SVGLength _fy;
  /***************************************************************************/
  public SVGLength cx {
    get { return _cx; }
  }
  public SVGLength cy {
    get { return _cy; }
  }
  public SVGLength r {
    get { return _r; }
  }
  public SVGLength fx {
    get { return _fx; }
  }
  public SVGLength fy {
    get { return _fy; }
  }
  /***************************************************************************/
  public SVGRadialGradientElement(uXMLImp xmlImp, AttributeList attrList) : base(xmlImp, attrList) {
    string temp;
    temp = attrList.GetValue("cx");
    _cx = new SVGLength((temp == "") ? "50%" : temp);

    temp = attrList.GetValue("cy");
    _cy = new SVGLength((temp == "") ? "50%" : temp);

    temp = attrList.GetValue("r");
    _r = new SVGLength((temp == "") ? "50%" : temp);

    temp = attrList.GetValue("fx");
    _fx = new SVGLength((temp == "") ? "50%" : temp);

    temp = attrList.GetValue("fy");
    _fy = new SVGLength((temp == "") ? "50%" : temp);
  }
}
