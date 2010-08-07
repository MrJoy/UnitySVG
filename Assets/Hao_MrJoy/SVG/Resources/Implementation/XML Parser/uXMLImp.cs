using System.IO;

public class uXMLImp : SmallXmlParser.IContentHandler {
  public enum XMLTagState {OPEN, CLOSE, BOTH}

  private SmallXmlParser _parser   = new SmallXmlParser();
  private TextReader _textReader;

  private string _currentName = "";
  private string _currentLineText = "";
  private XMLTagState _currentTagState;
  private AttributeList _currentList;

  private string _svgText = "";

  /***********************************************************************************/
  public uXMLImp() {
  }

  public uXMLImp(string text) {
    this._svgText = text;
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

  /**********************************************************************************
  Public Classes:
    public bool       ReadNextTag();
    public XMLTagState     GetCurrentTagState();
    public string       GetCurrentTagName();
    public void       Reset(string text);
    public bool       IsCurrentTagName(string name);
    public int         Length();
    public IAttrList    GetCurrentAttributesList
    public IAttrList    GetNextAttrList

  ************************************************************************************/

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
    return this._currentTagState;
  }

  //---------------------------------------------------------
  //Methods: Reset
  //Purpose: Reset new String.
  //---------------------------------------------------------
  public void Reset(string text) {
    this._svgText = text;
    _textReader = new StringReader(text);
    _parser.New();
    _parser.Pause();
    _parser.Parse(_textReader, this);
  }
  public void Reset() {
    _textReader = new StringReader(this._svgText);
    _parser.New();
    _parser.Pause();
    _parser.Parse(_textReader, this);
  }
  //---------------------------------------------------------
  //Methods: GetCurrentTagName
  //Purpose: return current Tag Name of XML Tag reading.
  //---------------------------------------------------------
  public string GetCurrentTagName() {
    return this._currentName;
  }
  //---------------------------------------------------------
  //Methods: GetCurrentLineText
  //Purpose: return current Tag Name of XML Tag reading.
  //---------------------------------------------------------
  public string GetCurrentLineText() {
    return this._currentLineText;
  }

  //---------------------------------------------------------
  //Methods: IsCurrentTagName
  //Purpose: return true when current Tag Name = name.
  //---------------------------------------------------------

  public bool IsCurrentTagName(string name) {
    if(name == this._currentName) {
      return true;
    }
    return false;
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
    return this._currentList;
  }


  //---------------------------------------------------------
  //Methods: Length
  //Purpose: Return Count of attribute List of current XMLTag.
  //---------------------------------------------------------

  public int Length() {
    return this._currentList.Length;
  }

  //-----------------------------------------------------------------------------------------//
  //Cac Functions lien qua den xu ly SVG Document
  //-----------------------------------------------------------------------------------------//
  //---------------------------------------------------------
  //Methods: ReadFirstElement
  //Purpose: ....
  //---------------------------------------------------------

  public string ReadFirstElement(string name) {
    int level = 0;
    string _return = "";
    if((this._currentName == name)&&(this._currentTagState == XMLTagState.OPEN)) {
      level++;
      _return += _currentLineText;
    }
    while(ReadNextTag()) {
      if(this._currentName == name) {
        if(this._currentTagState == XMLTagState.OPEN)level++;
        else if(this._currentTagState == XMLTagState.CLOSE) {
          level--;
          if(level == 0) {
            _return += _currentLineText;
            break;
          }
        }
      }

      if(level > 0)
        _return += _currentLineText;
    }
    return _return;
  }
  //---------------------------------------------------------
  //Methods: GetUntilCloseTag
  //Purpose: ....
  //---------------------------------------------------------
  public string GetUntilCloseTag(string name) {
    int level = 1;
    string _return = "";
    while(ReadNextTag()) {
      if(this._currentName == name) {
        if(this._currentTagState == XMLTagState.OPEN)level++;
        else if(this._currentTagState == XMLTagState.CLOSE) {
          level--;
          if(level == 0) {
            _return += _currentLineText;
            break;
          }
        }
      }

      if(level > 0)
        _return += _currentLineText;
    }
    return _return;
  }

  /***********************************************************************************/
  public void OnStartParsing(SmallXmlParser parser) {
  }

  public void OnEndParsing(SmallXmlParser parser) {
  }

  public void OnStartEndElement(string name, AttributeList attrs, string lineText) {
    this._parser.Pause();
    this._currentName = name;
    this._currentLineText = lineText;
    CloneAttrsList(attrs);
    this._currentTagState = XMLTagState.BOTH;
  }
  public void OnStartElement(string name, AttributeList attrs, string lineText) {
    this._parser.Pause();
    this._currentName = name;
    this._currentLineText = lineText;
    CloneAttrsList(attrs);
    this._currentTagState = XMLTagState.OPEN;
  }

  public void OnEndElement(string name, string lineText) {
    this._parser.Pause();
    this._currentName = name;
    this._currentLineText = lineText;
    this._currentTagState = XMLTagState.CLOSE;
  }

  public void OnChars(string s) {
  }

  public void OnIgnorableWhitespace(string s) {
  }

  public void OnProcessingInstruction(string name, string text) {
  }
}
