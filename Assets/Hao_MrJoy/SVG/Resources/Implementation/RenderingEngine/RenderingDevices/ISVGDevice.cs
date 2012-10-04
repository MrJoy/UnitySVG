using UnityEngine;

public interface ISVGDevice {
  void SetDevice(int width, int height);
  void SetPixel(int x, int y);
  Color GetPixel(int x, int y);
  void SetColor(Color color);
  Texture2D Render();
  void GetBufferSize(ref int width, ref int height);
}
