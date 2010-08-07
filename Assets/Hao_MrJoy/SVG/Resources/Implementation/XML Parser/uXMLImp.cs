using System.IO;
using System.Text;

public class uXMLImp : SmallXmlParser.IContentHandler {
  public enum XMLTagState {OPEN, CLOSE, BOTH}

  private SmallXmlParser _parser = new SmallXmlParser();
  private TextReader _textReader;

  private string _currentName = "";
  private string _currentLineText = "";
  private XMLTagState _currentTagState;
  private AttributeList _currentList;

  /***********************************************************************************/
  public uXMLImp() { }

  public uXMLImp(string text) {
    _textReader = new StringReader(text);
    _parser.Pause();
    _parser.Parse(_textReader, this);
  }

  //---------------------------------------------------------
  //Methods: CloneAttrsList
  //Purpose: create a clone Attribute List from XMLParser Handle for storing.
  //---------------------------------------------------------
  private void CloneAttrsList(AttributeList attrs) {
    this._currentList = new AttributeList(attrs);
  }

  //---------------------------------------------------------
  //Methods: ReadNextTag
  //Purpose: Unpause XMLParser to reading nex Tag.
  //---------------------------------------------------------
  public bool ReadNextTag() {
    return _parser.UnPause();
  }

  //---------------------------------------------------------
  //Methods: GetCurrentTagState
  //Purpose: return current XMLTag State
  //State:
  //    XMLTagState.OPEN   : current XMLTag is Opened.
  //    XMLTagState.CLOSE   : current XMLTag is Closed. This XMLTag is opened in before XMLTag.
  //    XMLTagState.BOTH   : current XMLTag is Open and Closed in the same XMLTag.
  //---------------------------------------------------------
  public XMLTagState GetCurrentTagState() {
    return _currentTagState;
  }

  //---------------------------------------------------------
  //Methods: GetCurrentTagName
  //Purpose: return current Tag Name of XML Tag reading.
  //---------------------------------------------------------
  public string GetCurrentTagName() {
    return _currentName;
  }

  //---------------------------------------------------------
  //Methods: GetCurrentAttributesList
  //Purpose: Return current Attributes List.
  //---------------------------------------------------------

  public AttributeList GetCurrentAttributesList() {
    return _currentList;
  }

  //---------------------------------------------------------
  //Methods: ReadFirstElement
  //Purpose: ....
  //---------------------------------------------------------
  public AttributeList GetNextAttrList() {
    ReadNextTag();
    return _currentList;
  }

  //-----------------------------------------------------------------------------------------//
  //Cac Functions lien qua den xu ly SVG Document
  //-----------------------------------------------------------------------------------------//
  //---------------------------------------------------------
  //Methods: ReadFirstElement
  //Purpose: ....
  //---------------------------------------------------------

  private StringBuilder sb = new StringBuilder();
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
  }
  //---------------------------------------------------------
  //Methods: GetUntilCloseTag
  //Purpose: ....
  //---------------------------------------------------------
  public string GetUntilCloseTag(string name) {
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
  }

  /***********************************************************************************/
  public void OnStartParsing(SmallXmlParser parser) { }

  public void OnEndParsing(SmallXmlParser parser) { }

  public void OnStartEndElement(string name, AttributeList attrs, string lineText) {
    _parser.Pause();
    _currentName = name;
    _currentLineText = lineText;
    CloneAttrsList(attrs);
    _currentTagState = XMLTagState.BOTH;
  }
  public void OnStartElement(string name, AttributeList attrs, string lineText) {
    _parser.Pause();
    _currentName = name;
    _currentLineText = lineText;
    CloneAttrsList(attrs);
    _currentTagState = XMLTagState.OPEN;
  }

  public void OnEndElement(string name, string lineText) {
    _parser.Pause();
    _currentName = name;
    _currentLineText = lineText;
    _currentTagState = XMLTagState.CLOSE;
  }

  public void OnChars(string s) { }

  public void OnIgnorableWhitespace(string s) { }

  public void OnProcessingInstruction(string name, string text) { }
}
