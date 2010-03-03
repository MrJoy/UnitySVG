using UnityEngine;
public class Implement {
	private TextAsset m_SVGFile;
	private Texture2D m_texture = null;
	private	uSVGDevice m_device;
	private	uSVGGraphics m_graphics;
	private uSVGDocument m_svgDocument;

	//private ArrayList m_listElement = new ArrayList();
	//private System.Type m_type;
	/***********************************************************************************/
	public Implement(TextAsset svgFile) {
		this.m_SVGFile = svgFile;
		m_device = new uSVGDevice();
		m_graphics = new uSVGGraphics(m_device);
		//m_xmlImplement = new uXMLImp();
		//m_type = typeof(uSVGRectElement);
	}
	/***********************************************************************************/
	/*-----------------------------------------------------------
	Methods: f_CreateEmptySVGDocument
	Use: tao 1 uSVGDocument trong, day la buoc khoi dau cua viec bat
	dau phan tich va do du lieu vao trong 1 SVGDocument
	-------------------------------------------------------------*/
	private void f_CreateEmptySVGDocument() {
		m_svgDocument = new uSVGDocument(this.m_SVGFile.text, this.m_graphics);
	}
	
	/*-----------------------------------------------------------
	Methods: f_StarProcess1
	Use: ta bat dau doc du lieu de do vao uSVGDocument
	-------------------------------------------------------------*/
	public void f_StartProcess() {
Profiler.BeginSample("SVG.Implement.StartProcess[CreateEmptySVGDocument]");
		f_CreateEmptySVGDocument();
Profiler.EndSample();
Profiler.BeginSample("SVG.Implement.StartProcess[GetRootElement]");
		uSVGSVGElement m_rootSVGElement = this.m_svgDocument.rootElement;
Profiler.EndSample();
		this.m_graphics.SetColor(Color.white);
Profiler.BeginSample("SVG.Implement.StartProcess[RenderSVGElement]");
		m_rootSVGElement.f_Render();
Profiler.EndSample();
Profiler.BeginSample("SVG.Implement.StartProcess[RenderGraphics]");
		this.m_texture = m_graphics.Render();
Profiler.EndSample();
	}
	/***********************************************************************************/
	public void f_NewSVGFile(TextAsset svgFile) {
		this.m_SVGFile = svgFile;
	}
	public Texture2D f_GetTexture() {
		if (this.m_texture == null) {
			return new Texture2D(0, 0, TextureFormat.ARGB32, false);
		}
		return this.m_texture;
	}
}