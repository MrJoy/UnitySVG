using UnityEngine;

public class SVGDeviceSmall : ISVGDevice
{
	private Texture2D _texture;

	private int _width;
	private int _height;

	public int Width { get {  return _width; } }
	public int Height { get { return _height; } }

	private Color _color = Color.white;

	public void SetDevice(int width, int height)
	{
		if (_texture == null)
		{
			_texture = new Texture2D(width, height, TextureFormat.RGB24, false);
			_texture.hideFlags = HideFlags.HideAndDontSave;
			_width = width;
			_height = height;
		}
	}

	public void SetPixel(int x, int y)
	{
		if ((x >= 0) && (x < _width) && (y >= 0) && (y < _height))
		{
			_texture.SetPixel(x, y, _color);
		}
	}

	public Color GetPixel(int x, int y)
	{
		return _texture.GetPixel(x, y);
	}

	public void SetColor(Color color)
	{
		_color.r = color.r;
		_color.g = color.g;
		_color.b = color.b;
	}

	public Texture2D Render()
	{
		_texture.Apply();
		return _texture;
	}
}