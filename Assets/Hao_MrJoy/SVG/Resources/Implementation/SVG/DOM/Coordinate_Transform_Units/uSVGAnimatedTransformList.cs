//using UnityEngine;
//using System.Collections;

public class uSVGAnimatedTransformList {
	private uSVGTransformList m_baseVal;
	private uSVGTransformList m_animVal;
	/*********************************************************************************************/
	public uSVGTransformList baseVal {
		get{return this.m_baseVal;}
	}
	public uSVGTransformList animVal {
		get{return this.m_animVal;}
	}
	/*********************************************************************************************/
	public uSVGAnimatedTransformList() {
		this.m_baseVal = new uSVGTransformList();
		this.m_animVal = this.m_baseVal;
	}
	public uSVGAnimatedTransformList(uSVGTransformList transformList) {
		this.m_baseVal = new uSVGTransformList();
		this.m_baseVal.AppendItems(transformList);
		this.m_animVal = this.m_baseVal;
	}
	public uSVGAnimatedTransformList(uSVGTransform svgTransform) {
		this.m_baseVal = new uSVGTransformList();
		this.m_baseVal.AppendItem(svgTransform);
		this.m_animVal = this.m_baseVal;
	}
	public uSVGAnimatedTransformList(string transform) {
		this.m_baseVal = new uSVGTransformList(transform);
		this.m_animVal = this.m_baseVal;
	}	
}