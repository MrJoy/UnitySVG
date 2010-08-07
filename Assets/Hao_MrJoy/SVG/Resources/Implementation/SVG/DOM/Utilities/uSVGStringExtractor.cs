using UnityEngine;
using System;
using System.Collections.Generic;

public class uSVGStringExtractor {
	//************************************************************************************
	private static string f_RemoveMultiSpace(string inputText) {
		string temp = "";
		inputText = inputText.Replace('\n',' ');
		inputText = inputText.Replace('\t',' ');
		inputText = inputText.Replace('\r',' ');
		temp = inputText;
		do {
			inputText = temp;
			temp = inputText.Replace("  "," ");
		} while (temp != inputText);
		return temp;
	}
	//--------------------------------------------------
	//Extract for Syntax:   translate(700 200) rotate(-30)
	private static char[] splitPipe = new char[] {'|', ' ', ')', '\n', '\t', '\r'};
	public static Dictionary<string, string> f_ExtractTransformList(string inputText) {
Profiler.BeginSample("uSVGStringExtractor.f_ExtractTransformList(string)");
		Dictionary <string, string> m_return = new Dictionary<string, string>();

		string[] valuesStr = inputText.Split(splitPipe, StringSplitOptions.RemoveEmptyEntries);

		int len = valuesStr.Length;

		for (int i = 0; i < len; i++) {
			int vt1 = valuesStr[i].IndexOf('(');
			string m_key = valuesStr[i].Substring(0, vt1);
			string m_value = valuesStr[i].Substring(vt1+1);
			m_return.Add(m_key, m_value);
		}
Profiler.EndSample();
		return m_return;
	}
	//--------------------------------------------------
	//Extract for Syntax:  700 200 -30
	private static char[] splitSpaceComma = new char[] {' ', ',', '\n', '\t', '\r'};
	public static string[] f_ExtractTransformValue(string inputText) {
Profiler.BeginSample("uSVGStringExtractor.f_ExtractTransformValue(string)");
		string[] values = inputText.Split(splitSpaceComma, System.StringSplitOptions.RemoveEmptyEntries);
Profiler.EndSample();
		return values;
	}
	//--------------------------------------------------
	//Extract for Systax : M100 100 C200 100,...
	private static List<int> m_break = new List<int>();
	// WARNING:  This method is NOT thread-safe due to use of static m_break member!
	public static void f_ExtractPathSegList(string inputText,
											ref List<char> charList, ref List<string> valueList) {
Profiler.BeginSample("uSVGStringExtractor.f_ExtractPathSegList(string, ref List<char>, ref List<string>)");
		m_break.Clear();
		for(int i = 0; i<inputText.Length; i++) {
//		  if(char.IsLetter(inputText[i])) {
			if (((inputText[i] >= 'a') && (inputText[i] <= 'z')) ||
				((inputText[i] >= 'A') && (inputText[i] <= 'Z'))) {
				m_break.Add(i);
			}
		}
		m_break.Add(inputText.Length);
		charList.Capacity = m_break.Count - 1;
		valueList.Capacity = m_break.Count - 1;

		for(int i = 0; i < m_break.Count - 1; i++) {
			int m_breakSpot1 = m_break[i];
			int m_breakSpot2 = m_break[i+1];
			string m_string = inputText.Substring(m_breakSpot1+1, m_breakSpot2 - m_breakSpot1 -1);
			charList.Add(inputText[m_breakSpot1]);
			valueList.Add(m_string);
		}
Profiler.EndSample();
	}
	
	//--------------------------------------------------
	//Extract for Syntax:  fill: #ffffff; stroke:#000000; stroke-width:0.172
	private static char[] splitColonSemicolon = new char[]{':',';'};
	public static void f_ExtractStyleValue(string inputText, 
					ref Dictionary<string, string> dic) {

		inputText = inputText.Trim();		
		inputText = uSVGStringExtractor.f_RemoveMultiSpace(inputText);
		inputText = inputText.Replace(" ","");

		string[] valuesStr = inputText.Split(splitColonSemicolon, System.StringSplitOptions.RemoveEmptyEntries);

		int len = valuesStr.GetLength(0);
		for (int i = 0; i < len -1; i++) {
			dic.Add(valuesStr[i], valuesStr[i+1]);
			i++;
		}
	}
	//--------------------------------------------------
	//Extract for Syntax:   translate(700 200) rotate(-30)
	public static string f_ExtractUrl4Gradient(string inputText) {
		string m_return = "";
		inputText = inputText.Trim();		
		inputText = uSVGStringExtractor.f_RemoveMultiSpace(inputText);
		inputText = inputText.Replace(" ","");
		int vt1 = inputText.IndexOf("url(#");
		int vt2;
		if (inputText.IndexOf(")") >= 0) {
			vt2 = inputText.IndexOf(")");
		} else {
			vt2 = inputText.Length;
		}
		
		m_return = inputText.Substring(vt1 + 5, vt2 - vt1 - 5);
		return m_return;
	}
}