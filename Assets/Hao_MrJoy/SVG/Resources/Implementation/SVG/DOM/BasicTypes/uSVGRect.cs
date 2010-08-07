// TODO: Look at using UnityEngine.Rect?
public class uSVGRect {
  private float _x;
  private float _y;
  private float _width;
  private float _height;
  /***********************************************************************************/
  public float x {
    get {return this._x;}
  }
  public float y {
    get {return this._y;}
  }
  public float width {
    get {return this._width;}
  }
    public float height {
    get {return this._height;}
  }
  /***********************************************************************************/
  public uSVGRect(float x, float y, float width, float height) {
    this._x = x;
    this._y = y;
    this._width = width;
    this._height = height;
  }
}