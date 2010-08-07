
public class uSVGDocument {
  private uSVGSVGElement _rootElement = null;
  private uSVGGraphics _render;
  /***********************************************************************************/
  public uSVGSVGElement rootElement {
    get {
      if(_rootElement == null) {
        while(!parser.IsEOF && parser.Node.Name != "svg")
          parser.Next();
        _rootElement = new uSVGSVGElement(parser, new uSVGTransformList(), new uSVGPaintable(), _render);
      }
      return _rootElement;
    }
  }
  /***********************************************************************************/
  //_nodeByTagName la 1 dictionary de luu tru cac Element ton tai ben trong 1 SVG Document
  //private Dictionary<string, uSVGElement> _nodeByTagName = new Dictionary<string, uSVGElement>();
  /***********************************************************************************/
  private uXMLImp parser;
  public uSVGDocument(string originalDocument, uSVGGraphics r) {
    parser = new uXMLImp(originalDocument);
    _render = r;
  }
}