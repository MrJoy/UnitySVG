public class uSVGTransformable {
	private uSVGAnimatedTransformList m_inheritTransformList;
	private uSVGAnimatedTransformList m_currentTransformList;
	private uSVGAnimatedTransformList m_summaryTransformList;
	/**********************************************************************************/
	public uSVGAnimatedTransformList inheritTransformList {
		get{ return this.m_inheritTransformList;}
		set{
			this.m_inheritTransformList = value;
			this.m_summaryTransformList = new uSVGAnimatedTransformList();
			this.m_summaryTransformList.animVal.AppendItems(this.m_inheritTransformList.animVal);				
			if (this.m_currentTransformList != null) {
				this.m_summaryTransformList.animVal.AppendItems(this.m_currentTransformList.animVal);
			}
		}
	}
	public uSVGAnimatedTransformList currentTransformList {
		get{ return this.m_currentTransformList;}
		set{ 
			this.m_currentTransformList = value;
			this.m_summaryTransformList = new uSVGAnimatedTransformList();
			this.m_summaryTransformList.animVal.AppendItems(this.m_inheritTransformList.animVal);
			this.m_summaryTransformList.animVal.AppendItems(this.m_currentTransformList.animVal);
		}
	}
	public uSVGAnimatedTransformList summaryTransformList {
		get{return this.m_summaryTransformList;}
	}
	public float transformAngle {
		get{
			float m_angle = 0.0f;
			for (int i = 0; i < m_summaryTransformList.animVal.numberOfItems; i++ ) {
				uSVGTransform m_temp = m_summaryTransformList.animVal.GetItem(i);
				if (m_temp.type == uSVGTransformType.SVG_TRANSFORM_ROTATE)  {
					m_angle += m_temp.angle;
				}
			}
			return m_angle;
		}
	}
	public uSVGMatrix transformMatrix {
		get{return this.summaryTransformList.animVal.Consolidate().matrix;}
	}
	/*********************************************************************************************/
	public uSVGTransformable(uSVGAnimatedTransformList transformList) {
		this.inheritTransformList = transformList;
	}
}