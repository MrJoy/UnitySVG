public abstract class SVGPathSegCurvetoQuadratic : SVGPathSeg{

  /***********************************************************************************/
  public SVGPathSegCurvetoQuadratic(SVGPathSegTypes type) : base(type) {
  }
  public SVGPathSegCurvetoQuadratic(ushort type) : base(type) {
  }
  /***********************************************************************************/
  public abstract SVGPoint controlPoint1{get;}
}
