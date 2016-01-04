using UnityEngine;

// TODO: Confirm that there's actually value in this.  I suspect it only appears to be smaller due to how Unity's
// TODO: profiler is accounting for things.  Things that may impact this:
// TODO: * Whether texture is marked as readable.
// TODO: * Whether rendering pipeline is GLES or Metal (not sure about analogues on the Direct3D front).
// TODO:   * Specifically, whether or not semantics require the GL stack to keep a local copy of the texture data in
// TODO:     host RAM to avoid clobbering the GPU if the GL app writes to that area again.
// TODO: * Internal representation of texture (24bpp vs. 32bpp).
public class SVGDeviceSmall : ISVGDevice {
  private Texture2D _texture;

  private int _width;
  private int _height;

  public int Width { get { return _width; } }

  public int Height { get { return _height; } }

  private Color _color = Color.white;

  public void SetDevice(int width, int height) {
    SetDevice(width, height, false, false);
  }

  public void SetDevice(int width, int height, bool mipmaps, bool linear) {
    if(_texture == null) {
      _texture = new Texture2D(width, height, TextureFormat.RGB24, mipmaps, linear);
      _texture.hideFlags = HideFlags.HideAndDontSave;
      _width = width;
      _height = height;
    }
  }

  public void SetPixel(int x, int y) {
    if((x >= 0) && (x < _width) && (y >= 0) && (y < _height))
      _texture.SetPixel(_width - x, y, _color);
  }

  public Color GetPixel(int x, int y) {
    return _texture.GetPixel(x, y);
  }

  public void SetColor(Color color) {
    _color.r = color.r;
    _color.g = color.g;
    _color.b = color.b;
  }

  public Texture2D Render() {
    _texture.Apply();
    return _texture;
  }
}
