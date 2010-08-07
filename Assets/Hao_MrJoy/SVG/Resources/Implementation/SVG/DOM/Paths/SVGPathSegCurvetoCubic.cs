public abstract class SVGPathSegCurvetoCubic : SVGPathSeg{

  /***********************************************************************************/
  public SVGPathSegCurvetoCubic(SVGPathSegTypes type) : base(type) {
  }
  public SVGPathSegCurvetoCubic(ushort type) : base(type) {
  }
  /***********************************************************************************/
  public abstract SVGPoint controlPoint1{get;}
  public abstract SVGPoint controlPoint2{get;}
}
