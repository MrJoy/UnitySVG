using System.Collections.Generic;

public class uSVGRadialGradientElement : uSVGGradientElement {

  private uSVGLength _cx;
  private uSVGLength _cy;
  private uSVGLength _r;
  private uSVGLength _fx;
  private uSVGLength _fy;
  //---------
  private string _id;
  private uXMLImp _xmlImp;
  private List<uSVGStopElement> _stopList;
  //---------
  private AttributeList _attrList;
  /***************************************************************************/
  public string id {
    get{ return this._id;}
  }
  public uSVGLength cx {
    get{return this._cx;}
  }
  public uSVGLength cy {
    get{return this._cy;}
  }
    public uSVGLength r {
    get{return this._r;}
  }
    public uSVGLength fx {
    get{return this._fx;}
  }
    public uSVGLength fy {
    get{return this._fy;}
  }

  public int stopCount {
    get{return this._stopList.Count;}
  }

  public List<uSVGStopElement> stopList {
    get{return this._stopList;}
  }
  /***************************************************************************/
  public uSVGRadialGradientElement(uXMLImp xmlImp,
                    AttributeList attrList) : base(attrList) {
    this._attrList = attrList;
    this._xmlImp = xmlImp;
    Initialize();
  }
  /***************************************************************************/
  private void Initialize() {
    this._stopList = new List<uSVGStopElement>();
    string temp = this._attrList.GetValue("ID");
    this._id = temp;

    temp = this._attrList.GetValue("CX");
    if(temp == "") {
      _cx = new uSVGLength("50%");
    } else _cx = new uSVGLength(temp);

    temp = this._attrList.GetValue("CY");
    if(temp == "") {
      _cy = new uSVGLength("50%");
    } else _cy = new uSVGLength(temp);

    temp = this._attrList.GetValue("R");
    if(temp == "") {
      _r = new uSVGLength("50%");
    } else _r = new uSVGLength(temp);

    temp = this._attrList.GetValue("FX");
    if(temp == "") {
      _fx = new uSVGLength("50%");
    } else _fx = new uSVGLength(temp);

    temp = this._attrList.GetValue("FY");
    if(temp == "") {
      _fy = new uSVGLength("50%");
    } else _fy = new uSVGLength(temp);

    GetElementList();
  }
  //---------------
  private void GetElementList() {
    bool exitFlag = false;
    while(!exitFlag && this._xmlImp.ReadNextTag()) {
      if(this._xmlImp.GetCurrentTagState() == uXMLImp.XMLTagState.CLOSE) {
        exitFlag = true;
        continue;
      }
      string t_name = this._xmlImp.GetCurrentTagName();
      AttributeList t_attrList;
        if(t_name == "stop") {
          t_attrList = this._xmlImp.GetCurrentAttributesList();
          uSVGStopElement temp = new uSVGStopElement(  t_attrList);
          _stopList.Add(temp);
        }
    }
  }
  /***************************************************************************/
  public uSVGStopElement GetStopElement(int i) {
    if((i >=0 )&&(i < stopCount)) {
      return this._stopList[i];
    }
    return null;
  }
}
