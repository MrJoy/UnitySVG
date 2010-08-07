using System.Collections.Generic;

//Day la Class cu duoc cung cap boi SmallXmlParser

public struct AttributeList {
  Dictionary<string,string> attrs;

  public AttributeList(AttributeList a) {
    attrs = new Dictionary<string,string>(a.attrs);
  }
  //-------------------------------------------------------------------------------------//

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