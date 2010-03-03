using System.IO;

public class uXMLImp : SmallXmlParser.IContentHandler {
	public enum XMLTagState {OPEN, CLOSE, BOTH}
	
	private SmallXmlParser m_parser 	= new SmallXmlParser();
	private TextReader m_textReader;
	
	private string m_currentName = "";
	private string m_currentLineText = "";
	private XMLTagState m_currentTagState;
	private AttributeList m_currentList;
	
	private string m_svgText = "";

	/***********************************************************************************/
	public uXMLImp (){
	}

	public uXMLImp(string text) {
		this.m_svgText = text;
		m_textReader = new StringReader(text);
		m_parser.f_Pause();
		m_parser.Parse(m_textReader, this);
	}
	
	//---------------------------------------------------------
	//Methods: f_CloneAttrsList
	//Purpose: create a clone Attribute List from XMLParser Handle for storing.
	//---------------------------------------------------------
	private void f_CloneAttrsList(AttributeList attrs) {
		this.m_currentList = new AttributeList(attrs);
	}
	
	/**********************************************************************************
	Public Classes:
		public bool 			f_ReadNextTag();
		public XMLTagState 		f_GetCurrentTagState();
		public string 			f_GetCurrentTagName();
		public void 			f_Reset(string text);
		public bool 			f_IsCurrentTagName(string name);
		public int 				f_Length();
		public IAttrList		f_GetCurrentAttributesList
		public IAttrList		f_GetNextAttrList
	
	************************************************************************************/
	
	//---------------------------------------------------------
	//Methods: f_ReadNextTag
	//Purpose: Unpause XMLParser to reading nex Tag.
	//---------------------------------------------------------
	public bool f_ReadNextTag() {
		return m_parser.f_UnPause();
	}
	
	//---------------------------------------------------------
	//Methods: f_GetCurrentTagState
	//Purpose: return current XMLTag State
	//State:
	//		XMLTagState.OPEN 	: current XMLTag is Opened.
	//		XMLTagState.CLOSE 	: current XMLTag is Closed. This XMLTag is opened in before XMLTag.
	//		XMLTagState.BOTH 	: current XMLTag is Open and Closed in the same XMLTag.
	//---------------------------------------------------------
	public XMLTagState f_GetCurrentTagState() {
		return this.m_currentTagState;
	}
	
	//---------------------------------------------------------
	//Methods: f_Reset
	//Purpose: Reset new String.
	//---------------------------------------------------------
	public void f_Reset(string text) {
		this.m_svgText = text;
		m_textReader = new StringReader(text);
		m_parser.f_New();
		m_parser.f_Pause();		
		m_parser.Parse(m_textReader, this);		
	}
	public void f_Reset() {
		m_textReader = new StringReader(this.m_svgText);
		m_parser.f_New();
		m_parser.f_Pause();		
		m_parser.Parse(m_textReader, this);
	}
	//---------------------------------------------------------
	//Methods: f_GetCurrentTagName
	//Purpose: return current Tag Name of XML Tag reading.
	//---------------------------------------------------------
	public string f_GetCurrentTagName() {
		return this.m_currentName;
	}
	//---------------------------------------------------------
	//Methods: f_GetCurrentLineText
	//Purpose: return current Tag Name of XML Tag reading.
	//---------------------------------------------------------
	public string f_GetCurrentLineText() {
		return this.m_currentLineText;
	}

	//---------------------------------------------------------
	//Methods: f_IsCurrentTagName
	//Purpose: return true when current Tag Name = name.
	//---------------------------------------------------------

	public bool f_IsCurrentTagName(string name) {
		if (name == this.m_currentName) {
			return true;
		}
		return false;		
	}
	
	//---------------------------------------------------------
	//Methods: f_GetCurrentAttributesList
	//Purpose: Return current Attributes List.
	//---------------------------------------------------------
	
	public AttributeList f_GetCurrentAttributesList() {
		return m_currentList;
	}

	//---------------------------------------------------------
	//Methods: f_ReadFirstElement
	//Purpose: ....
	//---------------------------------------------------------
	public AttributeList f_GetNextAttrList() {
		f_ReadNextTag();
		return this.m_currentList;		
	}


	//---------------------------------------------------------
	//Methods: f_Length
	//Purpose: Return Count of attribute List of current XMLTag.
	//---------------------------------------------------------

	public int f_Length() {
		return this.m_currentList.Length;
	}
	
	//-----------------------------------------------------------------------------------------//
	//Cac Functions lien qua den xu ly SVG Document
	//-----------------------------------------------------------------------------------------//
	//---------------------------------------------------------
	//Methods: f_ReadFirstElement
	//Purpose: ....
	//---------------------------------------------------------

	public string f_ReadFirstElement(string name) {
		int level = 0;
		string m_return = "";
		if ((this.m_currentName == name) && (this.m_currentTagState == XMLTagState.OPEN)) {
			level++;
			m_return += m_currentLineText;
		}
		while (f_ReadNextTag()) {			
			if (this.m_currentName == name) {
				if (this.m_currentTagState == XMLTagState.OPEN) level++;
				else if (this.m_currentTagState == XMLTagState.CLOSE) {
					level--;
					if (level == 0) {
						m_return += m_currentLineText;
						break;
					}
				}
			}
			
			if (level > 0)
				m_return += m_currentLineText;
		}
		return m_return;
	}
	//---------------------------------------------------------
	//Methods: f_GetUntilCloseTag
	//Purpose: ....
	//---------------------------------------------------------
	public string f_GetUntilCloseTag(string name) {
		int level = 1;
		string m_return = "";
		while (f_ReadNextTag()) {			
			if (this.m_currentName == name) {
				if (this.m_currentTagState == XMLTagState.OPEN) level++;
				else if (this.m_currentTagState == XMLTagState.CLOSE) {
					level--;
					if (level == 0) {
						m_return += m_currentLineText;
						break;
					}
				}
			}
			
			if (level > 0)
				m_return += m_currentLineText;
		}
		return m_return;
	}

	/***********************************************************************************/
	public void OnStartParsing (SmallXmlParser parser)	{
	}

	public void OnEndParsing (SmallXmlParser parser) {	
	}

	public void OnStartEndElement (string name, AttributeList attrs, string lineText) {
		this.m_parser.f_Pause();
		this.m_currentName = name;
		this.m_currentLineText = lineText;
		f_CloneAttrsList(attrs);
		this.m_currentTagState = XMLTagState.BOTH;
	}
	public void OnStartElement (string name, AttributeList attrs, string lineText) {
		this.m_parser.f_Pause();
		this.m_currentName = name;
		this.m_currentLineText = lineText;
		f_CloneAttrsList(attrs);
		this.m_currentTagState = XMLTagState.OPEN;
	}

	public void OnEndElement (string name, string lineText) {
		this.m_parser.f_Pause();
		this.m_currentName = name;
		this.m_currentLineText = lineText;
		this.m_currentTagState = XMLTagState.CLOSE;
	}

	public void OnChars (string s) {
	}

	public void OnIgnorableWhitespace (string s) {
	}

	public void OnProcessingInstruction (string name, string text) {
	}
}
