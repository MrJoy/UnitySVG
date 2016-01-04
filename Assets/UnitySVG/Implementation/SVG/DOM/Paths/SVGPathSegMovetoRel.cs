using UnityEngine;

public class SVGPathSegMovetoRel : SVGPathSeg, ISVGDrawableSeg {
  private float _x = 0f, _y = 0f;

  public float x { get { return this._x; } }

  public float y { get { return this._y; } }

  public SVGPathSegMovetoRel(float x, float y) : base() {
    this._x = x;
    this._y = y;
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

  public void Render(SVGGraphicsPath _graphicsPath) {
    _graphicsPath.AddMoveTo(currentPoint);
  }
}
