using UnityEngine;

public class SVGDeviceFast : ISVGDevice {
  private Texture2D _texture;

  private int _width;
  private int _height;

  public int Width { get { return _width; } }

  public int Height { get { return _height; } }

  private Color _color = Color.white;
  private Color[] pixels;

  public void SetDevice(int width, int height) {
    _width = width;
    _height = height;
    if(pixels == null || _width != width || _height != height)
      pixels = new Color[_width * _height];
  }

  public void SetPixel(int x, int y) {
    if((x >= 0) && (x < _width) && (y >= 0) && (y < _height))
      pixels[y * _height + (_width - x) - 1] = _color;
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
    _texture.SetPixels(pixels);
    _texture.Apply();
    return _texture;
  }
}
