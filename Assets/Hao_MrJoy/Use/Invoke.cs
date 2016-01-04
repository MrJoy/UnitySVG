using System.Collections;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[DisallowMultipleComponent()]
public class Invoke : MonoBehaviour {
  public TextAsset SVGFile = null;
  [Tooltip("Use a faster rendering approach that takes notably more memory.")]
  public bool fastRenderer = false;

  [Space(15)]
  public TextureWrapMode wrapMode = TextureWrapMode.Clamp;
  public FilterMode filterMode = FilterMode.Trilinear;
  [Range(0, 9)]
  public int anisoLevel = 9;

  private void Start() {
    //yield return new WaitForSeconds(0.1f);
    if(SVGFile != null) {
      Stopwatch w = new Stopwatch();

      w.Reset();
      w.Start();
      ISVGDevice device;
      if(fastRenderer)
        device = new SVGDeviceFast();
      else
        device = new SVGDeviceSmall();
      var implement = new Implement(SVGFile, device);
      w.Stop();
      long c = w.ElapsedMilliseconds;

      w.Reset();
      w.Start();
      implement.StartProcess();
      w.Stop();
      long p = w.ElapsedMilliseconds;

      w.Reset();
      w.Start();
      var myRenderer = GetComponent<Renderer>();
      var result = implement.GetTexture();
      result.wrapMode   = wrapMode;
      result.filterMode = filterMode;
      result.anisoLevel = anisoLevel;
      myRenderer.material.mainTexture = result;
      w.Stop();
      long r = w.ElapsedMilliseconds;
      UnityEngine.Debug.LogFormat("Construction: {0} ms, Processing: {1} ms, Rendering: {2} ms", c, p, r);
    }
  }
}
