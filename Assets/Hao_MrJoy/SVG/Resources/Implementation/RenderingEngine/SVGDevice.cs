using UnityEngine;

public class SVGDevice {
  private Texture2D _texture;

  private int _width;
  private int _height;

  private Color _color = Color.white;
  /***********************************************************************************/
  public void SetDevice(float width, float height) {
    this.SetDevice((int)width, (int)height);
  }
  public void SetDevice(int width, int height) {
    this._texture = new Texture2D(width, height, TextureFormat.RGB24, false);
    this._texture.hideFlags = HideFlags.HideAndDontSave;
    this._width = width;
    this._height = height;
  }

  public void SetPixel(int x, int y) {
    if((x >= 0)&&( x < this._width)&&(y >= 0)&&( y < this._height)) {
      this._texture.SetPixel(x, y, this._color);
    }
  }
  public Color GetPixel(int x, int y) {
    return this._texture.GetPixel(x, y);
  }

  public void SetColor(Color color) {
    this._color.r = color.r;
    this._color.g = color.g;
    this._color.b = color.b;
  }

  public Texture2D Render() {
    this._texture.Apply();
    return this._texture;
  }

  public void GetBufferSize(ref int width, ref int height) {
    width = this._width;
    height = this._height;
  }
  /***********************************************************************************/
}