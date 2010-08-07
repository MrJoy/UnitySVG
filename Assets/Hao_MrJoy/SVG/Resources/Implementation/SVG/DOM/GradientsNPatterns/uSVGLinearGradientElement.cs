using System.Collections.Generic;

public class uSVGLinearGradientElement : uSVGGradientElement {
  private uSVGLength _x1;
  private uSVGLength _y1;
  private uSVGLength _x2;
  private uSVGLength _y2;
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
  public uSVGLength x1 {
    get{return this._x1;}
  }

  public uSVGLength y1 {
    get{return this._y1;}
  }

  public uSVGLength x2 {
    get{return this._x2;}
  }

  public uSVGLength y2 {
    get{return this._y2;}
  }

  public int stopCount {
    get{return this._stopList.Count;}
  }

  public List<uSVGStopElement> stopList {
    get{return this._stopList;}
  }
  /***************************************************************************/
  public uSVGLinearGradientElement(uXMLImp xmlImp,
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

    temp = this._attrList.GetValue("X1");
    if(temp == "") {
      _x1 = new uSVGLength("0%");
    } else _x1 = new uSVGLength(temp);

    temp = this._attrList.GetValue("Y1");
    if(temp == "") {
      _y1 = new uSVGLength("0%");
    } else _y1 = new uSVGLength(temp);

    temp = this._attrList.GetValue("X2");
    if(temp == "") {
      _x2 = new uSVGLength("100%");
    } else _x2 = new uSVGLength(temp);

    temp = this._attrList.GetValue("Y2");
    if(temp == "") {
      _y2 = new uSVGLength("0%");
    } else _y2 = new uSVGLength(temp);

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
