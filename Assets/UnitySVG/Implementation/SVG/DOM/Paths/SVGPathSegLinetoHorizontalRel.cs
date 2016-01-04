using UnityEngine;

public class SVGPathSegLinetoHorizontalRel : SVGPathSeg, ISVGDrawableSeg {
  private float _x = 0f;

  public float x { get { return this._x; } }

  public SVGPathSegLinetoHorizontalRel(float x) : base() {
    this._x = x;
  }

  public override Vector2 currentPoint {
    get {
      Vector2 _return = new Vector2(0f, 0f);
      SVGPathSeg _prevSeg = previousSeg;
      if(_prevSeg != null) {
        _return.x = _prevSeg.currentPoint.x + this._x;
        _return.y = _prevSeg.currentPoint.y;
      }
      return _return;
    }
  }

  public void Render(SVGGraphicsPath _graphicsPath) {
    _graphicsPath.AddLineTo(currentPoint);
  }
}
