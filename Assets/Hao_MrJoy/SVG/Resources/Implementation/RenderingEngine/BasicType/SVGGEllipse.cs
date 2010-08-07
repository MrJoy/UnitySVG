public class SVGGEllipse {
  private SVGPoint _p;
  private float _r1;
  private float _r2;
  private float _angle;
  //================================================================================
  public SVGPoint point {
    get{ return this._p;}
  }
  //------
  public float r1 {
    get{return this._r1;}
  }
  //------
  public float r2 {
    get{return this._r2;}
  }
    //------
  public float angle {
    get{return this._angle;}
  }
  //================================================================================
  public SVGGEllipse(SVGPoint p, float r1, float r2, float angle) {
    this._p = new SVGPoint(p.x, p.y);
    this._r1 = r1;
    this._r2 = r2;
    this._angle = angle;
  }
}
