public class uSVGTransformable {
	private uSVGTransformList m_inheritTransformList;
	private uSVGTransformList m_currentTransformList;
	private uSVGTransformList m_summaryTransformList;
	/**********************************************************************************/
	public uSVGTransformList inheritTransformList {
		get{ return this.m_inheritTransformList;}
		set{
			this.m_inheritTransformList = value;
			this.m_summaryTransformList = new uSVGTransformList();
			this.m_summaryTransformList.AppendItems(this.m_inheritTransformList);				
			if (this.m_currentTransformList != null) {
				this.m_summaryTransformList.AppendItems(this.m_currentTransformList);
			}
		}
	}
	public uSVGTransformList currentTransformList {
		get{ return this.m_currentTransformList;}
		set{ 
			this.m_currentTransformList = value;
			this.m_summaryTransformList = new uSVGTransformList();
			this.m_summaryTransformList.AppendItems(this.m_inheritTransformList);
			this.m_summaryTransformList.AppendItems(this.m_currentTransformList);
		}
	}
	public uSVGTransformList summaryTransformList {
		get{return this.m_summaryTransformList;}
	}
	public float transformAngle {
		get{
			float m_angle = 0.0f;
			for (int i = 0; i < m_summaryTransformList.numberOfItems; i++ ) {
				uSVGTransform m_temp = m_summaryTransformList.GetItem(i);
				if (m_temp.type == uSVGTransformType.SVG_TRANSFORM_ROTATE)  {
					m_angle += m_temp.angle;
				}
			}
			return m_angle;
		}
	}
	public uSVGMatrix transformMatrix {
		get{return this.summaryTransformList.Consolidate().matrix;}
	}
	/*********************************************************************************************/
	public uSVGTransformable(uSVGTransformList transformList) {
		this.inheritTransformList = transformList;
	}
}