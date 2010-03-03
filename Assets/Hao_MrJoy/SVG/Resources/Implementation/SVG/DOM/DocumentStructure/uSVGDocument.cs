
public class uSVGDocument : uXMLImp {
	private uSVGSVGElement m_rootElement = null;
	private uSVGGraphics m_render;
	/***********************************************************************************/
	public uSVGSVGElement rootElement {
		get {
			if (m_rootElement == null) {
			  while(!f_IsCurrentTagName("svg"))
			    f_ReadNextTag();
				m_rootElement = new uSVGSVGElement(this, new uSVGAnimatedTransformList(),
														 new uSVGPaintable(), this.m_render);
			}
			return this.m_rootElement;
		}
	}
	/***********************************************************************************/
	//m_nodeByTagName la 1 dictionary de luu tru cac Element ton tai ben trong 1 SVG Document
	//private Dictionary<string, uSVGElement> m_nodeByTagName = new Dictionary<string, uSVGElement>();
	/***********************************************************************************/
	public uSVGDocument(string originalDocument, uSVGGraphics m_render):base(originalDocument) {
		this.m_render = m_render;
	}
}