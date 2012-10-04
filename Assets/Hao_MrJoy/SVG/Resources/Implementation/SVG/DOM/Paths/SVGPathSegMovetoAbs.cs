using UnityEngine;

public class SVGPathSegMovetoAbs : SVGPathSeg, ISVGDrawableSeg {
  private float _x = 0f;
  private float _y = 0f;
  //================================================================================
  public float x {
    get{ return this._x;}
  }
  //-----
  public float y {
    get{ return this._y;}
  }
  //================================================================================
  public SVGPathSegMovetoAbs(float x, float y) : base() {
    this._x = x;
    this._y = y;
  }
  //================================================================================
  public override Vector2 currentPoint{
    get{
      Vector2 _return = new Vector2(this._x, this._y);
      return _return;
    }
  }
  //--------------------------------------------------------------------------------
  //Method: Render
  //--------------------------------------------------------------------------------
  public void Render(SVGGraphicsPath _graphicsPath) {
    Vector2 p;
    p = new Vector2(this._x, this._y);
    _graphicsPath.AddMoveTo(p);
  }
}
