using UnityEngine;
using System;
using System.Diagnostics;

public class Invoke : MonoBehaviour {
  public TextAsset SVGFile = null;
  public bool useFastButBloatedRenderer = false;
  private Implement m_implement;
  // Use this for initialization
  void Start () {
    if (SVGFile != null) {
      Stopwatch w = new Stopwatch();

      w.Reset();
      w.Start();
      ISVGDevice device;
      if(useFastButBloatedRenderer)
        device = new SVGDeviceFast();
      else
        device = new SVGDeviceSmall();
      m_implement = new Implement(this.SVGFile, device);
      w.Stop();
      long c = w.ElapsedMilliseconds;

      w.Reset();
      w.Start();
      m_implement.StartProcess();
      w.Stop();
      long p = w.ElapsedMilliseconds;

      w.Reset();
      w.Start();
      renderer.material.mainTexture = m_implement.GetTexture();
      w.Stop();
      long r = w.ElapsedMilliseconds;
      UnityEngine.Debug.Log("Construction: " + Format(c) + ", Processing: " + Format(p) + ", Rendering: " + Format(r));

      Vector2 ts = renderer.material.mainTextureScale;
      ts.x *= -1;
      renderer.material.mainTextureScale = ts;
      renderer.material.mainTexture.filterMode = FilterMode.Trilinear;
    }
  }

  private string Format(long ts) {
    return String.Format("{0} ms", ts);
  }
}
