using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public enum NodeKind : int { Inline, BlockOpen, BlockClose };
public struct Node {
  public string Name;
  public NodeKind Kind;
  public AttributeList Attributes;
  public Node(string n, NodeKind k, AttributeList a) {
    Name = n;
    Kind = k;
    Attributes = a;
  }
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
    Stream.Add(new Node(name, NodeKind.Inline, new AttributeList(attrs)));
  }

  public void OnStartElement(string name, AttributeList attrs) {
    Stream.Add(new Node(name, NodeKind.BlockOpen, new AttributeList(attrs)));
  }

  public void OnEndElement(string name) {
    Stream.Add(new Node(name, NodeKind.BlockClose, new AttributeList()));
  }
}
