public enum SVGPathSegTypes : ushort {
  Unknown       = 0,
  Close       = 1,
  MoveTo_Abs       = 2,
  MoveTo_Rel       = 3,
  LineTo_Abs       = 4,
  LineTo_Rel       = 5,
  CurveTo_Cubic_Abs     = 6,
  CurveTo_Cubic_Rel     = 7,
  CurveTo_Quadratic_Abs     = 8,
  CurveTo_Quadratic_Rel     = 9,
  Arc_Abs       = 10,
  Arc_Rel       = 11,
  LineTo_Horizontal_Abs     = 12,
  LineTo_Horizontal_Rel     = 13,
  LineTo_Vertical_Abs     = 14,
  LineTo_Vertical_Rel     = 15,
  CurveTo_Cubic_Smooth_Abs   = 16,
  CurveTo_Cubic_Smooth_Rel   = 17,
  CurveTo_Quadratic_Smooth_Abs   = 18,
  CurveTo_Quadratic_Smooth_Rel   = 19
}
/*************************************************************************************************/
public abstract class SVGPathSeg {
  private SVGPathSegTypes _pathSegType;
  protected SVGPathSegList _segList;
  protected int _segIndex;
  /***********************************************************************************/
  public SVGPathSegTypes pathSegType {
    get { return this._pathSegType; }
  }
  public string pathSegTypeAsLetter {
    get { return TypeToLetter(); }
  }
  /***********************************************************************************/
  public SVGPathSeg(SVGPathSegTypes type) {
    this._pathSegType = type;
  }
  public SVGPathSeg(ushort type) {
    this._pathSegType = (SVGPathSegTypes)type;
  }
  /***********************************************************************************/
  internal void SetList(SVGPathSegList segList) {
    this._segList = segList;
  }
  internal void SetIndex(int segIndex) {
    this._segIndex = segIndex;
  }
  public SVGPathSeg previousSeg {
    get {
      return _segList.GetPreviousSegment(this);
    }
  }
  /***********************************************************************************/
  public abstract SVGPoint currentPoint{get;}
  public SVGPoint previousPoint {
    get {
      SVGPoint _return = new SVGPoint(0f,0f);
      SVGPathSeg _prevSeg = previousSeg;
      if(_prevSeg != null) {
        _return.x = _prevSeg.currentPoint.x;
        _return.y = _prevSeg.currentPoint.y;
      }
      return _return;
    }
  }
  /***********************************************************************************/
  private string TypeToLetter() {
    switch(this._pathSegType)
    {
    case SVGPathSegTypes.Unknown:
      return "";
    case SVGPathSegTypes.Arc_Abs:
      return "A";
    case SVGPathSegTypes.Arc_Rel:
      return "a";
    case SVGPathSegTypes.Close:
      return "z";
    case SVGPathSegTypes.CurveTo_Cubic_Abs:
      return "C";
    case SVGPathSegTypes.CurveTo_Cubic_Rel:
      return "c";
    case SVGPathSegTypes.CurveTo_Cubic_Smooth_Abs:
      return "S";
    case SVGPathSegTypes.CurveTo_Cubic_Smooth_Rel:
      return "s";
    case SVGPathSegTypes.CurveTo_Quadratic_Abs:
      return "Q";
    case SVGPathSegTypes.CurveTo_Quadratic_Rel:
      return "q";
    case SVGPathSegTypes.CurveTo_Quadratic_Smooth_Abs:
      return "T";
    case SVGPathSegTypes.CurveTo_Quadratic_Smooth_Rel:
      return "t";
    case SVGPathSegTypes.LineTo_Abs:
      return "L";
    case SVGPathSegTypes.LineTo_Horizontal_Abs:
      return "H";
    case SVGPathSegTypes.LineTo_Horizontal_Rel:
      return "h";
    case SVGPathSegTypes.LineTo_Rel:
      return "l";
    case SVGPathSegTypes.LineTo_Vertical_Abs:
      return "V";
    case SVGPathSegTypes.LineTo_Vertical_Rel:
      return "v";
    case SVGPathSegTypes.MoveTo_Abs:
      return "M";
    case SVGPathSegTypes.MoveTo_Rel:
      return "m";
    default:
      return "";
    }
  }
}