using UnityEngine;

public interface ISVGDevice {
  int Width { get; }

  int Height { get; }

  void SetDevice(int width, int height);

  void SetPixel(int x, int y);

  Color GetPixel(int x, int y);

  void SetColor(Color color);

  Texture2D Render();
}
