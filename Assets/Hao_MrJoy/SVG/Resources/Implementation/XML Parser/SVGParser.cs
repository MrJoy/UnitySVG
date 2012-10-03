using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class Node {
  public string Name;
  public AttributeList Attributes;
  public Node(string n, AttributeList a) {
    Name = n;
    Attributes = a;
  }
}
public class InlineNode : Node {
  public InlineNode(string n, AttributeList a) : base(n, a) {}
}
public class BlockOpenNode : Node {
  public BlockOpenNode(string n, AttributeList a) : base(n, a) {}
}
public class BlockCloseNode : Node {
  public BlockCloseNode(string n, AttributeList a) : base(n, a) {}
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
    Stream.Add(new InlineNode(name, new AttributeList(attrs)));
  }

  public void OnStartElement(string name, AttributeList attrs) {
    Stream.Add(new BlockOpenNode(name, new AttributeList(attrs)));
  }

  public void OnEndElement(string name) {
    Stream.Add(new BlockCloseNode(name, new AttributeList()));
  }
}
