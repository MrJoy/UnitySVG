public enum uSVGPathSegTypes : ushort {
  PATHSEG_UNKNOWN       = 0,
  PATHSEG_CLOSEPATH       = 1,
  PATHSEG_MOVETO_ABS       = 2,
  PATHSEG_MOVETO_REL       = 3,
  PATHSEG_LINETO_ABS       = 4,
  PATHSEG_LINETO_REL       = 5,
  PATHSEG_CURVETO_CUBIC_ABS     = 6,
  PATHSEG_CURVETO_CUBIC_REL     = 7,
  PATHSEG_CURVETO_QUADRATIC_ABS     = 8,
  PATHSEG_CURVETO_QUADRATIC_REL     = 9,
  PATHSEG_ARC_ABS       = 10,
  PATHSEG_ARC_REL       = 11,
  PATHSEG_LINETO_HORIZONTAL_ABS     = 12,
  PATHSEG_LINETO_HORIZONTAL_REL     = 13,
  PATHSEG_LINETO_VERTICAL_ABS     = 14,
  PATHSEG_LINETO_VERTICAL_REL     = 15,
  PATHSEG_CURVETO_CUBIC_SMOOTH_ABS   = 16,
  PATHSEG_CURVETO_CUBIC_SMOOTH_REL   = 17,
  PATHSEG_CURVETO_QUADRATIC_SMOOTH_ABS   = 18,
  PATHSEG_CURVETO_QUADRATIC_SMOOTH_REL   = 19
}
/*************************************************************************************************/
public abstract class uSVGPathSeg {
  private uSVGPathSegTypes _pathSegType;
  protected uSVGPathSegList _segList;
  protected int _segIndex;
  /***********************************************************************************/
  public uSVGPathSegTypes pathSegType {
    get { return this._pathSegType; }
  }
  public string pathSegTypeAsLetter {
    get { return TypeToLetter(); }
  }
  /***********************************************************************************/
  public uSVGPathSeg(uSVGPathSegTypes type) {
    this._pathSegType = type;
  }
  public uSVGPathSeg(ushort type) {
    this._pathSegType = (uSVGPathSegTypes)type;
  }
  /***********************************************************************************/
  internal void SetList(uSVGPathSegList segList) {
    this._segList = segList;
  }
  internal void SetIndex(int segIndex) {
    this._segIndex = segIndex;
  }
  public uSVGPathSeg previousSeg {
    get {
      return _segList.GetPreviousSegment(this);
    }
  }
  /***********************************************************************************/
  public abstract uSVGPoint currentPoint{get;}
  public uSVGPoint previousPoint {
    get {
      uSVGPoint _return = new uSVGPoint(0f,0f);
      uSVGPathSeg _prevSeg = previousSeg;
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
    case uSVGPathSegTypes.PATHSEG_UNKNOWN:
      return "";
    case uSVGPathSegTypes.PATHSEG_ARC_ABS:
      return "A";
    case uSVGPathSegTypes.PATHSEG_ARC_REL:
      return "a";
    case uSVGPathSegTypes.PATHSEG_CLOSEPATH:
      return "z";
    case uSVGPathSegTypes.PATHSEG_CURVETO_CUBIC_ABS:
      return "C";
    case uSVGPathSegTypes.PATHSEG_CURVETO_CUBIC_REL:
      return "c";
    case uSVGPathSegTypes.PATHSEG_CURVETO_CUBIC_SMOOTH_ABS:
      return "S";
    case uSVGPathSegTypes.PATHSEG_CURVETO_CUBIC_SMOOTH_REL:
      return "s";
    case uSVGPathSegTypes.PATHSEG_CURVETO_QUADRATIC_ABS:
      return "Q";
    case uSVGPathSegTypes.PATHSEG_CURVETO_QUADRATIC_REL:
      return "q";
    case uSVGPathSegTypes.PATHSEG_CURVETO_QUADRATIC_SMOOTH_ABS:
      return "T";
    case uSVGPathSegTypes.PATHSEG_CURVETO_QUADRATIC_SMOOTH_REL:
      return "t";
    case uSVGPathSegTypes.PATHSEG_LINETO_ABS:
      return "L";
    case uSVGPathSegTypes.PATHSEG_LINETO_HORIZONTAL_ABS:
      return "H";
    case uSVGPathSegTypes.PATHSEG_LINETO_HORIZONTAL_REL:
      return "h";
    case uSVGPathSegTypes.PATHSEG_LINETO_REL:
      return "l";
    case uSVGPathSegTypes.PATHSEG_LINETO_VERTICAL_ABS:
      return "V";
    case uSVGPathSegTypes.PATHSEG_LINETO_VERTICAL_REL:
      return "v";
    case uSVGPathSegTypes.PATHSEG_MOVETO_ABS:
      return "M";
    case uSVGPathSegTypes.PATHSEG_MOVETO_REL:
      return "m";
    default:
      return "";
    }
  }
}