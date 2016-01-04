public class SVGDocument {
  private SVGSVGElement rootElement;
  private readonly SVGGraphics render;
  private readonly SVGParser parser;

  public SVGSVGElement RootElement {
    get {
      if(rootElement == null) {
        while(!parser.IsEOF && parser.Node.Name != SVGNodeName.SVG)
          parser.Next();
        rootElement = new SVGSVGElement(parser, new SVGTransformList(), new SVGPaintable(), render);
      }
      return rootElement;
    }
  }

  public SVGDocument(string originalDocument, SVGGraphics r) {
    parser = new SVGParser(originalDocument);
    render = r;
  }
}
