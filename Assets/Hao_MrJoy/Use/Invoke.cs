using UnityEngine;

//[RequireComponent(typeof(Material))]
public class Invoke : MonoBehaviour {
	public TextAsset SVGFile = null;
	private Implement m_implement;
	// Use this for initialization
	void Start () {
		if (SVGFile != null) {
			m_implement = new Implement(this.SVGFile);
			m_implement.f_StartProcess();
			renderer.material.mainTexture = m_implement.f_GetTexture();	
			renderer.material.mainTexture.filterMode = FilterMode.Trilinear;
		}
	}
}
