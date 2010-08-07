public class uSVGGArcAbs  {
  private uSVGPoint _p;
  private float _r1;
  private float _r2;
  private float _angle;
  private bool _largeArcFlag;
  private bool _sweepFlag;
  //================================================================================
  public float r1 {
    get{return this._r1;}
  }
  //-----
  public float r2 {
    get{return this._r2;}
  }
  //-----
  public float angle {
    get{return this._angle;}
  }
  //-----
  public bool largeArcFlag {
    get{return this._largeArcFlag;}
  }
    //-----
  public bool sweepFlag {
    get{return this._sweepFlag;}
  }
  //-----
  public uSVGPoint point {
    get{return this._p;}
  }
  //================================================================================
  public uSVGGArcAbs(float r1, float r2, float angle,
              bool largeArcFlag, bool sweepFlag, uSVGPoint p) {
    this._r1 = r1;
    this._r2 = r2;
    this._angle = angle;
    this._largeArcFlag = largeArcFlag;
    this._sweepFlag = sweepFlag;
    this._p = p;
  }
}
