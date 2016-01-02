using UnityEngine;

public class SVGPathSegCurvetoCubicSmoothRel : SVGPathSegCurvetoCubic, ISVGDrawableSeg {
  private float _x = 0f, _y = 0f, _x2 = 0f, _y2 = 0f;

  public float x { get { return this._x; } }

  public float y { get { return this._y; } }

  public float x2 { get { return this._x2; } }

  public float y2 { get { return this._y2; } }

  public SVGPathSegCurvetoCubicSmoothRel(float x2, float y2, float x, float y) : base() {
    this._x = x;
    this._y = y;
    this._x2 = x2;
    this._y2 = y2;
  }

  public override Vector2 currentPoint {
    get {
      Vector2 _return = new Vector2(0f, 0f);
      SVGPathSeg _prevSeg = previousSeg;
      if(_prevSeg != null) {
        _return.x = _prevSeg.currentPoint.x + this._x;
        _return.y = _prevSeg.currentPoint.y + this._y;
      }
      return _return;
    }
  }

  public override Vector2 controlPoint1 {
    get {
      Vector2 _return = new Vector2(0f, 0f);
      SVGPathSeg _prevSeg = previousSeg;
      if(_prevSeg != null) {
        Vector2 t_currP = previousPoint;
        Vector2 t_prevCP2 = ((SVGPathSegCurvetoCubic)_prevSeg).controlPoint2;
        Vector2 t_P = t_currP - t_prevCP2;
        _return = t_currP + t_P;
      }
      return _return;
    }
  }

  public override Vector2 controlPoint2 {
    get {
      Vector2 _return = new Vector2(0f, 0f);
      SVGPathSeg _prevSeg = previousSeg;
      if(_prevSeg != null) {
        _return.x = _prevSeg.currentPoint.x + this._x2;
        _return.y = _prevSeg.currentPoint.y + this._y2;
      }
      return _return;
    }
  }

  public void Render(SVGGraphicsPath _graphicsPath) {
    _graphicsPath.AddCubicCurveTo(controlPoint1, controlPoint2, currentPoint);
  }
}
