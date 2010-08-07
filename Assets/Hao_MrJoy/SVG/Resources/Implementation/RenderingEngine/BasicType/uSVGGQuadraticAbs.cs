public class uSVGGQuadraticAbs  {
  private uSVGPoint _p1;
  private uSVGPoint _p;

  //================================================================================
  public uSVGPoint p1 {
    get{return this._p1;}
  }
  //-----
  public uSVGPoint p {
    get{return this._p;}
  }
  //================================================================================
  public uSVGGQuadraticAbs(uSVGPoint p1, uSVGPoint p) {
    this._p1 = new uSVGPoint(p1.x, p1.y);
    this._p = new uSVGPoint(p.x, p.y);
  }
}
