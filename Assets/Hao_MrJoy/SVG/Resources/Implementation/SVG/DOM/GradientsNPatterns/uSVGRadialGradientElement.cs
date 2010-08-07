using System.Collections.Generic;

public class uSVGRadialGradientElement : uSVGGradientElement {
  private uSVGLength _cx;
  private uSVGLength _cy;
  private uSVGLength _r;
  private uSVGLength _fx;
  private uSVGLength _fy;
  /***************************************************************************/
  public uSVGLength cx {
    get { return _cx; }
  }
  public uSVGLength cy {
    get { return _cy; }
  }
  public uSVGLength r {
    get { return _r; }
  }
  public uSVGLength fx {
    get { return _fx; }
  }
  public uSVGLength fy {
    get { return _fy; }
  }
  /***************************************************************************/
  public uSVGRadialGradientElement(uXMLImp xmlImp, AttributeList attrList) : base(xmlImp, attrList) {
    string temp;
    temp = attrList.GetValue("cx");
    _cx = new uSVGLength((temp == "") ? "50%" : temp);

    temp = attrList.GetValue("cy");
    _cy = new uSVGLength((temp == "") ? "50%" : temp);

    temp = attrList.GetValue("r");
    _r = new uSVGLength((temp == "") ? "50%" : temp);

    temp = attrList.GetValue("fx");
    _fx = new uSVGLength((temp == "") ? "50%" : temp);

    temp = attrList.GetValue("fy");
    _fy = new uSVGLength((temp == "") ? "50%" : temp);
  }
}
