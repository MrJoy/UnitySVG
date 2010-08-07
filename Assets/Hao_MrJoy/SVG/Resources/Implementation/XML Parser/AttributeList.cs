using System.Collections.Generic;

//Day la Class cu duoc cung cap boi SmallXmlParser

public struct AttributeList {
  Dictionary<string,string> attrs;

  public AttributeList(int i) {
    attrs = null;
  }

  public AttributeList(AttributeList a) {
    attrs = new Dictionary<string,string>(a.attrs);
  }
  //-------------------------------------------------------------------------------------//
  public int Length {
    get { return(attrs != null) ? attrs.Keys.Count : 0;}
  }

  public void Clear() {
    attrs.Clear();
  }

  public void Add(string name, string value) {
    if(attrs == null)
      attrs = new Dictionary<string,string>();
    attrs[name] = value;
  }
  //-------------------------------------------------------------------------------------//
  public string GetValue(string name) {
    string outVal;
    if((attrs != null) && attrs.TryGetValue(name, out outVal))
      return outVal;
    else
      return "";
    }

}