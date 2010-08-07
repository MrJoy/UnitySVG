using System.Collections.Generic;

public enum uSVGSpreadMethodTypes : ushort {
  SVG_SPREADMETHOD_UNKNOWN  = 0,
  SVG_SPREADMETHOD_PAD    = 1,
  SVG_SPREADMETHOD_REFLECT   = 2,
  SVG_SPREADMETHOD_REPEAT    = 3
}

public enum uSVGGradientUnitType : ushort {
  SVG_USER_SPACE_ON_USE    = 0,
  SVG_OBJECT_BOUNDING_BOX    = 1
}
public class uSVGGradientElement {
  private uSVGGradientUnitType _gradientUnits;
  private uSVGSpreadMethodTypes _spreadMethod;
  /***************************************************************************/
  private string _id;
  private uXMLImp _xmlImp;
  private List<uSVGStopElement> _stopList;
  protected AttributeList _attrList;
  /***************************************************************************/
  public uSVGGradientUnitType gradientUnits {
    get { return _gradientUnits; }
  }
  public uSVGSpreadMethodTypes spreadMethod {
    get { return _spreadMethod; }
  }
  public string id {
    get { return _id; }
  }
  public int stopCount {
    get { return _stopList.Count; }
  }
  public List<uSVGStopElement> stopList {
    get { return _stopList; }
  }
  /***************************************************************************/
  public uSVGGradientElement(uXMLImp xmlImp, AttributeList attrList) {
    _attrList = attrList;
    _xmlImp = xmlImp;
    _stopList = new List<uSVGStopElement>();
    _id = _attrList.GetValue("id");
    _gradientUnits = uSVGGradientUnitType.SVG_OBJECT_BOUNDING_BOX;
    if(this._attrList.GetValue("gradiantUnits") == "userSpaceOnUse") {
      _gradientUnits = uSVGGradientUnitType.SVG_USER_SPACE_ON_USE;
    }

    //------
    // TODO: It's probably a bug that the value is not innoculated for CaSe
    // VaRiAtIoN in GetValue, below:
    _spreadMethod = uSVGSpreadMethodTypes.SVG_SPREADMETHOD_PAD;
    if(this._attrList.GetValue("spreadMethod") == "reflect") {
      _spreadMethod = uSVGSpreadMethodTypes.SVG_SPREADMETHOD_REFLECT;
    } else if(this._attrList.GetValue("spreadMethod") == "repeat") {
      _spreadMethod = uSVGSpreadMethodTypes.SVG_SPREADMETHOD_REPEAT;
    }

    GetElementList();
  }

  /***************************************************************************/
  protected void GetElementList() {
    bool exitFlag = false;
    while(!exitFlag && _xmlImp.Next()) {
      if(_xmlImp.Node.Kind == NodeKind.BlockClose) {
        exitFlag = true;
        continue;
      }
      if(_xmlImp.Node.Name == "stop")
        _stopList.Add(new uSVGStopElement(_xmlImp.Node.Attributes));
    }
  }

  public uSVGStopElement GetStopElement(int i) {
    if((i >= 0) && (i < stopCount)) return this._stopList[i];
    return null;
  }
}
