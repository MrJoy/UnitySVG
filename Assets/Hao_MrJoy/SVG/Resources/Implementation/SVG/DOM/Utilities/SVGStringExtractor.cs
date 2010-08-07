using UnityEngine;
using System;
using System.Collections.Generic;

public static class SVGStringExtractor {
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
  public static List<SVGTransform> ExtractTransformList(string inputText) {
    List<SVGTransform> _return = new List<SVGTransform>();

    string[] valuesStr = inputText.Split(splitPipe, StringSplitOptions.RemoveEmptyEntries);

    int len = valuesStr.Length;
    for(int i = 0; i < len; i++) {
      int vt1 = valuesStr[i].IndexOf('(');
      string _key = valuesStr[i].Substring(0, vt1);
      string _value = valuesStr[i].Substring(vt1+1);
      _return.Add(new SVGTransform(_key, _value));
    }
    return _return;
  }
  //--------------------------------------------------
  //Extract for Syntax:  700 200 -30
  private static char[] splitSpaceComma = new char[] {' ', ',', '\n', '\t', '\r'};
  public static float[] ExtractTransformValueAsPX(string inputText) {
    string[] tmp = ExtractTransformValue(inputText);
    float[] values = new float[tmp.Length];
    for(int i = 0; i < values.Length; i++)
      values[i] = SVGLength.GetPXLength(tmp[i]);
    return values;
  }
  public static string[] ExtractTransformValue(string inputText) {
    return inputText.Split(splitSpaceComma, StringSplitOptions.RemoveEmptyEntries);
  }
  //--------------------------------------------------
  //Extract for Systax : M100 100 C200 100,...
  private static List<int> _break = new List<int>();
  // WARNING:  This method is NOT thread-safe due to use of static _break member!
  public static void ExtractPathSegList(string inputText,
                      ref List<char> charList, ref List<string> valueList) {
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
  }

  //--------------------------------------------------
  //Extract for Syntax:  fill: #ffffff; stroke:#000000; stroke-width:0.172
  private static char[] splitColonSemicolon = new char[]{':', ';', ' ', '\n', '\t', '\r'};
  public static void ExtractStyleValue(string inputText, ref Dictionary<string, string> dic) {
    string[] valuesStr = inputText.Split(splitColonSemicolon, StringSplitOptions.RemoveEmptyEntries);

    int len = valuesStr.Length - 1;
    for(int i = 0; i < len; i += 2)
      dic.Add(valuesStr[i], valuesStr[i + 1]);
  }
  //--------------------------------------------------
  //Extract for Syntax:   translate(700 200)rotate(-30)
  public static string ExtractUrl4Gradient(string inputText) {
Profiler.BeginSample("uSVGStringExtractor.ExtractUrl4Gradient(string)");
    // TODO: Optimize this routine...
    string _return = "";
    inputText = inputText.Trim();
    inputText = SVGStringExtractor.RemoveMultiSpace(inputText);
    inputText = inputText.Replace(" ","");
    int vt1 = inputText.IndexOf("url(#");
    int vt2;
    if(inputText.IndexOf(")") >= 0) {
      vt2 = inputText.IndexOf(")");
    } else {
      vt2 = inputText.Length;
    }

    _return = inputText.Substring(vt1 + 5, vt2 - vt1 - 5);
Profiler.EndSample();
    return _return;
  }
}
