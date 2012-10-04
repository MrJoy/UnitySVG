using UnityEngine;
public class Implement {
  private TextAsset _SVGFile;
  private Texture2D _texture = null;
  private ISVGDevice _device;
  private SVGGraphics _graphics;
  private SVGDocument _svgDocument;

  /***********************************************************************************/
  public Implement(TextAsset svgFile, ISVGDevice device) {
    this._SVGFile = svgFile;
    _device = device;
    _graphics = new SVGGraphics(_device);
  }
  /***********************************************************************************/
  /*-----------------------------------------------------------
  Methods: CreateEmptySVGDocument
  Use: tao 1 SVGDocument trong, day la buoc khoi dau cua viec bat
  dau phan tich va do du lieu vao trong 1 SVGDocument
  -------------------------------------------------------------*/
  private void CreateEmptySVGDocument() {
    _svgDocument = new SVGDocument(this._SVGFile.text, this._graphics);
  }

  /*-----------------------------------------------------------
  Methods: StarProcess1
  Use: ta bat dau doc du lieu de do vao SVGDocument
  -------------------------------------------------------------*/
  public void StartProcess() {
Profiler.BeginSample("SVG.Implement.StartProcess[CreateEmptySVGDocument]");
    CreateEmptySVGDocument();
Profiler.EndSample();
Profiler.BeginSample("SVG.Implement.StartProcess[GetRootElement]");
    SVGSVGElement _rootSVGElement = this._svgDocument.rootElement;
Profiler.EndSample();
Profiler.BeginSample("SVG.Implement.StartProcess[ClearCanvas]");
    this._graphics.SetColor(Color.white);
Profiler.EndSample();
Profiler.BeginSample("SVG.Implement.StartProcess[RenderSVGElement]");
    _rootSVGElement.Render();
Profiler.EndSample();
Profiler.BeginSample("SVG.Implement.StartProcess[RenderGraphics]");
    this._texture = _graphics.Render();
Profiler.EndSample();
  }
  /***********************************************************************************/
  public void NewSVGFile(TextAsset svgFile) {
    this._SVGFile = svgFile;
  }
  public Texture2D GetTexture() {
    if(this._texture == null) {
      return new Texture2D(0, 0, TextureFormat.ARGB32, false);
    }
    return this._texture;
  }
}
