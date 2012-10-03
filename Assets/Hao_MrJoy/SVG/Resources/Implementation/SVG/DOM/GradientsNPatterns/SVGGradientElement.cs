using System.Collections.Generic;

public enum SVGSpreadMethod : ushort {
  Unknown = 0,
  Pad = 1,
  Reflect = 2,
  Repeat = 3
}

public enum SVGGradientUnit : ushort {
  UserSpaceOnUse = 0,
  ObjectBoundingBox = 1
}

public class SVGGradientElement {
  private SVGGradientUnit _gradientUnits;
  private SVGSpreadMethod _spreadMethod;
  /***************************************************************************/
  private string _id;
  private SVGParser _xmlImp;
  private List<SVGStopElement> _stopList;
  protected AttributeList _attrList;
  /***************************************************************************/
  public SVGGradientUnit gradientUnits {
    get { return _gradientUnits; }
  }
  public SVGSpreadMethod spreadMethod {
    get { return _spreadMethod; }
  }
  public string id {
    get { return _id; }
  }
  public List<SVGStopElement> stopList {
    get { return _stopList; }
  }
  /***************************************************************************/
  public SVGGradientElement(SVGParser xmlImp, AttributeList attrList) {
    _attrList = attrList;
    _xmlImp = xmlImp;
    _stopList = new List<SVGStopElement>();
    _id = _attrList.GetValue("id");
    _gradientUnits = SVGGradientUnit.ObjectBoundingBox;
    if(_attrList.GetValue("gradiantUnits") == "userSpaceOnUse") {
      _gradientUnits = SVGGradientUnit.UserSpaceOnUse;
    }

    //------
    // TODO: It's probably a bug that the value is not innoculated for CaSe
    // VaRiAtIoN in GetValue, below:
    _spreadMethod = SVGSpreadMethod.Pad;
    if(_attrList.GetValue("spreadMethod") == "reflect") {
      _spreadMethod = SVGSpreadMethod.Reflect;
    } else if(_attrList.GetValue("spreadMethod") == "repeat") {
      _spreadMethod = SVGSpreadMethod.Repeat;
    }

    GetElementList();
  }

  /***************************************************************************/
  protected void GetElementList() {
    bool exitFlag = false;
    while(!exitFlag && _xmlImp.Next()) {
      if(_xmlImp.Node is BlockCloseNode) {
        exitFlag = true;
        continue;
      }
      if(_xmlImp.Node.Name == "stop")
        _stopList.Add(new SVGStopElement(_xmlImp.Node.Attributes));
    }
  }

  public SVGStopElement GetStopElement(int i) {
    if((i >= 0) && (i < _stopList.Count)) return _stopList[i];
    return null;
  }
}
