using UnityEngine;

public class SVGPathSegCurvetoCubicRel : SVGPathSegCurvetoCubic, ISVGDrawableSeg {
  private float _x  = 0f;
  private float _y  = 0f;
  private float _x1  = 0f;
  private float _y1  = 0f;
  private float _x2  = 0f;
  private float _y2  = 0f;
  //================================================================================
  public float x {
    get { return this._x; }
  }
  public float y {
    get { return this._y; }
  }
  public float x1 {
    get { return this._x1; }
  }
  public float y1 {
    get { return this._y1; }
  }
  public float x2 {
    get { return this._x2; }
  }
  public float y2 {
    get { return this._y2; }
  }
  //================================================================================
  public SVGPathSegCurvetoCubicRel(float x1, float y1, float x2, float y2, float x, float y) : base() {
    this._x = x;
    this._y = y;
    this._x1 = x1;
    this._y1 = y1;
    this._x2 = x2;
    this._y2 = y2;
  }
  //================================================================================
  public override Vector2 currentPoint {
    get {
      Vector2 _return = new Vector2(0f,0f);
      SVGPathSeg _prevSeg = previousSeg;
      if(_prevSeg != null) {
        _return.x = _prevSeg.currentPoint.x + this._x;
        _return.y = _prevSeg.currentPoint.y + this._y;
      }
      return _return;
    }
  }
  //-----
  public override Vector2 controlPoint1 {
    get {
      Vector2 _return = new Vector2(0f,0f);
      SVGPathSeg _prevSeg = previousSeg;
      if(_prevSeg != null) {
        _return.x = _prevSeg.currentPoint.x + this._x1;
        _return.y = _prevSeg.currentPoint.y + this._y1;
      }
      return _return;
    }
  }
  //-----
  public override Vector2 controlPoint2 {
    get {
      Vector2 _return = new Vector2(0f,0f);
      SVGPathSeg _prevSeg = previousSeg;
      if(_prevSeg != null) {
        _return.x = _prevSeg.currentPoint.x + this._x2;
        _return.y = _prevSeg.currentPoint.y + this._y2;
      }
      return _return;
    }
  }
  //--------------------------------------------------------------------------------
  //Method: Render
  //--------------------------------------------------------------------------------
  public void Render(SVGGraphicsPath _graphicsPath) {
    Vector2 p, p1, p2;
    p1 = controlPoint1;
    p2 = controlPoint2;
    p = currentPoint;
    _graphicsPath.AddCubicCurveTo(p1, p2, p);
  }
}
