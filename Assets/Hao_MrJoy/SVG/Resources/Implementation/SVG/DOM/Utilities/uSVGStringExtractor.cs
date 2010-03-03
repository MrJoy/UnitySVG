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
	private static char[] splitPipe = new char[]{'|'};
	public static Dictionary<string, string> f_ExtractTransformList(string inputText) {
		Dictionary <string, string> m_return = new Dictionary<string, string>();
		int i = 0;
		string m_key = "";
		string m_value = "";
		
		inputText = inputText.Trim();		
		
		inputText = uSVGStringExtractor.f_RemoveMultiSpace(inputText); //Lam sach bot khoang trang;
		inputText = inputText.Replace(") ",")|");
		
		string[] valuesStr = inputText.Split(splitPipe, System.StringSplitOptions.RemoveEmptyEntries);

		int len = valuesStr.GetLength(0);

		for (i = 0; i < len; i++) {
			int vt1 = valuesStr[i].IndexOf('(');
			int vt2 = valuesStr[i].IndexOf(')');
			m_key = valuesStr[i].Substring(0, vt1);
			m_value = valuesStr[i].Substring(vt1+1, vt2-vt1-1);
			m_return.Add(m_key, m_value);
		}
		return m_return;
	}
	//--------------------------------------------------
	//Extract for Syntax:  700 200 -30
	private static char[] splitSpaceComma = new char[]{' ',','};
	public static string[] f_ExtractTransformValue(string inputText) {
//		inputText = inputText.Trim();		
		inputText = uSVGStringExtractor.f_RemoveMultiSpace(inputText);

		string[] values = inputText.Split(splitSpaceComma, System.StringSplitOptions.RemoveEmptyEntries);
		return values;		
	}
	//--------------------------------------------------
	//Extract for Systax : M100 100 C200 100,...
	public static void f_ExtractPathSegList(string inputText,
											ref List<string> charList, ref List<string> valueList) {
		int i = 0;	
		inputText = inputText.Trim();		
		
		inputText = uSVGStringExtractor.f_RemoveMultiSpace(inputText);
		
		List<int> m_break = new List<int>();
		for(i = 0; i<inputText.Length; i++) {
			if (((inputText[i] >='a') && (inputText[i] <='z')) ||
				((inputText[i] >='A') && (inputText[i] <='Z'))) {
				m_break.Add(i);
			}
		}
		m_break.Add(inputText.Length);

		for(i = 0; i < m_break.Count - 1; i++) {
			int m_breakSpot1 = m_break[i];
			int m_breakSpot2 = m_break[i+1];
			string m_string1 = "";
			string m_string2 = "";
			m_string1 = inputText.Substring(m_breakSpot1, 1);
			m_string2 = inputText.Substring(m_breakSpot1+1, m_breakSpot2 - m_breakSpot1 -1);
			charList.Add(m_string1);
			valueList.Add(m_string2);
		}
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