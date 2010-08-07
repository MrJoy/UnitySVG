public abstract class uSVGPathSegCurvetoCubic : uSVGPathSeg{

  /***********************************************************************************/
  public uSVGPathSegCurvetoCubic(uSVGPathSegTypes type) : base(type) {
  }
  public uSVGPathSegCurvetoCubic(ushort type) : base(type) {
  }
  /***********************************************************************************/
  public abstract uSVGPoint controlPoint1{get;}
  public abstract uSVGPoint controlPoint2{get;}
}
