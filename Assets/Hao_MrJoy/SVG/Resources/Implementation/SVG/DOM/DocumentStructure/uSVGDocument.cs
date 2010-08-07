
public class uSVGDocument : uXMLImp {
  private uSVGSVGElement _rootElement = null;
  private uSVGGraphics _render;
  /***********************************************************************************/
  public uSVGSVGElement rootElement {
    get {
      if(_rootElement == null) {
        while(!IsCurrentTagName("svg"))
          ReadNextTag();
        _rootElement = new uSVGSVGElement(this, new uSVGTransformList(),
                             new uSVGPaintable(), this._render);
      }
      return this._rootElement;
    }
  }
  /***********************************************************************************/
  //_nodeByTagName la 1 dictionary de luu tru cac Element ton tai ben trong 1 SVG Document
  //private Dictionary<string, uSVGElement> _nodeByTagName = new Dictionary<string, uSVGElement>();
  /***********************************************************************************/
  public uSVGDocument(string originalDocument, uSVGGraphics _render) :base(originalDocument) {
    this._render = _render;
  }
}