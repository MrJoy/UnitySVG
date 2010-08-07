using UnityEngine;
using System;
using System.Collections.Generic;

public class uSVGStringExtractor {
  //************************************************************************************
  private static string RemoveMultiSpace(string inputText) {
    string temp = "";
    inputText = inputText.Replace('\n',' ');
    inputText = inputText.Replace('\t',' ');
    inputText = inputText.Replace('\r',' ');
    temp = inputText;
    do {
      inputText = temp;
      temp = inputText.Replace("  "," ");
    } while(temp != inputText);
    return temp;
  }
  //--------------------------------------------------
  //Extract for Syntax:   translate(700 200)rotate(-30)
  private static char[] splitPipe = new char[] {'|', ' ', ')', '\n', '\t', '\r'};
  public static List<uSVGTransform> ExtractTransformList(string inputText) {
Profiler.BeginSample("uSVGStringExtractor.ExtractTransformList(string)");
    List<uSVGTransform> _return = new List<uSVGTransform>();

    string[] valuesStr = inputText.Split(splitPipe, StringSplitOptions.RemoveEmptyEntries);

    int len = valuesStr.Length;

    for(int i = 0; i < len; i++) {
      int vt1 = valuesStr[i].IndexOf('(');
      string _key = valuesStr[i].Substring(0, vt1);
      string _value = valuesStr[i].Substring(vt1+1);
      _return.Add(new uSVGTransform(_key, _value));
    }
Profiler.EndSample();
    return _return;
  }
  //--------------------------------------------------
  //Extract for Syntax:  700 200 -30
  private static char[] splitSpaceComma = new char[] {' ', ',', '\n', '\t', '\r'};
  public static string[] ExtractTransformValue(string inputText) {
Profiler.BeginSample("uSVGStringExtractor.ExtractTransformValue(string)");
    string[] values = inputText.Split(splitSpaceComma, System.StringSplitOptions.RemoveEmptyEntries);
Profiler.EndSample();
    return values;
  }
  //--------------------------------------------------
  //Extract for Systax : M100 100 C200 100,...
  private static List<int> _break = new List<int>();
  // WARNING:  This method is NOT thread-safe due to use of static _break member!
  public static void ExtractPathSegList(string inputText,
                      ref List<char> charList, ref List<string> valueList) {
Profiler.BeginSample("uSVGStringExtractor.ExtractPathSegList(string, ref List<char>, ref List<string>)");
    _break.Clear();
    for(int i = 0; i<inputText.Length; i++) {
//      if(char.IsLetter(inputText[i])) {
      if(((inputText[i] >= 'a')&&(inputText[i] <= 'z'))||
       ((inputText[i] >= 'A')&&(inputText[i] <= 'Z'))) {
        _break.Add(i);
      }
    }
    _break.Add(inputText.Length);
    charList.Capacity = _break.Count - 1;
    valueList.Capacity = _break.Count - 1;

    for(int i = 0; i < _break.Count - 1; i++) {
      int _breakSpot1 = _break[i];
      int _breakSpot2 = _break[i+1];
      string _string = inputText.Substring(_breakSpot1+1, _breakSpot2 - _breakSpot1 -1);
      charList.Add(inputText[_breakSpot1]);
      valueList.Add(_string);
    }
Profiler.EndSample();
  }

  //--------------------------------------------------
  //Extract for Syntax:  fill: #ffffff; stroke:#000000; stroke-width:0.172
  private static char[] splitColonSemicolon = new char[]{':',';'};
  public static void ExtractStyleValue(string inputText,
          ref Dictionary<string, string> dic) {

    inputText = inputText.Trim();
    inputText = uSVGStringExtractor.RemoveMultiSpace(inputText);
    inputText = inputText.Replace(" ","");

    string[] valuesStr = inputText.Split(splitColonSemicolon, System.StringSplitOptions.RemoveEmptyEntries);

    int len = valuesStr.GetLength(0);
    for(int i = 0; i < len -1; i++) {
      dic.Add(valuesStr[i], valuesStr[i+1]);
      i++;
    }
  }
  //--------------------------------------------------
  //Extract for Syntax:   translate(700 200)rotate(-30)
  public static string ExtractUrl4Gradient(string inputText) {
    string _return = "";
    inputText = inputText.Trim();
    inputText = uSVGStringExtractor.RemoveMultiSpace(inputText);
    inputText = inputText.Replace(" ","");
    int vt1 = inputText.IndexOf("url(#");
    int vt2;
    if(inputText.IndexOf(")") >= 0) {
      vt2 = inputText.IndexOf(")");
    } else {
      vt2 = inputText.Length;
    }

    _return = inputText.Substring(vt1 + 5, vt2 - vt1 - 5);
    return _return;
  }
}