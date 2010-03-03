//using UnityEngine;
using System.Collections;
//using System.Collections.Generic;

/*public interface IAttrList {
	int Length { get; }
	bool IsEmpty { get; }
	void Clear ();
	void Add (string name, string value);			
	string GetName (int i);
	string GetValue (int i);
	string GetValue (string name);
	string GetValue (string name, bool upcase);
	string [] Names { get; }
	string [] Values { get; }
	//------------
	float GetFloatValue (int i);
	float GetFloatValue (string name);
}
*/
		
/*public class uAttributesStore : IAttrList {
	private Dictionary<string, string> m_attrList;	
	//-------------------------------------------------------------------------------------//
	public int Count {
		get{
			return this.m_attrList.Count;
		}
	}
	public int Length {
		get{
			return Count;
		}
	}
	public bool IsEmpty {
		get {
			return (Count==0?true : false);
		}
	}
	//-------------------------------------------------------------------------------------//
	//Cac lop ke thua cuaIAttrList
	//string GetName (int i);
	public string GetName(int i) {
		return "";
		//return this.m_attrList[i];
	}
	public string GetValue (int i) {
		return "";
	}
	public string GetValue (string name, bool upcase) {
		return "";
	}

	//------------
	public float GetFloatValue (int i) {
		return 0.0f;
	}
	public float GetFloatValue (string name) {
		return 0.0f;
	}
			
	//-------------------------------------------------------------------------------------//

	public uAttributesStore () {
		m_attrList = new Dictionary<string, string>();
	}
	public void Clear () {
		m_attrList.Clear();
	}
	public void Add1(string name, string value) {
		Debug.Log("22222" + name + "   " + value);
		if (ContainsKey(name) == false) {
			m_attrList.Add(name, value);
		} else {
			Debug.Log(name);
		}
	}
	public bool ContainsKey(string key) {
		return m_attrList.ContainsKey(key);
	}
	public string GetValue(string key) {
		return m_attrList[key];
	}
	
}
*/

//Day la Class cu duoc cung cap boi SmallXmlParser

public struct AttributeList {
	
	ArrayList attrNames;// = new ArrayList ();
	ArrayList attrValues;// = new ArrayList ();
	//-------------------------------------------------------------------------------------//
	public AttributeList(int i) {
		attrNames = new ArrayList ();
		attrValues = new ArrayList ();
	}
	public int Length {
		get { return attrNames.Count;}
	}
	
	public bool IsEmpty {
		get { return attrNames.Count == 0; }
	}
	
	public string [] Names {
		get { return (string []) attrNames.ToArray (typeof (string)); }
	}
	public string [] Values {
		get { return (string []) attrValues.ToArray (typeof (string)); }
	}
	
	public void Clear () {
		attrNames.Clear ();
		attrValues.Clear ();
	}

	public void Add (string name, string value) {
		if (attrNames == null) attrNames = new ArrayList ();
		if (attrValues == null) attrValues = new ArrayList ();
		attrNames.Add (name);
		attrValues.Add (value);
		//UnityEngine.Debug.Log("Name = " + name + " Value = " + value);
	}
	//-------------------------------------------------------------------------------------//
	public string GetName (int i) {
		if (i >= Length) return "";
		return (string) attrNames [i];
	}
	public string GetValue (int i) {
		if (i >= Length) return "";
		return (string) attrValues [i];
	}
	
	public string GetValue (string name) {
		return GetValue(name, false);
	}
	public string GetValue (string name, bool upcase) {
		for (int i = 0; i < attrNames.Count; i++) {
			if (((((string) attrNames[i]).ToUpper() == name.ToUpper()) && (upcase == true)) ||
				(((string) attrNames[i] == name) && (upcase == false))) {
				return (string) attrValues [i];
			}
		}
		return "";
	}

	//-------------------------------------------------------------------------------------//
	public float GetFloatValue (int i) {
		string temp = GetValue(i);
		if (temp != "") {
			return float.Parse(temp);
		} else {
			return 0.0f;
		}
	}
	public float GetFloatValue (string name) {
		string temp = GetValue(name, true);
		if (temp != "") {
			return float.Parse(temp);
		} else {
			return 0.0f;
		}
	}
}