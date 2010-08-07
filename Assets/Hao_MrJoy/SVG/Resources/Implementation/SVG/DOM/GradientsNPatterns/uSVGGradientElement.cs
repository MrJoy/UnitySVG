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
  private uSVGGradientUnitType     _gradientUnits;
  private uSVGTransformList   _gradientTransfrom;
  private uSVGSpreadMethodTypes    _spreadMethod;
  /***************************************************************************/
  private AttributeList _attrList;
  /***************************************************************************/
  public uSVGGradientUnitType gradientUnits {
    get{ return this._gradientUnits;}
  }

  public uSVGSpreadMethodTypes spreadMethod {
    get{ return this._spreadMethod;}
  }
  /***************************************************************************/
  public uSVGGradientElement(AttributeList attrList) {
    this._attrList = attrList;
    Initialize();
  }
  /***************************************************************************/
  private void Initialize() {
    _gradientUnits = uSVGGradientUnitType.SVG_OBJECT_BOUNDING_BOX;
    if(this._attrList.GetValue("GRADIENTUNITS").ToLower() == "userspaceonuse") {
      _gradientUnits = uSVGGradientUnitType.SVG_USER_SPACE_ON_USE;
    }

    //------
    // TODO: It's probably a bug that the value is not innoculated for CaSe
    // VaRiAtIoN in GetValue, below:
    _spreadMethod = uSVGSpreadMethodTypes.SVG_SPREADMETHOD_PAD;
    if(this._attrList.GetValue("SPREADMETHOD") == "reflect") {
      _spreadMethod = uSVGSpreadMethodTypes.SVG_SPREADMETHOD_REFLECT;
    } else if(this._attrList.GetValue("SPREADMETHOD") == "repeat") {
      _spreadMethod = uSVGSpreadMethodTypes.SVG_SPREADMETHOD_REPEAT;
    }
  }
}
