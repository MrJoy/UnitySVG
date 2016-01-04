using System.Collections;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Invoke : MonoBehaviour {
  public TextAsset SVGFile = null;
  public bool useFastButBloatedRenderer = false;

  private void Start() {
    //yield return new WaitForSeconds(0.1f);
    if(SVGFile != null) {
      Stopwatch w = new Stopwatch();

      w.Reset();
      w.Start();
      ISVGDevice device;
      if(useFastButBloatedRenderer)
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
      myRenderer.material.mainTexture = implement.GetTexture();
      w.Stop();
      long r = w.ElapsedMilliseconds;
      UnityEngine.Debug.LogFormat("Construction: {0} ms, Processing: {1} ms, Rendering: {2} ms", c, p, r);
      myRenderer.material.mainTexture.filterMode = FilterMode.Trilinear;
    }
  }
}
