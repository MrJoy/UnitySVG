using UnityEngine;

public class Invoke : MonoBehaviour {
	public TextAsset SVGFile = null;
	private Implement m_implement;
	// Use this for initialization
	void Start () {
		if (SVGFile != null) {
			m_implement = new Implement(this.SVGFile);
			m_implement.f_StartProcess();
			renderer.material.mainTexture = m_implement.f_GetTexture();	
			Vector2 ts = renderer.material.mainTextureScale;
			ts.x *= -1;
			renderer.material.mainTextureScale = ts;
			renderer.material.mainTexture.filterMode = FilterMode.Trilinear;
		}
	}
}
