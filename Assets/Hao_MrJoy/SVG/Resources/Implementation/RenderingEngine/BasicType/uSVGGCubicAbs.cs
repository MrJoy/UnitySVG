public class uSVGGCubicAbs  {
  private uSVGPoint _p1;
  private uSVGPoint _p2;
  private uSVGPoint _p;

  //================================================================================
  public uSVGPoint p1 {
    get{return this._p1;}
  }
  //-----
  public uSVGPoint p2 {
    get{return this._p2;}
  }
  //-----
  public uSVGPoint p {
    get{return this._p;}
  }
  //================================================================================
  public uSVGGCubicAbs(uSVGPoint p1, uSVGPoint p2, uSVGPoint p) {
    this._p1 = new uSVGPoint(p1.x, p1.y);
    this._p2 = new uSVGPoint(p2.x, p2.y);
    this._p = new uSVGPoint(p.x, p.y);
  }
}
