using UnityEngine;

public class SVGDevice {
  private Texture2D _texture;

  private int _width;
  private int _height;

  private Color _color = Color.white;
  private Color32[] pixels;
  /***********************************************************************************/
  public void SetDevice(int width, int height) {
    this._width = width;
    this._height = height;
    if(pixels == null || _width != width || _height != height) {
//Debug.Log("Establishing canvas of " + width + "x" + height + " pixels.");
      pixels = new Color32[_width * _height];
    }
  }

  public void SetPixel(int x, int y) {
    if((x >= 0) && (x < _width) && (y >= 0) && (y < _height)) {
      pixels[y * _height + x] = (Color32)_color;
    }
  }
  public Color GetPixel(int x, int y) {
    return pixels[y * _height + x];
  }

  public void SetColor(Color color) {
    _color = color;
  }

  public Texture2D Render() {
    if(_texture == null) {
      _texture = new Texture2D(_width, _height, TextureFormat.RGB24, false);
      _texture.hideFlags = HideFlags.HideAndDontSave;
    }
    _texture.SetPixels32(pixels);
    _texture.Apply();
    return _texture;
  }

  public void GetBufferSize(ref int width, ref int height) {
    width = _width;
    height = _height;
  }
  /***********************************************************************************/
}
