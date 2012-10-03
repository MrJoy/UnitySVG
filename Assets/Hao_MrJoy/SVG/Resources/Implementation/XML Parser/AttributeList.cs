using System.Collections.Generic;

//Day la Class cu duoc cung cap boi SmallXmlParser

public struct AttributeList {
  Dictionary<string,string> attrs;

  public AttributeList(AttributeList a) {
    if(a.attrs != null)
      attrs = new Dictionary<string,string>(a.attrs);
    else
      attrs = null;
  }
  //-------------------------------------------------------------------------------------//

  public void Clear() {
    if(attrs != null)
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

  public new string ToString() {
    if(attrs == null)
      return "null";
    else {
      bool isFirst = true;
      System.Text.StringBuilder sb = new System.Text.StringBuilder();
      foreach(KeyValuePair<string,string> kvp in attrs) {
        if(!isFirst)
          sb.Append(", ");
        sb.Append(kvp.Key).Append("=").Append(kvp.Value);
        isFirst = false;
      }
      return sb.ToString();
    }
  }
}
