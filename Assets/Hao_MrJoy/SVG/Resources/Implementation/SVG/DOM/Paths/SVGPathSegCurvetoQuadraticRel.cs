using UnityEngine;

public class SVGPathSegCurvetoQuadraticRel : SVGPathSegCurvetoQuadratic, ISVGDrawableSeg {
  private float _x = 0f, _y = 0f, _x1 = 0f, _y1 = 0f;

  public float x { get { return this._x; } }

  public float y { get { return this._y; } }

  public float x1 { get { return this._x1; } }

  public float y1 { get { return this._y1; } }

  public SVGPathSegCurvetoQuadraticRel(float x1, float y1, float x, float y) : base() {
    this._x = x;
    this._y = y;
    this._x1 = x1;
    this._y1 = y1;
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
        _return.x = _prevSeg.currentPoint.x + this._x1;
        _return.y = _prevSeg.currentPoint.y + this._y1;
      }
      return _return;
    }
  }

  public void Render(SVGGraphicsPath _graphicsPath) {
    _graphicsPath.AddQuadraticCurveTo(controlPoint1, currentPoint);
  }
}
