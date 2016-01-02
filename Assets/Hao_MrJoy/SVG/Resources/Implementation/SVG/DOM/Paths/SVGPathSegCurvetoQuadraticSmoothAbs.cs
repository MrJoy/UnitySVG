using UnityEngine;

public class SVGPathSegCurvetoQuadraticSmoothAbs : SVGPathSegCurvetoQuadratic, ISVGDrawableSeg {
  private float _x = 0f, _y = 0f;

  public float x { get { return this._x; } }

  public float y { get { return this._y; } }

  public SVGPathSegCurvetoQuadraticSmoothAbs(float x, float y) : base() {
    this._x = x;
    this._y = y;
  }

  public override Vector2 currentPoint { get { return new Vector2(this._x, this._y); } }

  public override Vector2 controlPoint1 {
    get {
      Vector2 _return = new Vector2(0f, 0f);
      SVGPathSeg _prevSeg = previousSeg;
      if(_prevSeg != null) {
        Vector2 t_currP = previousPoint;
        Vector2 t_prevCP2 = ((SVGPathSegCurvetoQuadratic)_prevSeg).controlPoint1;
        Vector2 t_P = t_currP - t_prevCP2;
        _return = t_currP + t_P;
      }
      return _return;
    }
  }

  public void Render(SVGGraphicsPath _graphicsPath) {
    _graphicsPath.AddQuadraticCurveTo(controlPoint1, currentPoint);
  }
}
