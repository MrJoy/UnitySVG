using UnityEngine;

public class SVGPathSegLinetoVerticalRel : SVGPathSeg, ISVGDrawableSeg {
  private float _y = 0f;

  public float y { get { return this._y; } }

  public SVGPathSegLinetoVerticalRel(float y) : base() {
    this._y = y;
  }

  public override Vector2 currentPoint {
    get {
      Vector2 _return = new Vector2(0f, 0f);
      SVGPathSeg _prevSeg = previousSeg;
      if(_prevSeg != null) {
        _return.x = _prevSeg.currentPoint.x;
        _return.y = _prevSeg.currentPoint.y + this._y;
      }
      return _return;
    }
  }

  public void Render(SVGGraphicsPath _graphicsPath) {
    _graphicsPath.AddLineTo(currentPoint);
  }
}
