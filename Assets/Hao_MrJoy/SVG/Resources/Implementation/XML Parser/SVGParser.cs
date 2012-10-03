using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public enum SVGNodeName {
  Rect, Line, Circle, Ellipse, PolyLine, Polygon, Path, SVG, G, LinearGradient, RadialGradient, Defs, Title, Desc, Stop
}

public class Node {
  public SVGNodeName Name;
  public AttributeList Attributes;
  public Node(SVGNodeName n, AttributeList a) {
    Name = n;
    Attributes = a;
  }
}
public class InlineNode : Node {
  public InlineNode(SVGNodeName n, AttributeList a) : base(n, a) {}
}
public class BlockOpenNode : Node {
  public BlockOpenNode(SVGNodeName n, AttributeList a) : base(n, a) {}
}
public class BlockCloseNode : Node {
  public BlockCloseNode(SVGNodeName n, AttributeList a) : base(n, a) {}
}

public class SVGParser : SmallXmlParser.IContentHandler {
  private SmallXmlParser _parser = new SmallXmlParser();

  /***********************************************************************************/
  public SVGParser() { }

  public SVGParser(string text) {
    _parser.Parse(new StringReader(text), this);
  }

  /***********************************************************************************/
  private List<Node> Stream = new List<Node>();
  private int idx = 0;

  public Node Node {
    get { return Stream[idx]; }
  }

  public bool Next() {
    idx++;
    return !IsEOF;
  }

  public bool IsEOF {
    get {
      return idx >= Stream.Count;
    }
  }

  public void OnStartParsing(SmallXmlParser parser) {
    idx = 0;
  }

  public void OnInlineElement(string name, AttributeList attrs) {
    Stream.Add(new InlineNode(Lookup(name), new AttributeList(attrs)));
  }

  public void OnStartElement(string name, AttributeList attrs) {
    Stream.Add(new BlockOpenNode(Lookup(name), new AttributeList(attrs)));
  }

  public void OnEndElement(string name) {
    Stream.Add(new BlockCloseNode(Lookup(name), new AttributeList()));
  }

  private static SVGNodeName Lookup(string name) {
    SVGNodeName retVal;
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
