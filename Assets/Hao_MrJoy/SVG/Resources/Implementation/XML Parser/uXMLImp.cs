using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public enum NodeKind : int { Inline, BlockOpen, BlockClose };
public struct Node {
  public string Name;
  public NodeKind Kind;
  public AttributeList Attributes;
  public int Level;
  public Node(string n, NodeKind k, AttributeList a, int l) {
    Name = n;
    Kind = k;
    Attributes = a;
    Level = l;
  }
}

public class uXMLImp : SmallXmlParser.IContentHandler {
  public enum XMLTagState {OPEN, CLOSE, BOTH}

  private SmallXmlParser _parser = new SmallXmlParser();
  private TextReader _textReader;

//  private string _currentName = "";
//  private string _currentLineText = "";
//  private XMLTagState _currentTagState;
//  private AttributeList _currentList;

  /***********************************************************************************/
  public uXMLImp() { }

  public uXMLImp(string text) {
    _textReader = new StringReader(text);
    _parser.Parse(_textReader, this);
  }

  //---------------------------------------------------------
  //Methods: CloneAttrsList
  //Purpose: create a clone Attribute List from XMLParser Handle for storing.
  //---------------------------------------------------------
//  private void CloneAttrsList(AttributeList attrs) {
//    this._currentList = new AttributeList(attrs);
//  }

  //---------------------------------------------------------
  //Methods: GetCurrentTagState
  //Purpose: return current XMLTag State
  //State:
  //    XMLTagState.OPEN   : current XMLTag is Opened.
  //    XMLTagState.CLOSE   : current XMLTag is Closed. This XMLTag is opened in before XMLTag.
  //    XMLTagState.BOTH   : current XMLTag is Open and Closed in the same XMLTag.
  //---------------------------------------------------------
//  public XMLTagState GetCurrentTagState() {
//    return _currentTagState;
//  }

  //---------------------------------------------------------
  //Methods: GetCurrentTagName
  //Purpose: return current Tag Name of XML Tag reading.
  //---------------------------------------------------------
//  public string GetCurrentTagName() {
//    return _currentName;
//  }

  //---------------------------------------------------------
  //Methods: GetCurrentAttributesList
  //Purpose: Return current Attributes List.
  //---------------------------------------------------------

//  public AttributeList GetCurrentAttributesList() {
//    return _currentList;
//  }

  //-----------------------------------------------------------------------------------------//
  //Cac Functions lien qua den xu ly SVG Document
  //-----------------------------------------------------------------------------------------//
  //---------------------------------------------------------
  //Methods: ReadFirstElement
  //Purpose: ....
  //---------------------------------------------------------

/*  private StringBuilder sb = new StringBuilder();
  public string ReadFirstElement(string name) {
    int level = 0;
    sb.Length = 0;
    if((_currentName == name)&&(_currentTagState == XMLTagState.OPEN)) {
      level++;
      sb.Append(_currentLineText);
    }
    while(ReadNextTag()) {
      if(_currentName == name) {
        if(_currentTagState == XMLTagState.OPEN) level++;
        else if(_currentTagState == XMLTagState.CLOSE) {
          level--;
          if(level == 0) {
            sb.Append(_currentLineText);
            break;
          }
        }
      }

      if(level > 0) sb.Append(_currentLineText);
    }
    return sb.ToString();
  } */
  //---------------------------------------------------------
  //Methods: GetUntilCloseTag
  //Purpose: ....
  //---------------------------------------------------------
/*  public string GetUntilCloseTag(string name) {
    int level = 1;
    sb.Length = 0;
    while(ReadNextTag()) {
      if(_currentName == name) {
        if(_currentTagState == XMLTagState.OPEN) level++;
        else if(_currentTagState == XMLTagState.CLOSE) {
          level--;
          if(level == 0) {
            sb.Append(_currentLineText);
            break;
          }
        }
      }

      if(level > 0) sb.Append(_currentLineText);
    }
    return sb.ToString();
  } */

  /***********************************************************************************/
  private int level = 0;
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
    level = 0;
    idx = 0;
  }

//  public void OnEndParsing(SmallXmlParser parser) { }

  public void OnInlineElement(string name, AttributeList attrs) {
    Stream.Add(new Node(name, NodeKind.Inline, new AttributeList(attrs), level));
  }
  public void OnStartElement(string name, AttributeList attrs) {
    Stream.Add(new Node(name, NodeKind.BlockOpen, new AttributeList(attrs), level++));
  }

  public void OnEndElement(string name) {
    Stream.Add(new Node(name, NodeKind.BlockClose, new AttributeList(), --level));
  }

//  public void OnChars(string s) { }

//  public void OnIgnorableWhitespace(string s) { }

//  public void OnProcessingInstruction(string name, string text) { }
}
