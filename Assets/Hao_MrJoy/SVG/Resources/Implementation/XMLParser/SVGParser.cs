using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public enum SVGNodeName {
  Rect,
  Line,
  Circle,
  Ellipse,
  PolyLine,
  Polygon,
  Path,
  SVG,
  G,
  LinearGradient,
  RadialGradient,
  Defs,
  Title,
  Desc,
  Stop
}

public class Node {
  public SVGNodeName Name;
  public Dictionary<string, string> Attributes;

  public Node(SVGNodeName n, Dictionary<string, string> a) {
    Name = n;
    Attributes = a;
  }
}

public class InlineNode : Node {
  public InlineNode(SVGNodeName n, Dictionary<string, string> a) : base(n, a) {
  }
}

public class BlockOpenNode : Node {
  public BlockOpenNode(SVGNodeName n, Dictionary<string, string> a) : base(n, a) {
  }
}

public class BlockCloseNode : Node {
  public BlockCloseNode(SVGNodeName n, Dictionary<string, string> a) : base(n, a) {
  }
}

public class SVGParser : SmallXmlParser.IContentHandler {
  private SmallXmlParser _parser = new SmallXmlParser();

  /***********************************************************************************/
  public SVGParser() {
  }

  public SVGParser(string text) {
    _parser.Parse(new StringReader(text), this);
  }

  /***********************************************************************************/
  private List<Node> Stream = new List<Node>();
  private int idx = 0;

  public Node Node { get { return Stream[idx]; } }

  public bool Next() {
    idx++;
    return !IsEOF;
  }

  public bool IsEOF { get { return idx >= Stream.Count; } }

  public void OnStartParsing(SmallXmlParser parser) {
    idx = 0;
  }

  public void OnInlineElement(string name, Dictionary<string, string> attrs) {
    Stream.Add(new InlineNode(Lookup(name), new Dictionary<string, string>(attrs)));
  }

  public void OnStartElement(string name, Dictionary<string, string> attrs) {
    Stream.Add(new BlockOpenNode(Lookup(name), new Dictionary<string, string>(attrs)));
  }

  public void OnEndElement(string name) {
    Stream.Add(new BlockCloseNode(Lookup(name), new Dictionary<string, string>()));
  }

  public void GetElementList(List<ISVGDrawable> elementList, SVGPaintable paintable,
                             SVGGraphics render, SVGTransformList summaryTransformList) {
    bool exitFlag = false;
    while(!exitFlag && Next()) {
      if(Node is BlockCloseNode) {
        exitFlag = true;
        continue;
      }

      switch(Node.Name) {
      case SVGNodeName.Rect:     elementList.Add(new SVGRectElement(Node.Attributes, summaryTransformList, paintable, render)); break;
      case SVGNodeName.Line:     elementList.Add(new SVGLineElement(Node.Attributes, summaryTransformList, paintable, render)); break;
      case SVGNodeName.Circle:   elementList.Add(new SVGCircleElement(Node.Attributes, summaryTransformList, paintable, render)); break;
      case SVGNodeName.Ellipse:  elementList.Add(new SVGEllipseElement(Node.Attributes, summaryTransformList, paintable, render)); break;
      case SVGNodeName.PolyLine: elementList.Add(new SVGPolylineElement(Node.Attributes, summaryTransformList, paintable, render)); break;
      case SVGNodeName.Polygon:  elementList.Add(new SVGPolygonElement(Node.Attributes, summaryTransformList, paintable, render)); break;
      case SVGNodeName.Path:     elementList.Add(new SVGPathElement(Node.Attributes, summaryTransformList, paintable, render)); break;

      case SVGNodeName.SVG:      elementList.Add(new SVGSVGElement(this, summaryTransformList, paintable, render)); break;
      case SVGNodeName.G:        elementList.Add(new SVGGElement(this, summaryTransformList, paintable, render)); break;

      case SVGNodeName.LinearGradient: paintable.AppendLinearGradient(new SVGLinearGradientElement(this, Node.Attributes)); break;
      case SVGNodeName.RadialGradient: paintable.AppendRadialGradient(new SVGRadialGradientElement(this, Node.Attributes)); break;

      case SVGNodeName.Defs:  GetElementList(elementList, paintable, render, summaryTransformList); break;
      case SVGNodeName.Title: GetElementList(elementList, paintable, render, summaryTransformList); break;
      case SVGNodeName.Desc:  GetElementList(elementList, paintable, render, summaryTransformList); break;
      }
    }
  }

  private static SVGNodeName Lookup(string name) {
    SVGNodeName retVal;
    // TODO: Experiment with a dictionary lookup here.
    switch(name) {
    case "rect": retVal = SVGNodeName.Rect; break;
    case "line": retVal = SVGNodeName.Line; break;
    case "circle": retVal = SVGNodeName.Circle; break;
    case "ellipse": retVal = SVGNodeName.Ellipse; break;
    case "polyline": retVal = SVGNodeName.PolyLine; break;
    case "polygon": retVal = SVGNodeName.Polygon; break;
    case "path": retVal = SVGNodeName.Path; break;
    case "svg": retVal = SVGNodeName.SVG; break;
    case "g": retVal = SVGNodeName.G; break;
    case "linearGradient": retVal = SVGNodeName.LinearGradient; break;
    case "radialGradient": retVal = SVGNodeName.RadialGradient; break;
    case "defs": retVal = SVGNodeName.Defs; break;
    case "title": retVal = SVGNodeName.Title; break;
    case "desc": retVal = SVGNodeName.Desc; break;
    case "stop": retVal = SVGNodeName.Stop; break;
    default: throw new System.Exception("Unknown element type '" + name + "'!");
    }
    return retVal;
  }
}
